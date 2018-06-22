using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ModelAnimationGraphs
{
    static class AnimationContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ModelAnimationGraph animation)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, animation);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ModelAnimationGraph animation)
        {
            context.AddCommand(new GetResourceInfoCommand(cacheContext, tag, animation));
            context.AddCommand(new ResourceDataCommand(cacheContext, tag, animation));
            context.AddCommand(new SortModesCommand(cacheContext, animation));
        }
    }
}