using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentManager.Web.Controllers
{
    [Route("api/transactions")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("userId/{userId}")]
        async public Task<IActionResult> GetAllTransactionsByUserId(string userId)
        {
            List<TransactionResponse> transactionHistory = await _transactionService.GetAllTransactionsByUserId(userId);

            return Ok(transactionHistory);
        }
    }
}
