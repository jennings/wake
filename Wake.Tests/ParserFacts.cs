using Sprache;
using System;
using System.Linq;
using Wake.Parser;
using Xunit;

namespace Wake.Tests
{
    public class ParserFacts
    {
        [Theory]
        [InlineData("hello")]
        [InlineData("HELLO_WORLD")]
        [InlineData("$HELLO_WORLD")]
        public void Valid_identifiers_are_recognized(string input)
        {
            var identifier = Grammar.Identifier.Parse(input);
            Assert.Equal(input, identifier);
        }

        [Theory]
        [InlineData("whitespace bad", "whitespace")]
        [InlineData("colons_are_for:targets", "colons_are_for")]
        [InlineData("comments#comments", "comments")]
        [InlineData("no=equals", "no")]
        public void Invalid_identifiers_are_rejected(string input, string expected)
        {
            var identifier = Grammar.Identifier.Parse(input);
            Assert.Equal(expected, identifier);
        }

        [Fact]
        public void A_target_is_recognized()
        {
            var input = "build";
            var target = Grammar.Target.Parse(input);
            Assert.Equal("build", target);
        }

        [Fact]
        public void Targets_can_contain_punctuation()
        {
            var input = "%.o";
            var target = Grammar.Target.Parse(input);
            Assert.Equal("%.o", target);
        }

        [Fact]
        public void Target_declaration_is_understood_with_no_dependencies()
        {
            var input = "build:\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Target);
            Assert.Empty(decl.Dependencies);
        }

        [Fact]
        public void Target_declaration_is_understood_with_one_dependency()
        {
            var input = "build: foo\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Target);
            Assert.Equal(decl.Dependencies, new []{ "foo" });
        }

        [Fact]
        public void Target_declaration_is_understood_with_many_dependencies()
        {
            var input = "build: foo bar baz\n";
            var decl = Grammar.TargetDeclaration.Parse(input);
            Assert.Equal("build", decl.Target);
            Assert.Equal(decl.Dependencies, new []{ "foo", "bar", "baz" });
        }

        [Fact]
        public void RecipeLine_must_start_with_a_tab()
        {
            var goodInput = "\tbody line\r\n";
            var noIndent = "body line\r\n";
            var spaceIndent = " body line\r\n";

            Assert.Equal(Grammar.RecipeLine.Parse(goodInput), "body line");
            Assert.Throws<ParseException>(() => Grammar.RecipeLine.Parse(noIndent));
            Assert.Throws<ParseException>(() => Grammar.RecipeLine.Parse(spaceIndent));
        }

        [Fact]
        public void An_empty_recipe_is_recognized()
        {
            var input = "";
            var body = Grammar.Recipe.Parse(input);
            Assert.Empty(body.Lines);
        }

        [Fact]
        public void A_whitespace_recipe_is_recognized()
        {
            var input = "\r\n\r\n\r\n";
            var body = Grammar.Recipe.Parse(input);
            Assert.Empty(body.Lines);
        }

        [Fact]
        public void Recipe_with_no_body_is_recognized()
        {
            var input = "build: foo\n";
            var target = Grammar.Rule.Parse(input);
            Assert.Equal("build", target.Target.Target);
            Assert.Empty(target.Body.Lines);
        }

        [Fact]
        public void Two_targets_in_a_row_with_no_bodies_are_recognized()
        {
            var input = "build:\ntest:";
            var targets = Grammar.Rule.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);
            Assert.Equal("build", targets[0].Target.Target);
            Assert.Equal("test", targets[1].Target.Target);
        }

        [Fact]
        public void Two_targets_in_a_row_with_one_body_are_recognized()
        {
            var input = "build:\n\tfoo\ntest:";
            var targets = Grammar.Rule.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);

            Assert.Equal("build", targets[0].Target.Target);
            Assert.Equal(targets[0].Body.Lines, new []{ "foo" });

            Assert.Equal("test", targets[1].Target.Target);
            Assert.Empty(targets[1].Body.Lines);
        }

        [Fact]
        public void Two_targets_in_a_row_with_two_bodies_are_recognized()
        {
            var input = "build:\n\tfoo\r\n\tbar\ntest:\r\n\tbaz\n";
            var targets = Grammar.Rule.Many().Parse(input).ToList();
            Assert.Equal(2, targets.Count);

            Assert.Equal("build", targets[0].Target.Target);
            Assert.Equal(targets[0].Body.Lines, new []{ "foo", "bar" });

            Assert.Equal("test", targets[1].Target.Target);
            Assert.Equal(targets[1].Body.Lines, new []{ "baz" });
        }
    }
}
