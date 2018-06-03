using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ExtractTagCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ExtractTagCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "ExtractTag",
                  "",

                  "ExtractTag <Tag> <Path>",

                  "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            if (!CacheContext.TryGetTag(args[0], out var instance))
                return false;

            var file = new FileInfo(args[1]);

            if (!file.Directory.Exists)
                file.Directory.Create();

            byte[] data;

            using (var stream = CacheContext.OpenTagCacheRead())
                data = CacheContext.TagCache.ExtractTagRaw(stream, instance);

            using (var outStream = file.Create())
            {
                outStream.Write(data, 0, data.Length);
                Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, file);
                Console.WriteLine("The tag's definition will be at offset 0x{0:X}.", instance.DefinitionOffset);
            }

            return true;
        }
    }
}
