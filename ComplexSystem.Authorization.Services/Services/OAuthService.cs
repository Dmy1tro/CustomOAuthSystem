using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ComplexSystem.Authorization.Services.Helpers;
using ComplexSystem.Authorization.Services.Interfaces;
using ComplexSystem.Authorization.Services.Models.OAuth;
using ComplexSystem.Authorization.Services.Models.Settings;
using ComplexSystem.Common.Claims;
using ComplexSystem.Common.Resources;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TokenValidationResult = ComplexSystem.Authorization.Services.Models.OAuth.TokenValidationResult;

namespace ComplexSystem.Authorization.Services.Services
{
    internal class OAuthService : IOAuthService
    {
        private readonly IClientStore _clientStore;
        private readonly OAuthSettings _authSettings;

        public OAuthService(IClientStore clientStore, IOptions<OAuthSettings> authSettings)
        {
            _clientStore = clientStore;
            _authSettings = authSettings.Value;
        }

        public GrantType GrantType { get; } = GrantType.ClientCridentials;

        public Task<AuthenticationResult> CreateToken(string clientId, string secret, string resource)
        {
            var client = _clientStore.GetClient(clientId);

            if (client is null)
            {
                return Task.FromResult(AuthenticationResult.Failed($"Client with id '{clientId}' was not found."));
            }

            if (client.SecretSHA256 != secret.ToSHA256())
            {
                return Task.FromResult(AuthenticationResult.Failed("Client secret is incorrect."));
            }

            if (!client.Permissions.Any(p => p.Resource == resource))
            {
                return Task.FromResult(AuthenticationResult.Failed($"Client doesn't have access to the resource '{resource}'."));
            }

            var permissions = client.Permissions.Where(p => p.Resource == resource).ToArray();

            var claims = new List<Claim>();
            claims.Add(new Claim(CustomClaimType.ClientId, client.Id));
            claims.AddRange(permissions.Select(p => new Claim(CustomClaimType.Permissions, $"{p.Permission}.{p.Scope}")));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_authSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "Certified OAuth server",
                Audience = resource,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(1)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return Task.FromResult(AuthenticationResult.Ok(jwtToken));
        }

        public Task<TokenValidationResult> ValidateToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            try
            {
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Key)),
                    ValidIssuer = "Certified OAuth server",
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateAudience = true,
                    ValidAudiences = new []
                    {
                        ResourceApi.Resource,
                        GoogleApi.Resource
                    }
                }, out var securityToken);

                return Task.FromResult(TokenValidationResult.Ok());
            }
            catch (Exception ex)
            {
                return Task.FromResult(TokenValidationResult.Failed(ex.Message));
            }
        }
    }
}
