using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Commands.Common;

namespace TagTool.Commands.Modding
{
    class ListModFilesCommand : Command
    {
        public GameCacheModPackage Cache { get; }

        public ListModFilesCommand(GameCacheModPackage cache) :
            base(true,
                "ListModFiles",
                "Lists the files in the current mod package",
                "ListModFiles",
                "Lists the files in the current mod package")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            foreach(var entry in Cache.BaseModPackage.Files)
            {
                Console.WriteLine($"{entry.Key} {entry.Value.Length/1024.0f/1024.0f:0.00} MB");
            }

            return true;
        }
    }
}
