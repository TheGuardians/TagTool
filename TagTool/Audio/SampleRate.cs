using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class SampleRate
    {
        public SampleRateValue value;

        public enum SampleRateValue : sbyte
        {
            _22khz,
            _44khz,
            _32khz
        }

        public int GetSampleRateHz()
        {
            switch (value)
            {
                case SampleRateValue._22khz:
                    return 22050;

                case SampleRateValue._32khz:
                    return 32000;

                case SampleRateValue._44khz:
                default:
                    return 44100;
            }
        }

        public float GetSampleRatekHz()
        {
            switch (value)
            {
                case SampleRateValue._22khz:
                    return 22.050f;

                case SampleRateValue._32khz:
                    return 32.000f;

                case SampleRateValue._44khz:
                default:
                    return 44.100f;
            }
        }
    }
}