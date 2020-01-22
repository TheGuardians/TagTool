using TagTool.Cache;

namespace TagTool.Commands.Shaders
{
    public static class ShaderContextFactory<T>
    {
        public static CommandContext Create(CommandContext parent, GameCache cache, CachedTag tag, T shader)
        {
            var groupName = cache.StringTable.GetString(tag.Group.Name);
            var commandContext = new CommandContext(parent, string.Format("{0:X8}.{1}", tag.Index, groupName));

            Populate(commandContext, cache, tag, shader);

            return commandContext;
        }

        public static void Populate(CommandContext commandContext, GameCache cache, CachedTag tag, T shader)
        {
            commandContext.AddCommand(new GenerateShader<T>(cache, tag, shader));
            commandContext.AddCommand(new CompileCommand<T>(cache, tag, shader));
            commandContext.AddCommand(new DisassembleCommand<T>(cache, tag, shader));
        }
    }
}