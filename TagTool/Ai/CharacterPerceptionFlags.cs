using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterPerceptionFlags : ushort
    {
        None = 0,
        CharacterCanSeeInDarkness = 1 << 0,
        IgnoreTrackingProjectiles = 1 << 1,
        IgnoreMinorTrackingProjectiles = 1 << 2
    }
}
