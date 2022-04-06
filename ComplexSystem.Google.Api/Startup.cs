using ComplexSystem.Common.Claims;
using ComplexSystem.Common.CustomAuthorization;
using ComplexSystem.Common.Middleware;
using ComplexSystem.Common.Resources;
using ComplexSystem.Google.Api.Policies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ComplexSystem.Resource.Api
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
            services.AddControllers();
            services.AddCustomAuthorization(settings =>
            {
                settings.TokenValidationEndpoint = "http://localhost:2000/api/authorization/client-credentials/validate-token";
            }).ConfigurePolicy(builder =>
            {
                builder.AddPolicy(PolicyName.DocsReadOnly,
                                  claims => claims.HasPermission($"{GoogleApi.Docs}.{Scope.Read}") ||
                                            claims.HasPermission($"{GoogleApi.Docs}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.DocsWriteOnly,
                                  claims => claims.HasPermission($"{GoogleApi.Docs}.{Scope.Write}") ||
                                            claims.HasPermission($"{GoogleApi.Docs}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.DocsReadWrite,
                                  claims => claims.HasPermission($"{GoogleApi.Docs}.{Scope.ReadWrite}"));
            });
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
