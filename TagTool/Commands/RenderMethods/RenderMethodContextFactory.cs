using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.RenderMethods
{
    static class RenderMethodContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext info, CachedTagInstance tag, RenderMethod renderMethod)
        {
            var groupName = info.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, info, tag, renderMethod);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext info, CachedTagInstance tag, RenderMethod renderMethod)
        {
            context.AddCommand(new ListArgumentsCommand(info, tag, renderMethod));
            context.AddCommand(new SetArgumentCommand(info, tag, renderMethod));
            context.AddCommand(new ListBitmapsCommand(info, tag, renderMethod));
            context.AddCommand(new SpecifyBitmapsCommand(info, tag, renderMethod));
        }
    }
}
