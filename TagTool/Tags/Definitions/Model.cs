using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Damage;
using static TagTool.Tags.Definitions.Gen4.Model;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x188, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1A0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1B4, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x1B8, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x220, MinVersion = CacheVersion.HaloReach)]
    public partial class Model : TagStructure
	{
        [TagField(ValidTags = new[] { "mode" })] public CachedTag RenderModel;
        [TagField(ValidTags = new[] { "coll" })] public CachedTag CollisionModel;
        [TagField(ValidTags = new[] { "jmad" })] public CachedTag Animation;
        [TagField(ValidTags = new[] { "phmo" })] public CachedTag PhysicsModel;

        [TagField(ValidTags = new[] { "impo" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag ImpostorModel;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int ModelChecksum;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int CollisionModelChecksum;

        public float DisappearDistance;
        public float BeginFadeDistance;
        public float AnimationDistance;
        public float ShadowFadeDistance; // (ReduceToL1) world units
        public float InstanceDisappearDistance;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ImposterQualityDefinition ImposterQuality;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ImposterPolicyDefinition ImposterPolicy;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ImposterBrightnessAdjustment;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float InstanceDisappearDistanceReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MidrangeDetailDisappearDistance;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float CloseDetailDisappearDistance;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float TessellationMaxDrawDistance;

        [TagField(ValidTags = new[] { "mode" }, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag LodModel;

        public List<Variant> Variants;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<RegionName> RegionSort;

        public List<InstanceGroup> InstanceGroups;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Material> ReachMaterialsOld;
        public List<Material> Materials;

        public List<GlobalDamageInfoBlock> NewDamageInfo;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public OmahaDamageInfoBlock OmahaDamageInfo; //this block has been inlined into the tag for Halo Reach, but the old block above was also preserved

        //Halo Reach preserves an old H3/ODST style targets block here, but we will ignore it in favor of unifying the blocks between versions
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TargetOldReach> ReachTargetsOld;

        public List<Target> Targets;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public List<GrenadeTarget> GrenadeTargets;
        
        public List<CollisionRegion> CollisionRegions;
        public List<Node> Nodes;
        public int RuntimeNodeListChecksum;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<ModelObjectDatum> ModelObjectData;

        [TagField(ValidTags = new[] { "udlg" })] public CachedTag PrimaryDialogue;
        [TagField(ValidTags = new[] { "udlg" })] public CachedTag SecondaryDialogue;

        public FlagsValue Flags;
        public StringId DefaultDialogueEffect;

        [TagField(Length = 8)]
        public int[] RenderOnlyNodeFlags = new int[8];
        [TagField(Length = 8)]
        public int[] RenderOnlySectionFlags = new int[8];

        public RuntimeFlagsValue RuntimeFlags;
        [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown3;

        public List<ScenarioLoadParametersBlock> ScenarioLoadParameters;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SkyParallaxPercent;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ShadowDepthCompareBias;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ShadowSlopeScaleBias;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ShadowDepthCompareBias_DynamicLights;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float ShadowSlopeScaleBias_DynamicLights;

        public PRTShadowDetail ShadowDetail;
        public PRTShadowBounces ShadowBounces;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Pad = new byte[2];
      
        public List<ShadowCastOverride> ShadowCastOverrides;
        public List<ShadowReceiveOverride> ShadowReceiveOverrides;
        public List<OcclusionSphere> OcclusionSpheres;

        [TagField(ValidTags = new[] { "shit" })]
        public CachedTag ShieldImpactThirdPerson;
        [TagField(ValidTags = new[] { "shit" })]
        public CachedTag ShieldImpactFirstPerson;

        [TagField(ValidTags = new[] { "shit" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag OvershieldThirdPerson;
        [TagField(ValidTags = new[] { "shit" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public CachedTag OvershieldFirstPerson;

        //Reach has the model object data block inlined here
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float Radius;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RealPoint3d Offset;

        public enum ShadowFadeDistanceValue : short
        {
            FadeAtSuperHighDetailLevel,
            FadeAtHighDetailLevel,
            FadeAtMediumDetailLevel,
            FadeAtLowDetailLevel,
            FadeAtSuperLowDetailLevel,
            FadeNever
        }

        public enum PRTShadowDetail : byte
        {
            AmbientOcclusion,
            Linear,
            Quadratic,
            Undefined // HO
        }

        public enum PRTShadowBounces : byte
        {
            ZeroBounces,
            OneBounce,
            TwoBounces,
            ThreeBounces
        }

        public enum ImposterQualityDefinition : short
        {
            Default,
            High,
            Super
        }

        public enum ImposterPolicyDefinition : short
        {
            Default,
            Never,
            Always
        }

        [TagStructure(Size = 0x38, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x30, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x50, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x38, MinVersion = CacheVersion.HaloReach)]
        public class Variant : TagStructure
		{
            public StringId Name;

            [TagField(ValidTags = new[] { "udlg" }, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public CachedTag Voice; // The dialogue tag for this model variant

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public StringId DialogueEffect;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public OdstReconVariantEnum AiCharacter;

            [TagField(Length = 0x3, Flags = Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public StringId SkinName;

            [TagField(Length = 16)]
            public sbyte[] ModelRegionIndices = new sbyte[16];

            public List<Region> Regions;
            public List<Object> Objects;

            public int InstanceGroupIndex;

            [TagField(Length = 0x8, Flags = Padding, Platform = CachePlatform.Original)]
            public byte[] Padding1;

            [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public class Region : TagStructure
			{
                public StringId Name;
                
                public sbyte RenderModelRegionIndex;             
                public sbyte RuntimeFlags;

                public short ParentVariant;

                public List<Permutation> Permutations;

                [TagField(Platform = CachePlatform.Original)]
                public SortOrderValue SortOrder;

                [TagStructure(Size = 0x24, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public class Permutation : TagStructure
				{
                    public StringId Name;                  
                    public sbyte RenderModelPermutationIndex;

                    public FlagsValue Flags;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Pad = new byte[2];

                    public float Probability;
                    public List<State> States;

                    [TagField(Length = 12, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
                    public sbyte[] RuntimeStatePermutationIndices = new sbyte[12];

                    [Flags]
                    public enum FlagsValue : byte
                    {
                        None,
                        CopyStatesToAllPermutations = 1 << 0,
                        Bit1 = 1 << 1
                    }

                    [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
                    [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloReach)]
                    public class State : TagStructure
					{
                        public StringId Name;
                        public sbyte ModelPermutationIndex;
                        public PropertyFlagsValue PropertyFlags;
                        public StateValue State2;
                        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                        public CachedTag LoopingEffect;
                        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                        public StringId LoopingEffectMarkerName;
                        public float InitialProbability;

                        [Flags]
                        public enum PropertyFlagsValue : byte
                        {
                            None = 0,
                            Blurred = 1 << 0,
                            HellaBlurred = 1 << 1,
                            Shielded = 1 << 2,
                            Bit3 = 1 << 3,
                            Bit4 = 1 << 4,
                            Bit5 = 1 << 5,
                            Bit6 = 1 << 6,
                            Bit7 = 1 << 7
                        }

                        public enum StateValue : short
                        {
                            Default,
                            MinorDamage,
                            MediumDamage,
                            MajorDamage,
                            Destroyed
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

            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
            public class Object : TagStructure
			{
                public StringId ParentMarker;
                public StringId ChildMarker;               
                public StringId ChildVariant;

                public CachedTag ChildObject;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public short DamageSection;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public byte EnablePhysics;
                [TagField(MinVersion = CacheVersion.HaloReach, Flags = Padding, Length = 1)]
                public byte[] Pad;
            }

            public enum OdstReconVariantEnum : sbyte
            {
                None,
                Rookie,
                Buck,
                Dare,
                Dutch,
                Johnson,
                Mickey,
                Romeo
            }
        }

        [TagStructure(Size = 0x4)]
        public class RegionName : TagStructure
		{
            public StringId Name;
        }

        [TagStructure(Size = 0x18)]
        public class InstanceGroup : TagStructure
		{
            public StringId Name;
            public ChoiceValue Choice;
            public List<InstanceMember> InstanceMembers;
            public float Probability;

            public enum ChoiceValue : int
            {
                ChooseOneMember,
                ChooseAllMembers
            }

            [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public class InstanceMember : TagStructure
			{
                public int SubgroupIndex; // if this member is chosen, this subgroup will be chosen as well
                public StringId InstanceName; // instance name, a partial name will choose all matching instances, leave blank for NONE
                public float Probability; // higher numbers make it more likely (> 0.0)

                public int InstancePlacementMask1;
                public int InstancePlacementMask2;

                [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
                public int InstancePlacementMask3;
                [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
                public int InstancePlacementMask4;
            }
        }

        [TagStructure(Size = 0x14)]
        public class Material : TagStructure
		{
            public StringId MaterialName;
            public MaterialTypeValue MaterialType;
            public short DamageSectionIndex;
            public short RuntimeCollisionMaterialIndex;
            public short RuntimeDamagerMaterialIndex;
            [TagField(Flags = GlobalMaterial)]
            public StringId GlobalMaterialName;
            [TagField(Flags = GlobalMaterial)]
            public short GlobalMaterialIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding0 = new byte[2];

            public enum MaterialTypeValue : short
            {
                Dirt,
                Sand,
                Stone,
                Snow,
                Wood,
                Metalhollow,
                Metalthin,
                Metalthick,
                Rubber,
                Glass,
                ForceField,
                Grunt,
                HunterArmor,
                HunterSkin,
                Elite,
                Jackal,
                JackalEnergyShield,
                EngineerSkin,
                EngineerForceField,
                FloodCombatForm,
                FloodCarrierForm,
                CyborgArmor,
                CyborgEnergyShield,
                HumanArmor,
                HumanSkin,
                Sentinel,
                Monitor,
                Plastic,
                Water,
                Leaves,
                EliteEnergyShield,
                Ice,
                HunterShield
            }
        }

        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.HaloReach)]
        public class OmahaDamageInfoBlock : TagStructure
        {
            public FlagsValue Flags;
            public float MaximumVitality;

            [TagField(Flags = GlobalMaterial)]
            public StringId GlobalIndirectMaterialName;
            public short IndirectDamageSection;
            public short ShieldedStateDamageSection;

            public DamageReportingType CollisionDamageReportingType;
            public DamageReportingType ResponseDamageReportingType;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] pad0 = new byte[2];

            public List<OmahaDamageSection> DamageSections;
            public List<DamageConstraint> DamageConstraints;
            public List<Node> Nodes;

            [TagField(Flags = GlobalMaterial)]
            public short RuntimeIndirectMaterialIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] pad1 = new byte[2];

            [TagStructure(Size = 0x10)]
            public class Node : TagStructure
            {
                public short RuntimeDamagePart;
                public short Unknown;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Unused1 = new byte[12];
            }

            [TagStructure(Size = 0x14)]
            public class DamageConstraint : TagStructure
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                public TypeValue Type;
                public short Index;

                public enum TypeValue : short
                {
                    None = -1,
                    Hinge,
                    LimitedHinge,
                    Ragdoll,
                    StiffSpring,
                    BallAndSocket,
                    Prismatic
                }
            }

            [TagStructure(Size = 0xB4, MinVersion = CacheVersion.HaloReach)]
            public class OmahaDamageSection : TagStructure
            {
                public StringId Name;
                public FlagsValue Flags;
                public float VitalityPercentage;
                [TagField(Flags = GlobalMaterial)]
                public StringId ShieldMaterialName;

                public float StunTime;
                public float MinimumStunDamage;
                public float RechargeTime;

                public List<RechargeSpeedCurve> RechargeSpeedCurves;
                public List<RechargeFraction> RechargeFractions;

                public CachedTag RechargingEffect;
                public float PreRechargeEffectWarnTime;
                public CachedTag PreRechargeEffect;
                public StringId PreRechargeEffectMarker;
                public CachedTag PreRechargeAbortEffect;
                public StringId PreRechargeAbortEffectMarker;
                public float OverchargeTime;
                public float OverchargeFraction;
                public float PreDecayTime;
                public float DecayTime;
                public StringId ResurrectionRestoredRegionName;

                public List<OmahaInstantResponse> InstantResponses;
                public List<DamageTransfer> SectionDamageTransfers;
                public List<Rendering> RenderingParameters;

                public float RuntimeRechargeVelocity;
                public float RuntimeOverchargeVelocity;
                public short RuntimeResurrectionRestoredRegionIndex;
                [TagField(Flags = GlobalMaterial)]
                public short RuntimeGlobalShieldMaterialType;

                [TagStructure(Size = 0x20, MinVersion = CacheVersion.HaloReach)]
                public class Rendering : TagStructure
                {
                    public CachedTag ThirdPersonImpactParameters;
                    public CachedTag FirstPersonImpactParameters;
                }

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

                [TagStructure(Size = 0xB0, MinVersion = CacheVersion.HaloReach)]
                public class OmahaInstantResponse : TagStructure
                {
                    public FlagsValue Flags;
                    public StringId Label;
                    public float DamageThreshold;
                    public CachedTag GenericTransitionEffect;
                    public StringId GenericEffectMarker;
                    public CachedTag SpecificTransitionEffect;
                    public StringId SpecificEffectMarker;
                    public CachedTag TransitionDamageEffect;
                    public StringId DamageEffectMarkerName;
                    public CachedTag LoopingEffect;

                    public List<RegionTransition> RegionTransitions;
                    public List<DamageTransfer> ResponseDamageTransfers;

                    public short DestroyInstanceGroupIndex;
                    public CustomResponseBehaviorValue CustomResponseBehavior;
                    public StringId CustomResponseLabel;
                    public float ResponseDelay;
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;
                    public List<SeatEject> SeatEjections;
                    public float SkipFraction;
                    public StringId DestroyedChildObjectMarkerName;
                    public float TotalDamageThreshold;
                    public StringId ConstraintGroupName;
                    public ConstraintDamageTypeValue ConstraintDamageType;
                    public short Unknown;

                    [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
                    public class SeatEject : TagStructure
                    {
                        public StringId Label;
                    }

                    [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloReach)]
                    public class RegionTransition : TagStructure
                    {
                        public StringId Region;
                        public NewStateValue State;
                        public short RuntimeRegionIndex;

                        public enum NewStateValue : short
                        {
                            Default,
                            MinorDamage,
                            MediumDamage,
                            MajorDamage,
                            Destroyed
                        }
                    }

                    public enum CustomResponseBehaviorValue : short
                    {
                        PlaysAlways,
                        PlaysIfLabelsMatch,
                        PlaysIfLabelsDiffer
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
                        CauseLeftLegDismemberment = 1 << 31
                    }
                }

                [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
                public class DamageTransfer : TagStructure
                {
                    public FlagsValue Flags;
                    public float TransferAmount;
                    public TransferFunction Function;
                    public short DamageSectionIndex;
                    public StringId SeatLabel;

                    public enum TransferFunction : short
                    {
                        percent,
                        points,
                        ceiling
                    }

                    [Flags]
                    public enum FlagsValue : int
                    {
                        None,
                        TransferDamagetoParentSection = 1 << 0,
                        TransferDamagetoParent = 1 << 1,
                        TransferDamagetoChildren = 1 << 2,
                        TransferDamagetoSeats = 1 << 3,
                        TransferDirectDamage = 1 << 4,
                        TransferAOEExposedDamage = 1 << 5,
                        TransferAOEObstructedDamage = 1 << 6,
                    }
                }

                [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
                public class RechargeSpeedCurve : TagStructure
                {
                    public TagFunction Function;
                }

                [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
                public class RechargeFraction : TagStructure
                {
                    public float VitalityPercentage;
                }
            }

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
        }

        [TagStructure(Size = 0x100, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0xA4, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x10C, MinVersion = CacheVersion.HaloReach)]
        public class GlobalDamageInfoBlock : TagStructure
		{
            public FlagsValue Flags;

            /// <summary>
            /// Absorbes AOE or child damage
            /// </summary>
            [TagField(Flags = GlobalMaterial)]
            public StringId GlobalIndirectMaterialName;

            /// <summary>
            /// Absorbes AOE or child damage
            /// </summary>
            public short IndirectDamageSection;

            [TagField(Flags = Padding, Length = 6, Platform = CachePlatform.Original)]
            public byte[] Padding0 = new byte[6];

            public DamageReportingType CollisionDamageReportingType;
            public DamageReportingType ResponseDamageReportingType;

            [TagField(Length = 0x2, Flags = Padding, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
            public byte[] MQ;

            [TagField(Flags = Padding, Length = 20, Platform = CachePlatform.Original)]
            public byte[] Padding1 = new byte[20];

            public float MaximumVitality;
            public float MinStunDamage;
            public float StunTime;
            public float RechargeTime;
            public float RechargeFraction;

            [TagField(Length = 64, Flags = Padding, Platform = CachePlatform.Original)]
            public byte[] Padding2 = new byte[64];

            public float MaxShieldVitality;
            [TagField(Flags = GlobalMaterial)]
            public StringId GlobalShieldMaterialName;
            public float ShieldMinStunDamage;
            public float ShieldStunTime;
            public float ShieldRechargeTime;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldOverchargeFraction;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float ShieldOverchargeTime;

            public float ShieldDamagedThreshold;
            public CachedTag ShieldDamagedEffect;
            public CachedTag ShieldDepletedEffect;
            public CachedTag ShieldRechargingEffect;

            public List<DamageSection> DamageSections;
            public List<Node> Nodes;

            [TagField(Flags = GlobalMaterial)]
            public short GlobalShieldMaterialIndex;
            [TagField(Flags = GlobalMaterial)]
            public short GlobalIndirectMaterialIndex;
            public float RuntimeShieldRechargeVelocity;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RuntimeOverchargeVelocity;

            public float RuntimeHealthRechargeVelocity;
            public List<DamageSeat> DamageSeats;
            public List<DamageConstraint> DamageConstraints;

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

            [TagStructure(Size = 0x44, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public class DamageSection : TagStructure
			{
                public StringId Name;
                public FlagsValue Flags;
                public float VitalityPercentage;
                public List<InstantResponse> InstantResponses;

                [TagField(Flags = Padding, Length = 24, Platform = CachePlatform.Original)]
                public byte[] NullBlocksPadding = new byte[24];
             
                public float StunTime;
                public float RechargeTime;
                public float RuntimeRechargeVelocity;

                public StringId ResurrectionRestoredRegionName;
                public short ResurrectionRegionRuntimeIndex;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1 = new byte[2];

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

                [TagStructure(Size = 0x88, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x90, MinVersion = CacheVersion.HaloReach)]
                public class InstantResponse : TagStructure
				{
                    public ResponseTypeValue ResponseType;
                    public ConstraintDamageTypeValue ConstraintDamageType;
                    
                    public StringId ConstraintGroupName;

                    public FlagsValue Flags;
                    public float DamageThreshold;

                    //TODO: verify these fields
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public int BodyThresholdFlags;
                    [TagField(MinVersion = CacheVersion.HaloReach)]
                    public float BodyDamageThreshold;

                    [TagField(ValidTags = new [] { "effe", "jpt!" })] public CachedTag PrimaryTransitionEffect;
                    [TagField(ValidTags = new [] { "effe", "jpt!" })] public CachedTag SecondaryTransitionEffect;
                    [TagField(ValidTags = new [] { "effe", "jpt!" })] public CachedTag TransitionDamageEffect;

                    public StringId Region;
                    public NewStateValue NewState;
                    public short RuntimeRegionIndex;
                    
                    public StringId SecondaryRegion;
                    public NewStateValue SecondaryNewState;
                    public short SecondaryRuntimeRegionIndex;
                   
                    public short DestroyInstanceGroup; //block index, all possible instances from this group will be destroyed

                    public CustomResponseBehaviorValue CustomResponseBehavior;                   
                    public StringId CustomResponseLabel;

                    public StringId EffectMarkerName;
                    public StringId DamageEffectMarkerName;
                    public float ResponseDelay;
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;

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
                        CauseLeftLegDismemberment = 1 << 31
                    }

                    public enum NewStateValue : short
                    {
                        Default,
                        MinorDamage,
                        MediumDamage,
                        MajorDamage,
                        Destroyed
                    }
                    
                    public enum CustomResponseBehaviorValue : short
                    {
                        PlaysAlways,
                        PlaysIfLabelsMatch,
                        PlaysIfLabelsDiffer
                    }
                }
            }

            [TagStructure(Size = 0x10, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x2, Platform = CachePlatform.MCC)]
            public class Node : TagStructure
			{
                public short RuntimeDamagePart;
                [TagField(Flags = Padding, Length = 14, Platform = CachePlatform.Original)]
                public byte[] Unused1 = new byte[14];
            }

            [TagStructure(Size = 0x20, MinVersion = CacheVersion.Halo3Retail)]
            public class DamageSeat : TagStructure
			{
                public StringId SeatLabel;
                public float DirectDamageScale;
                public float DamageTransferFallOffRadius;
                public float MaximumTransferDamageScale;
                public float MinimumTransferDamageScale;
                
                public List<RegionSpecificDamageBlock> RegionSpecificDamage;

                [TagStructure(Size = 0x2C)]
                public class RegionSpecificDamageBlock : TagStructure
				{
                    public StringId DamageRegionName;
                    public short RuntimeDamageRegionIndex;

                    [TagField(Flags = Padding, Length = 2)]
                    public short Unused;

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
            public class DamageConstraint : TagStructure
			{
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                public TypeValue Type;
                public short Index;

                public enum TypeValue : short
                {
                    None = -1,
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
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnlineED)]
        public class Target : TagStructure
		{
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public ByteFlags Flags;
            [TagField(MinVersion = CacheVersion.HaloOnlineED, Flags = Padding, Length = 3)]
            public byte[] pad = new byte[3];

            public StringId MarkerName;
            public float Size;
            public Angle ConeAngle;
            public short DamageSection;
            public short Variant;
            public float TargetingRelevance;
            public float AoeExclusionRadius;
            public TargetLockOnData LockOnData;

            [Flags]
            public enum ByteFlags : byte
            {
                None = 0,
                AoeTopLevel = 1 << 0,
                AoeTestObstruction = 1 << 1,
                ShowsTrackingReticle = 1 << 2,
            }
        }

        [TagStructure(Size = 0x24)]
        public class TargetOldReach : TagStructure
        {
            public StringId MarkerName; // multiple markers become multiple spheres of the same radius
            public float Size; // sphere radius
            public Angle ConeAngle; // the target is only visible when viewed within this angle of the marker's x axis
            public short DamageSection; // the target is associated with this damage section
            public short Variant; // the target will only appear with this variant
            public float TargetingRelevance; // higher relevances turn into stronger magnetisms
            public float AoeExclusionRadius; // ignored if zero
            public TargetLockOnData LockOnData;
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloOnlineED)]
        public class TargetLockOnData : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public FlagsValueOld FlagsOld;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public FlagsValue Flags;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x3, MinVersion = CacheVersion.HaloOnlineED)]
            public byte[] Padding1;
            public float LockOnDistance;
            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public StringId TrackingType; // a weapon can track/lock on this target if this string is in the weapon's tracking block

            [Flags]
            public enum FlagsValueOld : uint
            {
                None = 0,
                LockedByHumanTracking = 1 << 0,
                LockedByPlasmaTracking = 1 << 1,
                Headshot = 1 << 2,
                Vulnerable = 1 << 3,
                AlwaysLockedByPlasmaTracking = 1 << 4,
                IgnoredOnLocalPhysics = 1 << 5,
                UseForNetworkLeadVectorOnly = 1 << 6,
                Bit7 = 1 << 7   //unknown
            }

            [Flags]
            public enum FlagsValue : byte
            {
                Headshot = 1 << 0,
                Vulnerable = 1 << 1,
                IgnoredOnLocalPhysics = 1 << 2,
                UseForNetworkLeadVectorOnly = 1 << 3
            }
        }

        [TagStructure(Size = 0x14, MinVersion = CacheVersion.Halo3Retail)]
        public class CollisionRegion : TagStructure
		{
            public StringId Name;
            public sbyte CollisionRegionIndex;
            public sbyte PhysicsRegionIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Pad1 = new byte[2];
            public List<Permutation> Permutations;

            [TagStructure(Size = 0x8)]
            public class Permutation : TagStructure
			{
                public StringId Name;
                public FlagsValue Flags;
                public sbyte CollisionPermutationIndex;
                public sbyte PhysicsPermutationIndex;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Pad = new byte[1];

                [Flags]
                public enum FlagsValue : byte
                {
                    None = 0,
                    CannotBeChosenRandomly = 1 << 0
                }
            }
        }

        [TagStructure(Size = 0x5C)]
        public class Node : TagStructure
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
        public class ModelObjectDatum : TagStructure
		{
            public TypeValue Type;

            [TagField(Length = 0x2, Flags = Padding)]
            public byte[] Padding0;

            public RealPoint3d Offset;
            public float Radius;

            public enum TypeValue : short
            {
                NotSet,
                UserDefined,
                AutoGenerated
            }
        }

        [Flags]
        public enum FlagsValue : int
        {
            // todo: flags versioning
            None = 0,
            ActiveCamoAlwaysOn = 1 << 0,
            ActiveCamoNever = 1 << 1,
            HasShieldImpactEffect = 1 << 2,
            ModelUseSkyLighting = 1 << 3,
            InconsequentialTarget = 1 << 4,
            ModelUseAirprobeLighting = 1 << 5,
            Unknown6 = 1 << 6, // HO only (fabric_01_moving)
            IsDyanmic = 1 << 7 // HO only (armor pieces)
        }

        [Flags]
        public enum RuntimeFlagsValue : int
        {
            None,
            ContainsRuntimeNodes = 1 << 0
        }

        [TagStructure(Size = 0x44)]
        public class ScenarioLoadParametersBlock : TagStructure
		{
            public CachedTag Scenario;
            public byte[] Data;

            [TagField(Flags = Padding, Length = 32)]
            public byte[] Unused;
        }

        [TagStructure(Size = 0x8)]
        public class ShadowCastOverride : TagStructure
		{
            public StringId Region;
            public StringId Permutation;
        }

        [TagStructure(Size = 0x8)]
        public class ShadowReceiveOverride : TagStructure
		{
            public StringId Region;
            public ShadowTypes ShadowType;

            public enum ShadowTypes : int
            {
                PRTShadowsFromAllRegions,
                PRTSelfShadowOnly,
                NoPRTShadowsAtAll
            }
        }

        [TagStructure(Size = 0x14)]
        public class OcclusionSphere : TagStructure
		{
            public StringId Marker1Name;
            public uint Marker1Index;
            public StringId Marker2Name;
            public uint Marker2Index;
            public float Radius;
        }

        [TagStructure(Size = 0x8)]
        public class GrenadeTarget : TagStructure
		{
            public StringId MarkerName; // 1 marker == sphere, 2 markers == pill, >2 markers == multi sphere
            public float Size; // sphere or pill radius
        }
    }
}