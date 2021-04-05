using ProductService.Application.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace ProductService.Infrastructure
{
    public class ProductContext : ApplicationContext
    {
        public ProductContext(DbContextOptions options) : base(options)
        {
        }

        public ProductContext(DbContextOptions options, IHttpContextAccessor _httpContextAccessor)
            : base(options, _httpContextAccessor)
        {
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id).ValueGeneratedOnAdd();
        }
    }
}
