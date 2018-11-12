using InvoiceTest.Services;
using InvoiceTest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace InvoiceTest.Api.Security
{
    internal class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public CustomAuthenticationHandler(IOptionsMonitor<CustomAuthenticationOptions> options,
                                 ILoggerFactory logger,
                                 UrlEncoder encoder,
                                 ISystemClock clock,
                                 IHttpContextAccessor httpContextAccessor,
                                 IUserService userService,
                                 IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeaderValue = Request.Headers["Authorization"].FirstOrDefault().Replace("Basic", "").Trim();

            if (string.IsNullOrEmpty(authorizationHeaderValue)) return AuthenticateResult.Fail(Resources.ExceptionMessages.InvalidApiKey);

            var user = await _userService.GetByApiKey(authorizationHeaderValue);
            if (user == null) return AuthenticateResult.Fail(Resources.ExceptionMessages.UserNotFound);

            var claims = new List<Claim> { new Claim(Constants.CLAIM_USER_ID, user.Id.ToString()), new Claim(Constants.CLAIM_USER_ROLE, user.Role.ToString()) };

            var claimsIdentity = new ClaimsIdentity(claims, "Custom");
            var principal = new ClaimsPrincipal(claimsIdentity);

            _httpContextAccessor.HttpContext.User = principal;

            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.ClaimsIssuer);
            return AuthenticateResult.Success(ticket);
        }
    }
}
