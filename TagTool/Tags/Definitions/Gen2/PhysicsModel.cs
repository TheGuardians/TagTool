using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x18C)]
    public class PhysicsModel : TagStructure
    {
        public FlagsValue Flags;
        public float Mass;
        public float LowFreqDeactivationScale; // 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        public float HighFreqDeactivationScale; // 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        [TagField(Flags = Padding, Length = 24)]
        public byte[] Padding1;
        public List<PhysicsModelPhantomType> PhantomTypes;
        public List<PhysicsModelNodeConstraintEdge> NodeEdges;
        public List<PhysicsModelRigidBody> RigidBodies;
        public List<PhysicsModelMaterial> Materials;
        public List<PhysicsModelSphere> Spheres;
        public List<PhysicsModelMultiSphere> MultiSpheres;
        public List<PhysicsModelPill> Pills;
        public List<PhysicsModelBox> Boxes;
        public List<PhysicsModelTriangle> Triangles;
        public List<PhysicsModelPolyhedron> Polyhedra;
        /// <summary>
        /// ALL THESE WORLDS ARE YOURS, EXCEPT EUROPA...
        /// </summary>
        /// <remarks>
        /// Attempt no landings there.  And you can't edit anything below this point, so why even look at it?
        /// </remarks>
        public List<PolyhedronFourVectorsBlock> PolyhedronFourVectors;
        public List<Hkvector4> PolyhedronPlaneEquations;
        public List<PhysicsModelMassDistribution> MassDistributions;
        public List<PhysicsModelList> Lists;
        public List<ListShapesBlock> ListShapes;
        public List<PhysicsModelMopp> Mopps;
        public byte[] MoppCodes;
        public List<PhysicsModelHingeConstraint> HingeConstraints;
        public List<PhysicsModelRagdollConstraint> RagdollConstraints;
        public List<PhysicsModelRegion> Regions;
        public List<PhysicsModelNode> Nodes;
        public List<TagImportInfo> ImportInfo;
        public List<ErrorReportCategory> Errors;
        public List<PhysicsModelPointToPathCurve> PointToPathCurves;
        public List<PhysicsModelLimitedHingeConstraint> LimitedHingeConstraints;
        public List<PhysicsModelBallAndSocketConstraint> BallAndSocketConstraints;
        public List<PhysicsModelStiffSpringConstraint> StiffSpringConstraints;
        public List<PhysicsModelPrismaticConstraint> PrismaticConstraints;
        public List<PhysicsModelPhantom> Phantoms;
        
        [Flags]
        public enum FlagsValue : uint
        {
            Unused = 1 << 0
        }
        
        [TagStructure(Size = 0x68)]
        public class PhysicsModelPhantomType : TagStructure
        {
            public FlagsValue Flags;
            public MinimumSizeValue MinimumSize;
            public MaximumSizeValue MaximumSize;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public StringId MarkerName; // you don't need this if you're just generating effects.  If empty it defaults to the up of the object
            public StringId AlignmentMarkerName; // you don't need this if you're just generating effects.  If empty it defaults to "marker name"
            /// <summary>
            /// Linear Motion
            /// </summary>
            /// <remarks>
            /// 0 - means do nothing
            /// CENTER: motion towards marker position 
            /// AXIS: motion towards marker axis, such that object is on the axis
            /// DIRECTION: motion along marker direction
            /// </remarks>
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            public float HookesLawE; // 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            public float LinearDeadRadius; // radius from linear motion origin in which acceleration is dead.
            public float CenterAcc;
            public float CenterMaxVel;
            public float AxisAcc;
            public float AxisMaxVel;
            public float DirectionAcc;
            public float DirectionMaxVel;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
            /// <summary>
            /// Angular Motion
            /// </summary>
            /// <remarks>
            /// 0 - means do nothing
            /// ALIGNMENT: algin objects in the phantom with the marker
            /// SPIN: spin the object about the marker axis
            /// </remarks>
            public float AlignmentHookesLawE; // 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            public float AlignmentAcc;
            public float AlignmentMaxVel;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                GeneratesEffects = 1 << 0,
                UseAccAsForce = 1 << 1,
                NegatesGravity = 1 << 2,
                IgnoresPlayers = 1 << 3,
                IgnoresNonplayers = 1 << 4,
                IgnoresBipeds = 1 << 5,
                IgnoresVehicles = 1 << 6,
                IgnoresWeapons = 1 << 7,
                IgnoresEquipment = 1 << 8,
                IgnoresGarbage = 1 << 9,
                IgnoresProjectiles = 1 << 10,
                IgnoresScenery = 1 << 11,
                IgnoresMachines = 1 << 12,
                IgnoresControls = 1 << 13,
                IgnoresLightFixtures = 1 << 14,
                IgnoresSoundScenery = 1 << 15,
                IgnoresCrates = 1 << 16,
                IgnoresCreatures = 1 << 17,
                Bit18 = 1 << 18,
                Bit19 = 1 << 19,
                Bit20 = 1 << 20,
                Bit21 = 1 << 21,
                Bit22 = 1 << 22,
                Bit23 = 1 << 23,
                LocalizesPhysics = 1 << 24,
                DisableLinearDamping = 1 << 25,
                DisableAngularDamping = 1 << 26,
                IgnoresDeadBipeds = 1 << 27
            }
            
            public enum MinimumSizeValue : sbyte
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                ExtraHuge
            }
            
            public enum MaximumSizeValue : sbyte
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
        
        [TagStructure(Size = 0x1C)]
        public class PhysicsModelNodeConstraintEdge : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public short NodeA;
            public short NodeB;
            public List<PhysicsModelNodeConstraintEdgeConstraint> Constraints;
            public StringId NodeAMaterial; // if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to node a
            public StringId NodeBMaterial; // if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to node b, if node b is none we use whatever material a has
            
            [TagStructure(Size = 0xC)]
            public class PhysicsModelNodeConstraintEdgeConstraint : TagStructure
            {
                public TypeValue Type;
                public short Index;
                public FlagsValue Flags;
                public float Friction; // 0 is the default (takes what it was set in max) anything else overrides that value
                
                public enum TypeValue : short
                {
                    Hinge,
                    LimitedHinge,
                    Ragdoll,
                    StiffSpring,
                    BallAndSocket,
                    Prismatic
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    IsRigid = 1 << 0,
                    DisableEffects = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class PhysicsModelRigidBody : TagStructure
        {
            public short Node;
            public short Region;
            public short Permutattion;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public RealPoint3d BoudingSphereOffset;
            public float BoundingSphereRadius;
            public FlagsValue Flags;
            public MotionTypeValue MotionType;
            public short NoPhantomPowerAlt;
            public SizeValue Size;
            public float InertiaTensorScale; // 0.0 defaults to 1.0
            public float LinearDamping; // this goes from 0-10 (10 is really, really high)
            public float AngularDamping; // this goes from 0-10 (10 is really, really high)
            public RealVector3d CenterOffMassOffset;
            public ShapeTypeValue ShapeType;
            public short Shape;
            public float Mass; // kg*
            public RealVector3d CenterOfMass;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public RealVector3d IntertiaTensorX;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public RealVector3d IntertiaTensorY;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public RealVector3d IntertiaTensorZ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            public float BoundingSpherePad; // the bounding sphere for this rigid body will be outset by this much
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding2;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NoCollisionsWSelf = 1 << 0,
                OnlyCollideWEnv = 1 << 1,
                DisableEffects = 1 << 2,
                DoesNotInteractWEnvironment = 1 << 3,
                BestEarlyMoverBody = 1 << 4,
                HasNoPhantomPowerVersion = 1 << 5
            }
            
            public enum MotionTypeValue : short
            {
                Sphere,
                StabilizedSphere,
                Box,
                StabilizedBox,
                Keyframed,
                Fixed
            }
            
            public enum SizeValue : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                ExtraHuge
            }
            
            public enum ShapeTypeValue : short
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
        
        [TagStructure(Size = 0xC)]
        public class PhysicsModelMaterial : TagStructure
        {
            public StringId Name;
            public StringId GlobalMaterialName;
            public short PhantomType;
            public FlagsValue Flags;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DoesNotCollideWithFixedBodies = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x80)]
        public class PhysicsModelSphere : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public float Radius;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            public short Size1;
            public short Count2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown6;
            public RealVector3d RotationI;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown7;
            public RealVector3d RotationJ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown8;
            public RealVector3d RotationK;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown9;
            public RealVector3d Translation;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown10;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0xB0)]
        public class PhysicsModelMultiSphere : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public int NumSpheres;
            public RealVector3d Sphere;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            [TagField(Length = 8)]
            public FourVectorsStorageDatum[] FourVectorsStorage;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
            
            [TagStructure()]
            public class FourVectorsStorageDatum : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public RealVector3d Sphere;
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class PhysicsModelPill : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public float Radius;
            public RealVector3d Bottom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            public RealVector3d Top;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class PhysicsModelBox : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public float Radius;
            public RealVector3d HalfExtents;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            public short Size1;
            public short Count2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown6;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown7;
            public RealVector3d RotationI;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown8;
            public RealVector3d RotationJ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown9;
            public RealVector3d RotationK;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown10;
            public RealVector3d Translation;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown11;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class PhysicsModelTriangle : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public float Radius;
            public RealVector3d PointA;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            public RealVector3d PointB;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            public RealVector3d PointC;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown6;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x100)]
        public class PhysicsModelPolyhedron : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public short Phantom;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public float Radius;
            public RealVector3d AabbHalfExtents;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
            public RealVector3d AabbCenter;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown5;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown6;
            public int FourVectorsSize;
            public int FourVectorsCapacity;
            public int NumVertices;
            public RealVector3d FourVectorsX;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown7;
            public RealVector3d FourVectorsY;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown8;
            public RealVector3d FourVectorsZ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown9;
            [TagField(Length = 3)]
            public FourVectorsStorageDatum[] FourVectorsStorage;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown11;
            public int PlaneEquationsSize;
            public int PlaneEquationsCapacity;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown12;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
            
            [TagStructure()]
            public class FourVectorsStorageDatum : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown1;
                public RealVector3d FourVectorsZ;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public RealVector3d FourVectorsY;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public RealVector3d FourVectorsX;
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class PolyhedronFourVectorsBlock : TagStructure
        {
            public RealVector3d FourVectorsX;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public RealVector3d FourVectorsY;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public RealVector3d FourVectorsZ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
        }
        
        [TagStructure(Size = 0x10)]
        public class Hkvector4 : TagStructure
        {
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Unknown1;
        }
        
        [TagStructure(Size = 0x40)]
        public class PhysicsModelMassDistribution : TagStructure
        {
            public RealVector3d CenterOfMass;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public RealVector3d InertiaTensorI;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            public RealVector3d InertiaTensorJ;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public RealVector3d InertiaTensorK;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
        }
        
        [TagStructure(Size = 0x38)]
        public class PhysicsModelList : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public int ChildShapesSize;
            public int ChildShapesCapacity;
            public ShapeTypeValue ShapeType;
            public short Shape;
            public int CollisionFilter;
            [TagField(Length = 4)]
            public ChildShapesStorageDatum[] ChildShapesStorage;
            
            public enum ShapeTypeValue : short
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
            
            [TagStructure()]
            public class ChildShapesStorageDatum : TagStructure
            {
                public int CollisionFilter;
                public short Shape;
                public ShapeTypeValue ShapeType;
                
                public enum ShapeTypeValue : short
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
        
        [TagStructure(Size = 0x8)]
        public class ListShapesBlock : TagStructure
        {
            public ShapeTypeValue ShapeType;
            public short Shape;
            public int CollisionFilter;
            
            public enum ShapeTypeValue : short
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
        
        [TagStructure(Size = 0x14)]
        public class PhysicsModelMopp : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short List;
            public int CodeOffset;
        }
        
        [TagStructure(Size = 0x78)]
        public class PhysicsModelHingeConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x94)]
        public class PhysicsModelRagdollConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float MinTwist;
            public float MaxTwist;
            public float MinCone;
            public float MaxCone;
            public float MinPlane;
            public float MaxPlane;
            public float MaxFricitonTorque;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class PhysicsModelRegion : TagStructure
        {
            public StringId Name;
            public List<PhysicsModelPermutation> Permutations;
            
            [TagStructure(Size = 0x10)]
            public class PhysicsModelPermutation : TagStructure
            {
                public StringId Name;
                public List<RigidBodiesBlock> RigidBodies;
                
                [TagStructure(Size = 0x2)]
                public class RigidBodiesBlock : TagStructure
                {
                    public short RigidBody;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class PhysicsModelNode : TagStructure
        {
            public StringId Name;
            public FlagsValue Flags;
            public short Parent;
            public short Sibling;
            public short Child;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DoesNotAnimate = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x254)]
        public class TagImportInfo : TagStructure
        {
            public int Build;
            [TagField(Length = 256)]
            public string Version;
            [TagField(Length = 32)]
            public string ImportDate;
            [TagField(Length = 32)]
            public string Culprit;
            [TagField(Flags = Padding, Length = 96)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string ImportTime;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public List<TagImportFile> Files;
            [TagField(Flags = Padding, Length = 128)]
            public byte[] Padding3;
            
            [TagStructure(Size = 0x21C)]
            public class TagImportFile : TagStructure
            {
                [TagField(Length = 256)]
                public string Path;
                [TagField(Length = 32)]
                public string ModificationDate;
                [TagField(Flags = Padding, Length = 8)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 88)]
                public byte[] Padding1;
                public int Checksum; // crc32
                public int Size; // bytes
                public byte[] ZippedData;
                [TagField(Flags = Padding, Length = 128)]
                public byte[] Padding2;
            }
        }
        
        [TagStructure(Size = 0x2A8)]
        public class ErrorReportCategory : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ReportTypeValue ReportType;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 404)]
            public byte[] Padding3;
            public List<ErrorReport> Reports;
            
            public enum ReportTypeValue : short
            {
                Silent,
                Comment,
                Warning,
                Error
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Rendered = 1 << 0,
                TangentSpace = 1 << 1,
                Noncritical = 1 << 2,
                LightmapLight = 1 << 3,
                ReportKeyIsValid = 1 << 4
            }
            
            [TagStructure(Size = 0x284)]
            public class ErrorReport : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public byte[] Text;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVertex> Vertices;
                public List<ErrorReportVector> Vectors;
                public List<ErrorReportLine> Lines;
                public List<ErrorReportTriangle> Triangles;
                public List<ErrorReportQuad> Quads;
                public List<ErrorReportComment> Comments;
                [TagField(Flags = Padding, Length = 380)]
                public byte[] Padding1;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                [TagField(Flags = Padding, Length = 84)]
                public byte[] Padding2;
                
                public enum TypeValue : short
                {
                    Silent,
                    Comment,
                    Warning,
                    Error
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Rendered = 1 << 0,
                    TangentSpace = 1 << 1,
                    Noncritical = 1 << 2,
                    LightmapLight = 1 << 3,
                    ReportKeyIsValid = 1 << 4
                }
                
                [TagStructure(Size = 0x34)]
                public class ErrorReportVertex : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                    public float ScreenSize;
                }
                
                [TagStructure(Size = 0x40)]
                public class ErrorReportVector : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                }
                
                [TagStructure(Size = 0x50)]
                public class ErrorReportLine : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 2)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x70)]
                public class ErrorReportTriangle : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 3)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x90)]
                public class ErrorReportQuad : TagStructure
                {
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    [TagField(Length = 4)]
                    public RealPoint3d Points;
                    public RealArgbColor Color;
                }
                
                [TagStructure(Size = 0x44)]
                public class ErrorReportComment : TagStructure
                {
                    public byte[] Text;
                    public RealPoint3d Position;
                    public sbyte NodeIndex;
                    [TagField(Length = 4)]
                    public sbyte NodeIndices;
                    public float NodeWeight;
                    [TagField(Length = 4)]
                    public float NodeWeights;
                    public RealArgbColor Color;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class PhysicsModelPointToPathCurve : TagStructure
        {
            public StringId Name;
            public short NodeIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public List<PhysicsModelPointToPathCurvePoint> Points;
            
            [TagStructure(Size = 0x10)]
            public class PhysicsModelPointToPathCurvePoint : TagStructure
            {
                public RealPoint3d Position;
                public float TValue;
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class PhysicsModelLimitedHingeConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float LimitFriction;
            public float LimitMinAngle;
            public float LimitMaxAngle;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x78)]
        public class PhysicsModelBallAndSocketConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x7C)]
        public class PhysicsModelStiffSpringConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float SpringLength;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x84)]
        public class PhysicsModelPrismaticConstraint : TagStructure
        {
            public PhysicsModelConstraintBodies ConstraintBodies;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float MinLimit;
            public float MaxLimit;
            public float MaxFrictionForce;
            
            [TagStructure(Size = 0x74)]
            public class PhysicsModelConstraintBodies : TagStructure
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
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class PhysicsModelPhantom : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown3;
            public short Size1;
            public short Count2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown4;
        }
    }
}

