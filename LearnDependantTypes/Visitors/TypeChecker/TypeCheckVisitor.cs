using System;
using System.Collections.Generic;
using LearnDependantTypes.Visitors.Interpreter;

namespace LearnDependantTypes.Visitors.TypeChecker
{
    public class TypeCheckVisitor : IExprVisitor<IType>, IStatementVisitor<IType>, ITopLevelVisitor<IType>
    {
        private InterpreterEnvironment<IType> _te = new InterpreterEnvironment<IType>();

        private Dictionary<string, IType> _parseTypes = new Dictionary<string, IType>()
        {
            {"Int", new TInt()},
            {"Bool", new TBool()},
            {"Unit", new TUnit()}
        };

        public IType VisitBinaryOperation(BinaryOperation bop)
        {
            if (bop.Op == BinaryOperation.Bop.Equals || bop.Op == BinaryOperation.Bop.NotEquals)
            {
                return new TBool();
            }

            var lhs = this.VisitExpr(bop.Lhs);
            var rhs = this.VisitExpr(bop.Rhs);

            if (lhs.GetType() != rhs.GetType())
            {
                throw new TypeError($"{bop.Lhs}:[{rhs}] must match {bop.Rhs}:[{rhs}] to apply \"+\" or \"-\" at ({bop.Location})");
            }

            return lhs;
        }

        public IType VisitBoolean(Boolean boolean)
        {
            return new TBool();
        }

        public IType VisitFuncCall(FuncCall fnCall)
        {
            var fnMaybe = this.VisitExpr(fnCall);
            if (fnMaybe is TFunction fn)
            {
                return fn;
            }
            else
            {
                throw new TypeError($"Cannot call function at ({fnCall.Location}) because it has type {fnMaybe}");
            }
        }

        public IType VisitIfElse(IfElse ie)
        {
            var conditionType = this.VisitExpr(ie.Conditional);
            if (!(conditionType is TBool))
            {
                throw new TypeError(
                    $"if statement at ({ie.Location})'s condition is {conditionType} but must be {new TBool()}");
            }

            var tBlock = this.VisitStatement(ie.TrueBlock);
            if (ie.FalseBLock.HasValue)
            {
                var fBlock = this.VisitStatement(ie.FalseBLock.Value);

                if (fBlock.GetType() != tBlock.GetType())
                {
                    throw new TypeError(
                        $"if statement at ({ie.Location})'s true block has type {tBlock} and its false block has type {fBlock}. They must be the same.");
                }
            }
            
            return tBlock;
        }

        public IType VisitInteger(Integer i)
        {
            return new TInt();
        }

        public IType VisitUnaryOperation(UnaryOperation uop)
        {
            var rhs = this.VisitExpr(uop.Expr);
            if (uop.Op == UnaryOperation.Uop.Negate && rhs.GetType() != typeof(TBool))
            {
                return new TBool();
            }
            else if (uop.Op == UnaryOperation.Uop.MakeNegative && rhs.GetType() != typeof(TInt))
            {
                return new TInt();
            }

            throw new TypeError($"{uop.Op} is not able to be applied to type of {rhs} at ({uop.Location})");
        }

        public IType VisitVarGet(VarGet vg)
        {
            return _te.Get(vg.Name);
        }

        public IType VisitVarSet(VarSet vs)
        {
            var rhs = this.VisitExpr(vs.Expr);
            _te.Set(vs.Name, rhs);
            return rhs;
        }

        public IType VisitBlock(Block block)
        {
            var prior = _te;
           _te = new InterpreterEnvironment<IType>(prior);
            var ret = this.VisitStatement(block.Statements[^1]);
            _te = prior;
            return ret;
        }

        public IType VisitExprStatement(ExprStatement exprStatement)
        {
            return this.VisitExpr(exprStatement.Expr);
        }

        public IType VisitFnDecl(FnDecl funcDecl)
        {
            
        }

        public IType VisitReturn(Return ret)
        {
            return this.VisitExpr(ret.Expr);
        }

        public IType VisitVarDecl(VarDecl decl)
        {
            var typ = this.VisitExpr(decl.Expr);
            if (decl.Identifier == null)
            {
                _te.Bind(decl.Identifier, typ);
                return typ;
            }
            else
            {
                if (_parseTypes.ContainsKey(decl.Identifier))
                {
                    // we know what type its talking about
                    var lhs = _parseTypes[decl.Identifier];
                    var rhs = this.VisitExpr(decl.Expr);
                    if (lhs.GetType() == rhs.GetType())
                    {
                        throw new TypeError(
                            $"{decl.Identifier} is said to be of type {decl.TypeAnnotation} but is assigned to an expression of {rhs} at ({decl.Location})");
                    }

                    return lhs;
                }
                else
                {
                    throw new NotImplementedException("Something is fuck in VisitVarDecl");
                }
            }
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

    public class TypeError : Exception
    {
        public TypeError(string s) : base(s)
        { }
    }
}