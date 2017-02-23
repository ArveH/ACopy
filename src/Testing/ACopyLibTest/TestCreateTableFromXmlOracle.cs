using System;
using System.Data;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestCreateTableFromXmlOracle: TestCopyLibBase
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());
            TableName = "htablefromxml";

            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBinaryDoubleCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.BinaryDouble, 0, 0, 0, false, "", null);
            VerifyColumnType("BINARY_DOUBLE", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBinaryFloatCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.BinaryFloat, 0, 0, 0, false, "", null);
            VerifyColumnType("BINARY_FLOAT", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBlobCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Blob, 0, 0, 0, true, null, null);
            VerifyColumnType("BLOB", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBoolCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Bool, 0, 1, 0, false, "0", null);
            VerifyColumnType("NUMBER", null, 1, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestCharCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Char, 10, 0, 0, false, "", null);
            VerifyColumnType("CHAR", 10, 0, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestDateCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Date, 0, 0, 0, false, "", null);
            VerifyColumnType("DATE", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestDatetimeCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.DateTime, 0, 0, 0, false, "", null);
            VerifyColumnType("DATE", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestDecCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Dec, 0, 5, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 5, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestFloatCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Float, 0, 0, 0, false, "", null);
            VerifyColumnType("FLOAT", null, 126, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestFloatColWithLength_When_Oracle()
        {
            CreateTable(ColumnTypeName.Float, 47, 0, 0, false, "", null);
            VerifyColumnType("FLOAT", null, 47, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestGuidCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Guid, 0, 0, 0, false, "", null);
            VerifyColumnType("RAW", 16, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOldBlobCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.OldBlob, 0, 0, 0, false, "", null);
            VerifyColumnType("LONG RAW", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestOldTextCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.OldText, 0, 0, 0, false, "", null);
            VerifyColumnType("LONG", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestIntCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Int, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 38, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestInt16Col_When_Oracle()
        {
            CreateTable(ColumnTypeName.Int16, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 38, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestInt64Col_When_Oracle()
        {
            CreateTable(ColumnTypeName.Int64, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 38, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestInt8Col_When_Oracle()
        {
            CreateTable(ColumnTypeName.Int8, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 3, 0);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestLongTextCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.LongText, 0, 0, 0, false, "", null);
            VerifyColumnType("CLOB", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestMoneyCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Money, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 19, 4);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestNCharCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.NChar, 10, 0, 0, false, "", null);
            VerifyColumnType("NCHAR", 10, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestNLongTextCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.NLongText, 0, 0, 0, false, "", null);
            VerifyColumnType("NCLOB", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestNVarcharCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.NVarchar, 50, 0, 0, false, "", null);
            VerifyColumnType("NVARCHAR2", 50, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestRawCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Raw, 1000, 0, 0, false, "", null);
            VerifyColumnType("RAW", 1000, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestSmallDateTimeCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.SmallDateTime, 0, 0, 0, false, "", null);
            VerifyColumnType("DATE", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestSmallMoneyCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.SmallMoney, 0, 0, 0, false, "", null);
            VerifyColumnType("NUMBER", null, 10, 4);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestTimeCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Time, 0, 0, 0, false, "", null);
            VerifyColumnType("DATE", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestTimestampCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Timestamp, 0, 0, 0, false, "", null);
            VerifyColumnType("TIMESTAMP(6)", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestTimestampWithLengthCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Timestamp, 9, 0, 0, false, "", null);
            VerifyColumnType("TIMESTAMP(9)", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestVarcharCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Varchar, 250, 0, 0, false, "", null);
            VerifyColumnType("VARCHAR2", 250, null, null);
        }

        #region Private helpers

        private void VerifyColumnType(string expectedType, int? expectedLength, int? expectedPrec, int? expectedScale)
        {
            var selectStmt = "";
            selectStmt += "SELECT data_type, " + "\n";
            selectStmt += "       nvl(Decode(char_length, 0, data_length, " + "\n";
            selectStmt += "                           char_length), 0) AS col3, " + "\n";
            selectStmt += "       nvl(data_precision, 0) as col4, " + "\n";
            selectStmt += "       nvl(data_scale, 0) as col5 " + "\n";
            selectStmt += "FROM   user_tab_columns " + "\n";
            selectStmt += "WHERE  table_name = '" + TableName.ToUpper() + "' \n";
            selectStmt += "AND    column_name =  'COL1'" + "\n";

            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            string type = "";
            int length = -99;
            int prec = -99;
            int scale = -99;
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                reader.Read();
                type = reader.GetString(0);
                length = reader.GetInt16(1);
                prec = reader.GetInt16(2);
                scale = reader.GetInt32(3);
            }
            catch (Exception)
            {
                false.Should().BeTrue("because we want to fail when we cant read type from database.");
            }
            finally
            {
                cursor.Close();
            }
            type.Should().Be(expectedType);
            if (expectedLength != null) length.Should().Be(expectedLength, "because that's the expected length");
            if (expectedPrec != null) prec.Should().Be(expectedPrec, "because that's the expected precision");
            if (expectedScale != null) scale.Should().Be(expectedScale, "because that's the expected scale");
        }

        #endregion
    }
}