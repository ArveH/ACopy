namespace ADatabase
{
    public interface ITypeConstraint
    {
        string ConstraintType { get; }
        ITypeOperator Operator { get; }
        bool IsWithinConstraint(int? val);
    }
}