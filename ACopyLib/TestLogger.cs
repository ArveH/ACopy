using System;
using ACopyLib.Exceptions;
using ADatabase.Oracle;
using ALogger;

namespace ACopyLib
{
    internal class TestLogger: IALogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }

        public void Write(Exception ex)
        {
            throw new DiagnosticsException(string.Format("Connections: {0}", InternalOracleConnection.ConnectionCounter), ex);
        }
    }
}
