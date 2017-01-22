namespace ADatabase
{
    public class TypeDescriptionFactory : ITypeDescriptionFactory
    {
        private readonly ITypeConstraintFactory _typeConstraintFactory;

        public TypeDescriptionFactory(ITypeConstraintFactory typeConstraintFactory)
        {
            _typeConstraintFactory = typeConstraintFactory;
        }

        public ITypeDescription GetColumnTypeDescription()
        {
            return new TypeDescription(_typeConstraintFactory);
        }
    }
}