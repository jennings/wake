using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public static class Grammar
    {
        public static Parser<string> Identifier { get; } =
            Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token();

        public static Parser<RecipeDeclaration> RecipeDeclaration { get; } =
            from name in Identifier
            from colon in Parse.Char(':')
            from dependencies in Identifier.Until(Parse.LineTerminator)
            select new RecipeDeclaration(name, dependencies);

        public static Parser<string> RecipeBodyLine { get; } =
            from tab in Parse.Char('\t')
            from line in Parse.AnyChar.Until(Parse.LineTerminator).Text()
            select line;

        public static Parser<RecipeBody> RecipeBody { get; } =
            from lines in RecipeBodyLine.Many()
            select new RecipeBody(lines);

        public static Parser<Recipe> Recipe { get; } =
            from decl in RecipeDeclaration
            from body in RecipeBody
            select new Recipe(decl, body);
    }
}
