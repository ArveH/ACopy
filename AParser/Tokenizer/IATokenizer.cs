namespace AParser
{
    public interface IATokenizer
    {
        string Text { get; }
        ATokens Tokens { get; }
        IATokenFactory TokenFactory { get; set; }
        void Tokenize(string text);
        string ToString();
        bool UnicodeStringMark { get; set; } // If true, treat N'string' as one token, otherwise it's two tokens. Default is true.
        bool ExpandEmptyStrings { get; set; } // If true, put one space into empty strings. You may want to do this since empty string in Oracle is NULL, 
                                              // and often people compares to NULL instead of empty string without knowing it. Default is false.
    }
}
