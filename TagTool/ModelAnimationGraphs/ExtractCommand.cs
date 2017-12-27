using System;
using System.Collections.Generic;
using BlamCore.Cache;
using BlamCore.TagDefinitions;
using System.IO;
using BlamCore.Commands;

namespace TagTool.ModelAnimationGraphs
{
    class ExtractCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private ModelAnimationGraph Definition { get; }

        public ExtractCommand(GameCacheContext cacheContext, CachedTagInstance tag, ModelAnimationGraph definition)
            : base(CommandFlags.None,

                  "Extract",
                  "Extract <index> <filename>",

                  "Extract",

                  "Extract <index> <filename>")
        {
            CacheContext = cacheContext;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            int groupIndex = Convert.ToInt32(args[0]); // resource index
            var filePath = args[1]; // filename

            int resourceIndex = Definition.ResourceGroups[groupIndex].Resource.Page.Index;
            uint compressedSize = Definition.ResourceGroups[groupIndex].Resource.Page.CompressedBlockSize;

            if (groupIndex < 0)
            {
                Console.WriteLine("Current resource index = {0:X4}.", resourceIndex);
                return false;
            }
            try
            {
                using (var stream = File.OpenRead(CacheContext.TagCacheFile.DirectoryName + "\\" + "resources.dat"))
                {
                    var cache = new ResourceCache(stream);
                    using (var outStream = File.Open(filePath, FileMode.Create, FileAccess.Write))
                    {
                        cache.Decompress(stream, (int)resourceIndex, compressedSize, outStream);
                        Console.WriteLine("Wrote 0x{0:X} bytes to {1}.", outStream.Position, filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to extract resource: {0}", ex.Message);
            }

            return true;
        }
    }
}