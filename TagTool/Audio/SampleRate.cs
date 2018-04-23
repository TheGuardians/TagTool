using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class SampleRate
    {
        public SampleRateValue value;

        public enum SampleRateValue : sbyte
        {
            _48khz,
            _44khz,
            _32khz
        }

        // File converted using either 48000 or 24000 are broken, using 44100 seems to work for now.

        public int GetSampleRateHz()
        {
            switch (value)
            {
                case SampleRateValue._48khz:
                    return 44100;

                case SampleRateValue._32khz:
                    return 44100;

                case SampleRateValue._44khz:
                default:
                    return 44100;
            }
        }

        public float GetSampleRatekHz()
        {
            switch (value)
            {
                case SampleRateValue._48khz:
                    return 44.100f;

                case SampleRateValue._32khz:
                    return 44.100f;

                case SampleRateValue._44khz:
                default:
                    return 44.100f;
            }
        }
    }
}