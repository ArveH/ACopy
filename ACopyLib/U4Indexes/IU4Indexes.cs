using System.Collections.Generic;
using ADatabase;

namespace ACopyLib.U4Indexes
{
    public interface IU4Indexes
    {
        string AagTableName { get; set; }
        string AsysTableName { get; set; }
        List<IIndexDefinition> GetIndexes(string tableName);
    }
}
