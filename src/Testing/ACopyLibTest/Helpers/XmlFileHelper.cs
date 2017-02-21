using System.Text;

namespace ACopyLibTest.Helpers
{
    public class XmlFileHelper
    {
        public static string CreateSchemaXmlOneColumn(string tableName, string type, int? length, int? prec, int? scale, bool isNullable, string def, string collation)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.Append("<!--Table definition file for ACopy program-->");
            sb.Append("<Table");
            sb.Append($"  Name=\"{tableName}\"");
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