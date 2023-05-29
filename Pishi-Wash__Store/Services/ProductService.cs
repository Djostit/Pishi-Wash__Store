using AutoMapper;
using Pishi_Wash__Store.Models;


namespace Pishi_Wash__Store.Services
{
    public class ProductService
    {
        private readonly IMapper _mapper;
        private readonly TradeContext _tradeContext;
        public ProductService(TradeContext tradeContext)
        {
            _tradeContext = tradeContext;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, DbProduct>();
            }).CreateMapper();
        }
        public List<Pname> GetNames() => _tradeContext.Pnames.ToList();
        public List<Pprovider> GetProdivers() => _tradeContext.Pproviders.ToList();
        public List<Pcategory> GetPcategories() => _tradeContext.Pcategories.ToList();
        public List<Pmanufacturer> GetPmanufacturers() => _tradeContext.Pmanufacturers.ToList();
        public async Task<List<DbProduct>> GetProducts()
        {
            List<DbProduct> dbProduct = new();
            try
            {
                await _tradeContext.Pnames.ToListAsync();
                await _tradeContext.Pmanufacturers.ToListAsync();
                dbProduct = _mapper.Map<List<DbProduct>>(await _tradeContext.Products.ToListAsync());
            }
            catch { }
            return dbProduct;
        }
        public async Task<List<DbProduct>> GetCart()
        {
            List<DbProduct> dbProducts = new();
            var currentProducts = await GetProducts();

            foreach (var item in Global.CurrentCart)
            {
                var product = currentProducts.SingleOrDefault(s => s.ProductArticleNumber.Equals(item.ArticleName));
                if (product != null)
                {
                    product.Count = Global.CurrentCart.Single(s => s.ArticleName.Equals(product.ProductArticleNumber)).Count;
                    dbProducts.Add(product);
                }
            }
            return dbProducts;
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
            _tradeContext.Orderproducts.ToList();
            _tradeContext.Products.ToList();
            _tradeContext.Pnames.ToList();
            return _tradeContext.Orders.ToList();
        }

        public async Task<List<DbProduct>> GetListFullInformation()
        {
            var currentProduct = await GetProducts();
            List<DbProduct> product = new();
            foreach (var item in currentProduct)
            {
                if (Global.CurrentCart?.FirstOrDefault(c => c.ArticleName.Equals(item.ProductArticleNumber)) != null)
                    product.Add(item);
            }
            return product;
        }

        public async Task SaveChangesAsync() => await _tradeContext.SaveChangesAsync();
        public async Task UpdateCurrentProduct(DbProduct product)
        {
            var pt = await _tradeContext.Products.FindAsync(product.ProductArticleNumber);
            pt.ProductDiscountAmount = product.ProductDiscountAmount;
            pt.ProductStatus = product.ProductStatus;
            _tradeContext.Products.Update(pt);
            await _tradeContext.SaveChangesAsync();
        }
        public async Task<DbProduct> AddProductAsync(Product product)
        {
            await _tradeContext.Products.AddAsync(product);
            await _tradeContext.SaveChangesAsync();
            var dbProduct = _mapper.Map<DbProduct>(product);
            return dbProduct;
        }
        public async Task<Pcategory> AddCategoriesAsync(Pcategory foo)
        {
            await _tradeContext.Pcategories.AddAsync(foo);
            await _tradeContext.SaveChangesAsync();
            return foo;
        }
        public async Task<Pmanufacturer> AddManufacturersAsync(Pmanufacturer foo)
        {
            await _tradeContext.Pmanufacturers.AddAsync(foo);
            await _tradeContext.SaveChangesAsync();
            return foo;
        }

        public async Task<Pprovider> AddProviderAsync(Pprovider foo)
        {
            await _tradeContext.Pproviders.AddAsync(foo);
            await _tradeContext.SaveChangesAsync();
            return foo;
        }

        public async Task<Pname> AddNameAsync(Pname foo)
        {
            await _tradeContext.Pnames.AddAsync(foo);
            await _tradeContext.SaveChangesAsync();
            return foo;
        }
        

        //public async Task UpdateAmmountOrder()
        //{
        //    await _tradeContext.Orderproducts.ToListAsync();
        //    await _tradeContext.Products.ToListAsync();
        //    var currentList = await _tradeContext.Orders.ToListAsync();

        //    foreach (var item in currentList)
        //    {
        //        Func<float?> test = () =>
        //        {
        //            float? orderammount = 0;
        //            float? _orderammount = 0;
        //            foreach (var test1 in item.Orderproducts.ToList())
        //            {
        //                orderammount += (test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) - ((test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) * test1.ProductArticleNumberNavigation.ProductDiscountAmount / 100);
        //                _orderammount += test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost;
        //            }

        //            orderammount = (float)Math.Round((decimal)orderammount, 2);
        //            _orderammount = (float)Math.Round(((decimal)_orderammount - (decimal)orderammount), 2);
        //            return orderammount;
        //        };

        //        Func<float?> test2 = () =>
        //        {
        //            float? orderammount = 0;
        //            float? _orderammount = 0;
        //            foreach (var test1 in item.Orderproducts.ToList())
        //            {
        //                orderammount += (test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) - ((test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost) * test1.ProductArticleNumberNavigation.ProductDiscountAmount / 100);
        //                _orderammount += test1.ProductCount * test1.ProductArticleNumberNavigation.ProductCost;
        //            }

        //            orderammount = (float)Math.Round((decimal)orderammount, 2);
        //            _orderammount = (float)Math.Round(((decimal)_orderammount - (decimal)orderammount), 2);
        //            return _orderammount;
        //        };
        //        Debug.WriteLine(item.OrderId + " " + test() + " " + test2());

        //        item.OrderAmmount = (float)test();
        //        await _tradeContext.SaveChangesAsync();

        //        item.OrderDiscountAmmount = (float)test2();
        //        await _tradeContext.SaveChangesAsync();
        //    }
        //}
    }
}
