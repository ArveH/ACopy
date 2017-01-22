using System.Collections.Generic;
using ADatabase;

namespace ACopyTestHelper
{
    public static class TestTableCreator
    {
        public static void CreateTestableWithAllTypes(IDbContext dbContext, string tableName)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.Bool, "bool_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.DateTime, "date_col", false, "convert(datetime,'19000101',112)"),
                columnFactory.CreateInstance(ColumnTypeName.Float, "float_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Guid, "guid_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "int_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Int8, "int8_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Int16, "int16_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Int64, "int64_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Money, "money_col", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Raw, "raw_col", true, ""),
                columnFactory.CreateInstance(ColumnTypeName.String, "string_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            var tableDefinition = new TableDefinition(tableName, columns, "");
            var dbSchema = dbContext.PowerPlant.CreateDbSchema();
            dbSchema.CreateTable(tableDefinition);
            var stmt = $"insert into {tableName} (bool_col, char_col, date_col, float_col, guid_col, int_col, int8_col, int16_col, int64_col, longtext_col, money_col, raw_col, string_col, varchar_col) ";
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
                columnFactory.CreateInstance(ColumnTypeName.String, "index_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.String, "table_name", 60, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.String, "column_list", 510, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.String, "location_name", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnTypeName.Int8, "unique_flag", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.String, "db_name", 20, false, "' '", "Danish_Norwegian_CI_AS")
            };
            dbSchema.CreateTable(new TableDefinition(aagIndex, columns, ""));
            dbSchema.CreateTable(new TableDefinition(asysIndex, columns, ""));
        }

        public static void CreateTestTableWithIndex(IDbContext dbContext, string tableName)
        {
            var columnFactory = dbContext.PowerPlant.CreateColumnFactory();
            var columns = new List<IColumn>
            {
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id2", false, "0"),
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
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", false, "0"),
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