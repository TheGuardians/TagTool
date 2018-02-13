using TagTool.Cache;
using TagTool.Commands;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.VertexShaders
{
	static class VertexShaderContextFactory
	{
		public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, VertexShader vertexShader)
		{
			var groupName = cacheContext.GetString(tag.Group.Name);
			var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

			Populate(commandContext, cacheContext, tag, vertexShader);

			return commandContext;
		}

		public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, VertexShader vertexShader)
		{
			commandContext.AddCommand(new CompileCommand(cacheContext, tag, vertexShader));
			commandContext.AddCommand(new DisassembleCommand(cacheContext, tag, vertexShader));
		}
	}
}
