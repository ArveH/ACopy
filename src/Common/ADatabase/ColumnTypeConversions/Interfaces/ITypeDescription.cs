using System.Collections.Generic;

namespace ADatabase
{
    public interface ITypeDescription
    {
        string TypeName { get; set; }
        string ConvertTo { get; set; }
        List<ITypeConstraint> Constraints { get; }
        void AddConstraint(string constraintType, string operatorName, IEnumerable<int> constraintValues);
        void AddConstraint(string constraintType, string operatorName, int constraintValue);
    }
}