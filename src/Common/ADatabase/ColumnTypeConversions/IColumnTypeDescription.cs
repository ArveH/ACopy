using System.Collections.Generic;

namespace ADatabase
{
    public interface IColumnTypeDescription
    {
        string TypeName { get; set; }
        string ConvertTo { get; set; }
        List<ITypeConstraint> Constraints { get; set; }
    }
}