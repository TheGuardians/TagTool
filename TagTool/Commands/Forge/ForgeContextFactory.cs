using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Forge
{
    static class ForgeContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag instance, ForgeGlobalsDefinition definition)
        {
            var groupName = instance.Group.ToString();

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", instance.Index, groupName));

            Populate(context, cache, instance, definition);

            return context;
        }

        public static void Populate(CommandContext context, GameCache cache, CachedTag instance, ForgeGlobalsDefinition definition)
        {
            context.AddCommand(new ParseItemsXmlCommand(cache, instance, definition));
        }
    }
}
