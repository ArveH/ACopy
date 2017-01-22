using ADatabase;

namespace AParser
{
    public static class ATranslatorFactory
    {
        public static IATranslator CreateInstance(DbTypeName dbType, IASTNodeFactory nodeFactory)
        {
            if (dbType == DbTypeName.Oracle)
            {
                return new OracleTranslator(nodeFactory);
            }
            return new SqlServerTranslator(nodeFactory);
        }
    }
}
