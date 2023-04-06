using InvestmentManager.ApplicationCore.Domain.Entities;
using InvestmentManager.ApplicationCore.DTO;
using InvestmentManager.ApplicationCore.Helpers;
using InvestmentManager.ApplicationCore.Interfaces;

namespace InvestmentManager.ApplicationCore.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<TransactionResponse> CreateTransaction(AddTransactionRequest addTransactionRequest)
        {
            if (addTransactionRequest == null)
            {
                throw new ArgumentNullException(nameof(addTransactionRequest));
            }

            ValidationHelper.ModelValidation(addTransactionRequest);

            Transaction transaction = addTransactionRequest.ToTransaction();

            // think about exceptions
            await _transactionRepository.AddTransaction(transaction);

            TransactionResponse transactionResponse = transaction.ToTransactionResponse();

            return transactionResponse;
        }

        public async Task<List<TransactionResponse>> GetTransactionHistory(Guid positionId)
        {
            // think about null and exceptions
            List<Transaction> transactions = await _transactionRepository.GetTransactionByStockPositionId(positionId);
            return transactions.Select(temp => temp.ToTransactionResponse()).ToList();
        }

    }
}
