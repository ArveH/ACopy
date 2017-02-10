using System.Collections.Generic;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ADatabaseTest
{
    public abstract class TestColumnTypes
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IPowerPlant PowerPlant;
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected IColumnFactory ColumnFactory;
        protected string TableName;

        public abstract void Setup();

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TableName);
        }

        protected void AssertColumns(IColumn expected, IColumn actual)
        {
            actual.Name.Should().BeEquivalentTo(expected.Name);
            actual.Type.Should().Be(expected.Type);
            actual.IsNullable.Should().Be(expected.IsNullable);
            actual.Default.Should().BeEquivalentTo(expected.Default);
            actual.Details.Should().Equal(expected.Details);
        }

        protected void AssertTableDefinition(ITableDefinition expectedTableDefinition, ITableDefinition retrievedTableDefinition)
        {
            retrievedTableDefinition.Name.Should().BeEquivalentTo(expectedTableDefinition.Name);
            AssertColumns(expectedTableDefinition.Columns[0], retrievedTableDefinition.Columns[0]);
        }

        protected void TestColumn(ColumnTypeName type, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            List<IColumn> columns = new List<IColumn> { ColumnFactory.CreateInstance(type, "col1", length, prec, scale, isNullable, def, collation) };
            ITableDefinition expectedTableDefinition = PowerPlant.CreateTableDefinition(TableName, columns, "");
            DbSchema.CreateTable(expectedTableDefinition);
            var columnTypeConverter = DbContext.PowerPlant.CreateColumnTypeConverter("Resources/Unit4OracleWriterConversions.xml");
            ITableDefinition retrievedTableDefinition = DbSchema.GetTableDefinition(columnTypeConverter, TableName);

            AssertTableDefinition(expectedTableDefinition, retrievedTableDefinition);
        }

        protected void TestDateTimeDefauleValue(string defValue)
        {
            TestColumn(ColumnTypeName.DateTime, 0, 0, 0, false, defValue, "");
        }

        public void VerifyTableDefinitionDefaultValue(string expected)
        {
            var columnTypeConverter = DbContext.PowerPlant.CreateColumnTypeConverter("Resources/Unit4OracleWriterConversions.xml");
            DbSchema.GetTableDefinition(columnTypeConverter, TableName).Columns[0].Default.Should().Be(expected);
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MIN_DATE()
        {
            TestDateTimeDefauleValue("MIN_DATE");
            VerifyTableDefinitionDefaultValue("MIN_DATE");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MAX_DATE()
        {
            TestDateTimeDefauleValue("MAX_DATE");
            VerifyTableDefinitionDefaultValue("MAX_DATE");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MAX_DATE_Rounded()
        {
            TestDateTimeDefauleValue("TS2DAY(MAX_DATE)");
            VerifyTableDefinitionDefaultValue("TS2DAY(MAX_DATE)");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_TODAY()
        {
            TestDateTimeDefauleValue("TODAY");
            VerifyTableDefinitionDefaultValue("TODAY");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_NOW()
        {
            TestDateTimeDefauleValue("NOW");
            VerifyTableDefinitionDefaultValue("NOW");
        }

        // TestMethod
        public void TestCreateTable_When_Guid_And_GUIDAsDefault()
        {
            TestColumn(ColumnTypeName.Guid, 16, 0, 0, false, "GUID", "");
            VerifyTableDefinitionDefaultValue("GUID");
        }
    }
}