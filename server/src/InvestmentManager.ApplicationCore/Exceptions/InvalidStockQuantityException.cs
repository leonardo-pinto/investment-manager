namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents an invalid stock quantity to be sold
    /// </summary>
    public class InvalidStockQuantityException : ArgumentException
    {
        public InvalidStockQuantityException() : base() { }

        public InvalidStockQuantityException(string? message) : base(message) { }

        public InvalidStockQuantityException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}
