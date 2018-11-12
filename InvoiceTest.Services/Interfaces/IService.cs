using InvoiceTest.Models;
using System;
using System.Threading.Tasks;

namespace InvoiceTest.Services.Interfaces
{
    public interface IService<T> where T : BaseEntity
    {
        Task<T> GetById(Guid id);

        Task<ServiceActionResult<T>> Create(T item);

        Task<ServiceActionResult<T>> Update(T item);
    }
}
