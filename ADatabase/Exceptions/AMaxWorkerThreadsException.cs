using System;

namespace ADatabase.Exceptions
{
    public class AMaxWorkerThreadsException: Exception
    {
        public AMaxWorkerThreadsException()
        {
        }

        public AMaxWorkerThreadsException(string message)
            : base(message)
        {
        }

        public AMaxWorkerThreadsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }    
    }
}
