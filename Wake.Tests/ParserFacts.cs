using Sprache;
using System;
using Wake.Parser;
using Xunit;

namespace Wake.Tests
{
    public class ParserFacts
    {
        [Fact]
        public void An_identifier_is_recognized()
        {
            var input = "build";
            var target = Grammar.Identifier.Parse(input);
            Assert.Equal("build", target);
        }

        [Fact]
        public void Target_declaration_is_understood_with_no_dependencies()
        {
            var input = "build:\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Name);
            Assert.Empty(decl.Dependencies);
        }

        [Fact]
        public void Target_declaration_is_understood_with_one_dependency()
        {
            var input = "build: foo\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Name);
            Assert.Equal(decl.Dependencies, new []{ "foo" });
        }

        [Fact]
        public void Target_declaration_is_understood_with_many_dependencies()
        {
            var input = "build: foo bar baz\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Name);
            Assert.Equal(decl.Dependencies, new []{ "foo", "bar", "baz" });
        }

        [Fact]
        public void TargetBodyLine_must_start_with_a_tab()
        {
            var goodInput = "\tbody line\r\n";
            var noIndent = "body line\r\n";
            var spaceIndent = " body line\r\n";

            Assert.Equal(Grammar.TargetBodyLine.Parse(goodInput), "body line");
            Assert.Throws<ParseException>(() => Grammar.TargetBodyLine.Parse(noIndent));
            Assert.Throws<ParseException>(() => Grammar.TargetBodyLine.Parse(spaceIndent));
        }
    }
}
