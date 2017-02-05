using System.Collections.Generic;

namespace ACopyLib.Writer
{
    public interface IAWriter
    {
        bool UseCompression { get; set; }
        int MaxDegreeOfParallelism { get; set; }
        string Directory { get; set; }
        string DataFileSuffix { get; set; }
        string SchemaFileSuffix { get; set; }
        bool UseU4Indexes { get; set; }
        string ConversionsFile { get; set; }
        void Write(List<string> tables);
        bool WriteTable(string tableName);
    }
}