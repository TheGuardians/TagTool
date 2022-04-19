using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenery", Tag = "scen", Size = 0x8)]
    public class Scenery : GameObject
    {
        public PathfindingPolicyValue PathfindingPolicy;
        public SceneryFlagBits SceneryFlags;
        public LightmappingPolicyValue LightmappingPolicy;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Pad;

        public enum PathfindingPolicyValue : short
        {
            CutOut,
            Static,
            Dynamic,
            None
        }

        [Flags]
        public enum SceneryFlagBits : ushort
        {
            None,
            PhysicallySimulates = 1 << 0,
            UseComplexActivation = 1 << 1
        }

        public enum LightmappingPolicyValue : short
        {
            PerVertex,
            PerPixelNotImplemented,
            Dynamic
        }
    }
}