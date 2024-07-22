using Domain.Entities;

namespace Application.Abstractions.Repositories
{
    public interface ITransactionRepository
    {
        Task<WalletTransaction> Add(WalletTransaction transaction);
        Task<WalletTransaction?> Get(Guid id);
        Task<WalletTransaction?> GetByReference(string reference);
        Task<ICollection<WalletTransaction>> GetUserTransactions(Guid userId);
        Task<ICollection<WalletTransaction>> GetUserWalletTransactions(Guid userId, string walletAddress);
        Task<ICollection<WalletTransaction>> GetUserWalletTransactionsForTheDay(Guid userId, string walletAddress);
    }
}
