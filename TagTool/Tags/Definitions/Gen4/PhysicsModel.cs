using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x1A8)]
    public class PhysicsModel : TagStructure
    {
        public PhysicsModelFlags Flags;
        // scales the mass of each rigid body.  If you leave this field as 0, then it will be calculated from the total mass
        // below.
        public float MassScale;
        // override total mass of all rigid bodies.  Note that this will NOT be the mass of the object if not all rigid bodies
        // are present (for example if you are using permutations)
        // If you set a mass scale above, this field is unused.
        public float Mass;
        // 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        public float LowFreqDeactivationScale;
        // 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        public float HighFreqDeactivationScale;
        // 0 defaults to .016.  This field is intentionally hidden because we should only alter this for very special
        // situations.  Lower number == lower performance
        public float CustomShapeRadius;
        // 0 is default (1). for objects that are prone to falling through the world we can reduce this number at the cost of
        // performance
        public float MaximumPenetrationDepthScale;
        public sbyte ImportVersion;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<PhysicsModelDampedSpringMotorBlock> DampedSpringMotors;
        public List<PhysicsModelPositionMotorBlock> PositionMotors;
        public List<PhantomTypesBlockStruct> PhantomTypes;
        public List<PhysicsModelPoweredChainsBlock> PoweredChains;
        public List<PhysicsModelNodeConstraintEdgeBlock> NodeEdges;
        public List<RigidBodiesBlock> RigidBodies;
        public List<MaterialsBlock> Materials;
        public List<SpheresBlockStruct> Spheres;
        public List<MultiSpheresBlockStruct> MultiSpheres;
        public List<PillsBlockStruct> Pills;
        public List<BoxesBlockStruct> Boxes;
        public List<TrianglesBlockStruct> Triangles;
        public List<PolyhedraBlockStruct> Polyhedra;
        public List<PolyhedronFourVectorsBlock> PolyhedronFourVectors;
        public List<PolyhedronPlaneEquationsBlock> PolyhedronPlaneEquations;
        public List<MassDistributionsBlock> MassDistributions;
        public List<ListsBlock> Lists;
        public List<ListShapesBlockStruct> ListShapes;
        public List<MoppsBlockStruct> Mopps;
        public byte[] MoppCodes;
        public List<HingeConstraintsBlock> HingeConstraints;
        public List<RagdollConstraintsBlock> RagdollConstraints;
        public List<RegionsBlock> Regions;
        public List<NodesBlock> Nodes;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<PointToPathCurveBlock> PointToPathCurves;
        public List<LimitedHingeConstraintsBlock> LimitedHingeConstraints;
        public List<BallAndSocketConstraintsBlock> BallAndSocketConstraints;
        public List<StiffSpringConstraintsBlock> StiffSpringConstraints;
        public List<PrismaticConstraintsBlock> PrismaticConstraints;
        public List<PhantomsBlockStruct> Phantoms;
        public List<RigidBodySerializedShapesBlock> RigidBodySerializedShapes;
        
        [Flags]
        public enum PhysicsModelFlags : uint
        {
            MoppCodesDirty = 1 << 0,
            SerializedHavokDataConvertedToTargetPlatform = 1 << 1,
            MakePhysicalChildrenKeyframed = 1 << 2,
            ShrinkRadiusByHavokComplexRadius = 1 << 3
        }
        
        [TagStructure(Size = 0x18)]
        public class PhysicsModelDampedSpringMotorBlock : TagStructure
        {
            public StringId Name;
            // 0 defaults to k_real_max
            public float MaximumForce;
            // 0 defaults to maximum force.  In general you should leave this alone unless working on ragdolls or something like
            // them
            public float MinimumForce;
            public float SpringK;
            // 0 means default daming of 1.0f
            public float Damping;
            public float InitialPosition;
        }
        
        [TagStructure(Size = 0x20)]
        public class PhysicsModelPositionMotorBlock : TagStructure
        {
            public StringId Name;
            public float MaximumForce;
            // 0 defaults to maximum force.  In general you should leave this alone unless working on ragdolls or something like
            // them
            public float MinimumForce;
            // from 0-1.  relative stiffness
            public float Tau;
            // from 0-1
            public float Damping;
            // fraction of recover velocity used.  0 defaults to 1
            public float ProportionRecoverVel;
            // velocity used to recover when errors happen.  in degress per second
            public float ConsantRecoverVel;
            public float InitialPosition;
        }
        
        [TagStructure(Size = 0x74)]
        public class PhantomTypesBlockStruct : TagStructure
        {
            public PhantomFlagsV2 Flags;
            // objects in this phantom volume will be set to brittle collision damage for this amount of time.
            public float BrittleTimer; // seconds
            public RigidBodySizeEnum MinimumSize;
            public RigidBodySizeEnum MaximumSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // you don't need this if you're just generating effects.  If empty it defaults to the up of the object
            public StringId MarkerName;
            // you don't need this if you're just generating effects.  If empty it defaults to "marker name"
            public StringId AlignmentMarkerName;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            public float HookesLawE;
            // radius from linear motion origin in which acceleration is dead.
            public float LinearDeadRadius;
            public float CenterAcc;
            public float CenterMaxVel;
            public float AxisAcc;
            public float AxisMaxVel;
            public float DirectionAcc;
            public float DirectionMaxVel;
            // negative values spin the opposite direction from positive ones
            public float OrbitAcc;
            public float OrbitMaxVel;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            // 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            public float AlignmentHookesLawE;
            public float AlignmentAcc;
            public float AlignmentMaxVel;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
            [Flags]
            public enum PhantomFlagsV2 : uint
            {
                Unused = 1 << 0,
                UseAccAsForce = 1 << 1,
                NegatesGravity = 1 << 2,
                IgnoresPlayers = 1 << 3,
                IgnoresNonplayers = 1 << 4,
                IgnoresBipeds = 1 << 5,
                IgnoresVehicles = 1 << 6,
                IgnoresWeapons = 1 << 7,
                IgnoresEquipment = 1 << 8,
                IgnoresProjectiles = 1 << 9,
                IgnoresScenery = 1 << 10,
                IgnoresDevices = 1 << 11,
                IgnoresCrates = 1 << 12,
                IgnoresCreatures = 1 << 13,
                IgnoresGiants = 1 << 14,
                IgnoresSpawners = 1 << 15,
                IgnoresMechs = 1 << 16,
                IgnoresTeamObjects = 1 << 17,
                SmartDirectionalAcceleration = 1 << 18,
                IgnoresGroundedBipeds = 1 << 19,
                LocalizesPhysics = 1 << 20,
                DisableLinearDamping = 1 << 21,
                DisableAngularDamping = 1 << 22,
                IgnoresDeadBipeds = 1 << 23,
                IgnoresLiveBipeds = 1 << 24,
                ReciprocalAcc = 1 << 25,
                ReciprocalAccOnly = 1 << 26,
                LatchingDisabled = 1 << 27,
                MirroredAxis = 1 << 28,
                ActivateRagdolls = 1 << 29,
                IgnoresForgeEditor = 1 << 30
            }
            
            public enum RigidBodySizeEnum : sbyte
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                ExtraHuge
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class PhysicsModelPoweredChainsBlock : TagStructure
        {
            public List<PhysicsModelPoweredChainNodesBlock> Nodes;
            public List<PhysicsModelPoweredChainConstraintsBlock> Constraints;
            
            [TagStructure(Size = 0x2)]
            public class PhysicsModelPoweredChainNodesBlock : TagStructure
            {
                public short Node;
            }
            
            [TagStructure(Size = 0x10)]
            public class PhysicsModelPoweredChainConstraintsBlock : TagStructure
            {
                public RigidConstraintTypesEnum ConstraintType;
                public short ConstraintIndex;
                public PhysicsModelMotorReferenceStruct MotorX;
                public PhysicsModelMotorReferenceStruct MotorY;
                public PhysicsModelMotorReferenceStruct MotorZ;
                
                public enum RigidConstraintTypesEnum : short
                {
                    Hinge,
                    LimitedHinge,
                    Ragdoll,
                    StiffSpring,
                    BallAndSocket,
                    Prismatic,
                    PoweredChain
                }
                
                [TagStructure(Size = 0x4)]
                public class PhysicsModelMotorReferenceStruct : TagStructure
                {
                    public PhysicsModelMotorTypesEnum MotorType;
                    public short Index;
                    
                    public enum PhysicsModelMotorTypesEnum : short
                    {
                        None,
                        DampedSpring,
                        StongestForce
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class PhysicsModelNodeConstraintEdgeBlock : TagStructure
        {
            public short RuntimeMaterialTypeA;
            public short RuntimeMaterialTypeB;
            public short NodeA;
            public short NodeB;
            public List<PhysicsModelConstraintEdgeConstraintBlock> Constraints;
            // if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to
            // node a
            public StringId NodeAMaterial;
            // if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to
            // node b, if node b is none we use whatever material a has
            public StringId NodeBMaterial;
            
            [TagStructure(Size = 0x24)]
            public class PhysicsModelConstraintEdgeConstraintBlock : TagStructure
            {
                public RigidConstraintTypesEnum Type;
                public short Index;
                public RigidBodyConstraintEdgeConstraintFlags Flags;
                // 0 is the default (takes what it was set in max) anything else overrides that value
                public float Friction;
                public List<PhysicsModelRagdollMotorsBlock> RagdollMotors;
                public List<PhysicsModelLimitedHingeMotorsBlock> LimitedHingeMotors;
                
                public enum RigidConstraintTypesEnum : short
                {
                    Hinge,
                    LimitedHinge,
                    Ragdoll,
                    StiffSpring,
                    BallAndSocket,
                    Prismatic,
                    PoweredChain
                }
                
                [Flags]
                public enum RigidBodyConstraintEdgeConstraintFlags : uint
                {
                    // this constraint will only be created when this object is a child of another physical object (turrets on vehicles
                    // for example)
                    IsPhysicalChild = 1 << 0,
                    // this constraint makes the edge rigid until it is loosened by damage
                    IsRigid = 1 << 1,
                    // this constraint will not generate impact effects
                    DisableEffects = 1 << 2,
                    // this flag is used for special systems that need to create constraints dynamically
                    NotCreatedAutomatically = 1 << 3
                }
                
                [TagStructure(Size = 0xC)]
                public class PhysicsModelRagdollMotorsBlock : TagStructure
                {
                    public PhysicsModelMotorReferenceStruct TwistMotor;
                    public PhysicsModelMotorReferenceStruct ConeMotor;
                    public PhysicsModelMotorReferenceStruct PlaneMotor;
                    
                    [TagStructure(Size = 0x4)]
                    public class PhysicsModelMotorReferenceStruct : TagStructure
                    {
                        public PhysicsModelMotorTypesEnum MotorType;
                        public short Index;
                        
                        public enum PhysicsModelMotorTypesEnum : short
                        {
                            None,
                            DampedSpring,
                            StongestForce
                        }
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PhysicsModelLimitedHingeMotorsBlock : TagStructure
                {
                    public PhysicsModelMotorReferenceStruct Motor;
                    
                    [TagStructure(Size = 0x4)]
                    public class PhysicsModelMotorReferenceStruct : TagStructure
                    {
                        public PhysicsModelMotorTypesEnum MotorType;
                        public short Index;
                        
                        public enum PhysicsModelMotorTypesEnum : short
                        {
                            None,
                            DampedSpring,
                            StongestForce
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0xC0)]
        public class RigidBodiesBlock : TagStructure
        {
            public short Node;
            public short Region;
            public short Permutattion;
            public short SerializedShapes;
            public RealPoint3d BoudingSphereOffset;
            public float BoundingSphereRadius;
            public RigidBodyFlags Flags;
            public short NoPhantomPowerAlt;
            public RigidBodyMotionEnum MotionType;
            public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
            public RigidBodySizeEnum Size;
            // 0.0 defaults to 1.0
            public float InertiaTensorScale;
            // 0.0 defaults to 1.0
            public float InertiaTensorScaleX;
            // 0.0 defaults to 1.0
            public float InertiaTensorScaleY;
            // 0.0 defaults to 1.0
            public float InertiaTensorScaleZ;
            // this goes from 0-10 (10 is really, really high)
            public float LinearDamping;
            // this goes from 0-10 (10 is really, really high)
            public float AngularDamping;
            public RealVector3d CenterOffMassOffset;
            // x0 value of the water physics aabb
            public float WaterPhysicsX0;
            // x1 value of the water physics aabb
            public float WaterPhysicsX1;
            // y0 value of the water physics aabb
            public float WaterPhysicsY0;
            // y1 value of the water physics aabb
            public float WaterPhysicsY1;
            // z0 value of the water physics aabb
            public float WaterPhysicsZ0;
            // z1 value of the water physics aabb
            public float WaterPhysicsZ1;
            public int RuntimeShapePointer;
            public RealVector3d CenterOfMass;
            public float HavokWCenterOfMass;
            public RealVector3d IntertiaTensorX;
            public float HavokWIntertiaTensorX;
            public RealVector3d IntertiaTensorY;
            public float HavokWIntertiaTensorY;
            public RealVector3d IntertiaTensorZ;
            public float HavokWIntertiaTensorZ;
            public int RuntimeHavokGroupMask;
            public HavokShapeReferenceStruct ShapeReference;
            public float Mass; // kg*!
            // the bounding sphere for this rigid body will be outset by this much
            public float BoundingSpherePad;
            public RigidBodyCollisionQualityEnum CollisionQualityOverrideType;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short RuntimeFlags;
            public float MassBodyOverride;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum RigidBodyFlags : ushort
            {
                // don't check this flag without talking to eamon
                HasNoPhantomPowerVersion = 1 << 0,
                // rigid body will never have angular velocity
                InfiniteIntertiaTensor = 1 << 1,
                // this flag was invented for the behemoth and is dangerous to use anywhere else (obsolete)!
                CanUseMopps = 1 << 2,
                // rigid body properties like center of mass and inertia tensor come from Havok Content Tools (and shouldn't be edited
                // in Bonobo)
                HavokRigidBody = 1 << 3,
                // the mass is explicit, and not computed based on volume and density
                ExplicitMass = 1 << 4,
                // the 'center of mass offset' is relative to the pivot and not to the computed center
                AbsoluteCenterOfMass = 1 << 5,
                // rigid body is always keyframed and can't be overriden to dynamic or fixed
                ForceKeyframed = 1 << 6,
                // this rigid body will not generate a cutting silhouette
                ForceNotCutNavMesh = 1 << 7
            }
            
            public enum RigidBodyMotionEnum : sbyte
            {
                Sphere,
                StabilizedSphere,
                Box,
                StabilizedBox,
                Keyframed,
                Fixed
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
            
            public enum RigidBodySizeEnum : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                ExtraHuge
            }
            
            public enum RigidBodyCollisionQualityEnum : sbyte
            {
                None,
                DebrisSimpleToi,
                Moving,
                Critical,
                Bullet,
                Character,
                Fixed
            }
            
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
        
        [TagStructure(Size = 0x10)]
        public class MaterialsBlock : TagStructure
        {
            public StringId Name;
            public PhysicsMaterialFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId GlobalMaterialName;
            public short PhantomType;
            public PhysicsMaterialProxyCollisionGroups ProxyCollisionGroup;
            public sbyte RuntimeCollisionGroup;
            
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
        
        [TagStructure(Size = 0xC0)]
        public class MultiSpheresBlockStruct : TagStructure
        {
            public HavokPrimitiveStruct Base;
            public HavokShapeStruct SphereRepShape;
            public int NumSpheres;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 8)]
            public MultiSphereVectorStorage[]  FourVectorsStorage;
            
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
            
            [TagStructure(Size = 0x10)]
            public class MultiSphereVectorStorage : TagStructure
            {
                public RealVector3d Sphere;
                public float HavokWSphere;
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
        
        [TagStructure(Size = 0xB0)]
        public class BoxesBlockStruct : TagStructure
        {
            public HavokPrimitiveStruct Base;
            public HavokConvexShapeStruct BoxShape;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealVector3d HalfExtents;
            public float HavokWHalfExtents;
            public HavokConvexTransformShapeStruct ConvexTransformShape;
            
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
            
            [TagStructure(Size = 0x60)]
            public class HavokConvexTransformShapeStruct : TagStructure
            {
                public HavokConvexShapeStruct Convex;
                public int FieldPointerSkip;
                public HavokShapeReferenceStruct HavokShapeReferenceStruct1;
                public int ChildShapeSize;
                public RealVector3d RotationI;
                public float HavokWRotationI;
                public RealVector3d RotationJ;
                public float HavokWRotationJ;
                public RealVector3d RotationK;
                public float HavokWRotationK;
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
        
        [TagStructure(Size = 0x80)]
        public class TrianglesBlockStruct : TagStructure
        {
            public HavokPrimitiveStruct Base;
            public HavokConvexShapeStruct20102 TriangleShape;
            public short WeldingInfo;
            public sbyte WeldingType;
            public sbyte IsExtruded;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealVector3d PointA;
            public float HavokWPointA;
            public RealVector3d PointB;
            public float HavokWPointB;
            public RealVector3d PointC;
            public float HavokWPointC;
            public RealVector3d Extrusion;
            public float HavokWExtrusion;
            
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
            public class HavokConvexShapeStruct20102 : TagStructure
            {
                public HavokShapeStruct20102 Base;
                public float Radius;
                
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
        
        [TagStructure(Size = 0x90)]
        public class PolyhedraBlockStruct : TagStructure
        {
            public HavokPrimitiveStruct Base;
            public HavokConvexShapeStruct PolyhedronShape;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealVector3d AabbHalfExtents;
            public float HavokWAabbHalfExtents;
            public RealVector3d AabbCenter;
            public float HavokWAabbCenter;
            public int FieldPointerSkip;
            public int FourVectorsSize;
            public int FourVectorsCapacity;
            public int NumVertices;
            public sbyte MUsespuBuffer;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public int AnotherFieldPointerSkip;
            public int PlaneEquationsSize;
            public int PlaneEquationsCapacity;
            public int Connectivity;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            
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
        
        [TagStructure(Size = 0x30)]
        public class PolyhedronFourVectorsBlock : TagStructure
        {
            public RealVector3d FourVectorsX;
            public float HavokWFourVectorsX;
            public RealVector3d FourVectorsY;
            public float HavokWFourVectorsY;
            public RealVector3d FourVectorsZ;
            public float HavokWFourVectorsZ;
        }
        
        [TagStructure(Size = 0x10)]
        public class PolyhedronPlaneEquationsBlock : TagStructure
        {
            public RealVector3d PlaneEquations;
            public float HavokWPlaneEquations;
        }
        
        [TagStructure(Size = 0x40)]
        public class MassDistributionsBlock : TagStructure
        {
            public RealVector3d CenterOfMass;
            public float HavokWCenterOfMass;
            public RealVector3d InertiaTensorI;
            public float HavokWInertiaTensorI;
            public RealVector3d InertiaTensorJ;
            public float HavokWInertiaTensorJ;
            public RealVector3d InertiaTensorK;
            public float HavokWInertiaTensorK;
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
        
        [TagStructure(Size = 0x50)]
        public class MoppsBlockStruct : TagStructure
        {
            public HavokShapeStruct Base;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public int MoppCodePointer;
            public int MoppDataSkip;
            public int MoppDataSize;
            public RealVector3d MCodeinfoCopy;
            public float HavokWMCodeinfoCopy;
            public int ChildShapeVtable;
            public HavokShapeReferenceStruct ChildshapePointer;
            public int ChildSize;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float Scale;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            
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
        
        [TagStructure(Size = 0x78)]
        public class HingeConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x94)]
        public class RagdollConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float MinTwist;
            public float MaxTwist;
            public float MinCone;
            public float MaxCone;
            public float MinPlane;
            public float MaxPlane;
            public float MaxFricitonTorque;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class RegionsBlock : TagStructure
        {
            public StringId Name;
            public List<PermutationsBlock> Permutations;
            
            [TagStructure(Size = 0x10)]
            public class PermutationsBlock : TagStructure
            {
                public StringId Name;
                public List<RigidBodyIndicesBlock> RigidBodies;
                
                [TagStructure(Size = 0x2)]
                public class RigidBodyIndicesBlock : TagStructure
                {
                    public short RigidBody;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class NodesBlock : TagStructure
        {
            public StringId Name;
            public NodesFlags Flags;
            public short Parent;
            public short Sibling;
            public short Child;
            
            [Flags]
            public enum NodesFlags : ushort
            {
                DoesNotAnimate = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x118)]
        public class GlobalErrorReportCategoriesBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ErrorReportTypes ReportType;
            public ErrorReportFlags Flags;
            public short RuntimeGenerationFlags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int RuntimeSomething;
            public List<ErrorReportsBlock> Reports;
            
            public enum ErrorReportTypes : short
            {
                Silent,
                Comment,
                Warning,
                Error
            }
            
            [Flags]
            public enum ErrorReportFlags : ushort
            {
                Rendered = 1 << 0,
                TangentSpace = 1 << 1,
                NonCritical = 1 << 2,
                LightmapLight = 1 << 3,
                ReportKeyIsValid = 1 << 4
            }
            
            [TagStructure(Size = 0xB8)]
            public class ErrorReportsBlock : TagStructure
            {
                public ErrorReportTypes Type;
                public ErrorReportSource Source;
                public ErrorReportFlags Flags;
                public byte[] Text;
                public int SourceIdentifier;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVerticesBlock> Vertices;
                public List<ErrorReportVectorsBlock> Vectors;
                public List<ErrorReportLinesBlock> Lines;
                public List<ErrorReportTrianglesBlock> Triangles;
                public List<ErrorReportQuadsBlock> Quads;
                public List<ErrorReportCommentsBlock> Comments;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                
                public enum ErrorReportSource : sbyte
                {
                    None,
                    Structure,
                    Poop,
                    Lightmap,
                    Pathfinding
                }
                
                [TagStructure(Size = 0x34)]
                public class ErrorReportVerticesBlock : TagStructure
                {
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    public float ScreenSize;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
                
                [TagStructure(Size = 0x40)]
                public class ErrorReportVectorsBlock : TagStructure
                {
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
                
                [TagStructure(Size = 0x50)]
                public class ErrorReportLinesBlock : TagStructure
                {
                    [TagField(Length = 2)]
                    public ErrorReportLinePointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportLinePointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x70)]
                public class ErrorReportTrianglesBlock : TagStructure
                {
                    [TagField(Length = 3)]
                    public ErrorReportTrianglePointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportTrianglePointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x90)]
                public class ErrorReportQuadsBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public ErrorReportQuadPointArray[]  Points;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportQuadPointArray : TagStructure
                    {
                        public ErrorReportPointDefinition Point;
                        
                        [TagStructure(Size = 0x20)]
                        public class ErrorReportPointDefinition : TagStructure
                        {
                            public RealPoint3d Position;
                            [TagField(Length = 4)]
                            public ErrorPointNodeIndexArray[]  NodeIndices;
                            [TagField(Length = 4)]
                            public ErrorPointNodeWeightArray[]  NodeWeights;
                            
                            [TagStructure(Size = 0x1)]
                            public class ErrorPointNodeIndexArray : TagStructure
                            {
                                public sbyte NodeIndex;
                            }
                            
                            [TagStructure(Size = 0x4)]
                            public class ErrorPointNodeWeightArray : TagStructure
                            {
                                public float NodeWeight;
                            }
                        }
                    }
                }
                
                [TagStructure(Size = 0x44)]
                public class ErrorReportCommentsBlock : TagStructure
                {
                    public byte[] Text;
                    public ErrorReportPointDefinition Point;
                    public RealArgbColor Color;
                    
                    [TagStructure(Size = 0x20)]
                    public class ErrorReportPointDefinition : TagStructure
                    {
                        public RealPoint3d Position;
                        [TagField(Length = 4)]
                        public ErrorPointNodeIndexArray[]  NodeIndices;
                        [TagField(Length = 4)]
                        public ErrorPointNodeWeightArray[]  NodeWeights;
                        
                        [TagStructure(Size = 0x1)]
                        public class ErrorPointNodeIndexArray : TagStructure
                        {
                            public sbyte NodeIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class ErrorPointNodeWeightArray : TagStructure
                        {
                            public float NodeWeight;
                        }
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class PointToPathCurveBlock : TagStructure
        {
            public StringId Name;
            public short NodeIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<PointToPathCurvePointBlock> Points;
            
            [TagStructure(Size = 0x10)]
            public class PointToPathCurvePointBlock : TagStructure
            {
                public RealPoint3d Position;
                public float TValue;
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class LimitedHingeConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float LimitFriction;
            public float LimitMinAngle;
            public float LimitMaxAngle;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x78)]
        public class BallAndSocketConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x7C)]
        public class StiffSpringConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float SpringLength;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class PrismaticConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStruct ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float MinLimit;
            public float MaxLimit;
            public float MaxFrictionForce;
            
            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStruct : TagStructure
            {
                public StringId Name;
                public short NodeA;
                public short NodeB;
                public float AScale;
                public RealVector3d AForward;
                public RealVector3d ALeft;
                public RealVector3d AUp;
                public RealPoint3d APosition;
                public float BScale;
                public RealVector3d BForward;
                public RealVector3d BLeft;
                public RealVector3d BUp;
                public RealPoint3d BPosition;
                public short EdgeIndex;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class PhantomsBlockStruct : TagStructure
        {
            public HavokShapeStruct BvShape;
            public HavokShapeReferenceStruct HavokShapeReferenceStruct1;
            public int FieldPointerSkip;
            public int ChildShapePointer;
            public HavokShapeStruct PhantomShape;
            
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
        
        [TagStructure(Size = 0xC)]
        public class RigidBodySerializedShapesBlock : TagStructure
        {
            public List<MoppSerializedHavokDataBlock> MoppSerializedHavokData;
            
            [TagStructure(Size = 0x50)]
            public class MoppSerializedHavokDataBlock : TagStructure
            {
                public HavokPrimitiveStruct Base;
                public int Version;
                public int RuntimeDeserializedShapePointer;
                public byte[] SerializedHavokData;
                public byte[] SerializedHavokDataAnyTemp;
                
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
            }
        }
    }
}
