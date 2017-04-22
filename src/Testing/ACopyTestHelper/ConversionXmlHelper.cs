using System.Xml;
using ADatabase;

namespace ACopyTestHelper
{
    public static class ConversionXmlHelper
    {
        public static string GetHeadingXml()
        {
            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<!--Type conversion file for Unit4--> ";
        }

        private static XmlNode GetXmlNode(string txt)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(txt);
            return xmlDocument.FirstChild;
        }

        public static string GetOneNumberTypeXml(string destinationType, string prec, string scale)
        {
            return $"<Type Source=\"number(@Prec,@Scale)\" Destination=\"{destinationType}\">\n" +
                   $"<Prec Operator=\"=\">{prec}</Prec>\n" +
                   $"<Scale Operator=\"=\">{scale}</Scale>\n" +
                   $"</Type>\n";
        }

        public static string GetOneTypeNoOperatorXml(string sourceType, string destinationType)
        {
            return $"<Type Source=\"{sourceType}\" Destination=\"{destinationType}\"/>\n";
        }

        public static string LegalXmlButIncorrectRootElement()
        {
            return
                GetHeadingXml() +
                "<MyBody></MyBody>";
        }

        public static string TypeConversionsHasNoAttributesXml()
        {
            return
                GetHeadingXml() +
                "<TypeConversions />";
        }

        public static string DatabaseAttributeMissingXml()
        {
            return
                GetHeadingXml() +
                "<TypeConversions Direction=\"ACopy\">" +
                "</TypeConversions>";
        }

        public static string DirectionAttributeBlankXml()
        {
            return
                GetHeadingXml() +
                $"<TypeConversions Database=\"{DatabaseSystemName.Oracle}\" Direction=\"\">" +
                "</TypeConversions>";
        }

        public static string LegalRootButNoConversionsXml()
        {
            return
                GetHeadingXml() +
                $"<TypeConversions Database=\"{DatabaseSystemName.Oracle}\" Direction=\"{CopyDirection.FromFileToTable}\">" +
                "</TypeConversions>";
        }

        public static XmlNode SourceAttributeMissingForType()
        {
            var txt = "<Type NoSource=\"varchar2(@Langth)\" Destination=\"varchar(@Length)\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode DestinationAttributeMissingForType()
        {
            var txt = "<Type Source=\"varchar2(@Length)\" NoDestination=\"varchar(@Length)\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode IllegatTypeDetail()
        {
            var txt = "<Type Source=\"number(@Prec,@Scale)\" Destination=\"bool\">" +
                "<Prec Operator=\"=\">1</Prec>" +
                "<Illegal Operator=\"=\">0</Illegal>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleVarchar2()
        {
            var txt = "<Type Source=\"varchar2(@Length)\" Destination=\"varchar(@Length)\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleBool()
        {
            var txt = "<Type Source=\"number(@Prec,@Scale)\" Destination=\"bool\">" +
                "<Prec Operator=\"=\">1</Prec>" +
                "<Scale Operator=\"=\">0</Scale>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleGuid()
        {
            var txt = "<Type Source=\"raw(@Length)\" Destination=\"guid\">" +
                "<Length Operator=\"in\">16,32,17,34</Length>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static string OracleGuidXml()
        {
            return GetHeadingXml() +
                   $"<TypeConversions Database=\"Oracle\" Direction=\"{CopyDirection.FromTableToFile}\">" +
                   "<Type Source=\"raw(@Length)\" Destination=\"guid\">" +
                   "<Length Operator=\"in\">16,32,17,34</Length>" +
                   "</Type>" +
                   "</TypeConversions>";
        }

        public static string OneNumberTypeXml(string destinationType, string prec, string scale)
        {
            return GetHeadingXml() +
                $"<TypeConversions Database=\"Oracle\" Direction=\"{CopyDirection.FromTableToFile}\">\n" +
                GetOneNumberTypeXml(destinationType, prec, scale) +
                "</TypeConversions>";
        }

        public static string Unit4OracleWriterConversionsXml()
        {
            return GetHeadingXml() +
                $"<TypeConversions Database=\"Oracle\" Direction=\"{CopyDirection.FromTableToFile}\">\n" +
                GetOneTypeNoOperatorXml("varchar2(@Length)", "varchar(@Length)") +
                GetOneTypeNoOperatorXml("varchar(@Length)", "varchar(@Length)") +
                GetOneTypeNoOperatorXml("char(@Length)", "varchar(@Length)") +
                GetOneTypeNoOperatorXml("clob", "longtext") +
                GetOneTypeNoOperatorXml("integer", "int") +
                GetOneNumberTypeXml("bool", "1", "0") +
                GetOneNumberTypeXml("int8", "3", "0") +
                GetOneNumberTypeXml("int16", "5", "0") +
                GetOneNumberTypeXml("int", "15", "0") +
                GetOneNumberTypeXml("int64", "20", "0") +
                GetOneNumberTypeXml("money", "18", "2") +
                GetOneNumberTypeXml("money", "30", "3") +
                GetOneTypeNoOperatorXml("number(@Prec,@Scale)", "float") +
                GetOneTypeNoOperatorXml("float", "float") +
                GetOneTypeNoOperatorXml("date", "datetime") +
                   "<Type Source=\"raw(@Length)\" Destination=\"guid\">" +
                   "<Length Operator=\"in\">16,32,17,34</Length>" +
                   "</Type>" +
                GetOneTypeNoOperatorXml("blob", "blob") +
                GetOneTypeNoOperatorXml("raw", "blob") +
                GetOneTypeNoOperatorXml("oldblob", "blob") +
                "</TypeConversions>";
        }

        public static string Unit4OracleReaderConversionsXml()
        {
            return GetHeadingXml() +
                $"<TypeConversions Database=\"Oracle\" Direction=\"{CopyDirection.FromFileToTable}\">\n" +
                GetOneTypeNoOperatorXml("varchar(@Length)", "varchar2(@Length)") +
                GetOneTypeNoOperatorXml("char(@Length)", "varchar2(@Length)") +
                GetOneTypeNoOperatorXml("longtext", "clob") +
                GetOneTypeNoOperatorXml("integer", "int") +
                GetOneTypeNoOperatorXml("bool", "number(1,0)") +
                GetOneTypeNoOperatorXml("int8", "number(3,0)") +
                GetOneTypeNoOperatorXml("int16", "number(5,0)") +
                GetOneTypeNoOperatorXml("int", "number(15,0)") +
                GetOneTypeNoOperatorXml("int64", "number(20,0)") +
                GetOneTypeNoOperatorXml("money", "number(30,3)") +
                GetOneTypeNoOperatorXml("float", "float") +
                GetOneTypeNoOperatorXml("datetime", "date") +
                GetOneTypeNoOperatorXml("guid", "raw(16)") +
                GetOneTypeNoOperatorXml("blob", "blob") +
                "</TypeConversions>";
        }

    }
}