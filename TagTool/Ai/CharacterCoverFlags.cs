using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterCoverFlags : int
    {
        None = 0,
        UsePhasing = 1 << 1
    }
}
