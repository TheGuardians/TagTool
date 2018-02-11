using TagTool.Common;
using TagTool.IO;

using ugh_ = TagTool.Legacy.sound_cache_file_gestalt;
namespace TagTool.Legacy.Halo3Retail
{
    public class sound_cache_file_gestalt : ugh_
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
            Reader.SeekTo(Address + 60);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                PlayBacks.Add(new Playback(Cache, iOffset + 12 * i));
            #endregion

            #region SoundPermutation Chunk
            Reader.SeekTo(Address + 72);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                SoundPermutations.Add(new SoundPermutation(Cache, iOffset + 16 * i));
            #endregion

            #region Raw Chunks
            Reader.SeekTo(Address + 148);
            iCount = Reader.ReadInt32();
            iOffset = Reader.ReadInt32() - Cache.Magic;
            for (int i = 0; i < iCount; i++)
                RawChunks.Add(new RawChunk(Cache, iOffset + 20 * i));
            #endregion
        }

        new public class Codec : ugh_.Codec
        {
            public Codec(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Unknown = Reader.ReadByte();
                Type = (SoundType)Reader.ReadByte();
                Flags = new Bitmask(Reader.ReadByte());
            }
        }

        new public class SoundName : ugh_.SoundName
        {
            public SoundName(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                Name = Cache.Strings.GetItemByID(Reader.ReadInt32());
            }
        }

        new public class Playback : ugh_.Playback
        {
            public Playback(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                NameIndex = Reader.ReadUInt16();
                ParametersIndex = Reader.ReadInt16();
                Unknown = Reader.ReadInt16();
                FirstRuntimePermFlagIndex = Reader.ReadInt16();
                EncodedPermData = Reader.ReadInt16();
                FirstPermutation = Reader.ReadUInt16();
            }
        }

        new public class SoundPermutation : ugh_.SoundPermutation
        {
            public SoundPermutation(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                NameIndex = Reader.ReadInt16();
                EncodedSkipFraction = Reader.ReadInt16();
                EncodedGain = Reader.ReadByte();
                PermInfoIndex = Reader.ReadByte();
                LanguageNeutralTime = Reader.ReadInt16();
                RawChunkIndex = Reader.ReadInt32();
                ChunkCount = Reader.ReadInt16();
                EncodedPermIndex = Reader.ReadInt16();
            }
        }

        new public class RawChunk : ugh_.RawChunk
        {
            public RawChunk(Base.CacheFile Cache, int Address)
            {
                EndianReader Reader = Cache.Reader;
                Reader.SeekTo(Address);

                FileOffset = Reader.ReadInt32();
                Flags = new Bitmask(Reader.ReadInt16());
                Size = Reader.ReadUInt16();
                RuntimeIndex = Reader.ReadInt32();
                Unknown0 = Reader.ReadInt32();
                Unknown1 = Reader.ReadInt32();
            }
        }
    }
}
