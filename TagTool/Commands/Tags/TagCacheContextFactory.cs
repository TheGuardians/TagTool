using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache cache)
        {
            var context = new CommandContext(contextStack.Context, "tags");

            context.AddCommand(new ListTagsCommand(cache));
            
            return context;
        }
    }
}