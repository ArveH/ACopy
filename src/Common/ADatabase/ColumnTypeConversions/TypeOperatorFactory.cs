using System.Collections.Generic;

namespace ADatabase
{
    public class TypeOperatorFactory : ITypeOperatorFactory
    {
        public ITypeOperator GetTypeOperator(string operatorName, IEnumerable<int> constraintValues)
        {
            return new TypeOperator(operatorName, constraintValues);
        }
    }
}