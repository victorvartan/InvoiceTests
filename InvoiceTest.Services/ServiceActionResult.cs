using InvoiceTest.Models;

namespace InvoiceTest.Services
{
    public class ServiceActionResult<T> where T : BaseEntity
    {
        public T Entity { get; set; }

        public ServiceActionResultCode Code { get; set; }
    }
}
