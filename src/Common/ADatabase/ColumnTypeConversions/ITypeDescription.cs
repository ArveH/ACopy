using System.Collections.Generic;

namespace ADatabase
{
    public interface ITypeDescription
    {
        string TypeName { get; set; }
        string TypeTo { get; set; }
        List<ITypeConstraint> Constraints { get; set; }
    }
}