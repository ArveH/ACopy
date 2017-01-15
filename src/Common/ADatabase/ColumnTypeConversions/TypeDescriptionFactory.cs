namespace ADatabase
{
    public class TypeDescriptionFactory : ITypeDescriptionFactory
    {
        public ITypeDescription GetTypeDescription()
        {
            return new TypeDescription();
        }
    }
}