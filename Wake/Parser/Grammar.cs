using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public static class Grammar
    {
        public static Parser<string> Identifier { get; } =
            from identifier in Parse.CharExcept(new []{ ':', '#', '=' })
                               .Except(Parse.WhiteSpace)
                               .AtLeastOnce().Text().Token()
            select identifier;

        public static Parser<string> Target { get; } =
            from leading in Parse.WhiteSpace.Many()
            from token in Parse.AnyChar
                          .Except(Parse.Char(':').Or(Parse.WhiteSpace))
                          .AtLeastOnce().Text().Token()
            from trailing in Parse.WhiteSpace.Except(Parse.LineEnd).Optional()
            select token;

        public static Parser<RecipeDeclaration> RecipeDeclaration { get; } =
            from name in Target
            from colon in Parse.Char(':')
            from dependencies in Target.Until(Parse.LineTerminator)
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
