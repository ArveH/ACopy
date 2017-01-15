using System.Collections.Generic;

namespace ADatabase
{
    public class ColumnTypeDescription : IColumnTypeDescription
    {
        public string TypeName { get; set; }
        public string ConvertTo { get; set; }
        public List<ITypeConstraint> Constraints { get; set; }
    }
}