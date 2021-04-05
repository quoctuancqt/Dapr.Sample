using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProductService.Infrastructure
{
    public class ApplicationContextDesignTimeDbContextFactory
        : IDesignTimeDbContextFactory<ProductContext>
    {
        public ProductContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
               .Build();

            var builder = new DbContextOptionsBuilder<ProductContext>();

            var connectionString = configuration.GetConnectionString("Default");

            builder.UseSqlite(connectionString);

            return new ProductContext(builder.Options);
        }
    }
}
