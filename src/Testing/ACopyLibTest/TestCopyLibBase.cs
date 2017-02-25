using ACopyLib.Xml;
using ACopyLibTest.Helpers;
using ACopyTestHelper;
using ADatabase;
using ADatabase.Extensions;

namespace ACopyLibTest
{
    public class TestCopyLibBase
    {
        protected readonly ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IPowerPlant PowerPlant;
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;
        protected string TableName;

        public virtual void Setup()
        {
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
        }

        public virtual void Cleanup()
        {
            //DbSchema.DropTable(TableName);
        }

        protected void CreateTable(ColumnTypeName type, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            IAXmlReader xmlReader = new AXmlReader(DbContext);
            ITableDefinition tableDefinition = xmlReader.ReadSchema(
                DbContext.ColumnTypeConverterForRead,
                XmlFileHelper.CreateSchemaXmlOneColumn(TableName, type.ConvertToString(), length, prec, scale, isNullable, def, collation));
            DbSchema.CreateTable(tableDefinition);
        }

        protected void CreateTableWithAllColumns(bool addDeprecatedTypes)
        {
            if (!DbSchema.IsTable(TableName))
            {
                TestTableCreator.CreateTestTableWithAllTypes(DbContext, TableName, addDeprecatedTypes);
            }
        }
    }
}