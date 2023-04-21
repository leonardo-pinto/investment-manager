using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// Get all transactions for a user id
        /// </summary>
        [HttpGet]
        [Route("user-id/{userId}")]
        async public Task<ActionResult<TransactionsResponse>> GetAllTransactionsByUserId(string userId)
        {
            IEnumerable<TransactionResponse> transactionHistory = await _transactionService.GetAllTransactionsByUserId(userId);

            return Ok(new TransactionsResponse() { Transactions = transactionHistory });
        }
    }
}
