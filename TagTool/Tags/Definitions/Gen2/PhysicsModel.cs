using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "physics_model", Tag = "phmo", Size = 0x110)]
    public class PhysicsModel : TagStructure
    {
        public FlagsValue Flags;
        public float Mass;
        /// <summary>
        /// 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        /// </summary>
        public float LowFreqDeactivationScale;
        /// <summary>
        /// 0 is default (1). LESS than 1 deactivates less aggressively. GREATER than 1 is more agressive.
        /// </summary>
        public float HighFreqDeactivationScale;
        [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public List<PhantomTypesBlock> PhantomTypes;
        public List<PhysicsModelNodeConstraintEdgeBlock> NodeEdges;
        public List<RigidBodiesBlock> RigidBodies;
        public List<MaterialsBlock> Materials;
        public List<SpheresBlock> Spheres;
        public List<MultiSpheresBlock> MultiSpheres;
        public List<PillsBlock> Pills;
        public List<BoxesBlock> Boxes;
        public List<TrianglesBlock> Triangles;
        public List<PolyhedraBlock> Polyhedra;
        /// <summary>
        /// Attempt no landings there.  And you can't edit anything below this point, so why even look at it?
        /// </summary>
        public List<PolyhedronFourVectorsBlock> PolyhedronFourVectors;
        public List<PolyhedronPlaneEquationsBlock> PolyhedronPlaneEquations;
        public List<MassDistributionsBlock> MassDistributions;
        public List<ListsBlock> Lists;
        public List<ListShapesBlock> ListShapes;
        public List<MoppsBlock> Mopps;
        public byte[] MoppCodes;
        public List<HingeConstraintsBlock> HingeConstraints;
        public List<RagdollConstraintsBlock> RagdollConstraints;
        public List<RegionsBlock> Regions;
        public List<NodesBlock> Nodes;
        public List<GlobalTagImportInfoBlock> ImportInfo;
        public List<GlobalErrorReportCategoriesBlock> Errors;
        public List<PointToPathCurveBlock> PointToPathCurves;
        public List<LimitedHingeConstraintsBlock> LimitedHingeConstraints;
        public List<BallAndSocketConstraintsBlock> BallAndSocketConstraints;
        public List<StiffSpringConstraintsBlock> StiffSpringConstraints;
        public List<PrismaticConstraintsBlock> PrismaticConstraints;
        public List<PhantomsBlock> Phantoms;

        [Flags]
        public enum FlagsValue : uint
        {
            Unused = 1 << 0
        }

        [TagStructure(Size = 0x68)]
        public class PhantomTypesBlock : TagStructure
        {
            public FlagsValue Flags;
            public MinimumSizeValue MinimumSize;
            public MaximumSizeValue MaximumSize;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// you don't need this if you're just generating effects.  If empty it defaults to the up of the object
            /// </summary>
            public StringId MarkerName;
            /// <summary>
            /// you don't need this if you're just generating effects.  If empty it defaults to "marker name"
            /// </summary>
            public StringId AlignmentMarkerName;
            /// <summary>
            /// 0 - means do nothing
            /// CENTER: motion towards marker position 
            /// AXIS: motion towards marker axis, such that object is on the
            /// axis
            /// DIRECTION: motion along marker direction
            /// </summary>
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            /// </summary>
            public float HookesLawE;
            /// <summary>
            /// radius from linear motion origin in which acceleration is dead.
            /// </summary>
            public float LinearDeadRadius;
            public float CenterAcc;
            public float CenterMaxVel;
            public float AxisAcc;
            public float AxisMaxVel;
            public float DirectionAcc;
            public float DirectionMaxVel;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            /// <summary>
            /// 0 - means do nothing
            /// ALIGNMENT: algin objects in the phantom with the marker
            /// SPIN: spin the object about the marker axis
            /// </summary>
            /// <summary>
            /// 0 if you don't want this to behave like spring.  1 is a good starting point if you do.
            /// </summary>
            public float AlignmentHookesLawE;
            public float AlignmentAcc;
            public float AlignmentMaxVel;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;

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
                Unknown = 1 << 18,
                Unknown1 = 1 << 19,
                Unknown2 = 1 << 20,
                Unknown3 = 1 << 21,
                Unknown4 = 1 << 22,
                Unknown5 = 1 << 23,
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

        [TagStructure(Size = 0x18)]
        public class PhysicsModelNodeConstraintEdgeBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short NodeA;
            public short NodeB;
            public List<PhysicsModelConstraintEdgeConstraintBlock> Constraints;
            /// <summary>
            /// if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to node
            /// a
            /// </summary>
            public StringId NodeAMaterial;
            /// <summary>
            /// if you don't fill this out we will pluck the material from the first primitive, of the first rigid body attached to node
            /// b, if node b is none we use whatever material a has
            /// </summary>
            public StringId NodeBMaterial;

            [TagStructure(Size = 0xC)]
            public class PhysicsModelConstraintEdgeConstraintBlock : TagStructure
            {
                public TypeValue Type;
                public short Index;
                public FlagsValue Flags;
                /// <summary>
                /// 0 is the default (takes what it was set in max) anything else overrides that value
                /// </summary>
                public float Friction;

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
                    /// <summary>
                    /// this constraint makes the edge rigid until it is loosened by damage
                    /// </summary>
                    IsRigid = 1 << 0,
                    /// <summary>
                    /// this constraint will not generate impact effects
                    /// </summary>
                    DisableEffects = 1 << 1
                }
            }
        }

        [TagStructure(Size = 0x90)]
        public class RigidBodiesBlock : TagStructure
        {
            public short Node;
            public short Region;
            public short Permutattion;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint3d BoudingSphereOffset;
            public float BoundingSphereRadius;
            public FlagsValue Flags;
            public MotionTypeValue MotionType;
            public short NoPhantomPowerAlt;
            public SizeValue Size;
            /// <summary>
            /// 0.0 defaults to 1.0
            /// </summary>
            public float InertiaTensorScale;
            /// <summary>
            /// this goes from 0-10 (10 is really, really high)
            /// </summary>
            public float LinearDamping;
            /// <summary>
            /// this goes from 0-10 (10 is really, really high)
            /// </summary>
            public float AngularDamping;
            public RealVector3d CenterOffMassOffset;
            public ShapeTypeValue ShapeType;
            public short Shape;
            public float Mass; // kg*
            public RealVector3d CenterOfMass;
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public RealVector3d IntertiaTensorX;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public RealVector3d IntertiaTensorY;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public RealVector3d IntertiaTensorZ;
            [TagField(Length = 0x4)]
            public byte[] Unknown3;
            /// <summary>
            /// the bounding sphere for this rigid body will be outset by this much
            /// </summary>
            public float BoundingSpherePad;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;

            [Flags]
            public enum FlagsValue : ushort
            {
                NoCollisionsWSelf = 1 << 0,
                OnlyCollideWEnv = 1 << 1,
                /// <summary>
                /// this rigid body will not generate impact effects unless it hits another dynamic rigid body that does
                /// </summary>
                DisableEffects = 1 << 2,
                /// <summary>
                /// set this flag if this rigid bodies won't touch the environment, this allows us to open up some
                /// optimizations
                /// </summary>
                DoesNotInteractWEnvironment = 1 << 3,
                /// <summary>
                /// if you have either of the early mover flags set in the object definitoin this body will be choosen as
                /// the one to make every thing local to, otherwise I pick :-)
                /// </summary>
                BestEarlyMoverBody = 1 << 4,
                /// <summary>
                /// don't check this flag without talking to eamon
                /// </summary>
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
        public class MaterialsBlock : TagStructure
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
        public class SpheresBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public short Phantom;

            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public float Radius;

            public int FieldPointerSkip1;
            public short Size1;
            public short Count1;
            public int Offset1;
            public float Radius1;

            public RealVector3d RotationI;
            [TagField(Length = 0x4)]
            public byte[] Unknown6;
            public RealVector3d RotationJ;
            [TagField(Length = 0x4)]
            public byte[] Unknown7;
            public RealVector3d RotationK;
            [TagField(Length = 0x4)]
            public byte[] Unknown8;
            public RealVector3d Translation;
            [TagField(Length = 0x4)]
            public byte[] Unknown9;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }

        [TagStructure(Size = 0xB0)]
        public class MultiSpheresBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public short Phantom;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public int NumSpheres;
            [TagField(Length = 8)]
            public Unknown3Datum[] Unknown3;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }

            [TagStructure(Size = 0x10)]
            public class Unknown3Datum : TagStructure
            {
                public RealVector3d Sphere;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
            }
        }

        [TagStructure(Size = 0x50)]
        public class PillsBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short MassDistributionIndex;
            public short Phantom;

            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public float Radius;

            public RealVector3d Bottom;
            public float BottomRadius;
            public RealVector3d Top;
            public float TopRadius;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }

        [TagStructure(Size = 0x90)]
        public class BoxesBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            public short MassDistributionIndex;
            public short Phantom;

            public int FieldPointerSkip;
            public short Size;
            public short Count;
            public int Offset;
            public float Radius;

            public RealVector3d HalfExtents;
            public float HalfExtentsRadius;

            public int FieldPointerSkip1;
            public short Size1;
            public short Count1;
            public int Offset1;
            public float Radius1;

            public RealVector3d RotationI;
            public float RotationIRadius;
            public RealVector3d RotationJ;
            public float RotationJRadius;
            public RealVector3d RotationK;
            public float RotationKRadius;
            public RealVector3d Translation;
            public float TranslationRadius;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }

        [TagStructure(Size = 0x60)]
        public class TrianglesBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public short Phantom;

            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public float Radius;

            public RealVector3d PointA;
            [TagField(Length = 0x4)]
            public byte[] Unknown3;
            public RealVector3d PointB;
            [TagField(Length = 0x4)]
            public byte[] Unknown4;
            public RealVector3d PointC;
            [TagField(Length = 0x4)]
            public byte[] Unknown5;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }

        [TagStructure(Size = 0x100)]
        public class PolyhedraBlock : TagStructure
        {
            public StringId Name;
            public short Material;
            public FlagsValue Flags;
            public float RelativeMassScale;
            public float Friction;
            public float Restitution;
            public float Volume;
            public float Mass;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public short Phantom;

            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public float Radius;

            public RealVector3d AabbHalfExtents;
            public float AabbHalfExtentsRadius;
            public RealVector3d AabbCenter;
            public float AabbCenterRadius;
            public uint FieldPointerSkip;
            public int FourVectorsSize;
            public int FourVectorsCapacity;
            public int NumVertices;
            public PolyhedronFourVectorsBlock FourVectors;
            public uint m_useSpuBuffer;
            public int PlaneEquationsSize;
            public int PlaneEquationsCapacity;
            public uint Connectivity;

            [Flags]
            public enum FlagsValue : ushort
            {
                Unused = 1 << 0
            }
        }

        [TagStructure(Size = 0x30)]
        public class PolyhedronFourVectorsBlock : TagStructure
        {
            public RealVector3d FourVectorsX;
            public float FourVectorsXRadius;
            public RealVector3d FourVectorsY;
            public float FourVectorsYRadius;
            public RealVector3d FourVectorsZ;
            public float FourVectorsZRadius;
        }

        [TagStructure(Size = 0x10)]
        public class PolyhedronPlaneEquationsBlock : TagStructure
        {
            public RealPlane3d PlaneEquation;
        }

        [TagStructure(Size = 0x40)]
        public class MassDistributionsBlock : TagStructure
        {
            public RealVector3d CenterOfMass;
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public RealVector3d InertiaTensorI;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            public RealVector3d InertiaTensorJ;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public RealVector3d InertiaTensorK;
            [TagField(Length = 0x4)]
            public byte[] Unknown3;
        }

        [TagStructure(Size = 0x38)]
        public class ListsBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public int ChildShapesSize;
            public int ChildShapesCapacity;
            [TagField(Length = 4)]
            public CollisionFilterDatum[] CollisionFilter;

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

            [TagStructure(Size = 0x8)]
            public class CollisionFilterDatum : TagStructure
            {
                public ShapeTypeValue ShapeType;
                public short Shape;
                public int CollisionFilter;
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
        public class MoppsBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short List;
            public int CodeOffset;
        }

        [TagStructure(Size = 0x78)]
        public class HingeConstraintsBlock : TagStructure
        {
            public ConstraintBodiesStructBlock ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStructBlock : TagStructure
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
            public ConstraintBodiesStructBlock ConstraintBodies;
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
            public class ConstraintBodiesStructBlock : TagStructure
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

        [TagStructure(Size = 0xC)]
        public class RegionsBlock : TagStructure
        {
            public StringId Name;
            public List<PermutationsBlock> Permutations;

            [TagStructure(Size = 0xC)]
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

        [TagStructure(Size = 0x250)]
        public class GlobalTagImportInfoBlock : TagStructure
        {
            public int Build;
            [TagField(Length = 256)]
            public string Version;
            [TagField(Length = 32)]
            public string ImportDate;
            [TagField(Length = 32)]
            public string Culprit;
            [TagField(Length = 0x60, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string ImportTime;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<TagImportFileBlock> Files;
            [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;

            [TagStructure(Size = 0x210)]
            public class TagImportFileBlock : TagStructure
            {
                [TagField(Length = 256)]
                public string Path;
                [TagField(Length = 32)]
                public string ModificationDate;
                [TagField(Length = 0x8)]
                public byte[] Unknown;
                [TagField(Length = 0x58, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int Checksum; // crc32
                public int Size; // bytes
                public byte[] ZippedData;
                [TagField(Length = 0x80, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
            }
        }

        [TagStructure(Size = 0x2A4)]
        public class GlobalErrorReportCategoriesBlock : TagStructure
        {
            [TagField(Length = 256)]
            public string Name;
            public ReportTypeValue ReportType;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x194, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public List<ErrorReportsBlock> Reports;

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

            [TagStructure(Size = 0x260)]
            public class ErrorReportsBlock : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public byte[] Text;
                [TagField(Length = 32)]
                public string SourceFilename;
                public int SourceLineNumber;
                public List<ErrorReportVerticesBlock> Vertices;
                public List<ErrorReportVectorsBlock> Vectors;
                public List<ErrorReportLinesBlock> Lines;
                public List<ErrorReportTrianglesBlock> Triangles;
                public List<ErrorReportQuadsBlock> Quads;
                public List<ErrorReportCommentsBlock> Comments;
                [TagField(Length = 0x17C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public int ReportKey;
                public int NodeIndex;
                public Bounds<float> BoundsX;
                public Bounds<float> BoundsY;
                public Bounds<float> BoundsZ;
                public RealArgbColor Color;
                [TagField(Length = 0x54, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;

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
                public class ErrorReportVerticesBlock : TagStructure
                {
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                    public float ScreenSize;
                }

                [TagStructure(Size = 0x40)]
                public class ErrorReportVectorsBlock : TagStructure
                {
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                    public RealVector3d Normal;
                    public float ScreenLength;
                }

                [TagStructure(Size = 0x3C)]
                public class ErrorReportLinesBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 2)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }

                [TagStructure(Size = 0x48)]
                public class ErrorReportTrianglesBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 3)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }

                [TagStructure(Size = 0x54)]
                public class ErrorReportQuadsBlock : TagStructure
                {
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    [TagField(Length = 4)]
                    public RealPoint3d[] Position;
                    public RealArgbColor Color;
                }

                [TagStructure(Size = 0x38)]
                public class ErrorReportCommentsBlock : TagStructure
                {
                    public byte[] Text;
                    public RealPoint3d Position;
                    [TagField(Length = 4)]
                    public sbyte[] NodeIndex;
                    [TagField(Length = 4)]
                    public float[] NodeWeight;
                    public RealArgbColor Color;
                }
            }
        }

        [TagStructure(Size = 0x10)]
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
            public ConstraintBodiesStructBlock ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float LimitFriction;
            public float LimitMinAngle;
            public float LimitMaxAngle;

            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStructBlock : TagStructure
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
            public ConstraintBodiesStructBlock ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStructBlock : TagStructure
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
            public ConstraintBodiesStructBlock ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float SpringLength;

            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStructBlock : TagStructure
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
            public ConstraintBodiesStructBlock ConstraintBodies;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float MinLimit;
            public float MaxLimit;
            public float MaxFrictionForce;

            [TagStructure(Size = 0x74)]
            public class ConstraintBodiesStructBlock : TagStructure
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

        [TagStructure(Size = 0x20)]
        public class PhantomsBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Unknown;
            public short Size;
            public short Count;
            [TagField(Length = 0x4)]
            public byte[] Unknown1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4)]
            public byte[] Unknown2;
            public short Size1;
            public short Count1;
            [TagField(Length = 0x4)]
            public byte[] Unknown3;
        }
    }
}

