using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class ListScriptsCommand : Command
    {
        private GameCache CacheContext { get; }
        private CachedTag Tag { get; }
        private Scenario Definition { get; }

        public ListScriptsCommand(GameCache cacheContext, CachedTag tag, Scenario definition)
            : base(true,

                  "ListScripts",
                  "Lists all scripts in the current scenario tag.",

                  "ListScripts [Filter]",

                  "Lists all scripts in the current scenario tag.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 1)
                return new TagToolError(CommandError.ArgCount);

            var filter = args.Count == 1 ? args[0] : "";

            foreach (var script in Definition.Scripts)
                if (script.ScriptName.Contains(filter))
                    Console.WriteLine($"{script.ScriptName}: Index {script.RootExpressionHandle.Index}");

            return true;
        }
    }
}
