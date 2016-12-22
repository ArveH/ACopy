using System;

namespace ACopyLib.Exceptions
{
    public class ExecuteReaderException: Exception
    {
        public ExecuteReaderException()
        {
        }

        public ExecuteReaderException(string message)
            : base(message)
        {
        }

        public ExecuteReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
