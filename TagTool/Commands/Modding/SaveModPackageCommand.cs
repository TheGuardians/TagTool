using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Commands.Common;

namespace TagTool.Commands.Modding
{
    class SaveModPackageCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public SaveModPackageCommand(GameCacheModPackage cache) :
            base(true,
                "SaveModPackage",
                "Save current mod package cache to a pack",
                "SaveModPackage <.pak destination>",
                "Save current mod package cache to a pack")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var path = args[0];

            var file = new System.IO.FileInfo(path);

            if (!Cache.SaveModPackage(file))
                return new TagToolError(CommandError.OperationFailed, "Failed to save mod package");

            return true;
        }
    }
}