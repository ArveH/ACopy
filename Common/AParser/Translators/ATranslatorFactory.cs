using ADatabase;

namespace AParser
{
    public static class ATranslatorFactory
    {
        public static IATranslator CreateInstance(DbType dbType, IASTNodeFactory nodeFactory)
        {
            if (dbType == DbType.Oracle)
            {
                return new OracleTranslator(nodeFactory);
            }
            return new SqlServerTranslator(nodeFactory);
        }
    }
}
