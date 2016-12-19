using System.Data;
using ADatabase;

namespace ACopyLib.DataReader
{
    public static class ADataReaderFactory
    {
        public static IDataReader CreateInstance(string fileName, ITableDefinition tableDefinition, long largeBlobSize)
        {
            return new ADataReader(fileName, tableDefinition, largeBlobSize);
        }
    }
}