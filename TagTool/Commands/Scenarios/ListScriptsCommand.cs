using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    class ListScriptsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Scenario Definition { get; }

        public ListScriptsCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Scenario definition)
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
                return false;

            var filter = args.Count == 1 ? args[0] : "";

            foreach (var script in Definition.Scripts)
                if (script.ScriptName.Contains(filter))
                    Console.WriteLine($"{script.ScriptName}: Index {script.RootExpressionHandle.Index}");

            return true;
        }
    }
}
