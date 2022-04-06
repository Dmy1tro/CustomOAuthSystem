using ComplexSystem.Authorization.Services;
using ComplexSystem.Authorization.Services.Models.ClientStore;
using ComplexSystem.Authorization.Services.Models.Settings;
using ComplexSystem.Common.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComplexSystem.Authorization.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<OAuthSettings>(Configuration.GetSection("OAuthSettings"));
            services.AddAuthorizationServices(ResourceClients.GetClients());
            
            services.AddControllers();
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
