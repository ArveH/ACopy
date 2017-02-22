using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using ACopyLib.Exceptions;
using ACopyLib.Reader;
using ADatabase;
using ADatabase.SqlServer;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACopyLibTest.Unit4Tests
{
    [TestClass]
    public class TestDataFileReader
    {
        private IColumnFactory _columnFactory;
        private IDataFileReader _fileReader;
        private string _dataFileName;

        [TestInitialize]
        public void Setup()
        {
            _columnFactory = new SqlServerColumnFactory();
            _dataFileName = "testdatafile.adata";
        }

        [TestCleanup]
        public void Cleanup()
        {

        }

        private void CreateFile(string data)
        {
            using (StreamWriter stream = new StreamWriter(File.Open(_dataFileName, FileMode.Create), new UTF8Encoding(false)))
            {
                stream.Write(data);
            }
        }

        private List<string> ReadIt(List<IColumn> columns)
        {
            List<string> data;
            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                data = _fileReader.ReadLine(columns);
            }
            return data;
        }

        private List<IColumn> CreateTwoColumns()
        {
            List<IColumn> columns = new List<IColumn>
            { 
                _columnFactory.CreateInstance(ColumnTypeName.NVarchar, "val", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                _columnFactory.CreateInstance(ColumnTypeName.Int64, "id", false, "0")
            };
            return columns;
        }

        [TestMethod]
        public void TestOneLine_When_IntAndVarchar()
        {
            CreateFile("'test',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal("test", "1");
        }

        [TestMethod]
        [ExpectedException(typeof(NotValidDataException))]
        public void TestOneLine_When_MissingComma_Then_Exception()
        {
            CreateFile("'test',1\n");
            List<IColumn> columns = CreateTwoColumns();

            ReadIt(columns);
        }

        [TestMethod]
        [ExpectedException(typeof(NotValidDataException))]
        public void TestOneLine_When_MissingStartQuote_Then_Exception()
        {
            CreateFile("test',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            ReadIt(columns);
        }

        [TestMethod]
        [ExpectedException(typeof(NotValidDataException))]
        public void TestOneLine_When_MissingEndQuote_Then_Exception()
        {
            CreateFile("'test,1,\n");
            List<IColumn> columns = CreateTwoColumns();

            ReadIt(columns);
        }

        [TestMethod]
        [ExpectedException(typeof(NotValidDataException))]
        public void TestOneLine_When_StringContainsQuote_Then_Exception()
        {
            CreateFile("'t'est',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            ReadIt(columns);
        }

        [TestMethod]
        public void TestOneLine_When_StringContainsEscapedQuote_Then_Ok()
        {
            CreateFile("'t''est',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal("t'est", "1");
        }

        [TestMethod]
        public void TestOneLine_When_StringContainsLeadingEscapedQuote_Then_Ok()
        {
            CreateFile("'''test',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal("'test", "1");
        }

        [TestMethod]
        public void TestOneLine_When_StringContainsDoubleEscapedQuote_Then_Ok()
        {
            CreateFile("'''''test',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal("''test", "1");
        }

        [TestMethod]
        public void TestOneLine_When_StringContainsNewLine_Then_Ok()
        {
            CreateFile("'te\nst',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal("te\nst", "1");
        }

        private List<IColumn> CreateAllColumns()
        {
            return new List<IColumn>
            { 
                _columnFactory.CreateInstance(ColumnTypeName.Bool, "bool_col", 0, 1, 0, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Char, "char_col", 2, false, "' '", "Danish_Norwegian_CI_AS"),
                _columnFactory.CreateInstance(ColumnTypeName.DateTime, "date_col", false, "convert(datetime,'19000101',112)"),
                _columnFactory.CreateInstance(ColumnTypeName.Float, "float_col", 0, 30, 8, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Guid, "guid_col", true, ""),
                _columnFactory.CreateInstance(ColumnTypeName.Int, "int_col", 0, 15, 0, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Int8, "int8_col", 0, 3, 0, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Int16, "int16_col", 0, 5, 0, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Int64, "int64_col", 0, 20, 0, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.LongText, "longtext_col", 0, false, "' '", "Danish_Norwegian_CI_AS"),
                _columnFactory.CreateInstance(ColumnTypeName.Money, "money_col", 0, 30, 3, false, false, "0", ""),
                _columnFactory.CreateInstance(ColumnTypeName.Blob, "blob_col", true, ""),
                _columnFactory.CreateInstance(ColumnTypeName.NVarchar, "nvarchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS"),
                _columnFactory.CreateInstance(ColumnTypeName.Varchar, "varchar_col", 50, false, "' '", "Danish_Norwegian_CI_AS")
            };

        }

        [TestMethod]
        public void TestOneLine_When_AllColumTypes()
        {
            CreateFile("1,'NO',19000223 00:00:00,123.12345678,3f2504e0-4f89-11d3-9a0c-0305e82c3301,1234567890,150,12345,123456789012345,'Very long text with æøå',123.123,i000000000000000.raw,'A unicode ﺽ string','A varchar string',\n");
            List<IColumn> columns = CreateAllColumns();

            List<string> data;
            data = ReadIt(columns);
            data.Should().Equal(
                "1",
                "NO",
                "19000223 00:00:00",
                "123.12345678",
                "3f2504e0-4f89-11d3-9a0c-0305e82c3301",
                "1234567890",
                "150",
                "12345",
                "123456789012345",
                "Very long text with æøå",
                "123.123",
                "i000000000000000.raw",
                "A unicode ﺽ string",
                "A varchar string");
        }

        [TestMethod]
        public void TestTwoLines_When_StringAndInt()
        {
            CreateFile("'A unicode ﺽ string',1,\n'test',2,\n");
            List<IColumn> columns = CreateTwoColumns();

            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                var data = _fileReader.ReadLine(columns);
                data.Should().Equal("A unicode ﺽ string", "1");
                data = _fileReader.ReadLine(columns);
                data.Should().Equal("test", "2");
            }
        }

        [TestMethod]
        public void TestReadLine_When_NullValue()
        {
            CreateFile("NULL,1,\n");
            List<IColumn> columns = CreateTwoColumns();

            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                var data = _fileReader.ReadLine(columns);
                data.Should().Equal(null, "1");
            }
        }

        [TestMethod]
        public void TestReadLine_When_StringIsNULL()
        {
            CreateFile("'NULL',1,\n");
            List<IColumn> columns = CreateTwoColumns();

            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                var data = _fileReader.ReadLine(columns);
                data.Should().Equal("NULL", "1");
            }
        }

        private void CreateCompressedFile(string sampleString)
        {
            string line = string.Format("'{0}',1,\n", sampleString);
            using (FileStream compressedFileStream = File.Create(_dataFileName))
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(line);
                    compressionStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        [TestMethod]
        public void TestCompressedFile_With_æøå()
        {
            _dataFileName += ".dz";
            const string sampleString = "abæøåcø";
            CreateCompressedFile(sampleString);
            List<IColumn> columns = CreateTwoColumns();

            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                var val = _fileReader.ReadLine(columns);
                val[0].Should().Be(sampleString);
            }
        }

        [TestMethod]
        public void TestCompressedFile_With_HighUnicodeValue()
        {
            _dataFileName += ".dz";
            const string sampleString = "abﺽ cﺽ";
            CreateCompressedFile(sampleString);
            List<IColumn> columns = CreateTwoColumns();

            using (_fileReader = DataFileReaderFactory.CreateInstance(_dataFileName))
            {
                var val = _fileReader.ReadLine(columns);
                val[0].Should().Be(sampleString);
            }
        }
    }
}
