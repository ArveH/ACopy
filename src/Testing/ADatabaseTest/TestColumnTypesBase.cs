using System.Collections.Generic;
using System.IO;
using System.Text;
using ACopyTestHelper;
using ADatabase;

namespace ADatabaseTest
{
    public class TestColumnTypesBase
    {
        protected ConnectionStrings ConnectionStrings = new ConnectionStrings();
        protected IPowerPlant PowerPlant;
        protected IDbContext DbContext;
        protected IDbSchema DbSchema;
        protected IColumnFactory ColumnFactory;
        protected string TableName;

        public virtual void Setup()
        {
            PowerPlant = DbContext.PowerPlant;
            DbSchema = PowerPlant.CreateDbSchema();
            ColumnFactory = PowerPlant.CreateColumnFactory();
            TableName = "htestcolumntypes";
            DbSchema.DropTable(TableName);
        }

        public virtual void Cleanup()
        {
            DbSchema.DropTable(TableName);
        }

        protected void CreateTable(ColumnTypeName type, int length, int prec, int scale, bool isNullable, string def, string collation)
        {
            List<IColumn> columns = new List<IColumn> { ColumnFactory.CreateInstance(type, "col1", length, prec, scale, isNullable, def, collation) };
            ITableDefinition expectedTableDefinition = PowerPlant.CreateTableDefinition(TableName, columns, "");
            DbSchema.CreateTable(expectedTableDefinition);
        }

        private string CreateSchemaXmlOneColumn(string type, int? length, int? prec, int? scale, bool isNullable, string def, string collation)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<!--Table definition file for ACopy program-->");
            sb.Append("<Table");
            sb.Append($"  Name=\"{TableName}\"");
            sb.Append("  Location=\"PRIMARY\">");
            sb.Append("  <Columns>");
            sb.Append("    <Column");
            sb.Append("      Name=\"col1\">");
            sb.Append($"      <Type>{type}</Type>");
            sb.Append($"      <IsNullable>{isNullable}</IsNullable>");
            sb.AppendFormat("      <Default>{0}</Default>", def ?? "");
            sb.Append("      <Details>");
            if (length != null) sb.Append($"        <Length>{length}</Length>");
            if (prec != null) sb.Append($"        <Prec>{prec}</Prec>");
            if (scale != null) sb.Append($"        <Scale>{scale}</Scale>");
            if (collation != null) sb.Append($"        <Collation>{collation}</Collation>");
            sb.Append("      </Details>");
            sb.Append("    </Column>");
            sb.Append("  </Columns>");
            sb.Append("</Table>");

            return sb.ToString();
        }
    }
}