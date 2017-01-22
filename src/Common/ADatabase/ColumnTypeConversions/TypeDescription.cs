using System.Collections.Generic;

namespace ADatabase
{
    public class TypeDescription : ITypeDescription
    {
        private readonly ITypeConstraintFactory _typeConstraintFactory;

        public TypeDescription(ITypeConstraintFactory typeConstraintFactory)
        {
            _typeConstraintFactory = typeConstraintFactory;
        }

        public string TypeName { get; set; }
        public string ConvertTo { get; set; }
        public List<ITypeConstraint> Constraints { get; } = new List<ITypeConstraint>();

        public void AddConstraint(string constraintType, string operatorName, int constraintValue)
        {
            AddConstraint(constraintType, operatorName, new []{constraintValue});
        }

        public void AddConstraint(string constraintType, string operatorName, IEnumerable<int> constraintValues)
        {
            var constraint = _typeConstraintFactory.GetTypeConstraint(constraintType, operatorName, constraintValues);
            Constraints.Add(constraint);
        }
    }
}