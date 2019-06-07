using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;

namespace TagTool.Commands.Modding
{
    class MergeModPackagesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public MergeModPackagesCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "MergeModPackages",
                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n",

                "MergeModPackages <File 1> <File 2>",

                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var modPackage1 = new ModPackage();
            var modPackage2 = new ModPackage();

            modPackage1.Load(new FileInfo(args[0]));
            modPackage2.Load(new FileInfo(args[1]));


            ModPackage resultPackage = new ModPackage();


            //
            // Step 1: Copy over tags, names and resources
            //

            // Assume current cache is not modded.
            int lastTagIndex = CacheContext.TagCache.Index.Last().Index;

            int lastTagIndexPak1 = modPackage1.Tags.Index.Last().Index;
            int lastTagIndexPak2 = modPackage2.Tags.Index.Last().Index;


            for(int i = 0; i < lastTagIndex; i++)
            {
                if (modPackage1.Tags.Index.Contains(i))
                {
                    // TODO: apply package 1 tag
                    var newTag = resultPackage.Tags.AllocateTag();
                    // Add the tag name to the new mod package
                    if (modPackage1.TagNames.ContainsKey(i))
                    {
                        resultPackage.TagNames.Add(newTag.Index, modPackage1.TagNames[i]);
                    }
                }
                else if (modPackage2.Tags.Index.Contains(i))
                {
                    // TODO: apply package 2 tag
                    var newTag = resultPackage.Tags.AllocateTag();
                    // Add the tag name to the new mod package
                    if (modPackage2.TagNames.ContainsKey(i))
                    {
                        resultPackage.TagNames.Add(newTag.Index, modPackage2.TagNames[i]);
                    }
                }
                else
                {
                    resultPackage.Tags.AllocateTag();
                    continue;
                }
            }



            //
            // Step 2: Merge and fixup map files (very hard)
            //

            return true;
        }
    }
}
