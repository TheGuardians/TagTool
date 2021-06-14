using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "custom_app_globals", Tag = "capg", Size = 0x14)]
    public class CustomAppGlobals : TagStructure
    {
        public int MaximumActiveApps; // (-1 = unlimited)
        public CustomAppUpdateFrequencies CustomAppUpdateFrequency;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<CustomAppBlock> CustomApps;
        
        public enum CustomAppUpdateFrequencies : short
        {
            // not supported
            UpdateInstantly,
            UpdateOnRespawn,
            UpdateOnGameStart
        }
        
        [TagStructure(Size = 0x3C)]
        public class CustomAppBlock : TagStructure
        {
            public StringId Name;
            public StringId Headertext;
            public StringId Helptext;
            public StringId IconstringId;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag HudScreenReference;
            public CustomAppFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<GameEnginePlayerTraitsBlock> PlayerTraits;
            public List<CustomAppDamageModifierBlock> DamageTypeModifiers;
            
            [Flags]
            public enum CustomAppFlags : byte
            {
                Locked = 1 << 0
            }
            
            [TagStructure(Size = 0x3C)]
            public class GameEnginePlayerTraitsBlock : TagStructure
            {
                public List<PlayerTraitsVitalityBlock> VitalityTraits;
                public List<PlayerTraitsWeaponsBlock> WeaponTraits;
                public List<PlayerTraitsMovementBlock> MovementTraits;
                public List<PlayerTraitsAppearanceBlock> AppearanceTraits;
                public List<PlayerTraitsSensorsBlock> SensorTraits;
                
                [TagStructure(Size = 0x40)]
                public class PlayerTraitsVitalityBlock : TagStructure
                {
                    public PlayerTraitsVitalityFloatFlags ShouldApplyTrait;
                    public float DamageResistance;
                    public float ShieldMultiplier;
                    public float BodyMultiplier;
                    public float ShieldStunDuration;
                    public float ShieldRechargeRate;
                    public float BodyRechargeRate;
                    public float OvershieldRechargeRate;
                    public float VampirismPercent;
                    // incoming damage multiplied by (1 - resistance)
                    public float ExplosiveDamageResistance;
                    public float WheelmanArmorVehicleStunTimeModifier;
                    public float WheelmanArmorVehicleRechargeTimeModifier;
                    public float WheelmanArmorVehicleEmpDisabledTimeModifier;
                    public float FallDamageMultiplier;
                    public PlayerTraitBoolEnum HeadshotImmunity;
                    public PlayerTraitBoolEnum AssassinationImmunity;
                    public PlayerTraitBoolEnum Deathless;
                    public PlayerTraitBoolEnum FastTrackArmor;
                    public PlayerTraitPowerupCancellationEnum PowerupCancellation;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PlayerTraitsVitalityFloatFlags : uint
                    {
                        DamageResistance = 1 << 0,
                        ShieldMultiplier = 1 << 1,
                        BodyMultiplier = 1 << 2,
                        ShieldStunDuration = 1 << 3,
                        ShieldRechargeRate = 1 << 4,
                        BodyRechargeRate = 1 << 5,
                        OvershieldRechargeRate = 1 << 6,
                        VampirismPercent = 1 << 7,
                        ExplosiveDamageResistance = 1 << 8,
                        WheelmanArmorVehicleStunTimeModifier = 1 << 9,
                        WheelmanArmorVehicleRechargeTimeModifier = 1 << 10,
                        WheelmanArmorVehicleEmpDisabledTimeModifier = 1 << 11,
                        FallDamageMultiplier = 1 << 12
                    }
                    
                    public enum PlayerTraitBoolEnum : sbyte
                    {
                        Unchanged,
                        False,
                        True
                    }
                    
                    public enum PlayerTraitPowerupCancellationEnum : sbyte
                    {
                        Unchanged,
                        None,
                        NoOvershield
                    }
                }
                
                [TagStructure(Size = 0x80)]
                public class PlayerTraitsWeaponsBlock : TagStructure
                {
                    public PlayerTraitsWeaponsFloatFlags ShouldApplyTrait;
                    public float DamageMultiplier;
                    public float MeleeDamageMultiplier;
                    public float GrenadeRechargeSecondsFrag;
                    public float GrenadeRechargeSecondsPlasma;
                    public float GrenadeRechargeSecondsSpike;
                    public float HeroEquipmentEnergyUseRateModifier;
                    public float HeroEquipmentEnergyRechargeDelayModifier;
                    public float HeroEquipmentEnergyRechargeRateModifier;
                    public float HeroEquipmentInitialEnergyModifier;
                    public float EquipmentEnergyUseRateModifier;
                    public float EquipmentEnergyRechargeDelayModifier;
                    public float EquipmentEnergyUseRechargeRateModifier;
                    public float EquipmentEnergyInitialEnergyModifier;
                    public float SwitchSpeedModifier;
                    public float ReloadSpeedModifier;
                    public float OrdnancePointsModifier;
                    public float ExplosiveAreaOfEffectRadiusModifier;
                    public float GunnerArmorModifier;
                    public float StabilityArmorModifier;
                    public float DropReconWarningSeconds;
                    public float DropReconDistanceModifier;
                    public float AssassinationSpeedModifier;
                    public PlayerTraitBoolEnum WeaponPickupAllowed;
                    public PlayerTraitInitialGrenadeCountEnum InitialGrenadeCount;
                    public PlayerTraitInfiniteAmmoEnum InfiniteAmmo;
                    public PlayerTraitEquipmentUsageEnum EquipmentUsage;
                    // false will disable all equipment except auto turret
                    public PlayerTraitEquipmentUsageEnum EquipmentUsageExceptingAutoTurret;
                    public PlayerTraitBoolEnum EquipmentDrop;
                    public PlayerTraitBoolEnum InfiniteEquipment;
                    public PlayerTraitBoolEnum WeaponsAmmopack;
                    public PlayerTraitBoolEnum WeaponsGrenadier;
                    // spawns projectile specified in globals.globals
                    public PlayerTraitBoolEnum WeaponsExplodeOnDeathArmormod;
                    public PlayerTraitBoolEnum OrdnanceMarkersVisible;
                    public PlayerTraitBoolEnum WeaponsOrdnanceRerollAvailable;
                    // grenade probabilities defined in grenade_list.game_globals_grenade_list
                    public PlayerTraitBoolEnum WeaponsResourceful;
                    public PlayerTraitBoolEnum WeaponsWellEquipped;
                    public PlayerTraitBoolEnum OrdnanceDisabled;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public StringId InitialPrimaryWeapon;
                    public StringId InitialSecondaryWeapon;
                    public StringId InitialEquipment;
                    public StringId InitialTacticalPackage;
                    public StringId InitialSupportUpgrade;
                    
                    [Flags]
                    public enum PlayerTraitsWeaponsFloatFlags : uint
                    {
                        DamageMultiplier = 1 << 0,
                        MeleeDamageMultiplier = 1 << 1,
                        GrenadeRechargeSecondsFrag = 1 << 2,
                        GrenadeRechargeSecondsPlasma = 1 << 3,
                        GrenadeRechargeSecondsSpike = 1 << 4,
                        HeroEquipmentEnergyUseRateModifier = 1 << 5,
                        HeroEquipmentEnergyRechargeDelayModifier = 1 << 6,
                        HeroEquipmentEnergyRechargeRateModifier = 1 << 7,
                        HeroEquipmentInitialEnergyModifier = 1 << 8,
                        EquipmentEnergyUseRateModifier = 1 << 9,
                        EquipmentEnergyRechargeDelayModifier = 1 << 10,
                        EquipmentEnergyUseRechargeRateModifier = 1 << 11,
                        EquipmentEnergyInitialEnergyModifier = 1 << 12,
                        SwitchSpeedModifier = 1 << 13,
                        ReloadSpeedModifier = 1 << 14,
                        OrdnancePointsModifier = 1 << 15,
                        ExplosiveAreaOfEffectRadiusModifier = 1 << 16,
                        GunnerArmorModifier = 1 << 17,
                        StabilityArmorModifier = 1 << 18,
                        DropReconWarningSeconds = 1 << 19,
                        DropReconDistanceModifier = 1 << 20,
                        AssassinationSpeedModifier = 1 << 21
                    }
                    
                    public enum PlayerTraitBoolEnum : sbyte
                    {
                        Unchanged,
                        False,
                        True
                    }
                    
                    public enum PlayerTraitInitialGrenadeCountEnum : sbyte
                    {
                        Unchanged,
                        MapDefault,
                        _0,
                        _1Frag,
                        _2Frag,
                        _1Plasma,
                        _2Plasma,
                        _1Type2,
                        _2Type2,
                        _1Type3,
                        _2Type3,
                        _1Type4,
                        _2Type4,
                        _1Type5,
                        _2Type5,
                        _1Type6,
                        _2Type6,
                        _1Type7,
                        _2Type7
                    }
                    
                    public enum PlayerTraitInfiniteAmmoEnum : sbyte
                    {
                        Unchanged,
                        Off,
                        On,
                        BottomlessClip
                    }
                    
                    public enum PlayerTraitEquipmentUsageEnum : sbyte
                    {
                        Unchanged,
                        Off,
                        NotWithObjectives,
                        On
                    }
                }
                
                [TagStructure(Size = 0x1C)]
                public class PlayerTraitsMovementBlock : TagStructure
                {
                    public PlayerTraitsMovementFloatFlags ShouldApplyTrait;
                    public float Speed;
                    public float GravityMultiplier;
                    public float JumpMultiplier;
                    public float TurnSpeedMultiplier;
                    public PlayerTraitVehicleUsage VehicleUsage;
                    public PlayerTraitDoubleJump DoubleJump;
                    public PlayerTraitBoolEnum SprintUsage;
                    public PlayerTraitBoolEnum AutomaticMomentumUsage;
                    public PlayerTraitBoolEnum VaultingEnabled;
                    public PlayerTraitBoolEnum Stealthy;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PlayerTraitsMovementFloatFlags : uint
                    {
                        Speed = 1 << 0,
                        GravityMultiplier = 1 << 1,
                        JumpMultiplier = 1 << 2,
                        TurnSpeedMultiplier = 1 << 3
                    }
                    
                    public enum PlayerTraitVehicleUsage : sbyte
                    {
                        Unchanged,
                        None,
                        PassengerOnly,
                        DriverOnly,
                        GunnerOnly,
                        NotPassenger,
                        NotDriver,
                        NotGunner,
                        Full
                    }
                    
                    public enum PlayerTraitDoubleJump : sbyte
                    {
                        Unchanged,
                        Off,
                        On,
                        OnPlusLunge
                    }
                    
                    public enum PlayerTraitBoolEnum : sbyte
                    {
                        Unchanged,
                        False,
                        True
                    }
                }
                
                [TagStructure(Size = 0xC)]
                public class PlayerTraitsAppearanceBlock : TagStructure
                {
                    public PlayerTraitActiveCamo ActiveCamo;
                    public PlayerTraitWaypoint Waypoint;
                    public PlayerTraitWaypoint GamertagVisible;
                    public PlayerTraitAura Aura;
                    [TagField(Length = 32)]
                    public string DeathEffect;
                    public StringId AttachedEffect;
                    
                    public enum PlayerTraitActiveCamo : sbyte
                    {
                        Unchanged,
                        Off,
                        Poor,
                        Good,
                        Excellent,
                        Invisible
                    }
                    
                    public enum PlayerTraitWaypoint : sbyte
                    {
                        Unchanged,
                        Off,
                        Allies,
                        All
                    }
                    
                    public enum PlayerTraitAura : sbyte
                    {
                        Unchanged,
                        Off,
                        TeamColor,
                        Black,
                        White
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class PlayerTraitsSensorsBlock : TagStructure
                {
                    public PlayerTraitsSensorsFloatFlags ShouldApplyTrait;
                    public float MotionTrackerRange;
                    public float NemesisDuration; // seconds
                    public PlayerTraitMotionTracker MotionTracker;
                    public PlayerTraitBoolEnum MotionTrackerWhileZoomed;
                    public PlayerTraitBoolEnum DirectionalDamageIndicator;
                    public PlayerTraitBoolEnum VisionMode;
                    public PlayerTraitBoolEnum BattleAwareness;
                    public PlayerTraitBoolEnum ThreatView;
                    public PlayerTraitBoolEnum AuralEnhancement;
                    public PlayerTraitBoolEnum Nemesis;
                    
                    [Flags]
                    public enum PlayerTraitsSensorsFloatFlags : uint
                    {
                        MotionTrackerRange = 1 << 0,
                        NemesisDuration = 1 << 1
                    }
                    
                    public enum PlayerTraitMotionTracker : sbyte
                    {
                        Unchanged,
                        Off,
                        MovingFriendlyBipedsMovingNeutralVehicles,
                        MovingBipedsMovingVehicles,
                        AllBipedsMovingVehicles
                    }
                    
                    public enum PlayerTraitBoolEnum : sbyte
                    {
                        Unchanged,
                        False,
                        True
                    }
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class CustomAppDamageModifierBlock : TagStructure
            {
                public StringId DamageType;
                public float DamageResistanceMultiplier;
            }
        }
    }
}
