using System;
using ADatabase;

namespace ACopyLib.U4Views
{
    public class ViewDefinition: IViewDefinition
    {
        public ViewDefinition(DbTypeName dbType): this(dbType, "", "")
        {
        }

        public ViewDefinition(DbTypeName dbType, string viewName, string selectStatement)
        {
            DbType = dbType;
            ViewName = viewName;
            SelectStatement = selectStatement;
        }

        public string ViewName { get; set; }

        public DbTypeName DbType { get; private set; }

        public string SelectStatement { get; set; }

        public static DbTypeName ConvertFromStringToDbType(string dbType)
        {
            if (String.Compare(dbType, "oracle", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbTypeName.Oracle;
            }
            if (String.Compare(dbType, "mssql", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return DbTypeName.SqlServer;
            }
            return DbTypeName.Any;
        }
    }
}
