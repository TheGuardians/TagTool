using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.RenderModels
{
    static class RenderModelContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext info, CachedTagInstance tag, RenderModel renderModel)
        {
            var groupName = info.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, info, tag, renderModel);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, RenderModel renderModel)
        {
            context.AddCommand(new SpecifyShadersCommand(cacheContext, tag, renderModel));
            context.AddCommand(new GetResourceInfoCommand(cacheContext, tag, renderModel));
            context.AddCommand(new ResourceDataCommand(cacheContext, tag, renderModel));
            context.AddCommand(new DumpRenderGeometryCommand(cacheContext, renderModel.Geometry));
            context.AddCommand(new ReplaceRenderGeometryCommand(cacheContext, tag, renderModel));
            context.AddCommand(new ExtractModelCommand(cacheContext, renderModel));
            context.AddCommand(new ExtractBitmapsCommand(cacheContext, tag, renderModel));
        }
    }
}
