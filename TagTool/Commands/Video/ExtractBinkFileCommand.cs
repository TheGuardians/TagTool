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
        private HaloOnlineCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private Bink Definition { get; }

        public ExtractBinkFileCommand(HaloOnlineCacheContext cacheContext, CachedTagInstance tag, Bink definition)
            : base(false,
                  
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
            
            var resourceContext = new ResourceSerializationContext(CacheContext, Definition.ResourceReference.HaloOnlinePageableResource);
            var resourceDefinition = CacheContext.Deserializer.Deserialize<BinkResource>(resourceContext);

            using (var resourceStream = new MemoryStream())
            using (var resourceReader = new BinaryReader(resourceStream))
            using (var fileStream = binkFile.Create())
            using (var fileWriter = new BinaryWriter(fileStream))
            {
                CacheContext.ExtractResource(Definition.ResourceReference.HaloOnlinePageableResource, resourceStream);
                resourceReader.BaseStream.Position = resourceDefinition.Data.Address.Offset;
                fileWriter.Write(resourceReader.ReadBytes(resourceDefinition.Data.Size));
            }

            Console.WriteLine($"Created \"{binkFile.FullName}\" successfully.");

            return true;
        }
    }
}
