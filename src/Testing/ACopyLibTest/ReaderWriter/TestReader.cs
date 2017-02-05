using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Text;
using ACopyLib.Reader;
using ACopyLib.Writer;
using FluentAssertions;
using ACopyTestHelper;
using ADatabase;

namespace ACopyLibTest
{
    public abstract class TestReader
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected ICommands Commands;

        protected const string TestGuid = "3f2504e0-4f89-11d3-9a0c-0305e82c3301";
        protected const string TestTable = "testreader";
        private const string Directory = @".\";
        protected string SchemaFile;
        protected string DataFile;

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
            if (File.Exists(Directory + DataFile + DataFileWriter.CompressionFileEnding))
            {
                File.Delete(Directory + DataFile + DataFileWriter.CompressionFileEnding);
            }
            if (System.IO.Directory.Exists(Directory + TestTable))
            {
                System.IO.Directory.Delete(Directory + TestTable, true);
            }
        }

        private void CreateTestFiles()
        {
            CreateSimpleSchemaFile(Directory + SchemaFile);
            CreateSimpleDataFile(Directory + DataFile);
        }

        private void CreateSimpleDataFile(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("0,1,'Line 1',");
            }
        }

        private void CreateSimpleCompressedFile(string fullPath)
        {
            using (FileStream compressedFileStream = File.Create(fullPath))
            {
                using (DeflateStream compressionStream = new DeflateStream(compressedFileStream, CompressionMode.Compress))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes("0,1,'Line 1',");
                    compressionStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        private void CreateSimpleSchemaFile(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<!--Table definition file for ACopy program-->");
                writer.WriteLine("<Table");
                writer.WriteLine("  Name=\"{0}\"", TestTable);
                writer.WriteLine("  Location=\"PRIMARY\">");
                writer.WriteLine("  <Columns>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"id\">");
                writer.WriteLine("      <Type>Int</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"seq_no\">");
                writer.WriteLine("      <Type>Int</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"val\">");
                writer.WriteLine("      <Type>Varchar</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>50</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("  </Columns>");
                writer.WriteLine("</Table>");
            }
        }

        private void Read()
        {
            IAReader reader = AReaderFactory.CreateInstance(DbContext);
            reader.Directory = Directory;
            int totalTables;
            int failedTables;
            reader.Read(new List<string> { TestTable }, out totalTables, out failedTables);
        }

        //TestMethod
        protected void TestReader_When_SimpleTable()
        {
            CreateTestFiles();
            Read();

            DbSchema.IsTable(TestTable).Should().BeTrue();
            var val = Commands.ExecuteScalar(string.Format("select val from {0}", TestTable));
            val.Should().Be("Line 1");
        }

        //TestMethod
        protected void TestReader_When_CompressedTable()
        {
            CreateSimpleSchemaFile(Directory + SchemaFile);
            CreateSimpleCompressedFile(Directory + DataFile + DataFileWriter.CompressionFileEnding);
            Read();

            DbSchema.IsTable(TestTable).Should().BeTrue();
            var val = Commands.ExecuteScalar(string.Format("select val from {0}", TestTable));
            val.Should().Be("Line 1");
        }

        private void CreateSchemaFileForRaw(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<!--Table definition file for ACopy program-->");
                writer.WriteLine("<Table");
                writer.WriteLine("  Name=\"{0}\"", TestTable);
                writer.WriteLine("  Location=\"PRIMARY\">");
                writer.WriteLine("  <Columns>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"id\">");
                writer.WriteLine("      <Type>Int</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"raw_col\">");
                writer.WriteLine("      <Type>Raw</Type>");
                writer.WriteLine("      <IsNullable>True</IsNullable>");
                writer.WriteLine("      <Default />");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"val\">");
                writer.WriteLine("      <Type>Varchar</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>50</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("  </Columns>");
                writer.WriteLine("</Table>");
            }
        }

        private void CreateDataFilesForRaw(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("0,i000000000000000.raw,'Line 1',");
            }

            if (!System.IO.Directory.Exists(Directory + TestTable))
            {
                System.IO.Directory.CreateDirectory(Directory + TestTable);
            }
            using (StreamWriter writer = new StreamWriter(Directory + TestTable + @"\" + "i000000000000000.raw"))
            {
                writer.Write("A long blob");
            }
        }

        //TestMethod
        protected void TestReader_When_RawColumn()
        {
            CreateSchemaFileForRaw(Directory + SchemaFile);
            CreateDataFilesForRaw(Directory + DataFile);

            Read();
            DbSchema.IsTable(TestTable).Should().BeTrue();
        }

        //TestMethod
        protected void TestReader_When_RawColumn_And_Compressed()
        {
            CreateSchemaFileForRaw(Directory + SchemaFile);
            CreateDataFilesForRaw(Directory + DataFile);
            CompressDataFile(Directory + DataFile);
            CompressDataFile(Directory + TestTable + @"\" + "i000000000000000.raw");

            Read();
            DbSchema.IsTable(TestTable).Should().BeTrue();
        }

        private void CreateSchemaFileForAllTypes(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<!--Table definition file for ACopy program-->");
                writer.WriteLine("<Table");
                writer.WriteLine("  Name=\"{0}\"", TestTable);
                writer.WriteLine("  Location=\"PRIMARY\">");
                writer.WriteLine("  <Columns>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"bool_col\">");
                writer.WriteLine("      <Type>Bool</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"char_col\">");
                writer.WriteLine("      <Type>Char</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>2</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"date_col\">");
                writer.WriteLine("      <Type>DateTime</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>convert(datetime,'19000101',112)</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"float_col\">");
                writer.WriteLine("      <Type>Float</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Prec>28</Prec>");
                writer.WriteLine("        <Scale>8</Scale>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"guid_col\">");
                writer.WriteLine("      <Type>Guid</Type>");
                writer.WriteLine("      <IsNullable>True</IsNullable>");
                writer.WriteLine("      <Default />");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"int_col\">");
                writer.WriteLine("      <Type>Int</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"int8_col\">");
                writer.WriteLine("      <Type>Int8</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"int16_col\">");
                writer.WriteLine("      <Type>Int16</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"int64_col\">");
                writer.WriteLine("      <Type>Int64</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"longtext_col\">");
                writer.WriteLine("      <Type>LongText</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>-1</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"money_col\">");
                writer.WriteLine("      <Type>Money</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>0</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Prec>28</Prec>");
                writer.WriteLine("        <Scale>3</Scale>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"raw_col\">");
                writer.WriteLine("      <Type>Raw</Type>");
                writer.WriteLine("      <IsNullable>True</IsNullable>");
                writer.WriteLine("      <Default />");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"string_col\">");
                writer.WriteLine("      <Type>String</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>50</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"varchar_col\">");
                writer.WriteLine("      <Type>Varchar</Type>");
                writer.WriteLine("      <IsNullable>False</IsNullable>");
                writer.WriteLine("      <Default>' '</Default>");
                writer.WriteLine("      <Details>");
                writer.WriteLine("        <Length>50</Length>");
                writer.WriteLine("        <Collation>Danish_Norwegian_CI_AS</Collation>");
                writer.WriteLine("      </Details>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("  </Columns>");
                writer.WriteLine("</Table>");
            }
        }

        private void CreateDataFileForAllTypes(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("1,'NO',19000223 00:00:00,123.12345678,{0},1234567890,150,12345,123456789012345,'Very long text with æøå',123.123,i000000000000000.raw,'A unicode ﺽ string','A varchar string',", TestGuid);
            }
            System.IO.Directory.CreateDirectory(Directory + TestTable);
            using (StreamWriter writer = new StreamWriter(Directory + TestTable + "\\" + "i000000000000000.raw"))
            {
                writer.Write("A long blob");
            }
        }

        private static void CompressDataFile(string fullPath)
        {
            using (FileStream inFile = new FileStream(fullPath, FileMode.Open))
            {
                using (FileStream outFile = File.Create(fullPath + ".dz"))
                {
                    using (DeflateStream compress = new DeflateStream(outFile, CompressionMode.Compress))
                    {
                        inFile.CopyTo(compress);
                    }
                }
            }

            File.Delete(fullPath);
        }

        //TestMethod
        protected void TestReader_When_AllTypes()
        {
            CreateSchemaFileForAllTypes(Directory + SchemaFile);
            CreateDataFileForAllTypes(Directory + DataFile);
            Read();
            CheckAllValues();
        }

        //TestMethod
        protected void TestReader_When_AllTypes_And_Compressed()
        {
            CreateSchemaFileForAllTypes(Directory + SchemaFile);
            CreateDataFileForAllTypes(Directory + DataFile);
            CompressDataFile(Directory + DataFile);
            CompressDataFile(Directory + TestTable + "\\" + "i000000000000000.raw");
            Read();
            CheckAllValues();
        }

        private void CreateSchemaFileForGuid(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                writer.WriteLine("<!--Table definition file for ACopy program-->");
                writer.WriteLine("<Table");
                writer.WriteLine("  Name=\"{0}\"", TestTable);
                writer.WriteLine("  Location=\"PRIMARY\">");
                writer.WriteLine("  <Columns>");
                writer.WriteLine("    <Column");
                writer.WriteLine("      Name=\"test_col\">");
                writer.WriteLine("      <Type>Guid</Type>");
                writer.WriteLine("      <IsNullable>True</IsNullable>");
                writer.WriteLine("      <Default></Default>");
                writer.WriteLine("    </Column>");
                writer.WriteLine("  </Columns>");
                writer.WriteLine("</Table>");
            }
        }

        private void CreateDataFileForGuid(string fullPath)
        {
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                writer.WriteLine("{0},", TestGuid);
            }
        }

        //TestMethod
        protected void TestReader_When_Guid()
        {
            CreateSchemaFileForGuid(Directory + SchemaFile);
            CreateDataFileForGuid(Directory + DataFile);
            Read();
        }

        private void CheckAllValues()
        {
            IDataCursor cursor = DbContext.PowerPlant.CreateDataCursor();
            var columnTypeConverter = DbContext.PowerPlant.CreateColumnTypeConverter("Resources/Unit4OracleConversions.xml");
            ITableDefinition tableDefinition = DbSchema.GetTableDefinition(columnTypeConverter, TestTable);
            try 
	        {	        
                IDataReader reader = cursor.ExecuteReader(string.Format("select * from {0}", TestTable));
		        reader.Read();

                tableDefinition.Columns[0].ToString(reader["bool_col"]).Should().Be("1");
                tableDefinition.Columns[1].ToString(reader["char_col"]).Should().Be("'NO'");
                tableDefinition.Columns[2].ToString(reader["date_col"]).Should().Be("19000223 00:00:00");
                tableDefinition.Columns[3].ToString(reader["float_col"]).Should().Be("123.12345678");
                tableDefinition.Columns[4].ToString(reader["guid_col"]).Should().Be(TestGuid);
                tableDefinition.Columns[5].ToString(reader["int_col"]).Should().Be("1234567890");
                tableDefinition.Columns[6].ToString(reader["int8_col"]).Should().Be("150");
                tableDefinition.Columns[7].ToString(reader["int16_col"]).Should().Be("12345");
                tableDefinition.Columns[8].ToString(reader["int64_col"]).Should().Be("123456789012345");
                tableDefinition.Columns[9].ToString(reader["longtext_col"]).Should().Be("'Very long text with æøå'");
                tableDefinition.Columns[10].ToString(reader["money_col"]).Should().Be("123.123");
                tableDefinition.Columns[12].ToString(reader["string_col"]).Should().Be("'A unicode ﺽ string'");
                tableDefinition.Columns[13].ToString(reader["varchar_col"]).Should().Be("'A varchar string'");

                string blob = Encoding.Default.GetString((byte[])reader["raw_col"]);
                blob.Should().Be("A long blob");
	        }
	        finally
	        {
		        cursor.Close();
	        }
        }
    }
}