using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace ADatabase.Oracle
{
    public class InternalOracleCommand: IDisposable
    {
        public InternalOracleCommand(string statement, InternalOracleConnection connection)
        {
            Command = new OracleCommand();
            Command.CommandText = statement;
            Command.CommandType = CommandType.Text;
            Command.AddToStatementCache = false;
            Command.InitialLONGFetchSize = 4000;
            Command.InitialLOBFetchSize = 4000;
            Command.Connection = connection.Connection;
        }

        public OracleCommand Command { get; private set; }

        public void Dispose()
        {
            Command.Dispose();
        }
    }
}
