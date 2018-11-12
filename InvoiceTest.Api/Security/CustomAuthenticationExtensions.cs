using Microsoft.AspNetCore.Authentication;
using System;

namespace InvoiceTest.Api.Security
{
    public static class CustomAuthenticationExtensions
    {
        public const string AUTHENTICATION_SCHEME = "Custom Scheme";

        public static AuthenticationBuilder AddCustomAuth(this AuthenticationBuilder builder, Action<CustomAuthenticationOptions> configureOptions)
        {
            return builder.AddScheme<CustomAuthenticationOptions, CustomAuthenticationHandler>(AUTHENTICATION_SCHEME, AUTHENTICATION_SCHEME, configureOptions);
        }
    }
}
