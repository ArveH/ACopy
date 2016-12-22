using System;
using System.Data.SqlClient;

namespace ADatabase.Exceptions
{
    public class ADatabaseException: Exception
    {
        public ADatabaseException()
        {
        }

        public ADatabaseException(string message)
            : base(message)
        {
        }

        public ADatabaseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private string GetAllMessages(Exception exception)
        {
            if (exception.InnerException == null)
            {
                return exception.Message;
            }
            else
            {
                return GetAllMessages(exception.InnerException);
            }
        }

        public override string ToString()
        {
            return GetAllMessages(this);
        }

        public static bool ShouldThrottle(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return 
                    (ex is SqlException && ((ex as SqlException).Number == 10928 ||(ex as SqlException).Number == 10929)) 
                    || (ex.Message.Contains("limit") && ex.Message.Contains("reached"));
            }

            return ShouldThrottle(ex.InnerException);
        }
    }
}
