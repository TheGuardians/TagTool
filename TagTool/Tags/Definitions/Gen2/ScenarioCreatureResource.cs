using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_creature_resource", Tag = "*rea", Size = 0x4C)]
    public class ScenarioCreatureResource : TagStructure
    {
        public List<ScenarioObjectName> Names;
        public List<ScenarioEnvironmentObject> Unknown1;
        public List<ScenarioStructureBspReference> StructureReferences;
        public List<ScenarioObjectPaletteEntry> Palette;
        public List<ScenarioCreature> Objects;
        public int NextObjectIdSalt;
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
        
        [TagStructure(Size = 0x34)]
        public class ScenarioCreature : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            
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

