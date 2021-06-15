using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenery", Tag = "scen", Size = 0x18)]
    public class Scenery : GameObject
    {
        public PathfindingPolicyEnum PathfindingPolicy;
        public SceneryFlags Flags;
        public LightmappingPolicyEnum LightmappingPolicy;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "stli" })]
        public CachedTag StructureLightingTag;
        
        public enum PathfindingPolicyEnum : short
        {
            PathfindingCutOut,
            PathfindingStatic,
            PathfindingDynamic,
            PathfindingNone
        }
        
        [Flags]
        public enum SceneryFlags : ushort
        {
            // has no havok representation; will not build physics from collision
            NotPhysical = 1 << 0,
            // tests all clusters for activation instead of just the origin
            UseComplexActivation = 1 << 1
        }
        
        public enum LightmappingPolicyEnum : short
        {
            PerVertex,
            PerPixel,
            Dynamic
        }
    }
}
