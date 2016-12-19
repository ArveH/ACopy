using System.Data;

namespace ADatabase
{
    public interface IFastCopy
    {
        long LoadData(IDataReader reader, string fileName, ITableDefinition tableDefinition);
        long LargeBlobSize { get; set; }
        int BatchSize { get; set; }
    }
}