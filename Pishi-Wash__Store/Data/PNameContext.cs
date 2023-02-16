namespace Pishi_Wash__Store.Data
{
    public class PNameContext : DbContext
    {
        public DbSet<DbPName> PName{ get; set; }
        public PNameContext(DbContextOptions<PNameContext> options)
            : base(options) { }
    }
}
