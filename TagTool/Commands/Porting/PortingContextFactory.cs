using System.IO;
using TagTool.Cache;
using TagTool.Commands.Porting.Gen2;
using TagTool.Commands.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    static class PortingContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache currentCache, GameCache portingCache)
        {
            var context = new CommandContext(contextStack.Context, portingCache.DisplayName + "\\tags");

            Populate(contextStack, context, currentCache, portingCache);

            // add tags command to the new cache
            TagCacheContextFactory.Populate(contextStack, context, currentCache);

            return context;
        }

        public static SoundCacheFileGestalt LoadSoundGestalt(GameCache cache, Stream cacheStream)
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

            SoundCacheFileGestalt result = cache.Deserialize<SoundCacheFileGestalt>(cacheStream, blamTag);

            return result;
        }

        public static void Populate(CommandContextStack contextStack, CommandContext context, GameCache currentCache, GameCache portingCache)
        {
            if (currentCache is GameCacheHaloOnlineBase hoCache)
            {
                if (portingCache is GameCacheGen3)
                {
                    var portTagCommand = new PortTagCommand(hoCache, portingCache);
                    context.AddCommand(portTagCommand);
                    context.AddCommand(new MergeAnimationGraphsCommand(hoCache, portingCache, portTagCommand));
                    context.AddCommand(new PortMultiplayerEventsCommand(hoCache, portingCache));
                    context.AddCommand(new PortMultiplayerScenarioCommand(hoCache, portingCache, portTagCommand));
                    context.AddCommand(new PortInstancedGeometryObjectCommand(hoCache, portingCache));
                    context.AddCommand(new PortClusterGeometryObjectCommand(hoCache, portingCache));
                    context.AddCommand(new DoNotReplaceGroupsCommand());
                }
                else if (portingCache is GameCacheGen2 gen2cache)
                {
                    var portTagCommand = new PortTagGen2Command(hoCache, gen2cache);
                    context.AddCommand(portTagCommand);
                }
                else if (portingCache is GameCacheGen1 gen1cache)
                {
                    var portTagCommand = new PortTagGen1Command(hoCache, gen1cache);
                    context.AddCommand(portTagCommand);
                }

                context.AddCommand(new DiffTagCommand(currentCache, portingCache));
            }
        }
    }
}