using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Commands.Editing
{
    public abstract class BlockManipulationCommand : Command
    {
        private CommandContextStack ContextStack { get; }
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private TagStructureInfo Structure { get; set; }
        private object Owner { get; set; }

        protected BlockManipulationCommand(CommandContextStack contextStack, GameCache cache, CachedTag tag, TagStructureInfo structure, object owner,
            bool inherit, string name, string description, string usage, string helpMessage = "", 
            bool ignoreVars = false, string examples = "")
            : base(inherit, name, description, usage, helpMessage, ignoreVars, examples)
        {
            ContextStack = contextStack;
            Cache = cache;
            Tag = tag;
            Structure = structure;
            Owner = owner;
        }

        public void ContextReturn(CommandContext previousContext, object previousOwner, TagStructureInfo previousStructure)
        {
            while (ContextStack.Context != previousContext) ContextStack.Pop();
            Owner = previousOwner;
            Structure = previousStructure;
        }
    }
}
