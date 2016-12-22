using System;

namespace ACopyLib.Exceptions
{
    public class BlobDataException: Exception
    {
        public BlobDataException()
        {
        }

        public BlobDataException(string message)
            : base(message)
        {
        }
    }
}
