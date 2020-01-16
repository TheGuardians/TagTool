using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    static class BSPContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ScenarioStructureBsp bsp)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, bsp);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cacheContext, CachedTag tag, ScenarioStructureBsp bsp)
        {
            /*
            commandContext.AddCommand(new CollisionTestCommand(cacheContext, tag, bsp));
            commandContext.AddCommand(new ResourceDataCommand(cacheContext, tag, bsp));
            commandContext.AddCommand(new ExtractCollisionGeometryCommand(cacheContext, bsp));
            commandContext.AddCommand(new ExtractPathfindingGeometryCommand(cacheContext, bsp));
            commandContext.AddCommand(new GenerateJumpHintsCommand(cacheContext, bsp));
            commandContext.AddCommand(new ExtractRenderGeometryCommand(cacheContext, bsp));
            commandContext.AddCommand(new DumpRenderGeometryCommand(cacheContext, bsp.DecoratorGeometry, "Decorator"));
            commandContext.AddCommand(new DumpRenderGeometryCommand(cacheContext, bsp.Geometry, "Bsp"));
            commandContext.AddCommand(new DumpMoppCommand(cacheContext, bsp));
            commandContext.AddCommand(new MoppDataCommand(cacheContext, bsp));
            commandContext.AddCommand(new LocalizeTagResourcesCommand(cacheContext, bsp, tag));
            */
        }
    }
}