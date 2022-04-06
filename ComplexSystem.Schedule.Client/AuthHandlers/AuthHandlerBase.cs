using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.Authorization;
using ComplexSystem.Schedule.Client.Models.Settings;
using ComplexSystem.Schedule.Client.RefitClients.Authorization;
using ComplexSystem.Schedule.Client.Store;
using Microsoft.Extensions.Options;

namespace ComplexSystem.Schedule.Client.AuthHandlers
{
    internal abstract class AuthHandlerBase : DelegatingHandler
    {
        private readonly IAuthorizationApiClient _authorizationApiClient;
        private readonly AuthSettings _authSettings;
        private readonly TokenStorage _tokenStorage;

        protected AuthHandlerBase(IAuthorizationApiClient authorizationApiClient,
                                  IOptions<AuthSettings> authOptions,
                                  TokenStorage tokenStorage)
        {
            _authorizationApiClient = authorizationApiClient;
            _authSettings = authOptions.Value;
            _tokenStorage = tokenStorage;
        }

        protected abstract Resource Resource { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetToken();

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }

        private async Task<string> GetToken()
        {
            var token = _tokenStorage.VerifyAndGetToken(Resource);

            if (token is not null)
            {
                return token;
            }

            var tokenResponse = await _authorizationApiClient.RequestToken(new RequestToken
            {
                ClientId = _authSettings.Id,
                Secret = _authSettings.Secret,
                Resource = Resource.ToString()
            });

            token = tokenResponse!.Content!.AccessToken;
            _tokenStorage.SaveToken(token, Resource);

            return token;
        }
    }
}