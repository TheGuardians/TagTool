using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Commands.Geometry;

namespace TagTool.Commands.StructureDesigns
{
    static class StructureDesignContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, StructureDesign sddt)
        {
            var groupName = tag.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(context, cache, tag, sddt);

            return context;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, StructureDesign sddt)
        {
            commandContext.AddCommand(new ExtractStructureDesignCommand(cache, sddt));
            commandContext.AddCommand(new MoppDataCommand(sddt));
        }
    }
}