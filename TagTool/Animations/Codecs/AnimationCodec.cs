namespace TagTool.Animations.Codecs
{
    public enum AnimationCodec : sbyte
    {
        NoCompression,
        UncompressedStatic,
        UncompressedAnimated,
        QuantizedRotationOnly,
        ByteKeyframeLightlyQuantized,
        WordKeyframeLightlyQuantized,
        ReverseByteKeyframeLightlyQuantized,
        ReverseWordKeyframeLightlyQuantized,
        BlendScreen
    }
}