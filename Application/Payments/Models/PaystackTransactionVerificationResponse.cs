namespace Application.Payments.Models
{
    public class PaystackTransactionVerificationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public VerificationData Data { get; set; }

        public class VerificationData
        {
            public PaystackTransactionStatus Status { get; set; }
            public string Reference { get; set; }
            public decimal Amount { get; set; }
            public string GatewayResponse { get; set; }
            public string PaidAt { get; set; }
            public string CreatedAt { get; set; }
        }
    }

    public enum PaystackTransactionStatus
    {
        Success,
        Failed,
        Pending
    }
}
