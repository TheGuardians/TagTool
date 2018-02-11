using System;

namespace TagTool.Ai
{
    /// <summary>
    /// Bitwise flags for <see cref="AiVocalizationResponse"/>.
    /// </summary>
    [Flags]
    public enum AiVocalizationResponseFlags : ushort
    {
        None = 0,
        Nonexclusive = 1 << 0,
        TriggerResponse = 1 << 1
    }
}
