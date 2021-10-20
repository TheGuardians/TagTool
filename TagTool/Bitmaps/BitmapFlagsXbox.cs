using System;

namespace TagTool.Bitmaps
{
    [Flags]
    public enum BitmapFlagsXbox : byte
    {
        None = 0,
        MediumResolutionOffsetIsValid = 1 << 0, // DeleteFromCacheFile ?
        Xbox360PitchMemorySpacing = 1 << 1,
        Xbox360ByteOrder = 1 << 2,
        Xbox360TiledTexture = 1 << 3,
        Xbox360CreatedCorrectlyHackForBumps = 1 << 4,
        Xbox360HighResolutionOffsetIsValid = 1 << 5,
        Xbox360UseInterleavedTextures = 1 << 6,
        UseOnDemandOnly = 1 << 7
    }
}
