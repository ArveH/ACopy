using ADatabase;
using ADatabase.SqlServer.Columns;

namespace ACopyTestHelper
{
    public class MssTableCreator
    {
        private readonly IDbContext _dbContext;
        private readonly ICommands _commands;
        public string TableName { get; set; } = "hmsstesttable";

        public MssTableCreator(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _commands = dbContext.PowerPlant.CreateCommands();
        }

        public void SqlServerBigIntColumn()
        {
            var stmt = $"if OBJECT_ID('{TableName}', 'U') is not null drop table {TableName}";
            _commands.ExecuteNonQuery(stmt);
            stmt = $"create table {TableName} (col1 bigint)";
            _commands.ExecuteNonQuery(stmt);
            stmt = $"insert into {TableName} (col1) values ({TestTableCreator.GetInt64SqlValue()})";
            _commands.ExecuteNonQuery(stmt);
        }
    }
}