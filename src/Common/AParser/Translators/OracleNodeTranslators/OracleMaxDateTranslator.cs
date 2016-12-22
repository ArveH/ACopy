using System.Collections.Generic;

namespace AParser
{
    public class OracleMaxDateTranslator : ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // Translated to to_date('20991231 23:59:59', 'yyyymmdd hh24:mi:ss')
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                nodeFactory.CreateNode("'20991231 23:59:59'", false),
                nodeFactory.CreateNode("'yyyymmdd hh24:mi:ss'", false)
            };

            AddFunction(newNodes, "to_date", functionParameters);
        }
    }
}
