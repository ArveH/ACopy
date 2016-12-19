using System;

namespace ACopyLib.Exceptions
{
    public class NotValidXmlException: Exception
    {
        public NotValidXmlException()
        {
        }

        public NotValidXmlException(string message)
            : base(message)
        {
        }

        public NotValidXmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
