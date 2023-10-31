using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InvestmentManager.Web.Controllers
{
    [Route("transactions")]
    [Authorize]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        [HttpGet]
        async public Task<ActionResult<TransactionsResponse>> GetAllTransactions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            IEnumerable<TransactionResponse> transactionHistory = await _transactionService.GetAllTransactionsByUserId(userId);

            return Ok(new TransactionsResponse() { Transactions = transactionHistory });
        }
    }
}
