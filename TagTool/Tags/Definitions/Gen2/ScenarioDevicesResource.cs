using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_devices_resource", Tag = "dgr*", Size = 0x90)]
    public class ScenarioDevicesResource : TagStructure
    {
        public List<ScenarioObjectName> Names;
        public List<ScenarioEnvironmentObject> Unknown1;
        public List<ScenarioStructureBspReference> StructureReferences;
        public List<ScenarioDeviceGroup> DeviceGroups;
        public List<ScenarioMachine> Machines;
        public List<ScenarioObjectPaletteEntry> MachinesPalette;
        public List<ScenarioControl> Controls;
        public List<ScenarioObjectPaletteEntry> ControlsPalette;
        public List<ScenarioLightFixture> LightFixtures;
        public List<ScenarioObjectPaletteEntry> LightFixturesPalette;
        public int NextMachineIdSalt;
        public int NextControlIdSalt;
        public int NextLightFixtureIdSalt;
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
        
        [TagStructure(Size = 0x28)]
        public class ScenarioDeviceGroup : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public float InitialValue; // [0,1]
            public FlagsValue Flags;
            
            [Flags]
            public enum FlagsValue : uint
            {
                CanChangeOnlyOnce = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x4C)]
        public class ScenarioMachine : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioMachineDatum MachineData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            
            [TagStructure(Size = 0x10)]
            public class ScenarioMachineDatum : TagStructure
            {
                public FlagsValue Flags;
                public List<PathfindingObjectIndexList> PathfindingReferences;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    DoesNotOperateAutomatically = 1 << 0,
                    OneSided = 1 << 1,
                    NeverAppearsLocked = 1 << 2,
                    OpenedByMeleeAttack = 1 << 3,
                    OneSidedForPlayer = 1 << 4,
                    DoesNotCloseAutomatically = 1 << 5
                }
                
                [TagStructure(Size = 0x4)]
                public class PathfindingObjectIndexList : TagStructure
                {
                    public short BspIndex;
                    public short PathfindingObjectIndex;
                }
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class ScenarioObjectPaletteEntry : TagStructure
        {
            public CachedTag Name;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x44)]
        public class ScenarioControl : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioControlDatum ControlData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioControlDatum : TagStructure
            {
                public FlagsValue Flags;
                public short DonTTouchThis;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    UsableFromBothSides = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class ScenarioLightFixture : TagStructure
        {
            public short Type;
            public short Name;
            public ScenarioObjectDatum ObjectData;
            public ScenarioDeviceDatum DeviceData;
            public ScenarioLightFixtureDatum LightFixtureData;
            
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
            
            [TagStructure(Size = 0x8)]
            public class ScenarioDeviceDatum : TagStructure
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
            
            [TagStructure(Size = 0x18)]
            public class ScenarioLightFixtureDatum : TagStructure
            {
                public RealRgbColor Color;
                public float Intensity;
                public Angle FalloffAngle; // Degrees
                public Angle CutoffAngle; // Degrees
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

