using System;

namespace TagTool.Tags.Definitions.Common
{
    public enum MultiplayerTeamDesignator : sbyte
    {
        Red,
        Blue,
        Green,
        Orange,
        Purple,
        Yellow,
        Brown,
        Pink,
        Neutral,
    }

    public enum MultiplayerObjectBoundaryShape : sbyte
    {
        None,
        Sphere,
        Cylinder,
        Box,

        Count
    }

    [Flags]
    public enum MultiplayerObjectPlacementSpawnFlags : byte
    {
        UniqueSpawn = 1 << 0,
        NotInitiallyPlaced = 1 << 1,
        // reach+
        HideUnlessRequired = 1 << 2,
        IsShortcutObject = 1 << 3,
    }

    public enum MultiplayerObjectRemappingPolicy : sbyte
    {
        NormalDefault,
        DoNotReplace,
        OnlyReplace
    }

    public enum MultiplayerObjectType : sbyte
    {
        Ordinary,
        Weapon,
        Grenade,
        Projectile,
        Powerup,
        Equipment,
        LightLandVehicle,
        HeavyLandVehicle,
        FlyingVehicle,
        Teleporter2way,
        TeleporterSender,
        TeleporterReceiver,
        PlayerSpawnLocation,
        PlayerRespawnZone,
        HoldSpawnLocation, // OddballSpawnLocation (only? or assault also)
        CtfFlagSpawnLocation,
        AssaultTargetLocation, // formerly TargetSpawnLocation. assumed only for Assault
        CtfFlagReturnArea,
        KothHillArea,
        InfectionSafeArea,
        TerritoryArea,
        VipInfluenceArea,
        VipDestinationZone,
        JuggernautDestinationZone
    };

    public enum MultiplayerObjectTypeReach : sbyte
    {
        Ordinary,
        Weapon,
        Grenade,
        Projectile,
        Powerup,
        Equipment,
        AmmoPack,
        LightLandVehicle,
        HeavyLandVehicle,
        FlyingVehicle,
        Turret,
        Device,
        Teleporter2way,
        TeleporterSender,
        TeleporterReceiver,
        PlayerSpawnLocation,
        PlayerRespawnZone,
        SecondaryObjective,
        PrimaryObjective,
        NamedLocationArea,
        DangerZone,
        Fireteam1RespawnZone,
        Fireteam2RespawnZone,
        Fireteam3RespawnZone,
        Fireteam4RespawnZone,
        SafeVolume,
        KillVolume,
        CinematicCameraPosition,
        MoshEnemySpawnLocation,
        OrdnanceDropPoint,
        TraitZone,
        InitialOrdnanceDropPoint,
        RandomOrdnanceDropPoint,
        ObjectiveOrdnanceDropPoint,
        PersonalOrdnanceDropPoint
    }


    public enum MultiplayerObjectSpawnTimerType : sbyte
    {
        StartsOnDeath,
        StartsOnDisturbance
    }

    public enum GameEngineType : int
    {
        None,
        CaptureTheFlag,
        Slayer,
        Oddball,
        KingOfTheHill,
        Sandbox,
        Vip,
        Juggernaut,
        Territories,
        Assault,
        Infection,
    }

    public enum GameEngineSubType : int
    {
        CaptureTheFlag,
        Slayer,
        Oddball,
        KingOfTheHill,
        Juggernaut,
        Territories,
        Assault,
        Vip,
        Infection,
        TargetTraining,
        All
    }

    [Flags]
    public enum GameEngineSubTypeFlags : ushort
    {
        None = 0,
        CaptureTheFlag = 1 << 0,
        Slayer = 1 << 1,
        Oddball = 1 << 2,
        KingOfTheHill = 1 << 3,
        Juggernaut = 1 << 4,
        Territories = 1 << 5,
        Assault = 1 << 6,
        Vip = 1 << 7,
        Infection = 1 << 8,
        TargetTraining = 1 << 9,
        All = 0x3FF
    }


    [Flags]
    public enum GameEngineFlagsReach : byte
    {
        None = 0,
        Sandbox = 1 << 0,
        Megalogamengine = 1 << 1,
        Campaign = 1 << 2,
        Survival = 1 << 3,
        Firefight = 1 << 4
    }

    [Flags]
    public enum GameEngineSymmetry : int
    {
        Ignore,
        Symmetric,
        Asymmetric,
    }


    [Flags]
    public enum TeleporterPassabilityFlags : byte
    {
        None,
        DisallowPlayers = 1 << 0,
        AllowLightLandVehicles = 1 << 1,
        AllowHeavyLandVehicles = 1 << 2,
        AllowFlyingVehicles = 1 << 3,
        AllowProjectiles = 1 << 4,
    }
}
