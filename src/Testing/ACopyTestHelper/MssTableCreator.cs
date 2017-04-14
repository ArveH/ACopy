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
            CreateTable("date", TestTableCreator.GetDateSqlValue(_dbContext));
        }

        public void DateTimeColumn()
        {
            CreateTable("datetime", TestTableCreator.GetDateTimeSqlValue(_dbContext));
        }

        public void DateTime2Column(int length)
        {
            var type = "datetime2";
            if (length > 0) type += $"({length})";
            CreateTable(type, TestTableCreator.GetDateTimeSqlValue(_dbContext));
        }

        public void DecimalColumn(int prec, int scale)
        {
            CreateTable($"decimal({prec}, {scale})", TestTableCreator.GetDecSqlValue());
        }

        public void NumericColumn(int prec, int scale)
        {
            CreateTable($"numeric({prec}, {scale})", TestTableCreator.GetDecSqlValue());
        }

        public void FloatColumn(int prec)
        {
            var type = "float";
            if (prec > 0) type += $"({prec})";
            CreateTable(type, TestTableCreator.GetBinaryFloatSqlValue());
        }

        public void ImageColumn()
        {
            CreateTable("image", TestTableCreator.GetBlobSqlValue(_dbContext));
        }

        public void IntColumn()
        {
            CreateTable("int", TestTableCreator.GetIntSqlValue());
        }

        public void MoneyColumn()
        {
            CreateTable("money", TestTableCreator.GetMoneySqlValue());
        }

        public void NCharColumn(int length)
        {
            CreateTable($"nchar({length})", TestTableCreator.GetNCharSqlValue());
        }

        public void SmallIntColumn()
        {
            CreateTable("smallint", TestTableCreator.GetInt16SqlValue());
        }

        public void SmallMoneyColumn()
        {
            CreateTable("smallmoney", TestTableCreator.GetSmallMoneySqlValue());
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