using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x1A0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x198, MinVersion = CacheVersion.HaloOnline106708)]
    public class PhysicsModel
    {
        public PhysicsModelFlags Flags;
        public float Mass;
        public float LowFrequencyDeactivationScale;
        public float HighFrequencyDeactivationScale;
        public float CustomShapeRadius;
        public float MaximumPenetrationDepthScale;
        public sbyte ImportVersion;

        [TagField(Padding = true, Length = 3)]
        public byte[] Unused;

        public List<DampedSprintMotor> DampedSpringMotors;
        public List<PositionMotor> PositionMotors;
        public List<PhantomType> PhantomTypes;
        public List<PoweredChain> PoweredChains;
        public List<NodeEdge> NodeEdges;
        public List<RigidBody> RigidBodies;
        public List<Material> Materials;
        public List<Sphere> Spheres;

        [TagField(Padding = true, Length = 12)]
        public byte[] UnusedMultiSpheres;

        public List<Pill> Pills;
        public List<Box> Boxes;
        public List<Triangle> Triangles;
        public List<Polyhedron> Polyhedra;
        public List<PolyhedronFourVector> PolyhedronFourVectors;
        public List<PolyhedronPlaneEquation> PolyhedronPlaneEquations;

        [TagField(Padding = true, Length = 12)]
        public byte[] UnusedMassDistributions;

        public List<List> Lists;
        public List<ListShape> ListShapes;
        public List<Mopp> Mopps;
        public byte[] MoppCodes;
        public List<HingeConstraint> HingeConstraints;
        public List<RagdollConstraint> RagdollConstraints;
        public List<Region> Regions;
        public List<Node> Nodes;
        public uint Unknown11;
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public uint Unknown15;
        public uint Unknown16;
        public List<LimitedHingeConstraint> LimitedHingeConstraints;

        [TagField(Padding = true, Length = 12)]
        public byte[] UnusedBallAndSocketConstraints;

        [TagField(Padding = true, Length = 12)]
        public byte[] UnusedStiffSprintConstraints;

        [TagField(Padding = true, Length = 12)]
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
        public class DampedSprintMotor
        {
            public StringId Name;
            public float MaximumForce;
            public float MinimumForce;
            public float SpringK;
            public float Damping;
            public float InitialPosition;
        }

        [TagStructure(Size = 0x20)]
        public class PositionMotor
        {
            public StringId Name;
            public uint MaximumForce;
            public uint MinimumForce;
            public uint Tau;
            public uint Damping;
            public uint ProportionRecoverVelocity;
            public uint ConstantRecoverVelocity;
            public uint InitialPosition;
        }

        public enum PhantomTypeFlags : int
        {
            None = 0,
            GeneratesEffects = 1 << 0,
            UseAccelerationAsForce = 1 << 1,
            NegatesGravity = 1 << 2,
            IgnoresPlayers = 1 << 3,
            IgnoresNonPlayers = 1 << 4,
            IgnoresBipeds = 1 << 5,
            IgnoresVehicles = 1 << 6,
            IgnoresWeapons = 1 << 7,
            IgnoresEquipement = 1 << 8,
            IgnoresARGDevices = 1 << 9,
            IgnoresTerminals = 1 << 10,
            IgnoresProjectiles = 1 << 11,
            IgnoresScenery = 1 << 12,
            IgnoresMachines = 1 << 13,
            IgnoresControls = 1 << 14,
            IgnoresSoundScenery = 1 << 15,
            IgnoresCrates = 1 << 16,
            IgnoresCreatures = 1 << 17,
            IgnoresGiants = 1 << 18,
            IgnoresEffectScenery = 1 << 19
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

        [TagStructure(Size = 0x68)]
        public class PhantomType
        {
            public PhantomTypeFlags Flags; // NOTE: This has to be adjusted when converting because of the new armor object type. The "Ignores Armor" bit was inserted at position 8.
            public PhantomTypeSize MinimumSize;
            public PhantomTypeSize MaximumSize;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused1;

            public StringId MarkerName;
            public StringId AlignmentMarkerName;

            [TagField(Padding = true, Length = 8)]
            public byte[] Unused2;

            public float HookesLawE;
            public float LinearDeadRadius;
            public float CenterAcceleration;
            public float CenterMaxLevel;
            public float AxisAcceleration;
            public float AxisMaxVelocity;
            public float DirectionAcceleration;
            public float DirectionMaxVelocity;

            [TagField(Padding = true, Length = 28)]
            public byte[] Unused3;

            public float AlignmentHookesLawE;
            public float AlignmentAcceleration;
            public float AlignmentMaxVelocity;

            [TagField(Padding = true, Length = 8)]
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

        [TagStructure(Size = 0x18)]
        public class PoweredChain
        {
            public List<Node> Nodes;
            public List<Constraint> Constraints;

            [TagStructure(Size = 0x2, Align = 0x8)]
            public class Node
            {
                public short NodeIndex;
            }

            [TagStructure(Size = 0x10)]
            public class Constraint
            {
                public ConstraintType ConstraintType;
                public short ConstraintIndex;
                public MotorType MotorXType;
                public short MotorXIndex;
                public MotorType MotorYType;
                public short MotorYIndex;
                public MotorType MotorZType;
                public short MotorZIndex;
            }
        }

        [TagStructure(Size = 0x1C)]
        public class NodeEdge
        {
            public short NodeAGlobalMaterialIndex;
            public short NodeBGlobalMaterialIndex;
            public short NodeA;
            public short NodeB;
            public List<Constraint> Constraints;
            public StringId NodeAMaterial;
            public StringId NodeBMaterial;

            [TagStructure(Size = 0x24)]
            public class Constraint
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
                public class RagdollMotor
                {
                    public MotorType TwistMotorType;
                    public short TwistIndex;
                    public MotorType ConeMotorType;
                    public short ConeIndex;
                    public MotorType PlaneMotorType;
                    public short PlaneIndex;
                }

                [TagStructure(Size = 0x4)]
                public class LimitedHingeMotor
                {
                    public short MotorType;
                    public short Index;
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

        [TagStructure(Size = 0xB0, Align = 0x10)]
        public class RigidBody
        {
            public short Node;
            public short Region;
            public short Permutations;
            public short Unknown;
            public RealPoint3d BoundingSphereOffset;
            public float BoundingSphereRadius;
            public ushort Flags;
            public MotionTypeValue MotionType;
            public short NoPhantomPowerAltRigidBody;
            public RigidBodySize Size;
            public float InertiaTensorScale;
            public float LinearDampening;
            public float AngularDampening;
            public RealPoint3d CenterOfMassOffset;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public HavokShapeType ShapeType;
            public short ShapeIndex;
            public float Mass;
            public RealVector3d CenterOfMass;
            public float CenterOfMassRadius;
            public RealVector3d InertiaTensorX;
            public float InertiaTensorXRadius;
            public RealVector3d InertiaTensorY;
            public float InertiaTensorYRadius;
            public RealVector3d InertiaTensorZ;
            public float InertiaTensorZRadius;
            public float BoundingSpherePad;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;

            public enum MotionTypeValue : short
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
        public enum MaterialFlags : ushort
        {
            None = 0,
            SupressesEffects = 1 << 0,
            ForceEnableCollisionWithPlayer = 1 << 1
        }

        [TagStructure(Size = 0xC)]
        public class Material
        {
            public StringId Name;
            public StringId MaterialName;
            public short PhantomType;
            public MaterialFlags Flags;
        }

        [TagStructure(Size = 0x70, Align = 0x10)]
        public class Sphere
        {
            public StringId Name;
            public sbyte MaterialIndex;
            public sbyte Unknown;
            public short GlobalMaterialIndex;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short OverallShapeIndex;
            public sbyte PhantomIndex;
            public sbyte InteractionUnknown;
            public int Unknown2;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown3;
            public float Radius;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public int Unknown7;
            public short Size2;
            public short Count2;
            public int Offset2;
            public int Unknown8;
            public float Radius2;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public RealVector3d Translation;
            public float TranslationRadius;
        }

        [TagStructure(Size = 0x60, Align = 0x10)]
        public class Pill
        {
            public StringId Name;
            public sbyte MaterialIndex;
            public sbyte Unknown;
            public short GlobalMaterialIndex;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short Index;
            public sbyte PhantomIndex;
            public sbyte InteractionUnknown;
            public int Unknown2;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown3;
            public float Radius;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public RealVector3d Bottom;
            public float BottomRadius;
            public RealVector3d Top;
            public float TopRadius;
        }

        [TagStructure(Size = 0xB0, Align = 0x10)]
        public class Box
        {
            public StringId Name;
            public sbyte MaterialIndex;
            public sbyte Unknown;
            public short GlobalMaterialIndex;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short Index;
            public sbyte PhantomIndex;
            public sbyte InteractionUnknown;
            public int Unknown2;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown3;
            public float Radius;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;

            public RealVector3d HalfExtents;
            public float HalfExtentsRadius;

            public int Unknown7;
            public short Size2;
            public short Count2;
            public int Offset2;
            public int Unknown8;
            public float Radius2;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;

            public RealVector3d RotationI;
            public float RotationIRadius;

            public RealVector3d RotationJ;
            public float RotationJRadius;

            public RealVector3d RotationK;
            public float RotationKRadius;

            public RealVector3d Translation;
            public float TranslationRadius;
        }

        [TagStructure(Size = 0x80, Align = 0x10)]
        public class Triangle
        {
            public StringId Name;
            public sbyte MaterialIndex;
            public sbyte Unknown;
            public short GlobalMaterialIndex;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short OverallShapeIndex;
            public sbyte PhantomIndex;
            public sbyte InteractionUnknown;
            public int Unknown2;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
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
        }

        [TagStructure(Size = 0x80, Align = 0x10)]
        public class Polyhedron
        {
            public StringId Name;
            public sbyte MaterialIndex;
            public sbyte Unknown;
            public short GlobalMaterialIndex;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short OverallShapeIndex;
            public sbyte PhantomIndex;
            public sbyte InteractionUnknown;
            public int Unknown2;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown3;
            public float Radius;
            public uint Unknown4;
            public uint Unknown5;
            public uint Unknown6;

            public RealVector3d AabbHalfExtents;
            public float AabbHalfExtentsRadius;

            public RealVector3d AabbCenter;
            public float AabbCenterRadius;

            public uint Unknown7;
            public int FourVectorsSize;
            public uint FourVectorsCapacity;
            public int Unknown8;
            public uint Unknown9;
            public int PlaneEquationsSize;
            public uint PlaneEquationsCapacity;
            public uint Unknown10;
        }

        [TagStructure(Size = 0x30, Align = 0x10)]
        public class PolyhedronFourVector
        {
            public RealVector3d FourVectorsX;
            public float FourVectorsXRadius;

            public RealVector3d FourVectorsY;
            public float FourVectorsYRadius;

            public RealVector3d FourVectorsZ;
            public float FourVectorsZRadius;
        }

        [TagStructure(Size = 0x10, Align = 0x10)]
        public class PolyhedronPlaneEquation
        {
            public float Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
        }

        [TagStructure(Size = 0x50, Align = 0x10)]
        public class List
        {
            public int Unknown;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public int ChildShapesSize;
            public uint ChildShapesCapacity;
            public uint Unknown6;
            public uint Unknown7;
            public uint Unknown8;
            public uint Unknown9;
            public uint Unknown10;
            public uint Unknown11;
            public uint Unknown12;
            public uint Unknown13;
            public uint Unknown14;
            public uint Unknown15;
            public uint Unknown16;
        }

        [TagStructure(Size = 0x10)]
        public class ListShape
        {
            public HavokShapeType ShapeType;
            public short ShapeIndex;
            public uint Unknown;
            public uint Unknown2;
            public int Unknown3;
        }

        [TagStructure(Size = 0x20, Align = 0x10)]
        public class Mopp
        {
            public int Unknown;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown2;
            public uint Unknown3;
            public HavokShapeType ShapeType;
            public short ShapeIndex;
            public uint Unknown4;
            public uint Unknown5;
        }

        [TagStructure(Size = 0x78, Align = 0x10)]
        public class HingeConstraint
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
        public class RagdollConstraint
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
        public class Region
        {
            public StringId Name;
            public List<Permutation> Permutations;

            [TagStructure(Size = 0x10)]
            public class Permutation
            {
                public StringId Name;
                public List<RigidBody> RigidBodies;

                [TagStructure(Size = 0x2)]
                public class RigidBody
                {
                    public short RigidBodyIndex;
                }
            }
        }

        [TagStructure(Size = 0xC)]
        public class Node
        {
            public StringId Name;
            public ushort Flags;
            public short Parent;
            public short Sibling;
            public short Child;
        }

        [TagStructure(Size = 0x84)]
        public class LimitedHingeConstraint
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
        public class Phantom
        {
            public int Unknown;
            public short Size;
            public short Count;
            public int Offset;
            public int Unknown2;
            public HavokShapeType ShapeType;
            public short ShapeIndex;
            public uint Unknown3;
            public uint Unknown4;
            public int Unknown5;
            public short Size2;
            public short Count2;
            public int Offset2;
            public int Unknown6;
        }
    }
}