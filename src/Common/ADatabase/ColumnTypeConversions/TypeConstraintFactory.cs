using System.Collections.Generic;

namespace ADatabase
{
    public class TypeConstraintFactory : ITypeConstraintFactory
    {
        private readonly ITypeOperatorFactory _operatorFactory;

        public TypeConstraintFactory(ITypeOperatorFactory operatorFactory)
        {
            _operatorFactory = operatorFactory;
        }

        public ITypeConstraint GetTypeConstraint(
            string constraintType, 
            string operatorName,
            IEnumerable<int> constraintValues)
        {
            var op = _operatorFactory.GetTypeOperator(operatorName, constraintValues);
            return new TypeConstraint(constraintType, op);
        }
    }
}