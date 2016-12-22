using System.IO;
using ACopyLib.Writer;

namespace ACopyLib.Reader
{
    public static class DataFileReaderFactory
    {
        public static IDataFileReader CreateInstance(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            if (extension != null && extension.ToLower() == DataFileWriter.CompressionFileEnding)
            {
                return new DataFileCompressedReader(fileName);
            }
            return new DataFileUncompressedReader(fileName);
        }
    }
}