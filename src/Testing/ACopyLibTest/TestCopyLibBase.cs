using System.IO;
using System.Xml;
using ACopyLib.Xml;
using ACopyLibTest.Helpers;
using ACopyTestHelper;
using ADatabase;
using ADatabase.Extensions;
using FluentAssertions;

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

        #region Xml stuff

        protected void CheckColumnType(XmlDocument xmlDocument, string columnType)
        {
            var typeNode = xmlDocument.DocumentElement?.SelectSingleNode("/Table/Columns/Column/Type");
            typeNode.Should().NotBeNull("because column has to have Type");
            typeNode?.InnerText.Should().Be(columnType);
        }

        protected void CheckColumnType(XmlDocument xmlDocument, string columnType, int length)
        {
            CheckColumnType(xmlDocument, columnType, "Length", length);
        }

        protected void CheckColumnType(XmlDocument xmlDocument, string columnType, string name, int size)
        {
            CheckColumnType(xmlDocument, columnType);

            var detailsNode = xmlDocument.DocumentElement?.SelectSingleNode("/Table/Columns/Column/Details");
            var lengthNode = detailsNode?.SelectSingleNode(name);
            lengthNode.Should().NotBeNull($"because column should have {name}");
            lengthNode?.InnerText.Should().Be(size.ToString());
        }

        protected void CheckColumnType(XmlDocument xmlDocument, string columnType, int prec, int scale)
        {
            CheckColumnType(xmlDocument, columnType);

            var detailsNode = xmlDocument.DocumentElement?.SelectSingleNode("/Table/Columns/Column/Details");
            var precNode = detailsNode?.SelectSingleNode("Prec");
            precNode.Should().NotBeNull("because column should have precision");
            precNode?.InnerText.Should().Be(prec.ToString());
            var scaleNode = detailsNode?.SelectSingleNode("Scale");
            scaleNode.Should().NotBeNull("because column should have scale");
            scaleNode?.InnerText.Should().Be(scale.ToString());
        }

        protected void CheckThatDetailDoesNotExist(string schemaFile, string tagName)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(File.ReadAllText(schemaFile));

            var detailsNode = xmlDocument.DocumentElement?.SelectSingleNode("/Table/Columns/Column/Details");
            var lengthNode = detailsNode?.SelectSingleNode(tagName);
            lengthNode.Should().BeNull($"because '{tagName}' detail should NOT exist");
        }

        #endregion
    }
}