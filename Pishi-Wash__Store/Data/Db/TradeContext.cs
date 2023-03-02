namespace Pishi_Wash__Store.Data.Db;
public partial class TradeContext : DbContext
{
    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agenttype> Agenttypes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Pcategory> Pcategories { get; set; }

    public virtual DbSet<Pmanufacturer> Pmanufacturers { get; set; }

    public virtual DbSet<Pname> Pnames { get; set; }

    public virtual DbSet<Point> Points { get; set; }

    public virtual DbSet<Pprovider> Pproviders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Agenttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("agenttype");

            entity.Property(e => e.Image).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderPickupPoint).HasColumnType("text");
            entity.Property(e => e.OrderStatus).HasColumnType("text");
        });

        modelBuilder.Entity<Pcategory>(entity =>
        {
            entity.HasKey(e => e.PcategoryId).HasName("PRIMARY");

            entity.ToTable("pcategory");

            entity.Property(e => e.PcategoryId).HasColumnName("PCategoryID");
            entity.Property(e => e.ProductCategory).HasColumnType("text");
        });

        modelBuilder.Entity<Pmanufacturer>(entity =>
        {
            entity.HasKey(e => e.PmanufacturerId).HasName("PRIMARY");

            entity.ToTable("pmanufacturer");

            entity.Property(e => e.PmanufacturerId).HasColumnName("PManufacturerID");
            entity.Property(e => e.ProductManufacturer).HasColumnType("text");
        });

        modelBuilder.Entity<Pname>(entity =>
        {
            entity.HasKey(e => e.PnameId).HasName("PRIMARY");

            entity.ToTable("pname");

            entity.Property(e => e.PnameId).HasColumnName("PNameID");
            entity.Property(e => e.ProductName).HasColumnType("text");
        });

        modelBuilder.Entity<Point>(entity =>
        {
            entity.HasKey(e => e.Index).HasName("PRIMARY");

            entity.ToTable("point");

            entity.Property(e => e.Index)
                .ValueGeneratedNever()
                .HasColumnName("index");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Street)
                .HasMaxLength(100)
                .HasColumnName("street");
        });

        modelBuilder.Entity<Pprovider>(entity =>
        {
            entity.HasKey(e => e.PproviderId).HasName("PRIMARY");

            entity.ToTable("pprovider");

            entity.Property(e => e.PproviderId).HasColumnName("PProviderID");
            entity.Property(e => e.ProductProvider).HasColumnType("text");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.ProductCategory, "conn__PCategory");

            entity.HasIndex(e => e.ProductManufacturer, "conn__PManufacturer");

            entity.HasIndex(e => e.ProductName, "conn__PName");

            entity.HasIndex(e => e.ProductProvider, "conn__PProvider");

            entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);
            entity.Property(e => e.ProductCost).HasColumnType("float(10,2)");
            entity.Property(e => e.ProductDescription).HasColumnType("text");
            entity.Property(e => e.ProductPhoto).HasMaxLength(100);
            entity.Property(e => e.ProductStatus).HasColumnType("text");

            entity.HasOne(d => d.ProductCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategory)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__PCategory");

            entity.HasOne(d => d.ProductManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductManufacturer)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__PManufacturer");

            entity.HasOne(d => d.ProductNameNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductName)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__PName");

            entity.HasOne(d => d.ProductProviderNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductProvider)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__PProvider");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("producttype");

            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("supplier");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Inn).HasMaxLength(12);
            entity.Property(e => e.SupplierType).HasMaxLength(20);
            entity.Property(e => e.Title).HasMaxLength(150);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRole, "conn__User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
