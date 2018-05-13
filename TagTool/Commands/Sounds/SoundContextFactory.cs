using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Sounds
{
    static class SoundContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Sound sound)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, sound);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Sound sound)
        {
            commandContext.AddCommand(new ImportSoundCommand(cacheContext, tag, sound));
            commandContext.AddCommand(new ResourceDataCommand(cacheContext, tag, sound));
        }
    }
}
