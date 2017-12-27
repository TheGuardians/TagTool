using BlamCore.Cache;
using BlamCore.Commands;
using BlamCore.TagDefinitions;

namespace TagTool.PixelShaders
{
    static class PixelShaderContextFactory
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, PixelShader pixelShader)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, pixelShader);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, PixelShader pixelShader)
        {
			commandContext.AddCommand(new CompileCommand(cacheContext, tag, pixelShader));
			commandContext.AddCommand(new DisassembleCommand(cacheContext, tag, pixelShader));
		}
	}
}
