using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationContext _context;

        public RoleRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Role> Create(Role role)
        {
            await _context.Roles.AddAsync(role);
            return role;
        }

        public Task<Role> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Role>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Role?> GetByName(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}
