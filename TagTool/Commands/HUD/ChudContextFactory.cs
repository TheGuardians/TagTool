using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.HUD
{
    public static class ChudContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ChudDefinition chud_definition)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, chud_definition);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, ChudDefinition chud_definition)
        {
            commandContext.AddCommand(new UpdateChudTextCommand(chud_definition));
        }
    }
}