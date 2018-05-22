using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands;

namespace TagTool.Commands.Tags
{
    class DeleteTagCommand : Command
    {
        public HaloOnlineCacheContext CacheContext { get; }

        private HashSet<int> AudioResourceIndices { get; } = new HashSet<int>();
        private HashSet<int> TexturesResourceIndices { get; } = new HashSet<int>();
        private HashSet<int> TexturesBResourceIndices { get; } = new HashSet<int>();
        private HashSet<int> ResourcesBResourceIndices { get; } = new HashSet<int>();

        private HashSet<int> ResourcesResourceIndices { get; } = new HashSet<int>
        {
            0x169, 0x16A, 0x16B, 0x170, 0x171,
            0x2AF, 0x2B0, 0x38E, 0x540, 0x541,
            0x542, 0x543, 0x544, 0x546, 0x549,
            0x54D, 0x54F, 0x550, 0x55B, 0x56A,
            0x580, 0x587, 0x6B9, 0x6ED, 0x6EE
        };

        public DeleteTagCommand(HaloOnlineCacheContext cacheContext)
            : base(false,

                  "DeleteTag",
                  "Nulls and removes a tag from cache.",

                  "DeleteTag <tag>",

                  "Nulls and removes a tag from cache.")
        {
            CacheContext = cacheContext;
        }
        
        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var tag = ArgumentParser.ParseTagSpecifier(CacheContext, args[0]);

            if (tag == null)
                return false;

            using (var stream = CacheContext.OpenTagCacheReadWrite())
            {
                var tagName = CacheContext.TagNames.ContainsKey(tag.Index) ?
                    CacheContext.TagNames[tag.Index] :
                    "<unnamed>";

                Console.Write($"Nulling {tagName}.{CacheContext.GetString(tag.Group.Name)}...");
                CacheContext.TagCache.Index[tag.Index] = null;
                CacheContext.TagCache.SetTagDataRaw(stream, tag, new byte[] { });
                Console.WriteLine("done.");
            }

            return true;
        }
    }
}