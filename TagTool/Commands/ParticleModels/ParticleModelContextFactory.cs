using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.RenderModels
{
    static class ParticleModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, ParticleModel particleModel)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, particleModel);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, ParticleModel particleModel)
        {
            context.AddCommand(new DumpRenderGeometryCommand(cache, particleModel.Geometry, "particle"));
        }
    }
}
