using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterRetreatFlags : int
    {
        None = 0,
        ZigZagWhenFleeing = 1 << 0
    }
}
