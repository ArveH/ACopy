using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerImageColumn: SqlServerVarBinaryColumn
    {
        public SqlServerImageColumn(string name, bool isNullable, string def)
            : base(name, -1, isNullable, def)
        {
            Type = ColumnTypeName.OldRaw;
        }

        public override string TypeToString()
        {
            return "image";
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException("Column.ToFile for OLDRAW");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for OLDRAW");
        }
    }
}