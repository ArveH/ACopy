using System.Collections.Generic;

namespace AParser
{
    public class ATranslator: IATranslator
    {
        protected IASTNodeFactory NodeFactory;
        protected Dictionary<string, IASTNodeTranslator> NodeTranslators;

        public ASTNodeList Translate(ASTNodeList nodes)
        {
            ASTNodeList newNodes = new ASTNodeList();
            foreach (var node in nodes)
            {
                TranslateSubNodes(newNodes, node);
            }

            return newNodes;
        }

        public void TranslateSubNodes(ASTNodeList newNodes, IASTNode fromNode)
        {
            IASTNodeTranslator nodeTranslator;
            if (!string.IsNullOrEmpty(fromNode.Text) && NodeTranslators.TryGetValue(fromNode.Text, out nodeTranslator)) // Text property may add a space at the end, depending on the setting of AddSpace, so need to be trimmed
            {
                nodeTranslator.Translate(this, newNodes, fromNode.SubNodes, NodeFactory);
                return;
            }

            IASTNode newNode = fromNode.CloneWithoutSubNodes();
            newNodes.Add(newNode);
            if (fromNode.SubNodes == null || fromNode.SubNodes.Count == 0)
            {
                return;
            }

            foreach (var node in fromNode.SubNodes)
            {
                TranslateSubNodes(newNode.SubNodes, node);
            }
        }
    }
}
