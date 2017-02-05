using System;
using System.Collections.Generic;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.IntegrationTests
{
    public abstract class TestColumnTypes: TestBase
    {
        public virtual void Setup()
        {
            base.Setup("testcolumntypes");
        }

        protected void VerifyType(IColumn col)
        {
            ITableDefinition tableDefinition = DbSchema.GetTableDefinition(ColumnTypeConverter, TestTable);
            tableDefinition.Columns[1].Type.Should().Be(col.Type);
            tableDefinition.Columns[1].Default.Should().Be(col.Default);
            tableDefinition.Columns[1].IsNullable.Should().Be(col.IsNullable);
        }

        private void CheckString(IColumn col, string expectedValue)
        {
            VerifyType(col);
            var val = Commands.ExecuteScalar(string.Format("select test_col from {0}", TestTable));
            val.Should().Be(expectedValue);
        }

        //TestMethod
        protected void TestWriteRead_When_Varchar()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Varchar, "test_col", 50, false, "' '", "");
            CreateTestTable1Row3Columns1Value(col, "'Testing'");
            WriteAndRead();
            CheckString(col, "Testing");
        }

        //TestMethod
        protected void TestWriteRead_When_LongText()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.LongText, "test_col", -1, false, "' '", "");
            CreateTestTable1Row3Columns1Value(col, "'Testing'");
            WriteAndRead();
            CheckString(col, "Testing");
        }

        //TestMethod
        protected void TestWriteRead_When_Bool()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Bool, "test_col", false, "0");
            CreateTestTable1Row3Columns1Value(col, "1");
            WriteAndRead();
            VerifyType(col);
        }

        //TestMethod
        protected void TestWriteRead_When_Int64()
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Int64, "test_col", false, "0");
            CreateTestTable1Row3Columns1Value(col, "123456789012345");
            WriteAndRead();
            VerifyType(col);
        }

        private void CreateTestTableWith3Guids()
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid1_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid2_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid3_col", true, "")
            };
            TableDefinition tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
        }

        private void CheckTableWith3GuidsHasCorrectTypes()
        {
            ITableDefinition tableDefinition = DbSchema.GetTableDefinition(ColumnTypeConverter, TestTable);
            tableDefinition.Columns[1].Type.Should().Be(ColumnTypeName.Guid, "guid1_col should be guid");
            tableDefinition.Columns[2].Type.Should().Be(ColumnTypeName.Guid, "guid2_col should be guid");
            tableDefinition.Columns[4].Type.Should().Be(ColumnTypeName.Guid, "guid3_col should be guid");
        }

        //TestMethod
        protected void TestWriteRead_When_3Guids(Action insertFunc)
        {
            CreateTestTableWith3Guids();
            insertFunc();
            WriteAndRead();
            CheckTableWith3GuidsHasCorrectTypes();
        }

    }
}