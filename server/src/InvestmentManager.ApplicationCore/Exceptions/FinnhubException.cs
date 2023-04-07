﻿namespace InvestmentManager.ApplicationCore.Exceptions
{
    /// <summary>
    /// Represents a database connection failure
    /// </summary>
    public class FinnhubException : Exception
    {
        public FinnhubException() : base() { }

        public FinnhubException(string message) : base(message) { }

        public FinnhubException(string message, Exception? innerException) : base(message, innerException) { }
    }
}
