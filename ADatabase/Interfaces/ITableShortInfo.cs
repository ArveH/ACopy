namespace ADatabase.Interfaces
{
    public interface ITableShortInfo
    {
        string Name { get; set; }
        long RowCount { get; set; }
    }
}
