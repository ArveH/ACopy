using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ADatabaseTest
{
    [TestClass]
    public class TestTypeDescription
    {
        [TestMethod]
        public void TestSettingTypeName_When_NoParameters()
        {
            var typeConstraintsFactoryMock = new Mock<ITypeConstraintFactory>();
            var typeDesc = new TypeDescription(typeConstraintsFactoryMock.Object);
            typeDesc.TypeName = "integer";

            typeDesc.TypeName.Should().Be("integer", "because type has no parameters");
            typeDesc.TypeNameParameters.Count.Should().Be(0, "because there were no parameters");
        }

        [TestMethod]
        public void TestSettingTypeName_When_LengthParameter()
        {
            var typeConstraintsFactoryMock = new Mock<ITypeConstraintFactory>();
            var typeDesc = new TypeDescription(typeConstraintsFactoryMock.Object);
            typeDesc.TypeName = "varchar2(@Length)";

            typeDesc.TypeName.Should().Be("varchar2", "because parameter not part of type name");
            typeDesc.TypeNameParameters.Count.Should().Be(1, "because there is a Length parameter");
            typeDesc.TypeNameParameters.ContainsKey("Length").Should().BeTrue("because @Length was given.");
        }

        [TestMethod]
        public void TestSettingTypeName_When_NumberParameter()
        {
            var typeConstraintsFactoryMock = new Mock<ITypeConstraintFactory>();
            var typeDesc = new TypeDescription(typeConstraintsFactoryMock.Object);
            typeDesc.TypeName = "number(@Prec,@Scale)";

            typeDesc.TypeName.Should().Be("number", "because parameters not part of type name");
            typeDesc.TypeNameParameters.Count.Should().Be(2, "because there is a Length parameter");
            typeDesc.TypeNameParameters.ContainsKey("Prec").Should().BeTrue("because @Prec was given.");
            typeDesc.TypeNameParameters.ContainsKey("Scale").Should().BeTrue("because @Scale was given.");
        }
    }
}