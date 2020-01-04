using TagTool.Cache;
using TagTool.Commands.Editing;
using TagTool.Commands.Common;
using TagTool.Commands.Definitions;
using TagTool.Commands.Files;

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

            // Halo Online Specific Commands
            if(cache.GetType() == typeof(GameCacheContextHaloOnline))
            {

            }


            return context;
        }
    }
}