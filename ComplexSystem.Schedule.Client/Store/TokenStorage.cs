using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using ComplexSystem.Schedule.Client.Models.Authorization;

namespace ComplexSystem.Schedule.Client.Store
{
    internal class TokenStorage
    {
        private const string TokenKey = "access_token";
        private readonly ConcurrentDictionary<string, string> _store = new();

        public void SaveToken(string token, Resource resource)
        {
            _store.AddOrUpdate($"{TokenKey}_{resource}", token, (k, v) => token);
        }

        public string? VerifyAndGetToken(Resource resource)
        {
            var key = $"{TokenKey}_{resource}";

            if (!_store.ContainsKey(key))
            {
                return null;
            }

            _store.TryGetValue(key, out var token);

            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // If token near to expire then remove it
            if (jwtToken.ValidTo < DateTime.UtcNow.AddSeconds(-30))
            {
                _store.TryRemove(KeyValuePair.Create(key, token));
                return null;
            }

            return token;
        }
    }
}
