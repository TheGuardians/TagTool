using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x18C, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Beta)]
	[TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x1A0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x198, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x198, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x19C, MinVersion = CacheVersion.HaloReach)]
    public class PhysicsModel : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public PhysicsModelFlags Flags;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ReachPhysicsModelFlags FlagsReach;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float MassScale;

        public float Mass;
        public float LowFrequencyDeactivationScale;
        public float HighFrequencyDeactivationScale;

		[TagField(MinVersion = CacheVersion.Halo3Retail)]
		public float CustomShapeRadius;

		[TagField(MinVersion = CacheVersion.Halo3Retail)]
		public float MaximumPenetrationDepthScale;

		[TagField(MinVersion = CacheVersion.Halo3Retail)]
		public sbyte ImportVersion;

        [TagField(Flags = Padding, Length = 3, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Padding0;

        public List<DampedSpringMotor> DampedSpringMotors;
        public List<PositionMotor> PositionMotors;
        public List<PhantomType> PhantomTypes;
        public List<PoweredChain> PoweredChains;
        public List<NodeEdge> NodeEdges;
        public List<RigidBody> RigidBodies;
        public List<Material> Materials;
        public List<Sphere> Spheres;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedMultiSpheres;

        public List<Pill> Pills;
        public List<Box> Boxes;
        public List<Triangle> Triangles;
        public List<Polyhedron> Polyhedra;
        public List<PolyhedronFourVector> PolyhedronFourVectors;
        public List<PolyhedronPlaneEquation> PolyhedronPlaneEquations;

        public List<MassDistributionsBlock> MassDistributions;

        public List<List> Lists;
        public List<ListShape> ListShapes;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<CMoppBvTreeShape> Mopps;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.PhysicsModel.MoppsBlockStruct> ReachMopps;

        public byte[] MoppData;
        public List<HingeConstraint> HingeConstraints;
        public List<RagdollConstraint> RagdollConstraints;
        public List<Region> Regions;
        public List<Node> Nodes;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedErrors;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedPointToPathCurves;

        public List<LimitedHingeConstraint> LimitedHingeConstraints;
        public List<BallAndSocketConstraint> BallAndSocketConstraints;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedStiffSpringConstraints;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedPrismaticConstraints;

        public List<Phantom> Phantoms;

        [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public uint Unknown17;
        [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public uint Unknown18;

        [Flags]
        public enum PhysicsModelFlags : int
        {
            None = 0,
            SerializedHavokData = 1 << 0,
            MakePhysicalChildrenKeyframed = 1 << 1,
            ShrinkRadiusByHavokComplexRadius = 1 << 2,
            UsePhysicsForCollision = 1 << 3
        }

        [Flags]
        public enum ReachPhysicsModelFlags : uint
        {
            MoppCodesDirty = 1 << 0
        }

        [TagStructure(Size = 0x18)]
        public class DampedSpringMotor : TagStructure
		{
            public StringId Name;
            public float MaximumForce;
            public float MinimumForce;
            public float SpringK;
            public float Damping;
            public float InitialPosition;
        }

        [TagStructure(Size = 0x20)]
        public class PositionMotor : TagStructure
		{
            public StringId Name;
            public float MaximumForce;
            public float MinimumForce;
            public float Tau;
            public float Damping;
            public float ProportionRecoverVelocity;
            public float ConstantRecoverVelocity;
            public float InitialPosition;
        }

        [TagStructure(Size = 0x4)]
        public class PhantomTypeFlags : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo3Beta)]
            public Halo2Bits Halo2;

            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3Retail)]
            public Halo3RetailBits Halo3Retail;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline449175)]
            public Halo3ODSTBits Halo3ODST;

            [TagField(MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline700123)]
            public HaloOnlineBits HaloOnline;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public HaloReachBits HaloReach;

            [Flags]
            public enum Halo2Bits : uint
            {
                None = 0,
                GeneratesEffects = 1u << 0,
                UseAccelerationAsForce = 1u << 1,
                NegatesGravity = 1u << 2,
                IgnoresPlayers = 1u << 3,
                IgnoresNonPlayers = 1u << 4,
                IgnoresBipeds = 1u << 5,
                IgnoresVehicles = 1u << 6,
                IgnoresWeapons = 1u << 7,
                IgnoresEquipment = 1u << 8,
                IgnoresGarbage = 1u << 9,
                IgnoresProjectiles = 1u << 10,
                IgnoresScenery = 1u << 11,
                IgnoresMachines = 1u << 12,
                IgnoresControls = 1u << 13,
                IgnoresLightFixtures = 1u << 14,
                IgnoresSoundScenery = 1u << 15,
                IgnoresCrates = 1u << 16,
                IgnoresCreatures = 1u << 17,
                Unused0 = 1u << 19,
                Unused1 = 1u << 20,
                Unused2 = 1u << 21,
                Unused3 = 1u << 22,
                Unused4 = 1u << 23,
                LocalizesPhysics = 1 << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                IgnoresDeadBipeds = 1 << 27,
                Unknown28 = 1u << 28,
                Unknown29 = 1u << 29,
                Unknown30 = 1u << 30,
                Unknown31 = 1u << 31
            }

            [Flags]
            public enum Halo3RetailBits : uint
            {
                None = 0,
                GeneratesEffects = 1u << 0,
                UseAccelerationAsForce = 1u << 1,
                NegatesGravity = 1u << 2,
                IgnoresPlayers = 1u << 3,
                IgnoresNonPlayers = 1u << 4,
                IgnoresBipeds = 1u << 5,
                IgnoresVehicles = 1u << 6,
                IgnoresWeapons = 1u << 7,
                IgnoresEquipment = 1u << 8,
                IgnoresTerminals = 1u << 9,
                IgnoresProjectiles = 1u << 10,
                IgnoresScenery = 1u << 11,
                IgnoresMachines = 1u << 12,
                IgnoresControls = 1u << 13,
                IgnoresSoundScenery = 1u << 14,
                IgnoresCrates = 1u << 15,
                IgnoresCreatures = 1u << 16,
                IgnoresGiants = 1u << 17,
                IgnoresEffectScenery = 1u << 18,
                Unused0 = 1u << 19,     
                Unused1 = 1u << 20,
                Unused2 = 1u << 21,
                Unused3 = 1u << 22,
                Unused4 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1u << 25,
                DisableAngularDamping = 1u << 26,
                IgnoresDeadBipeds = 1u << 27,
                ReciprocalAcc = 1u << 28,
                ReciprocalAccOnly = 1u << 29,
                LatchingDisabled = 1u << 30,
                MirroredAxis = 1u << 31
            }

            [Flags]
            public enum Halo3ODSTBits : uint
            {
                None = 0,
                GeneratesEffects = 1u << 0,
                UseAccelerationAsForce = 1u << 1,
                NegatesGravity = 1u << 2,
                IgnoresPlayers = 1u << 3,
                IgnoresNonPlayers = 1u << 4,
                IgnoresBipeds = 1u << 5,
                IgnoresVehicles = 1u << 6,
                IgnoresWeapons = 1u << 7,
                IgnoresEquipment = 1u << 8,
                IgnoresARGDevices = 1u << 9,
                IgnoresTerminals = 1u << 10,
                IgnoresProjectiles = 1u << 11,
                IgnoresScenery = 1u << 12,
                IgnoresMachines = 1u << 13,
                IgnoresControls = 1u << 14,
                IgnoresSoundScenery = 1u << 15,
                IgnoresCrates = 1u << 16,
                IgnoresCreatures = 1u << 17,
                IgnoresGiants = 1u << 18,
                IgnoresEffectScenery = 1u << 19,
                Unused0 = 1u << 20,
                Unused1 = 1u << 21,
                Unused2 = 1u << 22,
                Unused3 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                IgnoresDeadBipeds = 1u << 27,
                ReciprocalAcc = 1u << 28,
                ReciprocalAccOnly = 1u << 29,
                LatchingDisabled = 1u << 30,
                MirroredAxis = 1u << 31
            }

            [Flags]
            public enum HaloOnlineBits : uint
            {
                None = 0,
                GeneratesEffects = 1u << 0,
                UseAccelerationAsForce = 1u << 1,
                NegatesGravity = 1u << 2,
                IgnoresPlayers = 1u << 3,
                IgnoresNonPlayers = 1u << 4,
                IgnoresBipeds = 1u << 5,
                IgnoresVehicles = 1u << 6,
                IgnoresWeapons = 1u << 7,
                IgnoresArmor = 1u << 8,
                IgnoresEquipment = 1u << 9,
                IgnoresARGDevices = 1u << 10,
                IgnoresTerminals = 1u << 11,
                IgnoresProjectiles = 1u << 12,
                IgnoresScenery = 1u << 13,
                IgnoresMachines = 1u << 14,
                IgnoresControls = 1u << 15,
                IgnoresSoundScenery = 1u << 16,
                IgnoresCrates = 1u << 17,
                IgnoresCreatures = 1u << 18,
                IgnoresGiants = 1u << 19,
                IgnoresEffectScenery = 1u << 20,
                Unused0 = 1u << 21,
                Unused1 = 1u << 22,
                Unused2 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                Unknown27 = 1 << 27,
                IgnoresDeadBipeds = 1u << 28,
                ReciprocalAcc = 1u << 29,
                ReciprocalAccOnly = 1u << 30,
                MirroredAxis = 1u << 31
            }
            [Flags]
            public enum HaloReachBits : uint
            {
                GeneratesEffects = 1 << 0,
                UseAccelerationAsForce = 1 << 1,
                NegatesGravity = 1 << 2,
                IgnoresPlayers = 1 << 3,
                IgnoresNonPlayers = 1 << 4,
                IgnoresBipeds = 1 << 5,
                IgnoresVehicles = 1 << 6,
                IgnoresWeapons = 1 << 7,
                IgnoresEquipment = 1 << 8,
                IgnoresGarbage = 1 << 9,
                IgnoresProjectiles = 1 << 10,
                IgnoresScenery = 1 << 11,
                IgnoresMachines = 1 << 12,
                IgnoresControls = 1 << 13,
                IgnoresSoundScenery = 1 << 14,
                IgnoresCrates = 1 << 15,
                IgnoresCreatures = 1 << 16,
                IgnoresGiants = 1 << 17,
                IgnoresEffectScenery = 1 << 18,
                Unused0 = 1 << 19,
                Unused1 = 1 << 20,
                Unused2 = 1 << 21,
                Unused3 = 1 << 22,
                IgnoresGroundedBipeds = 1 << 23,
                LocalizesPhysics = 1 << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                IgnoresDeadBipeds = 1 << 27,
                ReciprocalAcc = 1 << 28,
                ReciprocalAccOnly = 1 << 29,
                LatchingDisabled = 1 << 30,
                MirroredAxis = 1u << 31
            }
        }

        public enum PhantomTypeSize : sbyte
        {
            Default,
            Tiny,
            Small,
            Medium,
            Large,
            Huge,
            ExtraHuge
        }

        [TagStructure(Size = 0x68, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.HaloReach)]
        public class PhantomType : TagStructure
		{
            public PhantomTypeFlags Flags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BrittleTimer;

            public PhantomTypeSize MinimumSize;
            public PhantomTypeSize MaximumSize;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding0;

            public StringId MarkerName;
            public StringId AlignmentMarkerName;

            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;

            public float HookesLawE;
            public float LinearDeadRadius;
            public float CenterAcceleration;
            public float CenterMaxLevel;
            public float AxisAcceleration;
            public float AxisMaxVelocity;
            public float DirectionAcceleration;
            public float DirectionMaxVelocity;

            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding2;

            public float AlignmentHookesLawE;
            public float AlignmentAcceleration;
            public float AlignmentMaxVelocity;

            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding3;
        }

        public enum ConstraintType : short
        {
            Hinge,
            LimitedHinge,
            Ragdoll,
            StiffSpring,
            BallAndSocket,
            Prismatic,
            PoweredChain
        }

        public enum MotorType : short
        {
            None,
            DampedSprint,
            StrongestForce
        }

        [TagStructure(Size = 0x4)]
		public /*was_struct*/ class Motor : TagStructure
		{
            public MotorType Type;
            public short Index;
        }

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail)]
        public class PoweredChain : TagStructure
		{
            public List<Node> Nodes;
            public List<Constraint> Constraints;

            [TagStructure(Size = 0x2)]
            public class Node : TagStructure
			{
                public short NodeIndex;
            }

            [TagStructure(Size = 0x10)]
            public class Constraint : TagStructure
			{
                public ConstraintType ConstraintType;
                public short ConstraintIndex;
                public Motor MotorX;
                public Motor MotorY;
                public Motor MotorZ;
            }
        }

        [TagStructure(Size = 0x1C)]
        public class NodeEdge : TagStructure
		{
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public short NodeAGlobalMaterialIndex;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public short NodeBGlobalMaterialIndex;
            public short NodeA;
            public short NodeB;
            public List<Constraint> Constraints;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public StringId NodeAMaterial;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public StringId NodeBMaterial;

            [TagStructure(Size = 0x24)]
            public class Constraint : TagStructure
			{
                public ConstraintType Type;
                public short Index;
                public ConstraintFlags Flags;
                public float Friction;
                public List<RagdollMotor> RagdollMotors;
                public List<LimitedHingeMotor> LimitedHingeMotors;

                [Flags]
                public enum ConstraintFlags : int
                {
                    None = 0,
                    IsPhysicalChild = 1 << 0,
                    IsRigid = 1 << 1,
                    DisableEffects = 1 << 2,
                    NotCreatedAutomatically = 1 << 3
                }

                [TagStructure(Size = 0xC)]
                public class RagdollMotor : TagStructure
				{
                    public Motor TwistMotor;
                    public Motor ConeMotor;
                    public Motor PlaneMotor;
                }

                [TagStructure(Size = 0x4)]
                public class LimitedHingeMotor : TagStructure
				{
                    public Motor Motor;
                }
            }
        }

        public enum RigidBodySize : short
        {
            Default,
            Tiny,
            Small,
            Medium,
            Large,
            Huge,
            ExtraHuge
        }

		[TagStructure(Size = 0x90, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Beta, Platform = CachePlatform.Original)]
		[TagStructure(Size = 0xB0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0xC0, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)] 
        [TagStructure(Size = 0xC0, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0xD0, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public class RigidBody : TagStructure
		{
            public short Node;
            public short Region;
            public short Permutation;
            public short SerializedShapes;
            public RealPoint3d BoundingSphereOffset;
            public float BoundingSphereRadius;
            public RigidBodyFlags Flags;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public MotionTypeValue MotionType;
            public short NoPhantomPowerAltRigidBody;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MotionTypeValueByte MotionType_Reach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte ProxyCollisionGroup;
            public RigidBodySize Size;
            public float InertiaTensorScale;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float InertialTensorScaleX;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float InertialTensorScaleY;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float InertialTensorScaleZ;
            public float LinearDampening;
            public float AngularDampening;
            public RealVector3d CenterOfMassOffset;

			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsX0;
			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsX1;
			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsY0;
			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsY1;
			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsZ0;
			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public float WaterPhysicsZ1;

			[TagField(MinVersion = CacheVersion.Halo3Retail)]
			public uint RuntimeShapePointer;
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown9;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public BlamShapeType ShapeType;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short ShapeIndex;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0x4, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public uint RuntimeShapePointerPad;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Mass;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0xC, Platform = CachePlatform.MCC)]
            public byte[] Pad6 = new byte[0xC];

            public RealVector3d CenterOfMass;
            public float CenterOfMassRadius;
            public RealVector3d InertiaTensorX;
            public float InertiaTensorXRadius;
            public RealVector3d InertiaTensorY;
            public float InertiaTensorYRadius;
            public RealVector3d InertiaTensorZ;
            public float InertiaTensorZRadius;
            public uint RuntimeHavokGroupMask;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public BlamShapeType ShapeType_Reach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short ShapeIndex_Reach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Mass_Reach;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown10;
            public float BoundingSpherePad;
            public byte CollisionQualityOverrideType;

            [TagField(Flags = TagFieldFlags.Padding, Length = 1)]
            public byte[] Pad3 = new byte[1];

            public short RuntimeFlags;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float MassBodyOverride;
            [TagField(MinVersion = CacheVersion.HaloReach, Flags = TagFieldFlags.Padding, Length = 8)]
            public byte[] Pad4 = new byte[8];

            [Flags]
            public enum RigidBodyFlags : short
            {
                None = 0,
                NoCollisionsWithSelf = 1 << 0,
                OnlyCollideWithEnvironment = 1 << 1,
                DisableEffects = 1 << 2, //this rigid body will not generate impact effects unless it hits another dynamic rigid body that does
                DoesNotInteractWithEnvironment = 1 << 3, //set this flag if this rigid bodies won't touch the environment, this allows us to open up some optimizations
                BestEarlyMoverBody = 1 << 4, //if you have either of the early mover flags set in the object definitoin this body will be choosen as the one to make every thing local to, otherwise I pick :-
                HasNoPhantomPowerVersion = 1 << 5, //don't check this flag without talking to eamon
                InfiniteInertiaTensor = 1 << 6, //rigid body will never have angular velocity
                bit7 = 1 << 7,
                IgnoresGravity = 1 << 8,
                bit9 = 1 << 9,
                bit10 = 1 << 10,
                bit11 = 1 << 11,
                bit12 = 1 << 12,
                bit13 = 1 << 13,
                bit14 = 1 << 14
            }

            public enum MotionTypeValue : short
            {
                Sphere,
                StabilizedSphere,
                Box,
                StabilizedBox,
                Keyframed,
                Fixed,
            }

            public enum MotionTypeValueByte : byte
            {
                Sphere,
                StabilizedSphere,
                Box,
                StabilizedBox,
                Keyframed,
                Fixed,
            }
        }

        [Flags]
        public enum MaterialFlags : byte
        {
            None = 0,
            SupressesEffects = 1 << 0,
            ForceEnableCollisionWithPlayer = 1 << 1
        }

        [TagStructure(Size = 0xC, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class Material : TagStructure
		{
            public StringId Name;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte Flags;
            [TagField(MinVersion = CacheVersion.HaloReach, Flags = TagFieldFlags.Padding, Length = 3)]
            public byte[] Pad4 = new byte[3];
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public StringId MaterialName;
            public short PhantomType;
            public byte ProxyCollisionGroup;
            public byte RuntimeCollisionGroup;
        }

        [TagStructure(Size = 0x14, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x24, Platform = CachePlatform.MCC)]
        public class HavokShapeBase : TagStructure
        {
            public PlatformUnsignedValue FieldPointerSkip;
            public short Size;
            public short Count;
            public PlatformUnsignedValue Userdata;
            public int ShapeType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 4, Platform = CachePlatform.MCC)]
            public byte[] Padding1;
            public float Radius;
        }

        [TagStructure(Size = 0x10, Platform =  CachePlatform.Original)]
        [TagStructure(Size = 0x20, Platform = CachePlatform.MCC)]
        public class HavokShapeBaseNoRadius : TagStructure
        {
            public PlatformUnsignedValue FieldPointerSkip;
            public short Size;
            public short Count;
            public PlatformUnsignedValue Userdata;
            public int Type;
            [TagField(Length = 4, Flags = Padding, Platform = CachePlatform.MCC)]
            public byte[] Padding1;
        }

        [TagStructure(Size = 0x8)]
        public class HavokShapeReference : TagStructure
        {
            public BlamShapeType Shapetype;
            public short ShapeIndex;
            public uint ChildShapeSize;
        }
        
        [TagStructure(Size = 0x40, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x50, Platform = CachePlatform.MCC)]
        public class Shape : TagStructure
		{
            public StringId Name;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short MaterialIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte MaterialIndexReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MaterialFlags MaterialFlags;
            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public short RuntimeMaterialType;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short MassDistributionIndex;
            public sbyte PhantomIndex;
            public sbyte ProxyCollisionGroup;

            public HavokShapeBase ShapeBase;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0xC)]
            public byte[] Pad = new byte[0xC];
        }

        [TagStructure(Size = 0x30, Align = 0x10, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x50, Align = 0x10, Platform = CachePlatform.MCC)]
        public class Sphere : Shape
        {
            // translate shape
            public HavokShapeBase ConvexBase;
            public PlatformUnsignedValue FieldPointerSkip;
            public HavokShapeReference ShapeReference;
            [TagField(Align = 16)]
            public RealVector3d Translation;
            public float TranslationRadius;
        }

        [TagStructure(Size = 0x20, Align = 0x10)]
        public class Pill : Shape
        {
            public RealVector3d Bottom;
            public float BottomRadius;
            public RealVector3d Top;
            public float TopRadius;
        }

        [TagStructure(Size = 0x70, Align = 0x10, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x90, Align = 0x10, Platform = CachePlatform.MCC)]
        public class Box : Shape
        {
            public RealVector3d HalfExtents;
            public float HalfExtentsRadius;

            // transform shape
            public HavokShapeBase ConvexBase;
            public PlatformUnsignedValue FieldPointerSkip;
            public HavokShapeReference ShapeReference;
            [TagField(Align = 16)]
            public RealVector3d RotationI;
            public float RotationIRadius;
            public RealVector3d RotationJ;
            public float RotationJRadius;
            public RealVector3d RotationK;
            public float RotationKRadius;
            public RealVector3d Translation;
            public float TranslationRadius;
        }

        [TagStructure(Size = 0x40, Align = 0x10)]
        public class Triangle : Shape
        {
            public RealVector3d PointA;
            public float HavokwPointA;
            public RealVector3d PointB;
            public float HavokwPointB;
            public RealVector3d PointC;
            public float HavokwPointC;
            public RealVector3d Extrusion;
            public float HavokwExtrusion;
        }

        [TagStructure(Size = 0x40, Align = 0x10, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x50, Align = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x50, Align = 0x10, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x60, Align = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public class Polyhedron : Shape
        {
            public RealVector3d AabbHalfExtents;
            public float AabbHalfExtentsRadius;
            public RealVector3d AabbCenter;
            public float AabbCenterRadius;

            public PlatformUnsignedValue FieldPointerSkip;
            public int FourVectorsSize;
            public uint FourVectorsCapacity;

            public int NumVertices;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int UseSpuBuffer;

            public PlatformUnsignedValue AnotherFieldPointerSkip;
            public int PlaneEquationsSize;
            public uint PlaneEquationsCapacity;
            public uint Connectivity;
        }

        [TagStructure(Size = 0x30, Align = 0x10)]
        public class PolyhedronFourVector : TagStructure
		{
            public RealVector3d FourVectorsX;
            public float FourVectorsXW;
            public RealVector3d FourVectorsY;
            public float FourVectorsYW;
            public RealVector3d FourVectorsZ;
            public float FourVectorsZW;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class PolyhedronPlaneEquation : TagStructure
		{
            public RealPlane3d PlaneEquation;
        }

        [TagStructure(Size = 0x50, Align = 0x10, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x60, Align = 0x10, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0x70, Align = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x90, Align = 0x10, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public class List : TagStructure
		{
            // hkShapeCollection
            public PlatformUnsignedValue FieldPointerSkip;
            public short Size;
            public short Count;
            public PlatformUnsignedValue Offset;
            public PlatformUnsignedValue UserData;
            public PlatformUnsignedValue FieldPointerSkip0;
            public byte DisableWelding;
            public byte CollectionType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Pad0 = new byte[0x2];
            [TagField(Flags = TagFieldFlags.Padding, Length = 4, Platform = CachePlatform.MCC)]
            public byte[] Padding3;

            // Child info array (Points to "ListShapes" block at runtime)
            public PlatformUnsignedValue FieldPointerSkip1;
            public int ChildShapesSize;
            public uint ChildShapesCapacity;

            [TagField(Align = 16)]
            public RealVector3d AabbHalfExtents;
            public float AabbHalfExtentsRadius;
            public RealVector3d AabbCenter;
            public float AabbCenterRadius;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren0;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren1;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren5;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren6;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int EnabledChildren7;
        }

        [TagStructure(Size = 0x10, Align = 0x10, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x20, Align = 0x10, Platform = CachePlatform.MCC)]
        public class ListShape : TagStructure
		{
            public Havok.HavokShapeReference Shape;
            public uint CollisionFilter;
            public uint ShapeSize;
            public uint NumChildShapes;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding, Platform = CachePlatform.MCC)]
            public byte[] Padding1;
        }

        [TagStructure(Size = 0x78, Align = 0x10)]
        public class HingeConstraint : TagStructure
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
            public short Unknown;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x94)]
        public class RagdollConstraint : TagStructure
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
            public short Unknown;
            public uint Unknown2;
            public Bounds<float> TwistRange;
            public Bounds<float> ConeRange;
            public Bounds<float> PlaneRange;
            public float MaxFrictionTorque;
        }

        [TagStructure(Size = 0x10)]
        public class Region : TagStructure
		{
            public StringId Name;
            public List<Permutation> Permutations;

            [TagStructure(Size = 0x10)]
            public class Permutation : TagStructure
			{
                public StringId Name;
                public List<RigidBody> RigidBodies;

                [TagStructure(Size = 0x2)]
                public class RigidBody : TagStructure
				{
                    public short RigidBodyIndex;
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class Node : TagStructure
		{
            public StringId Name;
            public ushort Flags;
            public short Parent;
            public short Sibling;
            public short Child;
        }

        [TagStructure(Size = 0x84)]
        public class LimitedHingeConstraint : TagStructure
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
            public short Unknown;
            public uint Unknown2;
            public float LimitFriction;
            public Bounds<float> LimitAngleBounds;
        }

        [TagStructure(Size = 0x74)]
        public class BallAndSocketConstraint : TagStructure
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
            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] H;
        }

        [TagStructure(Size = 0x2C, Align = 0x10, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x58, Align = 0x10, Platform = CachePlatform.MCC)]
        public class Phantom : TagStructure
		{
            public HavokShapeBaseNoRadius ShapeBase;

            public BlamShapeType ShapeType;
            public short ShapeIndex;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0x4, Platform = CachePlatform.MCC)]
            public byte[] Padding;

            public PlatformUnsignedValue Unknown4;
            public PlatformUnsignedValue Unknown5;

            public HavokShapeBaseNoRadius PhantomShape;
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
    }
}