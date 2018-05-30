using TagTool.Cache;
using System;
using System.Collections.Generic;

namespace TagTool.Commands.Files
{
    class RebuildStringIdsCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public RebuildStringIdsCommand(HaloOnlineCacheContext cacheContext) :
            base(true,
                
                "RebuildStringIds",
                "Rebuilds the string_id cache.",
                
                "RebuildStringIds",
                
                "Rebuilds the string_id cache.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;



            throw new NotImplementedException();
        }
    }
}