using System;
using System.Collections.Generic;

namespace LearnDependantTypes
{
    public class Lexer
    {
        private string _toLex;

        public Lexer(string toLex)
        {
            _toLex = toLex;
        }

        private static Dictionary<string, TokenType> _staticTokens = new Dictionary<string, TokenType>()
        {
            {"struct", TokenType.Struct},
            {"if", TokenType.If},
            {"else", TokenType.Else},
            {"return", TokenType.Return},
            {"fn", TokenType.Fn},
            {"var", TokenType.Var},
            {"+", TokenType.Plus},
            {"-", TokenType.Minus},
            {"==", TokenType.EqualsEquals},
            {"!=", TokenType.BangEquals},
            {"!", TokenType.Bang},
            {"=", TokenType.Equals},
            {"where", TokenType.Where},
            {"true", TokenType.Boolean},
            {"false", TokenType.Boolean},
            {"{", TokenType.LCurlyBrace},
            {"}", TokenType.RCurlyBrace},
            {"(", TokenType.LParen},
            {")", TokenType.RParen},
            {",", TokenType.Comma},
            {";", TokenType.SemiColon},
            {"print", TokenType.Print},
            {"->", TokenType.Arrow}
        };

        public List<Token> Lex()
        {
            List<Token> ret = new List<Token>();
            int idx = 0;
            int linePos = 1, charPos = 1;
            while (idx < _toLex.Length)
            {
                char cur = _toLex[idx];
                if (char.IsWhiteSpace(cur))
                {
                    charPos++;
                    idx++;
                }
                else if (cur == '\n')
                {
                    charPos = 1;
                    linePos++;
                    idx++;
                }
                else
                {
                    bool isStatic = false;
                    foreach (var staticToken in _staticTokens)
                    {
                        if ((_toLex.Length - idx) >= staticToken.Key.Length && _toLex.Substring(idx, staticToken.Key.Length).Equals(staticToken.Key))
                        {
                            ret.Add(new Token(staticToken.Value, staticToken.Key, linePos, charPos));
                            charPos += staticToken.Key.Length;
                            idx += staticToken.Key.Length;
                            isStatic = true;
                            break;
                        }
                    }

                    if (!isStatic)
                    {
                        // handle ids
                        if (char.IsLetter(cur) || cur == '_')
                        {
                            string idName = "";
                            idx++;
                            while (idx < _toLex.Length)
                            {
                                if (char.IsLetterOrDigit(_toLex[idx]) || _toLex[idx] == '_')
                                {
                                    idName += _toLex[idx];
                                    idx++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ret.Add(new Token(TokenType.Identifier, idName, linePos, charPos));
                            charPos += idName.Length;
                        }
                        else if (char.IsDigit(cur))
                        {
                            string number = "" + cur;
                            idx++;
                            while (idx < _toLex.Length)
                            {
                                if (char.IsDigit(_toLex[idx]))
                                {
                                    number += _toLex[idx];
                                    idx++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            ret.Add(new Token(TokenType.Integer, number, linePos, charPos));
                            charPos += number.Length;
                        }
                    }
                }
                
            }
            ret.Add(new Token(TokenType.Eof, "", linePos, charPos));
            return ret;
        }
    }

    public class LexerError : Exception
    {
        public int LinePos, CharPos;

        public LexerError(string message, int linePos, int charPos) : base(message)
        {
            LinePos = linePos;
            CharPos = charPos;
        }
    }
}