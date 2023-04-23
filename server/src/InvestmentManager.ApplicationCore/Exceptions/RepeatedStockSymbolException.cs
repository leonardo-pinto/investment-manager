namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents a repeated stock symbol
    /// </summary>
    public class RepeatedStockSymbolException : ArgumentException
    {
        public RepeatedStockSymbolException() : base() { }

        public RepeatedStockSymbolException(string? message) : base(message) { }

        public RepeatedStockSymbolException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
