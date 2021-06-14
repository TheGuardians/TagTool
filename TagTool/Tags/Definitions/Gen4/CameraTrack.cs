using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "camera_track", Tag = "trak", Size = 0x10)]
    public class CameraTrack : TagStructure
    {
        public CameraTrackFlags Flags;
        public List<CameraTrackControlPointBlock> ControlPoints;
        [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [Flags]
        public enum CameraTrackFlags : uint
        {
        }
        
        [TagStructure(Size = 0x1C)]
        public class CameraTrackControlPointBlock : TagStructure
        {
            public RealVector3d Position;
            public RealQuaternion Orientation;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}
