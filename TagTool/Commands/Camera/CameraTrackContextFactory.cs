using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Camera
{
    static class CameraTrackContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, CameraTrack trak)
        {
            var groupName = tag.Group.ToString();
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, trak);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, CameraTrack trak)
        {
            commandContext.AddCommand(new AdjustPositionCommand(cache, tag, trak));
        }
    }
}
