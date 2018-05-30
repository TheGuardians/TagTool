using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Files
{
    static class VFilesContextFactory
    {
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, VFilesList definition)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cacheContext, tag, definition);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext cacheContext, CachedTagInstance tag, VFilesList definition)
        {
            context.AddCommand(new ListFilesCommand(definition));
            context.AddCommand(new ExtractFileCommand(definition));
            context.AddCommand(new ExtractFilesCommand(definition));
            context.AddCommand(new ReplaceFileCommand(cacheContext, tag, definition));
            context.AddCommand(new ReplaceAllFilesCommand(cacheContext, tag, definition));
            context.AddCommand(new AddFileCommand(cacheContext, tag, definition));
        }
    }
}
