using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TestAssigment.Models;

namespace TestAssigment
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Cars.Brand> Brands { get; set; }
        public DbSet<Cars.Model> Models { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> option) : base(option) => Database.EnsureCreated();
        public ApplicationContext() { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Cars.Brand>().HasIndex(entity => entity.Name).IsUnique(true);
            modelBuilder.Entity<Cars.Model>().HasIndex(entity => entity.Name).IsUnique(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("temp_db");
        }
    }
}
