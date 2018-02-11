using TagTool.Cache;
using System.IO;

namespace TagTool.Legacy.Halo3ODST
{
    public class CacheFile : Halo3Retail.CacheFile
    {
        public CacheFile(FileInfo file, CacheVersion version = CacheVersion.Halo3ODST)
            : base(file, version)
        {
            Resolver = new StringIdResolverHalo3ODST();
        }

        public override void LoadResourceTags()
        {
            foreach (IndexItem item in IndexItems)
            {
                if (item.ClassCode == "play")
                {
                    if (item.Offset > Reader.Length)
                    {
                        foreach (IndexItem item2 in IndexItems)
                        {
                            if (item2.ClassCode == "zone")
                            {
                                //fix for H4 prologue, play address is out of 
                                //bounds and data is held inside the zone tag 
                                //instead so make a fake play tag using zone data
                                item.Offset = item2.Offset + 28;
                                break;
                            }
                        }
                    }

                    ResourceLayoutTable = new Halo3Retail.cache_file_resource_layout_table(this, item.Offset);
                    break;
                }
            }

            foreach (IndexItem item in IndexItems)
            {
                if (item.ClassCode == "zone")
                {
                    ResourceGestalt = new Halo3Retail.cache_file_resource_gestalt(this, item.Offset);
                    break;
                }
            }
        }
    }
}
