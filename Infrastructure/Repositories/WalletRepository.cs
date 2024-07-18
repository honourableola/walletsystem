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
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationContext _context;
        public WalletRepository(ApplicationContext context)
        {
            _context = context;
        }
        public async Task<Wallet> Create(Wallet wallet)
        {
            var wall = await _context.Wallets.AddAsync(wallet);
            return await wall;
                
        }

        public async Task Delete(Wallet wallet)
        {
            var walls = await _context.Wallets.Remove(wallet);
            return await walls;

        }

        public async Task<Wallet> Update(Wallet wallet)
        {
            var wallets = await _context.Wallets.Update(wallet);
            return await wallets;
        }
    }
}
