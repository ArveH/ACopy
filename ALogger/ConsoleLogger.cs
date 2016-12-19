using System;

namespace ALogger
{
    public class ConsoleLogger: IALogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Write(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            if (ex.InnerException != null)
            {
                Write(ex.InnerException);
            }
        }
    }
}
