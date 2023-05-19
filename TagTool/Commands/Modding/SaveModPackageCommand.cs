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
            string path;
            if (Cache.BaseCacheReference.Directory != null)
            {
                path = Cache.BaseCacheReference.Directory.FullName;
                path = path.Substring(0, path.Length - 4);

                if (args.Count != 0)
                    path = args[0];
                else if (Cache.ModPackageFile != null)
                    path = Cache.ModPackageFile.FullName;
                else
                    path += "mods\\downloads\\" + Cache.DisplayName;
            }
            else
            {
                if (args.Count < 1)
                    return new TagToolError(CommandError.ArgCount, "To save a mod pak from within another mod pak, you must provide a path.");

                path = args[0];
            }

            if (!path.EndsWith(".pak"))
                path += ".pak";

            var file = new System.IO.FileInfo(path);

            if (!Cache.SaveModPackage(file))
                return new TagToolError(CommandError.OperationFailed, "Failed to save mod package.");
			else
				Console.WriteLine("ModPackage saved to \"" + path + "\".");

			Program.ReportElapsed();
			return true;
        }
    }
}