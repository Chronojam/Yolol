﻿using YololEmulator.Execution;

namespace YololEmulator.Grammar.AST.Expressions.Unary
{
    public class VariableExpression
        : BaseExpression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public override Value Evaluate(MachineState state)
        {
            return state.Get(_name).Value;
        }
    }
}
