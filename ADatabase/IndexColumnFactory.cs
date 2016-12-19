using ADatabase.Interfaces;

namespace ADatabase
{
    public static class IndexColumnFactory
    {
        public static IIndexColumn CreateInstance(string name, string expression = "")
        {
            return new IndexColumn(name, expression);
        }
    }
}
