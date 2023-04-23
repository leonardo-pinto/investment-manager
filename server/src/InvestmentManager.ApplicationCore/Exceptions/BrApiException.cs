namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents a brapi api connection failure
    /// </summary>
    public class BrApiException : Exception
    {
        public BrApiException() : base() { }

        public BrApiException(string message) : base(message) { }

        public BrApiException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
