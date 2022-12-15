using Microsoft.EntityFrameworkCore;
using Rating.Commons;
using Rating.DataAccess.Entities;

namespace Rating.DataAccess.DbContexts
{
    public class ReviewContext : DbContext
    {
        public DbSet<Review> Reviews { get; set; } = null!;

        public ReviewContext(DbContextOptions<ReviewContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

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
