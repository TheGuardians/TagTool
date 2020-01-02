using TagTool.Cache;
using TagTool.Commands.Editing;

namespace TagTool.Commands.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache cache)
        {
            var context = new CommandContext(contextStack.Context, "tags");

            context.AddCommand(new TestCommand(cache));
            context.AddCommand(new ListTagsCommand(cache));
            context.AddCommand(new EditTagCommand(contextStack, cache));

            return context;
        }
    }
}