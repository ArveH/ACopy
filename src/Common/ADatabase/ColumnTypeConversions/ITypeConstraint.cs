namespace ADatabase
{
    public interface ITypeConstraint
    {
        ConstraintTypeName ConstraintType { get; set; }
        ITypeOperator Operator { get; set; }
    }
}