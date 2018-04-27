using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class RecursivelyExpandedVariable
    {
        public string Name { get; }
        public string Expression { get; }

        public RecursivelyExpandedVariable(string name, string expression)
        {
            Name = name;
            Expression = expression;
        }

        public override string ToString()
        {
            return $"{Name} = {Expression}";
        }
    }
}
