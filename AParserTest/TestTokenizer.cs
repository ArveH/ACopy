using AParser;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AParserTest
{
    [TestClass]
    public class TestTokenizer
    {
        [TestMethod]
        public void TestTokenizer_When_EmptyString()
        {
            const string input = "";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(0, "because the string is empty");
        }

        [TestMethod]
        public void TestTokenizer_When_EOL_And_Tab()
        {
            const string input = "one \ttwo \n three";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(3, "because there are three words");
            tokenizer.Tokens[0].Text.Should().Be("one", "because first word is one");
            tokenizer.Tokens[1].Text.Should().Be("two", "because second word is two");
            tokenizer.Tokens[2].Text.Should().Be("three", "because third word is three");
        }

        [TestMethod]
        public void TestTokenizer_When_String()
        {
            const string input = "one 'this is two' three";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(3, "because there are only three tokens");
            tokenizer.Tokens[1].Text.Should().Be("'this is two'", "because the whole string is a single token");
        }

        [TestMethod]
        public void TestTokenizer_When_StringContainsQuoteInMiddle()
        {
            const string input = "one 'this '' two' three";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens[1].Text.Should().Be("'this '' two'", "because two quotes is ok");
        }

        [TestMethod]
        public void TestTokenizer_When_StringContainsOnlyQuote()
        {
            const string input = "one '''' three";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens[1].Text.Should().Be("''''", "because two quotes is ok");
        }

        [TestMethod]
        public void TestTokenizer_When_StringIsLastToken()
        {
            const string input = "one ''''";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens[1].Text.Should().Be("''''", "because two quotes is ok");
        }

        [TestMethod]
        public void TestParenteces_When_ContainsOneSimpleElement()
        {
            const string input = "try func(123)";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(5, "because there are 5 tokens");
        }

        [TestMethod]
        public void TestParenteces_When_ContainsCompoundElement()
        {
            const string input = "try func(func2((123)))";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens[6].Text.Should().Be("123", "because 123 is the 6th element");
        }

        [TestMethod]
        public void TestToString()
        {
            const string input = "try func  (func2 (123))";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.ToString().Should().Be("try func (func2 (123))", "because it should keep spaces (but truncate several contiguous spaces to one)");
        }

        [TestMethod]
        [ExpectedException(typeof(ATokenizerException))]
        public void TestTokenize_When_MissingQuote_Then_Exception()
        {
            const string input = "try 'something  (func2 ('123'))";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
        }

        [TestMethod]
        [ExpectedException(typeof(ATokenizerException))]
        public void TestTokenize_When_MissingEndParentheses_Then_Exception()
        {
            const string input = "try something  (func2 ('123')";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
        }

        [TestMethod]
        [ExpectedException(typeof(ATokenizerException))]
        public void TestTokenize_When_MissingStartParentheses_Then_Exception()
        {
            const string input = "try something func2 ('123'))";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
        }

        [TestMethod]
        public void TestFunction_When_WrappedWithCurlyBraces()
        {
            const string input = "try {fn func  ({ fn func2 (123) })}";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.ToString().Should().Be("try func (func2 (123) )", "because wrapping function is legal syntax");
        }

        [TestMethod]
        public void TestOracleQuotedIdentifier()
        {
            const string input = "select int_col as \"mycol\" from mytable";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(6, "because \"mycol\" is one token, not three");
        }

        [TestMethod]
        public void TestSqlServerQuotedIdentifier()
        {
            const string input = "select int_col as [mycol] from mytable";
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            tokenizer.Tokenize(input);
            tokenizer.Tokens.Count.Should().Be(6, "because [mycol] is one token, not three");
        }

        private void TestUnicodeString(IATokenizer tokenizer, bool unicodeStringMark)
        {
            const string input = "N'value'";
            tokenizer.UnicodeStringMark = unicodeStringMark;
            tokenizer.Tokenize(input);
        }

        [TestMethod]
        public void TestUnicodeString_When_UnicodeStringMarkIsTrue()
        {
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            TestUnicodeString(tokenizer, true);
            tokenizer.Tokens.Count.Should().Be(1, "because N'value' should be only one token");
        }

        [TestMethod]
        public void TestUnicodeString_When_UnicodeStringMarkIsFalse()
        {
            IATokenizer tokenizer = ATokenizerFactory.CreateInstance();
            TestUnicodeString(tokenizer, false);
            tokenizer.Tokens.Count.Should().Be(2, "because N'value' should be two tokens");
        }
    }
}
