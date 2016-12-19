using System;
using ADatabase;

namespace ACopyLib.U4Views
{
    public class ViewDefinition: IViewDefinition
    {
        public ViewDefinition(DbType dbType): this(dbType, "", "")
        {
        }

        public ViewDefinition(DbType dbType, string viewName, string selectStatement)
        {
            DbType = dbType;
            ViewName = viewName;
            SelectStatement = selectStatement;
        }

        public string ViewName { get; set; }

        public DbType DbType { get; private set; }

        public string SelectStatement { get; set; }

        public static DbType ConvertFromStringToDbType(string dbType)
        {
            if (String.Compare(dbType, "oracle", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbType.Oracle;
            }
            if (String.Compare(dbType, "mssql", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbType.SqlServer;
            }
            return DbType.Any;
        }
    }
}
