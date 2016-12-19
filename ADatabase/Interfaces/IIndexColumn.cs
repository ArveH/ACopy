namespace ADatabase.Interfaces
{
    public interface IIndexColumn
    {
        string Name { get; set; }
        bool IsExpression { get; set; }
        string Expression { get; set; }
    }
}
