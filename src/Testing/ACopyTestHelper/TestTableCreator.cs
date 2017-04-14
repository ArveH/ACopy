using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ADatabase;

namespace ACopyTestHelper
{
    public static class TestTableCreator
    {
        public static void CreateTestTableWithAllTypes(IDbContext dbContext, string tableName, bool addDepercatedTypes)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.BinaryDouble, "binarydouble_col", 0, 0, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.BinaryFloat, "binaryfloat_col", 0, 0, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Blob, "blob_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Bool, "bool_col", 0, 1, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Date, "date_col", false, "MIN_DATE"),
                columnFactory.CreateInstance(ColumnTypeName.DateTime, "datetime_col", false, "MIN_DATE"),
                columnFactory.CreateInstance(ColumnTypeName.Dec, "dec_col", 0, 8, 5, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Float, "float_col", 0, 0, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Float, "float47_col", 47, 0, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "int_col", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int16, "int16_col", 0, 5, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int64, "int64_col", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int8, "int8_col", 0, 3, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Money, "money_col", 0, 19, 4, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.NChar, "nchar_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.NLongText, "nlongtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "nvarchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Raw, "raw_col", 1000, 0, 0, true, false, "", ""),
                columnFactory.CreateInstance(ColumnTypeName.SmallDateTime, "smalldatetime_col", false, "MIN_DATE"),
                columnFactory.CreateInstance(ColumnTypeName.SmallMoney, "smallmoney_col", 0, 10, 4, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Time, "time_col", false, "MIN_DATE"),
                columnFactory.CreateInstance(ColumnTypeName.Timestamp, "timestamp_col", false, ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            if (addDepercatedTypes)
            {
                columns.Add(columnFactory.CreateInstance(ColumnTypeName.OldBlob, "oldblob_col", true, ""));
                columns.Add(columnFactory.CreateInstance(ColumnTypeName.OldText, "oldtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"));
            }
            var tableDefinition = new TableDefinition(tableName, columns, "");
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.CreateTable(tableDefinition);
            var stmt = new StringBuilder();
            stmt.Append($"insert into {tableName} (");
            stmt.Append("binarydouble_col, ");
            stmt.Append("binaryfloat_col, ");
            stmt.Append("blob_col, ");
            stmt.Append("bool_col, ");
            stmt.Append("char_col, ");
            stmt.Append("date_col, ");
            stmt.Append("datetime_col, ");
            stmt.Append("dec_col, ");
            stmt.Append("float_col, ");
            stmt.Append("float47_col, ");
            stmt.Append("guid_col, ");
            stmt.Append("int_col, ");
            stmt.Append("int16_col, ");
            stmt.Append("int64_col, ");
            stmt.Append("int8_col, ");
            stmt.Append("longtext_col, ");
            stmt.Append("money_col, ");
            stmt.Append("nchar_col, ");
            stmt.Append("nlongtext_col, ");
            stmt.Append("nvarchar_col, ");
            stmt.Append("raw_col, ");
            stmt.Append("smalldatetime_col, ");
            stmt.Append("smallmoney_col, ");
            stmt.Append("time_col, ");
            stmt.Append("timestamp_col, ");
            stmt.Append("varchar_col");
            if (addDepercatedTypes)
            {
                stmt.Append(", oldblob_col, ");
                stmt.Append("oldtext_col ");
            }
            stmt.Append(")");

            stmt.Append("values (");

            stmt.Append(GetVinaryDoubleSqlValue());
            stmt.Append(GetBinaryFloatSqlValue());
            stmt.Append(GetBlobSqlValue(dbContext));
            stmt.Append(GetBoolSqlValue());
            stmt.Append(GetCharSqlValue());
            stmt.Append(GetDateSqlValue(dbContext));
            stmt.Append(GetDateTimeSqlValue(dbContext));
            stmt.Append(GetDecSqlValue());
            stmt.Append(GetFloatSqlValue());
            stmt.Append(GetFloat47SqlValue());
            stmt.Append(GetGuidSqlValue(dbContext));
            stmt.Append(GetIntSqlValue());
            stmt.Append(GetInt16SqlValue());
            stmt.Append(GetInt64SqlValue());
            stmt.Append(GetInt8SqlValue());
            stmt.Append(GetLongTextSqlValue());
            stmt.Append(GetMoneySqlValue());
            stmt.Append(GetNCharSqlValue());
            stmt.Append(GetNLongTextSqlValue());
            stmt.Append(GetNVarcharSqlValue());
            stmt.Append(GetRawSqlValue(dbContext));
            stmt.Append(GetSmallDateTimeSqlValue(dbContext));
            stmt.Append(GetSmallMoneySqlValue());
            stmt.Append(GetTimeSqlValue(dbContext));
            stmt.Append(GetTimeStampSqlValue(dbContext));
            stmt.Append(GetVarcharSqlValue());

            stmt.Append($"{BinaryDoubleValue:F15}, ");
            stmt.Append($"{BinaryFloatValue:F10}, ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"convert(varbinary, '{BlobValue}'), " : $"utl_raw.cast_to_raw('{BlobValue}'), ");
            stmt.Append(BoolValue ? "1, ": "0, ");
            stmt.Append($"'{CharValue}', ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{DateValue:MMM dd yyyy}', " : $"to_date('{DateValue:MMM dd yyyy}', 'Mon DD YYYY'), ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{DateTimeValue:MMM dd yyyy HH:mm:ss}', " : $"to_date('{DateTimeValue:MMM dd yyyy HH:mm:ss}', 'Mon DD YYYY HH24:MI:SS'), ");
            stmt.Append($"{DecValue:###.#####}, ");
            stmt.Append($"{FloatValue:F15}, ");
            stmt.Append($"{Float47Value:F15}, ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{GuidValue}', " : $"hextoraw('{GuidValue.ToString().Replace("-","")}'), ");
            stmt.Append($"{IntValue}, ");
            stmt.Append($"{Int16Value}, ");
            stmt.Append($"{Int64Value}, ");
            stmt.Append($"{Int8Value}, ");
            stmt.Append($"'{LongTextValue}', ");
            stmt.Append($"{MoneyValue}, ");
            stmt.Append($"'{NCharValue}', ");
            stmt.Append($"'{NLongTextValue}', ");
            stmt.Append($"'{NVarcharValue}', ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"convert(varbinary, '{RawValue}'), " : $"utl_raw.cast_to_raw('{RawValue}'), ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{SmallDateTimeValue:MMM dd yyyy}', " : $"to_date('{SmallDateTimeValue:MMM dd yyyy}', 'Mon DD YYYY'), ");
            stmt.Append($"{SmallMoneyValue:F3}, ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{TimeValue:HH:mm:ss}', " : $"to_date('{TimeValue:HH:mm:ss}', 'HH24:MI:SS'), ");
            stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"'{TimeStampValue:MMM dd yyyy HH:mm:ss.fffffff}', " : $"to_timestamp('{TimeStampValue:MMM dd yyyy HH:mm:ss.fffffff}', 'Mon DD YYYY HH24:MI:SS.FF'), ");
            stmt.Append($"'{VarcharValue}'");
            if (addDepercatedTypes)
            {
                stmt.Append(", ");
                stmt.Append(dbContext.DbType == DbTypeName.SqlServer ? $"convert(image, '{BlobValue}'), " : $"utl_raw.cast_to_raw('{BlobValue}'), ");
                stmt.Append($"'{LongTextValue}'");
            }
            stmt.Append(")");

            var commands = dbContext.PowerPlant.CreateCommands();
            commands.ExecuteNonQuery(stmt.ToString());
        }

        public static double BinaryDoubleValue { get; } = 1.01234567890123;
        public static double BinaryFloatValue { get; } = 1.0123456;
        public static string BlobValue { get; } = "Lots of bytes";
        public static bool BoolValue { get; } = true;
        public static string CharValue { get; } = "MO";
        public static DateTime DateValue { get; } = new DateTime(1900, 2, 23);
        public static DateTime DateTimeValue { get; } = new DateTime(1900, 2, 23, 11, 12, 13);
        public static decimal DecValue { get; } = 123.12345m;
        public static decimal FloatValue { get; } = 1234567890.012345678901234m;
        public static decimal Float47Value { get; } = 1234567890.012345m;
        public static Guid GuidValue { get; } = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301");
        public static int IntValue { get; } = 1234567890;
        public static int Int16Value { get; } = 12345;
        public static long Int64Value { get; } = 123456789012345;
        public static int Int8Value { get; } = 150;
        public static string LongTextValue { get; } = "Very long text with æøå";
        public static decimal MoneyValue { get; } = 123.123m;
        public static string NCharValue { get; } = "ﺽ";
        public static string NLongTextValue { get; } = "Very long unicode text with ﺽ æøå";
        public static string NVarcharValue { get; } = "A unicode varchar ﺽ string";
        public static string RawValue { get; } = "Raw bytes";
        public static DateTime SmallDateTimeValue { get; } = new DateTime(1900, 2, 23);
        public static decimal SmallMoneyValue { get; } = 123.123m;
        public static TimeSpan TimeValue { get; } = new TimeSpan(0, 11, 9, 13, 123);
        public static DateTime TimeStampValue { get; } = DateTime.Parse("Feb 23 1900 11:12:13.12345678");
        public static string VarcharValue { get; } = "A varchar string";

        public static string GetVinaryDoubleSqlValue() { return $"{BinaryDoubleValue:F15}"; }
        public static string GetBinaryFloatSqlValue() { return $"{BinaryFloatValue.ToString("###.#####", CultureInfo.InvariantCulture)}"; }
        public static string GetBlobSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"convert(varbinary, '{BlobValue}')" : $"utl_raw.cast_to_raw('{BlobValue}')"; }
        public static string GetBoolSqlValue() { return BoolValue ? "1": "0"; }
        public static string GetCharSqlValue() { return $"'{CharValue}'"; }
        public static string GetDateSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"'{DateValue:MMM dd yyyy}'" : $"to_date('{DateValue:MMM dd yyyy}', 'Mon DD YYYY')"; }
        public static string GetDateTimeSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"'{DateTimeValue:MMM dd yyyy HH:mm:ss}'" : $"to_date('{DateTimeValue:MMM dd yyyy HH:mm:ss}', 'Mon DD YYYY HH24:MI:SS')"; }
        public static string GetDecSqlValue() { return $"{DecValue.ToString("###.#####", CultureInfo.InvariantCulture)}"; }
        public static string GetFloatSqlValue() { return $"{FloatValue:F15}"; }
        public static string GetFloat47SqlValue() { return $"{Float47Value:F15}"; }
        public static string GetGuidSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"'{GuidValue}'" : $"hextoraw('{GuidValue.ToString().Replace("-","")}')"; }
        public static string GetIntSqlValue() { return $"{IntValue}"; }
        public static string GetInt16SqlValue() { return $"{Int16Value.ToString(CultureInfo.InvariantCulture)}"; }
        public static string GetInt64SqlValue() { return $"{Int64Value}"; }
        public static string GetInt8SqlValue() { return $"{Int8Value}"; }
        public static string GetLongTextSqlValue() { return $"'{LongTextValue}'"; }
        public static string GetMoneySqlValue() { return $"{MoneyValue.ToString(CultureInfo.InvariantCulture)}"; }
        public static string GetNCharSqlValue() { return $"N'{NCharValue}'"; }
        public static string GetNLongTextSqlValue() { return $"N'{NLongTextValue}'"; }
        public static string GetNVarcharSqlValue() { return $"N'{NVarcharValue}'"; }
        public static string GetRawSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"convert(binary, '{RawValue}')" : $"utl_raw.cast_to_raw('{RawValue}')"; }
        public static string GetSmallDateTimeSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"'{SmallDateTimeValue:MMM dd yyyy}'" : $"to_date('{SmallDateTimeValue:MMM dd yyyy}', 'Mon DD YYYY')"; }
        public static string GetSmallMoneySqlValue() { return $"{SmallMoneyValue.ToString("F3", CultureInfo.InvariantCulture)}"; }
        public static string GetTimeSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"convert(time,'{TimeValue:c}')" : $"to_date('{TimeValue:HH:mm:ss}', 'HH24:MI:SS')"; }
        public static string GetTimeStampSqlValue(IDbContext dbContext) { return dbContext.DbType == DbTypeName.SqlServer? $"'{TimeStampValue:MMM dd yyyy HH:mm:ss.fffffff}'" : $"to_timestamp('{TimeStampValue:MMM dd yyyy HH:mm:ss.fffffff}', 'Mon DD YYYY HH24:MI:SS.FF')"; }
        public static string GetVarcharSqlValue() { return $"'{VarcharValue}'"; }

    public static void CreateUnit4TestableWithAllTypes(IDbContext dbContext, string tableName)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.Bool, "bool_col", 0, 1, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.DateTime, "date_col", false, "convert(datetime,'19000101',112)"),
                columnFactory.CreateInstance(ColumnTypeName.Float, "float_col", 0, 30, 8, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "int_col", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int8, "int8_col", 0, 3, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int16, "int16_col", 0, 5, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int64, "int64_col", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Money, "money_col", 0, 30, 3, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Blob, "blob_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "nvarchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            var tableDefinition = new TableDefinition(tableName, columns, "");
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.CreateTable(tableDefinition);
            var stmt = $"insert into {tableName} (bool_col, char_col, date_col, float_col, guid_col, int_col, int8_col, int16_col, int64_col, longtext_col, money_col, blob_col, nvarchar_col, varchar_col) ";
            if (dbContext.DbType == DbTypeName.SqlServer)
            {
                stmt += "values (1,'NO', 'Feb 23 1900', 123.12345678, '3f2504e0-4f89-11d3-9a0c-0305e82c3301', 1234567890, 150, 12345, 123456789012345, N'Very long text with æøå', 123.123, convert(varbinary, 'Lots of bytes'), N'A unicode ﺽ string', 'A varchar string')";
            }
            else
            {
                stmt += "values (1,'NO', to_date('Feb 23 1900', 'Mon DD YYYY'), 123.12345678, hextoraw('3f2504e04f8911d39a0c0305e82c3301'), 1234567890, 150, 12345, 123456789012345, 'Very long text with æøå', 123.123, utl_raw.cast_to_raw('Lots of bytes'), 'A unicode ﺽ string', 'A varchar string')";
            }
            var commands = dbContext.PowerPlant.CreateCommands();
            commands.ExecuteNonQuery(stmt);
        }

        public static void CreateIndexTables(IDbContext dbContext, string asysIndex, string aagIndex)
        {
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.DropTable(asysIndex);
            dbSchema.DropTable(aagIndex);
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "index_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "table_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "column_list", 510, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "location_name", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Int8, "unique_flag", 0, 3, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.NVarchar, "db_name", 20, false, "' '", "Danish_Norwegian_CI_AS")
            };
            dbSchema.CreateTable(new TableDefinition(aagIndex, columns, ""));
            dbSchema.CreateTable(new TableDefinition(asysIndex, columns, ""));
        }

        public static void CreateTestTableWithIndex(IDbContext dbContext, string tableName)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id2", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            var tableDefinition = new TableDefinition(tableName, columns, "");
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.CreateTable(tableDefinition);
            string tmp = $"insert into {tableName} (id, val) values (9, 'control value')";
            var commands = dbContext.PowerPlant.CreateCommands();
            commands.ExecuteNonQuery(tmp);
            commands.ExecuteNonQuery($"create unique index {"i_" + tableName} on {tableName}(id)");
        }

        public static TableDefinition CreateTableSomeColumnsAndOneRow(IDbContext dbContext, string tableName)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "flag", 1, false, "' '", ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "val", 50, false, "' '", "")
            };
            var tableDefinition = new TableDefinition(tableName, columns, "");
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.CreateTable(tableDefinition);
            var commands = dbContext.PowerPlant.CreateCommands();
            commands.ExecuteNonQuery($"insert into {tableName} (id, flag, val) values (1, 'A', 'Some value')");
            return tableDefinition;
        }
    }
}