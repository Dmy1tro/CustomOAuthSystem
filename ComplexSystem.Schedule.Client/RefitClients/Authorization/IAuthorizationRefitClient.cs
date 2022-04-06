using System.Threading.Tasks;
using ComplexSystem.Schedule.Client.Models.Authorization;
using Refit;

namespace ComplexSystem.Schedule.Client.RefitClients.Authorization
{
    internal interface IAuthorizationApiClient
    {
        [Post("/authorization/client-credentials/request-token")]
        Task<ApiResponse<TokenResponse>> RequestToken(RequestToken requestToken);
    }
}
