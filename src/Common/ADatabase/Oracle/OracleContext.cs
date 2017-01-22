using ALogger;
using Oracle.DataAccess.Client;

namespace ADatabase.Oracle
{
    public class OracleContext: DbContext
    {
        public OracleContext(IALogger logger = null)
            : base(new OraclePowerPlant(), logger)
        {
            DbType = DbTypeName.Oracle;
            PowerPlant.DbContext = this;
        }

        public OracleContext(string connectionString, IALogger logger=null)
            : this(logger)
        {
            ConnectionString = connectionString;
        }

        public override void CreateConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override void CreateConnectionString(string user, string password, string database, string server)
        {
            OracleConnectionStringBuilder builder = new OracleConnectionStringBuilder
            {
                DataSource = server,
                UserID = user,
                Password = password,
                ConnectionTimeout = 10*60
            };

            ConnectionString = builder.ToString();
        }
    }
}
