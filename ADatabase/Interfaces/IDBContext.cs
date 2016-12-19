using ALogger;

namespace ADatabase
{
    public interface IDbContext
    {
        DbType DbType { get; }
        void CreateConnectionString(string connectionString);
        void CreateConnectionString(string user, string password, string database, string server);
        string ConnectionString { get; }
        IPowerPlant PowerPlant { get; }
        IALogger Logger { get; }
    }
}