using System.Collections.Generic;

namespace ADatabase
{
    public interface ITypeOperator
    {
        TypeOperatorName OperatorName { get; }
        List<int> ConstraintValues { get; }
        bool IsWithinConstraint(int value);
    }
}