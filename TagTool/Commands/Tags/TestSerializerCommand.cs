using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Commands.Common;
using System.IO;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Tags
{
    public class TestSerializerCommand : Command
    {
        private GameCacheHaloOnlineBase Cache { get; }
        public TestSerializerCommand(GameCacheHaloOnlineBase cachecontext)
            : base(false,

                  "TestSerializer",
                  "Tests whether the serializer serializes tags that can match those that exist in the cache",

                  "",

                  "")
        {
            Cache = cachecontext;
        }

        public override object Execute(List<string> args)
        {
            foreach (var tag in Cache.TagCache.TagTable)
            {
                CachedTagHaloOnline CacheTag = (CachedTagHaloOnline)Cache.TagCacheGenHO.GetTag(tag.Index);

                int headersize = (int)CacheTag.CalculateHeaderSize();

                byte[] tagcachedata;
                object cachedef;
                using (var stream = Cache.OpenCacheRead())
                using (var outstream = new MemoryStream())
                using (EndianWriter writer = new EndianWriter(outstream, EndianFormat.LittleEndian))
                {
                    //deserialize the cache def then reserialize to a stream
                    cachedef = Cache.Deserialize(stream, CacheTag);
                    var dataContext = new DataSerializationContext(writer);
                    Cache.Serializer.Serialize(dataContext, cachedef);
                    StreamUtil.Align(outstream, 0x10);
                    tagcachedata = outstream.ToArray();
                }

                int diff = (int)CacheTag.TotalSize - (int)CacheTag.CalculateHeaderSize() - (int)tagcachedata.Length;
                if (diff != 0)
                {
                    Console.WriteLine($"{tag.Name}.{tag.Group} failed! (difference {diff})");
                    /*
                    using (var stream = Cache.OpenCacheReadWrite())
                    {
                        var dataContext = new HaloOnlineSerializationContext(stream, Cache, CacheTag);
                        Cache.Serializer.Serialize(dataContext, cachedef);
                    }
                    */
                }
                    
            }
            return true;
        }
    }
}
