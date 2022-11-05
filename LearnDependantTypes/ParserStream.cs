using System.Collections.Generic;

namespace LearnDependantTypes
{
    public class ParserStream
    {
        private List<Token> _toParse;
        private int _idx = 0;
        private Token _current => _toParse[_idx];

        public ParserStream(List<Token> toParse)
        {
            _toParse = toParse;
        }

        public void Next()
        {
            _idx++;
        }

        public Token? Peek(int by)
        {
            int at = _idx + by;
            if (at < 0 || at >= _toParse.Count)
            {
                return null;
            }

            return _toParse[at];
        }
        
        public bool Expect(TokenType t)
        {
            if (_current.Type == t)
            {
                return true;
            }

            return false;
        }
        
        public bool Expect(TokenType t, out Token? tok)
        {
            if (Expect(t))
            {
                tok = _current;
                return true;
            }

            tok = null;
            return false;
        }
        
        public bool Match(TokenType t)
        {
            if (Expect(t))
            {
                Next();
                return true;
            }

            return false;
        }
        
        public bool Match(TokenType t, out Token? tok)
        {
            if (Expect(t))
            {
                tok = _current;
                Next();
                return true;
            }

            tok = null;
            return false;
        }
    }    
}