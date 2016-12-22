using System.Collections.Generic;

namespace AParser
{
    public class OracleMonthAddTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // MONTHADD(3, today) => add_months(today, 3)
            Translator = translator;
            NodeFactory = nodeFactory;

            List<IASTNode> functionParameters = new List<IASTNode>
            {
                CreateFunctionParameterNodeWithSubNodes(subNodes[3]),
                CreateFunctionParameterNodeWithSubNodes(subNodes[1])
            };

            AddFunction(newNodes, "add_months", functionParameters);
        }
    }
}
