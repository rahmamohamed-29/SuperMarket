using Microsoft.EntityFrameworkCore;
using SmartMart.Models;

namespace SmartMart.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=db68822.public.databaseasp.net; Database=db68822; User Id=db68822; Password=8Ke+N-2o6r?R; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");
        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, Name = "Fruits & Vegetables", Description = "Fresh fruits and vegetables." }
                );
        }
    }
}
