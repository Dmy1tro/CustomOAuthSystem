using ComplexSystem.Common.Claims;
using ComplexSystem.Common.CustomAuthorization;
using ComplexSystem.Common.Middleware;
using ComplexSystem.Common.Resources;
using ComplexSystem.Resource.Api.Policies;
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
                builder.AddPolicy(PolicyName.GalleryReadOnly,
                                  claims => claims.HasPermission($"{ResourceApi.Gallery}.{Scope.Read}") ||
                                            claims.HasPermission($"{ResourceApi.Gallery}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.GalleryWriteOnly,
                                  claims => claims.HasPermission($"{ResourceApi.Gallery}.{Scope.Write}") ||
                                            claims.HasPermission($"{ResourceApi.Gallery}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.GalleryReadWrite,
                                  claims => claims.HasPermission($"{ResourceApi.Gallery}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.UserReadOnly,
                                  claims => claims.HasPermission($"{ResourceApi.User}.{Scope.Read}") ||
                                            claims.HasPermission($"{ResourceApi.User}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.UserWriteOnly,
                                  claims => claims.HasPermission($"{ResourceApi.User}.{Scope.Write}") ||
                                            claims.HasPermission($"{ResourceApi.User}.{Scope.ReadWrite}"));

                builder.AddPolicy(PolicyName.UserReadWrite,
                                  claims => claims.HasPermission($"{ResourceApi.User}.{Scope.ReadWrite}"));
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
