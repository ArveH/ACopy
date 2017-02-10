using System.Data;

namespace ADatabase
{
    public interface IDataCursor
    {
        IDataReader ExecuteReader(string selectStatement, bool hasBlobColumn=false);
        void Close();
    }
}
