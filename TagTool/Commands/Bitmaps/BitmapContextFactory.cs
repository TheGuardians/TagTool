using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Bitmaps
{
    static class BitmapContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Bitmap bitmap)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, bitmap);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, Bitmap bitmap)
        {
            commandContext.AddCommand(new ExtractBitmapCommand(cache, tag, bitmap));
            //commandContext.AddCommand(new ImportBitmapCommand(cache, tag, bitmap));
        }
    }
}