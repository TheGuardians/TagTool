using TagTool.Common;
using TagTool.IO;

using snd_ = TagTool.Legacy.sound;
namespace TagTool.Legacy.Halo3Retail
{
    public class sound : snd_
    {
        public sound(Base.CacheFile Cache, int Address)
        {
            EndianReader Reader = Cache.Reader;
            Reader.SeekTo(Address);

            Flags = new Bitmask(Reader.ReadInt16());
            SoundClass = Reader.ReadByte();
            SampleRate = (SampleRateValue)Reader.ReadByte();
            Encoding = Reader.ReadByte();
            CodecIndex = Reader.ReadByte();
            PlaybackIndex = Reader.ReadInt16();
            DialogueUnknown = Reader.ReadInt16();
            Unknown0 = Reader.ReadInt16();
            PitchRangeIndex1 = Reader.ReadInt16();
            PitchRangeIndex2 = Reader.ReadByte();
            ScaleIndex = Reader.ReadByte();
            PromotionIndex = Reader.ReadByte();
            CustomPlaybackIndex = Reader.ReadByte();
            ExtraInfoIndex = Reader.ReadInt16();
            Unknown1 = Reader.ReadInt32();
            RawID = Reader.ReadInt32();
            MaxPlaytime = Reader.ReadInt32();
        }
    }
}
