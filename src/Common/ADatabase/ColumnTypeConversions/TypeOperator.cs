using System;
using System.Collections.Generic;

namespace ADatabase
{
    public class TypeOperator : ITypeOperator
    {
        private readonly List<int> _constraintValues = new List<int>();
        private readonly TypeOperatorName _typeOperatorName;

        public TypeOperator(int constraintValue, TypeOperatorName typeOperatorName)
        {
            _constraintValues.Add(constraintValue);
            _typeOperatorName = typeOperatorName;
        }

        public TypeOperator(IEnumerable<int> constraintValues, TypeOperatorName typeOperatorName)
        {
            _constraintValues.AddRange(constraintValues);
            _typeOperatorName = typeOperatorName;
        }

        public bool IsWithinConstraint(int value)
        {
            switch (_typeOperatorName)
            {
                case TypeOperatorName.Eq:
                    return value == _constraintValues[0];
                case TypeOperatorName.Lt:
                    return value < _constraintValues[0];
                case TypeOperatorName.Gt:
                    return value > _constraintValues[0];
                case TypeOperatorName.LtEq:
                    return value <= _constraintValues[0];
                case TypeOperatorName.GtEq:
                    return value >= _constraintValues[0];
                case TypeOperatorName.In:
                    return _constraintValues.Contains(value);
                default:
                    throw new ArgumentOutOfRangeException($"TypeOperatorName \"{_typeOperatorName}\" doesn't exist.");
            }
        }
    }
}