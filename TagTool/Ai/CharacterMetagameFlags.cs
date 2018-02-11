using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterMetagameFlags : byte
    {
        None,
        MustHaveActiveSeats = 1 << 0
    }
}