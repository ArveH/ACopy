using System.Collections.Generic;
using AParser;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AParserTest
{
    [TestClass]
    public class TestParser
    {
        IAParser _parser;
        List<IASTNode> _nodes;

        [TestInitialize]
        public void Setup()
        {
            _parser = AParserFactory.CreateInstance(new ASTNodeFactory());
        }

        void CreateNodeList(string input)
        {
            _nodes = _parser.CreateNodeList(input);
        }

        [TestMethod]
        public void TestCreateExpressionList()
        {
            CreateNodeList("one two");
            _nodes.Count.Should().Be(2, "because there are two identifiers");
        }

        [TestMethod]
        public void TestCreateExpressionList_When_GroupedExpression()
        {
            CreateNodeList("one (two)");
            _nodes.Count.Should().Be(2, "because we have grouped expression");
            _nodes[1].SubNodes.Count.Should().Be(2, "because we have end-parentheses and one expression");
        }

        [TestMethod]
        public void TestCreateExpressionList_When_SeveralGroupedExpressions()
        {
            CreateNodeList("one (two three)");
            _nodes.Count.Should().Be(2, "because we have grouped expression");
            _nodes[1].SubNodes.Count.Should().Be(3, "because we have end-parentheses and two expressions");
        }

        [TestMethod]
        public void TestCreateExpressionList_When_NestedGroupedExpression()
        {
            CreateNodeList("one ((two))");
            _nodes.Count.Should().Be(2, "because we have grouped expression");
            _nodes[1].SubNodes.Count.Should().Be(2, "because we have end-parentheses and one expressions");
            _nodes[1].SubNodes[0].SubNodes.Count.Should().Be(2, "because innermost parentheses has one expressions (plus end-parentheses)");
        }

        [TestMethod]
        public void TestCreateExpressionList_When_NestedGroupedExpressionAndEmptyGroup()
        {
            CreateNodeList("one ((two()))");
            _nodes.Count.Should().Be(2, "because we have grouped expression");
            _nodes[1].SubNodes.Count.Should().Be(2, "because we have end-parentheses and one expressions");
            _nodes[1].SubNodes[0].SubNodes.Count.Should().Be(3, "because we have end-parentheses + two + ()");
            _nodes[1].SubNodes[0].SubNodes[1].SubNodes.Count.Should().Be(1, "because we only have end-parentheses");
        }

        [TestMethod]
        public void TestToIntFunction_When_String()
        {
            CreateNodeList("one (to_int('123'))");
            _nodes[1].SubNodes[0].SubNodes.Count.Should().Be(3, "because to_int function has one parameter (plus parentheses)");
            _nodes[1].SubNodes[0].Text.Should().Be("to_int", "because we to_int function");
        }

        [TestMethod]
        public void TestToIntFunction_When_Expression()
        {
            CreateNodeList("to_int('123' + '4')");
            _nodes.Count.Should().Be(1, "because we only have the to_int function");
            _nodes[0].Should().BeOfType <ASTToIntNode>();
            _nodes[0].SubNodes.Count.Should().Be(3, "because to_int function has one parameter (plus parentheses)");
            _nodes[0].SubNodes[1].SubNodes.Count.Should().Be(3, "because parameter consists of 3 elements");
        }

        [TestMethod]
        public void TestMinDate()
        {
            CreateNodeList("min_date");
            _nodes.Count.Should().Be(1, "because we only have the min_date keyword");
            _nodes[0].Should().BeOfType<ASTMinDateNode>();
            _nodes[0].SubNodes.Count.Should().Be(0, "because min_date has no parameters");
        }

        [TestMethod]
        public void TestMod()
        {
            CreateNodeList("mod(3, 3+2)");
            _nodes.Count.Should().Be(1, "because we only have the mod keyword");
            _nodes[0].Should().BeOfType<ASTModNode>();
            _nodes[0].SubNodes.Count.Should().Be(5, "because mod has two parameters (plus parentheses and comma)");
            _nodes[0].SubNodes[3].SubNodes.Count.Should().Be(3, "because second parameter to mod is an expression with three elements");
        }

        [TestMethod]
        public void TestToFloatFunction_When_String()
        {
            CreateNodeList("to_float('123.123456')");
            _nodes.Count.Should().Be(1, "because we only have the to_float keyword");
            _nodes[0].Should().BeOfType<ASTToFloatNode>();
            _nodes[0].SubNodes.Count.Should().Be(3, "because to_float has one parameters (plus parentheses)");
            _nodes[0].SubNodes[1].SubNodes[0].Text.Should().Be("'123.123456'", "because parameter to to_float is a string");
        }

        [TestMethod]
        public void TestGuid2Str()
        {
            CreateNodeList("guid2str(guid_col)");
            _nodes.Count.Should().Be(1, "because we only have the guid2str keyword");
            _nodes[0].Should().BeOfType<ASTGuid2StrNode>();
            _nodes[0].SubNodes.Count.Should().Be(3, "because guid2str has one parameters (plus parentheses)");
            _nodes[0].SubNodes[1].SubNodes[0].Text.Should().Be("guid_col", "because parameter to guid2str is a column name");
        }

        [TestMethod]
        public void TestDayAdd()
        {
            CreateNodeList("dayadd(3, day_col)");
            _nodes.Count.Should().Be(1, "because we only have the dayadd keyword");
            _nodes[0].Should().BeOfType<ASTDayAddNode>();
            _nodes[0].SubNodes.Count.Should().Be(5, "because dayadd has two parameters (plus parentheses and comma)");
            _nodes[0].SubNodes[3].SubNodes[0].Text.Should().Be("day_col", "because second parameter to dayadd is is a column name");
        }

        [TestMethod]
        public void TestToCounter()
        {
            CreateNodeList("to_counter('123456789')");
            _nodes.Count.Should().Be(1, "because we only have the to_counter keyword");
            _nodes[0].Should().BeOfType<ASTToCounterNode>();
            _nodes[0].SubNodes.Count.Should().Be(3, "because to_counter has one parameters (plus parentheses)");
            _nodes[0].SubNodes[1].SubNodes[0].Text.Should().Be("'123456789'", "because parameter to to_counter is a string");
        }

        [TestMethod]
        public void TestMonthAdd()
        {
            CreateNodeList("monthadd(3, month_col)");
            _nodes.Count.Should().Be(1, "because we only have the monthadd keyword");
            _nodes[0].Should().BeOfType<ASTMonthAddNode>();
            _nodes[0].SubNodes.Count.Should().Be(5, "because monthadd has two parameters (plus parentheses and comma)");
            _nodes[0].SubNodes[3].SubNodes[0].Text.Should().Be("month_col", "because second parameter to monthadd is is a column name");
        }
    }
}
