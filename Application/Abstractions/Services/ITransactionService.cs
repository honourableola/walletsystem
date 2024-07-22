using Application.Models;
using Application.Models.Response;

namespace Application.Abstractions.Services
{
    public interface ITransactionService
    {
        Task<InitializaDepositResponse> InitializeFunding(Guid userId, string walletAddress, decimal amount, string email);
        Task<BaseResponse> VerifyFunding(Guid userId, string reference);
    }
}
