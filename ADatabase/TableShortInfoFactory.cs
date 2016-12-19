using ADatabase.Interfaces;

namespace ADatabase
{
    public static class TableShortInfoFactory
    {
        public static ITableShortInfo CreateInstance(string name, long rowCount=0)
        {
            return new TableShortInfo(name, rowCount);
        }
    }
}
