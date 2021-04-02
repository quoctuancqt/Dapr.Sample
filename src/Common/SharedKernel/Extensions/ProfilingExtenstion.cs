using Microsoft.Extensions.DependencyInjection;
using System;

namespace SharedKernel.Extensions
{
    public static class ProfilingExtenstion
    {
        public static IServiceCollection AddProfiling<TIService, TService>(this IServiceCollection services,
         ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
         where TIService : class
         where TService : class, TIService
        {
            switch (serviceLifetime)
            {
                case ServiceLifetime.Singleton:

                    services.AddSingleton(p => CreateProxyByServiceProvider<TIService, TService>(p));

                    break;

                case ServiceLifetime.Transient:

                    services.AddTransient(p => CreateProxyByServiceProvider<TIService, TService>(p));

                    break;

                case ServiceLifetime.Scoped:

                    services.AddScoped(p => CreateProxyByServiceProvider<TIService, TService>(p));

                    break;
            }

            return services;
        }

        private static TIService CreateProxyByServiceProvider<TIService, TService>(IServiceProvider provider)
            where TIService : class
            where TService : class, TIService
        {
            return ServiceProfiling<TIService, TService>.CreateProxy(provider);
        }
    }
}
