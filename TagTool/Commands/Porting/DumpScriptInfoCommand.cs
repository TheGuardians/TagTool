using TagTool.Cache;
using TagTool.Commands;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    public class DumpScriptInfoCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public DumpScriptInfoCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(CommandFlags.Inherit,

                "DumpScriptInfo",
                "DumpScriptInfo <scnr tag>",
                "DumpScriptInfo",
                "DumpScriptInfo <scnr tag>")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            //
            // Verify the Blam scenario tag
            //
            
            CacheFile.IndexItem blamTag = null;

            Console.WriteLine("Verifying Blam scenario tag...");

            foreach (var tag in BlamCache.IndexItems)
            {
                if (tag.GroupTag == "scnr")
                {
                    blamTag = tag;
                    break;
                }
            }
            
            //
            // Load the Blam scenario tag
            //
            
            var blamContext = new CacheSerializationContext(BlamCache, blamTag);
            var blamScenario = BlamCache.Deserializer.Deserialize<Scenario>(blamContext);

            foreach (var script in blamScenario.Scripts)
            {
                var index = (int)blamScenario.ScriptExpressions[(int)(script.RootExpressionHandle & ushort.MaxValue) + 1].NextExpressionHandle & ushort.MaxValue;
                Console.WriteLine($"{script.ScriptName}: 0x{blamScenario.ScriptExpressions[index].Opcode:X3}");
            }

            return true;
        }
    }
}
