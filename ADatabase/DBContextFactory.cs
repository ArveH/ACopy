using System.Configuration;
using ADatabase.Oracle;
using ADatabase.SqlServer;

namespace ADatabase
{
    public static class DbContextFactory 
    {
        public static IDbContext CreateSqlServerContext(string connectionStringName)
        {
            return new SqlServerContext(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }

        public static IDbContext CreateSqlServerContext()
        {
            return new SqlServerContext();
        }

        public static IDbContext CreateOracleContext(string connectionStringName)
        {
            return new OracleContext(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
        }

        public static IDbContext CreateOracleContext()
        {
            return new OracleContext();
        }
    }
}
