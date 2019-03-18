using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ImportTagCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ImportTagCommand(HaloOnlineCacheContext cacheContext)
            : base(true,

                  "ImportTag",
                  "Overwrites a tag with data from a file.",

                  "ImportTag <Tag> <Path>",

                  "Overwrites a tag with data from a file.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            if (!CacheContext.TryGetTag(args[0], out var instance))
                return false;

            var path = args[1];

            if (!File.Exists(path))
                return false;

            byte[] data;

            using (var inStream = File.OpenRead(path))
            {
                data = new byte[inStream.Length];
                inStream.Read(data, 0, data.Length);
            }

            using (var stream = CacheContext.OpenTagCacheReadWrite())
                CacheContext.TagCache.SetTagDataRaw(stream, instance, data);

            Console.WriteLine($"Imported 0x{data.Length:X} bytes.");

            return true;
        }
    }
}