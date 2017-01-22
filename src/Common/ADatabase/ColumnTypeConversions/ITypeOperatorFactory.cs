using System.Collections.Generic;

namespace ADatabase
{
    public interface ITypeOperatorFactory
    {
        ITypeOperator GetTypeOperator(string operatorName, IEnumerable<int> constraintValues);
    }
}