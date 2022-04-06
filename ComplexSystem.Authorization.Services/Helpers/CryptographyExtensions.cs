using System;
using System.Security.Cryptography;
using System.Text;

namespace ComplexSystem.Authorization.Services.Helpers
{
    public static class CryptographyExtensions
    {
        public static string ToSHA256(this string text)
        {
            using var sha256 = new SHA256Managed();
            return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
        }
    }
}
