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

        public static Parser<TargetDeclaration> TargetDeclaration { get; } =
            from name in Identifier
            from colon in Parse.Char(':')
            from dependencies in Identifier.Until(Parse.LineTerminator)
            select new TargetDeclaration(name, dependencies);

        public static Parser<string> TargetBodyLine { get; } =
            from tab in Parse.Char('\t')
            from line in Parse.AnyChar.Until(Parse.LineTerminator).Text()
            select line;

        public static Parser<TargetBody> TargetBody { get; } =
            from lines in TargetBodyLine.Many()
            select new TargetBody(lines);

        public static Parser<Target> Target { get; } =
            from decl in TargetDeclaration
            from body in TargetBody
            select new Target(decl, body);
    }
}
