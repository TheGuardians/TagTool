using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.Commands.Modding
{
    class OpenModPackageCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public OpenModPackageCommand(HaloOnlineCacheContext cacheContext) :
            base(false,
                
                "OpenModPackage",
                "",
                
                "OpenModPackage <File>",
                
                "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var modPackage = new ModPackage();
            modPackage.Load(new FileInfo(args[0]));

            return true;
        }
    }
}
