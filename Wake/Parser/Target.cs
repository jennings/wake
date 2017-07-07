using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class Target
    {
        public TargetDeclaration Declaration { get; }
        public TargetBody Body { get; }

        public Target(TargetDeclaration declaration, TargetBody body)
        {
            Declaration = declaration;
            Body = body;
        }

        public override string ToString()
        {
            return $"Target {Declaration.Name} ({Body.Lines.Count} lines)";
        }
    }
}
