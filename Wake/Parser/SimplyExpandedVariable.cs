using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class SimplyExpandedVariable
    {
        public string Name { get; }
        public string Expression { get; }

        public SimplyExpandedVariable(string name, string expression)
        {
            Name = name;
            Expression = expression;
        }

        public override string ToString()
        {
            return $"{Name} := {Expression}";
        }
    }
}
