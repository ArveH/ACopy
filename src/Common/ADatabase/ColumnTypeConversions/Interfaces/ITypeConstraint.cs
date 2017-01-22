namespace ADatabase
{
    public interface ITypeConstraint
    {
        ConstraintTypeName ConstraintType { get; }
        ITypeOperator Operator { get; }
    }
}