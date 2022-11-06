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
        
        private List<T> ParseList<T>(TokenType openType, Func<T> parseContained, TokenType closedType)
        {
            if (!_ps.Match(openType, out Token? openTok))
            {
                throw new ParserError($"Expected a {openType} to start list.", _ps.Current);
            }

            List<T> ret = new List<T>();
            
            while (!_ps.Match(closedType))
            {
                if (_ps.Current.Type == TokenType.Eof)
                {
                    Debug.Assert(openTok != null, nameof(openTok) + " != null");
                    throw new ParserError("Found end of file while parsing list starting at ", openTok.Value);
                }
                else
                {
                    ret.Add(parseContained());
                    _ps.Next();
                    if (!_ps.Match(TokenType.Comma, out Token? errorDelim) && !_ps.Expect(closedType))
                    {
                        throw new ParserError($"Found {_ps.Current} but expected ',' as list delimiter",
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
                
                
                // parse argument list
                if (!_ps.Match(TokenType.LParen, out Token? argOpener))
                {
                    throw new ParserError($"Expected a {TokenType.LParen} to start parameter list.", _ps.Current);
                }

                List<(Identifier, Identifier)> paramsList = new List<(Identifier, Identifier)>();
                while (!_ps.Match(TokenType.RParen))
                {
                    if (!_ps.Match(TokenType.Identifier, out Token? paramName))
                    {
                        throw new ParserError($"Expected a {TokenType.Identifier} for parameter name.", _ps.Current);
                    }

                    if (!_ps.Match(TokenType.Colon))
                    {
                        throw new ParserError($"Expected a {TokenType.Colon} for parameter type annotation.", _ps.Current);
                    }

                    if (!_ps.Match(TokenType.Identifier, out Token? annotation))
                    {
                        throw new ParserError($"Expected a {TokenType.Identifier} as parameter type annotation.", _ps.Current);
                    }
                    
                    paramsList.Add((new Identifier(paramName.Value), new Identifier(annotation.Value)));
                    
                    if (!_ps.Match(TokenType.Comma) && !_ps.Expect(TokenType.RParen))
                    {
                        throw new ParserError($"Expected a {TokenType.Comma} to delimit the list", _ps.Current);
                    }
                }
                
                if (!_ps.Match(TokenType.Arrow))
                {
                    throw new ParserError($"Expected arrow and return type, but found {_ps.Current.Type}", _ps.Current);
                }

                if (!_ps.Match(TokenType.Identifier, out Token? retType))
                {
                    throw new ParserError($"Expected return type at {_ps.Current}", _ps.Current);
                }
                

                return new FnDeclTopLevel(new FnDecl(fnName, paramsList, ParseBlock(), new Identifier(retType.Value)));
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
            if (_ps.Match(TokenType.Return, out Token? retStart))
            {
                var expr = ParseExpr();
                if (!_ps.Match(TokenType.SemiColon))
                {
                    throw new ParserError($"Expected {TokenType.SemiColon} after return statement.", _ps.Current);
                }
                return new Return(expr);
            }
            else if (_ps.Match(TokenType.Var, out Token? varStart))
            {
                if (!_ps.Match(TokenType.Identifier, out Token? nameTok))
                {
                    throw new ParserError($"Expected identifier after {TokenType.Var} but found {_ps.Current.Type}", _ps.Current);
                }

                Token? tAnnTok = null;
                
                // Handle optional type annotation
                if (_ps.Match(TokenType.Colon))
                {
                    if (!_ps.Match(TokenType.Identifier, out tAnnTok))
                    {
                        throw new ParserError($"Expected identifier after {TokenType.Colon} for type annotation but found {_ps.Current.Type}", _ps.Current);
                    }
                }
                
                if (!_ps.Match(TokenType.Equals))
                {
                    throw new ParserError($"Expected {TokenType.Equals} after variable decl at ", varStart.Value);
                }

                return new VarDecl(new Identifier(nameTok.Value), tAnnTok.HasValue? new Identifier(tAnnTok.Value): null, ParseExpr());
            }
            else
            {
                return new ExprStatement(ParseExpr());
            }
        }

        private IAstExpr ParseExpr()
        {
            return ParseVarSet();
        }

        private IAstExpr ParseVarSet()
        {
            var expr = ParseFunctionCall();
            if (_ps.Match(TokenType.Equals))
            {
                var right = ParseVarSet();
                if (expr is VarGet vg)
                {
                    expr = new VarSet(vg.Name, right);
                }
            }

            return expr;
        }
        
        private IAstExpr ParseFunctionCall()
        {
            var maybeFunction = ParseIf();

            if (maybeFunction is VarGet vg)
            {
                if (_ps.Expect(TokenType.LParen))
                {
                    List<IAstExpr> argList = ParseList(TokenType.LParen, ParseExpr, TokenType.RParen);
                    maybeFunction = new FuncCall(maybeFunction, argList);
                }
            }
            
            return maybeFunction;
        }

        private IAstExpr ParseIf()
        {
            if (_ps.Match(TokenType.If, out Token? ifStartTok))
            {
                var conditional = ParseBopPrece1();
                var block = ParseBlock();
                if (_ps.Match(TokenType.Else, out Token? elseTok))
                {
                    var elseBlock = ParseBlock();
                    return new IfElse(ifStartTok.Value, conditional, block, elseTok.Value, elseBlock);
                }

                return new IfElse(ifStartTok.Value, conditional, block);
            }
            else
            {
                return ParseBopPrece1();
            }
        }

        private IAstExpr ParseBopPrece1()
        {
            IAstExpr left = ParseBopPrece2();
            while (true)
            {
                if (_ps.Match(TokenType.EqualsEquals, out Token? eqTok))
                {
                    left = new BinaryOperation(eqTok.Value, left, ParseAtom(), BinaryOperation.Bop.Equals);
                }
                else if (_ps.Match(TokenType.BangEquals, out Token? bangTok))
                {
                    left = new BinaryOperation(bangTok.Value, left, ParseAtom(), BinaryOperation.Bop.NotEquals);
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        private IAstExpr ParseBopPrece2()
        {
            IAstExpr left = ParseAtom();
            while (true)
            {
                if (_ps.Match(TokenType.Plus, out Token? plusTok))
                {
                    left = new BinaryOperation(plusTok.Value, left, ParseAtom(), BinaryOperation.Bop.Plus);
                }
                else if (_ps.Match(TokenType.Minus, out Token? minusTok))
                {
                    left = new BinaryOperation(minusTok.Value, left, ParseAtom(), BinaryOperation.Bop.Minus);
                }
                else
                {
                    break;
                }
            }

            return left;
        }

        private IAstExpr ParseAtom()
        {
            if (_ps.Match(TokenType.Identifier, out Token? identTok))
            {
                return new VarGet(new Identifier(identTok.Value));
            }
            else if (_ps.Match(TokenType.Integer, out Token? intTok))
            {
                return new Integer(intTok.Value, long.Parse(intTok.Value.Lexeme));
            }
            else if (_ps.Match(TokenType.Boolean, out Token? boolTok))
            {
                return new Boolean(boolTok.Value, bool.Parse(boolTok.Value.Lexeme));
            }
            else if (_ps.Match(TokenType.LParen, out Token? parenStart))
            {
                var inner = ParseExpr();
                if (!_ps.Match(TokenType.RParen))
                {
                    throw new ParserError($"Unclosed parenthesis starting at", parenStart.Value);
                }

                return inner;
            }
            else if (_ps.Match(TokenType.Minus, out Token? minusTok))
            {
                return new UnaryOperation(minusTok.Value, ParseExpr(), UnaryOperation.Uop.MakeNegative);
            }
            else if (_ps.Match(TokenType.Bang, out Token? bangTok))
            {
                return new UnaryOperation(bangTok.Value, ParseExpr(), UnaryOperation.Uop.Negate);
            }

            throw new ParserError($"Unexpect {_ps.Current} in token stream at ", _ps.Current);
        }
    }
}