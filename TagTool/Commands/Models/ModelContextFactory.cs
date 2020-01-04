using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Models
{
    static class ModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Model model)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, model);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, Model model)
        {
            context.AddCommand(new ListVariantsCommand(cache, model));
            //context.AddCommand(new ExtractModelCommand(cache, model));
            //context.AddCommand(new ExtractBitmapsCommand(cacheContext, tag, model));
        }
    }
}
