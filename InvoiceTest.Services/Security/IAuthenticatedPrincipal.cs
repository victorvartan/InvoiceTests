using InvoiceTest.Models;
using System;

namespace InvoiceTest.Services.Security
{
    public interface IAuthenticatedPrincipal
    {
        Guid UserId { get; }

        UserRole UserRole { get; }
    }
}
