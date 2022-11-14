using System.Collections.Generic;
using LearnDependantTypes.Lexing;

namespace LearnDependantTypes.Parsing
{
    public class ParserStream
    {
        private List<Token> _toParse;
        private int _idx = 0;
        public bool Done => _idx >= _toParse.Count;
        public Token Current => _idx<_toParse.Count? _toParse[_idx]: new Token(TokenType.Eof, "", -1, -1);

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
            if (at < 0)
            {
                return null;
            } else if (at >= _toParse.Count)
            {
                return new Token(TokenType.Eof, "", -1, -1);
            }

            return _toParse[at];
        }
        
        public bool Expect(TokenType t)
        {
            if (Current.Type == t)
            {
                return true;
            }

            return false;
        }
        
        public bool Expect(TokenType t, out Token? tok)
        {
            if (Expect(t))
            {
                tok = Current;
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
                tok = Current;
                Next();
                return true;
            }

            tok = null;
            return false;
        }
    }    
}