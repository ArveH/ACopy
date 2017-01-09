using System;

namespace ADatabase.Exceptions
{
    public class AColumnTypeException: Exception
    {
        public AColumnTypeException()
        {
        }

        public AColumnTypeException(string message)
                : base(message)
        {
        }

        public AColumnTypeException(string message, Exception innerException)
                : base(message, innerException)
        {
        }
    }
}
