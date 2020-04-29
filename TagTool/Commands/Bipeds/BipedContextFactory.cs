using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Bipeds
{
    static class BipedContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Biped biped)
        {
            var groupName = tag.Group.ToString();
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, biped);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, Biped biped)
        {
        }
    }
}