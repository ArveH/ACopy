using System.Data;

namespace ADatabase
{
    public interface IDataCursor
    {
        IDataReader ExecuteReader(string selectStatement, bool hasRawColumn=false);
        void Close();
    }
}
