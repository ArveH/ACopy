using System;

namespace AParser
{
    public class AParserException: Exception
    {
        public AParserException()
        {
        }

        public AParserException(string message)
            : base(message)
        {
        }

        public AParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
