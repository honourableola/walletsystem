using Domain.Enums;
using System.Transactions;

namespace Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string WalletAddress { get; set; }
        public decimal Balance { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public CurrencyType Currency { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
