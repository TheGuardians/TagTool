using TagTool.Cache;
using TagTool.Tags.Definitions.Gen2;

namespace TagTool.Commands.Gen2.Bitmaps
{
    static class BitmapContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, Bitmap bitm)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, bitm);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, Bitmap bitm)
        {
            commandContext.AddCommand(new ExtractBitmapCommand((GameCacheGen2)cache, tag, bitm));
        }
    }
}
