using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> Create(Role role);
        Task<Role> Get(Guid id);
        Task<Role> GetByName(string name);
        Task<IEnumerable<Role>> GetAll();
    }
}
