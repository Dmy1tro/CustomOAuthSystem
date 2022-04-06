using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ComplexSystem.MegaAwesomeAuthorization.Module.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace ComplexSystem.Common.CustomAuthorization
{
    public class CustomAuthCheckerFilter : IAsyncActionFilter
    {
        private readonly HttpClient _httpClient;
        private readonly CustomAuthSettings _authSettings;
        private readonly CustomAuthPolicyValidator _authPolicyValidator;
        private readonly string _policyName;

        public CustomAuthCheckerFilter(IHttpClientFactory httpClientFactory,
                                       IOptions<CustomAuthSettings> authSettings,
                                       CustomAuthPolicyValidator authPolicies,
                                       string policy)
        {
            _httpClient = httpClientFactory.CreateClient("CustomAuth");
            _authSettings = authSettings.Value;
            _authPolicyValidator = authPolicies;
            _policyName = policy;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check is token in header
            if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Make request to OAuth.Api to verify that token is trusted
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();
            var request = JsonConvert.SerializeObject(new { Token = token });
            using var content = new StringContent(request, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(_authSettings.TokenValidationEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            // Verify policy
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            if (!_authPolicyValidator.CheckPolicyRule(_policyName, jwtToken.Claims))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            await next();
        }
    }
}
