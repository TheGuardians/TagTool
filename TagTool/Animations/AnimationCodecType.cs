namespace TagTool.Animations
{
    public enum AnimationCodecType : sbyte
    {
        NoCompression,
        UncompressedStatic,
        UncompressedAnimated,
        _8ByteQuantizedRotationOnly,
        ByteKeyframeLightlyQuantized,
        WordKeyframeLightlyQuantized,
        ReverseByteKeyframeLightlyQuantized,
        ReverseWordKeyframeLightlyQuantized,
        BlendScreen,
        Curve,
        RevisedCurve,
        SharedStatic
    }
}