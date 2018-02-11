using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterMovementFlags : int
    {
        None = 0,
        DangerCrouchAllowMovement = 1 << 0,
        NoSideStep = 1 << 1,
        PreferToCombatNearFriends = 1 << 2,
        AllowBoostedJump = 1 << 3,
        Perch = 1 << 4,
        Climb = 1 << 5,
        PreferWallMovement = 1 << 6,
        HasFlyingMode = 1 << 7,
        DisallowCrouch = 1 << 8,
        DisallowAllMovement = 1 << 9,
        AlwaysUseSearchPoints = 1 << 10,
        KeepMoving = 1 << 11,
        CureIsolationJump = 1 << 12,
        GainElevation = 1 << 13,
        RepositionDistant = 1 << 14,
        OnlyUseAerialFiringPositions = 1 << 15,
        UseHighPriorityPathFinding = 1 << 16,
        LowerWeaponWhenNoAlertMovementOverride = 1 << 17,
        Phase = 1 << 18,
        NoOverrideWhenFiring = 1 << 19,
        NoStowDuringIdleActivities = 1 << 20,
        FlipAnyVehicle = 1 << 21,
        BoundAlongPath = 1 << 22
    }
}
