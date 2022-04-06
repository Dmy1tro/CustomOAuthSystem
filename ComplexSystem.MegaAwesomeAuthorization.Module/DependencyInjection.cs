using System;
using System.Collections.Generic;
using System.Security.Claims;
using ComplexSystem.MegaAwesomeAuthorization.Module.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ComplexSystem.Common.CustomAuthorization
{
    public static class DependencyInjection
    {
        public static ICustomAuthBuilder AddCustomAuthorization(this IServiceCollection services, Action<CustomAuthSettings> setup)
        {
            services.AddHttpClient("CustomAuth");
            services.Configure<CustomAuthSettings>(setup);
            
            return new CustomAuthBuilder(services);
        }
    }

    public interface ICustomAuthBuilder
    {
        void ConfigurePolicy(Action<ICustomPolicyBuilder> policyBuilder);
    }

    public interface ICustomPolicyBuilder
    {
        ICustomPolicyBuilder AddPolicy(string name, Func<IEnumerable<Claim>, bool> predicate);
    }

    class CustomAuthBuilder : ICustomAuthBuilder
    {
        private readonly IServiceCollection _services;

        public CustomAuthBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public void ConfigurePolicy(Action<ICustomPolicyBuilder> policyBuilder)
        {
            var builder = new CustomPolicyBuilder();

            policyBuilder(builder);

            var validator = builder.BuildValidator();

            _services.AddSingleton<CustomAuthPolicyValidator>(validator);
        }
    }

    class CustomPolicyBuilder : ICustomPolicyBuilder
    {
        private readonly CustomAuthPolicyValidator _authValidator = new();

        public ICustomPolicyBuilder AddPolicy(string name, Func<IEnumerable<Claim>, bool> predicate)
        {
            _authValidator.AddPolicyRule(name, predicate);
            return this;
        }

        public CustomAuthPolicyValidator BuildValidator()
        {
            return _authValidator;
        }
    }
}
