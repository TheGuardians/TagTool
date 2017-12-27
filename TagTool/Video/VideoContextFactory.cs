using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.TagDefinitions;

namespace TagTool.Video
{
    static class VideoContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, Bink bink)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, bink);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, Bink bink)
        {
            commandContext.AddCommand(new ExtractBinkFileCommand(cacheContext, tag, bink));
        }
    }
}
