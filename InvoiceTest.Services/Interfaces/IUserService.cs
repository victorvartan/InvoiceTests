using InvoiceTest.Models;
using System.Threading.Tasks;

namespace InvoiceTest.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> GetByApiKey(string apiKey);

        Task<bool> Any();
    }
}
