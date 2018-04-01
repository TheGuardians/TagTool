using TagTool.Cache;
using System.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Legacy.Halo3ODST
{
    public class CacheFile : Halo3Retail.CacheFile
    {
        public CacheFile(FileInfo file, GameCacheContext cacheContext, CacheVersion version = CacheVersion.Halo3ODST)
            : base(file, cacheContext, version)
        {
            Resolver = new StringIdResolverHalo3ODST();
        }

        public override void LoadResourceTags()
        {
            TagDeserializer deserializer = new TagDeserializer(CacheVersion.Halo3ODST);

            foreach (IndexItem item in IndexItems)
            {
                if (item.ClassCode == "play")
                {
                    var blamContext = new CacheSerializationContext(CacheContext, this, item);
                    ResourceLayoutTable = deserializer.Deserialize<CacheFileResourceLayoutTable>(blamContext);
                    break;
                }
            }

            foreach (IndexItem item in IndexItems)
            {
                if (item.ClassCode == "zone")
                {
                    var blamContext = new CacheSerializationContext(CacheContext, this, item);
                    ResourceGestalt = deserializer.Deserialize<CacheFileResourceGestalt>(blamContext);

                    foreach (var tagresource in ResourceGestalt.TagResources)
                    {
                        foreach (var fixup in tagresource.ResourceFixups)
                        {
                            fixup.Offset = (fixup.Address & 0x0FFFFFFF);
                            fixup.Type = (fixup.Address >> 28) & 0xF;
                            fixup.RawAddress = fixup.Address;
                        }
                    }

                    break;
                }
            }
        }
    }
}
