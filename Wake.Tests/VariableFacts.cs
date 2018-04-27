using Sprache;
using System;
using System.Linq;
using Wake.Parser;
using Xunit;

namespace Wake.Tests
{
    public class VariableFacts
    {
        [Theory]
        [InlineData("foo=bar\n")]
        [InlineData("foo = bar\n")]
        [InlineData("foo   = bar\n")]
        public void A_recursively_expanded_variable_is_recognized(string input)
        {
            var variable = Grammar.RecursivelyExpandedVariable.Parse(input);
            Assert.Equal("foo", variable.Name);
            Assert.Equal("bar", variable.Expression);
        }

        [Theory]
        [InlineData("foo:=bar\n")]
        [InlineData("foo := bar\n")]
        [InlineData("foo   := bar\n")]
        public void A_simply_expanded_variable_is_recognized(string input)
        {
            var variable = Grammar.SimplyExpandedVariable.Parse(input);
            Assert.Equal("foo", variable.Name);
            Assert.Equal("bar", variable.Expression);
        }
    }
}
