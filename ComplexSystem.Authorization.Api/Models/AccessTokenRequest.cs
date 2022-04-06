using System.ComponentModel.DataAnnotations;

namespace ComplexSystem.Authorization.Api.Models
{
    public class AccessTokenRequest
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string Secret { get; set; }

        [Required]
        public string Resource { get; set; }
    }
}
