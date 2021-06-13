using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "forge_globals", Tag = "forg", Size = 0x58)]
    public class ForgeGlobals : TagStructure
    {
        public List<ForgeColorBlock> ForgeColors;
        [TagField(ValidTags = new [] { "efsc" })]
        public CachedTag MagnetEffectScenery;
        public StringId ParentMagnetMarkerName;
        public StringId ChildMagnetMarkerName;
        public float ThrottleSensitivity;
        // multiplied against selected object bounding sphere radius
        public float VLowMagnetismFactor;
        // multiplied against selected object bounding sphere radius
        public float LowMagnetismFactor;
        // multiplied against selected object bounding sphere radius
        public float MedMagnetismFactor;
        // multiplied against selected object bounding sphere radius
        public float HighMagnetismFactor;
        // multiplied against selected object bounding sphere radius
        public float VHighMagnetismFactor;
        // multiplied against selected object bounding sphere radius
        public float MagnetismAngle;
        // influence of magnet selection based on selection center in forge
        public float MagnetOffsetInfluence;
        // how close you must be to an object in order to grab it for manipulation
        public float ObjectGrabRange;
        // default manipulation camera distance
        public float DefaultFocalDistance;
        // minimum manipulation camera distance
        public float MinimumFocalDistance;
        // maximum manipulation camera distance
        public float MaximumFocalDistance;
        // effective maximum distance = MIN(max_focal_distance, multiplier x min_focal_distance)
        public float FocalDistanceMultiplier;
        
        [TagStructure(Size = 0x10)]
        public class ForgeColorBlock : TagStructure
        {
            public StringId Name;
            public RealRgbColor Color;
        }
    }
}
