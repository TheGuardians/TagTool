namespace TagTool.Animations
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