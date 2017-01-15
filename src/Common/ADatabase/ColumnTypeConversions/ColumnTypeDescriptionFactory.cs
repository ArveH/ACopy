namespace ADatabase
{
    public class ColumnTypeDescriptionFactory : IColumnTypeDescriptionFactory
    {
        public IColumnTypeDescription GetColumnTypeDescription()
        {
            return new ColumnTypeDescription();
        }
    }
}