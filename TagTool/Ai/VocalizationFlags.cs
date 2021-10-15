using System;

namespace TagTool.Ai
{
    /// <summary>
    /// Bitwise flags for <see cref="VocalizationDefinition"/>.
    /// </summary>
    [Flags]
    public enum VocalizationFlags : int
    {
        None = 0,
        Immediate = 1 << 0,
        Interrupt = 1 << 1,
        CancelLowPriority = 1 << 2,
        DisableDialogueEffect = 1 << 3,
        PredictFacialAnimations = 1 << 4,
        NoDistanceCheckExceptions = 1 << 5
    }
}
