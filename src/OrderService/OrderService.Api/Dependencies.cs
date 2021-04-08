using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Services;
using SharedKernel.Extensions;

namespace OrderService.Api
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddProfiling<IOrderService, Application.Services.OrderService>();
            //services.AddScoped<IOrderService, Application.Services.OrderService>();

            return services;
        }
    }
}
