using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "creature", Tag = "crea", Size = 0x198)]
    public class Creature : GameObject
    {
        public CreatureDefinitionFlags Flags;
        public UnitDefaultTeams DefaultTeam;
        public GlobalChudBlipType MotionSensorBlipSize;
        public Angle TurningVelocityMaximum; // degrees per second
        public Angle TurningAccelerationMaximum; // degrees per second squared
        public float CasualTurningModifier; // [0,1]
        public float AutoaimWidth; // world units
        public CharacterPhysicsStruct Physics;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ImpactDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        // if not specified, uses 'impact damage'
        public CachedTag ImpactShieldDamage;
        public List<CampaignMetagameBucketBlock> CampaignMetagameBucket;
        // if non-zero, the creature will destroy itself upon death after this much time
        public Bounds<float> DestroyAfterDeathTime; // seconds
        public CreatureBigBattleDefinitionFlags BigBattleFlags;
        [TagField(ValidTags = new [] { "cpem" })]
        public CachedTag BigBattleWeaponEmitter;
        public RealPoint3d BigBattleWeaponOffset;
        [TagField(ValidTags = new [] { "cpem" })]
        // if you leave this empty, only the first emitter will fire
        public CachedTag BigBattleWeaponEmitter2;
        public RealPoint3d BigBattleWeaponOffset2;
        public List<CreatureScalarTimingBlock> BigBattleWeaponFireTiming;
        [TagField(ValidTags = new [] { "effe" })]
        // this fires a full effect from location up, oriented along vehicle's forward and up axes
        public CachedTag BigBattleExpensiveWeaponEffect;
        public Bounds<float> ExpensiveWeaponFireTime; // seconds
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag BigBattleDeathEffect;
        
        [Flags]
        public enum CreatureDefinitionFlags : uint
        {
            Unused = 1 << 0,
            ImmuneToFallingDamage = 1 << 1,
            RotateWhileAirborne = 1 << 2,
            ZappedByShields = 1 << 3,
            AttachUponImpact = 1 << 4,
            NotOnMotionSensor = 1 << 5,
            ForceGroundMovement = 1 << 6
        }
        
        public enum UnitDefaultTeams : short
        {
            Default,
            Player,
            Human,
            Covenant,
            Brute,
            Mule,
            Spare,
            CovenantPlayer,
            Forerunner
        }
        
        public enum GlobalChudBlipType : short
        {
            Medium,
            Small,
            Large
        }
        
        [Flags]
        public enum CreatureBigBattleDefinitionFlags : uint
        {
            // setting this forces boid to aim at target instead of firing straight ahead
            BoidAimsAtBigBattleTarget = 1 << 0,
            // flying boids will always stay level when changing altitude
            BoidsFlyWithNoPitch = 1 << 1,
            // flying boids will move like helicopters
            BoidsFlyNonDirectionally = 1 << 2
        }
        
        [TagStructure(Size = 0xDC)]
        public class CharacterPhysicsStruct : TagStructure
        {
            public CharacterPhysicsFlags Flags;
            public float HeightStanding;
            public float HeightCrouching;
            public float Radius;
            public float Mass;
            // collision material used when character is alive
            public StringId LivingMaterialName;
            // collision material used when character is dead
            public StringId DeadMaterialName;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short RuntimeGlobalMaterialType;
            public short RuntimeDeadGlobalMaterialType;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> DeadSphereShapes;
            // don't be an asshole, edit something else!
            public List<PillsBlockStruct> PillShapes;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> SphereShapes;
            // don't be an asshole, edit something else!
            public List<SpheresBlockStruct> ListSphereShapes;
            // don't be an asshole, edit something else!
            public List<ListsBlock> ListShapes;
            // don't be an asshole, edit something else!
            public List<ListShapesBlockStruct> ListShapeChildinfos;
            public CharacterPhysicsGroundStruct GroundPhysics;
            public CharacterPhysicsFlyingStruct FlyingPhysics;
            
            [Flags]
            public enum CharacterPhysicsFlags : uint
            {
                CenteredAtOrigin = 1 << 0,
                ShapeSpherical = 1 << 1,
                UsePlayerPhysics = 1 << 2,
                ClimbAnySurface = 1 << 3,
                Flying = 1 << 4,
                NotPhysical = 1 << 5,
                DeadCharacterCollisionGroup = 1 << 6,
                SuppressGroundPlanesOnBipeds = 1 << 7,
                PhysicalRagdoll = 1 << 8,
                DoNotResizeDeadSpheres = 1 << 9,
                MultipleMantisShapes = 1 << 10,
                IAmAnExtremeSlipsurface = 1 << 11,
                SlipsOffMovers = 1 << 12
            }
            
            [TagStructure(Size = 0x70)]
            public class SpheresBlockStruct : TagStructure
            {
                public HavokPrimitiveStruct Base;
                public HavokConvexShapeStruct SphereShape;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public HavokConvexTranslateShapeStruct TranslateShape;
                
                [TagStructure(Size = 0x20)]
                public class HavokPrimitiveStruct : TagStructure
                {
                    public StringId Name;
                    public sbyte Material;
                    public PhysicsMaterialFlags MaterialFlags;
                    public short RuntimeMaterialType;
                    public float RelativeMassScale;
                    public float Friction;
                    public float Restitution;
                    public float Volume;
                    public float Mass;
                    public short MassDistributionIndex;
                    public sbyte Phantom;
                    public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
                    
                    [Flags]
                    public enum PhysicsMaterialFlags : byte
                    {
                        SupressesEffects = 1 << 0,
                        // enables collision with the player regardless of the collision group
                        ForceEnableCollisionWithPlayer = 1 << 1
                    }
                    
                    public enum PhysicsMaterialProxyCollisionGroups : sbyte
                    {
                        None,
                        SmallCrate,
                        Crate,
                        HugeCrate,
                        Item,
                        Projectile,
                        Biped,
                        Machine,
                        EarlyMoverMachine,
                        OnlyCollideWithEnvironment,
                        TechArtCustom,
                        SmallExpensivePlant,
                        IgnoreEnvironment,
                        HugeVehicle,
                        Ragdoll,
                        SuperCollidableRagdoll,
                        ItemBlocker,
                        User00,
                        User01,
                        Everything,
                        Creatures
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HavokConvexShapeStruct : TagStructure
                {
                    public HavokShapeStruct Base;
                    public float Radius;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public sbyte Type;
                        public sbyte Dispatchtype;
                        public sbyte BitsperKey;
                        public sbyte Codectype;
                        public int UserData;
                    }
                }
                
                [TagStructure(Size = 0x30)]
                public class HavokConvexTranslateShapeStruct : TagStructure
                {
                    public HavokConvexShapeStruct Convex;
                    public int FieldPointerSkip;
                    public HavokShapeReferenceStruct HavokShapeReferenceStruct1;
                    public int ChildShapeSize;
                    public RealVector3d Translation;
                    public float HavokWTranslation;
                    
                    [TagStructure(Size = 0x4)]
                    public class HavokShapeReferenceStruct : TagStructure
                    {
                        public ShapeEnum ShapeType;
                        public short Shape;
                        
                        public enum ShapeEnum : short
                        {
                            Sphere,
                            Pill,
                            Box,
                            Triangle,
                            Polyhedron,
                            MultiSphere,
                            Unused0,
                            Unused1,
                            Unused2,
                            Unused3,
                            Unused4,
                            Unused5,
                            Unused6,
                            Unused7,
                            List,
                            Mopp
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class PillsBlockStruct : TagStructure
            {
                public HavokPrimitiveStruct Base;
                public HavokConvexShapeStruct CapsuleShape;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d Bottom;
                public float HavokWBottom;
                public RealVector3d Top;
                public float HavokWTop;
                
                [TagStructure(Size = 0x20)]
                public class HavokPrimitiveStruct : TagStructure
                {
                    public StringId Name;
                    public sbyte Material;
                    public PhysicsMaterialFlags MaterialFlags;
                    public short RuntimeMaterialType;
                    public float RelativeMassScale;
                    public float Friction;
                    public float Restitution;
                    public float Volume;
                    public float Mass;
                    public short MassDistributionIndex;
                    public sbyte Phantom;
                    public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
                    
                    [Flags]
                    public enum PhysicsMaterialFlags : byte
                    {
                        SupressesEffects = 1 << 0,
                        // enables collision with the player regardless of the collision group
                        ForceEnableCollisionWithPlayer = 1 << 1
                    }
                    
                    public enum PhysicsMaterialProxyCollisionGroups : sbyte
                    {
                        None,
                        SmallCrate,
                        Crate,
                        HugeCrate,
                        Item,
                        Projectile,
                        Biped,
                        Machine,
                        EarlyMoverMachine,
                        OnlyCollideWithEnvironment,
                        TechArtCustom,
                        SmallExpensivePlant,
                        IgnoreEnvironment,
                        HugeVehicle,
                        Ragdoll,
                        SuperCollidableRagdoll,
                        ItemBlocker,
                        User00,
                        User01,
                        Everything,
                        Creatures
                    }
                }
                
                [TagStructure(Size = 0x14)]
                public class HavokConvexShapeStruct : TagStructure
                {
                    public HavokShapeStruct Base;
                    public float Radius;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public sbyte Type;
                        public sbyte Dispatchtype;
                        public sbyte BitsperKey;
                        public sbyte Codectype;
                        public int UserData;
                    }
                }
            }
            
            [TagStructure(Size = 0x70)]
            public class ListsBlock : TagStructure
            {
                public HavokShapeCollectionStruct20102 Base;
                public int FieldPointerSkip;
                public int ChildShapesSize;
                public int ChildShapesCapacity;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public RealVector3d AabbHalfExtents;
                public float HavokWAabbHalfExtents;
                public RealVector3d AabbCenter;
                public float HavokWAabbCenter;
                public int EnabledChildren0;
                public int EnabledChildren1;
                public int EnabledChildren2;
                public int EnabledChildren3;
                public int EnabledChildren4;
                public int EnabledChildren5;
                public int EnabledChildren6;
                public int EnabledChildren7;
                
                [TagStructure(Size = 0x18)]
                public class HavokShapeCollectionStruct20102 : TagStructure
                {
                    public HavokShapeStruct20102 Base;
                    public int FieldPointerSkip;
                    public sbyte DisableWelding;
                    public sbyte CollectionType;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    [TagStructure(Size = 0x10)]
                    public class HavokShapeStruct20102 : TagStructure
                    {
                        public int FieldPointerSkip;
                        public short Size;
                        public short Count;
                        public int UserData;
                        public int Type;
                    }
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class ListShapesBlockStruct : TagStructure
            {
                public HavokShapeReferenceStruct ShapeReference;
                public int CollisionFilter;
                public int ShapeSize;
                public int NumChildShapes;
                
                [TagStructure(Size = 0x4)]
                public class HavokShapeReferenceStruct : TagStructure
                {
                    public ShapeEnum ShapeType;
                    public short Shape;
                    
                    public enum ShapeEnum : short
                    {
                        Sphere,
                        Pill,
                        Box,
                        Triangle,
                        Polyhedron,
                        MultiSphere,
                        Unused0,
                        Unused1,
                        Unused2,
                        Unused3,
                        Unused4,
                        Unused5,
                        Unused6,
                        Unused7,
                        List,
                        Mopp
                    }
                }
            }
            
            [TagStructure(Size = 0x44)]
            public class CharacterPhysicsGroundStruct : TagStructure
            {
                public Angle MaximumSlopeAngle; // degrees
                public Angle DownhillFalloffAngle; // degrees
                public Angle DownhillCutoffAngle; // degrees
                public Angle UphillFalloffAngle; // degrees
                public Angle UphillCutoffAngle; // degrees
                public float DownhillVelocityScale;
                public float UphillVelocityScale;
                public float RuntimeMinimumNormalK;
                public float RuntimeDownhillK0;
                public float RuntimeDownhillK1;
                public float RuntimeUphillK0;
                public float RuntimeUphillK1;
                // angle for bipeds at which climb direction changes between up and down
                public Angle ClimbInflectionAngle;
                // scale on the time for the entity to realize it is airborne
                public float ScaleAirborneReactionTime;
                // scale on velocity with which the entity is pushed back into its ground plane (set to -1 to lock to ground)
                public float ScaleGroundAdhesionVelocity;
                // scale on gravity for this entity
                public float GravityScale;
                // scale on airborne acceleration maximum
                public float AirborneAccelerationScale;
            }
            
            [TagStructure(Size = 0x30)]
            public class CharacterPhysicsFlyingStruct : TagStructure
            {
                // angle at which we bank left/right when sidestepping or turning while moving forwards
                public Angle BankAngle; // degrees
                // time it takes us to apply a bank
                public float BankApplyTime; // seconds
                // time it takes us to recover from a bank
                public float BankDecayTime; // seconds
                // amount that we pitch up/down when moving up or down
                public float PitchRatio;
                // max velocity when not crouching
                public float MaxVelocity; // world units per second
                // max sideways or up/down velocity when not crouching
                public float MaxSidestepVelocity; // world units per second
                public float Acceleration; // world units per second squared
                public float Deceleration; // world units per second squared
                // turn rate
                public Angle AngularVelocityMaximum; // degrees per second
                // turn acceleration rate
                public Angle AngularAccelerationMaximum; // degrees per second squared
                // how much slower we fly if crouching (zero = same speed)
                public float CrouchVelocityModifier; // [0,1]
                public FlyingPhysicsFlags Flags;
                
                [Flags]
                public enum FlyingPhysicsFlags : uint
                {
                    UseWorldUp = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class CampaignMetagameBucketBlock : TagStructure
        {
            public CampaignMetagameBucketFlags Flags;
            public CampaignMetagameBucketTypeEnum Type;
            public CampaignMetagameBucketClassEnum Class;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short PointCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum CampaignMetagameBucketFlags : byte
            {
                OnlyCountsWithRiders = 1 << 0
            }
            
            public enum CampaignMetagameBucketTypeEnum : sbyte
            {
                Brute,
                Grunt,
                Jackel,
                Skirmisher,
                Marine,
                Spartan,
                Bugger,
                Hunter,
                FloodInfection,
                FloodCarrier,
                FloodCombat,
                FloodPure,
                Sentinel,
                Elite,
                Engineer,
                Mule,
                Turret,
                Mongoose,
                Warthog,
                Scorpion,
                Hornet,
                Pelican,
                Revenant,
                Seraph,
                Shade,
                Watchtower,
                Ghost,
                Chopper,
                Mauler,
                Wraith,
                Banshee,
                Phantom,
                Scarab,
                Guntower,
                TuningFork,
                Broadsword,
                Mammoth,
                Lich,
                Mantis,
                Wasp,
                Phaeton,
                Bishop,
                Knight,
                Pawn
            }
            
            public enum CampaignMetagameBucketClassEnum : sbyte
            {
                Infantry,
                Leader,
                Hero,
                Specialist,
                LightVehicle,
                HeavyVehicle,
                GiantVehicle,
                StandardVehicle
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class CreatureScalarTimingBlock : TagStructure
        {
            public ScalarFunctionNamedStruct FunctionCurve;
            
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
