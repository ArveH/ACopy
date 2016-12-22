using System;
using System.Collections.Generic;
using System.IO;
using ACopyLib.Exceptions;
using ADatabase;

namespace ACopyLib.Reader
{
    public abstract class DataFileReader: IDataFileReader
    {
        protected DataFileReader(string fileName)
        {
            FileName = fileName;
            _rowCounter = 0;
        }

        protected int CurrentChar = -1;

        private long _rowCounter;
        public long RowCounter
        {
            get { return _rowCounter; }
        }

        public string FileName { get; protected set; }

        public virtual char QuoteChar 
        {
            get { return '\''; } 
        }

        public abstract bool IsCompressed { get; }
        public abstract void Dispose();
        protected abstract int Peek();
        protected abstract int Read();
        protected abstract string ReadUnquotedData();
        protected abstract string ReadInsideQuotes();

        public List<string> ReadLine(List<IColumn> columns)
        {
            List<string> data = new List<string>(columns.Count);
            foreach (var col in columns)
            {
                data.Add(ReadDataForColumn(col));
                ReadComma(col);
            }

            ReadNewlineCharacter();
            _rowCounter++;

            if (_rowCounter % 10000 == 0)
            {
                Console.WriteLine("...... reached row {0} for {1} ......", _rowCounter, Path.GetFileNameWithoutExtension(FileName));
            }

            return data;
        }

        private string ReadDataForColumn(IColumn col)
        {
            string data;
            if ((char)CurrentChar == QuoteChar)
            {
                data = ReadQuotedData(col.Name);
            }
            else
            {
                data = ReadUnquotedData();
                CheckIfDataShouldBeQuoted(col, data);
                if (data == "NULL")
                {
                    return null;
                }
            }

            return data;
        }

        private void CheckIfDataShouldBeQuoted(IColumn col, string data)
        {
            if (data != "NULL")
            {
                switch (col.Type)
                {
                    case ColumnType.Varchar:
                    case ColumnType.Char:
                    case ColumnType.String:
                    case ColumnType.LongText:
                        throw new NotValidDataException(string.Format("Data for column '{0}' missing quote in line{1}", col.Name, _rowCounter));
                }
            }
        }


        protected string ReadQuotedData(string colName)
        {
            ReadQuote(colName);
            var data = ReadInsideQuotes();
            ReadQuote(colName);

            return data;
        }

        private void ReadQuote(string colName)
        {
            if ((char)CurrentChar != QuoteChar)
            {
                throw new NotValidDataException(string.Format("Data for column '{0}' missing quote in line {1}", colName, _rowCounter));
            }
            CurrentChar = Read();
        }

        private void ReadComma(IColumn col)
        {
            if ((char)CurrentChar != ',')
            {
                throw new NotValidDataException(string.Format("Data for column '{0}' missing field terminator in line {3}. Found '{1}' ({2})", col.Name, (char)CurrentChar, CurrentChar, _rowCounter));
            }
            CurrentChar = Read();
        }

        private void ReadNewlineCharacter()
        {
            if ((char)CurrentChar != '\n' && (char)CurrentChar != '\r' && !IsEndOfFile)
            {
                throw new NotValidDataException(string.Format("Newline not found in correct position in line {0}. Found '{1}' ({2})", _rowCounter, (char)CurrentChar, CurrentChar));
            }
            while (((char)CurrentChar == '\n' || (char)CurrentChar == '\r') && !IsEndOfFile)
            {
                CurrentChar = Read();
            }
        }

        public bool IsEndOfFile
        {
            get { return CurrentChar == -1; }
        }
    }
}