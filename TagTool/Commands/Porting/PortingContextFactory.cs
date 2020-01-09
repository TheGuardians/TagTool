using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    static class PortingContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache currentCache, GameCache portingCache)
        {
            var context = new CommandContext(contextStack.Context, portingCache.Version.ToString());

            Populate(contextStack, context, currentCache, portingCache);

            return context;
        }
        
        public static SoundCacheFileGestalt LoadSoundGestalt(GameCache cache)
        {
            CachedTag blamTag = null;

            foreach (var tag in cache.TagCache.TagTable)
            {
                if (tag.Group.Tag == "ugh!")
                {
                    blamTag = tag;
                    break;
                }
            }

            if (blamTag == null)
                return null;
            SoundCacheFileGestalt result;
            using (var stream = cache.TagCache.OpenTagCacheRead())
            {
                result = cache.Deserialize<SoundCacheFileGestalt>(null, blamTag);
            }
            return result;
        }

        public static void Populate(CommandContextStack contextStack, CommandContext context, GameCache currentCache, GameCache portingCache)
        {
            /*
            var portTagCommand = new PortTagCommand(cacheContext, blamCache);

            context.AddCommand(portTagCommand);
            context.AddCommand(new ExtractXMACommand(cacheContext, blamCache));
            context.AddCommand(new ExtractSoundCommand(cacheContext, blamCache));
            context.AddCommand(new ExtractBitmapCommand(blamCache));
            context.AddCommand(new EditTagCommand(contextStack, blamCache));
            context.AddCommand(new ListBlamTagsCommand(cacheContext, blamCache));
            context.AddCommand(new PortArmorVariantCommand(cacheContext, blamCache));
            context.AddCommand(new PortMultiplayerEventsCommand(cacheContext, blamCache));
            context.AddCommand(new NameBlamTagCommand(cacheContext, blamCache));
            context.AddCommand(new MergeAnimationGraphsCommand(cacheContext, blamCache, portTagCommand));
            context.AddCommand(new PortMultiplayerScenarioCommand(cacheContext, blamCache, portTagCommand));
            context.AddCommand(new CopyTagNamesCommand(cacheContext, blamCache));
            */
		}
	}
}