using System.Collections.Generic;

namespace ADatabase
{
    public interface ITypeConstraintFactory
    {
        ITypeConstraint GetTypeConstraint(string constraintType, string operatorName, IEnumerable<int> constraintValues);
    }
}