using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterPreSearchFlags : int
    {
        None = 0,
        Flag1 = 1 << 0
    }
}
