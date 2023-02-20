using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.Data
{
    public class DataContext : DbContext
    {
        public DbSet<DbUser> User { get; set; }
        public DbSet<DbProduct> Product { get; set; }
        public DbSet<DbPName> PName { get; set; }
        public DbSet<DbPManufacturer> PManufacturer { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
    }
}
