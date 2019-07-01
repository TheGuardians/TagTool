using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.Commands.Modding
{
    class UpgradeModPackage : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public UpgradeModPackage(HaloOnlineCacheContext cacheContext) :
            base(false,
                
                "UpgradeModPackage",
                "Upgrade a Version 1 mod package to the extended mod package (Version 2). \n",

                "UpgradeModPackage <File> [Destination]",

                "Upgrade a Version 1 mod package to the extended mod package (Version 2). \n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            string sourcePackagePath;
            string destPackagePath;

            if(args.Count == 1)
            {
                sourcePackagePath = args[0];
                destPackagePath = sourcePackagePath;
            }
            else
            {
                sourcePackagePath = args[0];
                destPackagePath = args[0];
            }

            if (!File.Exists(sourcePackagePath))
            {
                Console.WriteLine("Source package not found!");
                return false;
            }

            var sourcePackage = new ModPackageSimplified();
            sourcePackage.Load(new FileInfo(sourcePackagePath));

            var destPackage = new ModPackageExtended();

            var metadata = destPackage.Metadata;
            metadata.Author = sourcePackage.Metadata.Author;
            metadata.Description = sourcePackage.Metadata.Description;
            metadata.Name = sourcePackage.Metadata.Name;
            metadata.BuildDate = DateTime.Now.ToFileTime();

            destPackage.Tags = sourcePackage.Tags;
            destPackage.TagsStream = sourcePackage.TagsStream;
            destPackage.TagNames = sourcePackage.TagNames;

            destPackage.Resources = sourcePackage.Resources;
            destPackage.ResourcesStream = sourcePackage.ResourcesStream;

            destPackage.MapFileStreams = sourcePackage.CacheStreams;

            destPackage.Save(new FileInfo(destPackagePath));
            Console.WriteLine("Done!");
            return true;
        }
    }
}
