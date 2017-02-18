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
        protected string ConversionFileForWrite;
        protected string ConversionFileForRead;

        public virtual void Setup()
        {
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
            ColumnFactory = PowerPlant.CreateColumnFactory();
            TableName = "hmstestcolumntypes";
            DbSchema.DropTable(TableName);
        }

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
            var columnTypeConverter = DbContext.PowerPlant.CreateColumnTypeConverter(ConversionFileForWrite);
            ITableDefinition retrievedTableDefinition = DbSchema.GetTableDefinition(columnTypeConverter, TableName);

            AssertTableDefinition(expectedTableDefinition, retrievedTableDefinition);
        }
    }
}