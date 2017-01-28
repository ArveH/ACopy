using System;

namespace ADatabase
{
    public class TypeConstraint : ITypeConstraint
    {
        public TypeConstraint(string constraintType, ITypeOperator op)
        {
            ConstraintType = constraintType;
            Operator = op;
        }

        public string ConstraintType { get; }
        public ITypeOperator Operator { get;  }

        public bool IsWithinConstraint(int? val)
        {
            return Operator.IsWithinConstraint(val);
        }
    }
}