namespace AParser
{
    public class AToken: IAToken
    {
        public AToken(string text, bool addSpace=true)
        {
            Text = text;
            AddSpace = addSpace;
        }

        public string Text { get; set; }

        public bool AddSpace { get; set; }
    }
}
