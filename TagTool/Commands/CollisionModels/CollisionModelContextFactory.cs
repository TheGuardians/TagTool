using TagTool.Cache;
using TagTool.Commands;
using TagTool.TagDefinitions;

namespace TagTool.Commands.CollisionModels
{
    public static class CollisionModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, CollisionModel definition)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, definition);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, CollisionModel definition)
        {
            commandContext.AddCommand(new ExtractModelCommand(definition));
        }
    }
}