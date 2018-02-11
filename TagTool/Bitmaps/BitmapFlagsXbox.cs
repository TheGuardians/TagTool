using System;

namespace TagTool.Bitmaps
{
    [Flags]
    public enum BitmapFlagsXbox : byte
    {
        None = 0,
        MediumResolutionOffsetIsValid = 1 << 0,
        Xbox360MemorySpacing = 1 << 1,
        Xbox360ByteOrder = 1 << 2,
        TiledTexture = 1 << 3,
        CreatedCorrectly = 1 << 4,
        HighResolutionOffsetIsValid = 1 << 5,
        UseInterleavedTextures = 1 << 6,
        UseOnDemandOnly = 1 << 7
    }
}
