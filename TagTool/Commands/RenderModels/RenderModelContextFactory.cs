using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.RenderModels
{
    static class RenderModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, RenderModel renderModel)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, renderModel);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, RenderModel renderModel)
        {
            context.AddCommand(new SpecifyShadersCommand(cache, tag, renderModel));
            context.AddCommand(new DumpRenderGeometryCommand(cache, renderModel.Geometry));
            context.AddCommand(new ReplaceRenderGeometryCommand(cache, tag, renderModel));
            context.AddCommand(new ExtractModelCommand(cache, renderModel));
            context.AddCommand(new ExtractBitmapsCommand(cache, renderModel));
            context.AddCommand(new ExtractBMFCommand(cache, renderModel));
        }
    }
}
