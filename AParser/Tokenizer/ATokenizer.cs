using System;
using System.Text;

namespace AParser
{
    public class ATokenizer: IATokenizer
    {
        private const char QuoteChar = '\'';
        private int _parenthesesCounter;
        private int _curlyBracesCounter;
        private const int InitialStringBuilderLength = 20000;

        public ATokenizer()
        {
            Tokens = new ATokens();
        }

        public ATokens Tokens { get; private set; }

        private string _text = "";
        public string Text
        {
            get { return _text; }
        }

        IATokenFactory _tokenFactory;
        public IATokenFactory TokenFactory
        {
            get 
            { 
                return _tokenFactory ?? (_tokenFactory = new ATokenFactory()); 
            }
            set { _tokenFactory = value; }
        }

        public void Tokenize(string text)
        {
            _text = text;
            _parenthesesCounter = 0;
            _curlyBracesCounter = 0;
            int textPos = 0;
            IAToken token = GetNextToken(ref textPos);
            while (token != null)
            {
                Tokens.Add(token);
                token = GetNextToken(ref textPos);
            }

            CheckThatParenthesesMatch();
        }

        private void CheckThatParenthesesMatch()
        {
            if (_parenthesesCounter > 0)
            {
                throw new ATokenizerException(string.Format("Found {0} start parentheses without matching end parentheses", _parenthesesCounter));
            }
            if (_parenthesesCounter < 0)
            {
                throw new ATokenizerException(string.Format("Found {0} end parentheses without matching start parentheses", _parenthesesCounter));
            }

            if (_curlyBracesCounter > 0)
            {
                throw new ATokenizerException(string.Format("Found {0} start parentheses without matching end parentheses", _curlyBracesCounter));
            }
            if (_curlyBracesCounter < 0)
            {
                throw new ATokenizerException(string.Format("Found {0} end parentheses without matching start parentheses", _curlyBracesCounter));
            }
        }

        private bool _unicodeStringMark = true;
        public bool UnicodeStringMark
        {
            get { return _unicodeStringMark; }
            set { _unicodeStringMark = value; }
        }

        public bool ExpandEmptyStrings { get; set; }

        private bool IsUnicodeString(int textPos)
        {
            return textPos + 1 < _text.Length && _text[textPos] == 'N' && _text[textPos + 1] == QuoteChar;
        }

        private IAToken GetNextToken(ref int textPos)
        {
            SkipWhiteSpace(ref textPos);
            if (textPos >= _text.Length)
            {
                return null;
            }

            char currentChar = _text[textPos];
            if (IsUnicodeString(textPos) && UnicodeStringMark)
            {
                textPos++;
                return GetStringToken(ref textPos);
            }
            if (IsLegalNameChar(currentChar))
            {
                return GetIdentifierToken(ref textPos);
            }
            if (currentChar == '(')
            {
                return GetParentheses(ref textPos);
            }
            if (currentChar == ')')
            {
                return GetEndParentheses(ref textPos);
            }
            if (currentChar == '{')
            {
                RemoveFunctionWrapper(ref textPos);
                return GetNextToken(ref textPos);
            }
            if (currentChar == '}')
            {
                RemoveEndFunctionWrapper(ref textPos);
                return GetNextToken(ref textPos);
            }
            if (currentChar == QuoteChar)
            {
                return GetStringToken(ref textPos);
            }
            if (currentChar == '"')
            {
                return GetQuotedIdentifier(ref textPos, '"');
            }
            if (currentChar == '[')
            {
                return GetQuotedIdentifier(ref textPos, ']');
            }
            return CreateToken(textPos, ++textPos);
        }

        private IAToken CreateToken(int startPos, int endPos)
        {
            IAToken token = TokenFactory.CreateToken(_text.Substring(startPos, endPos - startPos));
            SetAddSpaceFlag(token, endPos);
            return token;
        }

        private IAToken GetIdentifierToken(ref int textPos)
        {
            int startPos = textPos;
            textPos = GetEndPositionOfCurrentToken(textPos);
            return CreateToken(startPos, textPos);
        }

        private int GetEndPositionOfCurrentToken(int textPos)
        {
            while (textPos < _text.Length && IsLegalNameChar(_text[textPos]))
            {
                textPos++;
            }
            return textPos;
        }

        private void SetAddSpaceFlag(IAToken token, int textPos)
        {
            token.AddSpace = textPos < _text.Length && IsWhitespace(_text[textPos]);
        }

        private IAToken GetParentheses(ref int textPos)
        {
            _parenthesesCounter++;
            return CreateToken(textPos, ++textPos);
        }

        private IAToken GetEndParentheses(ref int textPos)
        {
            _parenthesesCounter--;
            return CreateToken(textPos, ++textPos);
        }

        private void RemoveFunctionWrapper(ref int textPos)
        {
            _curlyBracesCounter++;
            textPos++;
            SkipWhiteSpace(ref textPos);
            textPos = GetEndPositionOfCurrentToken(textPos);
        }

        private void RemoveEndFunctionWrapper(ref int textPos)
        {
            _curlyBracesCounter--;
            textPos++;
        }

        private IAToken GetQuotedIdentifier(ref int textPos, char endQuote)
        {
            int startPos = textPos;
            textPos++;
            while (textPos < _text.Length)
            {
                if (_text[textPos] == endQuote)
                {
                    break;
                }
                textPos++;
            }

            if (textPos == _text.Length)
            {
                throw new ATokenizerException(string.Format("Missing terminating quote for quoted identifier starting at position {0}", startPos));
            }

            textPos++;
            IAToken token = TokenFactory.CreateToken(_text.Substring(startPos, textPos - startPos));
            SetAddSpaceFlag(token, textPos);

            return token;
        }

        private IAToken GetStringToken(ref int textPos)
        {
            int startPos = textPos;
            textPos++;
            while (textPos < _text.Length)
            {
                if (_text[textPos] == QuoteChar)
                {
                    if (textPos + 1 < _text.Length && _text[textPos+1] == QuoteChar)
                    {
                        textPos += 2;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    textPos++;
                }
            }

            if (textPos == _text.Length)
            {
                throw new ATokenizerException(string.Format("Missing terminating quote for string starting at position {0}", startPos));
            }

            textPos++;
            IAToken token = TokenFactory.CreateToken(_text.Substring(startPos, textPos - startPos));
            if (ExpandEmptyStrings && token.Text == "''")
            {
                token.Text = "' '";
            }
            SetAddSpaceFlag(token, textPos);

            return token;
        }

        private void SkipWhiteSpace(ref int textPos)
        {
            while (textPos < _text.Length && IsWhitespace(_text[textPos]))
            {
                textPos++;
            }
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder("", InitialStringBuilderLength);
            foreach (var token in Tokens)
            {
                text.Append(token.Text);
                if (token.AddSpace)
                {
                    text.Append(" ");
                }
            }
            return text.ToString();
        }

        #region Character methods
        private static bool IsLegalNameChar(char c)
        {
            return Char.IsLetterOrDigit(c) || c == '_';
        }

        private static bool IsWhitespace(char c)
        {
            return Char.IsWhiteSpace(c);
        }
        #endregion
    }
}
