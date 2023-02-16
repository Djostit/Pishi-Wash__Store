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
                List<DbProduct> a = await _context.Product.ToListAsync();
                for (int i = 0; i < a.Count; i++)
                {
                    products.Add(new Product
                    {
                        Image = a[i].ProductPhoto == string.Empty ? "picture.png" : a[i].ProductPhoto,
                        Title = await Task.Run(async () => (await _pnamecontext.PName.FindAsync(a[i].ProductName)).ProductName),
                        Description = a[i].ProductDescription,
                        Manufacturer = await Task.Run(async () => (await _pmanufacturercontext.PManufacturer.FindAsync(a[i].ProductManufacturer)).ProductManufacturer),
                        Price = a[i].ProductCost,
                        Discount = a[i].ProductDiscountAmount
                    });
                }
            });
            return products;
        }
    }
}
