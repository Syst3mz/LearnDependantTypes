using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace LearnDependantTypes
{
    public class Parser
    {
        private ParserStream _ps;
        public Parser(List<Token> toParse)
        {
            _ps = new ParserStream(toParse);
        }

        public Parser(ParserStream ps)
        {
            _ps = ps;
        }
        
        private List<Token> ParseList(TokenType openType, TokenType delimType, TokenType closedType)
        {
            if (!_ps.Match(openType, out Token? openTok))
            {
                throw new ParserError($"Expected a {openType} to start list.", _ps.Current);
            }

            List<Token> ret = new List<Token>();
            
            while (!_ps.Match(closedType))
            {
                if (_ps.Current.Type == TokenType.Eof)
                {
                    Debug.Assert(openTok != null, nameof(openTok) + " != null");
                    throw new ParserError("Found end of file while parsing list starting at ", openTok.Value);
                }
                else
                {
                    ret.Add(_ps.Current);
                    _ps.Next();
                    if (!_ps.Match(delimType, out Token? errorDelim))
                    {
                        throw new ParserError($"Found {_ps.Current} but expected {delimType} as list delimiter",
                            _ps.Current);
                    }
                }
            }

            return ret;
        }

        public List<IAstTopLevel> Parse()
        {
            List<IAstTopLevel> topLevels = new List<IAstTopLevel>();
            while (!_ps.Done)
            {
                topLevels.Add(ParseTopLevel());
            }

            return topLevels;
        }

        private FnDeclTopLevel ParseTopLevel()
        {
            if (_ps.Match(TokenType.Fn, out Token? fnTok))
            {
                if (!_ps.Match(TokenType.Identifier, out Token? fnNameTok))
                {
                    throw new ParserError($"Expected identifier token after \"fn\"", _ps.Current);
                }

                Identifier fnName = new Identifier(fnNameTok.Value);
                if (!_ps.Expect(TokenType.LParen, out Token? argListOpen))
                {
                    throw new ParserError($"Expected open paren after function name.", _ps.Current);
                }

                List<Token> args = ParseList(TokenType.LParen, TokenType.Comma, TokenType.RParen);

                if (!_ps.Match(TokenType.Arrow))
                {
                    throw new ParserError($"Expected arrow and return type, but found {_ps.Current.Type}", _ps.Current);
                }

                if (!_ps.Match(TokenType.Identifier, out Token? retType))
                {
                    throw new ParserError($"Expected return type at {_ps.Current}", _ps.Current);
                }

                List<(Token, Token?)> argsTuples = new List<(Token, Token?)>();

                return new FnDeclTopLevel(new FnDecl(fnName, args, ParseBlock(), retType));
            }
            else if (_ps.Match(TokenType.Struct))
            {
                throw new NotImplementedException("I didn't make structs yet.");
            }

            throw new ParserError($"Unexpected {_ps.Current.Type} at top level.", _ps.Current);
        }

        private Block ParseBlock()
        {
            if (!_ps.Match(TokenType.LCurlyBrace, out Token? openTok))
            {
                throw new ParserError("Expected opening \"{\" to start block, but found" + $"{_ps.Current.Type}", _ps.Current);
            }

            List<IAstStatement> statements = new List<IAstStatement>();
            while (!_ps.Match(TokenType.RCurlyBrace))
            {
                if (_ps.Match(TokenType.Eof))
                {
                    throw new ParserError("Found end of file while parsing block starting at ", openTok.Value);
                }
                else
                {
                    statements.Add(ParseStatement());
                }
            }

            return new Block(statements);
        }

        private IAstStatement ParseStatement()
        {
            throw new NotImplementedException();
        }

        private IAstExpr ParseExpr()
        {
            throw new NotImplementedException();
        }
    }
}