namespace Application.Payments.Models
{
    public class PaystackWithdrawalResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public WithdrawalData Data { get; set; }

        public class WithdrawalData
        {
            public string TransferCode { get; set; }
            public string Reference { get; set; }
            public string Status { get; set; }
            public string CreatedAt { get; set; }
            public string Currency { get; set; }
        }
    }
}
