using ComplexSystem.Schedule.Client.Models.Authorization;
using ComplexSystem.Schedule.Client.Models.Settings;
using ComplexSystem.Schedule.Client.RefitClients.Authorization;
using ComplexSystem.Schedule.Client.Store;
using Microsoft.Extensions.Options;

namespace ComplexSystem.Schedule.Client.AuthHandlers
{
    internal class ResourceApiAuthHandler : AuthHandlerBase
    {
        public ResourceApiAuthHandler(IAuthorizationApiClient authorizationApiClient,
                                      IOptions<AuthSettings> authOptions,
                                      TokenStorage tokenStorage)
            : base(authorizationApiClient, authOptions, tokenStorage)
        {
        }

        protected override Resource Resource { get; } = Resource.ResourceApi;
    }
}