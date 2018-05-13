using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Serialization;
using TagTool.Commands;

namespace TagTool.Commands.Video
{
    class ExtractBinkFileCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Bink Definition { get; }

        public ExtractBinkFileCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Bink definition)
            : base(CommandFlags.None,
                  
                  "ExtractBinkFile",
                  "Extracts the .bik file from the bink tag's resource.",
                  
                  "ExtractBinkFile <Output File>",

                  "Extracts the .bik file from the bink tag's resource.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var binkFile = new FileInfo(args[0]);
            
            var resourceContext = new ResourceSerializationContext(Definition.Resource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<BinkResource>(resourceContext);

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new BinaryReader(resourceStream))
            using (var fileStream = binkFile.Create())
            using (var fileWriter = new BinaryWriter(fileStream))
            {
                CacheContext.ExtractResource(Definition.Resource, resourceStream);
                resourceReader.BaseStream.Position = resourceDefinition.Data.Address.Offset;
                fileWriter.Write(resourceReader.ReadBytes(resourceDefinition.Data.Size));
            }

            Console.WriteLine($"Created \"{binkFile.FullName}\" successfully.");

            return true;
        }
    }
}
