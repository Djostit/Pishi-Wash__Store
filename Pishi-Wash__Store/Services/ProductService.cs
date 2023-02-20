namespace Pishi_Wash__Store.Services
{
    public class ProductService
    {
        private readonly DataContext _context;
        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();

            await Task.Run(async () => 
            {
                try
                {
                    List<DbProduct> product = await _context.Product.ToListAsync();
                    List<DbPName> pnames = await _context.PName.ToListAsync();
                    List<DbPManufacturer> pmanufactures = await _context.PManufacturer.ToListAsync();

                    foreach (var item in product)
                    {
                        products.Add(new Product
                        {
                            Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                            Title = pnames.SingleOrDefault(pn => pn.PNameID == item.ProductName).ProductName,
                            Description = item.ProductDescription,
                            Manufacturer = pmanufactures.SingleOrDefault(pm => pm.PManufacturerID == item.ProductManufacturer).ProductManufacturer,
                            Price = item.ProductCost,
                            Discount = item.ProductDiscountAmount
                        });
                    }
                }
                catch { }
            });
            return products;
        }
    }
}
