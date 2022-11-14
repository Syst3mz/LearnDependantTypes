using System;

namespace LearnDependantTypes.Visitors.TypeChecker
{
    public class TypeCheckVisitor : IExprVisitor<IType>, IStatementVisitor<IType>, ITopLevelVisitor<IType>
    {
        public IType VisitBinaryOperation(BinaryOperation bop)
        {
            if (bop.Op == BinaryOperation.Bop.Equals || bop.Op == BinaryOperation.Bop.NotEquals)
            {
                return new TBool();
            }
        }

        public IType VisitBoolean(Boolean boolean)
        {
            return new TBool();
        }

        public IType VisitFuncCall(FuncCall fnCall)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitIfElse(IfElse ie)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitInteger(Integer i)
        {
            return new TInt();
        }

        public IType VisitUnaryOperation(UnaryOperation uop)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitVarGet(VarGet vg)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitVarSet(VarSet vs)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitBlock(Block block)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitExprStatement(ExprStatement exprStatement)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitFnDecl(FnDecl funcDecl)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitReturn(Return ret)
        {
            return this.VisitExpr(ret.Expr);
        }

        public IType VisitVarDecl(VarDecl decl)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitStruct(Struct s)
        {
            throw new System.NotImplementedException();
        }

        public IType FnDeclTopLevel(FnDeclTopLevel s)
        {
            return this.VisitStatement(s.Function);
        }
    }
}