namespace ADatabase
{
    public class TypeConstraint : ITypeConstraint
    {
        public ConstraintTypeName ConstraintType { get; set; }
        public ITypeOperator Operator { get; set; }
    }
}