using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0x214)]
    public class ModelAnimationGraph : TagStructure
    {
        public AnimationGraphDefinitionsStruct Definitions;
        public AnimationGraphContentsStruct Content;
        public ModelAnimationRuntimeDataStruct RunTimeData;
        public List<AdditionalNodeDataBlock> AdditionalNodeData;
        public List<ModelAnimationTagResourceGroup> TagResourceGroups;
        public AnimationCodecDataStruct CodecData;
        
        [TagStructure(Size = 0x138)]
        public class AnimationGraphDefinitionsStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "jmad" })]
            public CachedTag ParentAnimationGraph;
            public PublicAnimationGraphFlags InheritanceFlags;
            public PrivateAnimationGraphFlags PrivateFlags;
            public short AnimationCodecPack;
            public CompressionForceSettings ForceCompressionSetting;
            public AnimationGraphMiscFlags MiscGraphFlags;
            public int SkeletonChecksum;
            public int SkeletonChecksumLite;
            [TagField(ValidTags = new [] { "frms" })]
            public CachedTag ImportedEventsAbcdcc;
            public List<AnimationUsageBlock> NodeUsage;
            public List<AnimationNodeMaskBlock> NodeMasksAbcdcc;
            public List<AnimationFunctionBlock> FunctionsAbcdcc;
            public List<ModelAnimationVariantBlock> ModelAnimationVariantsAbcdcc;
            public List<AnimationGraphNodeBlock> SkeletonNodes;
            // Legacy field - please edit in new frame event tag below
            public List<AnimationGraphSoundReferenceBlock> SoundReferencesAbcdcc;
            // Legacy field - please edit in new frame event tag below
            public List<AnimationGraphEffectReferenceBlock> EffectReferencesAbcdcc;
            // Legacy field - please edit in NEW blend screens tag below
            public List<AnimationBlendScreenBlock> BlendScreensAbcdcc;
            public List<FootTrackingMemberBlock> FootMarkersAbcdcc;
            public List<AnimationPoolBlockStruct> Animations;
            public List<NewAnimationBlendScreenBlockStruct> NewBlendScreensCcbbaa;
            public List<NewAnimationFunctionOverlayBlock> NewFunctionOverlaysCcaabb;
            public List<OverlayGroupDefinitionBlock> OverlayGroups;
            public List<AnimationGaitBlock> GaitsAbcdcc;
            public List<AnimationGaitGroupBlock> GaitGroupsAbcdcc;
            public List<AnimationIkBlock> IkDataCcbbaa;
            public List<AnimationIkSet> IkSetsCcbbaa;
            public List<AnimationIkChainBlock> IkChainsCcbbaa;
            public List<GCompositetagStruct> Composites;
            public PcaanimationDataStruct PcaData;
            
            [Flags]
            public enum PublicAnimationGraphFlags : byte
            {
                InheritRootTransScaleOnly = 1 << 0,
                InheritForUseOnPlayer = 1 << 1
            }
            
            [Flags]
            public enum PrivateAnimationGraphFlags : byte
            {
                PreparedForCache = 1 << 0,
                UseASingleMixingBoard = 1 << 1,
                ImportedWithCodecCompressors = 1 << 2,
                UsesDataDrivenAnimation = 1 << 3,
                WrittenToCache = 1 << 4,
                AnimationDataReordered = 1 << 5,
                ReadyForUse = 1 << 6
            }
            
            public enum CompressionForceSettings : short
            {
                None,
                MediumCompression,
                RoughCompression,
                Uncompressed,
                OldCodec,
                ReachMediumCompression,
                ReachRoughCompression
            }
            
            [Flags]
            public enum AnimationGraphMiscFlags : ushort
            {
                IgnoreGaits = 1 << 0
            }
            
            [TagStructure(Size = 0x4)]
            public class AnimationUsageBlock : TagStructure
            {
                public AnimationUsageEnumeration Usage;
                public short NodeToUse;
                
                public enum AnimationUsageEnumeration : short
                {
                    PhysicsControl,
                    CameraControl,
                    OriginMarker,
                    LeftClavicle,
                    LeftUpperarm,
                    PoseBlendPitch,
                    PoseBlendYaw,
                    Pedestal,
                    Pelvis,
                    LeftFoot,
                    RightFoot,
                    DamageRootGut,
                    DamageRootChest,
                    DamageRootHead,
                    DamageRootLeftShoulder,
                    DamageRootLeftArm,
                    DamageRootLeftLeg,
                    DamageRootLeftFoot,
                    DamageRootRightShoulder,
                    DamageRootRightArm,
                    DamageRootRightLeg,
                    DamageRootRightFoot,
                    LeftHand,
                    RightHand,
                    WeaponIk
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class AnimationNodeMaskBlock : TagStructure
            {
                public StringId Name;
                public List<AnimationNodeMaskEntryBlock> Nodes;
                [TagField(Length = 8)]
                public GNodeFlagStorageArray[]  NodeFlags;
                
                [TagStructure(Size = 0x4)]
                public class AnimationNodeMaskEntryBlock : TagStructure
                {
                    public short Node;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0x4)]
                public class GNodeFlagStorageArray : TagStructure
                {
                    public int FlagData;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class AnimationFunctionBlock : TagStructure
            {
                public StringId Name;
                public ScalarFunctionNamedStruct Function;
                
                [TagStructure(Size = 0x14)]
                public class ScalarFunctionNamedStruct : TagStructure
                {
                    public MappingFunction Function;
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ModelAnimationVariantBlock : TagStructure
            {
                public StringId VariantName;
                public ModelAnimationVariantFlags VariantFlags;
                public List<ModeOrStanceAliasBlockStruct> ModeOrStanceAliases;
                
                [Flags]
                public enum ModelAnimationVariantFlags : uint
                {
                    NeedsMandibleReplacement = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class ModeOrStanceAliasBlockStruct : TagStructure
                {
                    public StringId ModeOrStance;
                    public StringId Alias;
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class AnimationGraphNodeBlock : TagStructure
            {
                public StringId Name;
                public short NextSiblingNodeIndex;
                public short FirstChildNodeIndex;
                public short ParentNodeIndex;
                public AnimationNodeModelFlags ModelFlags;
                public NodeJointFlags NodeJointFlags1;
                public NodeInfoFlags AdditionalFlags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d BaseVector;
                public float VectorRange;
                public float ZPos;
                public int FrameId1;
                public int FrameId2;
                
                [Flags]
                public enum AnimationNodeModelFlags : byte
                {
                    PrimaryModel = 1 << 0,
                    SecondaryModel = 1 << 1,
                    LocalRoot = 1 << 2,
                    LeftHand = 1 << 3,
                    RightHand = 1 << 4,
                    LeftArmMember = 1 << 5
                }
                
                [Flags]
                public enum NodeJointFlags : byte
                {
                    BallSocket = 1 << 0,
                    Hinge = 1 << 1,
                    NoMovement = 1 << 2
                }
                
                [Flags]
                public enum NodeInfoFlags : byte
                {
                    ImportantForImposter = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class AnimationGraphSoundReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Sound;
                public KeyEventFlagsEnum Flags;
                public KeyEventInternalFlagsEnum InternalFlags;
                [TagField(ValidTags = new [] { "hlmt" })]
                // optional. only allow this event when used on this model
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
                [TagField(ValidTags = new [] { "hlmt" })]
                // optional. only allow this event when used on this model
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
            
            [TagStructure(Size = 0x1C)]
            public class AnimationBlendScreenBlock : TagStructure
            {
                public StringId Label;
                public AnimationAimingScreenStruct AimingScreen;
                
                [TagStructure(Size = 0x18)]
                public class AnimationAimingScreenStruct : TagStructure
                {
                    public Angle RightYawPerFrame;
                    public Angle LeftYawPerFrame;
                    public short RightFrameCount;
                    public short LeftFrameCount;
                    public Angle DownPitchPerFrame;
                    public Angle UpPitchPerFrame;
                    public short DownPitchFrameCount;
                    public short UpPitchFrameCount;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class FootTrackingMemberBlock : TagStructure
            {
                public StringId FootMarkerName;
                public Bounds<float> FootIkRange;
                public StringId AnkleMarkerName;
                public Bounds<float> AnkleIkRange;
                public FootTrackingDefaultValues DefaultState;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum FootTrackingDefaultValues : short
                {
                    Off,
                    On
                }
            }
            
            [TagStructure(Size = 0x40)]
            public class AnimationPoolBlockStruct : TagStructure
            {
                public StringId Name;
                public float Weight;
                public short LoopFrameIndex;
                public AnimationIndexFlags UserFlags;
                public float OverrideBlendInTime;
                public float OverrideBlendOutTime;
                public short ParentAnimation;
                public short NextAnimation;
                public ProductionStatusFlags ProductionFlags;
                public short Composite;
                public StringId PcaGroupName;
                public SharedAnimationReferenceBlock SharedAnimationReference;
                public List<SharedModelAnimationBlock> SharedAnimationData;
                
                [Flags]
                public enum AnimationIndexFlags : ushort
                {
                    DisableInterpolationIn = 1 << 0,
                    DisableInterpolationOut = 1 << 1,
                    DisableModeIk = 1 << 2,
                    DisableWeaponIk = 1 << 3,
                    DisableWeaponAimFirstPerson = 1 << 4,
                    DisableLookScreen = 1 << 5,
                    DisableTransitionAdjustment = 1 << 6,
                    ForceWeaponIkOn = 1 << 7,
                    // when possible, interpolate into this animation using the 'blend in' time value below
                    UseCustomBlendInTime = 1 << 8,
                    EnableAnimatedSourceInterpolation = 1 << 9,
                    DisableIkSets = 1 << 10,
                    DisableIkChains = 1 << 11,
                    // ignore all transtion and scale on all nodes except the root
                    TranslateAndScaleRootOnly = 1 << 12,
                    // fade out this animation as they reach the end of the animation.
                    EnableBlendOutOnReplacementAnims = 1 << 13,
                    // when possible, fade this animation out over the 'blend out' time below (requires 'enable blend-out' above,
                    // replacement anims only)
                    UseCustomBlendOutTime = 1 << 14,
                    // use the movement data in this anim instead of player physics (player only)
                    OverridePlayerInputWithMotion = 1 << 15
                }
                
                [Flags]
                public enum ProductionStatusFlags : ushort
                {
                    DoNotMonitorChanges = 1 << 0,
                    VerifySoundEvents = 1 << 1,
                    DoNotInheritForPlayerGraphs = 1 << 2,
                    HasErrorsOrWarnings = 1 << 3,
                    KeepRawDataInTag = 1 << 4,
                    // prevents foot-ik from settling
                    AllowBallRollOnFoot = 1 << 5
                }
                
                [TagStructure(Size = 0x14)]
                public class SharedAnimationReferenceBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "jmad" })]
                    public CachedTag GraphReference;
                    public short SharedAnimationIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
                
                [TagStructure(Size = 0xDC)]
                public class SharedModelAnimationBlock : TagStructure
                {
                    public short FrameCount;
                    public byte NodeCount;
                    public AnimationTypeEnum AnimationType;
                    public FrameInfoTypeEnum FrameInfoType;
                    public FrameInfoTypeEnum DesiredFrameInfoType;
                    public CompressionSettings DesiredCompression;
                    public CompressionSettings CurrentCompression;
                    public InternalAnimationFlags InternalFlags;
                    public short CompressorVersion;
                    public int Uid;
                    public StringId SharedId;
                    public int NodeListChecksum;
                    public short ResourceGroup;
                    public short ResourceGroupMember;
                    public RealVector3d Heading;
                    public float HeadingAngle;
                    public float AverageTranslationMagnitude;
                    public float AveragePivotYaw;
                    // Legacy field - please edit in new frame event tag below
                    public List<AnimationFrameEventBlockStruct> FrameEventsAbcdcc;
                    // Legacy field - please edit in new frame event tag below
                    public List<AnimationSoundEventBlock> SoundEventsAbcdcc;
                    // Legacy field - please edit in new frame event tag below
                    public List<AnimationEffectEventBlock> EffectEventsAbcdcc;
                    // Legacy field - please edit in new frame event tag below
                    public List<AnimationDialogueEventBlock> DialogueEventsAbcdcc;
                    // Legacy field - please edit in new frame event tag below
                    public List<AnimationScriptEventBlock> ScriptEventsAbcdcc;
                    public List<ObjectSpaceNodeDataBlock> ObjectSpaceParentNodesAbcdcc;
                    public List<FootTrackingBlock> FootTrackingAbcdcc;
                    public List<ObjectSpaceOffsetNodeBlock> ObjectSpaceOffsetNodesAbcdcc;
                    public List<FikAnchorNodeBlock> ForwardInvertKineticAnchorNodesAbcdcc;
                    public List<AnimationIkChainEventsStruct> IkChainEvents;
                    public List<AnimationIkChainProxiesStruct> IkChainProxies;
                    public List<AnimationFacialWrinkleEventsStruct> FacialWrinkleEvents;
                    public List<AnimationExtendedEventsStruct> ExtendedDataEvents;
                    public List<AnimationObjectFunctionsStruct> AnimationObjectFunctions;
                    
                    public enum AnimationTypeEnum : sbyte
                    {
                        None,
                        Base,
                        Overlay,
                        Replacement
                    }
                    
                    public enum FrameInfoTypeEnum : sbyte
                    {
                        None,
                        DxDy,
                        DxDyDyaw,
                        DxDyDzDyaw,
                        DxDyDzDangleAxis,
                        XYZAbsolute,
                        Auto
                    }
                    
                    public enum CompressionSettings : sbyte
                    {
                        BestScore,
                        BestCompression,
                        BestAccuracy,
                        OldCodec,
                        ReachMediumCompression,
                        ReachRoughCompression
                    }
                    
                    [Flags]
                    public enum InternalAnimationFlags : ushort
                    {
                        Unused0 = 1 << 0,
                        WorldRelative = 1 << 1,
                        Unused1 = 1 << 2,
                        Unused3 = 1 << 3,
                        Unused2 = 1 << 4,
                        ResourceGroup = 1 << 5,
                        CompressionDisabled = 1 << 6,
                        OldProductionChecksum = 1 << 7,
                        ValidProductionChecksum = 1 << 8,
                        OverrideForceCompression = 1 << 9,
                        ContainsPcaData = 1 << 10
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class AnimationFrameEventBlockStruct : TagStructure
                    {
                        public FrameEventTypeNew Type;
                        public short Frame;
                        
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
                    
                    [TagStructure(Size = 0x8)]
                    public class AnimationSoundEventBlock : TagStructure
                    {
                        public short Sound;
                        public short Frame;
                        public StringId MarkerName;
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class AnimationEffectEventBlock : TagStructure
                    {
                        public short Effect;
                        public short Frame;
                        public StringId MarkerName;
                        public GlobalDamageReportingEnum DamageEffectReportingType;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
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
                    
                    [TagStructure(Size = 0x4)]
                    public class AnimationDialogueEventBlock : TagStructure
                    {
                        public AnimationDialogueEventEnum DialogueEvent;
                        public short Frame;
                        
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
                    public class AnimationScriptEventBlock : TagStructure
                    {
                        public StringId ScriptName;
                        public short Frame;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class ObjectSpaceNodeDataBlock : TagStructure
                    {
                        public short NodeIndex;
                        public ObjectSpaceNodeFlags Flags;
                        public QuantizedOrientationStruct ParentOrientation;
                        
                        [Flags]
                        public enum ObjectSpaceNodeFlags : ushort
                        {
                            Rotation = 1 << 0,
                            Translation = 1 << 1,
                            Scale = 1 << 2,
                            MotionRoot = 1 << 3
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class QuantizedOrientationStruct : TagStructure
                        {
                            public short RotationX;
                            public short RotationY;
                            public short RotationZ;
                            public short RotationW;
                            public RealPoint3d DefaultTranslation;
                            public float DefaultScale;
                        }
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class FootTrackingBlock : TagStructure
                    {
                        public short Foot;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public List<FootLockCycleBlock> CyclesAbcdcc;
                        
                        [TagStructure(Size = 0x14)]
                        public class FootLockCycleBlock : TagStructure
                        {
                            public short StartLocking;
                            public short Locked;
                            public short StartUnlocking;
                            public short Unlocked;
                            public RealPoint3d LockPoint;
                        }
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class ObjectSpaceOffsetNodeBlock : TagStructure
                    {
                        public short ObjectSpaceOffsetNode;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class FikAnchorNodeBlock : TagStructure
                    {
                        public short AnchorNode;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class AnimationIkChainEventsStruct : TagStructure
                    {
                        public StringId ChainName;
                        public AnimationIkChainTypeEnumeration ChainType;
                        public short ChainStartNode;
                        public short ChainEffectorNode;
                        public AnimationIkChainEventUsage ChainUsage;
                        public StringId ProxyMarker;
                        public int ProxyId;
                        public AnimationIkChainEventType EventType;
                        public byte EffectorTransformDataIndex;
                        public byte EffectorWeightDataIndex;
                        public byte PolePointDataIndex;
                        public byte ChainIndex;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        public enum AnimationIkChainTypeEnumeration : short
                        {
                            Standard
                        }
                        
                        public enum AnimationIkChainEventUsage : short
                        {
                            None,
                            World,
                            Self,
                            Parent,
                            PrimaryWeapon,
                            SecondaryWeapon,
                            Assassination
                        }
                        
                        public enum AnimationIkChainEventType : sbyte
                        {
                            Active,
                            Passive
                        }
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class AnimationIkChainProxiesStruct : TagStructure
                    {
                        public int Id;
                        public StringId TargetMarker;
                        public byte ProxyTransformDataIndex;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class AnimationFacialWrinkleEventsStruct : TagStructure
                    {
                        public StringId WrinkleName;
                        public float DefaultValue;
                        public short StartFrame;
                        public short FrameCount;
                        public AnimationFacialWrinkleRegion Region;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public short WrinkleDataIndex;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding1;
                        
                        public enum AnimationFacialWrinkleRegion : sbyte
                        {
                            UpperBrow,
                            CenterBrow,
                            LeftSquint,
                            RightSquint,
                            LeftSmile,
                            RightSmile,
                            LeftSneer,
                            RightSneer
                        }
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class AnimationExtendedEventsStruct : TagStructure
                    {
                        public StringId Name;
                        public short StartFrame;
                        public short FrameCount;
                        public float DefaultValue;
                        public short DataIndex;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class AnimationObjectFunctionsStruct : TagStructure
                    {
                        public StringId RealName;
                        public AnimationObjectFunctionName Name;
                        public short StartFrame;
                        public short FrameCount;
                        public ScalarFunctionNamedStruct FunctionCurve;
                        
                        public enum AnimationObjectFunctionName : int
                        {
                            AnimationObjectFunctionA,
                            AnimationObjectFunctionB,
                            AnimationObjectFunctionC,
                            AnimationObjectFunctionD
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class ScalarFunctionNamedStruct : TagStructure
                        {
                            public MappingFunction Function;
                            
                            [TagStructure(Size = 0x14)]
                            public class MappingFunction : TagStructure
                            {
                                public byte[] Data;
                            }
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class NewAnimationBlendScreenBlockStruct : TagStructure
            {
                public StringId Name;
                public BlendScreenDefinitionFlags Flags;
                public float Weight;
                // A value of zero or one means no interpolation.
                public float InterpolationRate; // [0,1]
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public BlendScreenVariableSources YawSource;
                public BlendScreenVariableSources PitchSource;
                public BlendScreenWeightSources WeightSource;
                public StringId YawSourceObjectFunction;
                public StringId PitchSourceObjectFunction;
                public StringId WeightSourceObjectFunction;
                // Function applied to input from weight function source
                public short WeightFunction;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public AnimationIndexStruct Animation;
                
                [Flags]
                public enum BlendScreenDefinitionFlags : uint
                {
                    ActiveOnlyWhenWeaponDown = 1 << 0,
                    AttemptPieceWiseBlending = 1 << 1,
                    AllowParentAdjustment = 1 << 2
                }
                
                public enum BlendScreenVariableSources : short
                {
                    None,
                    ObjectFunction,
                    HorizontalTest,
                    VerticalTest,
                    AimYaw,
                    AimPitch,
                    LookYaw,
                    LookPitch,
                    ObjectYaw,
                    ObjectPitch,
                    AccelerationYaw,
                    AccelerationPitch,
                    Steering,
                    VelocityYaw,
                    VelocityPitch,
                    DamageGutYaw,
                    DamageGutPitch,
                    DamageChestYaw,
                    DamageChestPitch,
                    DamageHeadYaw,
                    DamageHeadPitch,
                    DamageLeftShoulderYaw,
                    DamageLeftShoulderPitch,
                    DamageLeftArmYaw,
                    DamageLeftArmPitch,
                    DamageLeftLegYaw,
                    DamageLeftLegPitch,
                    DamageLeftFootYaw,
                    DamageLeftFootPitch,
                    DamageRightShoulderYaw,
                    DamageRightShoulderPitch,
                    DamageRightArmYaw,
                    DamageRightArmPitch,
                    DamageRightLegYaw,
                    DamageRightLegPitch,
                    DamageRightFootYaw,
                    DamageRightFootPitch,
                    DefenseYaw,
                    DefensePitch,
                    FirstPersonPitch,
                    FirstPersonTurn,
                    ThrottleSide,
                    ThrottleForward
                }
                
                public enum BlendScreenWeightSources : short
                {
                    None,
                    ObjectFunction,
                    AccelerationMagnitude,
                    VelocityMagnitude,
                    StateFunctionA,
                    StateFunctionB,
                    StateFunctionC,
                    StateFunctionD,
                    DamageChest,
                    DamageGut,
                    DamageHead,
                    DamageLeftShoulder,
                    DamageLeftArm,
                    DamageLeftLeg,
                    DamageLeftFoot,
                    DamageRightShoulder,
                    DamageRightArm,
                    DamageRightLeg,
                    DamageRightFoot,
                    Defense
                }
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStruct : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class NewAnimationFunctionOverlayBlock : TagStructure
            {
                public StringId Name;
                public FunctionOverlayDefinitionFlags Flags;
                public StringId FrameRatioObjectFunction;
                public StringId PlaybackSpeedObjectFunction;
                public StringId BlendWeightObjectFunction;
                public AnimationIndexStruct Animation;
                
                [Flags]
                public enum FunctionOverlayDefinitionFlags : uint
                {
                    // loop without blending the first and last frames
                    StrictLooping = 1 << 0
                }
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStruct : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class OverlayGroupDefinitionBlock : TagStructure
            {
                public StringId Name;
                public List<BlendScreenItemDefinitionBlock> BlendScreens;
                public List<FunctionOverlayItemDefinitionBlock> FunctionOverlaysCcbbaa;
                
                [TagStructure(Size = 0x8)]
                public class BlendScreenItemDefinitionBlock : TagStructure
                {
                    public short BlendScreen;
                    public PoseOverlayItemDefinitionBlockFlags Flags;
                    public short NodeMask;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum PoseOverlayItemDefinitionBlockFlags : ushort
                    {
                        Disable = 1 << 0
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class FunctionOverlayItemDefinitionBlock : TagStructure
                {
                    public short FunctionOverlay;
                    public PoseOverlayItemDefinitionBlockFlags Flags;
                    
                    [Flags]
                    public enum PoseOverlayItemDefinitionBlockFlags : ushort
                    {
                        Disable = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class AnimationGaitBlock : TagStructure
            {
                public StringId Name;
                public StringId SlowGaitName;
                // animation name used for the speed variations
                public StringId IntermediateGaitName;
                public StringId FastGaitName;
                public AnimationGaitDirections MoveState;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum AnimationGaitDirections : short
                {
                    MoveFront,
                    MoveBack,
                    MoveLeft,
                    MoveRight,
                    TurnLeft,
                    TurnRight
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class AnimationGaitGroupBlock : TagStructure
            {
                public StringId Name;
                public List<AnimationGaitItemBlock> AnimationGaits;
                
                [TagStructure(Size = 0x4)]
                public class AnimationGaitItemBlock : TagStructure
                {
                    public short AnimationGait;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class AnimationIkBlock : TagStructure
            {
                public StringId Name;
                // the marker name on this object where the point of attachment is
                public StringId SourceMarker;
                public AnimationIkTargetEnum AttachTo;
                // the marker name of the attachment destination point
                public StringId DestinationMarker;
                public RealPoint3d PoleMarker;
                
                public enum AnimationIkTargetEnum : int
                {
                    Parent,
                    NyiAnyChild,
                    PrimaryWeapon,
                    SecondaryWeapon
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class AnimationIkSet : TagStructure
            {
                public StringId Name;
                public List<AnimationIkSetItem> IkPointsCcbbaa;
                
                [TagStructure(Size = 0x4)]
                public class AnimationIkSetItem : TagStructure
                {
                    public short IkPoint;
                    public AnimationIkSetItemFlags Flags;
                    
                    [Flags]
                    public enum AnimationIkSetItemFlags : ushort
                    {
                        Disable = 1 << 0
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class AnimationIkChainBlock : TagStructure
            {
                public StringId Name;
                public AnimationIkChainTypeEnumeration Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short StartNode;
                public short EffectorNode;
                // calculated during post process where rank is default ordinal for solving
                public short Rank;
                // calculated during post process where bit index represents chain index
                public short Antecedents;
                
                public enum AnimationIkChainTypeEnumeration : short
                {
                    Standard
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class GCompositetagStruct : TagStructure
            {
                public StringId Name;
                public List<CompositeAxisDefinition> Axes;
                public List<CompositeEntryDefinition> Anims;
                public List<CompositePhaseSetDefinition> Sets;
                public List<StringBlock> Strings;
                public StringId TimingSource;
                public short TiminganimIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [TagStructure(Size = 0x3C)]
                public class CompositeAxisDefinition : TagStructure
                {
                    public StringId Name;
                    public StringId AnimationSource;
                    public StringId InputFunction;
                    public Bounds<float> AnimationBounds;
                    public Bounds<float> InputBounds;
                    public Bounds<float> ClampBounds;
                    public float BlendLimit;
                    public List<CompositeDeadZoneDefinition> DeadZones;
                    public sbyte Divisions;
                    public sbyte Priority;
                    public sbyte Update;
                    public sbyte Functionindex;
                    public CompositeAxisFlags Flags;
                    
                    [Flags]
                    public enum CompositeAxisFlags : uint
                    {
                        Wrapped = 1 << 0,
                        Clamped = 1 << 1
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class CompositeDeadZoneDefinition : TagStructure
                    {
                        public Bounds<float> Bounds;
                        public float Rate;
                        public float Center;
                        public float Radius;
                        public float Amount;
                        public int Delay;
                    }
                }
                
                [TagStructure(Size = 0x18)]
                public class CompositeEntryDefinition : TagStructure
                {
                    public StringId Source;
                    public List<CompositeEntryValueDefinition> Values;
                    public int Overridden;
                    public short Animindex;
                    public sbyte Slideaxis;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x4)]
                    public class CompositeEntryValueDefinition : TagStructure
                    {
                        public float Value;
                    }
                }
                
                [TagStructure(Size = 0x84)]
                public class CompositePhaseSetDefinition : TagStructure
                {
                    public StringId Name;
                    public StringId TimingSource;
                    public List<SyncKeyBlock> SyncPoints;
                    public byte[] SyncFrames;
                    public byte[] Facets;
                    public byte[] Neighbors;
                    public byte[] Containment;
                    public byte[] ExampleGrid;
                    public Bounds<float> NormalizedBounds;
                    public sbyte Offset;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short TiminganimIndex;
                    
                    [TagStructure(Size = 0x2)]
                    public class SyncKeyBlock : TagStructure
                    {
                        public FrameEventTypeNew Key;
                        
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
                }
                
                [TagStructure(Size = 0x4)]
                public class StringBlock : TagStructure
                {
                    public StringId Name;
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class PcaanimationDataStruct : TagStructure
            {
                public List<PcagroupSettingsBlock> PcaGroups;
                [TagField(ValidTags = new [] { "pcaa" })]
                public CachedTag PcaAnimationAbcdcc;
                public int PcaAnimationCount;
                public int PcaChecksum;
                
                [TagStructure(Size = 0x8)]
                public class PcagroupSettingsBlock : TagStructure
                {
                    public StringId GroupName;
                    public int DesiredMeshCount;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class AnimationGraphContentsStruct : TagStructure
        {
            public short DefaultGaitGroupCcaabb;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<AnimationModeBlock> ModesAabbcc;
            public List<VehicleSuspensionBlock> VehicleSuspensionCcaabb;
            public List<FunctionOverlayAnimationBlock> FunctionOverlays;
            
            [TagStructure(Size = 0x30)]
            public class AnimationModeBlock : TagStructure
            {
                public StringId Label;
                public short OverlayGroup;
                public short IkSet;
                public AnimationModeFlags Flags;
                public List<WeaponClassBlockStruct> WeaponClassAabbcc;
                public List<AnimationIkBlockV1> ModeIkAabbcc;
                public List<FootTrackingDefaults> FootDefaultsAabbcc;
                
                [Flags]
                public enum AnimationModeFlags : uint
                {
                    ThisIsAStance = 1 << 0
                }
                
                [TagStructure(Size = 0x38)]
                public class WeaponClassBlockStruct : TagStructure
                {
                    public StringId Label;
                    public short OverlayGroup;
                    public short IkSet;
                    public List<WeaponTypeBlockStruct> WeaponTypeAabbcc;
                    public List<AnimationIkBlockV1> WeaponIkAabbcc;
                    public List<AnimationRangedActionBlock> RangedActions;
                    public List<AnimationSyncActionGroupBlock> SyncActionsGroups;
                    
                    [TagStructure(Size = 0x14)]
                    public class WeaponTypeBlockStruct : TagStructure
                    {
                        public StringId Label;
                        public short OverlayGroup;
                        public short IkSet;
                        public List<AnimationSetBlock> SetsAabbcc;
                        
                        [TagStructure(Size = 0x48)]
                        public class AnimationSetBlock : TagStructure
                        {
                            public StringId Label;
                            public short OverlayGroup;
                            public short IkSet;
                            public short GaitGroupAabbcc;
                            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                            public byte[] Padding;
                            public List<AnimationEntryBlock> ActionsAabbcc;
                            public List<AnimationEntryBlock> OverlayAnimations;
                            public List<DamageAnimationBlock> DeathAndDamageAabbcc;
                            public List<AnimationTransitionSourceBlockStruct> TransitionsAabbcc;
                            public List<AnimationVelocityBoundariesBlock> VelocityBoundaries;
                            
                            [TagStructure(Size = 0xC)]
                            public class AnimationEntryBlock : TagStructure
                            {
                                public StringId Label;
                                public short OverlayGroup;
                                public short IkSet;
                                public AnimationIndexStruct Animation;
                                
                                [TagStructure(Size = 0x4)]
                                public class AnimationIndexStruct : TagStructure
                                {
                                    public short GraphIndex;
                                    public short Animation;
                                }
                            }
                            
                            [TagStructure(Size = 0x10)]
                            public class DamageAnimationBlock : TagStructure
                            {
                                public StringId Label;
                                public List<DamageDirectionBlock> Directions;
                                
                                [TagStructure(Size = 0xC)]
                                public class DamageDirectionBlock : TagStructure
                                {
                                    public List<DamageRegionBlock> Regions;
                                    
                                    [TagStructure(Size = 0x4)]
                                    public class DamageRegionBlock : TagStructure
                                    {
                                        public AnimationIndexStruct Animation;
                                        
                                        [TagStructure(Size = 0x4)]
                                        public class AnimationIndexStruct : TagStructure
                                        {
                                            public short GraphIndex;
                                            public short Animation;
                                        }
                                    }
                                }
                            }
                            
                            [TagStructure(Size = 0x10)]
                            public class AnimationTransitionSourceBlockStruct : TagStructure
                            {
                                // name of the state this transition starts in
                                public StringId StateName;
                                public List<AnimationTransitionDestinationBlock> Destinations;
                                
                                [TagStructure(Size = 0xC)]
                                public class AnimationTransitionDestinationBlock : TagStructure
                                {
                                    // name of the mode this transition ends in
                                    public StringId ModeName;
                                    // name of the state this transition ends in
                                    public StringId StateName;
                                    public AnimationIndexStruct Animation;
                                    
                                    [TagStructure(Size = 0x4)]
                                    public class AnimationIndexStruct : TagStructure
                                    {
                                        public short GraphIndex;
                                        public short Animation;
                                    }
                                }
                            }
                            
                            [TagStructure(Size = 0x20)]
                            public class AnimationVelocityBoundariesBlock : TagStructure
                            {
                                [TagField(Length = 8)]
                                public AnimationVelocityBoundaries[]  VelocityBoundaryEntries;
                                
                                [TagStructure(Size = 0x4)]
                                public class AnimationVelocityBoundaries : TagStructure
                                {
                                    public float Values;
                                }
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class AnimationIkBlockV1 : TagStructure
                    {
                        // the marker name on the object being attached
                        public StringId Marker;
                        // the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                        public StringId AttachToMarker;
                    }
                    
                    [TagStructure(Size = 0x24)]
                    public class AnimationRangedActionBlock : TagStructure
                    {
                        public StringId Label;
                        public List<RangedAnimationEntryBlockStruct> Animations;
                        public List<TriangulationEntryBlock> TriangulationData;
                        public RangedActionVariableSources HorizontalSource;
                        public RangedActionVariableSources VerticalSource;
                        public FrameEventTypeNew StartKey;
                        public FrameEventTypeNew EndKey;
                        
                        public enum RangedActionVariableSources : short
                        {
                            None,
                            OffsetX,
                            OffsetY,
                            OffsetZ,
                            OffsetHorizontal,
                            NegativeOffsetX,
                            NegativeOffsetY
                        }
                        
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
                        
                        [TagStructure(Size = 0x10)]
                        public class RangedAnimationEntryBlockStruct : TagStructure
                        {
                            public short OverlayGroup;
                            public short IkSet;
                            public AnimationIndexStruct Animation;
                            // Numerical value associated with the ranged action animation (e.g. velocity for jumps)
                            public float AnimationParameter;
                            public float AnimationParameterB;
                            
                            [TagStructure(Size = 0x4)]
                            public class AnimationIndexStruct : TagStructure
                            {
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class TriangulationEntryBlock : TagStructure
                        {
                            public List<TriangulationPointBlock> Points;
                            public List<TriangulationTriangleBlock> Triangles;
                            
                            [TagStructure(Size = 0x8)]
                            public class TriangulationPointBlock : TagStructure
                            {
                                public RealVector2d Point;
                            }
                            
                            [TagStructure(Size = 0x8)]
                            public class TriangulationTriangleBlock : TagStructure
                            {
                                public byte Vertex1;
                                public byte Vertex2;
                                public byte Vertex3;
                                public byte Link12;
                                public byte Link23;
                                public byte Link31;
                                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                                public byte[] Padding;
                            }
                        }
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class AnimationSyncActionGroupBlock : TagStructure
                    {
                        public StringId Name;
                        public List<AnimationSyncActionBlock> SyncActions;
                        
                        [TagStructure(Size = 0x1C)]
                        public class AnimationSyncActionBlock : TagStructure
                        {
                            public StringId Name;
                            public List<AnimationSyncActionSameTypeParticipant> SameTypeParticipants;
                            public List<AnimationSyncActionOtherTypeParticipant> OtherParticipants;
                            
                            [TagStructure(Size = 0x3C)]
                            public class AnimationSyncActionSameTypeParticipant : TagStructure
                            {
                                public AnimationSyncActionFlags Flags;
                                public AnimationIndexStruct Animation;
                                public RealPoint3d StartOffset;
                                public RealVector3d StartFacing;
                                public RealPoint3d EndOffset;
                                public float TimeUntilHurt;
                                public RealPoint3d ApexOffset;
                                
                                [Flags]
                                public enum AnimationSyncActionFlags : uint
                                {
                                    Initiator = 1 << 0,
                                    CriticalParticipant = 1 << 1,
                                    Disabled = 1 << 2,
                                    Airborne = 1 << 3
                                }
                                
                                [TagStructure(Size = 0x4)]
                                public class AnimationIndexStruct : TagStructure
                                {
                                    public short GraphIndex;
                                    public short Animation;
                                }
                            }
                            
                            [TagStructure(Size = 0x14)]
                            public class AnimationSyncActionOtherTypeParticipant : TagStructure
                            {
                                public AnimationSyncActionOtherTypeFlags Flags;
                                [TagField(ValidTags = new [] { "unit","scen" })]
                                public CachedTag ObjectType;
                                
                                [Flags]
                                public enum AnimationSyncActionOtherTypeFlags : uint
                                {
                                    SupportsAnyType = 1 << 0,
                                    Disabled = 1 << 1
                                }
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class AnimationIkBlockV1 : TagStructure
                {
                    // the marker name on the object being attached
                    public StringId Marker;
                    // the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                    public StringId AttachToMarker;
                }
                
                [TagStructure(Size = 0x4)]
                public class FootTrackingDefaults : TagStructure
                {
                    public short Foot;
                    public FootTrackingDefaultValues DefaultState;
                    
                    public enum FootTrackingDefaultValues : short
                    {
                        Off,
                        On
                    }
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class VehicleSuspensionBlock : TagStructure
            {
                public StringId Label;
                public AnimationIndexStruct Animation;
                public StringId FunctionName;
                // this marker should be parented to the vehicle root node
                public StringId MarkerName;
                // this marker should be parented to the wheel node
                public StringId ContactMarkerName;
                // distance along the vehicle's up direction to move the wheel from the marker location
                public float MassPointOffset;
                public float FullExtensionGroundDepth;
                public float FullCompressionGroundDepth;
                public StringId RegionName;
                public float DestroyedMassPointOffset;
                public float DestroyedFullExtensionGroundDepth;
                public float DestroyedFullCompressionGroundDepth;
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStruct : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class FunctionOverlayAnimationBlock : TagStructure
            {
                public StringId Label;
                public AnimationIndexStruct Animation;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public FunctionOverlayAnimationMode FunctionControls;
                public StringId Function;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                public enum FunctionOverlayAnimationMode : short
                {
                    Frame,
                    Scale
                }
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStruct : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
        }
        
        [TagStructure(Size = 0x78)]
        public class ModelAnimationRuntimeDataStruct : TagStructure
        {
            public List<InheritedAnimationBlock> InheritenceList;
            public List<InheritedAnimationBlock> NewInheritanceList;
            public List<WeaponClassLookupBlock> WeaponListBbaaaa;
            [TagField(Length = 8)]
            public GNodeFlagStorageArray[]  LeftArmBitVector;
            [TagField(Length = 8)]
            public GNodeFlagStorageArray[]  RightArmBitVector;
            public byte[] AnimationplayCounts;
            
            [TagStructure(Size = 0x30)]
            public class InheritedAnimationBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "jmad" })]
                public CachedTag InheritedGraph;
                public List<InheritedAnimationNodeMapBlock> NodeMap;
                public List<InheritedAnimationNodeMapFlagBlock> NodeMapFlags;
                public int InheritanceFlags;
                public float UniformTranslationScale;
                
                [TagStructure(Size = 0x2)]
                public class InheritedAnimationNodeMapBlock : TagStructure
                {
                    public short LocalNode;
                }
                
                [TagStructure(Size = 0x4)]
                public class InheritedAnimationNodeMapFlagBlock : TagStructure
                {
                    public int LocalNodeFlags;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class WeaponClassLookupBlock : TagStructure
            {
                public StringId WeaponName;
                public StringId WeaponClass;
            }
            
            [TagStructure(Size = 0x4)]
            public class GNodeFlagStorageArray : TagStructure
            {
                public int FlagData;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class AdditionalNodeDataBlock : TagStructure
        {
            public StringId NodeName;
            public RealQuaternion DefaultRotation;
            public RealPoint3d DefaultTranslation;
            public float DefaultScale;
            public RealPoint3d MinBounds;
            public RealPoint3d MaxBounds;
        }
        
        [TagStructure(Size = 0xC)]
        public class ModelAnimationTagResourceGroup : TagStructure
        {
            public int ReferenceCount;
            public TagResourceReference TagResource;
        }
        
        [TagStructure(Size = 0x24)]
        public class AnimationCodecDataStruct : TagStructure
        {
            public SharedStaticDataCodecGraphDataStruct SharedStaticCodec;
            
            [TagStructure(Size = 0x24)]
            public class SharedStaticDataCodecGraphDataStruct : TagStructure
            {
                public List<SharedStaticDataCodecRotationBlock> Rotations;
                public List<SharedStaticDataCodecTranslationBlock> Translations;
                public List<SharedStaticDataCodecScaleBlock> Scale;
                
                [TagStructure(Size = 0x8)]
                public class SharedStaticDataCodecRotationBlock : TagStructure
                {
                    public short I;
                    public short J;
                    public short K;
                    public short W;
                }
                
                [TagStructure(Size = 0xC)]
                public class SharedStaticDataCodecTranslationBlock : TagStructure
                {
                    public float X;
                    public float Y;
                    public float Z;
                }
                
                [TagStructure(Size = 0x4)]
                public class SharedStaticDataCodecScaleBlock : TagStructure
                {
                    public float Scale;
                }
            }
        }
    }
}
