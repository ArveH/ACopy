using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ACopyLib.Writer
{
    public class DataFileTextWriter: IDisposable
    {
        private readonly StreamWriter _streamWriter;
                
        public DataFileTextWriter(string fileName, bool createCompressed)
        {
            IsCompressed = createCompressed;
            _fileName = fileName;
            _streamWriter = new StreamWriter(FileName, false, new UTF8Encoding(false));
            if (createCompressed)
            {
                _compressedStream = new DeflateStream(_streamWriter.BaseStream, CompressionLevel.Fastest, true);
            }
        }

        private readonly string _fileName;
        public string FileName
        {
            get
            {
                return _fileName + (IsCompressed ? ".dz" : "");
            }
        }

        public bool IsCompressed { get; private set; }

        private readonly Stream _compressedStream;

        public void Flush()
        {
            _streamWriter.Flush();
        }

        public void Write(string value)
        {
            if (IsCompressed)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                _compressedStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                _streamWriter.Write(value);
            }
        }

        public void WriteLine()
        {
            if (IsCompressed)
            {
                byte[] bytes = Encoding.UTF8.GetBytes("\n");
                _compressedStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                _streamWriter.WriteLine();
            }
        }

        public void Dispose()
        {
            if (IsCompressed)
            {
                _compressedStream.Dispose();
            }
            _streamWriter.Dispose();
        }
    }
}
