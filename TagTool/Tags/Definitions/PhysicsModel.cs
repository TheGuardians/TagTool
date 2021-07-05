using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x18C, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Beta)]
	[TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x1A0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x198, MinVersion = CacheVersion.HaloOnlineED)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x19C, MinVersion = CacheVersion.HaloReach)]
    public class PhysicsModel : TagStructure
	{
        public PhysicsModelFlags Flags;

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
        public byte[] Unused;

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

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedMassDistributions;

        public List<List> Lists;
        public List<ListShape> ListShapes;
        public List<CMoppBvTreeShape> Mopps;
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

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedBallAndSocketConstraints;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedStiffSpringConstraints;

        [TagField(Flags = Padding, Length = 12)]
        public byte[] UnusedPrismaticConstraints;

        public List<Phantom> Phantoms;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public uint Unknown17;
        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
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

            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public HaloOnlineBits HaloOnline;

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
                IgnoresEquipement = 1u << 8,
                IgnoresGarbage = 1u << 9,
                IgnoresProjectiles = 1u << 10,
                IgnoresScenery = 1u << 11,
                IgnoresMachines = 1u << 12,
                IgnoresControls = 1u << 13,
                IgnoresLightFixtures = 1u << 14,
                IgnoresSoundScenery = 1u << 15,
                IgnoresCrates = 1u << 16,
                IgnoresCreatures = 1u << 17,
                Unknown19 = 1u << 19,
                Unknown20 = 1u << 20,
                Unknown21 = 1u << 21,
                Unknown22 = 1u << 22,
                Unknown23 = 1u << 23,
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
                IgnoresEquipement = 1u << 8,
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
                Unused1 = 1u << 19,     
                Unknown1 = 1u << 20,
                Unknown2 = 1u << 21,
                Unknown3 = 1u << 22,
                Unknown4 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1u << 25,
                DisableAngularDamping = 1u << 26,
                IgnoresDeadBipeds = 1u << 27,
                Unknown5 = 1u << 28,
                Unknown6 = 1u << 29,
                Unknown7 = 1u << 30,
                AccelerateAlongInputDirection = 1u << 31
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
                IgnoresEquipement = 1u << 8,
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
                Unknown1 = 1u << 20,
                Unknown2 = 1u << 21,
                Unknown3 = 1u << 22,
                Unknown4 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                IgnoresDeadBipeds = 1u << 27,
                Unknown5 = 1u << 28,
                Unknown6 = 1u << 29,
                Unknown7 = 1u << 30,
                AccelerateAlongInputDirection = 1u << 31
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
                IgnoresEquipement = 1u << 9,
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
                Unknown21 = 1u << 21,
                Unknown22 = 1u << 22,
                Unknown23 = 1u << 23,
                LocalizesPhysics = 1u << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                Unknown27 = 1 << 27,
                IgnoresDeadBipeds = 1u << 28,
                Unknown29 = 1u << 29,
                Unknown30 = 1u << 30,
                AccelerateAlongInputDirection = 1u << 31
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
            public byte[] Unused1;

            public StringId MarkerName;
            public StringId AlignmentMarkerName;

            [TagField(Flags = Padding, Length = 8)]
            public byte[] Unused2;

            public float HookesLawE;
            public float LinearDeadRadius;
            public float CenterAcceleration;
            public float CenterMaxLevel;
            public float AxisAcceleration;
            public float AxisMaxVelocity;
            public float DirectionAcceleration;
            public float DirectionMaxVelocity;

            [TagField(Flags = Padding, Length = 28)]
            public byte[] Unused3;

            public float AlignmentHookesLawE;
            public float AlignmentAcceleration;
            public float AlignmentMaxVelocity;

            [TagField(Flags = Padding, Length = 8)]
            public byte[] Unused4;
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

            [TagStructure(Size = 0x2, Align = 0x8)]
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
            public short NodeAGlobalMaterialIndex;
            public short NodeBGlobalMaterialIndex;
            public short NodeA;
            public short NodeB;
            public List<Constraint> Constraints;
            public StringId NodeAMaterial;
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

		[TagStructure(Size = 0x90, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3Beta)]
		[TagStructure(Size = 0xB0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xC0, MinVersion = CacheVersion.HaloReach)]
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
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Mass;
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
                HasNoPhantomPowerVersion = 1 << 5 //don't check this flag without talking to eamon
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
            public StringId MaterialName;
            public short PhantomType;
            public byte ProxyCollisionGroup;
            public byte RuntimeCollisionGroup;
        }

        [TagStructure(Size = 0x14)]
        public class HavokShapeBase : TagStructure
        {
            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public int UserData;
            public float Radius;
        }

        [TagStructure(Size = 0x10)]
        public class HavokShapeBaseNoRadius : TagStructure
        {
            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public int UserData;
        }

        [TagStructure(Size = 0x8)]
        public class HavokShapeReference : TagStructure
        {
            public BlamShapeType Shapetype;
            public short ShapeIndex;
            public uint ChildShapeSize;
        }
        
        [TagStructure(Size = 0x40)]
        public class Shape : TagStructure
		{
            public StringId Name;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short MaterialIndex;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public byte MaterialIndexReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public MaterialFlags MaterialFlags;
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

        [TagStructure(Size = 0x30, Align = 0x10)]
        public class Sphere : Shape
        {
            public HavokShapeBase ConvexBase;
            public uint FieldPointerSkip;
            public HavokShapeReference ShapeReference;

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

        [TagStructure(Size = 0x70, Align = 0x10)]
        public class Box : Shape
        {
            public RealVector3d HalfExtents;
            public float HalfExtentsRadius;

            public HavokShapeBase ConvexBase;
            public uint FieldPointerSkip;
            public HavokShapeReference ShapeReference;

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

        [TagStructure(Size = 0x40, Align = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x50, Align = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class Polyhedron : Shape
        {
            public RealVector3d AabbHalfExtents;
            public float AabbHalfExtentsRadius;
            public RealVector3d AabbCenter;
            public float AabbCenterRadius;

            public uint FieldPointerSkip;
            public int FourVectorsSize;
            public uint FourVectorsCapacity;
            public int NumVertices;
            public uint m_useSpuBuffer;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int AnotherFieldPointerSkip;
            public int PlaneEquationsSize;
            public uint PlaneEquationsCapacity;
            public uint Connectivity;
            [TagField(MinVersion = CacheVersion.HaloReach, Flags = TagFieldFlags.Padding, Length = 0xC)]
            public byte[] Pad = new byte[0xC];
        }

        [TagStructure(Size = 0x30, Align = 0x10)]
        public class PolyhedronFourVector : TagStructure
		{
            public RealVector3d FourVectorsX;
            public float FourVectorsXRadius;
            public RealVector3d FourVectorsY;
            public float FourVectorsYRadius;
            public RealVector3d FourVectorsZ;
            public float FourVectorsZRadius;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class PolyhedronPlaneEquation : TagStructure
		{
            public RealPlane3d PlaneEquation;
        }

        [TagStructure(Size = 0x50, Align = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x70, Align = 0x10, MinVersion = CacheVersion.HaloReach)]
        public class List : TagStructure
		{
            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public int UserData;

            public int FieldPointerSkip0;
            public byte DisableWelding;
            public byte CollectionType;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Pad0 = new byte[0x2];
            public int FieldPointerSkip1;

            public int ChildShapesSize;
            public uint ChildShapesCapacity;

            [TagField(Flags = TagFieldFlags.Padding, Length = 0xC)]
            public byte[] nail_in_dick = new byte[0xC];

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

        [TagStructure(Size = 0x10)]
        public class ListShape : TagStructure
		{
            public BlamShapeType ShapeType;
            public short ShapeIndex;
            public uint CollisionFilter;
            public uint ShapeSize;
            public uint NumChildShapes;
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
            public Bounds<Angle> LimitAngleBounds;
        }

        [TagStructure(Size = 0x2C, Align = 0x10)]
        public class Phantom : TagStructure
		{
            public HavokShapeBaseNoRadius ShapeBase;

            public BlamShapeType ShapeType;
            public short ShapeIndex;
            public uint Unknown4;
            public uint Unknown5;

            public HavokShapeBaseNoRadius PhantomShape;
        }
    }
}