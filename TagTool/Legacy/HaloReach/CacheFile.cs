using TagTool.Cache;
using System.IO;

namespace TagTool.Legacy.HaloReach
{
    public class CacheFile : Halo3Retail.CacheFile
    {
        public CacheFile(FileInfo file, CacheVersion version = CacheVersion.HaloReach) :
            base(file, version)
        {
        }
    }
}
