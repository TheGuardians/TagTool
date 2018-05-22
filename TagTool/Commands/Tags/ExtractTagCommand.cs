using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands;

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

            var instance = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (instance == null)
                return false;

            var path = args[1];

            byte[] data;

            using (var stream = CacheContext.OpenTagCacheRead())
                data = CacheContext.TagCache.ExtractTagRaw(stream, instance);

            using (var outStream = File.Open(path, FileMode.Create, FileAccess.Write))
            {
                outStream.Write(data, 0, data.Length);
                Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, path);
                Console.WriteLine("The tag's definition will be at offset 0x{0:X}.", instance.DefinitionOffset);
            }

            return true;
        }
    }
}
