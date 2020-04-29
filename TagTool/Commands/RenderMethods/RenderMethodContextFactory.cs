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

        public static void Populate(CommandContext context, GameCache info, CachedTag tag, RenderMethod renderMethod)
        {
            context.AddCommand(new ListArgumentsCommand(info, tag, renderMethod));
            context.AddCommand(new SetArgumentCommand(info, tag, renderMethod));
            context.AddCommand(new ListBitmapsCommand(info, tag, renderMethod));
            context.AddCommand(new SpecifyBitmapsCommand(info, tag, renderMethod));
        }
    }
}
