using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.ModelAnimationGraphs
{
    static class AnimationContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ModelAnimationGraph animation)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, animation);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, ModelAnimationGraph animation)
        {
            context.AddCommand(new SortModesCommand(cache, animation));
            context.AddCommand(new ApplySprintFixupsCommand(cache));
            context.AddCommand(new AddAnimationCommand(cache, animation, tag));
            context.AddCommand(new ReplaceFPAnimationCommand(cache, animation, tag));
        }
    }
}