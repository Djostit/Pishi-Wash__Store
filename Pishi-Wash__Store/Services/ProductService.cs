using Pishi_Wash__Store.Data.Models;

namespace Pishi_Wash__Store.Services
{
    public class ProductService
    {
        private readonly TradeContext _tradeContext;
        public ProductService(TradeContext tradeContext)
        {
            _tradeContext = tradeContext;
        }

        public async Task<List<DbProduct>> GetProducts()
        {
            List<DbProduct> products = new();
            try
            {
                var product = await _tradeContext.Products.ToListAsync();
                await _tradeContext.Pnames.ToListAsync();
                await _tradeContext.Pmanufacturers.ToListAsync();
                await Task.Run(() =>
                {
                    foreach (var item in product)
                    {
                        products.Add(new DbProduct
                        {
                            Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            Title = item.ProductNameNavigation.ProductName,
                            Description = item.ProductDescription,
                            Manufacturer = item.ProductManufacturerNavigation.ProductManufacturer,
                            Price = item.ProductCost,
                            Discount = item.ProductDiscountAmount.Value,
                            Article = item.ProductArticleNumber,
                            Quantity = item.ProductQuantityInStock
                        });
                    }
                });
            }
            catch { }
            return products;
        }
        public async Task<List<DbProduct>> GetCart()
        {
            List<DbProduct> a = new();
            var b = await GetProducts();

            foreach (var item in Global.CurrentCart)
            {
                var product = b.SingleOrDefault(c => c.Article.Equals(item.ArticleName));
                if (product != null)
                {
                    product.Count = Global.CurrentCart.Single(a => a.ArticleName.Equals(product.Article)).Count;
                    a.Add(product); 
                }
            }
            return a;
        }

        public async Task<List<Point>> GetPoints() => await _tradeContext.Points.AsNoTracking().ToListAsync();

        public async Task<int> AddOrder(Order order)
        {
            await _tradeContext.Orders.AddAsync(order);
            await _tradeContext.SaveChangesAsync();

            foreach (var item in Global.CurrentCart)
            {
                await _tradeContext.Orderproducts.AddAsync(new Orderproduct
                {
                    OrderId = order.OrderId,
                    ProductArticleNumber = item.ArticleName,
                    ProductCount = item.Count
                });
                await _tradeContext.SaveChangesAsync();
            }

            return order.OrderId;
        }
    }
}
