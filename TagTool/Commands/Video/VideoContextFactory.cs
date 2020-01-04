using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Video
{
    static class VideoContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Bink bink)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, bink);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, Bink bink)
        {
            commandContext.AddCommand(new ExtractBinkFileCommand(cache, tag, bink));
        }
    }
}
