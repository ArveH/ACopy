using ADatabase.Oracle;
using ADatabase.SqlServer;

namespace ADatabase
{
    public static class DbContextFactory 
    {
        public static IDbContext CreateSqlServerContext(string connectionString)
        {
            return new SqlServerContext(connectionString);
        }

        public static IDbContext CreateSqlServerContext()
        {
            return new SqlServerContext();
        }

        public static IDbContext CreateOracleContext(string connectionString)
        {
            return new OracleContext(connectionString);
        }

        public static IDbContext CreateOracleContext()
        {
            return new OracleContext();
        }
    }
}
