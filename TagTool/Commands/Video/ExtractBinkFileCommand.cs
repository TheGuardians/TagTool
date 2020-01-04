using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Video
{
    class ExtractBinkFileCommand : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private Bink Definition { get; }

        public ExtractBinkFileCommand(GameCache cache, CachedTag tag, Bink definition)
            : base(false,
                  
                  "ExtractBinkFile",
                  "Extracts the .bik file from the bink tag's resource.",
                  
                  "ExtractBinkFile <Output File>",

                  "Extracts the .bik file from the bink tag's resource.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var binkFile = new FileInfo(args[0]);
            
            using (var fileStream = binkFile.Create())
            using (var fileWriter = new BinaryWriter(fileStream))
            {
                var binkResourceDefinition = Cache.ResourceCache.GetBinkResource(Definition.ResourceReference);
                fileWriter.Write(binkResourceDefinition.Data.Data);
            }

            Console.WriteLine($"Created \"{binkFile.FullName}\" successfully.");

            return true;
        }
    }
}
