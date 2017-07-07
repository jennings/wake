using Sprache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wake.Parser
{
    public class TargetBody
    {
        public IReadOnlyList<string> Commands { get; }

        public TargetBody(IEnumerable<string> commands)
        {
            Commands = commands.ToList().AsReadOnly();
        }
    }
}
