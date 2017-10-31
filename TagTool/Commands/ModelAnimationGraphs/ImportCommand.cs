using System;
using System.Collections.Generic;
using BlamCore.Cache;
using BlamCore.TagDefinitions;
using System.IO;
using BlamCore.Common;

namespace TagTool.Commands.ModelAnimationGraphs
{
    class ImportCommand : Command
    {
        private GameCacheContext CacheContext { get; }
        private CachedTagInstance Tag { get; }
        private ModelAnimationGraph Definition { get; }

        public ImportCommand(GameCacheContext cacheContext, CachedTagInstance tag, ModelAnimationGraph definition)
            : base(CommandFlags.None,

                  "Import",
                  "Import <index> <filename>",

                  "Import",

                  "Import <index> <filename>")
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
                var data = File.ReadAllBytes(filePath);
                using (var stream = File.OpenRead(CacheContext.TagCacheFile.DirectoryName + "\\" + "resources.dat"))
                {
                    var cache = new ResourceCache(stream);

                    Definition.ResourceGroups[groupIndex].Resource.Page.Index = cache.Add(stream, data, out compressedSize);
                    Definition.ResourceGroups[groupIndex].Resource.Page.CompressedBlockSize = compressedSize;
                    Definition.ResourceGroups[groupIndex].Resource.Page.OldFlags = (OldRawPageFlags)2;

                    Console.WriteLine("Imported 0x{0:X} bytes.", data.Length);
                    Console.WriteLine("Compressed size = 0x{0:X}", compressedSize);
                    Console.WriteLine("New resource index: {0:X8}", Definition.ResourceGroups[groupIndex].Resource.Page.Index);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to import resource: {0}", ex.Message);
            }

            return true;
        }
    }
}
