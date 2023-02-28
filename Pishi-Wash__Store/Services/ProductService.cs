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
            try
            {
                List<DbProduct> product = await _context.Product.AsNoTracking().ToListAsync();
                await _context.SaveChangesAsync();
                List<DbPName> pnames = await _context.PName.AsNoTracking().ToListAsync();
                await _context.SaveChangesAsync();
                List<DbPManufacturer> pmanufactures = await _context.PManufacturer.AsNoTracking().ToListAsync();
                await _context.SaveChangesAsync();


                foreach (var item in product)
                {   
                    products.Add(new Product
                    {
                        Image = item.ProductPhoto == string.Empty ? "picture.png" : item.ProductPhoto,
                        Title = pnames.First(pn => pn.PNameID == item.ProductName).ProductName,
                        Description = item.ProductDescription,
                        Manufacturer = pmanufactures.First(pm => pm.PManufacturerID == item.ProductManufacturer).ProductManufacturer,
                        Price = item.ProductCost,
                        Discount = item.ProductDiscountAmount
                    });
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return products;
        }
    }
}
