using System;
using System.Data;
using System.Data.SqlClient;
using ADatabase.Exceptions;

namespace ADatabase.SqlServer
{
    public class InternalSqlServerConnection
    {
        public InternalSqlServerConnection(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            try
            {
	            Connection.Open();
            }
            catch (Exception ex)
            {
            	throw new ADatabaseException(string.Format("Can't open connection {0}", ConnectionCounter+1), ex);
            }
            ConnectionCounter++;
        }

        public SqlConnection Connection { get; private set; }

        public static int ConnectionCounter { get; private set; }

        public void Close()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
                ConnectionCounter--;
            }
            Connection.Dispose();
        }

        public static T ExecuteInConnectionScope<T>(IDbContext dbContext, string sql, Func<InternalSqlServerCommand, T> func)
        {
            InternalSqlServerConnection connection = null;
            try
            {
                connection = new InternalSqlServerConnection(dbContext.ConnectionString);
                using (var command = new InternalSqlServerCommand(sql, connection))
                {
                    return func(command);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
    }
}
