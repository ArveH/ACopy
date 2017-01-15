namespace ADatabase
{
    public interface ITypeConstraint
    {
        ConstraintTypeName ConstraintType { get; set; }
        ITypeOperatorChecker Operator { get; set; }
    }
}