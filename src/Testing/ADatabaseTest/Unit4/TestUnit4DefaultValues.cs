using ADatabase;
using FluentAssertions;

namespace ADatabaseTest
{
    public abstract class TestUnit4DefaultValues: TestUnit4ColumnTypesBase
    {
        protected void TestDateTimeDefauleValue(string defValue)
        {
            TestColumn(ColumnTypeName.DateTime, 0, 0, 0, false, defValue, "");
        }

        public void VerifyTableDefinitionDefaultValue(string expected)
        {
            var columnTypeConverter = DbContext.PowerPlant.CreateColumnTypeConverter(ConversionFileForWrite);
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