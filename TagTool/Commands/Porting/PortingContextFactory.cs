using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    static class PortingContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, HaloOnlineCacheContext cacheContext, CacheFile blamCache)
        {
            var context = new CommandContext(contextStack.Context, blamCache.Header.ScenarioPath);

            Populate(contextStack, context, cacheContext, blamCache);

            return context;
        }
        
        public static SoundCacheFileGestalt LoadSoundGestalt(HaloOnlineCacheContext cacheContext, ref CacheFile blamCache)
        {
            CacheFile.IndexItem blamTag = null;

            foreach (var tag in blamCache.IndexItems)
            {
                if (tag.GroupTag == "ugh!")
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
                return null;

            var blamContext = new CacheSerializationContext(ref blamCache, blamTag);
            var ugh = blamCache.Deserializer.Deserialize<SoundCacheFileGestalt>(blamContext);

            //
            // Apply conversion to ugh! data (gain increase and such)
            //

            return ugh;
        }

        public static void Populate(CommandContextStack contextStack, CommandContext context, HaloOnlineCacheContext cacheContext, CacheFile blamCache)
        {
            var portTagCommand = new PortTagCommand(cacheContext, blamCache);

            context.AddCommand(portTagCommand);
            context.AddCommand(new ExtractXMACommand(cacheContext, blamCache));
            context.AddCommand(new ExtractBitmapCommand(blamCache));
            context.AddCommand(new EditTagCommand(contextStack, blamCache));
            context.AddCommand(new ListBlamTagsCommand(cacheContext, blamCache));
            context.AddCommand(new PortArmorVariantCommand(cacheContext, blamCache));
            context.AddCommand(new PortMultiplayerEventsCommand(cacheContext, blamCache));
            context.AddCommand(new NameBlamTagCommand(cacheContext, blamCache));
            context.AddCommand(new MergeAnimationGraphsCommand(cacheContext, blamCache, portTagCommand));
		}
	}
}