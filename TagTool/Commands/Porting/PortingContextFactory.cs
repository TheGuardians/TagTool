using TagTool.Cache;
using TagTool.Commands;
using TagTool.Legacy.Base;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    static class PortingContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCacheContext cacheContext, CacheFile blamCache)
        {
            var context = new CommandContext(contextStack.Context, blamCache.Header.ScenarioPath);

            Populate(contextStack, context, cacheContext, blamCache);

            return context;
        }

        public static CacheFileResourceGestalt LoadResourceGestalt(GameCacheContext cacheContext, CacheFile blamCache)
        {
            CacheFile.IndexItem blamTag = null;

            foreach (var tag in blamCache.IndexItems)
            {
                if (tag.ClassCode == "zone")
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
                return null;

            var blamContext = new CacheSerializationContext(cacheContext, blamCache, blamTag);
            return new TagDeserializer(blamCache.Version).Deserialize<CacheFileResourceGestalt>(blamContext);
        }

        public static SoundCacheFileGestalt LoadSoundGestalt(GameCacheContext cacheContext, CacheFile blamCache)
        {
            CacheFile.IndexItem blamTag = null;

            foreach (var tag in blamCache.IndexItems)
            {
                if (tag.ClassCode == "ugh!")
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
                return null;

            var blamContext = new CacheSerializationContext(cacheContext, blamCache, blamTag);
            return new TagDeserializer(blamCache.Version).Deserialize<SoundCacheFileGestalt>(blamContext);
        }

        public static void Populate(CommandContextStack contextStack, CommandContext context, GameCacheContext cacheContext, CacheFile blamCache)
        {
            context.AddCommand(new ConvertBitmapCommand(cacheContext, blamCache));
            context.AddCommand(new DumpBspGeometryCommand(cacheContext, blamCache));
            context.AddCommand(new DumpScriptInfoCommand(cacheContext, blamCache));
            context.AddCommand(new DumpTagFunctionCommand(cacheContext, blamCache));
            context.AddCommand(new ListBitmapsCommand(cacheContext, blamCache));
            context.AddCommand(new ListBlamTagsCommand(cacheContext, blamCache));
            context.AddCommand(new ListBspMoppCodesCommand(cacheContext, blamCache));
            context.AddCommand(new MatchShadersCommand(cacheContext, blamCache));
            context.AddCommand(new PortArmorVariantCommand(cacheContext, blamCache));
            context.AddCommand(new PortMultiplayerEventsCommand(cacheContext, blamCache));
            context.AddCommand(new PortTagCommand(cacheContext, blamCache));
            context.AddCommand(new PortingTestCommand(cacheContext, blamCache));
        }
    }
}