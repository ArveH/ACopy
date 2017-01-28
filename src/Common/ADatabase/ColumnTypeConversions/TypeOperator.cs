using System;
using System.Collections.Generic;

namespace ADatabase
{
    public class TypeOperator : ITypeOperator
    {
        public List<int> ConstraintValues { get; } = new List<int>();
        public TypeOperatorName OperatorName { get; }

        public TypeOperator(string typeOperatorName, IEnumerable<int> constraintValues)
        {
            ConstraintValues.AddRange(constraintValues);
            OperatorName = Convert(typeOperatorName);
        }

        public bool IsWithinConstraint(int? value)
        {
            if (value == null) return false;

            switch (OperatorName)
            {
                case TypeOperatorName.Eq:
                    return value == ConstraintValues[0];
                case TypeOperatorName.Lt:
                    return value < ConstraintValues[0];
                case TypeOperatorName.Gt:
                    return value > ConstraintValues[0];
                case TypeOperatorName.LtEq:
                    return value <= ConstraintValues[0];
                case TypeOperatorName.GtEq:
                    return value >= ConstraintValues[0];
                case TypeOperatorName.In:
                    return ConstraintValues.Contains(value.Value);
                default:
                    throw new ArgumentOutOfRangeException($"TypeOperatorName \"{OperatorName}\" doesn't exist.");
            }
        }

        private TypeOperatorName Convert(string txt)
        {
            switch (txt)
            {
                case "=": return TypeOperatorName.Eq;
                case "<": return TypeOperatorName.Lt;
                case ">": return TypeOperatorName.Gt;
                case "<=": return TypeOperatorName.LtEq;
                case ">=": return TypeOperatorName.GtEq;
                case "in": return TypeOperatorName.In;
            }

            throw new ArgumentException($"Can't convert '{txt}' to TypeOperatorName. Legal values are '=', '<', '>', '<=', '>=', 'in'");
        }
    }
}