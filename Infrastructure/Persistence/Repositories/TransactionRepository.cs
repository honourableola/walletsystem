using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationContext _context;

        public TransactionRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<WalletTransaction> Add(WalletTransaction transaction)
        {
            await _context.AddAsync(transaction);
            return transaction;
        }

        public async Task<WalletTransaction?> Get(Guid id)
        {
            return await _context.WalletTransactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<WalletTransaction?> GetByReference(string reference)
        {
            return await _context.WalletTransactions.FirstOrDefaultAsync(t => t.Reference == reference);
        }

        public async Task<ICollection<WalletTransaction>> GetUserTransactions(Guid userId)
        {
            return await _context.WalletTransactions.Where(t => t.Wallet.UserId == userId).ToListAsync();
        }

        public async Task<ICollection<WalletTransaction>> GetUserWalletTransactions(Guid userId, string walletAddress)
        {
            return await _context.WalletTransactions
                .Where(t => t.Wallet.UserId == userId 
                && t.Wallet.WalletAddress == walletAddress)
                .ToListAsync();
        }
        public async Task<ICollection<WalletTransaction>> GetUserWalletTransactionsForTheDay(Guid userId, string walletAddress)
        {
            return await _context.WalletTransactions
                .Where(t => t.Wallet.UserId == userId
                && t.Wallet.WalletAddress == walletAddress
                && t.Date.Date == DateTime.Now.Date)
                .ToListAsync();
        }
    }
}
