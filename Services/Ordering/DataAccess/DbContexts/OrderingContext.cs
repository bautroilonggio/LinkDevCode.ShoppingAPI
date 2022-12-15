using Microsoft.EntityFrameworkCore;
using Ordering.Commons;
using Ordering.DataAccess.Entities;

namespace Ordering.DataAccess.DbContexts
{
    public class OrderingContext : DbContext
    {
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;



        public OrderingContext(DbContextOptions<OrderingContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(key => new {key.ProductId, key.OrderId});
                entity.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
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
