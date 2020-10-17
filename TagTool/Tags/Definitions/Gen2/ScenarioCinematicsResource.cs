using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_cinematics_resource", Tag = "cin*", Size = 0x18)]
    public class ScenarioCinematicsResource : TagStructure
    {
        public List<ScenarioCutsceneFlagBlock> Flags;
        public List<ScenarioCutsceneCameraPointBlock> CameraPoints;
        public List<RecordedAnimationBlock> RecordedAnimations;
        
        [TagStructure(Size = 0x38)]
        public class ScenarioCutsceneFlagBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioCutsceneCameraPointBlock : TagStructure
        {
            public FlagsValue Flags;
            public TypeValue Type;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles3d Orientation;
            public Angle Unused;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                EditAsRelative = 1 << 0
            }
            
            public enum TypeValue : short
            {
                Normal,
                IgnoreTargetOrientation,
                Dolly,
                IgnoreTargetUpdates
            }
        }
        
        [TagStructure(Size = 0x34)]
        public class RecordedAnimationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short LengthOfAnimation; // ticks
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public byte[] RecordedAnimationEventStream;
        }
    }
}

