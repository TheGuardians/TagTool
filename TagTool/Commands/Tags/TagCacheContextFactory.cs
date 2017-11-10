using BlamCore.Cache;
using TagTool.Commands.Bitmaps;
using TagTool.Commands.CollisionModels;
using TagTool.Commands.Core;
using TagTool.Commands.Definitions;
using TagTool.Commands.Editing;
using TagTool.Commands.Files;
using TagTool.Commands.PhysicsModels;
using TagTool.Commands.RenderModels;
using TagTool.Commands.Strings;

namespace TagTool.Commands.Tags
{
    public static class TagCacheContextFactory
    {
        public static CommandContext Create(CommandContextStack stack, GameCacheContext cacheContext)
        {
            var context = new CommandContext(stack.Context, "tags");

            context.AddCommand(new HelpCommand(stack));
            context.AddCommand(new ClearCommand());
            context.AddCommand(new DumpLogCommand());
            context.AddCommand(new EchoCommand());
            context.AddCommand(new SetLocaleCommand());
            context.AddCommand(new CleanCsvFileCommand(cacheContext));
            context.AddCommand(new TagDependencyCommand(cacheContext));
            context.AddCommand(new ExtractTagCommand(cacheContext));
            context.AddCommand(new ImportTagCommand(cacheContext));
            context.AddCommand(new GetTagInfoCommand(cacheContext));
            context.AddCommand(new ListTagsCommand(cacheContext));
            context.AddCommand(new GetMapInfoCommand());
            context.AddCommand(new DuplicateTagCommand(cacheContext));
            context.AddCommand(new GetTagAddressCommand());
            context.AddCommand(new TagResourceCommand());
            context.AddCommand(new DeleteTagCommand(cacheContext));
            context.AddCommand(new CleanCacheFilesCommand(cacheContext));
            context.AddCommand(new TestCommand(cacheContext));
            context.AddCommand(new ListUnusedTagsCommand(cacheContext));
            context.AddCommand(new ListNullTagsCommand(cacheContext));
            context.AddCommand(new CreateTagCommand(cacheContext));
            context.AddCommand(new ExtractAllTagsCommand(cacheContext));
            context.AddCommand(new EditTagCommand(stack, cacheContext));
            context.AddCommand(new CollisionModelTestCommand(cacheContext));
            context.AddCommand(new PhysicsModelTestCommand(cacheContext));
            context.AddCommand(new StringIdCommand(cacheContext));
            context.AddCommand(new ListAllStringsCommand(cacheContext));
            context.AddCommand(new GenerateTagStructuresCommand(cacheContext));
            context.AddCommand(new RenderModelTestCommand(cacheContext));
            context.AddCommand(new ConvertPluginsCommand(cacheContext));
            context.AddCommand(new GenerateTagNamesCommand(cacheContext));
            context.AddCommand(new NameTagCommand(cacheContext));
            context.AddCommand(new SaveTagNamesCommand(cacheContext));
            context.AddCommand(new MatchTagsCommand(cacheContext));
            context.AddCommand(new ConvertTagCommand(cacheContext));
            context.AddCommand(new UpdateMapFilesCommand(cacheContext));
            context.AddCommand(new ExtractBitmapsCommand(cacheContext));
            context.AddCommand(new GenerateAssemblyPluginsCommand());

            return context;
        }
    }
}