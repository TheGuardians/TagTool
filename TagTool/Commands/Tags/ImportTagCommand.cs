using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Tags
{
    class ImportTagCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public ImportTagCommand(HaloOnlineCacheContext cacheContext)
            : base(CommandFlags.Inherit,

                  "ImportTag",
                  "",

                  "ImportTag <index> <path>",

                  "")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var instance = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);
            var path = args[1];

            if (!File.Exists(path))
                return false;
                
            if (instance == null)
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
