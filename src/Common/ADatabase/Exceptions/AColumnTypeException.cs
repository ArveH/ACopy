using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
