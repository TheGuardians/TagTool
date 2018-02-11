using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterSearchFlags : int
    {
        None = 0,
        CrouchOnInvestigate = 1 << 0,
        WalkOnPursuit = 1 << 1,
        SearchForever = 1 << 2,
        SearchExclusively = 1 << 3,
        SearchPointsOnlyWhileWalking = 1 << 4
    }
}
