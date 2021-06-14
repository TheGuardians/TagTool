using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "portrait_poses_definition", Tag = "ppod", Size = 0xC)]
    public class PortraitPosesDefinition : TagStructure
    {
        public List<GuiPortraitPoseBlock> PortraitPoses;
        
        [TagStructure(Size = 0x10)]
        public class GuiPortraitPoseBlock : TagStructure
        {
            public StringId PoseName;
            public StringId AnimationName;
            public StringId CameraViewName;
            public int ScenarioProfileIndex;
        }
    }
}
