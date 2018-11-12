using InvoiceTest.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InvoiceTest.Services.Interfaces
{
    public interface IInvoiceNoteService : IService<InvoiceNote>
    {
        Task<List<InvoiceNote>> GetByInvoiceId(Guid invoiceId);
    }
}
