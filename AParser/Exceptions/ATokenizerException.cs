using System;

namespace AParser
{
    public class ATokenizerException: Exception
    {
        public ATokenizerException()
        {
        }

        public ATokenizerException(string message)
            : base(message)
        {
        }

        public ATokenizerException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
