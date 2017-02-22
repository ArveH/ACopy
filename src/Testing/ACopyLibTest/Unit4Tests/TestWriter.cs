using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using ACopyLib.Writer;
using ACopyTestHelper;
using ADatabase;
using FluentAssertions;

namespace ACopyLibTest.Unit4Tests
{
    public abstract class TestWriter
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;
        protected string ConversionFileForWrite;
        protected string ConversionFileForRead;

        private const string Directory = @".\";
        private const string SchemaFile = "testwriter.aschema";
        private const string DataFile = "testwriter.adata";
        protected const string TestTable = "testwriter";
        private IAWriter writer; 

        #region Setup and Cleanup

        public virtual void Setup()
        {
            writer = AWriterFactory.CreateInstance(DbContext);
            writer.Directory = Directory;
        }

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
            WriteSimpleTable();
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
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("0,1,'O''Line',");
        }

        //TestMethod
        protected void TestWriter_When_StringContainsNewLine()
        {
            CreateTestTableForDifferentStrings("O\nLine\n");
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("0,1,'O\nLine\n',");
        }

        //TestMethod
        protected void TestWriter_When_UseCompression()
        {
            CreateTestTableForDifferentStrings("Hope it gets compressed");
            writer.UseCompression = true;
            writer.Write(new List<string> { TestTable });

            GetLineFromCompressedFile(writer.Directory + DataFile + DataFileWriter.CompressionFileEnding).Should().Be("0,1,'Hope it gets compressed',");
        }

        //TestMethod
        protected void TestWriter_When_BlobTableAndCompressedFlag(string blobValue)
        {
            CreateTestableWithBlob(TestTable, blobValue);
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
            writer.UseCompression = false;
            writer.Write(new List<string> { TestTable });

            GetLine(writer.Directory + DataFile).Should().Be("'A',");
        }
        #endregion

        #region Private helper methods
        private void WriteSimpleTable()
        {
            CreateSimpleTestTable();
            writer.Write(new List<string> { TestTable });
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
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "seq_no", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Varchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS") 
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
                columnFactory.CreateInstance(ColumnTypeName.Char, "char_col", 20, false, "' '", "Danish_Norwegian_CI_AS")
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
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "seq_no", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Blob, "val", true, "") 
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
                columnFactory.CreateInstance(ColumnTypeName.Bool, "bool_col", 0, 20, 0, false, false, "0", ""),
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
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            string stmt =
                $"insert into {testTable} (bool_col, char_col, date_col, float_col, guid_col, int_col, int8_col, int16_col, int64_col, longtext_col, money_col, blob_col, nvarchar_col, varchar_col) ";
            if (DbContext.DbType == DbTypeName.SqlServer)
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
                columnFactory.CreateInstance(ColumnTypeName.Int64, "id", 0, 20, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Int, "seq_no", 0, 15, 0, false, false, "0", ""),
                columnFactory.CreateInstance(ColumnTypeName.Blob, "val", true, "") 
            };
            TableDefinition tableDefinition = new TableDefinition(testTable, columns, "");
            DbSchema.CreateTable(tableDefinition);
            Commands.ExecuteNonQuery($"insert into {testTable} (id, seq_no) values (0, 1)");
        }
        #endregion
    }
}
