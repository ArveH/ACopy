using System.Collections.Generic;

namespace AParser
{
    public class SqlServerMinDateTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // Translated to convert(datetime, '19000101 00:00:00:000', 9)
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                nodeFactory.CreateNode("datetime", false),
                nodeFactory.CreateNode("'19000101 00:00:00:000'", false),
                nodeFactory.CreateNode("9", false)
            };

            AddFunction(newNodes, "convert", functionParameters);
        }
    }
}
