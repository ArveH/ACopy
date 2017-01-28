using System.Xml;

namespace ADatabaseTest.Helpers
{
    public static class ConversionXmlHelper
    {
        private static string GetHeading()
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

        public static string LegalXmlButIncorrectRootElement()
        {
            return
                GetHeading() +
                "<MyBody></MyBody>";
        }

        public static string TypeConversionsHasNoAttributes()
        {
            return
                GetHeading() +
                "<TypeConversions />";
        }

        public static string FromAttributeMissing()
        {
            return
                GetHeading() +
                "<TypeConversions To=\"ACopy\">" +
                "</TypeConversions>";
        }

        public static string ToAttributeBlank()
        {
            return
                GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"\">" +
                "</TypeConversions>";
        }

        public static string LegalRootButNoConversions()
        {
            return
                GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                "</TypeConversions>";
        }

        public static string LegalRootOneVarcharColumn()
        {
            return
                GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                "<Type Source=\"varchar2(@Length)\" Destination=\"varchar(@Length)\"></Type>" +
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

        public static string OneTypeNoConstraints(string sourceType, string destinationType)
        {
            return GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                $"<Type Source=\"{sourceType}\" Destination=\"{destinationType}\"></Type>" +
                "</TypeConversions>";
        }

        public static string FromNumberXml(string destinationType, string prec, string scale)
        {
            return GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">\n" +
                $"<Type Source=\"number(@Prec,@Scale)\" Destination=\"{destinationType}\">\n" +
                $"<Prec Operator=\"=\">{prec}</Prec>\n" +
                $"<Scale Operator=\"=\">{scale}</Scale>\n" +
                $"</Type>\n" +
                "</TypeConversions>";
        }
    }
}