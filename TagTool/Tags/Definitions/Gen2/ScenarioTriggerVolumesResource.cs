using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_trigger_volumes_resource", Tag = "trg*", Size = 0x18)]
    public class ScenarioTriggerVolumesResource : TagStructure
    {
        public List<ScenarioTriggerVolume> KillTriggerVolumes;
        public List<ScenarioObjectName> ObjectNames;
        
        [TagStructure(Size = 0x44)]
        public class ScenarioTriggerVolume : TagStructure
        {
            public StringId Name;
            public short ObjectName;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unknown1;
            public StringId NodeName;
            public float Unknown3;
            [TagField(Length = 6)]
            public float EmptyString;
            public RealPoint3d Position;
            public RealPoint3d Extents;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public short KillTriggerVolume;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x24)]
        public class ScenarioObjectName : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short Unknown1;
            public short Unknown2;
        }
    }
}

