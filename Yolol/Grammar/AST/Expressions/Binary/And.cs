﻿using System;
using JetBrains.Annotations;
using Yolol.Execution;

namespace Yolol.Grammar.AST.Expressions.Binary
{
    public class And
        : BaseBinaryExpression, IEquatable<And>
    {
        public override bool CanRuntimeError => Left.CanRuntimeError || Right.CanRuntimeError;

        public override bool IsBoolean => true;

        public And([NotNull] BaseExpression left, [NotNull] BaseExpression right)
            : base(left, right)
        {
        }

        public bool Equals(And other)
        {
            return other != null
                && other.Left.Equals(Left)
                && other.Right.Equals(Right);
        }

        public override bool Equals(BaseExpression other)
        {
            return other is And a
                && a.Equals(this);
        }

        public override string ToString()
        {
            return $"{Left} and {Right}";
        }

        protected override Value Evaluate(Value l, Value r)
        {
            return l & r;
        }
    }
}
