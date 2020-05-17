using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    static class RenderMethodContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, RenderMethod renderMethod)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, renderMethod);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, RenderMethod renderMethod)
        {
            context.AddCommand(new ListArgumentsCommand(cache, tag, renderMethod));
            context.AddCommand(new SetArgumentCommand(cache, tag, renderMethod));
            context.AddCommand(new ListBitmapsCommand(cache, tag, renderMethod));
            context.AddCommand(new SpecifyBitmapsCommand(cache, tag, renderMethod));
            context.AddCommand(new PopulateParametersCommand(cache, tag, renderMethod));
        }
    }
}
