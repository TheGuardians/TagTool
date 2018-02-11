using TagTool.Cache;
using TagTool.Commands;
using TagTool.TagDefinitions;

namespace TagTool.Commands.Models
{
    static class ModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, Model model)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, model);

            return context;
        }

        public static void Populate(CommandContext context, GameCacheContext cacheContext, CachedTagInstance tag, Model model)
        {
            context.AddCommand(new ListVariantsCommand(cacheContext, model));
            context.AddCommand(new ExtractModelCommand(cacheContext, model));
            context.AddCommand(new ExtractBitmapsCommand(cacheContext, tag, model));
        }
    }
}
