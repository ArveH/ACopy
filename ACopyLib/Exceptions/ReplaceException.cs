using System;

namespace ACopyLib.Exceptions
{
    public class ReplaceException: Exception
    {
        public ReplaceException()
        {
        }

        public ReplaceException(string message)
            : base(message)
        {
        }

        public ReplaceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
