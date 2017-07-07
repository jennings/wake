using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class RecipeDeclaration
    {
        public string Name { get; }

        public IReadOnlyList<string> Dependencies { get; }

        public RecipeDeclaration(string name, IEnumerable<string> dependencies)
        {
            Name = name;
            Dependencies = dependencies.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            if (Dependencies.Count > 0)
            {
                return $"{Name}: {string.Join(" ", Dependencies)}";
            }
            else
            {
                return Name;
            }
        }
    }
}
