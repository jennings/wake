using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class Recipe
    {
        public IReadOnlyList<string> Lines { get; }

        public Recipe(IEnumerable<string> lines)
        {
            Lines = lines.ToList().AsReadOnly();
        }

        public override string ToString()
        {
            return string.Join("\n", Lines);
        }
    }
}
