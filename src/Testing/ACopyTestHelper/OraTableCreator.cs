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