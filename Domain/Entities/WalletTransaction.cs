using Domain.Enums;

namespace Domain.Entities
{
    public class WalletTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid WalletId { get; set; }
        public TransactionType Type { get; set; }
        public TransactionStatus Status { get; set; }
        public Wallet Wallet { get; set; }

    }
}
