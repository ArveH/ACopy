using System.Collections.Generic;

namespace ADatabase
{
    public interface ITableDefinition
    {
        string Name { get; set; }
        string Location { get; set; }
        List<IColumn> Columns { get; }
        List<IIndexDefinition> Indexes { get; set; }
        bool HasRawColumn { get; set; }
        void SetSizeForGuid(int rawLength);
        void SetCollation(string collation);
    }
}