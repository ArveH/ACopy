using System;

namespace ADatabase.SqlServer.Columns
{
    public class SqlServerImageColumn: SqlServerVarBinaryColumn
    {
        public SqlServerImageColumn(string name, bool isNullable, string def)
            : base(name, -1, isNullable, def)
        {
            Type = ColumnTypeName.OldBlob;
        }

        public override string TypeToString()
        {
            return "image";
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException("Column.ToFile for OLDBLOB");
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException("Column.ToInternalType for OLDBLOB");
        }
    }
}