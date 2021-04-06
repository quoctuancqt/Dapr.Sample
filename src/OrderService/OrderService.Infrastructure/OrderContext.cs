using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Entities;
using SharedKernel;

namespace OrderService.Infrastructure
{
    public class OrderContext : ApplicationContext
    {
        public OrderContext(DbContextOptions options) : base(options)
        {
        }

        public OrderContext(DbContextOptions options, IHttpContextAccessor _httpContextAccessor) : base(options, _httpContextAccessor)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Order>().OwnsOne(o => o.Address, a => a.WithOwner());

            modelBuilder.Entity<OrderItem>().HasKey(x => x.Id);
            modelBuilder.Entity<OrderItem>().Property(x => x.Id).ValueGeneratedOnAdd();

        }
    }
}
