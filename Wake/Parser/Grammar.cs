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

        public static Parser<TargetDeclaration> TargetDeclaration { get; } =
            from name in Target
            from colon in Parse.Char(':')
            from dependencies in Target.Until(Parse.LineTerminator)
            select new TargetDeclaration(name, dependencies);

        public static Parser<string> RecipeLine { get; } =
            from tab in Parse.Char('\t')
            from line in Parse.AnyChar.Until(Parse.LineTerminator).Text()
            select line;

        public static Parser<Recipe> Recipe { get; } =
            from lines in RecipeLine.Many()
            select new Recipe(lines);

        public static Parser<Rule> Rule { get; } =
            from decl in TargetDeclaration
            from body in Recipe
            select new Rule(decl, body);

        public static Parser<RecursivelyExpandedVariable> RecursivelyExpandedVariable { get; } =
            from identifier in Identifier
            from eq in Parse.String("=").Text().Token()
            from expression in Parse.AnyChar.Until(Parse.LineTerminator).Text()
            select new RecursivelyExpandedVariable(identifier, expression);

        public static Parser<SimplyExpandedVariable> SimplyExpandedVariable { get; } =
            from identifier in Identifier
            from eq in Parse.String(":=").Text().Token()
            from expression in Parse.AnyChar.Until(Parse.LineTerminator).Text()
            select new SimplyExpandedVariable(identifier, expression);
    }
}
