using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Unicode
{
    static class UnicodeContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, MultilingualUnicodeStringList unic)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, unic);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, MultilingualUnicodeStringList unic)
        {
            if (cache.StringTable == null)
                return;

            context.AddCommand(new ListStringsCommand(cache, unic));
            context.AddCommand(new GetStringCommand(cache, tag, unic));
            context.AddCommand(new SetStringCommand(cache, tag, unic));
            context.AddCommand(new RemoveStringCommand(cache, tag, unic));
        }
    }
}