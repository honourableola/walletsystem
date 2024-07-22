using Application.Abstractions.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("Initialize-deposit")]
        public async Task<IActionResult> AddFunds(string walletAddress, decimal amount, string email)
        {
            var userId = User.FindFirst("Id")?.Value;
            var response = await _transactionService.InitializeFunding(Guid.Parse(userId), walletAddress, amount, email);
            return Ok(response);
        }

        [HttpGet("verify-deposit")]
        public async Task<IActionResult> VerifyDeposit([FromQuery] string reference)
        {
            var userId = User.FindFirst("Id")?.Value;
            var response = await _transactionService.VerifyFunding(Guid.Parse(userId), reference);
            return Ok(response);
        }
    }
}
