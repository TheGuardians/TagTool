using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "camera_track", Tag = "trak", Size = 0x30)]
    public class CameraTrack : TagStructure
    {
        public FlagsValue Flags;
        public List<CameraTrackControlPointBlock> ControlPoints;
        [TagField(Length = 0x20)]
        public byte[] Padding;
        
        [Flags]
        public enum FlagsValue : uint
        {
        }
        
        [TagStructure(Size = 0x3C)]
        public class CameraTrackControlPointBlock : TagStructure
        {
            public RealVector3d Position;
            public RealQuaternion Orientation;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
    }
}

