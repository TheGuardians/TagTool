using TagTool.Cache;
using TagTool.Tags.Definitions.Gen2;

namespace TagTool.Commands.Gen2.ModelAnimationGraphs
{
    static class AnimationContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ModelAnimationGraph jmad)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, jmad);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, ModelAnimationGraph jmad)
        {
            commandContext.AddCommand(new ExtractAnimationCommand(cache, jmad));
        }
    }
}
