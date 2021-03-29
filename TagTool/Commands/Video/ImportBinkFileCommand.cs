using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Tags.Definitions;
using TagTool.Video;

namespace TagTool.Commands.Video
{
    class ImportBinkFileCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bink Definition { get; }

        public ImportBinkFileCommand(GameCache cache, CachedTag tag, Bink definition)
            : base(false,

                  "ImportBinkFile",
                  "Imports a .bik file to the bink tag's resource.",

                  "ImportBinkFile <Input File>",

                  "Imports a .bik file to the bink tag's resource.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return new TagToolError(CommandError.ArgCount);

            byte[] binkData = File.ReadAllBytes(args[0]);
            var binkResource = VideoUtils.CreateBinkResourceDefinition(binkData);
            var resourceReference = Cache.ResourceCache.CreateBinkResource(binkResource);
            Definition.ResourceReference = resourceReference;

            Definition.FrameCount = BitConverter.ToInt32(binkData, 8);

            Console.WriteLine($"Imported bink successfully.");

            return true;
        }
    }
}
