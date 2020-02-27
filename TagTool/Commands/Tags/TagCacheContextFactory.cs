using TagTool.Cache;
using TagTool.Commands.Editing;
using TagTool.Commands.Common;
using TagTool.Commands.Definitions;
using TagTool.Commands.Files;
using TagTool.Commands.Strings;
using TagTool.Commands.Sounds;
using TagTool.Commands.Porting;
using TagTool.Commands.Modding;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.PhysicsModels;
using TagTool.Commands.CollisionModels;

namespace TagTool.Commands.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack contextStack, GameCache cache)
        {
            var context = new CommandContext(contextStack.Context, "tags");
            Populate(contextStack, context, cache);
            return context;
        }

        public static void Populate(CommandContextStack contextStack, CommandContext context, GameCache cache)
        {
            context.AddCommand(new TestCommand(cache));

            context.AddCommand(new DumpLogCommand());
            context.AddCommand(new RunCommands(contextStack));
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
            context.AddCommand(new DeleteTagCommand(cache));
            context.AddCommand(new ListNullTagsCommand(cache));
            context.AddCommand(new ListUnnamedTagsCommand(cache));
            context.AddCommand(new ExtractBitmapsCommand(cache));

            // Halo Online Specific Commands
            if (cache is GameCacheHaloOnlineBase)
            {
                var hoCache = cache as GameCacheHaloOnlineBase;
                context.AddCommand(new SaveTagNamesCommand(hoCache));
                context.AddCommand(new SaveModdedTagsCommand(hoCache));
                context.AddCommand(new CreateTagCommand(hoCache));
                context.AddCommand(new ImportTagCommand(hoCache));
                context.AddCommand(new TagDependencyCommand(hoCache));
                context.AddCommand(new TagResourceCommand(hoCache));
                context.AddCommand(new ListUnusedTagsCommand(hoCache));
                context.AddCommand(new GetTagInfoCommand(hoCache));
                context.AddCommand(new GetTagAddressCommand());
                context.AddCommand(new ExtractTagCommand(hoCache));
                context.AddCommand(new ExtractAllTagsCommand(hoCache));
                context.AddCommand(new ExportTagModCommand(hoCache));

                // modding commands
                context.AddCommand(new ApplyModPackageCommand(hoCache));
                context.AddCommand(new CreateCharacterType(cache));
                context.AddCommand(new ExportModPackageCommand(hoCache));

                context.AddCommand(new UpdateMapFilesCommand(cache));

                context.AddCommand(new PhysicsModelTestCommand(cache));
                context.AddCommand(new CollisionModelTestCommand(hoCache));
            }

            // porting related
            context.AddCommand(new UseAudioCacheCommand());
            context.AddCommand(new OpenCacheFileCommand(contextStack, cache));
        }
    }
}