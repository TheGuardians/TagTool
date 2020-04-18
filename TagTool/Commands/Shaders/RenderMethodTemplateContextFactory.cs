using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Shaders
{
    public static class RenderMethodTemplateContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, RenderMethodTemplate render_method_template)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, render_method_template);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, RenderMethodTemplate render_method_template)
        {
            //commandContext.AddCommand(new GenerateRenderMethodTemplate(cache, tag, render_method_template));
        }
    }
}