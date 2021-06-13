using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "structure_meta", Tag = "smet", Size = 0x3C)]
    public class StructureMeta : TagStructure
    {
        public List<StructurebspFxMarkerBlock> EffectsMarkers;
        public List<ScenarioAirprobesBlock> Airprobes;
        public List<StructuremetadataLightConeMarkerBlock> LightCones;
        public List<StructureBspEnvironmentObjectPaletteBlock> ObjectPalette;
        public List<StructureBspEnvironmentObjectBlock> Objects;
        
        [TagStructure(Size = 0x4C)]
        public class StructurebspFxMarkerBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string MarkerName;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            [TagField(ValidTags = new [] { "effe","lens" })]
            public CachedTag OptionalAttachedEffect;
        }
        
        [TagStructure(Size = 0x14)]
        public class ScenarioAirprobesBlock : TagStructure
        {
            public RealPoint3d AirprobePosition;
            public StringId AirprobeName;
            public int BspIndex;
        }
        
        [TagStructure(Size = 0x78)]
        public class StructuremetadataLightConeMarkerBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string MarkerName;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float Length;
            public float Width;
            public float Intensity;
            public RealArgbColor LightColor;
            [TagField(ValidTags = new [] { "licn" })]
            public CachedTag LightConeTag;
            [TagField(ValidTags = new [] { "crvs" })]
            public CachedTag IntensityCurve;
        }
        
        [TagStructure(Size = 0x24)]
        public class StructureBspEnvironmentObjectPaletteBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "obje" })]
            public CachedTag Definition;
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag Model;
            public int Gveyn;
        }
        
        [TagStructure(Size = 0x54)]
        public class StructureBspEnvironmentObjectBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Translation;
            public float Scale;
            public short PaletteIndex;
            public EnvironmentobjectFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public int UniqueId;
            public Tag ExportedObjectType;
            public StringId ScenarioObjectName;
            public StringId VariantName;
            
            [Flags]
            public enum EnvironmentobjectFlags : byte
            {
                ScriptsAlwaysRun = 1 << 0
            }
        }
    }
}
