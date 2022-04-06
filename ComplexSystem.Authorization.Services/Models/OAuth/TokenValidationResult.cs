namespace ComplexSystem.Authorization.Services.Models.OAuth
{
    public class TokenValidationResult
    {
        public bool IsValid { get; set; }

        public string FailedReason { get; set; }

        public static TokenValidationResult Failed(string reason)
        {
            return new TokenValidationResult
            {
                IsValid = false,
                FailedReason = reason
            };
        }

        public static TokenValidationResult Ok()
        {
            return new TokenValidationResult
            {
                IsValid = true
            };
        }
    }
}
