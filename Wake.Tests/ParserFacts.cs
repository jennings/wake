using Sprache;
using System;
using System.Linq;
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

        [Fact]
        public void An_empty_body_is_recognized()
        {
            var input = "";
            var body = Grammar.TargetBody.Parse(input);
            Assert.Empty(body.Lines);
        }

        [Fact]
        public void A_whitespace_body_is_recognized()
        {
            var input = "\r\n\r\n\r\n";
            var body = Grammar.TargetBody.Parse(input);
            Assert.Empty(body.Lines);
        }

        [Fact]
        public void Target_with_no_body_is_recognized()
        {
            var input = "build: foo\n";
            var target = Grammar.Target.Parse(input);
            Assert.Equal("build", target.Declaration.Name);
            Assert.Empty(target.Body.Lines);
        }

        [Fact]
        public void Two_targets_in_a_row_with_no_bodies_are_recognized()
        {
            var input = "build:\ntest:";
            var targets = Grammar.Target.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);
            Assert.Equal("build", targets[0].Declaration.Name);
            Assert.Equal("test", targets[1].Declaration.Name);
        }

        [Fact]
        public void Two_targets_in_a_row_with_one_body_are_recognized()
        {
            var input = "build:\n\tfoo\ntest:";
            var targets = Grammar.Target.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);

            Assert.Equal("build", targets[0].Declaration.Name);
            Assert.Equal(targets[0].Body.Lines, new []{ "foo" });

            Assert.Equal("test", targets[1].Declaration.Name);
            Assert.Empty(targets[1].Body.Lines);
        }

        [Fact]
        public void Two_targets_in_a_row_with_two_bodies_are_recognized()
        {
            var input = "build:\n\tfoo\r\n\tbar\ntest:\r\n\tbaz\n";
            var targets = Grammar.Target.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);

            Assert.Equal("build", targets[0].Declaration.Name);
            Assert.Equal(targets[0].Body.Lines, new []{ "foo", "bar" });

            Assert.Equal("test", targets[1].Declaration.Name);
            Assert.Equal(targets[1].Body.Lines, new []{ "baz" });
        }
    }
}
