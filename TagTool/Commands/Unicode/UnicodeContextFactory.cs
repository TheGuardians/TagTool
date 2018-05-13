using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Unicode
{
    static class UnicodeContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, MultilingualUnicodeStringList unic)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, unic);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, MultilingualUnicodeStringList unic)
        {
            if (cacheContext.StringIdCache == null)
                return;

            context.AddCommand(new ListStringsCommand(cacheContext, unic));
            context.AddCommand(new GetStringCommand(cacheContext, tag, unic));
            context.AddCommand(new SetStringCommand(cacheContext, tag, unic));
            context.AddCommand(new RemoveStringCommand(cacheContext, tag, unic));
        }
    }
}