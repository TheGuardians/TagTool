using TagTool.Cache;
using TagTool.Commands.Editing;
using TagTool.Commands.Common;
using TagTool.Commands.Definitions;
using TagTool.Commands.Files;
using TagTool.Commands.Strings;
using TagTool.Commands.Sounds;

namespace TagTool.Commands.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache cache)
        {
            var context = new CommandContext(contextStack.Context, "tags");

            context.AddCommand(new TestCommand(cache));

            context.AddCommand(new DumpLogCommand());
            context.AddCommand(new ClearCommand());
            context.AddCommand(new EchoCommand());
            context.AddCommand(new HelpCommand(contextStack));
            context.AddCommand(new SetLocaleCommand());
            context.AddCommand(new StopwatchCommand());
            context.AddCommand(new ConvertPluginsCommand(cache));
            context.AddCommand(new ListTagsCommand(cache));
            context.AddCommand(new EditTagCommand(contextStack, cache));
            context.AddCommand(new GenerateCampaignFileCommand(cache));
            context.AddCommand(new NameTagCommand(cache));
            context.AddCommand(new ForEachCommand(contextStack, cache));
            context.AddCommand(new ListAllStringsCommand(cache));
            context.AddCommand(new StringIdCommand(cache));
            context.AddCommand(new GenerateAssemblyPluginsCommand());
            context.AddCommand(new DuplicateTagCommand(cache));
            context.AddCommand(new CreateTagCommand(cache));
            context.AddCommand(new DeleteTagCommand(cache));
            context.AddCommand(new ListNullTagsCommand(cache));

            // Halo Online Specific Commands
            if (cache.GetType() == typeof(GameCacheContextHaloOnline))
            {
                var hoCache = cache as GameCacheContextHaloOnline;
                context.AddCommand(new SaveTagNamesCommand(hoCache));
                context.AddCommand(new SaveModdedTagsCommand(hoCache));
            }

            // porting related
            context.AddCommand(new UseAudioCacheCommand());
            context.AddCommand(new UpdateMapFilesCommand(cache));
            
            return context;
        }
    }
}