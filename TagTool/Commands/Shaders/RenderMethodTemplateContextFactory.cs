using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Shaders
{
    public static class RenderMethodTemplateContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, RenderMethodTemplate render_method_template)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, render_method_template);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, RenderMethodTemplate render_method_template)
        {
            commandContext.AddCommand(new GenerateRenderMethodTemplate(cacheContext, tag, render_method_template));
        }
    }
}