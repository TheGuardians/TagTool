using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Sounds
{
    static class SoundContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Sound sound)
        {
            var groupName = tag.Group.ToString();
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, sound);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, Sound sound)
        {
            commandContext.AddCommand(new ImportSoundCommand(cache, tag, sound));
            commandContext.AddCommand(new ExtractSoundCommand(cache, tag, sound));

            if (cache.GetType() == typeof(GameCacheGen3))
            {
                var h3Cache = cache as GameCacheGen3;
                commandContext.AddCommand(new ExtractXMACommand(h3Cache, tag, sound));
            }
        }
    }
}
