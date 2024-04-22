using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_lights_resource", Tag = "*igh", Size = 0x34)]
    public class ScenarioLightsResource : TagStructure
    {
        public List<ScenarioObjectNamesBlock> Names;
        public List<DontUseMeScenarioEnvironmentObjectBlock> Unknown;
        public List<ScenarioStructureBspReferenceBlock> StructureReferences;
        public List<ScenarioLightPaletteBlock> Palette;
        public List<ScenarioLightBlock> Objects;
        public int NextObjectIdSalt;
        public List<GScenarioEditorFolderBlock> EditorFolders;
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectNamesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown;
            public short Unknown1;
        }
        
        [TagStructure(Size = 0x40)]
        public class DontUseMeScenarioEnvironmentObjectBlock : TagStructure
        {
            public short Bsp;
            public short Unknown;
            public int UniqueId;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Tag ObjectDefinitionTag;
            public int Object;
            [TagField(Length = 0x2C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioStructureBspReferenceBlock : TagStructure
        {
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "sbsp" })]
            public CachedTag StructureBsp;
            [TagField(ValidTags = new [] { "ltmp" })]
            public CachedTag StructureLightmap;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float UnusedRadianceEstSearchDistance;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float UnusedLuminelsPerWorldUnit;
            public float UnusedOutputWhiteReference;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public short DefaultSky;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DefaultSkyEnabled = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class ScenarioLightPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh" })]
            public CachedTag Name;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x6C)]
        public class ScenarioLightBlock : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatumStructBlock ObjectData;
            public ScenarioDeviceStructBlock DeviceData;
            public ScenarioLightStructBlock LightData;
            
            [TagStructure(Size = 0x30)]
            public class ScenarioObjectDatumStructBlock : TagStructure
            {
                public PlacementFlagsValue PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public TransformFlagsValue TransformFlags;
                public ushort ManualBspFlags;
                public ScenarioObjectIdStructBlock ObjectId;
                public BspPolicyValue BspPolicy;
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public short EditorFolder;
                
                [Flags]
                public enum PlacementFlagsValue : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    LockTypeToEnvObject = 1 << 4,
                    LockTransformToEnvObject = 1 << 5,
                    NeverPlaced = 1 << 6,
                    LockNameToEnvObject = 1 << 7,
                    CreateAtRest = 1 << 8
                }
                
                [Flags]
                public enum TransformFlagsValue : ushort
                {
                    Mirrored = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class ScenarioObjectIdStructBlock : TagStructure
                {
                    public int UniqueId;
                    public short OriginBspIndex;
                    public TypeValue Type;
                    public SourceValue Source;
                    
                    public enum TypeValue : sbyte
                    {
                        Biped,
                        Vehicle,
                        Weapon,
                        Equipment,
                        Garbage,
                        Projectile,
                        Scenery,
                        Machine,
                        Control,
                        LightFixture,
                        SoundScenery,
                        Crate,
                        Creature
                    }
                    
                    public enum SourceValue : sbyte
                    {
                        Structure,
                        Editor,
                        Dynamic,
                        Legacy
                    }
                }
                
                public enum BspPolicyValue : sbyte
                {
                    Default,
                    AlwaysPlaced,
                    ManualBspPlacement
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceStructBlock : TagStructure
            {
                public short PowerGroup;
                public short PositionGroup;
                public FlagsValue Flags;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    InitiallyOpen10 = 1 << 0,
                    InitiallyOff00 = 1 << 1,
                    CanChangeOnlyOnce = 1 << 2,
                    PositionReversed = 1 << 3,
                    NotUsableFromAnySide = 1 << 4
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class ScenarioLightStructBlock : TagStructure
            {
                public TypeValue Type;
                public FlagsValue Flags;
                public LightmapTypeValue LightmapType;
                public LightmapFlagsValue LightmapFlags;
                public float LightmapHalfLife;
                public float LightmapLightScale;
                public RealPoint3d TargetPoint;
                public float Width; // World Units*
                public float HeightScale; // World Units*
                public Angle FieldOfView; // Degrees*
                public float FalloffDistance; // World Units*
                public float CutoffDistance; // World Units (from Far Plane)*
                
                public enum TypeValue : short
                {
                    Sphere,
                    Orthogonal,
                    Projective,
                    Pyramid
                }
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    CustomGeometry = 1 << 0,
                    Unused = 1 << 1,
                    CinematicOnly = 1 << 2
                }
                
                public enum LightmapTypeValue : short
                {
                    UseLightTagSetting,
                    DynamicOnly,
                    DynamicWithLightmaps,
                    LightmapsOnly
                }
                
                [Flags]
                public enum LightmapFlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class GScenarioEditorFolderBlock : TagStructure
        {
            public int ParentFolder;
            [TagField(Length = 256)]
            public string Name;
        }
    }
}

