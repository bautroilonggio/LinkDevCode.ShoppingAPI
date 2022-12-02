using Microsoft.EntityFrameworkCore;
using Shopping.API.Commons;
using Shopping.API.DataAccess.Entities;

namespace Shopping.API.DataAccess.DbContexts
{
    public class ShoppingContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;



        public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Phone).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasData(
                    new Account("admin", "admin", "Linh", "Nguyen", DateOnly.Parse("16/11/1999"), 
                            "0972901427", "nguyenkhaclinh100@gmail.com", "Hoang Mai", "ADMIN")
                    {
                        Id = 1
                    },
                    new Account("user", "user", "Linh", "Nguyen", DateOnly.Parse("16/11/1999"), 
                            "0928347519", "linknguyen@gmail.com", "Hai Ba Trung", "USER")
                    {
                        Id = 2
                    });
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(key => new {key.ProductId, key.OrderId});
                entity.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
                entity.HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);
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
