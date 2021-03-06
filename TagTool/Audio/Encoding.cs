using TagTool.Tags;

namespace TagTool.Audio
{
    public static class Encoding
	{
        public static int GetChannelCount(EncodingValue value)
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

        public static int GetChannelCount(Gen2Encoding value)
        {
            switch (value)
            {
                case Gen2Encoding.Codec:
                    return 2;

                case Gen2Encoding.Stereo:
                    return 2;

                case Gen2Encoding.Mono:
                default:
                    return 1;
            }
        }
    }

    public enum EncodingValue : sbyte
    {
        Mono,
        Stereo,
        Surround,
        _51Surround
    }

    public enum Gen2Encoding : sbyte
    {
        Mono,
        Stereo,
        Codec
    }
}