using System;

namespace LearnDependantTypes
{
    public interface ITopLevelVisitor<T>
    {
        public T VisitStruct(Struct s);
        public T FnDeclTopLevel(FnDeclTopLevel s);
    }

    public static class TopLevelVisitorExtension
    {
        public static T VisitTopLevel<T>(this ITopLevelVisitor<T> visitor, IAstTopLevel topLevel) 
        {
            switch (topLevel)
            {
                case FnDeclTopLevel fnDeclTopLevel:
                    return visitor.FnDeclTopLevel(fnDeclTopLevel);
                case Struct str:
                    return visitor.VisitStruct(str);
            }

            throw new Exception("Quite Literally how did we get here (Top Level)");
        }
    }
    
    public interface IStatementVisitor<T>
    {
        public T VisitBlock(Block block);
        public T VisitExprStatement(ExprStatement exprStatement);
        public T VisitFnDecl(FnDecl funcDecl);
        public T VisitReturn(Return ret);
        public T VisitVarDecl(VarDecl decl);
        
        
    }

    public static class StatementVisitorExtension
    {
        public static T VisitStatement<T>(this IStatementVisitor<T> visitor, IAstStatement statement) 
        {
            switch (statement)
            {
                case Block block:
                    return visitor.VisitBlock(block);
                case ExprStatement exprStatement:
                    return visitor.VisitExprStatement(exprStatement);
                case FnDecl fnDecl:
                    return visitor.VisitFnDecl(fnDecl);
                case Return @return:
                    return visitor.VisitReturn(@return);
                case VarDecl varDecl:
                    return visitor.VisitVarDecl(varDecl);
            }

            throw new Exception("Quite Literally how did we get here (Statement)");
        }
    }
    
    public interface IExprVisitor<T>
    {
        public T VisitBinaryOperation(BinaryOperation bop);
        public T VisitBoolean(Boolean boolean);
        public T VisitFuncCall(FuncCall fnCall);
        public T VisitIdentifier(Identifier id);
        public T VisitIfElse(IfElse ie);
        public T VisitInteger(Integer i);
        public T VisitUnaryOperation(UnaryOperation uop);
        public T VisitVarGet(VarGet vg);
        public T VisitVarSet(VarSet vs);
    }
    
    public static class ExprVisitorExtension
    {
        public static T VisitExpr<T>(this IExprVisitor<T> visitor, IAstExpr statement) 
        {
            switch (statement)
            {
                case BinaryOperation binaryOperation:
                    return visitor.VisitBinaryOperation(binaryOperation);
                case Boolean boolean:
                    return visitor.VisitBoolean(boolean);
                case FuncCall funcCall:
                    return visitor.VisitFuncCall(funcCall);
                case Identifier identifier:
                    return visitor.VisitIdentifier(identifier);
                case IfElse ifElse:
                    return visitor.VisitIfElse(ifElse);
                case Integer integer:
                    return visitor.VisitInteger(integer);
                case UnaryOperation unaryOperation:
                    return visitor.VisitUnaryOperation(unaryOperation);
                case VarGet varGet:
                    return visitor.VisitVarGet(varGet);
                case VarSet varSet:
                    return visitor.VisitVarSet(varSet);
            }

            throw new Exception("Quite Literally how did we get here (Expr)");
        }
    }
}