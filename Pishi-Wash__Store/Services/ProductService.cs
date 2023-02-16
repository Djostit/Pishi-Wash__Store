using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.Services
{
    public class ProductService
    {
        private readonly ProductContext _context;
        public ProductService(ProductContext context)
        {
            _context = context;
        }
        public async Task<List<DbProduct>> GetProducts()
        {
            return await _context.Product.ToListAsync();
        }
    }
}
