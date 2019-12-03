using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Bipeds
{
    static class BipedContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Biped biped)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, biped);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Biped biped)
        {
        }
    }
}