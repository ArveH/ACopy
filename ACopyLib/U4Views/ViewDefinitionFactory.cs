using ADatabase;

namespace ACopyLib.U4Views
{
    public static class ViewDefinitionFactory
    {
        public static IViewDefinition CreateInstance(DbType dbType, string viewName, string selectStatement)
        {
            return new ViewDefinition(dbType, viewName, selectStatement);
        }

        public static IViewDefinition CreateInstance(DbType dbType)
        {
            return CreateInstance(dbType, "", "");
        }
    }
}
