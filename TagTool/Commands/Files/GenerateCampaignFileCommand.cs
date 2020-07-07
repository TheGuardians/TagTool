using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.IO;
using TagTool.BlamFile;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;

namespace TagTool.Commands.Files
{
    class GenerateCampaignFileCommand : Command
    {
        public GameCache Cache { get; }

        public GenerateCampaignFileCommand(GameCache cache)
            : base(true,

                  "GenerateCampaignFile",
                  "Generate the halo3.campaign file for the specificed map info folder",

                  "GenerateCampaignFile <MapInfo Directory>",

                  "Generate the halo3.campaign file for the specificed map info folder")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            var fileName = $"halo3.campaign";

            var mapInfoDir = new DirectoryInfo(args[0]);
            var srcFile = new FileInfo(Path.Combine(mapInfoDir.FullName, fileName));

            if (!srcFile.Exists)
                return new TagToolError(CommandError.FileNotFound);

            if (Cache is GameCacheHaloOnline)
            {
                var destFile = new FileInfo(Path.Combine(Cache.Directory.FullName, fileName));

                using (var stream = srcFile.Open(FileMode.Open, FileAccess.Read))
                using (var reader = new EndianReader(stream))
                using (var destStream = destFile.Create())
                using (var writer = new EndianWriter(destStream))
                {
                    Blf campaignBlf = new Blf(CacheVersion.Halo3Retail);
                    campaignBlf.Read(reader);
                    campaignBlf.ConvertBlf(Cache.Version);
                    campaignBlf.Write(writer);
                }
            }
            else if (Cache is GameCacheModPackage)
            {
                var campaignFileStream = new MemoryStream();
                using (var stream = srcFile.Open(FileMode.Open, FileAccess.Read))
                using (var reader = new EndianReader(stream))
                using (var writer = new EndianWriter(campaignFileStream, leaveOpen:true))
                {
                    Blf campaignBlf = new Blf(CacheVersion.Halo3Retail);
                    campaignBlf.Read(reader);
                    campaignBlf.ConvertBlf(Cache.Version);
                    campaignBlf.Write(writer);
                }
                var modCache = Cache as GameCacheModPackage;

                modCache.SetCampaignFile(campaignFileStream);
            }
            else
            {
                return new TagToolError(CommandError.CacheUnsupported);
            }

            
            Console.WriteLine("Done!");
            return true;
        }

    }
}