namespace ADatabase
{
    public class TypeConstraint : ITypeConstraint
    {
        public ConstraintTypeName ConstraintType { get; set; }
        public ITypeOperatorChecker Operator { get; set; }
    }
}