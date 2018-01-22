using BlamCore.Cache;
using BlamCore.Commands;
using System;
using System.Collections.Generic;

namespace TagTool.Files
{
    class RebuildStringIdsCommand : Command
    {
        private GameCacheContext CacheContext { get; }

        public RebuildStringIdsCommand(GameCacheContext cacheContext) :
            base(CommandFlags.Inherit,
                
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