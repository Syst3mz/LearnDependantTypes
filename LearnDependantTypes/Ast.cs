using System.Collections.Generic;
using LearnDependantTypes.Lexing;

namespace LearnDependantTypes
{
    public interface IAstNode
    {
        public Location Location { get; }
    }

    public interface IAstTopLevel : IAstNode
    {
        
    }
    
    public interface IAstStatement : IAstNode
    {
    }

    public interface IAstExpr : IAstNode
    {
    }

    public struct Struct : IAstTopLevel
    {
        //todo deal with later
        public Location Location { get; set; }
    }

    public struct FnDeclTopLevel : IAstTopLevel
    {
        public FnDecl Function;
        public Location Location => Function.Location;

        public FnDeclTopLevel(FnDecl function)
        {
            Function = function;
        }
    }
    
    public struct Block : IAstStatement
    {
        public Location Location => Statements[0].Location;
        public List<IAstStatement> Statements;

        public Block(List<IAstStatement> statements)
        {
            Statements = statements;
        }
    }
    
    public struct FnDecl : IAstStatement
    {
        public Location Location { get; set; }
        public string Name;
        public List<(string, string)> ParametersAndType;
        public Block FunctionBody;
        public string RetType;

        public FnDecl(string name, List<(string, string)> parametersAndType, Block functionBody, string retType, Location location)
        {
            Name = name;
            ParametersAndType = parametersAndType;
            FunctionBody = functionBody;
            RetType = retType;
            Location = location;
        }
    }

    public struct Return : IAstStatement
    {
        public IAstExpr Expr;
        public Location Location => Expr.Location;

        public Return(IAstExpr expr)
        {
            Expr = expr;
        }
    }

    public struct ExprStatement : IAstStatement
    {
        public IAstExpr Expr;
        public Location Location => Expr.Location;

        public ExprStatement(IAstExpr expr)
        {
            Expr = expr;
        }
    }

    public struct VarDecl : IAstStatement
    {
        public Location Location { get; set; }
        public string Identifier;
        public string? TypeAnnotation;
        public IAstExpr Expr;

        public VarDecl(string identifier, string typeAnnotation, IAstExpr expr, Location location)
        {
            Identifier = identifier;
            TypeAnnotation = typeAnnotation;
            Expr = expr;
            Location = location;
        }
    }

    public struct IfElse : IAstExpr
    {
        public IAstExpr Conditional;
        public Block TrueBlock;
        public Token? ElseToken;
        public Block? FalseBLock;
        public Location Location { get; set; }

        public IfElse(IAstExpr conditional, Block trueBlock, Location location) : this()
        {
            Conditional = conditional;
            TrueBlock = trueBlock;
            Location = location;
        }

        public IfElse(IAstExpr conditional, Block trueBlock, Token? elseToken, Block? falseBLock, Location location)
        {
            Conditional = conditional;
            TrueBlock = trueBlock;
            ElseToken = elseToken;
            FalseBLock = falseBLock;
            Location = location;
        }
    }

    public struct UnaryOperation : IAstExpr
    {
        public enum Uop
        {
            Negate,
            MakeNegative
        }
        public Location Location { get; set; }
        public IAstExpr Expr;
        public Uop Op;

        public UnaryOperation(IAstExpr expr, Uop op, Location location)
        {
            Expr = expr;
            Op = op;
            Location = location;
        }
    }

    public struct BinaryOperation : IAstExpr
    {
        public enum Bop
        {
            Plus,
            Minus,
            Equals,
            NotEquals,
        }
        public Location Location { get; set; }
        public IAstExpr Lhs, Rhs;
        public Bop Op;

        public BinaryOperation(IAstExpr lhs, IAstExpr rhs, Bop op, Location location)
        {
            Lhs = lhs;
            Rhs = rhs;
            Op = op;
            Location = location;
        }
    }

    public struct VarGet : IAstExpr
    {
        public string Name;
        public Location Location { get; set; }

        public VarGet(string name, Location location)
        {
            Name = name;
            Location = location;
        }
    }
    
    public struct VarSet : IAstExpr
    {
        public string Name;
        public IAstExpr Expr;
        public Location Location { get; set; }

        public VarSet(string name, IAstExpr expr, Location location)
        {
            Name = name;
            Expr = expr;
            Location = location;
        }
    }

    public struct FuncCall : IAstExpr
    {
        public IAstExpr Callee;
        public List<IAstExpr> Arguments;
        public Location Location { get; set; }

        public FuncCall(IAstExpr callee, List<IAstExpr> arguments, Location location)
        {
            Callee = callee;
            Arguments = arguments;
            Location = location;
        }
    }

    // simple AST nodes
    
    public struct Integer : IAstExpr
    {
        public Location Location { get; set; }
        public long Value;

        public Integer(Location loc, long value)
        {
            Location = loc;
            Value = value;
        }
    }

    public struct Boolean : IAstExpr
    {
        public Location Location { get; set; }
        public bool Value;

        public Boolean(Location loc, bool value)
        {
            Location = loc;
            Value = value;
        }
    }
}