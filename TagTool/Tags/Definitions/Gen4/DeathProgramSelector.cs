using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "death_program_selector", Tag = "bdpd", Size = 0x1C)]
    public class DeathProgramSelector : TagStructure
    {
        [TagField(ValidTags = new [] { "bdpd" })]
        public CachedTag Parent;
        public List<DeathProgramSpecialBlock> SpecialType;
        
        [TagStructure(Size = 0x10)]
        public class DeathProgramSpecialBlock : TagStructure
        {
            public ObjectDamageAftermathSpecialDamageTypeEnum SpecialType;
            public List<DeathProgramDamageReportingBlock> DamageType;
            
            public enum ObjectDamageAftermathSpecialDamageTypeEnum : int
            {
                None,
                Headshot,
                Melee,
                Collision,
                Assassination
            }
            
            [TagStructure(Size = 0x10)]
            public class DeathProgramDamageReportingBlock : TagStructure
            {
                public GlobalDamageReportingEnum DamageType;
                public List<DeathProgramVelocityGateBlock> Velocity;
                
                public enum GlobalDamageReportingEnum : int
                {
                    Unknown,
                    TehGuardians,
                    Scripting,
                    AiSuicide,
                    ForerunnerSmg,
                    SpreadGun,
                    ForerunnerRifle,
                    ForerunnerSniper,
                    BishopBeam,
                    BoltPistol,
                    PulseGrenade,
                    IncinerationLauncher,
                    MagnumPistol,
                    AssaultRifle,
                    MarksmanRifle,
                    Shotgun,
                    BattleRifle,
                    SniperRifle,
                    RocketLauncher,
                    SpartanLaser,
                    FragGrenade,
                    StickyGrenadeLauncher,
                    LightMachineGun,
                    RailGun,
                    PlasmaPistol,
                    Needler,
                    GravityHammer,
                    EnergySword,
                    PlasmaGrenade,
                    Carbine,
                    BeamRifle,
                    AssaultCarbine,
                    ConcussionRifle,
                    FuelRodCannon,
                    Ghost,
                    RevenantDriver,
                    RevenantGunner,
                    Wraith,
                    WraithAntiInfantry,
                    Banshee,
                    BansheeBomb,
                    Seraph,
                    RevenantDeuxDriver,
                    RevenantDeuxGunner,
                    LichDriver,
                    LichGunner,
                    Mongoose,
                    WarthogDriver,
                    WarthogGunner,
                    WarthogGunnerGauss,
                    WarthogGunnerRocket,
                    Scorpion,
                    ScorpionGunner,
                    FalconDriver,
                    FalconGunner,
                    WaspDriver,
                    WaspGunner,
                    WaspGunnerHeavy,
                    MechMelee,
                    MechChaingun,
                    MechCannon,
                    MechRocket,
                    Broadsword,
                    BroadswordMissile,
                    TortoiseDriver,
                    TortoiseGunner,
                    MacCannon,
                    TargetDesignator,
                    OrdnanceDropPod,
                    OrbitalCruiseMissile,
                    PortableShield,
                    PersonalAutoTurret,
                    ThrusterPack,
                    FallingDamage,
                    GenericCollisionDamage,
                    GenericMeleeDamage,
                    GenericExplosion,
                    FireDamage,
                    BirthdayPartyExplosion,
                    FlagMeleeDamage,
                    BombMeleeDamage,
                    BombExplosionDamage,
                    BallMeleeDamage,
                    Teleporter,
                    TransferDamage,
                    ArmorLockCrush,
                    HumanTurret,
                    PlasmaCannon,
                    PlasmaMortar,
                    PlasmaTurret,
                    ShadeTurret,
                    ForerunnerTurret,
                    Tank,
                    Chopper,
                    Hornet,
                    Mantis,
                    MagnumPistolCtf,
                    FloodProngs
                }
                
                [TagStructure(Size = 0x10)]
                public class DeathProgramVelocityGateBlock : TagStructure
                {
                    // this death program will be selected if the biped's velocity is above this number^
                    public float MinimumVelocity; // wu/s
                    public DeathProgramResultEnum DeathProgram;
                    // input to the death program to scale the result (only affects ragdolls)
                    public float DeathProgramScale;
                    // Override stance that contains the death animations for this gait speed when using animate then ragdoll option.
                    public StringId DeathAnimationStance;
                    
                    public enum DeathProgramResultEnum : int
                    {
                        AnimateThenRagdoll,
                        DefaultRagdollProgram,
                        HeadshotRagdollProgram,
                        MeleeRagdollProgram
                    }
                }
            }
        }
    }
}
