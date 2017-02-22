using System;
using System.Data;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest
{
    [TestClass]
    public class TestCreateTableFromXmlSqlServer: TestColumnTypesBase
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateSqlServerContext(ConnectionStrings.GetSqlServer());

            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestBlobCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Blob, 0, 0, 0, true, null, null);
            VerifyColumnType("varbinary", -1, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestBoolCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Bool, 0, 1, 0, false, "0", null);
            VerifyColumnType("bit", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestCharCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Char, 10, 0, 0, false, "", null);
            VerifyColumnType("char", 10, 0, 0);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestDateCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Date, 0, 0, 0, false, "", null);
            VerifyColumnType("date", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestDatetimeCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.DateTime, 0, 0, 0, false, "", null);
            VerifyColumnType("datetime", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestTimestampCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Timestamp, 0, 0, 0, false, "", null);
            VerifyColumnType("datetime2", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestTimestampWithLengthCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Timestamp, 5, 0, 0, false, "", null);
            VerifyColumnType("datetime2", null, null, 5);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestDecCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Dec, 0, 5, 0, false, "", null);
            VerifyColumnType("decimal", null, 5, 0);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestBinaryDoubleCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.BinaryDouble, 0, 0, 0, false, "", null);
            VerifyColumnType("float", null, 53, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestBinaryFloatCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.BinaryFloat, 0, 0, 0, false, "", null);
            VerifyColumnType("real", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestFloatCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Float, 0, 0, 0, false, "", null);
            VerifyColumnType("float", null, 53, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestFloatColWithLength_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Float, 47, 0, 0, false, "", null);
            VerifyColumnType("float", null, 53, null); // it's always 24 or 53
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestGuidCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Guid, 0, 0, 0, false, "", null);
            VerifyColumnType("uniqueidentifier", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestIdentityCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Identity, 0, 20, 0, false, "", null);
            VerifyColumnType("decimal", null, 20, 0);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestIntCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Int, 0, 0, 0, false, "", null);
            VerifyColumnType("int", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestInt16Col_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Int16, 0, 0, 0, false, "", null);
            VerifyColumnType("smallint", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestInt64Col_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Int64, 0, 0, 0, false, "", null);
            VerifyColumnType("bigint", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestInt8Col_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Int8, 0, 0, 0, false, "", null);
            VerifyColumnType("tinyint", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestLongTextCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.LongText, 0, 0, 0, false, "", null);
            VerifyColumnType("varchar", -1, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestMoneyCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Money, 0, 0, 0, false, "", null);
            VerifyColumnType("money", null, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestNCharCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.NChar, 10, 0, 0, false, "", null);
            VerifyColumnType("nchar", 10, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestNLongTextCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.NLongText, 0, 0, 0, false, "", null);
            VerifyColumnType("nvarchar", -1, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestNVarcharCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.NVarchar, 50, 0, 0, false, "", null);
            VerifyColumnType("nvarchar", 50, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestRawCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Raw, 1000, 0, 0, false, "", null);
            VerifyColumnType("varbinary", 1000, null, null);
        }

        [TestMethod, TestCategory("SqlServer")]
        public void TestVarcharCol_When_SqlServer()
        {
            CreateTable(ColumnTypeName.Varchar, 250, 0, 0, false, "", null);
            VerifyColumnType("varchar", 250, null, null);
        }

        #region Private helpers

        private void VerifyColumnType(string expectedType, int? expectedLength, int? expectedPrec, int? expectedScale)
        {
            var selectStmt = "";
            selectStmt += "SELECT c.name, " + "\n";
            selectStmt += "       t.name AS t_name, " + "\n";
            selectStmt += "       isnull(c.max_length, 0) as length, " + "\n";
            selectStmt += "       isnull(c.precision, 0) as prec, " + "\n";
            selectStmt += "       isnull(c.scale, 0) as scale, " + "\n";
            selectStmt += "       c.is_nullable, " + "\n";
            selectStmt += "       convert(varchar(256), isnull(c.collation_name, '')) as collation, " + "\n";
            selectStmt += "       isnull(object_definition(c.default_object_id), '') as def, " + "\n";
            selectStmt += "       c.is_identity " + "\n";
            selectStmt += "FROM   sys.columns c " + "\n";
            selectStmt += "       JOIN sys.types t " + "\n";
            selectStmt += "         ON c.user_type_id = t.user_type_id " + "\n";
            selectStmt += $"WHERE  c.object_id = Object_id('{TableName}') " + "\n";
            selectStmt += "ORDER  BY c.column_id ";

            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            string type = "";
            int length = -99;
            int prec = -99;
            int scale = -99;
            try
            {
                IDataReader reader = cursor.ExecuteReader(selectStmt);
                reader.Read();
                type = reader.GetString(1).ToLower();
                length = reader.GetInt16(2);
                if (length != -1 && (type == "nvarchar" || type == "nchar")) length /= 2;
                prec = reader.GetByte(3);
                scale = reader.GetByte(4);
                if (reader.GetBoolean(8))
                {
                    type = "identity";
                }
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