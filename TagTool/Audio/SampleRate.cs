using System;
using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class SampleRate : TagStructure
	{
        public SampleRateValue value;

        public enum SampleRateValue : sbyte
        {
            _22khz,
            _44khz,
            _32khz,
            _48khz
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
                    return 44100;
                case SampleRateValue._48khz:
                    return 48000;
                default:
                    throw new NotSupportedException();
                    
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
                    return 44.100f;
                case SampleRateValue._48khz:
                    return 48.000f;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}