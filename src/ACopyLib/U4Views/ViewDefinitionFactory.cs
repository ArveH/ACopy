using ADatabase;

namespace ACopyLib.U4Views
{
    public static class ViewDefinitionFactory
    {
        public static IViewDefinition CreateInstance(DbTypeName dbType, string viewName, string selectStatement)
        {
            return new ViewDefinition(dbType, viewName, selectStatement);
        }

        public static IViewDefinition CreateInstance(DbTypeName dbType)
        {
            return CreateInstance(dbType, "", "");
        }
    }
}
