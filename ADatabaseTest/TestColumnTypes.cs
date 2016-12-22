using System.Collections.Generic;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ADatabaseTest
{
    public abstract class TestColumnTypes
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings(@"..\..\ConnectionStrings.json");
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

        protected void TestColumn(ColumnType type, int length, bool isNullable, string def, string collation)
        {
            List<IColumn> columns = new List<IColumn> { ColumnFactory.CreateInstance(type, "col1", length, isNullable, def, collation) };
            ITableDefinition expectedTableDefinition = PowerPlant.CreateTableDefinition(TableName, columns, "");
            DbSchema.CreateTable(expectedTableDefinition);
            ITableDefinition retrievedTableDefinition = DbSchema.GetTableDefinition(TableName);

            AssertTableDefinition(expectedTableDefinition, retrievedTableDefinition);
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MIN_DATE()
        {
            TestColumn(ColumnType.DateTime, 0, false, "MIN_DATE", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("MIN_DATE");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MAX_DATE()
        {
            TestColumn(ColumnType.DateTime, 0, false, "MAX_DATE", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("MAX_DATE");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_MAX_DATE_Rounded()
        {
            TestColumn(ColumnType.DateTime, 0, false, "TS2DAY(MAX_DATE)", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("TS2DAY(MAX_DATE)");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_TODAY()
        {
            TestColumn(ColumnType.DateTime, 0, false, "TODAY", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("TODAY");
        }

        // TestMethod
        public void TestCreateTable_When_Date_And_NOW()
        {
            TestColumn(ColumnType.DateTime, 0, false, "NOW", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("NOW");
        }

        // TestMethod
        public void TestCreateTable_When_Guid_And_GUIDAsDefault()
        {
            TestColumn(ColumnType.Guid, 0, false, "GUID", "");
            DbSchema.GetTableDefinition(TableName).Columns[0].Default.Should().Be("GUID");
        }
    }
}