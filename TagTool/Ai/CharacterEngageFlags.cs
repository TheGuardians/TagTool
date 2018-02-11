using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterEngageFlags : int
    {
        None = 0,
        DefendThreatAxis = 1 << 0,
        FightConstantMovement = 1 << 1,
        FlightFightConstantMovement = 1 << 2,
        DisallowCombatCrouching = 1 << 3,
        DisallowCrouchShooting = 1 << 4,
        FightStable = 1 << 5,
        ThrowShouldLob = 1 << 6,
        AllowPositioningBeyondIdealRange = 1 << 7,
        CanSuppress = 1 << 8,
        PrefersBunker = 1 << 9,
        BurstInhibitsEvasion = 1 << 10
    }
}
