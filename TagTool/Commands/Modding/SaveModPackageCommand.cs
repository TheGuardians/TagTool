using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Commands.Common;
using System;

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
			var path = Cache.BaseCacheReference.Directory.FullName;
			path = path.Substring(0, path.Length - 4);

			if (args.Count != 0)
				path = args[0];
			else
				path += "mods\\downloads\\" + Cache.BaseModPackage.Metadata.Name + ".pak";

			var file = new System.IO.FileInfo(path);

            if (!Cache.SaveModPackage(file))
                return new TagToolError(CommandError.OperationFailed, "Failed to save mod package");
			else
				Console.WriteLine("ModPackage saved to \"" + path + "\".");

			return true;
        }
    }
}