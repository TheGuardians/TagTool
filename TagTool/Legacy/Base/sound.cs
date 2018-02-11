using TagTool.Common;

namespace TagTool.Legacy
{
    public abstract class sound
    {
        public enum SampleRateValue : int
        {
            _22050Hz = 0,
            _44100Hz = 1
        }

        public Bitmask Flags;
        public int SoundClass;
        public SampleRateValue SampleRate;
        public int Encoding;
        public int CodecIndex;
        public int PlaybackIndex;
        public int DialogueUnknown;
        public int Unknown0;
        public int PitchRangeIndex1;
        public int PitchRangeIndex2;
        public int ScaleIndex;
        public int PromotionIndex;
        public int CustomPlaybackIndex;
        public int ExtraInfoIndex;
        public int Unknown1;
        public int RawID;
        public int MaxPlaytime;
    }
}
