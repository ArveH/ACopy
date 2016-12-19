using System.Collections.Generic;

namespace AParser
{
    public abstract class ASTNodeTranslator: IASTNodeTranslator
    {
        protected IASTNodeFactory NodeFactory;
        protected IATranslator Translator;

        public abstract void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory);

        protected void AddFunction(ASTNodeList newNodes, string functionName, IASTNode functionParameter1, IASTNode functionParameter2)
        {
            AddFunction(newNodes, functionName, new List<IASTNode> { functionParameter1, functionParameter2 });
        }

        protected void AddFunction(ASTNodeList newNodes, string functionName, IASTNode functionParameter)
        {
            AddFunction(newNodes, functionName, new List<IASTNode> { functionParameter });
        }

        protected void AddFunction(ASTNodeList newNodes, string functionName, List<IASTNode> functionParameters)
        {
            IASTNode oraFunctionNode = NodeFactory.CreateNode(functionName, false);
            AddFunctionParameters(functionParameters, oraFunctionNode);

            newNodes.Add(oraFunctionNode);
        }

        private void AddFunctionParameters(List<IASTNode> functionParameters, IASTNode oraFunctionNode)
        {
            oraFunctionNode.SubNodes.Add(NodeFactory.CreateNode(ASTStartParenthesesNode.KeyWord, false));
            for (int i = 0; i < functionParameters.Count; i++)
            {
                if (i > 0)
                {
                    oraFunctionNode.SubNodes.Add(NodeFactory.CreateNode(ASTCommaNode.KeyWord));
                }
                oraFunctionNode.SubNodes.Add(functionParameters[i]);
            }
            oraFunctionNode.SubNodes.Add(NodeFactory.CreateNode(ASTEndParenthesesNode.KeyWord));
        }

        protected IASTNode CreateFunctionParameterNode(string text)
        {
            IASTNode functionParameterNode = NodeFactory.CreateFunctionParameterNode();
            functionParameterNode.SubNodes.Add(NodeFactory.CreateNode(text, false));

            return functionParameterNode;
        }

        protected IASTNode CreateFunctionParameterNodeWithSubNodes(IASTNode functionParameterNode)
        {
            ASTNodeList tmpSubNodes = new ASTNodeList();
            Translator.TranslateSubNodes(tmpSubNodes, functionParameterNode);

            return tmpSubNodes[0];
        }
    }
}
