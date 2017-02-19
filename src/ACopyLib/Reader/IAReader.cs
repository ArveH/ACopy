using System.Collections.Generic;

namespace ACopyLib.Reader
{
    public interface IAReader
    {
        void Read(List<string> tableNameList, out int tableCounter, out int errorCounter);
        string Directory { get; set; }
        int BatchSize { get; set; }
        int MaxDegreeOfParallelism { get; set; }
        bool CreateClusteredIndex { get; set; }
        string Collation { get; set; }
        string DataFileSuffix { get; set; }
        string SchemaFileSuffix { get; set; }
    }
}