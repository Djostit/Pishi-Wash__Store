using Microsoft.EntityFrameworkCore;
using Pishi_Wash__Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pishi_Wash__Store.Data
{
    public class DataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }
    }
}
