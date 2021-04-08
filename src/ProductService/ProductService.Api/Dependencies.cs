using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Interfaces;
using ProductService.Infrastructure;
using SharedKernel.Extensions;

namespace ProductService.Api
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<DatabaseFactory>();

            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddProfiling<IProductService, Application.Services.ProductService>();
            //services.AddScoped<IProductService, Application.Services.ProductService>();

            return services;
        }
    }
}
