using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestTypeOperatorChecker
    {
        private ITypeOperatorChecker _typeOperatorChecker;

        public void GetTypeOperatorChecker(int value, TypeOperatorName typeOperatorName)
        {
            _typeOperatorChecker = new TypeOperatorChecker(value, typeOperatorName);
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Eq()
        {
            GetTypeOperatorChecker(3, TypeOperatorName.Eq);
            _typeOperatorChecker.IsWithinLimits(3).Should().BeTrue("because 3 is the limit value");
            _typeOperatorChecker.IsWithinLimits(4).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Lt()
        {
            GetTypeOperatorChecker(3, TypeOperatorName.Lt);
            _typeOperatorChecker.IsWithinLimits(2).Should().BeTrue("because it's less than 3");
            _typeOperatorChecker.IsWithinLimits(3).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Gt()
        {
            GetTypeOperatorChecker(3, TypeOperatorName.Gt);
            _typeOperatorChecker.IsWithinLimits(4000).Should().BeTrue("because it's greater than 3");
            _typeOperatorChecker.IsWithinLimits(2).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_LtEq()
        {
            GetTypeOperatorChecker(3, TypeOperatorName.LtEq);
            _typeOperatorChecker.IsWithinLimits(3).Should().BeTrue("because limit value is 3");
            _typeOperatorChecker.IsWithinLimits(2).Should().BeTrue("because limit value is 3");
            _typeOperatorChecker.IsWithinLimits(4).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_GtEq()
        {
            GetTypeOperatorChecker(3, TypeOperatorName.GtEq);
            _typeOperatorChecker.IsWithinLimits(3).Should().BeTrue("because limit value is 3");
            _typeOperatorChecker.IsWithinLimits(2).Should().BeFalse("because limit value is 3");
            _typeOperatorChecker.IsWithinLimits(4).Should().BeTrue("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_In()
        {
            _typeOperatorChecker = new TypeOperatorChecker(new [] {3, 5, 7, 9}, TypeOperatorName.In);
            _typeOperatorChecker.IsWithinLimits(3).Should().BeTrue("because its' an odd number");
            _typeOperatorChecker.IsWithinLimits(2).Should().BeFalse("because it's an even number");
        }
    }
}