using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Tags
{
    class ClearMultiplayerCustomizationCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ClearMultiplayerCustomizationCommand(HaloOnlineCacheContext cacheContext) :
            base(true,
                
                "ClearMultiplayerCustomization",
                "",
                
                "ClearMultiplayerCustomization",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;

            var mulgTag = CacheContext.TagCache.Index.FindFirstInGroup(typeof(MultiplayerGlobals).GetGroupTag());
            var mulgOffset = mulgTag.HeaderOffset + mulgTag.DefinitionOffset;

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            using (var reader = new BinaryReader(cacheStream))
            using (var writer = new BinaryWriter(cacheStream))
            {
                cacheStream.Position = mulgOffset + 4;
                var universalOffset = mulgTag.PointerToOffset(reader.ReadUInt32());

                cacheStream.Position = mulgTag.HeaderOffset + universalOffset + 16 + 16;
                writer.Write(new byte[24]);
            }

            return true;
        }
    }
}
