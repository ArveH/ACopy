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
                "<Type Name=\"varchar2\" To=\"varchar\"></Type>" +
                "</TypeConversions>";
        }

        public static XmlNode NameAttributeMissingForType()
        {
            var txt = "<Type NoName=\"varchar2\" To=\"varchar\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode ToAttributeMissingForType()
        {
            var txt = "<Type Name=\"varchar2\" NoTo=\"varchar\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode IllegatTypeDetail()
        {
            var txt = "<Type Name=\"number\" To=\"bool\">" +
                "<Precision Operator=\"=\">1</Precision>" +
                "<Illegal Operator=\"=\">0</Illegal>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleVarchar2()
        {
            var txt = "<Type Name=\"varchar2\" To=\"varchar\"></Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleBool()
        {
            var txt = "<Type Name=\"number\" To=\"bool\">" +
                "<Precision Operator=\"=\">1</Precision>" +
                "<Scale Operator=\"=\">0</Scale>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static XmlNode OracleGuid()
        {
            var txt = "<Type Name=\"raw\" To=\"guid\">" +
                "<Length Operator=\"in\">16,32,17,34</Length>" +
                "</Type>";
            return GetXmlNode(txt);
        }

        public static string OneTypeNoConstraints(string sourceType, string destinationType)
        {
            return GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                $"<Type Name=\"{sourceType}\" To=\"{destinationType}\"></Type>" +
                "</TypeConversions>";
        }

        public static string FromNumberXml(string destinationType, string prec, string scale)
        {
            return GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                $"<Type Name=\"number\" To=\"{destinationType}\"></Type>" +
                $"<Precision Operator=\"=\">{prec}</Precision>" +
                $"<Scale Operator=\"=\">{scale}</Scale>" +
                "</TypeConversions>";
        }
    }
}