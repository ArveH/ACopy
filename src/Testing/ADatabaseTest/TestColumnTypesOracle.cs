using System;
using System.Data;
using ACopyTestHelper;
using ADatabase;
using ADatabase.Extensions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypesOracle: TestColumnTypesBase
    {
        [TestInitialize]
        public override void Setup()
        {
            DbContext = DbContextFactory.CreateOracleContext(ConnectionStrings.GetOracle());

            base.Setup();
        }

        [TestCleanup]
        public override void Cleanup()
        {
            base.Cleanup();
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBlobCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Blob, -1, 0, 0, true, null, null);
            VerifyColumnType("BLOB", null, null, null);
        }

        [TestMethod, TestCategory("Oracle")]
        public void TestBoolCol_When_Oracle()
        {
            CreateTable(ColumnTypeName.Bool, 0, 1, 0, false, "0", null);
            VerifyColumnType("NUMBER", null, 1, 0); // Oracle will report data_length as 4000 (strange, but probably since up to 4000 bytes can be stored in-line in the tablespace).
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