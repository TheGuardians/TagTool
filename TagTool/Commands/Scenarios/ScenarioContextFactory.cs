using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Scenarios
{
    static class ScnrContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Scenario scenario)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, scenario);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, Scenario scenario)
        {
            context.AddCommand(new CopyForgePaletteCommand(cache, scenario));
            context.AddCommand(new ExtractScriptsCommand(cache, tag, scenario));
            context.AddCommand(new DumpScriptsCommand(cache, scenario));
            context.AddCommand(new ImportScriptsCommand(scenario));
            context.AddCommand(new CompileScriptsCommand(cache, scenario));
            context.AddCommand(new ListScriptsCommand(cache, tag, scenario));
            context.AddCommand(new ExtractZonesAreasModelCommand(cache, scenario));
            context.AddCommand(new ConvertInstancedGeometryCommand(cache, scenario));
        }
    }
}