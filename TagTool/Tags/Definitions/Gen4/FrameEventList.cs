using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "frame_event_list", Tag = "frms", Size = 0x24)]
    public class FrameEventList : TagStructure
    {
        public List<AnimationGraphSoundReferenceBlock> SoundReferencesAbcdcc;
        public List<AnimationGraphEffectReferenceBlock> EffectReferencesAbcdcc;
        public List<ImportAnimationEventBlock> FrameEventsAbcdcc;
        
        [TagStructure(Size = 0x28)]
        public class AnimationGraphSoundReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag Sound;
            public KeyEventFlagsEnum Flags;
            public KeyEventInternalFlagsEnum InternalFlags;
            // optional. only allow this event when used on this model
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag Model;
            // optional. only allow this event when used on this model variant
            public StringId Variant;
            
            [Flags]
            public enum KeyEventFlagsEnum : ushort
            {
                AllowOnPlayer = 1 << 0,
                LeftArmOnly = 1 << 1,
                RightArmOnly = 1 << 2,
                FirstPersonOnly = 1 << 3,
                ThirdPersonOnly = 1 << 4,
                ForwardOnly = 1 << 5,
                ReverseOnly = 1 << 6,
                FpNoAgedWeapons = 1 << 7
            }
            
            [Flags]
            public enum KeyEventInternalFlagsEnum : ushort
            {
                ModelIndexRequired = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class AnimationGraphEffectReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag Effect;
            public KeyEventFlagsEnum Flags;
            public KeyEventInternalFlagsEnum InternalFlags;
            // optional. only allow this event when used on this model
            [TagField(ValidTags = new [] { "hlmt" })]
            public CachedTag Model;
            // optional. only allow this event when used on this model variant
            public StringId Variant;
            
            [Flags]
            public enum KeyEventFlagsEnum : ushort
            {
                AllowOnPlayer = 1 << 0,
                LeftArmOnly = 1 << 1,
                RightArmOnly = 1 << 2,
                FirstPersonOnly = 1 << 3,
                ThirdPersonOnly = 1 << 4,
                ForwardOnly = 1 << 5,
                ReverseOnly = 1 << 6,
                FpNoAgedWeapons = 1 << 7
            }
            
            [Flags]
            public enum KeyEventInternalFlagsEnum : ushort
            {
                ModelIndexRequired = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class ImportAnimationEventBlock : TagStructure
        {
            public StringId AnimationName;
            public int AnimationFrameCount;
            public List<ImportFrameEventBlock> AnimationEvents;
            public List<AnimationSoundEventBlockExtended> SoundEvents;
            public List<AnimationEffectsEventBlockExtended> EffectEvents;
            public List<AnimationDialogueEventBlockExtended> DialogueEvents;
            public List<AnimationScriptEventBlockExtended> ScriptEvents;
            
            [TagStructure(Size = 0x14)]
            public class ImportFrameEventBlock : TagStructure
            {
                public StringId EventName;
                public StringId AnimationName;
                public short Frame;
                public short FrameOffset;
                public FrameEventTypeNew Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // (Do not change this)
                public int UniqueId;
                
                public enum FrameEventTypeNew : short
                {
                    None,
                    PrimaryKeyframe,
                    SecondaryKeyframe,
                    TertiaryKeyframe,
                    LeftFoot,
                    RightFoot,
                    AllowInterruption,
                    DoNotAllowInterruption,
                    BothFeetShuffle,
                    BodyImpact,
                    LeftFootLock,
                    LeftFootUnlock,
                    RightFootLock,
                    RightFootUnlock,
                    BlendRangeMarker,
                    StrideExpansion,
                    StrideContraction,
                    RagdollKeyframe,
                    DropWeaponKeyframe,
                    MatchA,
                    MatchB,
                    MatchC,
                    MatchD,
                    JetpackClosed,
                    JetpackOpen,
                    SoundEvent,
                    EffectEvent
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class AnimationSoundEventBlockExtended : TagStructure
            {
                public short FrameEvent;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Sound;
                // If a frame event is set, this number is relative to frame event, otherwise it's absolute.
                public short FrameOffset;
                public StringId MarkerName;
            }
            
            [TagStructure(Size = 0x10)]
            public class AnimationEffectsEventBlockExtended : TagStructure
            {
                public short FrameEvent;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short Effect;
                // If a frame event is set, this number is relative to frame event, otherwise it's absolute.
                public short FrameOffset;
                public StringId MarkerName;
                public GlobalDamageReportingEnum DamageEffectReportingType;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum GlobalDamageReportingEnum : sbyte
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
            }
            
            [TagStructure(Size = 0x8)]
            public class AnimationDialogueEventBlockExtended : TagStructure
            {
                public short FrameEvent;
                public AnimationDialogueEventEnum DialogueEvent;
                // If a frame event is set, this number is relative to frame event, otherwise it's absolute.
                public short FrameOffset;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum AnimationDialogueEventEnum : short
                {
                    Bump,
                    Dive,
                    Evade,
                    Lift,
                    Sigh,
                    Contempt,
                    Anger,
                    Fear,
                    Relief,
                    Sprint,
                    SprintEnd,
                    AssGrabber,
                    KillAss,
                    AssGrabbed,
                    DieAss
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class AnimationScriptEventBlockExtended : TagStructure
            {
                public short FrameEvent;
                // If a frame event is set, this number is relative to frame event, otherwise it's absolute.
                public short FrameOffset;
                public StringId ScriptName;
            }
        }
    }
}
