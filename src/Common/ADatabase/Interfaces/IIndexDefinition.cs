using System.Collections.Generic;
using ADatabase.Interfaces;

namespace ADatabase
{
    public interface IIndexDefinition
    {
        string IndexName { get; }
        string TableName { get; }
        int IndexId { get; }
        string Location { get; }
        bool IsUnique { get; }
        bool IsClustered { get; }
        DbTypeName DbSpecific { get; set; }
        List<IIndexColumn> Columns { get; set; }
    }
}
