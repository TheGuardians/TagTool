namespace TagTool.Animations.Codecs
{
    public enum AnimationCodecType : sbyte
    {
        NoCompression,
        UncompressedStatic,
        UncompressedAnimated,
        QuantizedRotationOnly,
        ByteKeyframeLightlyQuantized,
        WordKeyframeLightlyQuantized,
        ReverseByteKeyframeLightlyQuantized,
        ReverseWordKeyframeLightlyQuantized,
        BlendScreen,
    }
}