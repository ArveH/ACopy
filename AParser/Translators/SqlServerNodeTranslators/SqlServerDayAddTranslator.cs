using System.Collections.Generic;

namespace AParser
{
    public class SqlServerDayAddTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // DAYADD(3, day_col) => dateadd(dd, 3, day_col)
            Translator = translator;
            NodeFactory = nodeFactory;

            IASTNode unitParameter = NodeFactory.CreateFunctionParameterNode();
            unitParameter.SubNodes.Add(NodeFactory.CreateNode("dd", false));

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                unitParameter,
                CreateFunctionParameterNodeWithSubNodes(subNodes[1]),
                CreateFunctionParameterNodeWithSubNodes(subNodes[3])
            };

            AddFunction(newNodes, "dateadd", functionParameters);
        }
    }
}
