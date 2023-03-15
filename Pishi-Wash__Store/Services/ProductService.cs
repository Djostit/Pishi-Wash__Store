using Pishi_Wash__Store.Data.Models;
using System.Collections.Generic;
using System.Windows.Documents;

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
        public async Task<List<Order>> GetOrders()
        {
            await _tradeContext.Orderproducts.ToListAsync();
            return await _tradeContext.Orders.ToListAsync();
        }
        public async Task UpdateAmmountOrder()
        {
            await _tradeContext.Orderproducts.ToListAsync();
            await _tradeContext.Products.ToListAsync();
            var currentList = await _tradeContext.Orders.ToListAsync();

            foreach(var item in currentList)
            {
                Func<float?> test = ()=> 
                {
                    //OrderAmmount += (item.Count * item.Price) - ((item.Count * item.Price) * item.Discount / 100);
                    //_orderAmmount += item.Count * item.Price;

                    float? orderammount = 0;
                    float? _orderammount = 0;
                    foreach (var test1 in item.Orderproducts.ToList()) 
                    {
                        orderammount += (test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) - ((test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) * test1.ProductArticleNumberNavigation.ProductDiscountAmount / 100);
                        _orderammount += test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost;
                    }

                    orderammount = (float)Math.Round((decimal)orderammount, 2);
                    _orderammount = (float)Math.Round(((decimal)_orderammount - (decimal)orderammount), 2);
                    return orderammount;
                };

                Func<float?> test2 = () =>
                {
                    //OrderAmmount += (item.Count * item.Price) - ((item.Count * item.Price) * item.Discount / 100);
                    //_orderAmmount += item.Count * item.Price;

                    float? orderammount = 0;
                    float? _orderammount = 0;
                    foreach (var test1 in item.Orderproducts.ToList())
                    {
                        orderammount += (test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) - ((test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) * test1.ProductArticleNumberNavigation.ProductDiscountAmount / 100);
                        _orderammount += test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost;
                    }

                    orderammount = (float)Math.Round((decimal)orderammount, 2);
                    _orderammount = (float)Math.Round(((decimal)_orderammount - (decimal)orderammount), 2);
                    return _orderammount;
                };
                Debug.WriteLine(item.OrderId + " " + test() + " " + test2());
                //item.OrderAmmount = test();

                item.OrderAmmount = (float)test();
                await _tradeContext.SaveChangesAsync();

                item.OrderDiscountAmmount = (float)test2();
                await _tradeContext.SaveChangesAsync();
            }
        }

        public async Task GetListFullInformation()
        {
            var
        }
    }
}
