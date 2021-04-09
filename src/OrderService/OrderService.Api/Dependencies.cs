using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Services;
using OrderService.Infrastructure;
using SharedKernel.EventBus;
using SharedKernel.EventBus.Abstractions;
using SharedKernel.Extensions;

namespace OrderService.Api
{
    public static class Dependencies
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IEventBus, DaprEventBus>();

            services.AddTransient<DatabaseFactory>();

            return services.AddServices();
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddProfiling<IOrderService, Application.Services.OrderService>();

            return services;
        }
    }
}
