using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SharedKernel.Intefaces;
using System;
using System.IO;

namespace SharedKernel
{
    public abstract class ApplicationContextDesignTimeDbContextFactory<TContext>
        : IDesignTimeDbContextFactory<TContext> where TContext : ApplicationContext, IApplicationContext
    {
        protected readonly string ConnectionStringName;

        public ApplicationContextDesignTimeDbContextFactory(string connectionStringName = "Default")
        {
            ConnectionStringName = connectionStringName;
        }

        public TContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: true)
               .Build();

            var builder = new DbContextOptionsBuilder<TContext>();

            var connectionString = configuration.GetConnectionString(ConnectionStringName);

            builder.UseSqlite(connectionString);

            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
    }
}
