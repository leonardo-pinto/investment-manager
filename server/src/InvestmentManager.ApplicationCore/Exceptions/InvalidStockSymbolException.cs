namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents an invalid stock symbol
    /// </summary>
    public class InvalidStockSymbolException : ArgumentException
    {
        public InvalidStockSymbolException() : base() { }

        public InvalidStockSymbolException(string? message) : base(message) { }

        public InvalidStockSymbolException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
