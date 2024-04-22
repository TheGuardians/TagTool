using TagTool.Cache;
using System.Collections.Generic;
using TagTool.Commands.Common;
using System.IO;
using System;
using TagTool.Extensions;

namespace TagTool.Commands.Modding
{
    public class DeleteModFilesCommand : Command
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
            Console.WriteLine("Deleting " + Cache.BaseModPackage.Files.Count + " Files");
            Cache.BaseModPackage.Files.Clear();

            return true;
        }
    }
}
