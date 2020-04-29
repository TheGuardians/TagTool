using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.CollisionModels
{
    public static class CollisionModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, CollisionModel definition)
        {
            var groupName = tag.Group.ToString();
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, definition);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, CollisionModel definition)
        {
            commandContext.AddCommand(new ExtractModelCommand(cache, definition));
        }
    }
}