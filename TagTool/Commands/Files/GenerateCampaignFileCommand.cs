using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Commands.Files
{
    class GenerateCampaignFileCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        public GenerateCampaignFileCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "GenerateCampaignFile",
                  "Generate the halo3.campaign file for the specificed map info folder",

                  "GenerateCampaignFile <MapInfo Directory>",

                  "Generate the halo3.campaign file for the specificed map info folder")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
            {
                return false;
            }

            var fileName = $"halo3.campaign";
            var mapInfoDir = new DirectoryInfo(args[0]);

            var srcFile = new FileInfo(Path.Combine(mapInfoDir.FullName, fileName));
            var destFile = new FileInfo(Path.Combine(CacheContext.Directory.FullName, fileName));

            using (var stream = srcFile.Open(FileMode.Open, FileAccess.Read))
            using (var reader = new EndianReader(stream))
            using (var destStream = destFile.Create())
            using (var writer = new EndianWriter(destStream))
            {

                Blf campaignBlf = new Blf(CacheVersion.Halo3Retail);
                campaignBlf.Read(reader);
                campaignBlf.ConvertBlf(CacheContext.Version);
                campaignBlf.Write(writer);
            }
            Console.WriteLine("Done!");
            return true;
        }

    }
}