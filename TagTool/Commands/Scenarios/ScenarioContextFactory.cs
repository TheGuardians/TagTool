using BlamCore.Cache;
using BlamCore.TagDefinitions;

namespace TagTool.Commands.Scenarios
{
    static class ScnrContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, Scenario scenario)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, scenario);

            return context;
        }

        public static void Populate(CommandContext context, GameCacheContext cacheContext, CachedTagInstance tag, Scenario scenario)
        {
            context.AddCommand(new CopyForgePaletteCommand(cacheContext, scenario));
            context.AddCommand(new ExtractScriptsCommand(cacheContext, tag, scenario));
        }
    }
}