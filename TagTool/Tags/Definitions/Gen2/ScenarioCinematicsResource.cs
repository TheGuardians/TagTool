using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenario_cinematics_resource", Tag = "cin*", Size = 0x24)]
    public class ScenarioCinematicsResource : TagStructure
    {
        public List<ScenarioCutsceneFlag> Flags;
        public List<ScenarioCutsceneCameraPoint> CameraPoints;
        public List<RecordedAnimationDefinition> RecordedAnimations;
        
        [TagStructure(Size = 0x38)]
        public class ScenarioCutsceneFlag : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string Name;
            public RealPoint3d Position;
            public RealEulerAngles2d Facing;
        }
        
        [TagStructure(Size = 0x40)]
        public class ScenarioCutsceneCameraPoint : TagStructure
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
        
        [TagStructure(Size = 0x40)]
        public class RecordedAnimationDefinition : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public sbyte Version;
            public sbyte RawAnimationData;
            public sbyte UnitControlDataVersion;
            [TagField(Flags = Padding, Length = 1)]
            public byte[] Padding1;
            public short LengthOfAnimation; // ticks
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public byte[] RecordedAnimationEventStream;
        }
    }
}

