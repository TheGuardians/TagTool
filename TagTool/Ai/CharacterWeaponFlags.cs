using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterWeaponFlags : int
    {
        None = 0,
        BurstingInhibitsMovement = 1 << 0,
        MustCrouchToShoot = 1 << 1,
        UseExtendedSafeToSaveRange = 1 << 2,
        FixedAimingVector = 1 << 3,
        AimAtFeet = 1 << 4,
        /// <summary>
        /// Use only for weapons with really, really long barrels (bfg),
        /// do NOT use for rotating turret weapons (warthog, falcon, etc)
        /// </summary>
        ForceAimFromBarrelPosition = 1 << 5,
        FavorForLongRange = 1 << 6,
        FavorForCloseRange = 1 << 7,
        FavorAgainstVehicles = 1 << 8,
        FavoredSpecialWeapon = 1 << 9,
        BurstingInhibitsEvasion = 1 << 10
    }
}
