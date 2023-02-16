﻿namespace Pishi_Wash__Store.Data
{
    public class UserContext : DbContext
    {
        public DbSet<DbUser> User { get; set; }
        public UserContext(DbContextOptions<UserContext> options)
            : base(options) { }
    }
}
