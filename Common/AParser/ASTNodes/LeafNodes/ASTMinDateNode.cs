namespace AParser
{
    public class ASTMinDateNode: ASTNodeBase
    {
        public ASTMinDateNode(bool addSpace = true)
        {
            Text = KeyWord;
            AddSpace = addSpace;
        }

        public static string KeyWord { get { return "min_date"; } }
    }
}
