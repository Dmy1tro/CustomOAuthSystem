using Microsoft.AspNetCore.Mvc;

namespace ComplexSystem.Common.CustomAuthorization
{
    public class CustomAuthCheckAttribute : TypeFilterAttribute
    {
        public CustomAuthCheckAttribute(string policy) : base(typeof(CustomAuthCheckerFilter))
        {
            Arguments = new object[] { policy };
        }
    }
}
