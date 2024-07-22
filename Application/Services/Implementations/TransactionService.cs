using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Models;
using Application.Models.Response;
using Application.Payments;
using Application.Payments.Models;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Application.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IWalletRepository _walletRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaystackService _paystackService;
        private readonly IAuditLogRepository _auditLogRepository;

        public TransactionService(ITransactionRepository transactionRepository, 
            IWalletRepository walletRepository, 
            IUserRepository userRepository, 
            IConfiguration configuration, 
            IPaystackService paystackService, 
            IUnitOfWork unitOfWork,
            IAuditLogRepository auditLogRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _paystackService = paystackService;
            _unitOfWork = unitOfWork;
            _auditLogRepository = auditLogRepository;
        }

        public async Task<InitializaDepositResponse> InitializeFunding(Guid userId, string walletAddress, decimal amount, string email)
        {
            var user = await _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var wallet = await _walletRepository.Get(walletAddress);
            if (wallet == null || wallet.UserId != userId)
            {
                throw new Exception("Wallet not found or does not belong to user.");
            }

            if(amount > user.Role.TransactionLimit)
            {
                throw new Exception($"The amount specified in the transaction is higher than the allowed limit of {user.Role.TransactionLimit}");
            }

            var userTransactionsForTheDay = await _transactionRepository.GetUserWalletTransactionsForTheDay(userId, walletAddress);
            var totalAmountOfTransactions = userTransactionsForTheDay.Select(x => x.Amount).Sum();
            if(amount + totalAmountOfTransactions > user.Role.DailyLimit)
            {
                throw new Exception($"The amount specified takes your total limit for the day to exceed the allowed limit of {user.Role.DailyLimit}");
            }

            var response = await _paystackService.InitializeTransaction(amount, email);

            if (response == null)
            {
                return new InitializaDepositResponse
                {
                    Message = "Deposit initialization failed"
                };
            }

            var transaction = new WalletTransaction
            {
                Reference = response.Data.Reference,
                Amount = amount,
                Date = DateTime.Now,
                Description = $"Deposit initialized for a sum of {amount}",
                Type = TransactionType.Deposit,
                WalletId = wallet.Id,
                Status = TransactionStatus.Pending
            };

            var auditLog = new AuditLog
            {
                Action = AuditAction.DepositInitialized,
                Timestamp = DateTime.Now,
                UserId = userId,
                Details = "Deposit initailized from a bank account"
            };

            await _transactionRepository.Add(transaction);

            if(await _unitOfWork.SaveChangesAsync() > 0)
            {
                return new InitializaDepositResponse
                {
                    Message = "Deposit initialization Successful",
                    Status = true,
                    AuthorizationUrl = response.Data.Authorization_Url
                };
            }

            return new InitializaDepositResponse
            {
                Message = "Deposit initialization failed"
            };
        }

        public async Task<BaseResponse> VerifyFunding(Guid userId, string reference)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var status = await _paystackService.VerifyTransaction(reference);

            var transaction = await _transactionRepository.GetByReference(reference);
            if (transaction == null)
                return new BaseResponse { Message = "Initialized transaction not found" };

            if (status == PaystackTransactionStatus.Failed)
            {
                transaction.Status = TransactionStatus.Failed;
                return new BaseResponse { Message = "Deposit not successful" };
            }

            if (status == PaystackTransactionStatus.Success)
            {
                transaction.Status = TransactionStatus.Verified;

                var wallet = await _walletRepository.Get(transaction.WalletId);
                if (wallet == null)
                    return new BaseResponse { Message = "Wallet associated to transaction not found" };

                wallet.Balance += transaction.Amount;
                await _walletRepository.Update(wallet);

                var auditLog = new AuditLog
                {
                    Action = AuditAction.DepositVerified,
                    Timestamp = DateTime.Now,
                    UserId = userId,
                    Details = $"Deposit Verfied and wallet credited with sum of {transaction.Amount}"
                };

                await _auditLogRepository.Create(auditLog);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                    return new BaseResponse { Message = "Payment verified successfully", Status = true };
            }

            return new BaseResponse { Message = "Deposit not successful" };
        }

        public async Task<string> Withdraw(Guid userId, int walletId, decimal amount, string bankCode, string accountNumber)
        {
            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            /*var wallet = _walletRepository.GetWalletById(walletId);
            if (wallet == null || wallet.UserId != userId)
            {
                throw new Exception("Wallet not found or does not belong to user.");
            }*/

            // Check transaction limits
            //CheckTransactionLimits(user, amount);

            // Check sufficient balance
            /*if (wallet.Balance < amount)
            {
                throw new Exception("Insufficient balance.");
            }*/

            // Initiate Paystack withdrawal
            var transferCode = await _paystackService.InitiateWithdrawal(amount, bankCode, accountNumber);

            // Log the withdrawal
            //_auditLogService.LogAction(user.Id, "Withdraw", $"Initiated withdrawal of {amount} from wallet {walletId} to bank account {accountNumber}.");

            return transferCode;
        }

        /*private void CheckTransactionLimits(User user, decimal amount)
        {
            var transactionLimits = _configuration.GetSection("TransactionLimits").Get<Dictionary<string, TransactionLimit>>();
            if (transactionLimits.ContainsKey(user.Role))
            {
                var limits = transactionLimits[user.Role];

                if (amount > limits.SingleTransactionLimit)
                {
                    throw new Exception("Transaction amount exceeds the single transaction limit.");
                }

                var dailyTotal = _transactionRepository.GetDailyTotal(user.Id, DateTime.UtcNow.Date);
                if (dailyTotal + amount > limits.DailyLimit)
                {
                    throw new Exception("Transaction amount exceeds the daily limit.");
                }
            }
        }*/
    }

}
