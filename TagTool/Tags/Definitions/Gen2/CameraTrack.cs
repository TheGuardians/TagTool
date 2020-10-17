using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "camera_track", Tag = "trak", Size = 0xC)]
    public class CameraTrack : TagStructure
    {
        public FlagsValue Flags;
        public List<CameraTrackControlPointBlock> ControlPoints;
        
        [Flags]
        public enum FlagsValue : uint
        {
        }
        
        [TagStructure(Size = 0x1C)]
        public class CameraTrackControlPointBlock : TagStructure
        {
            public RealVector3d Position;
            public RealQuaternion Orientation;
        }
    }
}

