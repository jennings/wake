using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class Rule
    {
        public TargetDeclaration Target { get; }
        public Recipe Body { get; }

        public Rule(TargetDeclaration target, Recipe body)
        {
            Target = target;
            Body = body;
        }

        public override string ToString()
        {
            return $"Recipe {Target.Target} ({Body.Lines.Count} lines)";
        }
    }
}
