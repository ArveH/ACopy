using System.Collections.Generic;

namespace AParser
{
    public class SqlServerGetDateTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            Translator = translator;
            NodeFactory = nodeFactory;

            AddFunction(newNodes, "getdate", new List<IASTNode>());
        }
    }
}
