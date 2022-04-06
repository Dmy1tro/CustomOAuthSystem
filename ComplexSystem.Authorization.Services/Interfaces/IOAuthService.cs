using System.Threading.Tasks;
using ComplexSystem.Authorization.Services.Models.OAuth;

namespace ComplexSystem.Authorization.Services.Interfaces
{
    public interface IOAuthService
    {
        GrantType GrantType { get; }

        Task<AuthenticationResult> CreateToken(string clientId, string secret, string resource);

        Task<TokenValidationResult> ValidateToken(string token);
    }
}
