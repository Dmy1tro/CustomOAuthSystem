using System.ComponentModel.DataAnnotations;

namespace ComplexSystem.Authorization.Api.Models
{
    public class ValidateTokenRequest
    {
        [Required]
        public string Token { get; set; }
    }
}
