using Microsoft.EntityFrameworkCore;

namespace Market.Models
{
    public class ProductContext : DbContext
    {
        public DbSet<Storage> Storages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public ProductContext()
        {

        }
        public ProductContext(DbContextOptions<ProductContext> dbc) : base(dbc)
        {

        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString: "Server=DESKTOP-2609IBJ; Database=WebStore;Integrated Security=False;TrustServerCertificate=True; Trusted_Connection=True;");
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .Build();


            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"))
                .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(x => x.ProductId).HasName("ProductID");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();

                entity.Property(e => e.Description).HasColumnName("Description").HasMaxLength(255).IsRequired();

                entity.Property(e => e.Cost).HasColumnName("Cost").IsRequired();

                entity.HasOne(x => x.Category).WithMany(c => c.Products).HasForeignKey(y => y.ProductId).OnDelete(DeleteBehavior.Cascade).IsRequired().HasConstraintName("CotegoryToProduct");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("ProductCategorys");

                entity.HasKey(x => x.CategoryId).HasName("CategoryID");
                entity.HasIndex(x => x.Name).IsUnique();

                entity.Property(e => e.Name).HasColumnName("ProductName").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<Storage>(entity =>
            {
                entity.ToTable("Storage");

                entity.HasKey(x => x.StorageId).HasName("StoreId");

                entity.Property(e => e.Name).HasColumnName("StorageName");

                entity.Property(e => e.Quantity).HasColumnName("ProductQuantity");

                entity.HasMany(x => x.Products).WithMany(m => m.Stores).UsingEntity(j => j.ToTable("StorageProduct"));
            });
        }
    }
}
