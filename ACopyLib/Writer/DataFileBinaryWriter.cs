using System.IO;
using System.IO.Compression;

namespace ACopyLib.Writer
{
    public class DataFileBinaryWriter: DataFileWriter 
    {
        private readonly BinaryWriter _binaryWriter;

        public DataFileBinaryWriter(string fileName, bool createCompressed): base(fileName, createCompressed)
        {
            _binaryWriter = new BinaryWriter(new FileStream(FileName, FileMode.Create, FileAccess.Write));
            BaseStream = _binaryWriter.BaseStream;
            CloseMethod = _binaryWriter.Close;
            FlushMethod = _binaryWriter.Flush;

            if (createCompressed)
            {
                CompressedStream = new DeflateStream(BaseStream, CompressionLevel.Fastest, true);
                WriteBytes = CompressedStream.Write;
            }
            else
            {
                WriteBytes = _binaryWriter.Write;
            }
        }

        public override void Dispose()
        {
            if (IsCompressed)
            {
                CompressedStream.Dispose();
            }
            _binaryWriter.Dispose();
        }
    }
}
