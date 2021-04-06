using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Intefaces;
using System;

namespace SharedKernel.Extensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddApplicationDbContext<TContext>(this IServiceCollection services, IConfiguration configuration, string connectionStringName = "Default") where TContext : ApplicationContext
        {
            services.AddScoped<IApplicationContext>(sp =>
            {
                var connectString = configuration.GetConnectionString(connectionStringName);

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>().UseSqlite(connectString);

                var httpContextAccessor = (IHttpContextAccessor)sp.GetService(typeof(IHttpContextAccessor));

                return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options, httpContextAccessor);
            });

            return services;
        }
    }
}
