using System.Text;

namespace AParser
{
    public abstract class ASTNodeBase: IASTNode
    {
        protected ASTNodeBase()
        {
        }

        public bool AddSpace { get; set; }
        public string Text { get; set; }

        public string TextUsingAddSpace
        {
            get { return Text + (AddSpace ? " " : ""); }
        }

        private ASTNodeList _subNodes;
        public ASTNodeList SubNodes
        {
            get 
            {
                return _subNodes ?? (_subNodes = new ASTNodeList()); 
            }
        }

        public bool TextEqualTo(string text, bool ignoreCase = true)
        {
            return string.Compare(text, Text, ignoreCase) == 0;
        }

        protected ASTNodeBase(string text, bool addSpace)
        {
            Text = text;
            AddSpace = addSpace;
        }

        public virtual IASTNode Parse(IASTNodeFactory nodeFactory, IAParser aParser, ATokens tokens)
        {
            tokens.GetNextToken();
            return this;
        }

        public void ResetSubNodes()
        {
            _subNodes = null;
        }

        public IASTNode CloneWithoutSubNodes()
        {
            IASTNode node = (IASTNode)MemberwiseClone();
            node.ResetSubNodes();

            return node;
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder(TextUsingAddSpace);
            if (_subNodes != null)
            {
                foreach (var node in _subNodes)
                {
                    text.Append(node.ToString());
                }
            }

            return text.ToString();
        }
    }
}
