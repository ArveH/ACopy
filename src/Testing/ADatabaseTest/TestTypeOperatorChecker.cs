using System;
using ADatabase;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADatabaseTest
{
    [TestClass]
    public class TestTypeOperatorChecker
    {
        private ITypeOperator _typeOperator;

        public void GetTypeOperatorChecker(int value, string typeOperatorName)
        {
            _typeOperator = new TypeOperator(typeOperatorName, new []{value});
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Eq()
        {
            GetTypeOperatorChecker(3, "=");
            _typeOperator.IsWithinConstraint(3).Should().BeTrue("because 3 is the limit value");
            _typeOperator.IsWithinConstraint(4).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Lt()
        {
            GetTypeOperatorChecker(3, "<");
            _typeOperator.IsWithinConstraint(2).Should().BeTrue("because it's less than 3");
            _typeOperator.IsWithinConstraint(3).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_Gt()
        {
            GetTypeOperatorChecker(3, ">");
            _typeOperator.IsWithinConstraint(4000).Should().BeTrue("because it's greater than 3");
            _typeOperator.IsWithinConstraint(2).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_LtEq()
        {
            GetTypeOperatorChecker(3, "<=");
            _typeOperator.IsWithinConstraint(3).Should().BeTrue("because limit value is 3");
            _typeOperator.IsWithinConstraint(2).Should().BeTrue("because limit value is 3");
            _typeOperator.IsWithinConstraint(4).Should().BeFalse("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_GtEq()
        {
            GetTypeOperatorChecker(3, ">=");
            _typeOperator.IsWithinConstraint(3).Should().BeTrue("because limit value is 3");
            _typeOperator.IsWithinConstraint(2).Should().BeFalse("because limit value is 3");
            _typeOperator.IsWithinConstraint(4).Should().BeTrue("because limit value is 3");
        }

        [TestMethod]
        public void TestTypeOperatorChecker_When_In()
        {
            _typeOperator = new TypeOperator("in", new[] {3, 5, 7, 9});
            _typeOperator.IsWithinConstraint(3).Should().BeTrue("because its' an odd number");
            _typeOperator.IsWithinConstraint(2).Should().BeFalse("because it's an even number");
        }

        [TestMethod]
        public void TestGetTypeOperator_When_UppercaseLetter_Then_Exception()
        {
            Action act = () => _typeOperator = new TypeOperator("In", new[] { 3, 5, 7, 9 });
            act.ShouldThrow<ArgumentException>()
                .WithMessage(
                    "Can't convert 'In' to TypeOperatorName. Legal values are '=', '<', '>', '<=', '>=', 'in'");
        }

    }
}