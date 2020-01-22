using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.ScenarioLightmaps
{
    static class LightmapContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ScenarioLightmapBspData Lbsp)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, Lbsp);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, ScenarioLightmapBspData Lbsp)
        {
            commandContext.AddCommand(new DumpRenderGeometryCommand(cache, Lbsp.Geometry, "Lightmap"));
            commandContext.AddCommand(new ExtractRenderGeometryCommand(cache, Lbsp));
        }
    }
}