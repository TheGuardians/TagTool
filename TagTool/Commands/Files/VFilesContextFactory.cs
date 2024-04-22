using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    static class VFilesContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, VFilesList definition)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, definition);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag tag, VFilesList definition)
        {
            context.AddCommand(new ListFilesCommand(definition));
            context.AddCommand(new ExtractFileCommand(definition));
            context.AddCommand(new ExtractFilesCommand(definition));
            context.AddCommand(new ReplaceFileCommand(cache, tag, definition));
            context.AddCommand(new ReplaceAllFilesCommand(cache, tag, definition));
            context.AddCommand(new AddFileCommand(cache, tag, definition));
        }
    }
}
