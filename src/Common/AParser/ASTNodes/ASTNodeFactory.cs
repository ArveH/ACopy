using System.Collections.Generic;

namespace AParser
{
    public class ASTNodeFactory: IASTNodeFactory
    {
        public Dictionary<string, IASTNode> KeyTokens { get; private set; }

        public ASTNodeFactory()
        {
            KeyTokens = new Dictionary<string, IASTNode>
            {
                {ASTCommaNode.KeyWord,            new ASTCommaNode(false)},
                {ASTDayAddNode.KeyWord,           new ASTDayAddNode(false)},
                {ASTEndParenthesesNode.KeyWord,   new ASTEndParenthesesNode(false)},
                {ASTGetDateNode.KeyWord,          new ASTGetDateNode(false)},
                {ASTGuid2StrNode.KeyWord,         new ASTGuid2StrNode(false)},
                {ASTIfNullNode.KeyWord,           new ASTIfNullNode(false)},
                {ASTMaxDateNode.KeyWord,          new ASTMaxDateNode(false)},
                {ASTMinDateNode.KeyWord,          new ASTMinDateNode(false)},
                {ASTModNode.KeyWord,              new ASTModNode(false)},
                {ASTMonthAddNode.KeyWord,         new ASTMonthAddNode(false)},
                {ASTStartParenthesesNode.KeyWord, new ASTStartParenthesesNode(false)},
                {ASTToCharNode.KeyWord,           new ASTToCharNode(false)},
                {ASTToCounterNode.KeyWord,        new ASTToCounterNode(false)},
                {ASTToFloatNode.KeyWord,          new ASTToFloatNode(false)},
                {ASTToIntNode.KeyWord,            new ASTToIntNode(false)}
            };
        }

        public IASTNode CreateNode(string text, bool addSpace=true)
        {
            IASTNode node;
            if (KeyTokens.TryGetValue(text.ToLower(), out node))
            {
                IASTNode newNode = node.CloneWithoutSubNodes();
                newNode.AddSpace = addSpace;
                return newNode;
            }
            return new ASTIdentifierNode(text, addSpace);
        }

        public IASTNode CreateFunctionParameterNode(bool addSpace = false)
        {
            return new ASTFunctionParameterNode(addSpace);
        }
    }
}
