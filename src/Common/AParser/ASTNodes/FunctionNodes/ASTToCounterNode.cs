namespace AParser
{
    public class ASTToCounterNode: ASTFunctionNode
    {
        public ASTToCounterNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "to_counter"; } }
    }
}
