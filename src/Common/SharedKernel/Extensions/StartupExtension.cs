using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Intefaces;
using System.Reflection;

namespace SharedKernel.Extensions
{
    public static class StartupExtension
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton<IValidatorHandler, ValidatorHandler>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
