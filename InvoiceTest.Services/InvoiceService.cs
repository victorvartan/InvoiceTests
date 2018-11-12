using InvoiceTest.DataAccess;
using InvoiceTest.Models;
using InvoiceTest.Services.Interfaces;
using InvoiceTest.Services.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InvoiceTest.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthenticatedPrincipal _authenticatedPrincipal;

        public InvoiceService(DataContext dataContext, IAuthenticatedPrincipal authenticatedPrincipal)
        {
            _authenticatedPrincipal = authenticatedPrincipal;
            _dataContext = dataContext;
            _dataContext.CurrentUserId = _authenticatedPrincipal.UserId;
        }

        public async Task<Invoice> GetById(Guid id)
        {
            var invoice = await _dataContext.Invoices.Include(i => i.Notes).SingleOrDefaultAsync(i => i.Id == id);
            return invoice;
        }

        public async Task<ServiceActionResult<Invoice>> Create(Invoice item)
        {
            if (_authenticatedPrincipal.UserRole != UserRole.Admin) return new ServiceActionResult<Invoice> { Entity = item, Code = ServiceActionResultCode.OnlyAdminsAllowed };

            _dataContext.Invoices.Add(item);
            await _dataContext.SaveChangesAsync();

            return new ServiceActionResult<Invoice> { Entity = item, Code = ServiceActionResultCode.Success };
        }

        public async Task<ServiceActionResult<Invoice>> Update(Invoice item)
        {
            if (_authenticatedPrincipal.UserRole != UserRole.Admin) return new ServiceActionResult<Invoice> { Entity = item, Code = ServiceActionResultCode.OnlyAdminsAllowed };

            var existingInvoice = await _dataContext.Invoices.SingleOrDefaultAsync(i => i.Id == item.Id);
            if (existingInvoice == null) return new ServiceActionResult<Invoice> { Entity = item, Code = ServiceActionResultCode.InvoiceNotFound };

            if (_authenticatedPrincipal.UserId != existingInvoice.CreatedByUserId) return new ServiceActionResult<Invoice> { Entity = item, Code = ServiceActionResultCode.OnlyOwnersAllowed };

            existingInvoice.CopyFrom(item);

            await _dataContext.SaveChangesAsync();

            return new ServiceActionResult<Invoice> { Entity = existingInvoice, Code = ServiceActionResultCode.Success };
        }
    }
}
