using Application.Abstractions.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationContext _context;

        public WalletRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Wallet> Create(Wallet wallet)
        {
            await _context.Wallets.AddAsync(wallet);
            return wallet;
        }

        public async Task<Wallet?> Get(string address)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.WalletAddress == address);
        }

        public async Task<Wallet?> Get(Guid id)
        {
            return await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
        }

        public Task Delete(Wallet wallet)
        {
            throw new NotImplementedException();
        }

        public Task<Wallet> Update(Wallet wallet)
        {
            throw new NotImplementedException();
        }
    }
}
