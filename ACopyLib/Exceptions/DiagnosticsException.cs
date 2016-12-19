using System;

namespace ACopyLib.Exceptions
{
    public class DiagnosticsException: Exception
    {
        public DiagnosticsException()
        {
        }

        public DiagnosticsException(string message)
            : base(message)
        {
        }

        public DiagnosticsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
