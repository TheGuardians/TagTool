using TagTool.IO;

using play = TagTool.Legacy.cache_file_resource_layout_table;
namespace TagTool.Legacy.Halo3Retail
{
    public class cache_file_resource_layout_table : play
    {
        protected cache_file_resource_layout_table() { }

        public cache_file_resource_layout_table(Base.CacheFile Cache, int Offset)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Offset);

            #region Shared Cache Block
            Reader.SeekTo(Offset + 12);
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                SharedCaches.Add(new SharedCache(Cache, iOffset + 264 * i));
            #endregion

            #region Page Block
            Reader.SeekTo(Offset + 24);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                Pages.Add(new Page(Cache, iOffset + 88 * i));
            #endregion

            #region SoundRawChunk Block
            Reader.SeekTo(Offset + 36);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                SoundRawChunks.Add(new SoundRawChunk(Cache, iOffset + 16 * i));
            #endregion

            #region RawLocation Block
            Reader.SeekTo(Offset + 48);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                Segments.Add(new Segment(Cache, iOffset + 16 * i));
            #endregion
        }

        new public class SharedCache : play.SharedCache
        {
            public SharedCache(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                FileName = Reader.ReadNullTerminatedString(32);
            }
        }

        new public class Page : play.Page
        {
            public Page(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Reader.ReadInt32();
                CacheIndex = Reader.ReadInt16();
                Reader.ReadInt16();
                RawOffset = Reader.ReadInt32();
                CompressedSize = Reader.ReadInt32();
                DecompressedSize = Reader.ReadInt32();

                Reader.SeekTo(Address + 84);

                RawChunkCount = Reader.ReadInt16();
                Reader.ReadInt16();
            }
        }

        new public class SoundRawChunk : play.SoundRawChunk
        {
            public SoundRawChunk(Base.CacheFile Cache, int Offset)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Offset);

                RawSize = Reader.ReadInt32();

                #region Size Chunk
                int iCount = Reader.ReadInt32();
                int iOffset = Reader.ReadInt32() - Cache.Magic;
                for (int i = 0; i < iCount; i++)
                    Sizes.Add(new Size(Cache, iOffset + 16 * i));
                Reader.SeekTo(Offset + 16);
                #endregion
            }

            new public class Size : play.SoundRawChunk.Size
            {
                public Size(Base.CacheFile Cache, int Offset)
                {
                    EndianReader Reader = Cache.Reader;
                    Reader.SeekTo(Offset);

                    Reader.ReadInt32();
                    PermutationSize = Reader.ReadInt32();

                    Reader.SeekTo(Offset + 16);
                }
            }
        }

        new public class Segment : play.Segment
        {
            public Segment(Base.CacheFile Cache, int Offset)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Offset);

                RequiredPageIndex = Reader.ReadInt16();
                OptionalPageIndex = Reader.ReadInt16();
                OptionalPageIndex2 = -1; //doesn't exist till Halo4
                RequiredPageOffset = Reader.ReadInt32();
                OptionalPageOffset = Reader.ReadInt32();
                OptionalPageOffset2 = -1; //doesn't exist till Halo4
                SoundNumber = Reader.ReadInt16();
                SoundRawIndex = Reader.ReadInt16();
            }
        }
    }
}
