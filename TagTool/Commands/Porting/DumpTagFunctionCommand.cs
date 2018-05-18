using TagTool.Cache;
using TagTool.Commands;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Porting
{
    public class DumpTagFunctionCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }
        public CacheFile BlamCache;
        public DumpTagFunctionCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache)
            : base(CommandFlags.Inherit,
                  
                  "DumpTagFunction",
                  "Dumps all tag function to a file",

                  "DumpTagFunction <filepath>",

                  "<filepath>  - A path to write functions that are not converted")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            int index = Convert.ToInt32(args[0]);
            Console.WriteLine(BlamCache.LocaleTables[0][index]);
            Console.WriteLine(BlamCache.Strings.GetItemByID(BlamCache.Strings.Count- 2849 +index));
            return true;
        }
        
    }
}
