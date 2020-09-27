using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_scenery_resource", Tag = "*cen", Size = 0x68)]
    public class ScenarioSceneryResource : TagStructure
    {
        public List<ScenarioObjectName> Names;
        public List<ScenarioEnvironmentObject> Unknown1;
        public List<ScenarioStructureBspReference> StructureReferences;
        public List<ScenarioObjectPaletteEntry> Palette;
        public List<ScenarioScenery> Objects;
        public int NextSceneryObjectIdSalt;
        public List<ScenarioObjectPaletteEntry> Palette1;
        public List<ScenarioCrate> Objects2;
        public int NextBlockObjectIdSalt;
        public List<ScenarioEditorFolder> EditorFolders;
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectName : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown1;
            public short Unknown2;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioEnvironmentObject : TagStructure
        {
            public short Bsp;
            public short Unknown2;
            public int UniqueId;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public Tag ObjectDefinitionTag;
            public int Object;
            [TagField(Flags = Padding, Length = 44)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioStructureBspReference : TagStructure
        {
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public CachedTag StructureBsp;
            public CachedTag StructureLightmap;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float UnusedRadianceEstSearchDistance;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public float UnusedLuminelsPerWorldUnit;
            public float UnusedOutputWhiteReference;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding4;
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding5;
            public short DefaultSky;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding6;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                DefaultSkyEnabled = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioObjectPaletteEntry : TagStructure
        {
            public CachedTag Name;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x60)]
        public class ScenarioScenery : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            public ScenarioSceneryDatum SceneryData;
            
            [TagStructure(Size = 0x30)]
            public class ScenarioObjectDatum : TagStructure
            {
                public PlacementFlagsValue PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public TransformFlagsValue TransformFlags;
                public ushort[] ManualBspFlags;
                public ObjectIdentifier ObjectId;
                public BspPolicyValue BspPolicy;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                public short EditorFolder;
                
                [Flags]
                public enum PlacementFlagsValue : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused = 1 << 1,
                    Unused0 = 1 << 2,
                    Unused1 = 1 << 3,
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
                public class ObjectIdentifier : TagStructure
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
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutation : TagStructure
            {
                public StringId VariantName;
                public ActiveChangeColorsValue ActiveChangeColors;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ActiveChangeColorsValue : uint
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ScenarioSceneryDatum : TagStructure
            {
                public PathfindingPolicyValue PathfindingPolicy;
                public LightmappingPolicyValue LightmappingPolicy;
                public List<PathfindingObjectIndexList> PathfindingReferences;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public ValidMultiplayerGamesValue ValidMultiplayerGames;
                
                public enum PathfindingPolicyValue : short
                {
                    TagDefault,
                    PathfindingDynamic,
                    PathfindingCutOut,
                    PathfindingStatic,
                    PathfindingNone
                }
                
                public enum LightmappingPolicyValue : short
                {
                    TagDefault,
                    Dynamic,
                    PerVertex
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexList : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
                
                [Flags]
                public enum ValidMultiplayerGamesValue : ushort
                {
                    CaptureTheFlag = 1 << 0,
                    Slayer = 1 << 1,
                    Oddball = 1 << 2,
                    KingOfTheHill = 1 << 3,
                    Juggernaut = 1 << 4,
                    Territories = 1 << 5,
                    Assault = 1 << 6
                }
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class ScenarioCrate : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioObjectPermutation PermutationData;
            
            [TagStructure(Size = 0x30)]
            public class ScenarioObjectDatum : TagStructure
            {
                public PlacementFlagsValue PlacementFlags;
                public RealPoint3d Position;
                public RealEulerAngles3d Rotation;
                public float Scale;
                public TransformFlagsValue TransformFlags;
                public ushort[] ManualBspFlags;
                public ObjectIdentifier ObjectId;
                public BspPolicyValue BspPolicy;
                [TagField(Flags = Padding, Length = 1)]
                public byte[] Padding1;
                public short EditorFolder;
                
                [Flags]
                public enum PlacementFlagsValue : uint
                {
                    NotAutomatically = 1 << 0,
                    Unused = 1 << 1,
                    Unused0 = 1 << 2,
                    Unused1 = 1 << 3,
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
                public class ObjectIdentifier : TagStructure
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
            
            [TagStructure(Size = 0x18)]
            public class ScenarioObjectPermutation : TagStructure
            {
                public StringId VariantName;
                public ActiveChangeColorsValue ActiveChangeColors;
                public ArgbColor PrimaryColor;
                public ArgbColor SecondaryColor;
                public ArgbColor TertiaryColor;
                public ArgbColor QuaternaryColor;
                
                [Flags]
                public enum ActiveChangeColorsValue : uint
                {
                    Primary = 1 << 0,
                    Secondary = 1 << 1,
                    Tertiary = 1 << 2,
                    Quaternary = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x104)]
        public class ScenarioEditorFolder : TagStructure
        {
            public int ParentFolder;
            [TagField(Length = 256)]
            public string Name;
        }
    }
}

