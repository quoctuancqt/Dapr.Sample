using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Intefaces;
using System;
using System.Reflection;

namespace SharedKernel.Extensions
{
    public static class StartupExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, Action<IMapperConfigurationExpression> mapperConfig = null)
        {
            if (mapperConfig != null)
            {
                services.AddAutoMapper(mapperConfig);
            }
            else
            {
                services.AddAutoMapper(Assembly.GetExecutingAssembly());
            }

            services.AddSingleton<IValidatorHandler, ValidatorHandler>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
