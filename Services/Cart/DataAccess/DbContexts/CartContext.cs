using Microsoft.EntityFrameworkCore;

namespace Cart.DataAccess.DbContexts
{
    public class CartContext : DbContext
    {
        public DbSet<Entities.Cart> Carts { get; set; } = null!;



        public CartContext(DbContextOptions<CartContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
