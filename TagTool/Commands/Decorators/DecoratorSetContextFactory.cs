using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Decorators
{
    class DecoratorSetContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, DecoratorSet decorator)
        {
            var groupName = tag.Group.ToString();
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, decorator);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, DecoratorSet decorator)
        {
            commandContext.AddCommand(new ExtractModelCommand(cache, tag, decorator));
        }
    }
}
