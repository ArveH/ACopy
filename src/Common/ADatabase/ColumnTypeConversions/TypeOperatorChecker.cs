using System;
using System.Collections.Generic;

namespace ADatabase
{
    public class TypeOperatorChecker : ITypeOperatorChecker
    {
        private readonly List<int> _limitValues = new List<int>();
        private readonly TypeOperatorName _typeOperatorName;

        public TypeOperatorChecker(int limitValue, TypeOperatorName typeOperatorName)
        {
            _limitValues.Add(limitValue);
            _typeOperatorName = typeOperatorName;
        }

        public TypeOperatorChecker(IEnumerable<int> limitValues, TypeOperatorName typeOperatorName)
        {
            _limitValues.AddRange(limitValues);
            _typeOperatorName = typeOperatorName;
        }

        public bool IsWithinLimits(int value)
        {
            switch (_typeOperatorName)
            {
                case TypeOperatorName.Eq:
                    return value == _limitValues[0];
                case TypeOperatorName.Lt:
                    return value < _limitValues[0];
                case TypeOperatorName.Gt:
                    return value > _limitValues[0];
                case TypeOperatorName.LtEq:
                    return value <= _limitValues[0];
                case TypeOperatorName.GtEq:
                    return value >= _limitValues[0];
                case TypeOperatorName.In:
                    return _limitValues.Contains(value);
                default:
                    throw new ArgumentOutOfRangeException($"TypeOperatorName \"{_typeOperatorName}\" doesn't exist.");
            }
        }
    }
}