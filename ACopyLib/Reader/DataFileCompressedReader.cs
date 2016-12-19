using System.IO;
using System.IO.Compression;
using System.Text;

namespace ACopyLib.Reader
{
    public class DataFileCompressedReader: DataFileReader
    {
        private readonly BinaryReader _stream;
        private readonly DeflateStream _compressedStream;

        public override bool IsCompressed
        {
            get { return true; }
        }

        public DataFileCompressedReader(string fileName): base(fileName)
        {
            _stream = new BinaryReader(new FileStream(FileName, FileMode.Open, FileAccess.Read));
            _compressedStream = new DeflateStream(_stream.BaseStream, CompressionMode.Decompress);
            CurrentChar = Read();
        }

        private bool _hasPeeked;
        private int _peekedChar = -1;
        protected override int Peek()
        {
            if (_hasPeeked)
            {
                return _peekedChar;
            }

            _peekedChar = Read();
            _hasPeeked = true;

            return _peekedChar;
        }

        protected override sealed int Read()
        {
            if (_hasPeeked)
            {
                _hasPeeked = false;
                return _peekedChar;
            }

            return _compressedStream.ReadByte();
        }

        public override void Dispose()
        {
            _compressedStream.Dispose();
            _stream.Dispose();
        }

        protected override string ReadUnquotedData()
        {
            using (MemoryStream bytes = new MemoryStream(4000))
            {
                while (CurrentChar >= 0 && (char)CurrentChar != ',')
                {
                    bytes.WriteByte((byte)CurrentChar);
                    CurrentChar = Read();
                }

                return Encoding.UTF8.GetString(bytes.ToArray());
            }
        }

        protected override string ReadInsideQuotes()
        {
            using (MemoryStream bytes = new MemoryStream(4000))
            {
                while (CurrentChar >= 0)
                {
                    if (CurrentChar == QuoteChar)
                    {
                        if (Peek() == QuoteChar)
                        {
                            CurrentChar = Read();
                        }
                        else
                        {
                            break;
                        }
                    }
                    bytes.WriteByte((byte)CurrentChar);
                    CurrentChar = Read();
                }
                return Encoding.UTF8.GetString(bytes.ToArray());
            }
        }
    }
}
