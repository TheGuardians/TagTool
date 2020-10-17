using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_trigger_volumes_resource", Tag = "trg*", Size = 0x10)]
    public class ScenarioTriggerVolumesResource : TagStructure
    {
        public List<ScenarioTriggerVolumeBlock> KillTriggerVolumes;
        public List<ScenarioObjectNamesBlock> ObjectNames;
        
        [TagStructure(Size = 0x44)]
        public class ScenarioTriggerVolumeBlock : TagStructure
        {
            public StringId Name;
            public short ObjectName;
            [TagField(Length = 0x2)]
            public byte[] Unknown;
            public StringId NodeName;
            [TagField(Length = 6)]
            public float[] Unknown1;
            public RealPoint3d Position;
            public RealPoint3d Extents;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short KillTriggerVolume;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectNamesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown;
            public short Unknown1;
        }
    }
}

