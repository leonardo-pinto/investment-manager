namespace InvestmentManager.ApplicationCore.Exceptions
{
    public class InvalidTransactionTypeException : ArgumentException
    {
        public InvalidTransactionTypeException() : base() { }

        public InvalidTransactionTypeException(string? message) : base(message) { }

        public InvalidTransactionTypeException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
