using System.Collections.Generic;
using ComplexSystem.Authorization.Services.Interfaces;
using ComplexSystem.Authorization.Services.Models.ClientStore;
using ComplexSystem.Authorization.Services.Services;
using ComplexSystem.Authorization.Services.Store;
using Microsoft.Extensions.DependencyInjection;

namespace ComplexSystem.Authorization.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAuthorizationServices(this IServiceCollection services, IEnumerable<Client> clients)
        {
            var inMemoryStore = new InMemoryStore();
            var clientStore = new ClientStore(inMemoryStore);

            foreach (var client in clients)
            {
                clientStore.AddClient(client);
            }

            return services.AddTransient<IOAuthService, OAuthService>()
                           .AddTransient<IClientStore, ClientStore>()
                           .AddSingleton<IStore>(inMemoryStore);
        }
    }
}
