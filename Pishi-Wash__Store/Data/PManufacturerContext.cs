namespace Pishi_Wash__Store.Data
{
    public class PManufacturerContext : DbContext
    {
        public DbSet<DbPManufacturer> PManufacturer { get; set; }
        public PManufacturerContext(DbContextOptions<PManufacturerContext> options)
            : base(options) { }
    }
}
