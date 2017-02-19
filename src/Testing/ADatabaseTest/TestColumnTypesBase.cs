using System.Collections.Generic;
using ACopyTestHelper;
using ADatabase;

namespace ADatabaseTest
{
    public class TestColumnTypesBase
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IPowerPlant PowerPlant;
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected IColumnFactory ColumnFactory;
        protected string TableName;

        public virtual void Setup()
        {
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
            ColumnFactory = PowerPlant.CreateColumnFactory();
            TableName = "htestcolumntypes";
            DbSchema.DropTable(TableName);
        }

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TableName);
        }

        protected void CreateTable(ColumnTypeName type, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            List<IColumn> columns = new List<IColumn> { ColumnFactory.CreateInstance(type, "col1", length, prec, scale, isNullable, def, collation) };
            ITableDefinition expectedTableDefinition = PowerPlant.CreateTableDefinition(TableName, columns, "");
            DbSchema.CreateTable(expectedTableDefinition);
        }
    }
}