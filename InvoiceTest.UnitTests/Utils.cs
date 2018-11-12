using FakeItEasy;
using InvoiceTest.DataAccess;
using InvoiceTest.Models;
using InvoiceTest.Services;
using InvoiceTest.Services.Security;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;

namespace InvoiceTest.UnitTests
{
    public class Utils
    {
        public static void AddTestUsers(DataContext dataContext)
        {
            dataContext.Users.AddRange(
                new User { Id = Guid.NewGuid(), ApiKey = "admin123", Role = UserRole.Admin, DateCreated = DateTime.UtcNow, CreatedByUserId = Guid.Empty },
                new User { Id = Guid.NewGuid(), ApiKey = "admin345", Role = UserRole.Admin, DateCreated = DateTime.UtcNow, CreatedByUserId = Guid.Empty },
                new User { Id = Guid.NewGuid(), ApiKey = "user123", Role = UserRole.User, DateCreated = DateTime.UtcNow, CreatedByUserId = Guid.Empty },
                new User { Id = Guid.NewGuid(), ApiKey = "user345", Role = UserRole.User, DateCreated = DateTime.UtcNow, CreatedByUserId = Guid.Empty }
            );
            dataContext.SaveChanges();
        }

        public static IAuthenticatedPrincipal MockAuthenticatedPrincipal(Guid userId, UserRole userRole)
        {
            var userIdClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(Constants.CLAIM_USER_ID, userId.ToString())));
            var userRoleClaim = A.Fake<Claim>(x => x.WithArgumentsForConstructor(() => new Claim(Constants.CLAIM_USER_ROLE, userRole.ToString())));

            var httpContextAccessor = A.Fake<IHttpContextAccessor>();
            httpContextAccessor.HttpContext = A.Fake<HttpContext>();
            httpContextAccessor.HttpContext.User = A.Fake<ClaimsPrincipal>();
            var ipAddress = IPAddress.Parse("127.0.0.1");
            A.CallTo(() => httpContextAccessor.HttpContext.Connection.RemoteIpAddress).Returns(ipAddress);
            A.CallTo(() => httpContextAccessor.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            A.CallTo(() => httpContextAccessor.HttpContext.User.Claims).Returns(new List<Claim> { userIdClaim, userRoleClaim });

            return new AuthenticatedPrincipal(httpContextAccessor);
        }
    }
}