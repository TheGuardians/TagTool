using TagTool.IO;

namespace TagTool.Legacy.Halo3ODST
{
    public class sound_cache_file_gestalt : Halo3Retail.sound_cache_file_gestalt
    {
        protected sound_cache_file_gestalt() { }

        public sound_cache_file_gestalt(Base.CacheFile Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            #region Codec Chunk
            int iCount = Reader.ReadInt32();
            int iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                Codecs.Add(new Codec(Cache, iOffset + 3 * i));
            #endregion

            #region SoundName Chunk
            Reader.SeekTo(Address + 36);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                SoundNames.Add(new SoundName(Cache, iOffset + 4 * i));
            #endregion

            #region Playback Chunk
            Reader.SeekTo(Address + 72);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                PlayBacks.Add(new Playback(Cache, iOffset + 12 * i));
            #endregion

            #region SoundPermutation Chunk
            Reader.SeekTo(Address + 84);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                SoundPermutations.Add(new SoundPermutation(Cache, iOffset + 16 * i));
            #endregion

            #region Raw Chunks
            Reader.SeekTo(Address + 160);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                RawChunks.Add(new RawChunk(Cache, iOffset + 20 * i));
            #endregion
        }
    }
}
