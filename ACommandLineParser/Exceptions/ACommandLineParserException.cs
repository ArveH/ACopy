using System;

namespace ACommandLineParser.Exceptions
{
    public class ACommandLineParserException: Exception
    {
        public ACommandLineParserException()
        {
        }

        public ACommandLineParserException(string message)
            : base(message)
        {
        }

        public ACommandLineParserException(string message, Exception innerException)
            : base(message, innerException)
        {
        }    
    }
}
