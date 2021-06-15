using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x260)]
    public class Model : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag RenderModel;
        [TagField(ValidTags = new [] { "coll" })]
        public CachedTag CollisionModel;
        [TagField(ValidTags = new [] { "jmad" })]
        public CachedTag Animation;
        [TagField(ValidTags = new [] { "phmo" })]
        public CachedTag PhysicsModel;
        [TagField(ValidTags = new [] { "impo" })]
        public CachedTag ImposterModel;
        public int RuntimeRenderChecksum;
        public int RuntimeCollisionChecksum;
        [TagField(ValidTags = new [] { "stli" })]
        public CachedTag LightingInfo;
        public ScenarioStructureSizeEnum SizeClass;
        public ModelLightmapFlags LightmapFlags;
        public int LightmapVariant;
        // How much we extend the PVS region around the objects AABB
        public float PvsBoundingBoxExtensionFactor; // [good initial value 2.5]
        // How big a single PVS block is, in world units
        public RealVector3d PvsBlockSize; // [good initial value (2.0,2.0,2.0)]
        // How many sample subdivisions we perform per PVS block when generating the data
        public int PvsSamplingSubdivisionPerAxis; // [good initial value 2]
        // Minimum amount we need to see of an individual part mesh to care about it
        public float PvsVisibilityThreshold; // [good initial value 0.004]
        public float DisappearDistance; // world units
        public float BeginFadeDistance; // world units
        public float AnimationLodDistance; // world units
        // NOTE this is only a maximum distance, shadows may fade closer when you exceed the shadow budget, you should balance
        // the total shadows in a scene
        public float ShadowFadeDistance; // world units
        public float ImposterRenderDistance; // world units
        public ImposterQualityDefinition ImposterQuality;
        public ImposterPolicyDefinition ImposterPolicy;
        public float ImposterBrightnessAdjustment;
        public float InstanceDisappearDistance; // world units
        // distance at which the midrange detail disappears
        public float MidrangeDetailDisappearDistance; // world units
        // distance at which the close detail disappears
        public float CloseDetailDisappearDistance; // world units
        public float TessellationMaxDrawDistance; // world units
        public ModelLodResourceDistanceFlags ResourceDistanceOverrideFlags;
        public float MediumPriorityDistance;
        public float LowPriorityDistance;
        public List<ModelVariantBlock> Variants;
        public List<RegionNameBlock> RegionSort;
        public List<GlobalModelInstanceGroupBlock> InstanceGroups;
        public List<ModelMaterialBlockNew> ModelMaterials;
        public List<GlobalDamageInfoBlock> NewDamageInfo;
        public ModelDamageInfoStruct DamageInfo;
        public List<ModelTargetBlockOld> TargetsOld;
        public List<ModelTargetBlockNew> ModelTargets;
        public List<ModelRegionBlock> RuntimeRegions;
        public List<ModelNodeBlock> RuntimeNodes;
        public int RuntimeNodeListChecksum;
        // The default dialogue tag for this model (overriden by variants)
        [TagField(ValidTags = new [] { "udlg" })]
        public CachedTag DefaultDialogue;
        // The default FEMALE dialogue tag for this model (overriden by variants)
        [TagField(ValidTags = new [] { "udlg" })]
        public CachedTag DefaultDialogueFemale;
        public ModelFlags Flags;
        // The default dialogue tag for this model (overriden by variants)
        public StringId DefaultDialogueEffect;
        [TagField(Length = 8)]
        public GNodeFlagStorageArray[]  RenderOnlyNodeFlags;
        [TagField(Length = 8)]
        public GNodeFlagStorageArray[]  RenderOnlySectionFlags;
        public ModelPrivateFlags RuntimeFlags;
        public List<GlobalScenarioLoadParametersBlock> ScenarioLoadParameters;
        public List<ModelGameModeRenderModelOverride> GameModeRenderModelOverride;
        // If flag checked % between sky pos and camera pos 0=camera
        public float SkyParallaxPercent;
        // Default is 0.002
        public float ShadowDepthCompareBias;
        // controls cutoff point for shadows around edges.  Default is 81 degrees
        public float ShadowSlopeScaleBias; // degrees
        // Default is 0.0008
        public float ShadowDepthCompareBias1;
        // controls cutoff point for shadows around edges.  Default is 81 degrees
        public float ShadowSlopeScaleBias1; // degrees
        // how much information is recorded about different light directions
        public ModelSelfShadowDetailDefinition PrtShadowDetail;
        // 0 means direct light only
        public ModelSelfShadowBouncesDefinition PrtShadowBounces;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<ModelSelfShadowRegionCastOverrideBlock> ShadowCastOverride;
        public List<ModelSelfShadowRegionReceiveOverrideBlock> ShadowReceiveOverride;
        public List<ModelOcclusionSphereBlock> OcclusionSpheres;
        [TagField(ValidTags = new [] { "shit" })]
        public CachedTag ShieldImpactParameterOverride;
        [TagField(ValidTags = new [] { "shit" })]
        public CachedTag _FirstPersonShieldImpactParameterOverride;
        public float RuntimeBoundingRadius; // world units*!
        public RealPoint3d RuntimeBoundingOffset;
        
        public enum ScenarioStructureSizeEnum : int
        {
            _32x32,
            _64x64,
            _128x128,
            _256x25604Meg,
            _512x51215Meg,
            _768x76834Meg,
            _1024x10246Meg,
            _1280x128094Meg,
            _1536x1536135Meg,
            _1792x1792184meg
        }
        
        [Flags]
        public enum ModelLightmapFlags : uint
        {
            PvsEnabled = 1 << 0,
            PerVertexAll = 1 << 1,
            GenerateForgeAtlas = 1 << 2
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
        
        [Flags]
        public enum ModelLodResourceDistanceFlags : uint
        {
            OverrideEnabled = 1 << 0
        }
        
        [Flags]
        public enum ModelFlags : uint
        {
            ActiveCamoAlwaysOn = 1 << 0,
            ActiveCamoNever = 1 << 1,
            // used in magnetism and campaign saving
            InconsequentialTarget = 1 << 2,
            ModelUseAirprobeLightingFirst = 1 << 3,
            // air or scenery probe
            LockedPrecomputedProbes = 1 << 4,
            // parallax % between sky pos and camera pos below
            IfSkyAttachesToCamera = 1 << 5,
            ModelIsBigBattleObject = 1 << 6,
            ModelNeverUsesCompressedVertexPosition = 1 << 7,
            ModelIsInvisibleEvenAttachments = 1 << 8,
            ModelCanHaveShieldImpactEffect = 1 << 9,
            ModelIsGoodZOccluder = 1 << 10,
            NoChildObjectsInLightmapShadow = 1 << 11,
            ShouldIncludeModelInFloatingShadow = 1 << 12
        }
        
        [Flags]
        public enum ModelPrivateFlags : uint
        {
            ContainsRunTimeNodes = 1 << 0
        }
        
        public enum ModelSelfShadowDetailDefinition : sbyte
        {
            AmbientOcclusion,
            Linear,
            Quadratic
        }
        
        public enum ModelSelfShadowBouncesDefinition : sbyte
        {
            _0Bounces,
            _1Bounce,
            _2Bounces,
            _3Bounces
        }
        
        [TagStructure(Size = 0x6C)]
        public class ModelVariantBlock : TagStructure
        {
            public StringId Name;
            [TagField(Length = 32)]
            public RuntimeRegionIndexArray[]  RuntimeVariantRegionIndices;
            public List<ModelVariantRegionBlock> Regions;
            public List<ModelVariantObjectBlock> Objects;
            // selects an instance group for this variant
            public int InstanceGroup;
            // turn off animation on these named nodes and children
            public List<ModelVariantMutedNodeBlock> MutedNodes;
            [TagField(Length = 8)]
            public GNodeFlagStorageArray[]  MutedFlag;
            
            [TagStructure(Size = 0x1)]
            public class RuntimeRegionIndexArray : TagStructure
            {
                public sbyte RuntimeRegionIndex;
            }
            
            [TagStructure(Size = 0x18)]
            public class ModelVariantRegionBlock : TagStructure
            {
                public StringId RegionName; // must match region name in render_model
                public sbyte RuntimeRegionIndex;
                public byte RuntimeFlags;
                public short ParentVariant;
                public List<ModelVariantPermutationBlock> Permutations;
                // negative values mean closer to the camera
                public RegionSortEnum SortOrder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum RegionSortEnum : short
                {
                    NoSorting,
                    _5,
                    _4,
                    _3,
                    _2,
                    _1,
                    _0,
                    _11,
                    _21,
                    _31,
                    _41,
                    _51
                }
                
                [TagStructure(Size = 0x24)]
                public class ModelVariantPermutationBlock : TagStructure
                {
                    public StringId PermutationName;
                    public sbyte RuntimePermutationIndex;
                    public ModelVariantPermutationFlags Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public float Probability; // (0,+inf)
                    public List<ModelVariantStateBlock> States;
                    [TagField(Length = 5)]
                    public ModelStatePermutationIndexArray[]  RuntimeStatePermutationIndices;
                    [TagField(Length = 0x7, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    [Flags]
                    public enum ModelVariantPermutationFlags : byte
                    {
                        CopyStatesToAllPermutations = 1 << 0
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class ModelVariantStateBlock : TagStructure
                    {
                        public StringId PermutationName;
                        public sbyte RuntimePermutationIndex;
                        public ModelStatePropertyFlags PropertyFlags;
                        public ModelStateEnum State;
                        public float InitialProbability;
                        
                        [Flags]
                        public enum ModelStatePropertyFlags : byte
                        {
                            Blurred = 1 << 0,
                            HellaBlurred = 1 << 1,
                            Unshielded = 1 << 2,
                            BatteryDepleted = 1 << 3
                        }
                        
                        public enum ModelStateEnum : short
                        {
                            Default,
                            MinorDamage,
                            MediumDamage,
                            MajorDamage,
                            Destroyed
                        }
                    }
                    
                    [TagStructure(Size = 0x1)]
                    public class ModelStatePermutationIndexArray : TagStructure
                    {
                        public sbyte RuntimePermutationIndex;
                    }
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class ModelVariantObjectBlock : TagStructure
            {
                public StringId ParentMarker;
                // the seat in my parent that will control me
                public StringId ParentControllingSeatLabel;
                public StringId ChildMarker;
                // optional
                public StringId ChildVariantName;
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag ChildObject;
                public short DamageSection;
                public ModelVariantObjectFlags Flags;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ModelVariantObjectFlags : byte
                {
                    EnablePhysics = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class ModelVariantMutedNodeBlock : TagStructure
            {
                public StringId NodeName; // must match node name in render_model
            }
            
            [TagStructure(Size = 0x4)]
            public class GNodeFlagStorageArray : TagStructure
            {
                public int FlagData;
            }
        }
        
        [TagStructure(Size = 0x4)]
        public class RegionNameBlock : TagStructure
        {
            public StringId Name;
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalModelInstanceGroupBlock : TagStructure
        {
            // name of this instance group
            public StringId Name;
            // how to choose members
            public ModelInstanceGroupChoiceEnum Choice;
            public List<ModelInstanceGroupMemberBlock> MemberList;
            public float TotalProbability;
            
            public enum ModelInstanceGroupChoiceEnum : int
            {
                ChooseOneMember,
                ChooseAllMembers
            }
            
            [TagStructure(Size = 0x1C)]
            public class ModelInstanceGroupMemberBlock : TagStructure
            {
                // if this member is chosen, this subgroup will be chosen as well
                public int Subgroup;
                // instance name, a partial name will choose all matching instances, leave blank for NONE
                public StringId Instances;
                // higher numbers make it more likely
                public float Probability; // > 0.0
                public int InstancePlacementMask0;
                public int InstancePlacementMask1;
                public int InstancePlacementMask2;
                public int InstancePlacementMask3;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelMaterialBlockNew : TagStructure
        {
            public StringId MaterialName;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short DamageSection;
            public short RuntimeCollisionMaterialIndex;
            public short RuntimeDamagerMaterialIndex;
            public StringId GlobalMaterialName;
            public short RuntimeGlobalMaterialIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x10C)]
        public class GlobalDamageInfoBlock : TagStructure
        {
            public ModelDamageInfoFlags Flags;
            // absorbes AOE or child damage
            public StringId GlobalIndirectMaterialName;
            // absorbes AOE or child damage
            public short IndirectDamageSection;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public GlobalDamageReportingEnum CollisionDamageReportingType;
            public GlobalDamageReportingEnum ResponseDamageReportingType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public DamageBodyParametersStruct Body;
            [TagField(Length = 0x40, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public DamageShieldParametersStruct Shield;
            public List<GlobalDamageSectionBlock> DamageSections;
            public List<GlobalDamageNodesBlock> Nodes;
            public short RuntimeShieldMaterialType;
            public short RuntimeIndirectMaterialType;
            public float RuntimeShieldRechargeVelocity;
            public float RuntimeOverchargeVelocity;
            public float RuntimeHealthRechargeVelocity;
            public List<DamageSeatInfoBlock> DamageSeats;
            public List<DamageConstraintInfoBlock> DamageConstraints;
            
            [Flags]
            public enum ModelDamageInfoFlags : uint
            {
                TakesShieldDamageForChildren = 1 << 0,
                TakesBodyDamageForChildren = 1 << 1,
                AlwaysShieldsFriendlyDamage = 1 << 2,
                PassesAreaDamageToChildren = 1 << 3,
                ParentNeverTakesBodyDamageForUs = 1 << 4,
                OnlyDamagedByExplosives = 1 << 5,
                ParentNeverTakesShieldDamageForUs = 1 << 6,
                CannotDieFromDamage = 1 << 7,
                PassesAttachedDamageToRiders = 1 << 8,
                ShieldDepletionIsPermanent = 1 << 9,
                ShieldDepletionForceHardPing = 1 << 10,
                AiDoNotDamageWithoutPlayer = 1 << 11,
                HealthRegrowsWhileDead = 1 << 12,
                ShieldRechargePlaysOnlyWhenEmpty = 1 << 13,
                IgnoreForceMinimumTransfer = 1 << 14,
                // check this to take control of the new damage info tag block
                OrphanFromPostprocessAutogen = 1 << 15,
                OnlyDamagedByBoardingDamage = 1 << 16
            }
            
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
            
            [TagStructure(Size = 0x14)]
            public class DamageBodyParametersStruct : TagStructure
            {
                public float MaximumVitality;
                // the minimum damage required to stun this object's health
                public float MinimumStunDamage;
                // the length of time the health stay stunned (do not recharge) after taking damage
                public float StunTime; // seconds
                // the length of time it would take for the shields to fully recharge after being completely depleted
                public float RechargeTime; // seconds
                // 0 defaults to 1 - to what maximum level the body health will be allowed to recharge
                public float RechargeFraction;
            }
            
            [TagStructure(Size = 0x50)]
            public class DamageShieldParametersStruct : TagStructure
            {
                // the default initial and maximum shield vitality of this object
                public float MaximumShieldVitality;
                public StringId GlobalShieldMaterialName;
                // the minimum damage required to stun this object's shields
                public float MinimumStunDamage;
                // the length of time the shields stay stunned (do not recharge) after taking damage
                public float StunTime; // seconds
                // the length of time it would take for the shields to fully recharge after being completely depleted
                public float RechargeTime; // seconds
                // fraction to which shields will automatically overcharge, values <= 1.0 are ignored
                public float ShieldOverchargeFraction;
                // time it takes to reach full "shield overcharge fraction"
                public float ShieldOverchargeTime;
                public float ShieldDamagedThreshold;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ShieldDamagedEffect;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ShieldDepletedEffect;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ShieldRechargingEffect;
            }
            
            [TagStructure(Size = 0x44)]
            public class GlobalDamageSectionBlock : TagStructure
            {
                public StringId Name;
                public DamageSectionFlags Flags;
                // percentage of total object vitality
                public float VitalityPercentage; // [0.1]
                public List<InstantaneousDamageRepsonseBlock> InstantResponses;
                public List<GNullBlock> Unused0;
                public List<GNullBlock> Unused1;
                public float StunTime; // seconds
                public float RechargeTime; // seconds
                public float RuntimeRechargeVelocity;
                public StringId ResurrectionRestoredRegionName;
                public short RuntimeResurrectionRestoredRegionIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum DamageSectionFlags : uint
                {
                    AbsorbsBodyDamage = 1 << 0,
                    TakesFullDmgWhenObjectDies = 1 << 1,
                    CannotDieWithRiders = 1 << 2,
                    TakesFullDmgWhenObjDstryd = 1 << 3,
                    RestoredOnRessurection = 1 << 4,
                    Unused0 = 1 << 5,
                    Unused1 = 1 << 6,
                    Heatshottable = 1 << 7,
                    IgnoresShields = 1 << 8,
                    TakesFullDmgWhenShieldDepl = 1 << 9,
                    Networked = 1 << 10,
                    AllowDamageResponseOverflow = 1 << 11
                }
                
                [TagStructure(Size = 0x98)]
                public class InstantaneousDamageRepsonseBlock : TagStructure
                {
                    public DamageResponseTypeEnum ResponseType;
                    public DamageResponseConstraintDamageTypeEnum ConstraintDamageType;
                    public StringId ConstraintGroupName;
                    public DamageResponseSet1 Flags;
                    public DamageResponseSet2 Flags2;
                    // response fires after crossing this threshold.  1=full health
                    public float DamageThreshold;
                    public DamageResponseBodyThresholdFlags BodyThresholdFlags;
                    // response fires after object body damage crosses this threshold, numbers can be negative.  You need to set the flag
                    // "body threshold active" for this number to be used. 1=full health
                    public float BodyDamageThreshold;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag TransitionEffect;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag TransitionEffect1;
                    public InstantaneousResponseDamageEffectStruct DamageEffect;
                    public StringId Region;
                    public ModelStateEnum NewState;
                    public short RuntimeRegionIndex;
                    public StringId Region1;
                    public ModelStateEnum NewState1;
                    public short RuntimeRegionIndex1;
                    // all possible instances from this group will be destroyed
                    public short DestroyInstanceGroup;
                    public DamageResponseCustomResponseBehaviorEnum CustomResponseBehavior;
                    public StringId CustomResponseLabel;
                    public StringId EffectMarkerName;
                    public InstantaneousResponseDamageEffectMarkerStruct DamageEffectMarker;
                    // in seconds
                    public float ResponseDelay;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;
                    public float ResponseDelayPrematureDamageThreshold;
                    public StringId EjectingSeatLabel;
                    public float SkipFraction;
                    public StringId DestroyedChildObjectMarkerName;
                    public float TotalDamageThreshold;
                    
                    public enum DamageResponseTypeEnum : short
                    {
                        ReceivesAllDamage,
                        ReceivesAreaEffectDamage,
                        ReceivesLocalDamage
                    }
                    
                    public enum DamageResponseConstraintDamageTypeEnum : short
                    {
                        None,
                        // sets the attached body of this constraint free
                        DestroyOneOfGroup,
                        // sets the attached body of all constraints free
                        DestroyEntireGroup,
                        // takes this constraint out of the rigid state - activates it
                        LoosenOneOfGroup,
                        // takes all constraints out of the rigid state - activates them
                        LoosenEntireGroup
                    }
                    
                    [Flags]
                    public enum DamageResponseSet1 : uint
                    {
                        // when the response fires the object dies regardless of its current health
                        KillsObject = 1 << 0,
                        // from halo 1 - disallows melee for a unit
                        InhibitsMeleeAttack = 1 << 1,
                        // from halo 1 - disallows weapon fire for a unit
                        InhibitsWeaponAttack = 1 << 2,
                        // from halo 1 - disallows walking for a unit
                        InhibitsWalking = 1 << 3,
                        // from halo 1 - makes the unit drop its current weapon
                        ForcesDropWeapon = 1 << 4,
                        KillsWeaponPrimaryTrigger = 1 << 5,
                        KillsWeaponSecondaryTrigger = 1 << 6,
                        // when the response fires the object is destroyed
                        DestroysObject = 1 << 7,
                        // destroys the primary trigger on the unit's current weapon
                        DamagesWeaponPrimaryTrigger = 1 << 8,
                        // destroys the secondary trigger on the unit's current weapon
                        DamagesWeaponSecondaryTrigger = 1 << 9,
                        LightDamageLeftTurn = 1 << 10,
                        MajorDamageLeftTurn = 1 << 11,
                        LightDamageRightTurn = 1 << 12,
                        MajorDamageRightTurn = 1 << 13,
                        LightDamageEngine = 1 << 14,
                        MajorDamageEngine = 1 << 15,
                        KillsObject1 = 1 << 16,
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
                        CauseLeftLegDismemberment = 1u << 31
                    }
                    
                    [Flags]
                    public enum DamageResponseSet2 : uint
                    {
                        CauseRightLegDismemberment = 1 << 0,
                        CauseLeftArmDismemberment = 1 << 1,
                        CauseRightArmDismemberment = 1 << 2,
                        AllowDamageRechargeOverflow = 1 << 3
                    }
                    
                    [Flags]
                    public enum DamageResponseBodyThresholdFlags : uint
                    {
                        // this resoponse fires when the body healh fraction crosses this boundary
                        BodyThresholdActive = 1 << 0
                    }
                    
                    public enum ModelStateEnum : short
                    {
                        Default,
                        MinorDamage,
                        MediumDamage,
                        MajorDamage,
                        Destroyed
                    }
                    
                    public enum DamageResponseCustomResponseBehaviorEnum : short
                    {
                        PlaysAlways,
                        PlaysIfLabelsMatch,
                        PlaysIfLabelsDiffer
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class InstantaneousResponseDamageEffectStruct : TagStructure
                    {
                        [TagField(ValidTags = new [] { "jpt!" })]
                        public CachedTag TransitionDamageEffect;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class InstantaneousResponseDamageEffectMarkerStruct : TagStructure
                    {
                        public StringId DamageEffectMarkerName;
                    }
                }
                
                [TagStructure(Size = 0x0)]
                public class GNullBlock : TagStructure
                {
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalDamageNodesBlock : TagStructure
            {
                public short RuntimeDamagePart;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x20)]
            public class DamageSeatInfoBlock : TagStructure
            {
                public StringId SeatLabel;
                // 0==no damage, 1==full damage
                public float DirectDamageScale;
                public float DamageTransferFallOffRadius;
                public float MaximumTransferDamageScale;
                public float MinimumTransferDamageScale;
                public List<DamageSeatRegionSettingBlock> RegionSpecificDamage;
                
                [TagStructure(Size = 0x2C)]
                public class DamageSeatRegionSettingBlock : TagStructure
                {
                    public StringId DamageRegionName;
                    public short RuntimeDamageRegionIndex;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public float DirectDamageScale;
                    public float MaxTransferScale;
                    public float MinTransferScale;
                    public float DirectDamageScale1;
                    public float MaxTransferScale1;
                    public float MinTransferScale1;
                    public float DirectDamageScale2;
                    public float MaxTransferScale2;
                    public float MinTransferScale2;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageConstraintInfoBlock : TagStructure
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                public short RuntimeConstraintType;
                public short RuntimeConstraintIndex;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class ModelDamageInfoStruct : TagStructure
        {
            public NewModelDamageInfoFlags Flags;
            // value of zero implies 'damage sections' should be empty
            public float MaximumVitality;
            // absorbes AOE or child damage
            public StringId IndirectMaterialName;
            // absorbes AOE or child damage
            public short IndirectDamageSection;
            // the model's shielded/unshielded state reflects the depletion of this damage section
            public short ShieldedStateDamageSection;
            public GlobalDamageReportingEnum CollisionDamageReportingType;
            public GlobalDamageReportingEnum ResponseDamageReportingType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<NewGlobalDamageSectionBlock> DamageSections;
            public List<DamageConstraintInfoBlock> DamageConstraints;
            public List<GlobalDamageNodesBlock> Nodes;
            public short RuntimeIndirectMaterialType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum NewModelDamageInfoFlags : uint
            {
                // useful for bipeds shielding/taking damage for weapons
                TakesDamageForChildren = 1 << 0,
                // useful for attached shields
                ParentNeverTakesDamageForUs = 1 << 1,
                CannotDieFromDamage = 1 << 2,
                DiesWithRiders = 1 << 3,
                PassesAreaDamageToChildren = 1 << 4
            }
            
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
            
            [TagStructure(Size = 0xB4)]
            public class NewGlobalDamageSectionBlock : TagStructure
            {
                public StringId Name;
                public NewDamageSectionFlags Flags;
                // percentage of total object vitality
                public float VitalityPercentage; // [0.1]
                // set this to make this damage section a shield
                public StringId ShieldMaterialName;
                public float StunTime; // seconds
                // the minimum damage required to stun this object's health
                public float MinimumStunDamage;
                public float RechargeTime; // seconds
                public List<DamageSectionRechargeSpeedCurveBlock> RechargeSpeedCurve;
                public List<DamageSectionSegmentedRechargeFraction> RechargeFractions;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag RechargingEffect;
                // (main shield only) how long before the recharge begins the pre-recharge effect fires
                public float PreRechargeEffectWarnTime; // seconds
                // (main shield only)
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag PreRechargeEffect;
                // (main shield only)
                public StringId PreRechargeEffectMarker;
                // (main shield only) if the pre-recharge effect is aborted before the actual recharge starts, this effect plays
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag PreRechargeAbortEffect;
                // (main shield only)
                public StringId PreRechargeAbortEffectMarker;
                // time it takes to reach full "overcharge fraction"
                public float OverchargeTime; // seconds
                // fraction to which shields will automatically overcharge, values <= 1.0 are ignored
                public float OverchargeFraction;
                // time for this section to be active before it will start to decay
                public float PreDecayTime; // seconds
                // time for need for this section to fully decay with full health.
                public float DecayTime; // seconds
                public StringId ResurrectionRestoredRegionName;
                public List<NewInstantaneousDamageResponseBlock> InstantResponses;
                public List<DamageTransferBlock> SectionDamageTransfers;
                public List<DamageSectionRenderingParamters> Rendering;
                public float RuntimeRechargeVelocity;
                public float RuntimeOverchargeVelocity;
                public short RuntimeResurrectionRestoredRegionIndex;
                public short RuntimeGlobalShieldMaterialType;
                
                [Flags]
                public enum NewDamageSectionFlags : uint
                {
                    // this section will be initialized with zero health and will be stunned indefinitely
                    StartsInactive = 1 << 0,
                    TakesFullDmgWhenObjectDies = 1 << 1,
                    TakesFullDmgWhenObjDstryd = 1 << 2,
                    RestoredOnRessurection = 1 << 3,
                    // takes extra headshot damage when shot
                    Headshot = 1 << 4,
                    DepletionIsPermanent = 1 << 5,
                    RechargesWhileDead = 1 << 6,
                    PlayRechargeEffectOnlyWhenEmpty = 1 << 7,
                    Networked = 1 << 8,
                    // always a shield layer to recharge even if there is an inner shield layer that is stunned
                    CanRechargeIndependently = 1 << 9
                }
                
                [TagStructure(Size = 0x14)]
                public class DamageSectionRechargeSpeedCurveBlock : TagStructure
                {
                    public MappingFunction Mapping;
                    
                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class DamageSectionSegmentedRechargeFraction : TagStructure
                {
                    // recharge will stop once this percentage of vitality is reached.
                    public float VitalityPercentage; // [0.1]
                }
                
                [TagStructure(Size = 0xB4)]
                public class NewInstantaneousDamageResponseBlock : TagStructure
                {
                    public NewdamageResponseFlagsPart1 Flags;
                    public NewdamageResponseFlagsPart2 Flags2;
                    public StringId Label;
                    // response fires after crossing this threshold.  1=full health
                    public float DamageThreshold;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag TransitionEffect;
                    public StringId GenericEffectMarker;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag TransitionEffect1;
                    public StringId SpecificEffectMarker;
                    public InstantaneousResponseDamageEffectStruct DamageEffect;
                    public InstantaneousResponseDamageEffectMarkerStruct DamageEffectMarker;
                    // will play until the next response is triggered.
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag LoopingEffect;
                    public List<DamageResponseRegionTransitionBlock> RegionTransitions;
                    public List<DamageTransferBlock> ResponseDamageTransfers;
                    // all possible instances from this group will be destroyed
                    public short DestroyInstanceGroup;
                    public DamageResponseCustomResponseBehaviorEnum CustomResponseBehavior;
                    public StringId CustomResponseLabel;
                    // time to wait until firing the response. This delay is pre-empted if another timed response for the same section
                    // fires.
                    public float ResponseDelay; // seconds
                    // plays while the timer is counting down
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;
                    public List<SeatEjectionBlock> SeatEject;
                    // 0.0 always fires, 1.0 never fires
                    public float SkipFraction;
                    // when this response fires, any children objects created at the supplied marker name will be destroyed
                    public StringId DestroyedChildObjectMarkerName;
                    // scale on total damage section vitality
                    public float TotalDamageThreshold;
                    // can specify a randomly-selected single constraint or the entire group of named constraints
                    public StringId ConstraintGroupName;
                    public DamageResponseConstraintDamageTypeEnum ConstraintDamageType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [Flags]
                    public enum NewdamageResponseFlagsPart1 : uint
                    {
                        // when the response fires the object dies regardless of its current health
                        KillsObject = 1 << 0,
                        // from halo 1 - disallows melee for a unit
                        InhibitsMeleeAttack = 1 << 1,
                        // from halo 1 - disallows weapon fire for a unit
                        InhibitsWeaponAttack = 1 << 2,
                        // from halo 1 - disallows walking for a unit
                        InhibitsWalking = 1 << 3,
                        // from halo 1 - makes the unit drop its current weapon
                        ForcesDropWeapon = 1 << 4,
                        KillsWeaponPrimaryTrigger = 1 << 5,
                        KillsWeaponSecondaryTrigger = 1 << 6,
                        // when the response fires the object is destroyed
                        DestroysObject = 1 << 7,
                        // destroys the primary trigger on the unit's current weapon
                        DamagesWeaponPrimaryTrigger = 1 << 8,
                        // destroys the secondary trigger on the unit's current weapon
                        DamagesWeaponSecondaryTrigger = 1 << 9,
                        KillsObject1 = 1 << 10,
                        CausesDetonation = 1 << 11,
                        KillsVariantObjects = 1 << 12,
                        ForceUnattachedEffects = 1 << 13,
                        FiresUnderThreshold = 1 << 14,
                        TriggersSpecialDeath = 1 << 15,
                        OnlyOnSpecialDeath = 1 << 16,
                        OnlyNotOnSpecialDeath = 1 << 17,
                        BucklesGiants = 1 << 18,
                        CausesSpDetonation = 1 << 19,
                        SkipSoundsOnGenericEffect = 1 << 20,
                        KillsGiants = 1 << 21,
                        SkipSoundsOnSpecialDeath = 1 << 22,
                        ForceHardPing = 1 << 23,
                        // can fire again if the section ever recharges past its threshold
                        CanRefire = 1 << 24,
                        // will not spawn effects if previous response also fired
                        CanSkipTransitionEffects = 1 << 25,
                        CauseHeadDismemberment = 1 << 26,
                        CauseLeftLegDismemberment = 1 << 27,
                        CauseRightLegDismemberment = 1 << 28,
                        CauseLeftArmDismemberment = 1 << 29,
                        CauseRightArmDismemberment = 1 << 30,
                        AllowDamageRechargeOverflow = 1u << 31
                    }
                    
                    [Flags]
                    public enum NewdamageResponseFlagsPart2 : uint
                    {
                        // if the destroys object flag and this flag are both checked, the object will be hidden on predicted clients when the
                        // response fires (hologram hack)
                        HidesObjectIfDestroyedByResponseOnPredictedClient = 1 << 0
                    }
                    
                    public enum DamageResponseCustomResponseBehaviorEnum : short
                    {
                        PlaysAlways,
                        PlaysIfLabelsMatch,
                        PlaysIfLabelsDiffer
                    }
                    
                    public enum DamageResponseConstraintDamageTypeEnum : short
                    {
                        None,
                        // sets the attached body of this constraint free
                        DestroyOneOfGroup,
                        // sets the attached body of all constraints free
                        DestroyEntireGroup,
                        // takes this constraint out of the rigid state - activates it
                        LoosenOneOfGroup,
                        // takes all constraints out of the rigid state - activates them
                        LoosenEntireGroup
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class InstantaneousResponseDamageEffectStruct : TagStructure
                    {
                        [TagField(ValidTags = new [] { "jpt!" })]
                        public CachedTag TransitionDamageEffect;
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class InstantaneousResponseDamageEffectMarkerStruct : TagStructure
                    {
                        public StringId DamageEffectMarkerName;
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class DamageResponseRegionTransitionBlock : TagStructure
                    {
                        public StringId Region;
                        public ModelStateEnum NewState;
                        public short RuntimeRegionIndex;
                        
                        public enum ModelStateEnum : short
                        {
                            Default,
                            MinorDamage,
                            MediumDamage,
                            MajorDamage,
                            Destroyed
                        }
                    }
                    
                    [TagStructure(Size = 0x10)]
                    public class DamageTransferBlock : TagStructure
                    {
                        public DamageTransferFlags Flags;
                        public float TransferAmount;
                        public DamageTransferFunctionEnum TransferFunction;
                        public short DamageSection;
                        public StringId SeatLabel;
                        
                        [Flags]
                        public enum DamageTransferFlags : uint
                        {
                            TransferDamageToDamageSection = 1 << 0,
                            TransferDamageToParent = 1 << 1,
                            TransferDamageToChildren = 1 << 2,
                            TransferDamageToSeats = 1 << 3,
                            TransferDirectDamage = 1 << 4,
                            TransferAoeExposedDamage = 1 << 5,
                            TransferAoeObstructedDamage = 1 << 6
                        }
                        
                        public enum DamageTransferFunctionEnum : short
                        {
                            Percent,
                            Points,
                            Ceiling
                        }
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class SeatEjectionBlock : TagStructure
                    {
                        public StringId SeatLabel;
                    }
                }
                
                [TagStructure(Size = 0x10)]
                public class DamageTransferBlock : TagStructure
                {
                    public DamageTransferFlags Flags;
                    public float TransferAmount;
                    public DamageTransferFunctionEnum TransferFunction;
                    public short DamageSection;
                    public StringId SeatLabel;
                    
                    [Flags]
                    public enum DamageTransferFlags : uint
                    {
                        TransferDamageToDamageSection = 1 << 0,
                        TransferDamageToParent = 1 << 1,
                        TransferDamageToChildren = 1 << 2,
                        TransferDamageToSeats = 1 << 3,
                        TransferDirectDamage = 1 << 4,
                        TransferAoeExposedDamage = 1 << 5,
                        TransferAoeObstructedDamage = 1 << 6
                    }
                    
                    public enum DamageTransferFunctionEnum : short
                    {
                        Percent,
                        Points,
                        Ceiling
                    }
                }
                
                [TagStructure(Size = 0x20)]
                public class DamageSectionRenderingParamters : TagStructure
                {
                    [TagField(ValidTags = new [] { "shit" })]
                    public CachedTag _ThirdPersonImpactParameters;
                    [TagField(ValidTags = new [] { "shit" })]
                    public CachedTag _FirstPersonImpactParameters;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageConstraintInfoBlock : TagStructure
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                public short RuntimeConstraintType;
                public short RuntimeConstraintIndex;
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalDamageNodesBlock : TagStructure
            {
                public short RuntimeDamagePart;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ModelTargetBlockOld : TagStructure
        {
            // multiple markers become multiple spheres of the same radius
            public StringId MarkerName;
            // sphere radius
            public float Size;
            // the target is only visible when viewed within this angle of the marker's x axis
            public Angle ConeAngle;
            // the target is associated with this damage section
            public short DamageSection;
            // the target will only appear with this variant
            public short Variant;
            // higher relevances turn into stronger magnetisms
            public float TargetingRelevance;
            // ignored if zero
            public float AoeExclusionRadius;
            public ModelTargetLockOnDataStruct LockOnData;
            
            [TagStructure(Size = 0xC)]
            public class ModelTargetLockOnDataStruct : TagStructure
            {
                public ModelTargetLockOnFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float LockOnDistance;
                // a weapon can track/lock on this target if this string is in the weapon's tracking block
                public StringId TrackingType;
                
                [Flags]
                public enum ModelTargetLockOnFlags : byte
                {
                    Headshot = 1 << 0,
                    Vulnerable = 1 << 1,
                    IgnoredOnLocalPhysics = 1 << 2,
                    UseForNetworkLeadVectorOnly = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ModelTargetBlockNew : TagStructure
        {
            public ModelTargetFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // multiple markers become multiple spheres of the same radius
            public StringId MarkerName;
            // sphere radius
            public float Size;
            // the target is only visible when viewed within this angle of the marker's x axis
            public Angle ConeAngle;
            // the target is associated with this damage section
            public short DamageSection;
            // the target will only appear with this variant
            public short Variant;
            // higher relevances turn into stronger magnetisms
            public float TargetingRelevance;
            // ignored if zero
            public float AoeExclusionRadius;
            public ModelTargetLockOnDataStruct LockOnData;
            
            [Flags]
            public enum ModelTargetFlags : byte
            {
                AoeTopLevel = 1 << 0,
                AoeTestObstruction = 1 << 1,
                // use this model targets center for displaying the targetting reticle
                ShowsTrackingReticle = 1 << 2
            }
            
            [TagStructure(Size = 0xC)]
            public class ModelTargetLockOnDataStruct : TagStructure
            {
                public ModelTargetLockOnFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float LockOnDistance;
                // a weapon can track/lock on this target if this string is in the weapon's tracking block
                public StringId TrackingType;
                
                [Flags]
                public enum ModelTargetLockOnFlags : byte
                {
                    Headshot = 1 << 0,
                    Vulnerable = 1 << 1,
                    IgnoredOnLocalPhysics = 1 << 2,
                    UseForNetworkLeadVectorOnly = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelRegionBlock : TagStructure
        {
            public StringId Name;
            public sbyte CollisionRegionIndex;
            public sbyte PhysicsRegionIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<ModelPermutationBlock> Permutations;
            
            [TagStructure(Size = 0x8)]
            public class ModelPermutationBlock : TagStructure
            {
                public StringId Name;
                public ModelPermutationFlags Flags;
                public sbyte CollisionPermutationIndex;
                public sbyte PhysicsPermutationIndex;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum ModelPermutationFlags : byte
                {
                    CannotBeChosenRandomly = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class ModelNodeBlock : TagStructure
        {
            public StringId Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float DefaultInverseScale;
            public RealVector3d DefaultInverseForward;
            public RealVector3d DefaultInverseLeft;
            public RealVector3d DefaultInverseUp;
            public RealPoint3d DefaultInversePosition;
        }
        
        [TagStructure(Size = 0x4)]
        public class GNodeFlagStorageArray : TagStructure
        {
            public int FlagData;
        }
        
        [TagStructure(Size = 0x44)]
        public class GlobalScenarioLoadParametersBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag Scenario;
            public byte[] Parameters;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelGameModeRenderModelOverride : TagStructure
        {
            public ModelGameModeTypes GameMode;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag RenderModelOverride;
            
            public enum ModelGameModeTypes : short
            {
                Campaign,
                Multiplayer,
                Firefight,
                Mainmenu
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class ModelSelfShadowRegionCastOverrideBlock : TagStructure
        {
            public StringId Region;
            // leave blank for none
            public StringId ShadowCastPermutation;
        }
        
        [TagStructure(Size = 0x8)]
        public class ModelSelfShadowRegionReceiveOverrideBlock : TagStructure
        {
            public StringId Region;
            public ModelPrtShadowReceiveModeDefinition ShadowType;
            
            public enum ModelPrtShadowReceiveModeDefinition : int
            {
                PrtShadowsFromAllRegions,
                PrtSelfShadowOnly,
                NoPrtShadowsAtAll
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelOcclusionSphereBlock : TagStructure
        {
            public StringId Marker1Name;
            public int Marker1Index;
            public StringId Marker2Name;
            public int Marker2Index;
            public float Radius;
        }
    }
}
