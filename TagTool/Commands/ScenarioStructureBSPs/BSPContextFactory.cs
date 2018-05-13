using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    static class BSPContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ScenarioStructureBsp bsp)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, bsp);

            return context;
        }

        public static void Populate(CommandContext commandContext, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ScenarioStructureBsp bsp)
        {
            commandContext.AddCommand(new CollisionTestCommand(cacheContext, tag, bsp));
            commandContext.AddCommand(new ResourceDataCommand(cacheContext, tag, bsp));
            commandContext.AddCommand(new ExtractRenderGeometryCommand(cacheContext, bsp));
            commandContext.AddCommand(new DumpRenderGeometryCommand(cacheContext, bsp.Geometry, "Decorator"));
            commandContext.AddCommand(new DumpRenderGeometryCommand(cacheContext, bsp.Geometry2, "Bsp"));
            commandContext.AddCommand(new DumpMoppCommand(cacheContext, bsp));
        }
    }
}