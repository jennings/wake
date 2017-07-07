using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class RecipeDeclaration
    {
        public string Target { get; }

        public IReadOnlyList<string> Dependencies { get; }

        public RecipeDeclaration(string target, IEnumerable<string> dependencies)
        {
            Target = target;
            Dependencies = dependencies.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            if (Dependencies.Count > 0)
            {
                return $"{Target}: {string.Join(" ", Dependencies)}";
            }
            else
            {
                return Target;
            }
        }
    }
}
