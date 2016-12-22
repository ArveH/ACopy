using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using ACopyLib.Writer;
using FluentAssertions;
using ACopyTestHelper;
using ADatabase;

namespace ACopyLibTest
{
    public abstract class TestWriter
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings(@"..\..\ConnectionStrings.json");
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;

        private const string Directory = @"e:\Temp\";
        private const string SchemaFile = "testwriter.aschema";
        private const string DataFile = "testwriter.adata";
        protected const string TestTable = "testwriter";

        #region Setup and Cleanup
        public abstract void Setup();

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TestTable);
            DeleteFiles();
        }

        protected void DeleteFiles()
        {
            if (File.Exists(Directory + SchemaFile))
            {
                File.Delete(Directory + SchemaFile);
            }
            if (File.Exists(Directory + DataFile))
            {
                File.Delete(Directory + DataFile);
            }
            if (System.IO.Directory.Exists(Directory + TestTable))
            {
                System.IO.Directory.Delete(Directory + TestTable, true);
            }
        }
        #endregion

        #region Tests
        //TestMethod
        protected void TestWriter_When_SimpleTable_Then_SchemaFileCreated()
        {
            IAWriter writer = WriteSimpleTable();
            File.Exists(writer.Directory + SchemaFile).Should().BeTrue();
        }

        //TestMethod
        protected void TestWriter_When_SimpleTable_Then_DataFileCreated()
        {
            WriteSimpleTable();
            CheckDataFile();
        }

        //TestMethod
        protected void TestWriter_When_BlobTable(string blobValue)
        {
            CreateTestableWithBlob(TestTable, blobValue);
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });

            File.Exists(writer.Directory + DataFile).Should().BeTrue("because data was written");
            string blobDirectory = $@"{writer.Directory}\{TestTable}\";
            System.IO.Directory.Exists(blobDirectory).Should().BeTrue("because table has blob column");
            string blobFile = $@"{blobDirectory}i000000000000000.raw";
            File.Exists(blobFile).Should().BeTrue("because table has blob values");
        }

        //TestMethod
        protected void TestWriter_When_AllTypes()
        {
            CreateTestableWithAllTypes(TestTable);

            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });

            File.Exists(writer.Directory + DataFile).Should().BeTrue("because data was written");

            string data = GetLine(writer.Directory + DataFile);

            const string expected = "1,'NO',19000223 00:00:00,123.12345678,3f2504e0-4f89-11d3-9a0c-0305e82c3301,1234567890,150,12345,123456789012345,'Very long text with æøå',123.123,i000000000000000.raw,'A unicode ﺽ string','A varchar string',";
            data.Should().BeEquivalentTo(expected);
        }

        //TestMethod
        protected void TestWriter_When_NullValue()
        {
            CreateTestableWithNull(TestTable);
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });

            File.Exists(writer.Directory + DataFile).Should().BeTrue("because data was written");

            string data = GetLine(writer.Directory + DataFile);

            const string expected = "0,1,NULL,";
            data.Should().BeEquivalentTo(expected);
        }

        //TestMethod
        protected void TestWriter_When_StringContainsQuote()
        {
            CreateTestTableForDifferentStrings("O''Line");
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("0,1,'O''Line',");
        }

        //TestMethod
        protected void TestWriter_When_StringContainsNewLine()
        {
            CreateTestTableForDifferentStrings("O\nLine\n");
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("0,1,'O\nLine\n',");
        }

        //TestMethod
        protected void TestWriter_When_UseCompression()
        {
            CreateTestTableForDifferentStrings("Hope it gets compressed");
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.UseCompression = true;
            writer.Write(new List<string> { TestTable });

            GetLineFromCompressedFile(writer.Directory + DataFile + DataFileWriter.CompressionFileEnding).Should().Be("0,1,'Hope it gets compressed',");
        }

        //TestMethod
        protected void TestWriter_When_BlobTableAndCompressedFlag(string blobValue)
        {
            CreateTestableWithBlob(TestTable, blobValue);
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.UseCompression = true;
            writer.Write(new List<string> { TestTable });

            File.Exists(writer.Directory + DataFile + ".dz").Should().BeTrue("because data was written");
            string blobDirectory = $@"{writer.Directory}\{TestTable}\";
            System.IO.Directory.Exists(blobDirectory).Should().BeTrue("because table has blob column");
            string blobFile = $@"{blobDirectory}i000000000000000.raw.dz";
            File.Exists(blobFile).Should().BeTrue("because blob value should be compressed");
        }

        //TestMethod
        protected void TestWriter_When_CharColAndTrailingSpaces_Then_NoTrailingSpacesInDataFile()
        {
            CreateTestableWithCharCol(TestTable);
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.UseCompression = false;
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("'A',");
        }
        #endregion

        #region Private helper methods
        private IAWriter WriteSimpleTable()
        {
            CreateSimpleTestTable();
            IAWriter writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
            writer.Write(new List<string> { TestTable });
            return writer;
        }

        private string GetLine(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string tmp = "";
                do
                {
                    tmp += (char)reader.Read();
                } while (!reader.EndOfStream);
                return tmp.TrimEnd();
            }
        }

        private string GetLineFromCompressedFile(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                using (DeflateStream compressionStream = new DeflateStream(reader.BaseStream, CompressionMode.Decompress, true))
                {
                    byte[] bytes = new byte[100000];
                    int count = compressionStream.Read(bytes, 0, 100000);
                    return Encoding.UTF8.GetString(bytes, 0, count).TrimEnd();
                }
            }
        }

        private void CheckDataFile()
        {
            File.Exists(Directory + DataFile).Should().BeTrue();

            using (StreamReader file = File.OpenText(Directory + DataFile))
            {
                file.ReadLine().Should().Be("0,1,'Line 1',");
            }
        }
        #endregion

        #region Methods for creating a test table

        private void CreateTable()
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int, "seq_no", false, "0"),
                columnFactory.CreateInstance(ColumnType.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
            };
            TableDefinition tableDefinition = new TableDefinition(TestTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
        }

        private void CreateSimpleTestTable()
        {
            CreateTestTableForDifferentStrings("Line 1");
        }

        private void CreateTestTableForDifferentStrings(string stringValue)
        {
            CreateTable();
            Commands.ExecuteNonQuery($"insert into {TestTable} (id, seq_no, val) values (0, 1, '{stringValue}')");
        }

        private void CreateTestableWithCharCol(string testTable)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Char, "char_col", 20, false, "' '", "Danish_Norwegian_CI_AS")
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string stmt = $"insert into {testTable} (char_col) values ('A  ')";
            Commands.ExecuteNonQuery(stmt);
        }

        private void CreateTestableWithBlob(string testTable, string blobValue)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int, "seq_no", false, "0"),
                columnFactory.CreateInstance(ColumnType.Raw, "val", true, "") 
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            Commands.ExecuteNonQuery($"insert into {testTable} (id, seq_no, val) values (0, 1, {blobValue})");
        }

        private void CreateTestableWithAllTypes(string testTable)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Bool, "bool_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.DateTime, "date_col", false, "convert(datetime,'19000101',112)"),
                columnFactory.CreateInstance(ColumnType.Float, "float_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Guid, "guid_col", true, ""),
                columnFactory.CreateInstance(ColumnType.Int, "int_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int8, "int8_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int16, "int16_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int64, "int64_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.Money, "money_col", false, "0"),
                columnFactory.CreateInstance(ColumnType.Raw, "raw_col", true, ""),
                columnFactory.CreateInstance(ColumnType.String, "string_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                columnFactory.CreateInstance(ColumnType.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string stmt =
                $"insert into {testTable} (bool_col, char_col, date_col, float_col, guid_col, int_col, int8_col, int16_col, int64_col, longtext_col, money_col, raw_col, string_col, varchar_col) ";
            if (DbContext.DbType == DbType.SqlServer)
            {
                stmt += "values (1,'NO', 'Feb 23 1900', 123.12345678, '3f2504e0-4f89-11d3-9a0c-0305e82c3301', 1234567890, 150, 12345, 123456789012345, N'Very long text with æøå', 123.123, convert(varbinary, 'Lots of bytes'), N'A unicode ﺽ string', 'A varchar string')";
            }
            else
            {
                stmt += "values (1,'NO', to_date('Feb 23 1900', 'Mon DD YYYY'), 123.12345678, hextoraw('3f2504e04f8911d39a0c0305e82c3301'), 1234567890, 150, 12345, 123456789012345, 'Very long text with æøå', 123.123, utl_raw.cast_to_raw('Lots of bytes'), 'A unicode ﺽ string', 'A varchar string')";
            }
            Commands.ExecuteNonQuery(stmt);
        }

        private void CreateTestableWithNull(string testTable)
        {
            IColumnFactory columnFactory = DbContext.PowerPlant.CreateColumnFactory();
            List<IColumn> columns = new List<IColumn>
            { 
                columnFactory.CreateInstance(ColumnType.Int64, "id", false, "0"),
                columnFactory.CreateInstance(ColumnType.Int, "seq_no", false, "0"),
                columnFactory.CreateInstance(ColumnType.Raw, "val", true, "") 
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            Commands.ExecuteNonQuery($"insert into {testTable} (id, seq_no) values (0, 1)");
        }
        #endregion
    }
}
