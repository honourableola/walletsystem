namespace Application.Payments.Models
{
    public class PaystackWithdrawalVerificationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public VerificationData Data { get; set; }

        public class VerificationData
        {
            public string Status { get; set; } // Could be "success", "failed", "pending"
            public string TransferCode { get; set; }
            public decimal Amount { get; set; }
            public string BankCode { get; set; }
            public string AccountNumber { get; set; }
            public string CreatedAt { get; set; }
        }
    }

    public enum PaystackWithdrawalStatus
    {
        Success,
        Failed,
        Pending
    }
}
