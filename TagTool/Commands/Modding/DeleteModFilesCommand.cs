using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Commands.Common;
using System.IO;
using System;
using TagTool.Extensions;

namespace TagTool.Commands.Modding
{
    class DeleteModFilesCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public DeleteModFilesCommand(GameCacheModPackage cache) :
            base(true,
                "DeleteModFiles",
                "Deletes all the Mod files in the package",
                "DeleteModFiles",
                "Deletes all the Mod files in the package")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var path = args[0];

            var directory = new DirectoryInfo(path);
            if (!directory.Exists)
            {
                Console.WriteLine($"ERROR: Directory does not exist.");
                return new TagToolError(CommandError.DirectoryNotFound);
            }

            Cache.BaseModPackage.Files.Clear();

            return true;
        }
    }
}
