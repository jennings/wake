using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class Recipe
    {
        public RecipeDeclaration Declaration { get; }
        public RecipeBody Body { get; }

        public Recipe(RecipeDeclaration declaration, RecipeBody body)
        {
            Declaration = declaration;
            Body = body;
        }

        public override string ToString()
        {
            return $"Recipe {Declaration.Name} ({Body.Lines.Count} lines)";
        }
    }
}
