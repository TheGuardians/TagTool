using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;

namespace TagTool.Cache
{
    /// <summary>
    /// Frontend for game cache context, determines the right format for the given map file and opens the right generation of game cache context. Implements the IGameCacheContext
    /// for ease of modification.
    /// </summary>
    public class GameCacheContextNew : IGameCacheContext
    {
        public FileInfo File;
        public Stream Stream;
        public EndianReader Reader;

        public IGameCacheContext CacheContext;

        public GameCacheContextNew(FileInfo file)
        {
            File = file;
            Stream = file.OpenRead();
            Reader = new EndianReader(Stream);

            var mapFile = new MapFile(Reader);

            switch (mapFile.Version)
            {
                /*
                case CacheVersion.HaloPC:
                case CacheVersion.HaloXbox:
                */
                case CacheVersion.Halo2Vista:
                case CacheVersion.Halo2Xbox:
                case CacheVersion.HaloOnline106708:
                    throw new Exception("Not implemented!");

                //case CacheVersion.Halo3Beta:
                case CacheVersion.Halo3ODST:
                case CacheVersion.Halo3Retail:
                case CacheVersion.HaloReach:
                    CacheContext = new GameCacheContextGen3(mapFile, Reader);
                    break;
            }
        }

        ~GameCacheContextNew()
        {
            Reader.Close();
            Reader.Dispose();
            Stream.Close();
            Stream.Dispose();
        }


    }
}
