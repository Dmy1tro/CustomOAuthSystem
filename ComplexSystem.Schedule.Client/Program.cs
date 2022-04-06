using System;
using System.Threading;
using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.AuthHandlers;
using ComplexSystem.Schedule.Client.Models.Settings;
using ComplexSystem.Schedule.Client.RefitClients.Authorization;
using ComplexSystem.Schedule.Client.RefitClients.GoogleApi;
using ComplexSystem.Schedule.Client.RefitClients.ResourceApi;
using ComplexSystem.Schedule.Client.Store;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace ComplexSystem.Schedule.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Console.CancelKeyPress += (sender, args) =>
            {
                cts.Cancel();
                Console.WriteLine("Worker is stopped.");
                args.Cancel = true;
            };

            var worker = BuildServiceProvider().GetRequiredService<Worker>();

            try
            {
                worker.Start(cts.Token).GetAwaiter().GetResult();
            }
            catch (TaskCanceledException)
            {
                // it's ok
            }
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = (IServiceCollection)new ServiceCollection();

            services.Configure<AuthSettings>(settings =>
            {
                settings.Id = "service_client_id";
                settings.Secret = "service_client_strong_long_secret_{$%~/'@}";
            });

            services.AddSingleton<Worker>()
                    .AddSingleton<TokenStorage>()
                    .AddTransient<ResourceApiAuthHandler>()
                    .AddTransient<GoogleApiAuthHandler>();

            services.AddRefitClient<IAuthorizationApiClient>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:2000/api");
                    });

            services.AddRefitClient<IGalleryApiClient>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:5000/api");
                    }).AddHttpMessageHandler<ResourceApiAuthHandler>();

            services.AddRefitClient<IUserApiClient>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:5000/api");
                    }).AddHttpMessageHandler<ResourceApiAuthHandler>();

            services.AddRefitClient<IGoogleDocsApiService>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri("http://localhost:6000/api");
                    }).AddHttpMessageHandler<GoogleApiAuthHandler>();

            return services.BuildServiceProvider();
        }
    }
}
