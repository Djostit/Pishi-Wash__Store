namespace Pishi_Wash__Store.Data.Models;

public partial class TradeContext : DbContext
{
    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderproduct> Orderproducts { get; set; }

    public virtual DbSet<Pcategory> Pcategories { get; set; }

    public virtual DbSet<Pmanufacturer> Pmanufacturers { get; set; }

    public virtual DbSet<Pname> Pnames { get; set; }

    public virtual DbSet<Point> Points { get; set; }

    public virtual DbSet<Pprovider> Pproviders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.OrderPickupPoint, "conn__Point");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.OrderAmmount).HasColumnType("float(10,2)");
            entity.Property(e => e.OrderDiscountAmmount).HasColumnType("float(10,2)");
            entity.Property(e => e.OrderFullName).HasColumnType("text");
            entity.Property(e => e.OrderStatus).HasMaxLength(50);

            entity.HasOne(d => d.OrderPickupPointNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderPickupPoint)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("conn__Point");
        });

        modelBuilder.Entity<Orderproduct>(entity =>
        {
            entity.HasKey(e => new { e.OrderId, e.ProductArticleNumber })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("orderproduct");

            entity.HasIndex(e => e.ProductArticleNumber, "ProductArticleNumber");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductArticleNumber).HasMaxLength(100);

            entity.HasOne(d => d.Order).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_1");

            entity.HasOne(d => d.ProductArticleNumberNavigation).WithMany(p => p.Orderproducts)
                .HasForeignKey(d => d.ProductArticleNumber)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderproduct_ibfk_2");
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
            entity.HasKey(e => e.PointId).HasName("PRIMARY");

            entity.ToTable("point");

            entity.Property(e => e.PointId).HasColumnName("PointID");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Index).HasColumnName("index");
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

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(100);
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
