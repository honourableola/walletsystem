using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    internal class WalletTransactionRespository : IWalletTransactionRepository
    {
        private readonly ApplicationContext _context;
        public WalletTransactionRespository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<WalletTransaction> Add(WalletTransaction transaction)
        {
            var wallet = await _context.WalletTransactions.AddAsync(transaction);
            return await wallet;
        }

        public async Task<WalletTransaction> Get(Guid id)
        {
            var walletTrans = await _context.WalletTransactions
                .FirstOrDefaultAsync(a => a.Id == id);
            return await walletTrans;
        }

        public async Task<WalletTransaction> GetUserTransactions(Guid userId)
        {
            var userTrans = await _context.WalletTransactions
                .FirstOrDefaultAsync(x => x.UserId == userId);
            return await userTrans;
        }

        public async Task<WalletTransaction> GetUserWalletTransactions(Guid userId, string walletAddress)
        {
            var userWallet = await _context.WalletTransactions.Include(a => a.Wallet)
                .FirstOrDefaultAsync(s => s.UserId == userId && s.WalletAddress == walletAddress);
            return await userWallet;
        }
    }
}
