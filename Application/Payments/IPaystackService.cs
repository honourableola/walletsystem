using Application.Payments.Models;
namespace Application.Payments
{
    public interface IPaystackService
    {
        Task<PaystackTransactionInitializationResponse> InitializeTransaction(decimal amount, string email);
        Task<PaystackTransactionStatus> VerifyTransaction(string reference);
        Task<string> InitiateWithdrawal(decimal amount, string bankCode, string accountNumber);
        Task<PaystackWithdrawalStatus> VerifyWithdrawal(string transferCode);
    }
}
