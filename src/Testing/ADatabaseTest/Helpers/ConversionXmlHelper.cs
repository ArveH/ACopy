namespace ADatabaseTest.Helpers
{
    public static class ConversionXmlHelper
    {
        public static string LegalXmlButNoConversions()
        {
            return
                GetHeading() +
                "<TypeConversions From=\"Oracle\" To=\"ACopy\">" +
                "</TypeConversions>";
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

        private static string GetHeading()
        {
            return
                "<?xml version=\"1.0\" encoding=\"utf-8\"?> " +
                "<!--Type conversion file for Unit4--> ";
        }
    }
}