using InvoiceTest.DataAccess;
using InvoiceTest.Models;
using InvoiceTest.Services.Interfaces;
using InvoiceTest.Services.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceTest.Services
{
    public class InvoiceNoteService : IInvoiceNoteService
    {
        private readonly DataContext _dataContext;
        private readonly IAuthenticatedPrincipal _authenticatedPrincipal;

        public InvoiceNoteService(DataContext dataContext, IAuthenticatedPrincipal authenticatedPrincipal)
        {
            _authenticatedPrincipal = authenticatedPrincipal;
            _dataContext = dataContext;
            _dataContext.CurrentUserId = _authenticatedPrincipal.UserId;
        }

        public async Task<List<InvoiceNote>> GetByInvoiceId(Guid invoiceId)
        {
            var invoiceNotes = await _dataContext.InvoiceNotes.Where(n => n.InvoiceId == invoiceId).ToListAsync();
            return invoiceNotes;
        }

        public async Task<InvoiceNote> GetById(Guid id)
        {
            var invoiceNote = await _dataContext.InvoiceNotes.SingleOrDefaultAsync(n => n.Id == id);
            return invoiceNote;
        }

        public async Task<ServiceActionResult<InvoiceNote>> Create(InvoiceNote item)
        {
            _dataContext.InvoiceNotes.Add(item);
            await _dataContext.SaveChangesAsync();
            return new ServiceActionResult<InvoiceNote> { Entity = item, Code = ServiceActionResultCode.Success };
        }

        public async Task<ServiceActionResult<InvoiceNote>> Update(InvoiceNote item)
        {
            var existingInvoiceNote = await _dataContext.InvoiceNotes.SingleOrDefaultAsync(i => i.Id == item.Id);
            if (existingInvoiceNote == null) return new ServiceActionResult<InvoiceNote> { Entity = item, Code = ServiceActionResultCode.InvoiceNoteNotFound };
            if (_authenticatedPrincipal.UserId != existingInvoiceNote.CreatedByUserId) return new ServiceActionResult<InvoiceNote> { Entity = item, Code = ServiceActionResultCode.OnlyOwnersAllowed };

            existingInvoiceNote.CopyFrom(item);

            await _dataContext.SaveChangesAsync();

            return new ServiceActionResult<InvoiceNote> { Entity = existingInvoiceNote, Code = ServiceActionResultCode.Success };
        }
    }
}
