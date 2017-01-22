namespace ADatabase
{
    public interface ITypeDescriptionFactory
    {
        ITypeDescription GetColumnTypeDescription();
    }
}