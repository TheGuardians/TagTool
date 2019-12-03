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
        public static CommandContext Create(CommandContext parent, HaloOnlineCacheContext cacheContext, CachedTagInstance instance, ForgeGlobalsDefinition definition)
        {
            var groupName = cacheContext.GetString(instance.Group.Name);

            var context = new CommandContext(parent,
                string.Format("{0:X8}.{1}", instance.Index, groupName));

            Populate(context, cacheContext, instance, definition);

            return context;
        }

        public static void Populate(CommandContext context, HaloOnlineCacheContext cacheContext, CachedTagInstance instance, ForgeGlobalsDefinition definition)
        {
            context.AddCommand(new ParseItemsXmlCommand(cacheContext, instance, definition));
        }
    }
}
