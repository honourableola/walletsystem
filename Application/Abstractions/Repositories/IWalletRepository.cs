using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet> Create(Wallet wallet);
        Task<Wallet> Update(Wallet wallet);
        Task<Wallet?> Get(Guid id);
        Task<Wallet?> Get(string address);
        Task Delete(Wallet wallet);
    }
}
