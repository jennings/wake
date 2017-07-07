using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class TargetDeclaration
    {
        public string Name { get; }

        public IReadOnlyList<string> Dependencies { get; }

        public TargetDeclaration(string name, IEnumerable<string> dependencies)
        {
            Name = name;
            Dependencies = dependencies.ToList().AsReadOnly();
        }
    }
}
