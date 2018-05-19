using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Damage;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x15C, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x188, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1A0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1B4, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1B8, MinVersion = CacheVersion.HaloOnline498295)]
    public partial class Model
    {
        public CachedTagInstance RenderModel;

        public CachedTagInstance CollisionModel;

        public CachedTagInstance Animation;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTagInstance Physics;

        public CachedTagInstance PhysicsModel;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float DisappearDistance;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float BeginFadeDistance;

        [TagField(Padding = true, Length = 4, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused1;

        public float ReduceToL1SuperLow;
        public float ReduceToL2Low;
        public float ReduceToL3Medium;
        public float ReduceToL4High;
        public float ReduceToL5SuperHigh;

        [TagField(Padding = true, Length = 4, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused2;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public ShadowFadeDistanceValue ShadowFadeDistance;

        [TagField(Padding = true, Length = 2, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused3;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance LodModel;

        public List<Variant> Variants;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<InstanceGroup> InstanceGroups;

        public List<Material> Materials;

        public List<NewDamageInfoBlock> NewDamageInfo;

        public List<Target> Targets;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public List<UnknownTarget> UnknownTargets;

        public List<CollisionRegion> CollisionRegions;

        public List<Node> Nodes;

        [TagField(Padding = true, Length = 4)]
        public byte[] Unused4;

        public List<ModelObjectDatum> ModelObjectData;

        public CachedTagInstance PrimaryDialogue;

        public CachedTagInstance SecondaryDialogue;

        public FlagsValue Flags;

        public StringId DefaultDialogueEffect;

        public RenderOnlyNodeFlags1Value RenderOnlyNodeFlags1;
        public RenderOnlyNodeFlags2Value RenderOnlyNodeFlags2;
        public RenderOnlyNodeFlags3Value RenderOnlyNodeFlags3;
        public RenderOnlyNodeFlags4Value RenderOnlyNodeFlags4;
        public RenderOnlyNodeFlags5Value RenderOnlyNodeFlags5;
        public RenderOnlyNodeFlags6Value RenderOnlyNodeFlags6;
        public RenderOnlyNodeFlags7Value RenderOnlyNodeFlags7;
        public RenderOnlyNodeFlags8Value RenderOnlyNodeFlags8;

        public RenderOnlySectionFlags1Value RenderOnlySectionFlags1;
        public RenderOnlySectionFlags2Value RenderOnlySectionFlags2;
        public RenderOnlySectionFlags3Value RenderOnlySectionFlags3;
        public RenderOnlySectionFlags4Value RenderOnlySectionFlags4;
        public RenderOnlySectionFlags5Value RenderOnlySectionFlags5;
        public RenderOnlySectionFlags6Value RenderOnlySectionFlags6;
        public RenderOnlySectionFlags7Value RenderOnlySectionFlags7;
        public RenderOnlySectionFlags8Value RenderOnlySectionFlags8;

        public RuntimeFlagsValue RuntimeFlags;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public uint Unknown3; // TODO: Version number

        public List<ScenarioLoadParametersBlock> ScenarioLoadParameters;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public CachedTagInstance HologramShader;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public StringId HologramControlFunction;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short Unknown4;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public short Unknown5;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<UnknownBlock2> Unknown6;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<UnknownBlock3> Unknown7;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<UnknownBlock4> Unknown8;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance ShieldImpactThirdPerson;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public CachedTagInstance ShieldImpactFirstPerson;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance OvershieldThirdPerson;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance OvershieldFirstPerson;

        public enum ShadowFadeDistanceValue : short
        {
            FadeAtSuperHighDetailLevel,
            FadeAtHighDetailLevel,
            FadeAtMediumDetailLevel,
            FadeAtLowDetailLevel,
            FadeAtSuperLowDetailLevel,
            FadeNever
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x50, MinVersion = CacheVersion.Halo3ODST)]
        public class Variant
        {
            public StringId Name;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTagInstance VariantDialogue;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public StringId DefaultDialogEffect;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown2;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown3;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public sbyte Unknown4;

            [TagField(MinVersion = CacheVersion.HaloOnline700123)]
            public StringId SkinName;

            [TagField(Length = 16)]
            public sbyte[] ModelRegionIndices = new sbyte[16];

            public List<Region> Regions;
            public List<Object> Objects;
            public int InstanceGroupIndex;
            public uint Unknown6;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public StringId DialogueSoundEffect;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTagInstance Dialogue;

            [TagField(MaxVersion = CacheVersion.HaloOnline571627)]
            public uint Unknown7;

            [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail)]
            public class Region
            {
                public StringId Name;

                public sbyte RenderModelRegionIndex;

                public sbyte Unknown;

                [TagField(MaxVersion = CacheVersion.Halo2Vista)]
                public short ParentVariant;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public sbyte Unknown2;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public sbyte Unknown3;

                public List<Permutation> Permutations;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public SortOrderValue SortOrder;

                [TagStructure(Size = 0x24)]
                public class Permutation
                {
                    public StringId Name;
                    public sbyte RenderModelPermutationIndex;

                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public sbyte Unknown;

                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public sbyte Unknown2;

                    public FlagsValue Flags;

                    [TagField(Padding = true, Length = 2, MaxVersion = CacheVersion.Halo2Vista)]
                    public byte[] Unused;

                    public float Probability;
                    public List<State> States;

                    [TagField(Length = 12)]
                    public sbyte[] RuntimeStatePermutationIndices = new sbyte[12];

                    [Flags]
                    public enum FlagsValue : byte
                    {
                        None,
                        CopyStatesToAllPermutations = 1 << 0
                    }

                    [TagStructure(Size = 0x20)]
                    public class State
                    {
                        public StringId Name;
                        public sbyte Unknown;
                        public PropertyFlagsValue PropertyFlags;
                        public StateValue State2;
                        public CachedTagInstance LoopingEffect;
                        public StringId LoopingEffectMarkerName;
                        public float InitialProbability;

                        [Flags]
                        public enum PropertyFlagsValue : byte
                        {
                            None,
                            Blurred,
                            HellaBlurred,
                            Shielded = 4,
                            Bit3 = 8,
                            Bit4 = 16,
                            Bit5 = 32,
                            Bit6 = 64,
                            Bit7 = 128,
                        }

                        public enum StateValue : short
                        {
                            Default,
                            MinorDamage,
                            MediumDamage,
                            MajorDamage,
                            Destroyed,
                        }
                    }
                }

                public enum SortOrderValue : int
                {
                    NoSorting,
                    _5Closest,
                    _4,
                    _3,
                    _2,
                    _1,
                    _0SameAsModel,
                    _1_2,
                    _2_2,
                    _3_2,
                    _4_2,
                    _5Farthest,
                }
            }

            [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Retail)]
            public class Object
            {
                public StringId ParentMarker;
                public StringId ChildMarker;
                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public StringId ChildVariant;
                public CachedTagInstance ChildObject;
            }
        }

        [TagStructure(Size = 0x4)]
        public class UnknownBlock
        {
            public uint Unknown;
        }

        [TagStructure(Size = 0x18)]
        public class InstanceGroup
        {
            public StringId Name;
            public int Unknown;
            public List<InstanceMember> InstanceMembers;
            public float Probability;

            [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST)]
            public class InstanceMember
            {
                public int Unknown;
                public StringId InstanceName;
                public float Probability;
                public InstanceFlags1Value InstanceFlags1;
                public InstanceFlags2Value InstanceFlags2;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown4;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown5;

                public enum InstanceFlags1Value : int
                {
                    None,
                    Instance0,
                    Instance1,
                    Instance2 = 4,
                    Instance3 = 8,
                    Instance4 = 16,
                    Instance5 = 32,
                    Instance6 = 64,
                    Instance7 = 128,
                    Instance8 = 256,
                    Instance9 = 512,
                    Instance10 = 1024,
                    Instance11 = 2048,
                    Instance12 = 4096,
                    Instance13 = 8192,
                    Instance14 = 16384,
                    Instance15 = 32768,
                    Instance16 = 65536,
                    Instance17 = 131072,
                    Instance18 = 262144,
                    Instance19 = 524288,
                    Instance20 = 1048576,
                    Instance21 = 2097152,
                    Instance22 = 4194304,
                    Instance23 = 8388608,
                    Instance24 = 16777216,
                    Instance25 = 33554432,
                    Instance26 = 67108864,
                    Instance27 = 134217728,
                    Instance28 = 268435456,
                    Instance29 = 536870912,
                    Instance30 = 1073741824,
                    Instance31 = -2147483648,
                }

                public enum InstanceFlags2Value : int
                {
                    None,
                    Instance32,
                    Instance33,
                    Instance34 = 4,
                    Instance35 = 8,
                    Instance36 = 16,
                    Instance37 = 32,
                    Instance38 = 64,
                    Instance39 = 128,
                    Instance40 = 256,
                    Instance41 = 512,
                    Instance42 = 1024,
                    Instance43 = 2048,
                    Instance44 = 4096,
                    Instance45 = 8192,
                    Instance46 = 16384,
                    Instance47 = 32768,
                    Instance48 = 65536,
                    Instance49 = 131072,
                    Instance50 = 262144,
                    Instance51 = 524288,
                    Instance52 = 1048576,
                    Instance53 = 2097152,
                    Instance54 = 4194304,
                    Instance55 = 8388608,
                    Instance56 = 16777216,
                    Instance57 = 33554432,
                    Instance58 = 67108864,
                    Instance59 = 134217728,
                    Instance60 = 268435456,
                    Instance61 = 536870912,
                    Instance62 = 1073741824,
                    Instance63 = -2147483648,
                }
            }
        }

        [TagStructure(Size = 0x14)]
        public class Material
        {
            public StringId Name;
            public MaterialTypeValue MaterialType;
            public short DamageSectionIndex;
            public short Unknown2;
            public short Unknown3;
            public StringId MaterialName;
            public short GlobalMaterialIndex;
            public short Unknown4;

            public enum MaterialTypeValue : short
            {
                Dirt = 0,
                Sand = 1,
                Stone = 2,
                Snow = 3,
                Wood = 4,
                Metalhollow = 5,
                Metalthin = 6,
                Metalthick = 7,
                Rubber = 8,
                Glass = 9,
                ForceField = 10,
                Grunt = 11,
                HunterArmor = 12,
                HunterSkin = 13,
                Elite = 14,
                Jackal = 15,
                JackalEnergyShield = 16,
                EngineerSkin = 17,
                EngineerForceField = 18,
                FloodCombatForm = 19,
                FloodCarrierForm = 20,
                CyborgArmor = 21,
                CyborgEnergyShield = 22,
                HumanArmor = 23,
                HumanSkin = 24,
                Sentinel = 25,
                Monitor = 26,
                Plastic = 27,
                Water = 28,
                Leaves = 29,
                EliteEnergyShield = 30,
                Ice = 31,
                HunterShield = 32,
            }
        }

        [TagStructure(Size = 0x140, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x100, MinVersion = CacheVersion.Halo3Retail)]
        public class NewDamageInfoBlock
        {
            public FlagsValue Flags;
            public StringId GlobalIndirectMaterialName;
            public short IndirectDamageSection;
            public short Unknown;
            public uint Unknown2;
            public DamageReportingType CollisionDamageReportingType;
            public DamageReportingType ResponseDamageReportingType;
            public short Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public float MaxVitality;
            public float MinStunDamage;
            public float StunTime;
            public float RechargeTime;
            public float RechargeFraction;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
            public uint Unknown17;
            public uint Unknown18;
            public uint Unknown19;
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;
            public uint Unknown23;
            public uint Unknown24;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTagInstance ShieldDamagedFirstPersonShader;
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTagInstance ShieldDamagedShader;
            public float MaxShieldVitality;
            public StringId GlobalShieldMaterialName;
            public float MinStunDamage2;
            public float StunTime2;
            public float ShieldRechargeTime;
            public float ShieldDamagedThreshold;
            public CachedTagInstance ShieldDamagedEffect;
            public CachedTagInstance ShieldDepletedEffect;
            public CachedTagInstance ShieldRechargingEffect;
            public List<DamageSection> DamageSections;
            public List<Node> Nodes;
            public short GlobalShieldMaterialIndex;
            public short GlobalIndirectMaterialIndex;
            public uint Unknown25;
            public uint Unknown26;
            public List<DamageSeat> DamageSeats;
            public List<DamageConstraint> DamageConstraints;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTagInstance OvershieldFirstPersonShader;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTagInstance OvershieldShader;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                TakesShieldDamageForChildren = 1 << 0,
                TakesBodyDamageForChildren = 1 << 1,
                AlwaysShieldsFriendlyDamage = 1 << 2,
                PassesAreaDamageToChildren = 1 << 3,
                ParentNeverTakesBodyDamageForChildren = 1 << 4,
                OnlyDamagedByExplosives = 1 << 5,
                ParentNeverTakesShieldDamageForChildren = 1 << 6,
                CannotDieFromDamage = 1 << 7,
                PassesAttachedDamageToRiders = 1 << 8,
                ShieldDepletionIsPermanent = 1 << 9,
                ShieldDepletionForceHardPing = 1 << 10,
                AiDoNotDamageWithoutPlayer = 1 << 11,
                HealthRegrowsWhileDead = 1 << 12,
                ShieldRechargePlaysOnlyWhenEmpty = 1 << 13,
                IgnoreForceMinimumTransfer = 1 << 14,
                OrphanFromPostprocessAutogen = 1 << 15,
                OnlyDamagedByBoardingDamage = 1 << 16
            }

            [TagStructure(Size = 0x44)]
            public class DamageSection
            {
                public StringId Name;
                public FlagsValue Flags;
                public float VitalityPercentage;
                public List<InstantResponse> InstantResponses;
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public float StunTime;
                public float RechargeTime;
                public uint Unknown7;
                public StringId ResurrectionRegionName;
                public short RessurectionRegionRuntimeIndex;
                public short Unknown8;

                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    AbsorbsBodyDamage = 1 << 0,
                    TakesFullDamageWhenObjectDies = 1 << 1,
                    CannotDieWithRiders = 1 << 2,
                    TakesFullDamageWhenObjectDestroyed = 1 << 3,
                    RestoredOnRessurection = 1 << 4,
                    Unused5 = 1 << 5,
                    Unused6 = 1 << 6,
                    Headshotable = 1 << 7,
                    IgnoresShields = 1 << 8,
                    TakesFullDamageWhenShieldDepleted = 1 << 9,
                    Networked = 1 << 10,
                    AllowDamageResponseOverflow = 1 << 11
                }

                [TagStructure(Size = 0x68, MaxVersion = CacheVersion.Halo2Vista)]
                [TagStructure(Size = 0x88, MinVersion = CacheVersion.Halo3Retail)]
                public class InstantResponse
                {
                    public ResponseTypeValue ResponseType;
                    public ConstraintDamageTypeValue ConstraintDamageType;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public StringId Trigger;
                    public FlagsValue Flags;
                    public float DamageThreshold;
                    public CachedTagInstance PrimaryTransitionEffect;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public CachedTagInstance SecondaryTransitionEffect;
                    public CachedTagInstance TransitionDamageEffect;
                    public StringId Region;
                    public NewStateValue NewState;
                    public short RuntimeRegionIndex;
                    public StringId SecondaryRegion;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public NewStateValue SecondaryNewState;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public short SecondaryRuntimeRegionIndex;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public short Unknown;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public UnknownSpecialDamageValue UnknownSpecialDamage;
                    [TagField(MinVersion = CacheVersion.Halo3Retail)]
                    public StringId SpecialDamageCase;
                    public StringId EffectMarkerName;
                    public StringId DamageEffectMarkerName;
                    public float ResponseDelay;
                    public CachedTagInstance DelayEffect;
                    public StringId DelayEffectMarkerName;
                    [TagField(MaxVersion = CacheVersion.Halo2Vista)]
                    public StringId ConstraintGroupName;
                    public StringId EjectingSeatLabel;
                    public float SkipFraction;
                    public StringId DestroyedChildObjectMarkerName;
                    public float TotalDamageThreshold;

                    public enum ResponseTypeValue : short
                    {
                        RecievesAllDamage,
                        RecievesAreaEffectDamage,
                        RecievesLocalDamage
                    }

                    public enum ConstraintDamageTypeValue : short
                    {
                        None,
                        DestroyOneOfGroup,
                        DestroyEntireGroup,
                        LoosenOneOfGroup,
                        LoosenEntireGroup
                    }

                    [Flags]
                    public enum FlagsValue : int
                    {
                        None,

                        /// <summary>
                        /// When the response fires the object dies regardless of its current health.
                        /// </summary>
                        KillsObject = 1 << 0,

                        /// <summary>
                        /// From halo 1 - disallows melee for a unit.
                        /// </summary>
                        InhibitsMeleeAttack = 1 << 1,

                        /// <summary>
                        /// From halo 1 - disallows weapon fire for a unit.
                        /// </summary>
                        InhibitsWeaponAttack = 1 << 2,

                        /// <summary>
                        /// From halo 1 - disallows walking for a unit.
                        /// </summary>
                        InhibitsWalking = 1 << 3,

                        /// <summary>
                        /// From halo 1 - makes the unit drop its current weapon.
                        /// </summary>
                        ForcesDropWeapon = 1 << 4,

                        KillsWeaponPrimaryTrigger = 1 << 5,
                        KillsWeaponSecondaryTrigger = 1 << 6,

                        /// <summary>
                        /// When the response fires the object is destroyed.
                        /// </summary>
                        DestroysObject = 1 << 7,

                        /// <summary>
                        /// Destroys the primary trigger on the unit's current weapon.
                        /// </summary>
                        DamagesWeaponPrimaryTrigger = 1 << 8,

                        /// <summary>
                        /// Destroys the secondary trigger on the unit's current weapon.
                        /// </summary>
                        DamagesWeaponSecondaryTrigger = 1 << 9,

                        LightDamageLeftTurn = 1 << 10,
                        MajorDamageLeftTurn = 1 << 11,
                        LightDamageRightTurn = 1 << 12,
                        MajorDamageRightTurn = 1 << 13,
                        LightDamageEngine = 1 << 14,
                        MajorDamageEngine = 1 << 15,
                        KillsObjectNoPlayerSolo = 1 << 16,
                        CausesDetonation = 1 << 17,
                        FiresOnCreation = 1 << 18,
                        KillsVariantObjects = 1 << 19,
                        ForceUnattachedEffects = 1 << 20,
                        FiresUnderThreshold = 1 << 21,
                        TriggersSpecialDeath = 1 << 22,
                        OnlyOnSpecialDeath = 1 << 23,
                        OnlyNotOnSpecialDeath = 1 << 24,
                        BucklesGiants = 1 << 25,
                        CausesSpDetonation = 1 << 26,
                        SkipSoundsOnGenericEffect = 1 << 27,
                        KillsGiants = 1 << 28,
                        SkipSoundsOnSpecialDeath = 1 << 29,
                        CauseHeadDismemberment = 1 << 30,
                        CauseLeftLegDismemberment = 1 << 31,
                    }

                    public enum NewStateValue : short
                    {
                        Default,
                        MinorDamage,
                        MediumDamage,
                        MajorDamage,
                        Destroyed,
                    }
                    
                    public enum UnknownSpecialDamageValue : short
                    {
                        None,
                        Primary,
                        Secondary,
                        Tertiary
                    }
                }
            }

            [TagStructure(Size = 0x10)]
            public class Node
            {
                public short Unknown;
                public short Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
            }

            [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Retail)]
            public class DamageSeat
            {
                public StringId SeatLabel;
                public float DirectDamageScale;
                public float DamageTransferFallOffRadius;
                public float MaximumTransferDamageScale;
                public float MinimumTransferDamageScale;

                [TagField(MinVersion = CacheVersion.Halo3Retail)]
                public List<RegionSpecificDamageBlock> RegionSpecificDamage;

                [TagStructure(Size = 0x2C)]
                public class RegionSpecificDamageBlock
                {
                    public StringId DamageRegionName;
                    public short RuntimeDamageRegionIndex;

                    [TagField(Padding = true, Length = 2)]
                    public byte Unused;

                    public float DirectDamageScaleMinor;
                    public float MaxTransferScaleMinor;
                    public float MinTransferScaleMinor;

                    public float DirectDamageScaleMedium;
                    public float MaxTransferScaleMedium;
                    public float MinTransferScaleMedium;

                    public float DirectDamageScaleMajor;
                    public float MaxTransferScaleMajor;
                    public float MinTransferScaleMajor;
                }
            }

            [TagStructure(Size = 0x14)]
            public class DamageConstraint
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                public TypeValue Type;
                public short Index;

                public enum TypeValue : short
                {
                    Hinge,
                    LimitedHinge,
                    Ragdoll,
                    StiffSpring,
                    BallAndSocket,
                    Prismatic
                }
            }
        }

        [TagStructure(Size = 0x20, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
        public class Target
        {
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown;

            public StringId MarkerName;
            public float Size;
            public Angle ConeAngle;
            public short DamageSection;
            public short Variant;
            public float TargetingRelevance;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public uint Unknown2;
            public FlagsValue Flags;
            public float LockOnDistance;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public StringId TargetFilter;

            [Flags]
            public enum FlagsValue : int
            {
                None,
                LockedByHumanTracking,
                LockedByPlasmaTracking,
                Headshot = 4,
                Bit3 = 8,
                Vulnerable = 16,
                Bit5 = 32,
                AlwaysLockedByPlasmaTracking = 64,
                Bit7 = 128,
                Bit8 = 256,
                Bit9 = 512,
                Bit10 = 1024,
                Bit11 = 2048,
                Bit12 = 4096,
                Bit13 = 8192,
                Bit14 = 16384,
                Bit15 = 32768,
                Bit16 = 65536,
                Bit17 = 131072,
                Bit18 = 262144,
                Bit19 = 524288,
                Bit20 = 1048576,
                Bit21 = 2097152,
                Bit22 = 4194304,
                Bit23 = 8388608,
                Bit24 = 16777216,
                Bit25 = 33554432,
                Bit26 = 67108864,
                Bit27 = 134217728,
                Bit28 = 268435456,
                Bit29 = 536870912,
                Bit30 = 1073741824,
                Bit31 = -2147483648,
            }
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
        public class CollisionRegion
        {
            public StringId Name;
            public sbyte CollisionRegionIndex;
            public sbyte PhysicsRegionIndex;
            public sbyte Unknown;
            public sbyte Unknown2;
            public List<Permutation> Permutations;

            [TagStructure(Size = 0x8)]
            public class Permutation
            {
                public StringId Name;
                public FlagsValue Flags;
                public sbyte CollisionPermutationIndex;
                public sbyte PhysicsPermutationIndex;
                public sbyte Unknown;

                [Flags]
                public enum FlagsValue : byte
                {
                    None,
                    CannotBeChosenRandomly,
                    Bit1,
                    Bit2 = 4,
                    Bit3 = 8,
                    Bit4 = 16,
                    Bit5 = 32,
                    Bit6 = 64,
                    Bit7 = 128,
                }
            }
        }

        [TagStructure(Size = 0x5C)]
        public class Node
        {
            public StringId Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            public short ImportNodeIndex;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float DefaultScale;
            public RealMatrix4x3 Inverse;
        }

        [TagStructure(Size = 0x14)]
        public class ModelObjectDatum
        {
            public TypeValue Type;
            public short Unknown;
            public RealPoint3d Offset;
            public float Radius;

            public enum TypeValue : short
            {
                NotSet,
                UserDefined,
                AutoGenerated
            }
        }

        public enum FlagsValue : int
        {
            None,
            ActiveCamoAlwaysOn,
            ActiveCamoAlwaysMerge,
            ActiveCamoNeverMerge = 4,
            Bit3 = 8,
            Bit4 = 16,
            Bit5 = 32,
            Bit6 = 64,
            Bit7 = 128,
            Bit8 = 256,
            Bit9 = 512,
            Bit10 = 1024,
            Bit11 = 2048,
            Bit12 = 4096,
            Bit13 = 8192,
            Bit14 = 16384,
            Bit15 = 32768,
            Bit16 = 65536,
            Bit17 = 131072,
            Bit18 = 262144,
            Bit19 = 524288,
            Bit20 = 1048576,
            Bit21 = 2097152,
            Bit22 = 4194304,
            Bit23 = 8388608,
            Bit24 = 16777216,
            Bit25 = 33554432,
            Bit26 = 67108864,
            Bit27 = 134217728,
            Bit28 = 268435456,
            Bit29 = 536870912,
            Bit30 = 1073741824,
            Bit31 = -2147483648,
        }

        public enum RenderOnlyNodeFlags1Value : int
        {
            None,
            Node0,
            Node1,
            Node2 = 4,
            Node3 = 8,
            Node4 = 16,
            Node5 = 32,
            Node6 = 64,
            Node7 = 128,
            Node8 = 256,
            Node9 = 512,
            Node10 = 1024,
            Node11 = 2048,
            Node12 = 4096,
            Node13 = 8192,
            Node14 = 16384,
            Node15 = 32768,
            Node16 = 65536,
            Node17 = 131072,
            Node18 = 262144,
            Node19 = 524288,
            Node20 = 1048576,
            Node21 = 2097152,
            Node22 = 4194304,
            Node23 = 8388608,
            Node24 = 16777216,
            Node25 = 33554432,
            Node26 = 67108864,
            Node27 = 134217728,
            Node28 = 268435456,
            Node29 = 536870912,
            Node30 = 1073741824,
            Node31 = -2147483648,
        }

        public enum RenderOnlyNodeFlags2Value : int
        {
            None,
            Node32,
            Node33,
            Node34 = 4,
            Node35 = 8,
            Node36 = 16,
            Node37 = 32,
            Node38 = 64,
            Node39 = 128,
            Node40 = 256,
            Node41 = 512,
            Node42 = 1024,
            Node43 = 2048,
            Node44 = 4096,
            Node45 = 8192,
            Node46 = 16384,
            Node47 = 32768,
            Node48 = 65536,
            Node49 = 131072,
            Node50 = 262144,
            Node51 = 524288,
            Node52 = 1048576,
            Node53 = 2097152,
            Node54 = 4194304,
            Node55 = 8388608,
            Node56 = 16777216,
            Node57 = 33554432,
            Node58 = 67108864,
            Node59 = 134217728,
            Node60 = 268435456,
            Node61 = 536870912,
            Node62 = 1073741824,
            Node63 = -2147483648,
        }

        public enum RenderOnlyNodeFlags3Value : int
        {
            None,
            Node64,
            Node65,
            Node66 = 4,
            Node67 = 8,
            Node68 = 16,
            Node69 = 32,
            Node70 = 64,
            Node71 = 128,
            Node72 = 256,
            Node73 = 512,
            Node74 = 1024,
            Node75 = 2048,
            Node76 = 4096,
            Node77 = 8192,
            Node78 = 16384,
            Node79 = 32768,
            Node80 = 65536,
            Node81 = 131072,
            Node82 = 262144,
            Node83 = 524288,
            Node84 = 1048576,
            Node85 = 2097152,
            Node86 = 4194304,
            Node87 = 8388608,
            Node88 = 16777216,
            Node89 = 33554432,
            Node90 = 67108864,
            Node91 = 134217728,
            Node92 = 268435456,
            Node93 = 536870912,
            Node94 = 1073741824,
            Node95 = -2147483648,
        }

        public enum RenderOnlyNodeFlags4Value : int
        {
            None,
            Node96,
            Node97,
            Node98 = 4,
            Node99 = 8,
            Node100 = 16,
            Node101 = 32,
            Node102 = 64,
            Node103 = 128,
            Node104 = 256,
            Node105 = 512,
            Node106 = 1024,
            Node107 = 2048,
            Node108 = 4096,
            Node109 = 8192,
            Node110 = 16384,
            Node111 = 32768,
            Node112 = 65536,
            Node113 = 131072,
            Node114 = 262144,
            Node115 = 524288,
            Node116 = 1048576,
            Node117 = 2097152,
            Node118 = 4194304,
            Node119 = 8388608,
            Node120 = 16777216,
            Node121 = 33554432,
            Node122 = 67108864,
            Node123 = 134217728,
            Node124 = 268435456,
            Node125 = 536870912,
            Node126 = 1073741824,
            Node127 = -2147483648,
        }

        public enum RenderOnlyNodeFlags5Value : int
        {
            None,
            Node128,
            Node129,
            Node130 = 4,
            Node131 = 8,
            Node132 = 16,
            Node133 = 32,
            Node134 = 64,
            Node135 = 128,
            Node136 = 256,
            Node137 = 512,
            Node138 = 1024,
            Node139 = 2048,
            Node140 = 4096,
            Node141 = 8192,
            Node142 = 16384,
            Node143 = 32768,
            Node144 = 65536,
            Node145 = 131072,
            Node146 = 262144,
            Node147 = 524288,
            Node148 = 1048576,
            Node149 = 2097152,
            Node150 = 4194304,
            Node151 = 8388608,
            Node152 = 16777216,
            Node153 = 33554432,
            Node154 = 67108864,
            Node155 = 134217728,
            Node156 = 268435456,
            Node157 = 536870912,
            Node158 = 1073741824,
            Node159 = -2147483648,
        }

        public enum RenderOnlyNodeFlags6Value : int
        {
            None,
            Node160,
            Node161,
            Node162 = 4,
            Node163 = 8,
            Node164 = 16,
            Node165 = 32,
            Node166 = 64,
            Node167 = 128,
            Node168 = 256,
            Node169 = 512,
            Node170 = 1024,
            Node171 = 2048,
            Node172 = 4096,
            Node173 = 8192,
            Node174 = 16384,
            Node175 = 32768,
            Node176 = 65536,
            Node177 = 131072,
            Node178 = 262144,
            Node179 = 524288,
            Node180 = 1048576,
            Node181 = 2097152,
            Node182 = 4194304,
            Node183 = 8388608,
            Node184 = 16777216,
            Node185 = 33554432,
            Node186 = 67108864,
            Node187 = 134217728,
            Node188 = 268435456,
            Node189 = 536870912,
            Node190 = 1073741824,
            Node191 = -2147483648,
        }

        public enum RenderOnlyNodeFlags7Value : int
        {
            None,
            Node192,
            Node193,
            Node194 = 4,
            Node195 = 8,
            Node196 = 16,
            Node197 = 32,
            Node198 = 64,
            Node199 = 128,
            Node200 = 256,
            Node201 = 512,
            Node202 = 1024,
            Node203 = 2048,
            Node204 = 4096,
            Node205 = 8192,
            Node206 = 16384,
            Node207 = 32768,
            Node208 = 65536,
            Node209 = 131072,
            Node210 = 262144,
            Node211 = 524288,
            Node212 = 1048576,
            Node213 = 2097152,
            Node214 = 4194304,
            Node215 = 8388608,
            Node216 = 16777216,
            Node217 = 33554432,
            Node218 = 67108864,
            Node219 = 134217728,
            Node220 = 268435456,
            Node221 = 536870912,
            Node222 = 1073741824,
            Node223 = -2147483648,
        }

        public enum RenderOnlyNodeFlags8Value : int
        {
            None,
            Node224,
            Node225,
            Node226 = 4,
            Node227 = 8,
            Node228 = 16,
            Node229 = 32,
            Node230 = 64,
            Node231 = 128,
            Node232 = 256,
            Node233 = 512,
            Node234 = 1024,
            Node235 = 2048,
            Node236 = 4096,
            Node237 = 8192,
            Node238 = 16384,
            Node239 = 32768,
            Node240 = 65536,
            Node241 = 131072,
            Node242 = 262144,
            Node243 = 524288,
            Node244 = 1048576,
            Node245 = 2097152,
            Node246 = 4194304,
            Node247 = 8388608,
            Node248 = 16777216,
            Node249 = 33554432,
            Node250 = 67108864,
            Node251 = 134217728,
            Node252 = 268435456,
            Node253 = 536870912,
            Node254 = 1073741824,
            Node255 = -2147483648,
        }

        public enum RenderOnlySectionFlags1Value : int
        {
            None,
            Section0,
            Section1,
            Section2 = 4,
            Section3 = 8,
            Section4 = 16,
            Section5 = 32,
            Section6 = 64,
            Section7 = 128,
            Section8 = 256,
            Section9 = 512,
            Section10 = 1024,
            Section11 = 2048,
            Section12 = 4096,
            Section13 = 8192,
            Section14 = 16384,
            Section15 = 32768,
            Section16 = 65536,
            Section17 = 131072,
            Section18 = 262144,
            Section19 = 524288,
            Section20 = 1048576,
            Section21 = 2097152,
            Section22 = 4194304,
            Section23 = 8388608,
            Section24 = 16777216,
            Section25 = 33554432,
            Section26 = 67108864,
            Section27 = 134217728,
            Section28 = 268435456,
            Section29 = 536870912,
            Section30 = 1073741824,
            Section31 = -2147483648,
        }

        public enum RenderOnlySectionFlags2Value : int
        {
            None,
            Section32,
            Section33,
            Section34 = 4,
            Section35 = 8,
            Section36 = 16,
            Section37 = 32,
            Section38 = 64,
            Section39 = 128,
            Section40 = 256,
            Section41 = 512,
            Section42 = 1024,
            Section43 = 2048,
            Section44 = 4096,
            Section45 = 8192,
            Section46 = 16384,
            Section47 = 32768,
            Section48 = 65536,
            Section49 = 131072,
            Section50 = 262144,
            Section51 = 524288,
            Section52 = 1048576,
            Section53 = 2097152,
            Section54 = 4194304,
            Section55 = 8388608,
            Section56 = 16777216,
            Section57 = 33554432,
            Section58 = 67108864,
            Section59 = 134217728,
            Section60 = 268435456,
            Section61 = 536870912,
            Section62 = 1073741824,
            Section63 = -2147483648,
        }

        public enum RenderOnlySectionFlags3Value : int
        {
            None,
            Section64,
            Section65,
            Section66 = 4,
            Section67 = 8,
            Section68 = 16,
            Section69 = 32,
            Section70 = 64,
            Section71 = 128,
            Section72 = 256,
            Section73 = 512,
            Section74 = 1024,
            Section75 = 2048,
            Section76 = 4096,
            Section77 = 8192,
            Section78 = 16384,
            Section79 = 32768,
            Section80 = 65536,
            Section81 = 131072,
            Section82 = 262144,
            Section83 = 524288,
            Section84 = 1048576,
            Section85 = 2097152,
            Section86 = 4194304,
            Section87 = 8388608,
            Section88 = 16777216,
            Section89 = 33554432,
            Section90 = 67108864,
            Section91 = 134217728,
            Section92 = 268435456,
            Section93 = 536870912,
            Section94 = 1073741824,
            Section95 = -2147483648,
        }

        public enum RenderOnlySectionFlags4Value : int
        {
            None,
            Section96,
            Section97,
            Section98 = 4,
            Section99 = 8,
            Section100 = 16,
            Section101 = 32,
            Section102 = 64,
            Section103 = 128,
            Section104 = 256,
            Section105 = 512,
            Section106 = 1024,
            Section107 = 2048,
            Section108 = 4096,
            Section109 = 8192,
            Section110 = 16384,
            Section111 = 32768,
            Section112 = 65536,
            Section113 = 131072,
            Section114 = 262144,
            Section115 = 524288,
            Section116 = 1048576,
            Section117 = 2097152,
            Section118 = 4194304,
            Section119 = 8388608,
            Section120 = 16777216,
            Section121 = 33554432,
            Section122 = 67108864,
            Section123 = 134217728,
            Section124 = 268435456,
            Section125 = 536870912,
            Section126 = 1073741824,
            Section127 = -2147483648,
        }

        public enum RenderOnlySectionFlags5Value : int
        {
            None,
            Section128,
            Section129,
            Section130 = 4,
            Section131 = 8,
            Section132 = 16,
            Section133 = 32,
            Section134 = 64,
            Section135 = 128,
            Section136 = 256,
            Section137 = 512,
            Section138 = 1024,
            Section139 = 2048,
            Section140 = 4096,
            Section141 = 8192,
            Section142 = 16384,
            Section143 = 32768,
            Section144 = 65536,
            Section145 = 131072,
            Section146 = 262144,
            Section147 = 524288,
            Section148 = 1048576,
            Section149 = 2097152,
            Section150 = 4194304,
            Section151 = 8388608,
            Section152 = 16777216,
            Section153 = 33554432,
            Section154 = 67108864,
            Section155 = 134217728,
            Section156 = 268435456,
            Section157 = 536870912,
            Section158 = 1073741824,
            Section159 = -2147483648,
        }

        public enum RenderOnlySectionFlags6Value : int
        {
            None,
            Section160,
            Section161,
            Section162 = 4,
            Section163 = 8,
            Section164 = 16,
            Section165 = 32,
            Section166 = 64,
            Section167 = 128,
            Section168 = 256,
            Section169 = 512,
            Section170 = 1024,
            Section171 = 2048,
            Section172 = 4096,
            Section173 = 8192,
            Section174 = 16384,
            Section175 = 32768,
            Section176 = 65536,
            Section177 = 131072,
            Section178 = 262144,
            Section179 = 524288,
            Section180 = 1048576,
            Section181 = 2097152,
            Section182 = 4194304,
            Section183 = 8388608,
            Section184 = 16777216,
            Section185 = 33554432,
            Section186 = 67108864,
            Section187 = 134217728,
            Section188 = 268435456,
            Section189 = 536870912,
            Section190 = 1073741824,
            Section191 = -2147483648,
        }

        public enum RenderOnlySectionFlags7Value : int
        {
            None,
            Section192,
            Section193,
            Section194 = 4,
            Section195 = 8,
            Section196 = 16,
            Section197 = 32,
            Section198 = 64,
            Section199 = 128,
            Section200 = 256,
            Section201 = 512,
            Section202 = 1024,
            Section203 = 2048,
            Section204 = 4096,
            Section205 = 8192,
            Section206 = 16384,
            Section207 = 32768,
            Section208 = 65536,
            Section209 = 131072,
            Section210 = 262144,
            Section211 = 524288,
            Section212 = 1048576,
            Section213 = 2097152,
            Section214 = 4194304,
            Section215 = 8388608,
            Section216 = 16777216,
            Section217 = 33554432,
            Section218 = 67108864,
            Section219 = 134217728,
            Section220 = 268435456,
            Section221 = 536870912,
            Section222 = 1073741824,
            Section223 = -2147483648,
        }

        public enum RenderOnlySectionFlags8Value : int
        {
            None,
            Section224,
            Section225,
            Section226 = 4,
            Section227 = 8,
            Section228 = 16,
            Section229 = 32,
            Section230 = 64,
            Section231 = 128,
            Section232 = 256,
            Section233 = 512,
            Section234 = 1024,
            Section235 = 2048,
            Section236 = 4096,
            Section237 = 8192,
            Section238 = 16384,
            Section239 = 32768,
            Section240 = 65536,
            Section241 = 131072,
            Section242 = 262144,
            Section243 = 524288,
            Section244 = 1048576,
            Section245 = 2097152,
            Section246 = 4194304,
            Section247 = 8388608,
            Section248 = 16777216,
            Section249 = 33554432,
            Section250 = 67108864,
            Section251 = 134217728,
            Section252 = 268435456,
            Section253 = 536870912,
            Section254 = 1073741824,
            Section255 = -2147483648,
        }

        public enum RuntimeFlagsValue : int
        {
            None,
            ContainsRuntimeNodes,
            Bit1,
            Bit2 = 4,
            Bit3 = 8,
            Bit4 = 16,
            Bit5 = 32,
            Bit6 = 64,
            Bit7 = 128,
            Bit8 = 256,
            Bit9 = 512,
            Bit10 = 1024,
            Bit11 = 2048,
            Bit12 = 4096,
            Bit13 = 8192,
            Bit14 = 16384,
            Bit15 = 32768,
            Bit16 = 65536,
            Bit17 = 131072,
            Bit18 = 262144,
            Bit19 = 524288,
            Bit20 = 1048576,
            Bit21 = 2097152,
            Bit22 = 4194304,
            Bit23 = 8388608,
            Bit24 = 16777216,
            Bit25 = 33554432,
            Bit26 = 67108864,
            Bit27 = 134217728,
            Bit28 = 268435456,
            Bit29 = 536870912,
            Bit30 = 1073741824,
            Bit31 = -2147483648,
        }

        [TagStructure(Size = 0x44)]
        public class ScenarioLoadParametersBlock
        {
            public CachedTagInstance Scenario;
            public byte[] Data;

            [TagField(Padding = true, Length = 32)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x8)]
        public class UnknownBlock2
        {
            public StringId Region;
            public StringId Permutation;
        }

        [TagStructure(Size = 0x8)]
        public class UnknownBlock3
        {
            public StringId Unknown;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x14)]
        public class UnknownBlock4
        {
            public StringId Marker;
            public uint Unknown;
            public StringId Marker2;
            public uint Unknown2;
            public uint Unknown3;
        }

        [TagStructure(Size = 0x8)]
        public class UnknownTarget
        {
            public StringId MarkerName;
            public float Unknown;
        }
    }
}