using System.Collections.Generic;

namespace AParser
{
    public class OracleGuid2StrTranslator: ASTNodeTranslator
    {
        public override void Translate(IATranslator translator, ASTNodeList newNodes, ASTNodeList subNodes, IASTNodeFactory nodeFactory)
        {
            // The guid2str function is pretty ugly in Oracle. A call like guid2str(guid_col) becomes:
            //     lower(substr(guid_col,1,8) || '-' || substr(guid_col,9,4) || '-' || substr(guid_col,13,4) || '-' || substr(guid_col,17,4) || '-' || substr(guid_col,21,12))
            Translator = translator;
            NodeFactory = nodeFactory;

            IASTNode newNode = NodeFactory.CreateNode("lower", false);
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTStartParenthesesNode.KeyWord, false));
            newNode.SubNodes.Add(NodeFactory.CreateFunctionParameterNode());
            newNode.SubNodes.Add(NodeFactory.CreateNode(ASTEndParenthesesNode.KeyWord));

            CreateNodesForGuidParts(newNode.SubNodes[1].SubNodes, subNodes[1]);

            newNodes.Add(newNode);
        }

        private void CreateNodesForGuidParts(ASTNodeList newNodes, IASTNode parameter)
        {
            CreateSubStringNode (newNodes, parameter, "1", "8");
            CreateSpearatorNodes(newNodes);
            CreateSubStringNode (newNodes, parameter, "9", "4");
            CreateSpearatorNodes(newNodes);
            CreateSubStringNode (newNodes, parameter, "13", "4");
            CreateSpearatorNodes(newNodes);
            CreateSubStringNode (newNodes, parameter, "17", "4");
            CreateSpearatorNodes(newNodes);
            CreateSubStringNode(newNodes, parameter, "21", "12");
        }

        private void CreateSubStringNode(ASTNodeList newNodes, IASTNode parameter, string start, string length)
        {
            List<IASTNode> functionParameters = new List<IASTNode>
            {
                CreateFunctionParameterNodeWithSubNodes(parameter),
                CreateFunctionParameterNode(start),
                CreateFunctionParameterNode(length)
            };
            AddFunction(newNodes, "substr", functionParameters);
        }

        private void CreateSpearatorNodes(ASTNodeList newNodes)
        {
            newNodes.Add(NodeFactory.CreateNode("||"));
            newNodes.Add(NodeFactory.CreateNode("'-'"));
            newNodes.Add(NodeFactory.CreateNode("||"));
        }
    }
}
