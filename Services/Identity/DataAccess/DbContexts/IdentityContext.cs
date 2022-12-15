using Microsoft.EntityFrameworkCore;
using Identity.Commons;
using Identity.DataAccess.Entities;

namespace Identity.DataAccess.DbContexts
{
    public class IdentityContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; } = null!;

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.UserName).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.HasData(
                    new Account()
                    {
                        Id = 1,
                        UserName = "admin",
                        Password = "admin",
                        FirstName = "Linh",
                        LastName = "Nguyen",
                        DateOfBirth = DateOnly.Parse("16/11/1999"),
                        Phone = "0972901427",
                        Email = "admin@gmail.com",
                        Address = "Hoang Mai",
                        Role = "ADMIN"
                    },
                    new Account()
                    {
                        Id = 2,
                        UserName = "user",
                        Password = "user",
                        FirstName = "Linh",
                        LastName = "Nguyen",
                        DateOfBirth = DateOnly.Parse("16/11/1999"),
                        Phone = "0928347519",
                        Email = "user@gmail.com",
                        Address = "Hai Ba Trung",
                        Role = "USER"
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
