using TagTool.Cache;
using System;
using System.IO;

namespace TagTool.Legacy.Halo2Vista
{
    public class CacheFile : Halo2Xbox.CacheFile
    {
        public CacheFile(FileInfo file, CacheVersion version = CacheVersion.Halo2Vista) :
            base(file, version)
        {
        }

        public override byte[] GetRawFromID(int ID, int DataLength)
        {
            throw new NotImplementedException();
        }

        public override void LoadResourceTags()
        {
            throw new NotImplementedException();
        }
    }
}
