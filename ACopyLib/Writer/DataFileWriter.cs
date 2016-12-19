using System;
using System.IO;

namespace ACopyLib.Writer
{
    public abstract class DataFileWriter: IDisposable
    {
        protected DataFileWriter(string fileName, bool isCompressed)
        {
            IsCompressed = isCompressed;
            _fileName = fileName;
        }

        public bool IsCompressed { get; private set; }

        protected Stream CompressedStream { get; set; }

        private readonly string _fileName;
        public string FileName
        {
            get 
            { 
                return _fileName + (IsCompressed ? CompressionFileEnding : "");
            }
        }

        protected Stream BaseStream { get; set; }

        protected Action FlushMethod { get; set; }

        protected Action CloseMethod { get; set; }

        protected Action<byte[], int, int> WriteBytes { get; set; }

        public static string CompressionFileEnding
        {
            get { return ".dz"; }
        }

        public void Flush()
        {
            FlushMethod();
        }

        public void Write(byte[] value, int offset, int numberOfBytes)
        {
            WriteBytes(value, offset, numberOfBytes);
        }

        public void Close()
        {
            if (CompressedStream != null)
            {
                CompressedStream.Dispose();
            }
            CloseMethod();
        }

        public abstract void Dispose();
    }
}
