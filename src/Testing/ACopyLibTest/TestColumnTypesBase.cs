using System.Collections.Generic;
using System.Text;
using ACopyTestHelper;
using ADatabase;

namespace ACopyLibTest
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


    }
}