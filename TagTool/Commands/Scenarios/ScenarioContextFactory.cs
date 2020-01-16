using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    static class ScnrContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Scenario scenario)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, scenario);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, Scenario scenario)
        {
            /*
            context.AddCommand(new CopyForgePaletteCommand(cacheContext, scenario));
            context.AddCommand(new ExtractScriptsCommand(cacheContext, tag, scenario));
            context.AddCommand(new DumpScriptsCommand(cacheContext, scenario));
            context.AddCommand(new CompileScriptsCommand(cacheContext, scenario));
            context.AddCommand(new ListScriptsCommand(cacheContext, tag, scenario));
            context.AddCommand(new ExtractZonesAreasModelCommand(cacheContext, scenario));
            */
        }
    }
}