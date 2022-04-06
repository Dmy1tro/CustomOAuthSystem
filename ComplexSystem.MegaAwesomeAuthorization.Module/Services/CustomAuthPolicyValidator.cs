using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Claims;

namespace ComplexSystem.MegaAwesomeAuthorization.Module.Services
{
    public class CustomAuthPolicyValidator
    {
        private readonly ConcurrentDictionary<string, Func<IEnumerable<Claim>, bool>> _policyRules = new();

        public void AddPolicyRule(string name, Func<IEnumerable<Claim>, bool> rule)
        {
            _policyRules.AddOrUpdate(name, rule, (k, v) => rule);
        }

        public bool CheckPolicyRule(string name, IEnumerable<Claim> claims)
        {
            _policyRules.TryGetValue(name, out var rule);

            return rule(claims);
        }
    }
}
