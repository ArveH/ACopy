using System.Collections.Generic;

namespace AParser
{
    public class SqlServerIfNullTranslator : ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // IFNULL(somecol, 0) => isnull(somecol, 0)
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                CreateFunctionParameterNodeWithSubNodes(subNodes[1]),
                CreateFunctionParameterNodeWithSubNodes(subNodes[3])
            };

            AddFunction(newNodes, "isnull", functionParameters);
        }
    }
}
