using System.IO;
using System.Text;

namespace ACopyLib.Reader
{
    public class DataFileUncompressedReader: DataFileReader
    {
        private readonly StreamReader _stream;

        public override bool IsCompressed
        {
            get { return false; }
        }

        public DataFileUncompressedReader(string fileName): base(fileName)
        {
            _stream = new StreamReader(fileName, new UTF8Encoding(false));
            CurrentChar = Read();
        }

        protected override int Peek()
        {
            return _stream.Peek();
        }

        protected override sealed int Read()
        {
            return _stream.Read();
        }

        protected override string ReadUnquotedData()
        {
            string data = "";
            while (CurrentChar >= 0 && (char)CurrentChar != ',')
            {
                data += (char)CurrentChar;
                CurrentChar = Read();
            }

            return data;
        }

        protected override string ReadInsideQuotes()
        {
            StringBuilder data = new StringBuilder(4000);
            while (CurrentChar >= 0)
            {
                if (CurrentChar == QuoteChar)
                {
                    if (Peek() == QuoteChar)
                    {
                        Read();
                    }
                    else
                    {
                        break;
                    }
                }
                data.Append((char)CurrentChar);
                CurrentChar = Read();
            }
            return data.ToString();
        }

        public override void Dispose()
        {
            _stream.Dispose();
        }
    }
}
