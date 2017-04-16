using System.Collections.Generic;

namespace ADatabase
{
    public interface ITableDefinition
    {
        string Name { get; set; }
        string Location { get; set; }
        List<IColumn> Columns { get; }
        List<IIndexDefinition> Indexes { get; set; }
        bool HasBlobColumn { get; set; }
        void SetCollation(string collation);
        List<string> GetRaw16Columns();
    }
}