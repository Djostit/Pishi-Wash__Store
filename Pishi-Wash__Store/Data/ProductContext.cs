using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.Data
{
    public class ProductContext : DbContext
    {
        public DbSet<DbProduct> Product { get; set; }
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options) { }
    }
}
