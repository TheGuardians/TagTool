using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0xFC)]
    public class Model : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag RenderModel;
        [TagField(ValidTags = new [] { "coll" })]
        public CachedTag CollisionModel;
        [TagField(ValidTags = new [] { "jmad" })]
        public CachedTag Animation;
        [TagField(ValidTags = new [] { "phys" })]
        public CachedTag Physics;
        [TagField(ValidTags = new [] { "phmo" })]
        public CachedTag PhysicsModel;
        /// <summary>
        /// If a model is further away than the LOD reduction distance, it will be reduced to that LOD.
        /// So the L1 reduction distance
        /// should be really long and the L5 reduction distance should be really short.
        /// This means distances should be in descending
        /// order, e.g. 5 4 3 2 1.
        /// 
        /// Note that if we run out of memory or too many models are on screen at once, the engine may
        /// reduce
        /// models to a lower LOD BEFORE they reach the reduction distance for that LOD.
        /// 
        /// If a model has a begin fade distance
        /// and disappear distance, it will begin fading out at that distance,
        /// reaching zero alpha and disappearing at the disappear
        /// distance. These distances should be greater than all
        /// of the LOD reduction distances.
        /// 
        /// </summary>
        public float DisappearDistance; // world units
        public float BeginFadeDistance; // world units
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float ReduceToL1; // world units (super low)
        public float ReduceToL2; // world units (low)
        public float ReduceToL3; // world units (medium)
        public float ReduceToL4; // world units (high)
        public float ReduceToL5; // world units (super high)
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public ShadowFadeDistanceValue ShadowFadeDistance;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<ModelVariantBlock> Variants;
        public List<ModelMaterialBlock> Materials;
        public List<GlobalDamageInfoBlock> NewDamageInfo;
        public List<ModelTargetBlock> Targets;
        public List<ModelRegionBlock> CollisionRegions;
        public List<ModelNodeBlock> Nodes;
        public int RuntimeNodeListChecksum;
        public List<ModelObjectDataBlock> ModelObjectData;
        /// <summary>
        /// The default dialogue tag for this model (overriden by variants)
        /// </summary>
        [TagField(ValidTags = new [] { "udlg" })]
        public CachedTag DefaultDialogue;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag Unused;
        public FlagsValue Flags;
        /// <summary>
        /// The default dialogue tag for this model (overriden by variants)
        /// </summary>
        public StringId DefaultDialogueEffect;
        [TagField(Length = 8)]
        public int[] RenderOnlyNodeFlags = new int[8];
        [TagField(Length = 8)]
        public int[] RenderOnlySectionFlags = new int[8];

        public RuntimeFlagsValue RuntimeFlags;
        public List<GlobalScenarioLoadParametersBlock> ScenarioLoadParameters;
        /// <summary>
        /// hologram shader is applied whenever the control function from it's object is non-zero
        /// </summary>
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag HologramShader;
        public StringId HologramControlFunction;
        
        public enum ShadowFadeDistanceValue : short
        {
            FadeAtSuperHighDetailLevel,
            FadeAtHighDetailLevel,
            FadeAtMediumDetailLevel,
            FadeAtLowDetailLevel,
            FadeAtSuperLowDetailLevel,
            FadeNever
        }
        
        [TagStructure(Size = 0x38)]
        public class ModelVariantBlock : TagStructure
        {
            public StringId Name;
            [TagField(Length = 16)]
            public sbyte[] ModelRegionIndices = new sbyte[16];
            public List<ModelVariantRegionBlock> Regions;
            public List<ModelVariantObjectBlock> Objects;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public StringId DialogueSoundEffect;
            [TagField(ValidTags = new [] { "udlg" })]
            public CachedTag Dialogue;
            
            [TagStructure(Size = 0x14)]
            public class ModelVariantRegionBlock : TagStructure
            {
                public StringId RegionName; // must match region name in render_model
                public sbyte ModelRegionIndex;
                public sbyte RuntimeFlags;
                public short ParentVariant;
                public List<ModelVariantPermutationBlock> Permutations;
                /// <summary>
                /// negative values mean closer to the camera
                /// </summary>
                public SortOrderValue SortOrder;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;

                [TagStructure(Size = 0x20)]
                public class ModelVariantPermutationBlock : TagStructure
                {
                    public StringId PermutationName;
                    public sbyte ModelPermutationIndex;
                    public FlagsValue Flags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    public float Probability; // (0,+inf)
                    public List<ModelVariantStateBlock> States;
                    [TagField(Length = 12)]
                    public sbyte[] RuntimeStatePermutationIndices = new sbyte[12];

                    [Flags]
                    public enum FlagsValue : byte
                    {
                        CopyStatesToAllPermutations = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x18)]
                    public class ModelVariantStateBlock : TagStructure
                    {
                        public StringId Name;
                        public sbyte ModelPermutationIndex;
                        public PropertyFlagsValue PropertyFlags;
                        public StateValue State2;
                        /// <summary>
                        /// played while the model is in this state
                        /// </summary>
                        [TagField(ValidTags = new [] { "effe" })]
                        public CachedTag LoopingEffect;
                        public StringId LoopingEffectMarkerName;
                        public float InitialProbability;

                        [Flags]
                        public enum PropertyFlagsValue : byte
                        {
                            Blurred = 1 << 0,
                            HellaBlurred = 1 << 1,
                            Shielded = 1 << 2,
                            BatteryDepleted = 1 << 3
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
                
                public enum SortOrderValue : short
                {
                    NoSorting,
                    _5Closest,
                    _4,
                    _3,
                    _2,
                    _1,
                    _0SameAsModel,
                    _11,
                    _21,
                    _31,
                    _41,
                    _5Farthest
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ModelVariantObjectBlock : TagStructure
            {
                public StringId ParentMarker;
                public StringId ChildMarker;
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag ChildObject;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelMaterialBlock : TagStructure
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding = new byte[2];

            public enum MaterialTypeValue : short
            {
                Dirt,
                Sand,
                Stone,
                Snow,
                Wood,
                MetalHollow,
                MetalThin,
                MetalThick,
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
        
        [TagStructure(Size = 0xF8)]
        public class GlobalDamageInfoBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// absorbes AOE or child damage
            /// </summary>
            public StringId GlobalIndirectMaterialName;
            /// <summary>
            /// absorbes AOE or child damage
            /// </summary>
            public short IndirectDamageSection;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public Damage.DamageReportingType CollisionDamageReportingType;
            public Damage.DamageReportingType ResponseDamageReportingType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public float MaximumVitality;
            /// <summary>
            /// the minimum damage required to stun this object's health
            /// </summary>
            public float MinimumStunDamage;
            /// <summary>
            /// the length of time the health stay stunned (do not recharge) after taking damage
            /// </summary>
            public float StunTime; // seconds
            /// <summary>
            /// the length of time it would take for the shields to fully recharge after being completely depleted
            /// </summary>
            public float RechargeTime; // seconds
            /// <summary>
            /// 0 defaults to 1 - to what maximum level the body health will be allowed to recharge
            /// </summary>
            public float RechargeFraction;
            [TagField(Length = 64)]
            public byte[] UnknownRuntimeValues = new byte[64];
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag ShieldDamagedFirstPersonShader;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag ShieldDamagedShader;
            /// <summary>
            /// the default initial and maximum shield vitality of this object
            /// </summary>
            public float MaximumShieldVitality;
            public StringId GlobalShieldMaterialName;
            /// <summary>
            /// the minimum damage required to stun this object's shields
            /// </summary>
            public float MinimumStunDamage1;
            /// <summary>
            /// the length of time the shields stay stunned (do not recharge) after taking damage
            /// </summary>
            public float StunTime1; // seconds
            /// <summary>
            /// the length of time it would take for the shields to fully recharge after being completely depleted
            /// </summary>
            public float RechargeTime1; // seconds
            public float ShieldDamagedThreshold;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ShieldDamagedEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ShieldDepletedEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ShieldRechargingEffect;
            public List<GlobalDamageSectionBlock> DamageSections;
            public List<GlobalDamageNodesBlock> Nodes;
            [TagField(Flags = GlobalMaterial)]
            public short GlobalShieldMaterialIndex;
            [TagField(Flags = GlobalMaterial)]
            public short GlobalIndirectMaterialIndex;
            public float RuntimeShieldRechargeVelocity;
            public float RuntimeHealthRechargeVelocity;
            public List<DamageSeatInfoBlock> DamageSeats;
            public List<DamageConstraintInfoBlock> DamageConstraints;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag OvershieldFirstPersonShader;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag OvershieldShader;
            
            [Flags]
            public enum FlagsValue : uint
            {
                TakesShieldDamageForChildren = 1 << 0,
                TakesBodyDamageForChildren = 1 << 1,
                AlwaysShieldsFriendlyDamage = 1 << 2,
                PassesAreaDamageToChildren = 1 << 3,
                ParentNeverTakesBodyDamageForUs = 1 << 4,
                OnlyDamagedByExplosives = 1 << 5,
                ParentNeverTakesShieldDamageForUs = 1 << 6,
                CannotDieFromDamage = 1 << 7,
                PassesAttachedDamageToRiders = 1 << 8
            }
            
            public enum CollisionDamageReportingTypeValue : sbyte
            {
                TehGuardians11,
                FallingDamage,
                GenericCollisionDamage,
                GenericMeleeDamage,
                GenericExplosion,
                MagnumPistol,
                PlasmaPistol,
                Needler,
                Smg,
                PlasmaRifle,
                BattleRifle,
                Carbine,
                Shotgun,
                SniperRifle,
                BeamRifle,
                RocketLauncher,
                FlakCannon,
                BruteShot,
                Disintegrator,
                BrutePlasmaRifle,
                EnergySword,
                FragGrenade,
                PlasmaGrenade,
                FlagMeleeDamage,
                BombMeleeDamage,
                BombExplosionDamage,
                BallMeleeDamage,
                HumanTurret,
                PlasmaTurret,
                Banshee,
                Ghost,
                Mongoose,
                Scorpion,
                SpectreDriver,
                SpectreGunner,
                WarthogDriver,
                WarthogGunner,
                Wraith,
                Tank,
                SentinelBeam,
                SentinelRpg,
                Teleporter
            }
            
            public enum ResponseDamageReportingTypeValue : sbyte
            {
                TehGuardians11,
                FallingDamage,
                GenericCollisionDamage,
                GenericMeleeDamage,
                GenericExplosion,
                MagnumPistol,
                PlasmaPistol,
                Needler,
                Smg,
                PlasmaRifle,
                BattleRifle,
                Carbine,
                Shotgun,
                SniperRifle,
                BeamRifle,
                RocketLauncher,
                FlakCannon,
                BruteShot,
                Disintegrator,
                BrutePlasmaRifle,
                EnergySword,
                FragGrenade,
                PlasmaGrenade,
                FlagMeleeDamage,
                BombMeleeDamage,
                BombExplosionDamage,
                BallMeleeDamage,
                HumanTurret,
                PlasmaTurret,
                Banshee,
                Ghost,
                Mongoose,
                Scorpion,
                SpectreDriver,
                SpectreGunner,
                WarthogDriver,
                WarthogGunner,
                Wraith,
                Tank,
                SentinelBeam,
                SentinelRpg,
                Teleporter
            }
            
            [TagStructure(Size = 0x38)]
            public class GlobalDamageSectionBlock : TagStructure
            {
                public StringId Name;
                /// <summary>
                /// * absorbs body damage: damage to this section does not count against body vitality
                /// * headshottable: takes extra headshot
                /// damage when shot
                /// * ignores shields: damage to this section bypasses shields
                /// </summary>
                public FlagsValue Flags;
                /// <summary>
                /// percentage of total object vitality
                /// </summary>
                public float VitalityPercentage; // [0.1]
                public List<InstantaneousDamageRepsonseBlock> InstantResponses;
                public List<GNullBlock> Unknown;
                public List<GNullBlock1> Unknown1;
                public float StunTime; // seconds
                public float RechargeTime; // seconds
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId ResurrectionRestoredRegionName;
                public short ResurrectionRegionRuntimeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;

                [Flags]
                public enum FlagsValue : int
                {
                    None,
                    AbsorbsBodyDamage = 1 << 0,
                    TakesFullDamageWhenObjectDies = 1 << 1,
                    CannotDieWithRiders = 1 << 2,
                    TakesFullDamageWhenObjectDestroyed = 1 << 3,
                    RestoredOnRessurection = 1 << 4,
                    Unused = 1 << 5,
                    Unused1 = 1 << 6,
                    Heatshottable = 1 << 7,
                    IgnoresShields = 1 << 8
                }

                
                [TagStructure(Size = 0x50)]
                public class InstantaneousDamageRepsonseBlock : TagStructure
                {
                    public ResponseTypeValue ResponseType;
                    /// <summary>
                    /// * if you specify a constraint group name (see lower section of this block)
                    ///   you can specify a constraint damage
                    /// *
                    /// loosening a constraint takes it out of the rigid state - activates it
                    /// * destroying a constraint sets the attached body
                    /// free
                    /// </summary>
                    public ConstraintDamageTypeValue ConstraintDamageType;
                    /// <summary>
                    /// * kills object: when the response fires the object dies regardless of its current health
                    /// * inhibits x: from halo 1 -
                    /// disallows basic behaviors for a unit
                    /// * forces drop weapon: from halo 1 - makes the unit drop its current weapon
                    /// * kills
                    /// weapon x trigger: destroys the x trigger on the unit's current weapon
                    /// * destroys object: when the response fires the
                    /// object is destroyed
                    /// </summary>
                    public FlagsValue Flags;
                    /// <summary>
                    /// repsonse fires after crossing this threshold.  1=full health
                    /// </summary>
                    public float DamageThreshold;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag TransitionEffect;
                    public InstantaneousResponseDamageEffectStructBlock DamageEffect;
                    public StringId Region;
                    public NewStateValue NewState;
                    public short RuntimeRegionIndex;
                    public StringId EffectMarkerName;
                    public InstantaneousResponseDamageEffectMarkerStructBlock DamageEffectMarker;
                    /// <summary>
                    /// If desired, you can specify a delay until the response fires.This delay is pre-empted if another timed response for the
                    /// same section fires.The delay effect plays while the timer is counting down
                    /// </summary>
                    /// <summary>
                    /// in seconds
                    /// </summary>
                    public float ResponseDelay;
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;
                    /// <summary>
                    /// - a response can destroy a single constraint by naming it explicitly.
                    /// - alternatively it can randomly destroy a single
                    /// constraint from a specified group if the "destroy one group constraint" flag is set
                    /// - also it can destroy all constraints
                    /// in a specified group if the "destroy all group constraints" flag is set
                    /// 
                    /// </summary>
                    public StringId ConstraintGroupName;
                    public StringId EjectingSeatLabel;
                    /// <summary>
                    /// 0.0 always fires, 1.0 never fires
                    /// </summary>
                    public float SkipFraction;
                    /// <summary>
                    /// when this response fires, any children objects created at the supplied marker name will be destroyed
                    /// </summary>
                    public StringId DestroyedChildObjectMarkerName;
                    /// <summary>
                    /// scale on total damage section vitality
                    /// </summary>
                    public float TotalDamageThreshold;
                    
                    public enum ResponseTypeValue : short
                    {
                        ReceivesAllDamage,
                        ReceivesAreaEffectDamage,
                        ReceivesLocalDamage
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
                    public enum FlagsValue : uint
                    {
                        KillsObject = 1 << 0,
                        InhibitsMeleeAttack = 1 << 1,
                        InhibitsWeaponAttack = 1 << 2,
                        InhibitsWalking = 1 << 3,
                        ForcesDropWeapon = 1 << 4,
                        KillsWeaponPrimaryTrigger = 1 << 5,
                        KillsWeaponSecondaryTrigger = 1 << 6,
                        DestroysObject = 1 << 7,
                        DamagesWeaponPrimaryTrigger = 1 << 8,
                        DamagesWeaponSecondaryTrigger = 1 << 9,
                        LightDamageLeftTurn = 1 << 10,
                        MajorDamageLeftTurn = 1 << 11,
                        LightDamageRightTurn = 1 << 12,
                        MajorDamageRightTurn = 1 << 13,
                        LightDamageEngine = 1 << 14,
                        MajorDamageEngine = 1 << 15,
                        KillsObjectNoPlayerSolo = 1 << 16,
                        CausesDetonation = 1 << 17,
                        DestroyAllGroupConstraints = 1 << 18,
                        KillsVariantObjects = 1 << 19,
                        ForceUnattachedEffects = 1 << 20,
                        FiresUnderThreshold = 1 << 21,
                        TriggersSpecialDeath = 1 << 22,
                        OnlyOnSpecialDeath = 1 << 23,
                        OnlyNotOnSpecialDeath = 1 << 24
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class InstantaneousResponseDamageEffectStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "jpt!" })]
                        public CachedTag TransitionDamageEffect;
                    }
                    
                    public enum NewStateValue : short
                    {
                        Default,
                        MinorDamage,
                        MediumDamage,
                        MajorDamage,
                        Destroyed
                    }
                    
                    [TagStructure(Size = 0x4)]
                    public class InstantaneousResponseDamageEffectMarkerStructBlock : TagStructure
                    {
                        public StringId DamageEffectMarkerName;
                    }
                }
                
                [TagStructure()]
                public class GNullBlock : TagStructure
                {
                }
                
                [TagStructure()]
                public class GNullBlock1 : TagStructure
                {
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalDamageNodesBlock : TagStructure
            {
                public short RuntimeDamagePart;
                public short Unknown;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageSeatInfoBlock : TagStructure
            {
                public StringId SeatLabel;
                /// <summary>
                /// 0==no damage, 1==full damage
                /// </summary>
                public float DirectDamageScale;
                public float DamageTransferFallOffRadius;
                public float MaximumTransferDamageScale;
                public float MinimumTransferDamageScale;
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageConstraintInfoBlock : TagStructure
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class ModelTargetBlock : TagStructure
        {
            /// <summary>
            /// multiple markers become multiple spheres of the same radius
            /// </summary>
            public StringId MarkerName;
            /// <summary>
            /// sphere radius
            /// </summary>
            public float Size;
            /// <summary>
            /// the target is only visible when viewed within this angle of the marker's x axis
            /// </summary>
            public Angle ConeAngle;
            /// <summary>
            /// the target is associated with this damage section
            /// </summary>
            public short DamageSection;
            /// <summary>
            /// the target will only appear with this variant
            /// </summary>
            public short Variant;
            /// <summary>
            /// higher relevances turn into stronger magnetisms
            /// </summary>
            public float TargetingRelevance;
            public ModelTargetLockOnDataStructBlock LockOnData;
            
            [TagStructure(Size = 0x8)]
            public class ModelTargetLockOnDataStructBlock : TagStructure
            {
                public FlagsValue Flags;
                public float LockOnDistance;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    LockedByHumanTracking = 1 << 0,
                    LockedByPlasmaTracking = 1 << 1,
                    Headshot = 1 << 2,
                    Vulnerable = 1 << 3,
                    AlwasLockedByPlasmaTracking = 1 << 4
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
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
                public FlagsValue Flags;
                public sbyte CollisionPermutationIndex;
                public sbyte PhysicsRegionIndex;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;

                [Flags]
                public enum FlagsValue : byte
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
            public short ImportNodeIndex;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float DefaultScale;
            public RealMatrix4x3 Inverse;
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelObjectDataBlock : TagStructure
        {
            public TypeValue Type;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        public enum FlagsValue : uint
        {
            ActiveCamoAlwaysOn = 1 << 0,
            ActiveCamoAlwaysMerge = 1 << 1,
            ActiveCamoNeverMerge = 1 << 2
        }
        
        [Flags]
        public enum RuntimeFlagsValue : uint
        {
            ContainsRunTimeNodes = 1 << 0
        }
        
        [TagStructure(Size = 0x30)]
        public class GlobalScenarioLoadParametersBlock : TagStructure
        {
            /// <summary>
            /// strip-variant variant-name
            /// strips a given variant out of the model tag
            /// strip-dialogue
            /// strips all the dialogue for this
            /// model i.e. cinematic only
            /// </summary>
            [TagField(ValidTags = new [] { "scnr" })]
            public CachedTag Scenario;
            public byte[] Parameters;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}

