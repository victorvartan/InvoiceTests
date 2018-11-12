using InvoiceTest.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace InvoiceTest.Services.Security
{
    public class AuthenticatedPrincipal : IAuthenticatedPrincipal
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedPrincipal(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => GetClaim<Guid>(Constants.CLAIM_USER_ID);

        public UserRole UserRole => GetClaim<UserRole>(Constants.CLAIM_USER_ROLE);

        private T GetClaim<T>(string claimType)
        {
            var user = _httpContextAccessor.HttpContext.User;
            if (user == null) return default(T);

            var claim = user.Claims.FirstOrDefault(c => c.Type == claimType);
            if (claim == null) return default(T);

            var claimValueType = typeof(T);
            if (claimValueType == typeof(Guid)) return (T)Convert.ChangeType(new Guid(claim.Value), claimValueType);

            if (claimValueType.IsEnum) return (T)Enum.Parse(claimValueType, claim.Value, true);

            return (T)Convert.ChangeType(claim.Value, claimValueType);
        }
    }
}
