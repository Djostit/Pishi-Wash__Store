namespace Pishi_Wash__Store.Services
{
    public class ProductService
    {
        private readonly ProductContext _context;
        private readonly PNameContext _pnamecontext;
        private readonly PManufacturerContext _pmanufacturercontext;
        public ProductService(ProductContext context, PNameContext pnamecontext, PManufacturerContext pmanufacturercontext)
        {
            _context = context;
            _pnamecontext = pnamecontext;
            _pmanufacturercontext = pmanufacturercontext;
        }
        public async Task<List<Product>> GetProducts()
        {
            List<Product> products = new();

            await Task.Run(async () => 
            {
                List<DbProduct> product = await _context.Product.ToListAsync();
                List<DbPName> pnames = await _pnamecontext.PName.ToListAsync();
                List<DbPManufacturer> pmanufactures = await _pmanufacturercontext.PManufacturer.ToListAsync();
                for (int i = 0; i < product.Count; i++)
                {
                    products.Add(new Product
                    {
                        Image = product[i].ProductPhoto == string.Empty ? "picture.png" : product[i].ProductPhoto,
                        Title = pnames.Find(pn => pn.PNameID == product[i].ProductName).ProductName,
                        Description = product[i].ProductDescription,
                        Manufacturer = pmanufactures.Find(pm => pm.PManufacturerID == product[i].ProductManufacturer).ProductManufacturer,
                        Price = product[i].ProductCost,
                        Discount = product[i].ProductDiscountAmount
                    });
                }
            });
            return products;
        }
    }
}
