using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IUserRepository
    {
        Task<User> Create(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User?> GetById(Guid id);
        Task<bool> Exists(string email);
        Task<User?> GetByEmail(string email);
        Task<User> Update(User user);
    }
}
