using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using static TagTool.Cache.CacheFile;
using static TagTool.Commands.Porting.PortTagCommand;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.Commands.Bipeds
{
    class CreateCharacterType : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private Biped Biped { get; }
        private ModGlobalsDefinition ModGlobals;
        private Globals Globals;

        public CreateCharacterType(HaloOnlineCacheContext cacheContext, Biped biped) :
            base(true,

                "CreateCharacterType",
                "Builds a character type from the current biped tag.",

                "CreateCharacterType",

                "Builds a character type from the current biped tag")
        {
            CacheContext = cacheContext;
            Biped = biped;
        }

        public override object Execute(List<string> args)
        {
            OpenGlobalTags();
            return true;
        }

        private void OpenGlobalTags()
        {
            using (var cacheStream = CacheContext.TagCacheFile.OpenRead())
            {
                Globals = CacheContext.Deserialize<Globals>(cacheStream, CacheContext.GetTag($"globals\\globals.matg"));
                ModGlobals = CacheContext.Deserialize<ModGlobalsDefinition>(cacheStream, CacheContext.GetTag($"multiplayer\\mod_globals.modg"));
            }
        }
    }
}
