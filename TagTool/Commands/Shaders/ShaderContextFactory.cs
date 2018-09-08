using TagTool.Cache;

namespace TagTool.Commands.Shaders
{
    public static class ShaderContextFactory<T>
    {
        public static CommandContext Create(CommandContext parent, GameCacheContext cacheContext, CachedTagInstance tag, T shader)
        {
            var groupName = cacheContext.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cacheContext, tag, shader);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCacheContext cacheContext, CachedTagInstance tag, T shader)
        {
            commandContext.AddCommand(new GenerateShader<T>(cacheContext, tag, shader));
            commandContext.AddCommand(new CompileCommand<T>(cacheContext, tag, shader));
            commandContext.AddCommand(new DisassembleCommand<T>(cacheContext, tag, shader));
        }
    }
}