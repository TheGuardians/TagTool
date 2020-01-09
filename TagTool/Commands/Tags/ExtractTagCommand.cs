using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;

namespace TagTool.Commands.Tags
{
    class ExtractTagCommand : Command
    {
        private GameCacheContextHaloOnline Cache { get; }

        public ExtractTagCommand(GameCacheContextHaloOnline cache)
            : base(true,

                  "ExtractTag",
                  "Writes tag data to a file. (To be used with 'ImportTag')",

                  "ExtractTag <Tag> <Path>",

                  "Writes tag data to a file. (To be used with 'ImportTag')")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            if (!Cache.TryGetTag(args[0], out var instance))
                return false;

            var file = new FileInfo(args[1]);

            if (!file.Directory.Exists)
                file.Directory.Create();


            byte[] data;

            using (var stream = Cache.TagCache.OpenTagCacheRead())
                data = Cache.TagCacheGenHO.ExtractTagRaw(stream, (CachedTagHaloOnline)instance);

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
