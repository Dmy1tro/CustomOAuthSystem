namespace ComplexSystem.Authorization.Services.Models.OAuth
{
    public class AuthenticationResult
    {
        public bool IsSuccess { get; set; }

        public string AccessToken { get; set; }

        public string FailedReason { get; set; }

        public static AuthenticationResult Failed(string reason)
        {
            return new AuthenticationResult
            {
                IsSuccess = false,
                FailedReason = reason
            };
        }

        public static AuthenticationResult Ok(string token)
        {
            return new AuthenticationResult
            {
                IsSuccess = true,
                AccessToken = token
            };
        }
    }
}
