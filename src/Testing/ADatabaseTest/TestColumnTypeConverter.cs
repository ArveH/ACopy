using System;
using ADatabase;
using ADatabase.Exceptions;
using ADatabaseTest.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace ADatabaseTest
{
    [TestClass]
    public class TestColumnTypeConverter
    {
        [TestMethod]
        public void TestInitialization_When_IllegalXml()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            Action act = () => converter.Initialize("<illegal xml>");
            act.ShouldThrow<AColumnTypeException>().WithMessage("Error when reading conversion XML");
        }

        [TestMethod]
        public void TestInitialization_When_LegalXmlButIncorrectRootElement()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            Action act = () => converter.Initialize(ConversionXmlHelper.LegalXmlButIncorrectRootElement());
            act.ShouldThrow<AColumnTypeException>().WithMessage("Can't find root element 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_FromAttributeMissing()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            Action act = () => converter.Initialize(ConversionXmlHelper.FromAttributeMissing());
            act.ShouldThrow<AColumnTypeException>().WithMessage("Error with attribute 'From' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_ToAttributeMissing()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            Action act = () => converter.Initialize(ConversionXmlHelper.ToAttributeBlank());
            act.ShouldThrow<AColumnTypeException>().WithMessage("Error with attribute 'To' for 'TypeConversions'");
        }

        [TestMethod]
        public void TestInitialization_When_LegalXmlButNoConversions()
        {
            IColumnTypeConverter converter = new ColumnTypeConverter();
            Action act = () => converter.Initialize(ConversionXmlHelper.LegalXmlButNoConversions());
            act.ShouldThrow<AColumnTypeException>().WithMessage("No conversions found");
        }
    }
}