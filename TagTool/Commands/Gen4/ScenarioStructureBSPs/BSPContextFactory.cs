using TagTool.Cache;
using TagTool.Tags.Definitions.Gen4;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.Gen4.ScenarioStructureBSPs
{
    static class BSPContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ScenarioStructureBsp bsp)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, bsp);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, ScenarioStructureBsp bsp)
        {
            commandContext.AddCommand(new ExtractRenderGeometryCommand(cache, bsp, tag));
        }
    }
}