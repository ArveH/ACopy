using System;
using ADatabase;

namespace ACopyLibTest.IntegrationTests
{
    public abstract class TestNullValues: TestBase
    {
        public virtual void Setup()
        {
            base.Setup("testnullvalues");
        }

        protected override void CreateTestTable1Row3Columns1Value(IColumn col, string nullValue = "NULL")
        {
            base.CreateTestTable1Row3Columns1Value(col, nullValue);
        }

        //TestMethod
        protected void TestNullValue_When_Int(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Int, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Float(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Float, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Varchar(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Varchar, "test_col", 50, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_LongText(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.LongText, "test_col", -1, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Bool(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Bool, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Int64(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Int64, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Guid(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Guid, "test_col", 16, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Raw(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnType.Raw, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }
    }
}