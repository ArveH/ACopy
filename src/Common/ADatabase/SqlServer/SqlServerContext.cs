using System.Data.SqlClient;
using ADatabase.Exceptions;
using ALogger;

namespace ADatabase.SqlServer
{
    public class SqlServerContext : DbContext
    {
        public SqlServerContext(IALogger logger=null)
            : base(new SqlServerPowerPlant(), logger)
        {
            DbType = DbTypeName.SqlServer;
            PowerPlant.DbContext = this;
        }

        public SqlServerContext(string conectionString, IALogger logger=null)
            : this(logger)
        {
            ConnectionString = conectionString;
        }

        public override void CreateConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override void CreateConnectionString(string user, string password, string database, string server)
        {
            if (string.IsNullOrWhiteSpace(server))
            {
                throw new ADatabaseException("Server name can't be empty");
            }
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {DataSource = server};
            if (!string.IsNullOrWhiteSpace(database))
            {
                builder.InitialCatalog = database;
            }
            builder.UserID = user;
            builder.Password = password;
            builder.MinPoolSize = 2;
            builder.MaxPoolSize = 1000;
            builder.ConnectTimeout = 10 * 60;

            ConnectionString = builder.ToString();
        }
    }
}