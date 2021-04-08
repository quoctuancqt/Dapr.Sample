using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ProductService.Api;
using ProductService.Application.Dto;
using ProductService.Infrastructure;
using SharedKernel.Extensions;
using SharedKernel.Mapping;

namespace ProductService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var assembly = typeof(ProductDto).Assembly;

            services.AddControllers()
                //.AddFluentValidation(config => config.RegisterValidatorsFromAssembly(assembly))
                .AddDapr();

            services.AddApplicationDbContext<ProductContext>(Configuration);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductService", Version = "v1" });
            });

            services.AddApplication(config =>
            {
                config.AddProfile(new MappingProfile(assembly));
            }).AddInfrastructure();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DatabaseFactory databaseFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductService v1"));
            }

            databaseFactory.Migrate();
            databaseFactory.SeedData();

            app.UseCustomExceptionHandler(HandlerException.Handle);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
