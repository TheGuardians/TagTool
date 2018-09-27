using TagTool.Tags;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x1)]
    public class Encoding : TagStructure
	{
        public EncodingValue value;

        public enum EncodingValue : sbyte
        {
            Mono,
            Stereo,
            Surround,
            _51Surround
        }

        public int GetChannelCount()
        {
            switch (value)
            {
                case EncodingValue._51Surround:
                    return 6;

                case EncodingValue.Surround:
                    return 4;

                case EncodingValue.Stereo:
                    return 2;

                case EncodingValue.Mono:
                default:
                    return 1;
            }
        }
    }
}