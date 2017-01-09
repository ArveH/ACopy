using ADatabase;
using ADatabase.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypeConverter
    {
        [TestMethod]
        [ExpectedException(typeof(AColumnTypeException))]
        public void TestInitialization_When_IllegalXml()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            converter.Initialize("");
        }

    }
}