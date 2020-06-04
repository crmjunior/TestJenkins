using CAContext.Application.AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Hosting;
using CAContext.Data.IoC;
using CAContext.Infra.Data.Mappings;

namespace CAContext.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);
                
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt => {
                opt.AddDefaultPolicy(bldr => {
                    bldr.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            services.AddMemoryCache();
            services.AddAuthorization();

            IMapper mapper = AutoMapperConfig.Register();
            services.AddSingleton(mapper);

            services.AddControllers()
                .AddFluentValidation(f => f.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            RegisterServices(services);
            DapperMapping.RegisterDapperMappings();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(
                options => options.SetIsOriginAllowed(x => _ = true)
                                    .AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void RegisterServices(IServiceCollection services)
        {
            NativeInjectorBootStrapper.RegisterServices(services);
        }
    }
}