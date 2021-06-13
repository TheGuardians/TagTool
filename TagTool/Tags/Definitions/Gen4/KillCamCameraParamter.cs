using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "KillCamCameraParamter", Tag = "kccd", Size = 0xC)]
    public class KillCamCameraParamter : TagStructure
    {
        public float DistanceFromCamera;
        public float HeightAboveObject;
        public float MinimumVelocityToUpdate;
    }
}
