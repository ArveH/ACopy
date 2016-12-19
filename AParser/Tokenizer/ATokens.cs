using System.Collections.Generic;

namespace AParser
{
    public class ATokens: List<IAToken>
    {
        private int _currentToken;

        public void ResetPosition()
        {
            _currentToken = 0;
        }

        public IAToken CurrentToken
        {
            get
            {
                if (_currentToken < Count)
                {
                    return this[_currentToken];
                }
                return null;
            }
        }

        public IAToken GetNextToken()
        {
            if (_currentToken < Count)
            {
                _currentToken++;
            }

            return CurrentToken;
        }
    }
}
