using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.HaloOnline;
using TagTool.Cache.Monolithic;
using System.Linq;

namespace TagTool.Commands.Tags
{
    class ExtractTagCommand : Command
    {
        private GameCache Cache { get; }

        public ExtractTagCommand(GameCache cache)
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
                return new TagToolError(CommandError.ArgCount);
            if (!Cache.TagCache.TryGetTag(args[0], out var instance))
                return new TagToolError(CommandError.TagInvalid);

            var path = args[1];

            if (Path.GetExtension(path) == "")
                path = Path.Combine(path, instance.Name.Split('\\').Last() + "." + instance.Group);

            var file = new FileInfo(path);
            if (!file.Directory.Exists)
                file.Directory.Create();

            
            if(Cache is GameCacheHaloOnlineBase hoCache)
            {
                byte[] data;
                using (var stream = Cache.OpenCacheRead())
                    data = hoCache.TagCacheGenHO.ExtractTagRaw(stream, (CachedTagHaloOnline)instance);

                using (var outStream = file.Create())
                {
                    outStream.Write(data, 0, data.Length);
                    Console.WriteLine("Wrote 0x{0:X} bytes to \"{1}\".", outStream.Position, file);
                    Console.WriteLine("The tag's definition will be at offset 0x{0:X}.", instance.DefinitionOffset);
                }
            }
            else if (Cache is GameCacheMonolithic monolithicCache)
            {
                byte[] data;
                using (var stream = Cache.OpenCacheRead())
                    data = monolithicCache.Backend.ExtractTagRaw(instance.Index);

                using (var outStream = file.Create())
                {
                    outStream.Write(data, 0, data.Length);
                    Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, file);
                }
            }

            return true;
        }
    }
}
