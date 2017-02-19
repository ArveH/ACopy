using ALogger;

namespace ADatabase
{
    public abstract class DbContext : IDbContext
    {
        protected DbContext(IPowerPlant powerPlant, IALogger logger)
        {
            PowerPlant = powerPlant;
            Logger = logger ?? new ConsoleLogger();
        }

        protected DbContext(string connectionString, IPowerPlant powerPlant, IALogger logger): this(powerPlant, logger)
        {
            ConnectionString = connectionString;
        }

        public DbTypeName DbType { get; protected set; }

        public abstract void CreateConnectionString(string connectionString);
        public abstract void CreateConnectionString(string user, string password, string database, string server);
        public abstract IColumnTypeConverter ColumnTypeConverterForWrite { get; set; }
        public abstract IColumnTypeConverter ColumnTypeConverterForRead { get; set; }

        public string ConnectionString { get; protected set; }
        public IPowerPlant PowerPlant { get; protected set; }
        public IALogger Logger { get; protected set; }
    }
}