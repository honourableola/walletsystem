namespace Application.Payments.Models
{
    public class PaystackTransactionInitializationResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public InitializationData Data { get; set; }

        public class InitializationData
        {
            public string Authorization_Url { get; set; }
            public string AccessCode { get; set; }
            public string Reference { get; set; }
        }
    }
}
