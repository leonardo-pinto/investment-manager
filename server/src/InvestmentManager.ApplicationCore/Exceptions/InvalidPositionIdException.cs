namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents an invalid position id
    /// </summary>
    public class InvalidPositionIdException : ArgumentException
    {
        public InvalidPositionIdException(): base() { }

        public InvalidPositionIdException(string? message) : base(message) { }

        public InvalidPositionIdException(string? message, Exception? innerException) : base(message, innerException) { } 
    }
}
