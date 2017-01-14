using System;
using System.Collections.Generic;

namespace ADatabase
{
    public class TypeOperatorChecker : ITypeOperatorChecker
    {
        private readonly List<int> _limitValues = new List<int>();
        private readonly TypeOperator _typeOperator;

        public TypeOperatorChecker(int limitValue, TypeOperator typeOperator)
        {
            _limitValues.Add(limitValue);
            _typeOperator = typeOperator;
        }

        public TypeOperatorChecker(IEnumerable<int> limitValues, TypeOperator typeOperator)
        {
            _limitValues.AddRange(limitValues);
            _typeOperator = typeOperator;
        }

        public bool IsWithinLimits(int value)
        {
            switch (_typeOperator)
            {
                case TypeOperator.Eq:
                    return value == _limitValues[0];
                case TypeOperator.Lt:
                    return value < _limitValues[0];
                case TypeOperator.Gt:
                    return value > _limitValues[0];
                case TypeOperator.LtEq:
                    return value <= _limitValues[0];
                case TypeOperator.GtEq:
                    return value >= _limitValues[0];
                case TypeOperator.In:
                    return _limitValues.Contains(value);
                default:
                    throw new ArgumentOutOfRangeException($"TypeOperator \"{_typeOperator}\" doesn't exist.");
            }
        }
    }
}