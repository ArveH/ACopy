using System.Collections.Generic;

namespace AParser
{
    public class OracleMinDateTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // Translated to to_date('19000101 00:00:00', 'yyyymmdd hh24:mi:ss')
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                nodeFactory.CreateNode("'19000101 00:00:00'", false),
                nodeFactory.CreateNode("'yyyymmdd hh24:mi:ss'", false)
            };

            AddFunction(newNodes, "to_date", functionParameters);
        }
    }
}
