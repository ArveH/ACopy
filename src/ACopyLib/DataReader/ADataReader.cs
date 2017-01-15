using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using ACopyLib.Exceptions;
using ACopyLib.Reader;
using ACopyLib.Utils;
using ACopyLib.Writer;
using ADatabase;

namespace ACopyLib.DataReader
{
    public class ADataReader : IDataReader
    {
        private readonly ITableDefinition _tableDefinition;
        private List<string> _columnValues;
        private readonly string _rawFileDirectory;
        private readonly long _largeBlobSize;
        private readonly string _fileName;

        public ADataReader(string fileName, ITableDefinition tableDefinition, long largeBlobSize)
        {
            _tableDefinition = tableDefinition;
            _fileName = fileName;
            _dataFileReader = DataFileReaderFactory.CreateInstance(fileName);
            _largeBlobSize = largeBlobSize;
            _rawFileDirectory = Path.GetDirectoryName(fileName) + "\\" + _tableDefinition.Name + "\\";

        }

        private IDataFileReader _dataFileReader;
        public IDataFileReader DataFileReader
        {
            get 
            { 
                return _dataFileReader ?? DataFileReaderFactory.CreateInstance(_fileName); 
            }
            set { _dataFileReader = value; }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int Depth
        {
            get { throw new NotImplementedException(); }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { throw new NotImplementedException(); }
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        public bool Read()
        {
            if (DataFileReader.IsEndOfFile)
            {
                return false;
            }
            _columnValues = DataFileReader.ReadLine(_tableDefinition.Columns);
            return true;
        }

        public int RecordsAffected => (int)DataFileReader.RowCounter;

        public void Dispose()
        {
            DataFileReader.Dispose();
        }

        public int FieldCount => _tableDefinition.Columns.Count;

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            return _tableDefinition.Columns[i].GetDotNetType();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            if (_tableDefinition.Columns[i].Type == ColumnTypeName.Raw)
            {
                if (_columnValues[i] == null)
                {
                    return DBNull.Value;
                }
                return ConvertFileToBytes(_columnValues[i]);
            }
            //object tmp = _tableDefinition.Columns[i].ToInternalType(_columnValues[i]);
            return _tableDefinition.Columns[i].ToInternalType(_columnValues[i]);
        }

        private object ConvertFileToBytes(string fileName)
        {
            string fullPath = _rawFileDirectory + fileName;
            bool isCompressed = false;
            if (File.Exists(fullPath + DataFileWriter.CompressionFileEnding))
            {
                fullPath += DataFileWriter.CompressionFileEnding;
                isCompressed = true;
            }

            FileInfo fi = new FileInfo(fullPath);
            if (fi.Length >= _largeBlobSize)
            {
                return new BlobDataException($"Blob file ({fi.Length} bytes) larger than limit {_largeBlobSize}");
            }

            if (isCompressed)
            {
                return FileHelper.GetAllBytesFromCompressedFile(fullPath);
            }
            return File.ReadAllBytes(fullPath);
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            return _columnValues[i] == null;
        }

        public object this[string name]
        {
            get { throw new NotImplementedException(); }
        }

        public object this[int i]
        {
            get { throw new NotImplementedException(); }
        }
    }
}