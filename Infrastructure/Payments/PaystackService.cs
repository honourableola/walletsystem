using Application.Payments;
using Application.Payments.Models;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Infrastructure.Payments
{
    public class PaystackService : IPaystackService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PaystackService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            var secretKey = configuration["Paystack:SecretKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
        }

        public async Task<PaystackTransactionInitializationResponse?> InitializeTransaction(decimal amount, string email)
        {
            var request = new
            {
                amount = amount * 100, // Convert to kobo
                email,
                callback_url = _configuration["Paystack:CallbackUrl"]
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.paystack.co/transaction/initialize", request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<PaystackTransactionInitializationResponse>();
        }

        public async Task<PaystackTransactionStatus> VerifyTransaction(string reference)
        {
            var response = await _httpClient.GetAsync($"https://api.paystack.co/transaction/verify/{reference}");
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var responseData = await response.Content.ReadFromJsonAsync<PaystackTransactionVerificationResponse>();
            return responseData.Data.Status;
        }

        public async Task<string> InitiateWithdrawal(decimal amount, string bankCode, string accountNumber)
        {
            var request = new
            {
                amount = amount * 100, // Convert to kobo
                bank_code = bankCode,
                account_number = accountNumber
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.paystack.co/transfer", request);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<PaystackWithdrawalResponse>();
            return responseData.Data.TransferCode;
        }

        public async Task<PaystackWithdrawalStatus> VerifyWithdrawal(string transferCode)
        {
            var response = await _httpClient.GetAsync($"https://api.paystack.co/transfer/{transferCode}");
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadFromJsonAsync<PaystackWithdrawalVerificationResponse>();
            return Enum.Parse<PaystackWithdrawalStatus>(responseData.Data.Status, true);
        }
    }
}
