using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWalletRepository
    {
        Task<Wallet> Create(Wallet wallet);
        Task<Wallet> Update(Wallet wallet);
        Task Delete(Wallet wallet);
    }
}
