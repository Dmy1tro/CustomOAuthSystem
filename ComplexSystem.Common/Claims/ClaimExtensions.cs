using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace ComplexSystem.Common.Claims
{
    public static class ClaimExtensions
    {
        public static bool HasPermission(this IEnumerable<Claim> claims, string value)
        {
            return claims.Any(c => c.Type == CustomClaimType.Permissions && c.Value == value);
        }
    }
}
