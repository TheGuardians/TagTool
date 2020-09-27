using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "camera_track", Tag = "trak", Size = 0x10)]
    public class CameraTrack : TagStructure
    {
        public FlagsValue Flags;
        public List<CameraTrackControlPoint> ControlPoints;
        
        [Flags]
        public enum FlagsValue : uint
        {
        }
        
        [TagStructure(Size = 0x1C)]
        public class CameraTrackControlPoint : TagStructure
        {
            public RealVector3d Position;
            public RealQuaternion Orientation;
        }
    }
}

