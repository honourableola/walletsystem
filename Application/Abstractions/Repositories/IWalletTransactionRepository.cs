using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface IWalletTransactionRepository
    {
        Task<WalletTransaction> Add(WalletTransaction transaction);
        Task<WalletTransaction> Get(Guid id);
        Task<WalletTransaction> GetUserTransactions(Guid userId);
        Task<WalletTransaction> GetUserWalletTransactions(Guid userId, string walletAddress);
    }
}
