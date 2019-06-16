﻿using System;
using System.Collections.Generic;
using System.Text;
using YololEmulator.Execution;
using YololEmulator.Grammar.AST.Statements;

namespace YololEmulator.Grammar.AST.Expressions.Unary
{
    public abstract class BasePrePostModify
        : BaseExpression
    {
        private readonly VariableName _name;

        protected BasePrePostModify(VariableName name)
        {
            _name = name;
        }

        protected abstract Value Modify(Value value);

        protected abstract Value Return(Value original, Value modified);

        public override Value Evaluate(MachineState state)
        {
            var variable = state.Get(_name);

            var original = variable.Value;
            var modified = Modify(original);

            variable.Value = modified;

            return Return(original, modified);
        }
    }
}