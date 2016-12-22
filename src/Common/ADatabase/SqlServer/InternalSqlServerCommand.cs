using System;
using System.Data.SqlClient;

namespace ADatabase.SqlServer
{
    public class InternalSqlServerCommand: IDisposable
    {
        public InternalSqlServerCommand(string statement, InternalSqlServerConnection connection)
        {
            Command = new SqlCommand(statement);
            Command.CommandTimeout = 3 * 60 * 60;
            Command.Connection = connection.Connection;
        }

        public SqlCommand Command { get; private set; }

        public void Dispose()
        {
            Command.Dispose();
        }
    }
}
