using ALogger;

namespace ADatabase
{
    public interface IDbContext
    {
        DbTypeName DbType { get; }
        void CreateConnectionString(string connectionString);
        void CreateConnectionString(string user, string password, string database, string server);
        string ConnectionString { get; }
        IPowerPlant PowerPlant { get; }
        IALogger Logger { get; }
        IColumnTypeConverter ColumnTypeConverterForWrite { get; set; }
        IColumnTypeConverter ColumnTypeConverterForRead { get; set; }
    }
}