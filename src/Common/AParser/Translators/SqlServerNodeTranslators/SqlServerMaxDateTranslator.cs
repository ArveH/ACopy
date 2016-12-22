using System.Collections.Generic;

namespace AParser
{
    public class SqlServerMaxDateTranslator : ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // Translated to convert(datetime, '20991231 23:59:59:998',9)
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                nodeFactory.CreateNode("datetime", false),
                nodeFactory.CreateNode("'20991231 23:59:59:998'", false),
                nodeFactory.CreateNode("9", false)
            };

            AddFunction(newNodes, "convert", functionParameters);
        }
    }
}
