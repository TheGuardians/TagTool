using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using TagTool.Shaders.ShaderMatching;

namespace TagTool.Commands.Porting
{
    public class MatchTemplateCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }
        private GameCache BlamCache;

        public MatchTemplateCommand(GameCacheHaloOnlineBase cache, GameCache blamCache) :
            base(true,

                "MatchTemplate",
                "Simulates a shader match and prints the returned template.",

                "MatchTemplate [new] <BlamTemplate>",
                "Simulates a shader match and prints the returned template.")
        {
            Cache = cache;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            if (args[0] == "new")
                return MatchShaderNew(args[1]);
            else if (args.Count == 2)
                return false;

            RenderMethodTemplate bmRmt2 = new RenderMethodTemplate();
            CachedTag bmRmt2Instance;

            using (var blamStream = BlamCache.OpenCacheRead())
            {

                if (BlamCache.TryGetCachedTag(args[0], out bmRmt2Instance))
                    bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamStream, bmRmt2Instance);
                else
                    return false;
            }

            List<string> bmMaps = new List<string>();
            List<string> bmArgs = new List<string>();

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.SamplerArguments)
                bmMaps.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.VectorArguments)
                bmArgs.Add(BlamCache.StringTable.GetString(a.Name));

            string result = null;

            using (var cacheStream = Cache.OpenCacheRead())
            {
                ShaderMatcher Matcher = new ShaderMatcher();
                Matcher.Init(cacheStream, Cache, BlamCache);

                // Find a HO equivalent rmt2
                var edRmt2Instance = Matcher.FixRmt2Reference(cacheStream, bmRmt2Instance.Name, bmRmt2Instance, bmRmt2, bmMaps, bmArgs);
                result = edRmt2Instance.Name;
            }

            Console.WriteLine($"Blam template \"{bmRmt2Instance.Name}.rmt2\"\n matched with \"{result}.rmt2\".");
            return true;
        }

        private bool MatchShaderNew(string blamRmt2Name)
        {
            // call new matcher code here when functional

            return true;
        }
    }
}