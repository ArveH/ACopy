using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace ADatabase.Oracle
{
    /// <summary>
    /// Had to create this class to try to solve a problem I had with connection pool growing when running tests. 
    /// Could indicate that some resources are not released properly.
    /// OBS: Can probably remove this when problem is solved (yeah, right! :-)
    /// </summary>
    public class InternalOracleConnection
    {
        public OracleConnection Connection { get; private set; }

        public static int ConnectionCounter { get; private set; }

        public InternalOracleConnection(string connectionString)
        {
            Connection = new OracleConnection(connectionString);
            Connection.Open();
            ConnectionCounter++;
        }

        public void Dispose()
        {
            if (Connection.State != ConnectionState.Closed)
            {
                Connection.Close();
                ConnectionCounter--;
            }
            Connection.Dispose();
        }
    }
}
