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

        public void BigIntColumn()
        {
            CreateTable("bigint", TestTableCreator.GetInt64SqlValue());
        }

        public void BinaryColumn()
        {
            CreateTable("binary(50)", TestTableCreator.GetRawSqlValue(_dbContext));
        }

        public void BitColumn()
        {
            CreateTable("bit", TestTableCreator.GetBoolSqlValue());
        }

        public void CharColumn(int length)
        {
            CreateTable($"char({length})", TestTableCreator.GetCharSqlValue());
        }

        public void DateColumn()
        {
            CreateTable($"date", TestTableCreator.GetDateSqlValue(_dbContext));
        }

        #region Private

        private void CreateTable(string type, string sqlValue)
        {
            var stmt = $"if OBJECT_ID('{TableName}', 'U') is not null drop table {TableName}";
            _commands.ExecuteNonQuery(stmt);
            stmt = $"create table {TableName} (col1 {type})";
            _commands.ExecuteNonQuery(stmt);
            stmt = $"insert into {TableName} (col1) values ({sqlValue})";
            _commands.ExecuteNonQuery(stmt);
        }

        #endregion
    }
}