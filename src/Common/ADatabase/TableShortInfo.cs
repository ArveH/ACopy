using ADatabase.Interfaces;

namespace ADatabase
{
    public class TableShortInfo: ITableShortInfo
    {
        public TableShortInfo(string name, long rowCount = 0)
        {
            Name = name;
            RowCount = rowCount;
        }

        public string Name { get; set; }

        public long RowCount { get; set; }
    }
}
