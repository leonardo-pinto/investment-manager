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

        [HttpGet]
        async public Task<IActionResult> GetAllTransactions()
        {
            List<TransactionResponse> transactionHistory = await _transactionService.GetAllTransactions();

            return Ok(transactionHistory);
        }
    }
}
