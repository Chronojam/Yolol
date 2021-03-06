﻿using System;
using JetBrains.Annotations;
using Yolol.Execution;

namespace Yolol.Grammar.AST.Expressions.Unary
{
    public abstract class BaseTrigonometry
        : BaseUnaryExpression
    {
        private readonly string _name;

        public override bool CanRuntimeError => true;
        public override bool IsBoolean => false;

        protected BaseTrigonometry([NotNull] BaseExpression parameter, string name)
            : base(parameter)
        {
            _name = name;
        }

        public override string ToString()
        {
            return $"{_name}({Parameter})";
        }
    }
}
