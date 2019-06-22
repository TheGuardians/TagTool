using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Serialization;

namespace TagTool.Cache
{
    /// <summary>
    /// Frontend for game cache context, determines the right format for the given map file and opens the right generation of game cache context. Implements the IGameCacheContext
    /// for ease of modification.
    /// </summary>
    public class GameCache
    {
        public FileInfo File;
        public Stream Stream;
        public EndianReader Reader;

        public IGameCacheContext CacheContext;

        public GameCache(FileInfo file)
        {
            File = file;
            Stream = file.OpenRead();
            Reader = new EndianReader(Stream);

            var mapFile = new MapFile(Reader);

            switch (mapFile.Version)
            {
                case CacheVersion.HaloPC:
                case CacheVersion.HaloXbox:
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                    throw new Exception("Not implemented!");

                case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3ODST:
                case CacheVersion.Halo3Retail:
                case CacheVersion.HaloReach:
                    CacheContext = new GameCacheContextGen3(mapFile, Reader);
                    break;

                case CacheVersion.HaloOnline106708:
                case CacheVersion.HaloOnline235640:
                case CacheVersion.HaloOnline301003:
                case CacheVersion.HaloOnline327043:
                case CacheVersion.HaloOnline372731:
                case CacheVersion.HaloOnline416097:
                case CacheVersion.HaloOnline430475:
                case CacheVersion.HaloOnline454665:
                case CacheVersion.HaloOnline449175:
                case CacheVersion.HaloOnline498295:
                case CacheVersion.HaloOnline530605:
                case CacheVersion.HaloOnline532911:
                case CacheVersion.HaloOnline554482:
                case CacheVersion.HaloOnline571627:
                case CacheVersion.HaloOnline700123:
                    break;
            }
        }

        ~GameCache()
        {
            Reader.Close();
            Reader.Dispose();
            Stream.Close();
            Stream.Dispose();
        }

        public ISerializationContext CreateSerializationContext(object tag) => CacheContext.CreateSerializationContext(tag);
    }
}
