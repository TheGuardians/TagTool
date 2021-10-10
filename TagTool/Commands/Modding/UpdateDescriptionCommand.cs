using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Commands.Common;

namespace TagTool.Commands.Modding
{
    class UpdateDescriptionCommand : Command
    {
        private GameCacheModPackage Cache;
        private CommandContextStack ContextStack { get; }

        public UpdateDescriptionCommand(CommandContextStack contextStack, GameCacheModPackage cache) :
            base(true,

                "UpdateDescription",
                "Update description of mod package.\n",
                "UpdateDescription",
                "Update description of mod package.\n")
        {
            ContextStack = contextStack;
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return new TagToolError(CommandError.ArgCount);

            Cache.BaseModPackage.CreateDescription(ContextStack, IgnoreArgumentVariables);

            return true;

        }
    }
}