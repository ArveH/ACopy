using ACopyLib.Xml;
using ACopyLibTest.Helpers;
using ACopyTestHelper;
using ADatabase;
using ADatabase.Extensions;

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

        protected void CreateTable(ColumnTypeName type, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            IAXmlReader xmlReader = new AXmlReader(DbContext);
            ITableDefinition expectedTableDefinition = xmlReader.ReadSchema(
                DbContext.ColumnTypeConverterForRead,
                XmlFileHelper.CreateSchemaXmlOneColumn(TableName, type.ConvertToString(), length, prec, scale, isNullable, def, collation));
            DbSchema.CreateTable(expectedTableDefinition);
        }
    }
}