﻿using System;
using System.Linq;
using JetBrains.Annotations;
using Yolol.Grammar;
using Yolol.Grammar.AST.Expressions;
using Yolol.Grammar.AST.Expressions.Binary;
using Yolol.Grammar.AST.Expressions.Unary;
using Yolol.Grammar.AST.Statements;

namespace Yolol.Analysis
{
    public abstract class BaseTreeVisitor
        : ITreeVisitor
    {
        [NotNull] public virtual Program Visit([NotNull] Program program)
        {
            return new Program(program.Lines.Select(Visit));
        }

        [NotNull] protected virtual Line Visit([NotNull] Line line)
        {
            var r = Visit(line.Statements);
            if (r is StatementList sl)
                return new Line(sl);
            return new Line(new StatementList(new[] { r }));
        }

        [NotNull] protected virtual VariableName Visit([NotNull] VariableName var)
        {
            return var;
        }

        #region expression visiting
        [NotNull] public virtual BaseExpression Visit([NotNull] BaseExpression expression)
        {
            switch (expression)
            {
                case Bracketed a:   return Visit(a);
                case Application a: return Visit(a);

                case PostIncrement a: return Visit(a);
                case PreIncrement a:  return Visit(a);
                case PostDecrement a: return Visit(a);
                case PreDecrement a:  return Visit(a);

                case Add a:      return Visit(a);
                case Subtract a: return Visit(a);
                case Multiply a: return Visit(a);
                case Divide a:   return Visit(a);
                case Modulo a:   return Visit(a);
                case Negate a:   return Visit(a);

                case Variable a:       return Visit(a);
                case ConstantNumber a: return Visit(a);
                case ConstantString a: return Visit(a);

                case EqualTo a:            return Visit(a);
                case NotEqualTo a:         return Visit(a);
                case GreaterThan a:        return Visit(a);
                case GreaterThanEqualTo a: return Visit(a);
                case LessThan a:           return Visit(a);
                case LessThanEqualTo a:    return Visit(a);
            }

            throw new InvalidOperationException($"`Visit` not invalid for expression type `{expression.GetType().FullName}`");
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] LessThanEqualTo eq)
        {
            return new LessThanEqualTo(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] LessThan eq)
        {
            return new LessThan(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] GreaterThanEqualTo eq)
        {
            return new GreaterThanEqualTo(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] GreaterThan eq)
        {
            return new GreaterThan(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] NotEqualTo eq)
        {
            return new NotEqualTo(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] EqualTo eq)
        {
            return new EqualTo(Visit(eq.Left), Visit(eq.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Variable var)
        {
            return new Variable(Visit(var.Name));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Modulo mod)
        {
            return new Modulo(Visit(mod.Left), Visit(mod.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] PreDecrement inc)
        {
            return inc;
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] PostDecrement inc)
        {
            return inc;
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] PreIncrement inc)
        {
            return inc;
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] PostIncrement inc)
        {
            return inc;
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Application app)
        {
            return new Bracketed(Visit(app.Parameter));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Bracketed brk)
        {
            return new Bracketed(Visit(brk.Expression));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Add add)
        {
            return new Add(Visit(add.Left), Visit(add.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Subtract sub)
        {
            return new Subtract(Visit(sub.Left), Visit(sub.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Multiply mul)
        {
            return new Multiply(Visit(mul.Left), Visit(mul.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Divide div)
        {
            return new Divide(Visit(div.Left), Visit(div.Right));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] Negate neg)
        {
            return new Negate(Visit(neg.Expression));
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] ConstantNumber con)
        {
            return con;
        }

        [NotNull] protected virtual BaseExpression Visit([NotNull] ConstantString con)
        {
            return con;
        }
        #endregion

        #region statement visiting
        [NotNull] public BaseStatement Visit([NotNull] BaseStatement statement)
        {
            switch (statement)
            {
                case CompoundAssignment a: return Visit(a);
                case Assignment a:   return Visit(a);
                case ExpressionWrapper a: return Visit(a);
                case Goto a: return Visit(a);
                case If a: return Visit(a);
                case StatementList a: return Visit(a);
            }

            throw new InvalidOperationException($"`Visit` invalid for statement type `{statement.GetType().FullName}`");
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] StatementList list)
        {
            return new StatementList(list.Statements.Select(Visit));
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] CompoundAssignment compAss)
        {
            return new CompoundAssignment(Visit(compAss.Left), compAss.Op, Visit(compAss.Right));
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] Assignment ass)
        {
            return new Assignment(Visit(ass.Left), Visit(ass.Right));
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] ExpressionWrapper expr)
        {
            return new ExpressionWrapper(Visit(expr.Expression));
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] Goto @goto)
        {
            return new Goto(Visit(@goto.Destination));
        }

        [NotNull] protected virtual BaseStatement Visit([NotNull] If @if)
        {
            return new If(Visit(@if.Condition), Visit(@if.TrueBranch), Visit(@if.FalseBranch));
        }
        #endregion
    }
}