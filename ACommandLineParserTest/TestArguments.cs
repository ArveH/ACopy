using System;
using ACommandLineParser;
using ACommandLineParser.Exceptions;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACommandLineParserTest
{
    [TestClass]
    public class TestArguments
    {
        private IArgumentCollection _arguments;

        [TestInitialize]
        public void Setup()
        {
            _arguments = ArgumentCollectionFactory.CreateArgumentCollection();
        }

        [TestMethod]
        public void TestArgumentFactory_When_IllegalArgument_Then_Exception()
        {
            string[] args = { "-qillegal" };

            Action act = () => _arguments.AddCommandLineArguments(args);
            act.ShouldThrow<ACommandLineParserException>().WithMessage("Illegal command line argument: -qillegal");
        }

        [TestMethod]
        public void TestDirectionArgument()
        {
            string[] args = { "-din" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-d"].Value.Should().Be("in", "because an \"in\" direction was added");
        }

        [TestMethod]
        public void TestDirectionArgument_When_IllegalDirection_Then_Exeption()
        {
            string[] args = { "-dillegal" };

            Action act = () => _arguments.AddCommandLineArguments(args);
            act.ShouldThrow<ACommandLineParserException>().WithMessage("ERROR: 'illegal' is not a legal direction value");
        }

        [TestMethod]
        public void TestUserArgument_When_ServerAndPassworIsSet()
        {
            string[] args = { "-Uagr5", "-Pagresso", "-SAHANSEN08" };


            _arguments.AddCommandLineArguments(args);
            _arguments["User"].IsSet.Should().BeTrue("because user argument is set");
            _arguments["User"].IsRuleOk(_arguments).Should().BeTrue("because server and password is also set");
        }

        [TestMethod]
        public void TestUserArgument_When_PasswordIsNotSet_Then_RuleIsNotOK()
        {
            string[] args = { "-Uagr5", "-P", "-SAHANSEN08" };

            _arguments.AddCommandLineArguments(args);
            _arguments["User"].IsRuleOk(_arguments).Should().BeFalse("because password is not set");
        }

        [TestMethod]
        public void TestVerifyArguments_When_DBProviderMissing_Then_False()
        {
            string[] args = { "-din", "-Uagr5", "-Pagresso", "-SAHANSEN08" };

            _arguments.AddCommandLineArguments(args);
            _arguments.VerifyArguments().Should().BeFalse("because dbprovider is not set");
        }

        [TestMethod]
        public void TestVerifyArguments_When_EverythingOK()
        {
            string[] args = { "-din", "-RM", "-Uagr5", "-Pagresso", "-SAHANSEN08" };

            _arguments.AddCommandLineArguments(args);
            _arguments.VerifyArguments().Should().BeTrue("because all needed arguments present");
        }

        [TestMethod]
        public void TestViewArgument()
        {
            string[] args = { "-v" };

            _arguments["-v"].IsSet.Should().BeFalse("because -v is not used yet");
            _arguments.AddCommandLineArguments(args);
            _arguments["-v"].IsSet.Should().BeTrue("because -v was used");
        }

        [TestMethod]
        public void TestBatchSizeArgument_When_NoBatchSizeGiven_Then_Exception()
        {
            string[] args = { "-b" };

            Action act = () => _arguments.AddCommandLineArguments(args);
            act.ShouldThrow<ACommandLineParserException>().WithMessage("ERROR: Illegal batch size () for -b");
        }

        [TestMethod]
        public void TestBatchSizeArgument()
        {
            string[] args = { "-b1000" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-b"].Value.Should().Be("1000", "because it should be 1000");
        }

        [TestMethod]
        public void TestUseCollation_When_NoCollationGiven()
        {
            string[] args = { "-e" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-e"].Value.Should().BeEmpty("because no collation given");
        }

        [TestMethod]
        public void TestUseCollation_When_CollationGiven()
        {
            string[] args = { "-eLatin1_General_CI_AS" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-e"].Value.Should().Be("Latin1_General_CI_AS");
        }

        [TestMethod]
        public void TestDataFileSuffix()
        {
            string[] args = { "-gu4data" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-g"].Value.Should().Be("u4data");
        }

        [TestMethod]
        public void TestSchemaFileSuffix()
        {
            string[] args = { "-gu4schema" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-g"].Value.Should().Be("u4schema");
        }

        [TestMethod]
        public void TestUsingU4Indexes()
        {
            string[] args = { "-j" };

            _arguments.AddCommandLineArguments(args);
            _arguments["-j"].IsSet.Should().BeTrue("because parameter is set");
        }
    }
}
