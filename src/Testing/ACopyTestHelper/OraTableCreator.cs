using ADatabase;

namespace ACopyTestHelper
{
    public class OraTableCreator
    {
        private readonly IDbContext _dbContext;
        private readonly ICommands _commands;
        private readonly IDbSchema _dbSchema;
        public string TableName { get; set; } = "hmsstesttable";

        public OraTableCreator(IDbContext dbContext)
        {
            _dbContext = dbContext;
            _commands = dbContext.PowerPlant.CreateCommands();
            _dbSchema = _dbContext.PowerPlant.CreateDbSchema();

        }

        public void BinaryDoubleColumn()
        {
            CreateTable("binary_double", TestTableCreator.GetBinaryDoubleSqlValue());
        }

        public void BinaryFloatColumn()
        {
            CreateTable("binary_float", TestTableCreator.GetBinaryFloatSqlValue());
        }

        public void Blob()
        {
            CreateTable("blob", TestTableCreator.GetBlobSqlValue(_dbContext));
        }

        public void CharColumn(int length)
        {
            CreateTable($"char({length})", TestTableCreator.GetCharSqlValue());
        }

        public void Clob()
        {
            CreateTable("clob", TestTableCreator.GetLongTextSqlValue());
        }

        public void Date()
        {
            CreateTable("date", TestTableCreator.GetDateTimeSqlValue(_dbContext));
        }

        public void FloatColumn(int length)
        {
            if (length <= 0)
            {
                CreateTable("float", TestTableCreator.GetBinaryDoubleSqlValue());
            }
            else if (length > 24)
            {
                CreateTable($"float({length})", TestTableCreator.GetBinaryDoubleSqlValue());
            }
            else
            {
                CreateTable($"float({length})", TestTableCreator.GetBinaryFloatSqlValue());
            }
        }

        public void LongRawColumn()
        {
            CreateTable("long raw", TestTableCreator.GetBlobSqlValue(_dbContext));
        }

        public void LongColumn()
        {
            CreateTable("long", TestTableCreator.GetLongTextSqlValue());
        }

        public void NCharColumn(int length)
        {
            CreateTable($"nchar({length})", TestTableCreator.GetNCharSqlValue(_dbContext));
        }

        #region Private

        private void CreateTable(string type, string sqlValue)
        {
            _dbSchema.DropTable(TableName);

            var stmt = $"create table {TableName} (col1 {type})";
            _commands.ExecuteNonQuery(stmt);
            stmt = $"insert into {TableName} (col1) values ({sqlValue})";
            _commands.ExecuteNonQuery(stmt);
        }

        #endregion
    }
}