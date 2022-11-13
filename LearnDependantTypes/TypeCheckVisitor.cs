namespace LearnDependantTypes
{
    public class TypeCheckVisitor : IExprVisitor<IType>, IStatementVisitor<IType>, ITopLevelVisitor<IType>
    {
        public IType VisitBinaryOperation(BinaryOperation bop)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitBoolean(Boolean boolean)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitFuncCall(FuncCall fnCall)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitIdentifier(Identifier id)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitIfElse(IfElse ie)
        {
            throw new System.NotImplementedException();
        }

        public IType VisitInteger(Integer i)
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}