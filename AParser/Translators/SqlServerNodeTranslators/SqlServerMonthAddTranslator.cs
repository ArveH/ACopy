using System.Collections.Generic;

namespace AParser
{
    public class SqlServerMonthAddTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // MONTHADD(3, today) => dateadd(mm, 3, today)
            Translator = translator;
            NodeFactory = nodeFactory;

            IASTNode mmParameter = NodeFactory.CreateFunctionParameterNode();
            mmParameter.SubNodes.Add(NodeFactory.CreateNode("mm", false));

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                mmParameter,
                CreateFunctionParameterNodeWithSubNodes(subNodes[1]),
                CreateFunctionParameterNodeWithSubNodes(subNodes[3])
            };

            AddFunction(newNodes, "dateadd", functionParameters);
        }
    }
}
