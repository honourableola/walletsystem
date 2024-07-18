using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<User> Create(User user1)
        {
            var user = await _applicationContext.Users.AddAsync(user1);
            return user1;
        }

        public async Task<User> DeActivate(Guid UserId)
        {
            var get = await _applicationContext.Users
                .FirstOrDefaultAsync( a => a.Id == UserId);
            get.IsActive = true;
            return get;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _applicationContext.Users
                .Include(a => a.Wallets)
                .Include(a => a.AuditLogs)
                .ToListAsync();
            return users;

        }

        public async Task<User> GetByEmail(string Email)
        {
            var user = await _applicationContext.Users
                .Include(a => a.Wallets)
                .Include(a => a.AuditLogs)
                .FirstOrDefaultAsync(a => a.Email == Email);
            return user;
        }

        public async Task<User> GetById(Guid id)
        {
            var user = await _applicationContext.Users
                .Include(a => a.Wallets)
                .Include(a => a.AuditLogs)
                .FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }
    }
}
