using System;

namespace ADatabase.Oracle.Columns
{
    public class OracleRawColumn: OracleColumn
    {
        public OracleRawColumn(string name, bool isNullable, string def) 
            : base(name, ColumnTypeName.Guid, isNullable, def)
        {
        }

        public override string TypeToString()
        {
            return "raw(16)";
        }

        public override string ToString(object value)
        {
            throw new NotImplementedException();
        }

        public override object ToInternalType(string value)
        {
            throw new NotImplementedException();
        }

        public override Type GetDotNetType()
        {
            throw new NotImplementedException();
        }
    }
}