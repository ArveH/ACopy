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
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Int, "test_col", 0, 15, 0, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Float(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Float, "test_col", 0, 30, 8, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Varchar(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Varchar, "test_col", 50, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_LongText(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.LongText, "test_col", -1, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Bool(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Bool, "test_col", 0, 1, 0, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Int64(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Int64, "test_col", 0, 20, 0, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Guid(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Guid, "test_col", 16, true, "", "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }

        //TestMethod
        protected void TestNullValue_When_Raw(Action checkValue)
        {
            IColumn col = ColumnFactory.CreateInstance(ColumnTypeName.Blob, "test_col", true, "");
            CreateTestTable1Row3Columns1Value(col);
            WriteAndRead();
            checkValue();
        }
    }
}