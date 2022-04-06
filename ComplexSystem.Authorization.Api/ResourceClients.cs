using System.Collections.Generic;
using ComplexSystem.Authorization.Services.Helpers;
using ComplexSystem.Authorization.Services.Models.ClientStore;
using ComplexSystem.Common.Resources;

namespace ComplexSystem.Authorization.Api
{
    public class ResourceClients
    {
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                _serviceClient
            };
        }
        
        private static readonly Client _serviceClient = new()
        {
            Id = "service_client_id",
            SecretSHA256 = "service_client_strong_long_secret_{$%~/'@}".ToSHA256(),
            Permissions = new List<ClientPermission>
            {
                // ResourceApi
                new ClientPermission
                {
                    Resource = ResourceApi.Resource,
                    Permission = ResourceApi.Gallery,
                    Scope = Scope.ReadWrite
                },
                new ClientPermission
                {
                    Resource = ResourceApi.Resource,
                    Permission = ResourceApi.User,
                    Scope = Scope.Read
                },

                // GoogleApi
                new ClientPermission
                {
                    Resource = GoogleApi.Resource,
                    Permission = GoogleApi.Docs,
                    Scope = Scope.ReadWrite
                }
            }
        };
    }
}
