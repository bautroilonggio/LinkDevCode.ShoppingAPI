using Microsoft.EntityFrameworkCore;
using Catalog.Commons;
using Catalog.DataAccess.Entities;

namespace Catalog.DataAccess.DbContexts
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; } = null!;

        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasData(
                    new Product()
                    {
                        Id = 1,
                        Name = "Galaxy S21 FE (5G)",
                        Brand = "Samsung",
                        Category = "Smartphone",
                        Description = "8GB/128GB",
                        SellingPrice = 11850000,
                        TotalQuantity = 20,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    },
                    new Product()
                    {
                        Id = 2,
                        Name = "Galaxy S22 Ultra",
                        Brand = "Samsung",
                        Category = "Smartphone",
                        Description = "12GB/512GB",
                        SellingPrice = 28990000,
                        TotalQuantity = 25,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    },
                    new Product()
                    {
                        Id = 3,
                        Name = "Galaxy Watch 5 Pro",
                        Brand = "Samsung",
                        Category = "Smartwatch",
                        Description = "45mm",
                        SellingPrice = 10990000,
                        TotalQuantity = 21,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    },
                    new Product()
                    {
                        Id = 4,
                        Name = "Galaxy Buds 2 Pro",
                        Brand = "Samsung",
                        Category = "Bluetooth Headphone",
                        Description = "In-Ear",
                        SellingPrice = 4590000,
                        TotalQuantity = 14,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    },
                    new Product()
                    {
                        Id = 5,
                        Name = "Samsung Sound Tower ST50B/XV",
                        Brand = "Samsung",
                        Category = "Bluetooth Speaker",
                        Description = "Sound Tower",
                        SellingPrice = 8490000,
                        TotalQuantity = 18,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    },
                    new Product()
                    {
                        Id = 6,
                        Name = "Galaxy Tab S8 Ultra",
                        Brand = "Samsung",
                        Category = "Tablet",
                        Description = "8B/128GB",
                        SellingPrice = 25390000,
                        TotalQuantity = 12,
                        CreateAt = DateOnly.Parse(DateTime.Now.ToShortDateString())
                    });
            });

            base.OnModelCreating(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }
    }
}
