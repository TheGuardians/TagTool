using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "model", Tag = "hlmt", Size = 0x15C)]
    public class Model : TagStructure
    {
        /// <summary>
        /// MODEL
        /// </summary>
        public CachedTag RenderModel;
        public CachedTag CollisionModel;
        public CachedTag Animation;
        public CachedTag Physics;
        public CachedTag PhysicsModel;
        /// <summary>
        /// level of detail
        /// </summary>
        /// <remarks>
        /// If a model is further away than the LOD reduction distance, it will be reduced to that LOD.
        /// So the L1 reduction distance should be really long and the L5 reduction distance should be really short.
        /// This means distances should be in descending order, e.g. 5 4 3 2 1.
        /// 
        /// Note that if we run out of memory or too many models are on screen at once, the engine may reduce
        /// models to a lower LOD BEFORE they reach the reduction distance for that LOD.
        /// 
        /// If a model has a begin fade distance and disappear distance, it will begin fading out at that distance,
        /// reaching zero alpha and disappearing at the disappear distance. These distances should be greater than all
        /// of the LOD reduction distances.
        /// 
        /// </remarks>
        public float DisappearDistance; // world units
        public float BeginFadeDistance; // world units
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public float ReduceToL1; // world units (super low)
        public float ReduceToL2; // world units (low)
        public float ReduceToL3; // world units (medium)
        public float ReduceToL4; // world units (high)
        public float ReduceToL5; // world units (super high)
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unknown2;
        public ShadowFadeDistanceValue ShadowFadeDistance;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public List<ModelVariant> Variants;
        public List<ModelMaterial> Materials;
        public List<ModelDamageInfo> NewDamageInfo;
        public List<ModelTarget> Targets;
        public List<ModelRegion> Unknown3;
        public List<ModelNode> Unknown4;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public List<ModelObjectDataBlock> ModelObjectData;
        /// <summary>
        /// more stuff
        /// </summary>
        public CachedTag DefaultDialogue; // The default dialogue tag for this model (overriden by variants)
        public CachedTag Unused;
        public FlagsValue Flags;
        public StringId DefaultDialogueEffect; // The default dialogue tag for this model (overriden by variants)
        public sbyte Unknown5;
        [TagField(Length = 32)]
        public sbyte RenderOnlyNodeFlags;
        public sbyte Unknown7;
        [TagField(Length = 32)]
        public sbyte RenderOnlySectionFlags;
        public RuntimeFlagsValue RuntimeFlags;
        public List<ScenarioLoadParametersBlock> ScenarioLoadParameters;
        /// <summary>
        /// HOLOGRAM
        /// </summary>
        /// <remarks>
        /// hologram shader is applied whenever the control function from it's object is non-zero
        /// </remarks>
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
        
        [TagStructure(Size = 0x48)]
        public class ModelVariant : TagStructure
        {
            public StringId Name;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public List<ModelVariantRegion> Regions;
            public List<ModelVariantObject> Objects;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            public StringId DialogueSoundEffect;
            public CachedTag Dialogue;
            
            [TagStructure(Size = 0x18)]
            public class ModelVariantRegion : TagStructure
            {
                public StringId RegionName; // must match region name in render_model
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding2;
                public short ParentVariant;
                public List<ModelVariantPermutation> Permutations;
                public SortOrderValue SortOrder; // negative values mean closer to the camera
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                
                [TagStructure(Size = 0x24)]
                public class ModelVariantPermutation : TagStructure
                {
                    public StringId PermutationName;
                    [TagField(Flags = Padding, Length = 1)]
                    public byte[] Padding1;
                    public FlagsValue Flags;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding2;
                    public float Probability; // (0,+inf)
                    public List<ModelVariantState> States;
                    [TagField(Flags = Padding, Length = 5)]
                    public byte[] Padding3;
                    [TagField(Flags = Padding, Length = 7)]
                    public byte[] Padding4;
                    
                    [Flags]
                    public enum FlagsValue : byte
                    {
                        CopyStatesToAllPermutations = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x20)]
                    public class ModelVariantState : TagStructure
                    {
                        public StringId PermutationName;
                        [TagField(Flags = Padding, Length = 1)]
                        public byte[] Padding1;
                        public PropertyFlagsValue PropertyFlags;
                        public StateValue State;
                        public CachedTag LoopingEffect; // played while the model is in this state
                        public StringId LoopingEffectMarkerName;
                        public float InitialProbability;
                        
                        [Flags]
                        public enum PropertyFlagsValue : byte
                        {
                            Blurred = 1 << 0,
                            HellaBlurred = 1 << 1,
                            Shielded = 1 << 2
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
                    _10,
                    _21,
                    _32,
                    _43,
                    _5Farthest
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class ModelVariantObject : TagStructure
            {
                public StringId ParentMarker;
                public StringId ChildMarker;
                public CachedTag ChildObject;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelMaterial : TagStructure
        {
            public StringId MaterialName;
            public MaterialTypeValue MaterialType;
            public short DamageSection;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public StringId GlobalMaterialName;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            
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
        
        [TagStructure(Size = 0x140)]
        public class ModelDamageInfo : TagStructure
        {
            public FlagsValue Flags;
            public StringId GlobalIndirectMaterialName; // absorbes AOE or child damage
            public short IndirectDamageSection; // absorbes AOE or child damage
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public CollisionDamageReportingTypeValue CollisionDamageReportingType;
            public ResponseDamageReportingTypeValue ResponseDamageReportingType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding4;
            /// <summary>
            /// body
            /// </summary>
            public float MaximumVitality;
            public float MinimumStunDamage; // the minimum damage required to stun this object's health
            public float StunTime; // seconds
            public float RechargeTime; // seconds
            public float RechargeFraction; // 0 defaults to 1 - to what maximum level the body health will be allowed to recharge
            [TagField(Flags = Padding, Length = 64)]
            public byte[] Padding5;
            /// <summary>
            /// shield
            /// </summary>
            public CachedTag ShieldDamagedFirstPersonShader;
            public CachedTag ShieldDamagedShader;
            public float MaximumShieldVitality; // the default initial and maximum shield vitality of this object
            public StringId GlobalShieldMaterialName;
            public float MinimumStunDamage1; // the minimum damage required to stun this object's shields
            public float StunTime2; // seconds
            public float RechargeTime3; // seconds
            public float ShieldDamagedThreshold;
            public CachedTag ShieldDamagedEffect;
            public CachedTag ShieldDepletedEffect;
            public CachedTag ShieldRechargingEffect;
            public List<ModelDamageSection> DamageSections;
            public List<DamageNode> Nodes;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding6;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding7;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding9;
            public List<DamageSeatInfo> DamageSeats;
            public List<DamageConstraintInfo> DamageConstraints;
            /// <summary>
            /// overshield
            /// </summary>
            public CachedTag OvershieldFirstPersonShader;
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
            
            [TagStructure(Size = 0x44)]
            public class ModelDamageSection : TagStructure
            {
                public StringId Name;
                /// <summary>
                /// damage section flags
                /// </summary>
                /// <remarks>
                /// * absorbs body damage: damage to this section does not count against body vitality
                /// * headshottable: takes extra headshot damage when shot
                /// * ignores shields: damage to this section bypasses shields
                /// </remarks>
                public FlagsValue Flags;
                public float VitalityPercentage; // [0.1]
                public List<InstantaneousDamageResponse> InstantResponses;
                public List<GNullBlock> Unknown1;
                public List<GNullBlock> Unknown2;
                public float StunTime; // seconds
                public float RechargeTime; // seconds
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public StringId ResurrectionRestoredRegionName;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding2;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    AbsorbsBodyDamage = 1 << 0,
                    TakesFullDmgWhenObjectDies = 1 << 1,
                    CannotDieWithRiders = 1 << 2,
                    TakesFullDmgWhenObjDstryd = 1 << 3,
                    RestoredOnRessurection = 1 << 4,
                    Unused = 1 << 5,
                    Unused0 = 1 << 6,
                    Heatshottable = 1 << 7,
                    IgnoresShields = 1 << 8
                }
                
                [TagStructure(Size = 0x68)]
                public class InstantaneousDamageResponse : TagStructure
                {
                    public ResponseTypeValue ResponseType;
                    /// <summary>
                    /// Constraint damage type
                    /// </summary>
                    /// <remarks>
                    /// * if you specify a constraint group name (see lower section of this block)
                    ///   you can specify a constraint damage
                    /// * loosening a constraint takes it out of the rigid state - activates it
                    /// * destroying a constraint sets the attached body free
                    /// </remarks>
                    public ConstraintDamageTypeValue ConstraintDamageType;
                    /// <summary>
                    /// Damage response flags
                    /// </summary>
                    /// <remarks>
                    /// * kills object: when the response fires the object dies regardless of its current health
                    /// * inhibits x: from halo 1 - disallows basic behaviors for a unit
                    /// * forces drop weapon: from halo 1 - makes the unit drop its current weapon
                    /// * kills weapon x trigger: destroys the x trigger on the unit's current weapon
                    /// * destroys object: when the response fires the object is destroyed
                    /// </remarks>
                    public FlagsValue Flags;
                    public float DamageThreshold; // repsonse fires after crossing this threshold.  1=full health
                    public CachedTag TransitionEffect;
                    public TagReference DamageEffect;
                    public StringId Region;
                    public NewStateValue NewState;
                    public short RuntimeRegionIndex;
                    public StringId EffectMarkerName;
                    public StringId DamageEffectMarker;
                    /// <summary>
                    /// Response delay
                    /// </summary>
                    /// <remarks>
                    /// If desired, you can specify a delay until the response fires.This delay is pre-empted if another timed response for the same section fires.The delay effect plays while the timer is counting down
                    /// </remarks>
                    public float ResponseDelay; // in seconds
                    public CachedTag DelayEffect;
                    public StringId DelayEffectMarkerName;
                    /// <summary>
                    /// Constraint destruction
                    /// </summary>
                    /// <remarks>
                    /// - a response can destroy a single constraint by naming it explicitly.
                    /// - alternatively it can randomly destroy a single constraint from a specified group if the "destroy one group constraint" flag is set
                    /// - also it can destroy all constraints in a specified group if the "destroy all group constraints" flag is set
                    /// 
                    /// </remarks>
                    public StringId ConstraintGroupName;
                    /// <summary>
                    /// seat ejaculation
                    /// </summary>
                    public StringId EjectingSeatLabel;
                    /// <summary>
                    /// skip fraction
                    /// </summary>
                    /// <remarks>
                    /// 0.0 always fires, 1.0 never fires
                    /// </remarks>
                    public float SkipFraction;
                    /// <summary>
                    /// destroyed child object marker name
                    /// </summary>
                    /// <remarks>
                    /// when this response fires, any children objects created at the supplied marker name will be destroyed
                    /// </remarks>
                    public StringId DestroyedChildObjectMarkerName;
                    /// <summary>
                    /// total damage threshold
                    /// </summary>
                    /// <remarks>
                    /// scale on total damage section vitality
                    /// </remarks>
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
                    
                    [TagStructure(Size = 0x10)]
                    public class TagReference : TagStructure
                    {
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
                    public class StringId : TagStructure
                    {
                        public StringId DamageEffectMarkerName;
                    }
                }
                
                [TagStructure()]
                public class GNullBlock : TagStructure
                {
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class DamageNode : TagStructure
            {
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding3;
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageSeatInfo : TagStructure
            {
                public StringId SeatLabel;
                public float DirectDamageScale; // 0==no damage, 1==full damage
                public float DamageTransferFallOffRadius;
                public float MaximumTransferDamageScale;
                public float MinimumTransferDamageScale;
            }
            
            [TagStructure(Size = 0x14)]
            public class DamageConstraintInfo : TagStructure
            {
                public StringId PhysicsModelConstraintName;
                public StringId DamageConstraintName;
                public StringId DamageConstraintGroupName;
                public float GroupProbabilityScale;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class ModelTarget : TagStructure
        {
            public StringId MarkerName; // multiple markers become multiple spheres of the same radius
            public float Size; // sphere radius
            public Angle ConeAngle; // the target is only visible when viewed within this angle of the marker's x axis
            public short DamageSection; // the target is associated with this damage section
            public short Variant; // the target will only appear with this variant
            public float TargetingRelevance; // higher relevances turn into stronger magnetisms
            public ModelTargetLockOnData LockOnData;
            
            [TagStructure(Size = 0x8)]
            public class ModelTargetLockOnData : TagStructure
            {
                /// <summary>
                /// lock-on fields
                /// </summary>
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
        
        [TagStructure(Size = 0x14)]
        public class ModelRegion : TagStructure
        {
            public StringId Name;
            public sbyte CollisionRegionIndex;
            public sbyte PhysicsRegionIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<ModelPermutation> Permutations;
            
            [TagStructure(Size = 0x8)]
            public class ModelPermutation : TagStructure
            {
                public StringId Name;
                public FlagsValue Flags;
                public sbyte CollisionPermutationIndex;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    CannotBeChosenRandomly = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x5C)]
        public class ModelNode : TagStructure
        {
            public StringId Name;
            public short ParentNode;
            public short FirstChildNode;
            public short NextSiblingNode;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public RealPoint3d DefaultTranslation;
            public RealQuaternion DefaultRotation;
            public float DefaultInverseScale;
            public RealVector3d DefaultInverseForward;
            public RealVector3d DefaultInverseLeft;
            public RealVector3d DefaultInverseUp;
            public RealPoint3d DefaultInversePosition;
        }
        
        [TagStructure(Size = 0x14)]
        public class ModelObjectDataBlock : TagStructure
        {
            public TypeValue Type;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
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
        
        [TagStructure(Size = 0x44)]
        public class ScenarioLoadParametersBlock : TagStructure
        {
            /// <summary>
            /// SCENARIO LOAD PARAMETERS
            /// </summary>
            /// <remarks>
            /// strip-variant variant-name
            /// strips a given variant out of the model tag
            /// strip-dialogue
            /// strips all the dialogue for this model i.e. cinematic only
            /// </remarks>
            public CachedTag Scenario;
            public byte[] Parameters;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
    }
}

