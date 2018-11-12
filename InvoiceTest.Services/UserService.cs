using InvoiceTest.DataAccess;
using InvoiceTest.Models;
using InvoiceTest.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InvoiceTest.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;

        public UserService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<User> GetByApiKey(string apiKey)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.ApiKey == apiKey);
            return user;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<bool> Any()
        {
            var anyUsers = await _dataContext.Users.AnyAsync();
            return anyUsers;
        }

        public Task<ServiceActionResult<User>> Create(User item)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceActionResult<User>> Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
