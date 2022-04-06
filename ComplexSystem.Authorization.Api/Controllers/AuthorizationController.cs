using System.Threading.Tasks;
using ComplexSystem.Authorization.Api.Models;
using ComplexSystem.Authorization.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComplexSystem.Authorization.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOAuthService _oAuthService;

        public AuthorizationController(IOAuthService oAuthService)
        {
            _oAuthService = oAuthService;
        }

        [HttpPost("client-credentials/request-token")]
        public async Task<IActionResult> ClientCredentialsToken(AccessTokenRequest tokenRequest)
        {
            var token = await _oAuthService.CreateToken(tokenRequest.ClientId, tokenRequest.Secret, tokenRequest.Resource);

            if (!token.IsSuccess)
            {
                return BadRequest(new { Message = token.FailedReason });
            }

            return Ok(new { AccessToken = token.AccessToken });
        }

        [HttpPost("client-credentials/validate-token")]
        public async Task<IActionResult> ClientCredentialsValidateToken(ValidateTokenRequest validateRequest)
        {
            var result = await _oAuthService.ValidateToken(validateRequest.Token);

            if (!result.IsValid)
            {
                return BadRequest(result.FailedReason);
            }

            return NoContent();
        }
    }
}
