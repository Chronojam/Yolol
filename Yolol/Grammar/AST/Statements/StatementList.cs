﻿using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Yolol.Execution;

namespace Yolol.Grammar.AST.Statements
{
    public class StatementList
        : BaseStatement
    {
        [NotNull] public IReadOnlyList<BaseStatement> Statements { get; }

        public StatementList([NotNull] IEnumerable<BaseStatement> statements)
        {
            Statements = statements.ToArray();
        }

        public override ExecutionResult Evaluate(MachineState state)
        {
            foreach (var statement in Statements)
            {
                var r = statement.Evaluate(state);
                switch (r.Type)
                {
                    case ExecutionResultType.Goto:
                        return r;

                    case ExecutionResultType.None:
                        continue;

                    //ncrunch: no coverage start
                    default:
                        throw new ExecutionException($"Unknown ExecutionResult `{r.Type}`");
                    //ncrunch: no coverage end

                }
            }

            return new ExecutionResult();
        }

        public override string ToString()
        {
            return string.Join(" ", Statements.Select(s => s.ToString()));
        }
    }
}