using Microsoft.EntityFrameworkCore;
using TAC.Domain.Infrastructure.Extensions;

namespace TAC.Domain.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public AppDbContext()
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                  .HasMany(x => x.Vehicles)
                  .WithOne(x => x.Customer);

            modelBuilder.Seed();
        }
    }
}
