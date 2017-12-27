using System;
using System.Collections.Generic;
using System.IO;
using BlamCore.Cache;
using BlamCore.TagDefinitions;
using BlamCore.Serialization;
using BlamCore.Commands;

namespace TagTool.Files
{
    class ReplaceFileCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private VFilesList Definition { get; }

        public ReplaceFileCommand(GameCacheContext cacheContext, CachedTagInstance tag, VFilesList definition)
            : base(CommandFlags.None,

                  "ReplaceFile",
                  "Replace a file stored in the tag",

                  "ReplaceFile <virtual path> [filename]",

                  "Replaces a file stored in the tag. The tag will be resized as necessary.")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1 && args.Count != 2)
                return false;

            var virtualPath = args[0];
            var inputPath = (args.Count == 2) ? args[1] : virtualPath;
            var file = Definition.Find(virtualPath);

            if (file == null)
            {
                Console.WriteLine("Unable to find file {0}.", virtualPath);
                return true;
            }

            byte[] data;

            try
            {
                data = File.ReadAllBytes(inputPath);
            }
            catch (IOException)
            {
                Console.WriteLine("Unable to read from {0}.", inputPath);
                return true;
            }

            Definition.Replace(file, data);

            using (var stream = CacheContext.OpenTagCacheReadWrite())
                CacheContext.Serializer.Serialize(new TagSerializationContext(stream, CacheContext, Tag), Definition);

            Console.WriteLine("Imported 0x{0:X} bytes.", data.Length);

            return true;
        }
    }
}
