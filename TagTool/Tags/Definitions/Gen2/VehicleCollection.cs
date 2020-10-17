using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vehicle_collection", Tag = "vehc", Size = 0xC)]
    public class VehicleCollection : TagStructure
    {
        public List<VehiclePermutation> VehiclePermutations;
        public short SpawnTimeInSeconds0Default;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x10)]
        public class VehiclePermutation : TagStructure
        {
            /// <summary>
            /// relatively how likely this vehicle will be chosen
            /// </summary>
            public float Weight;
            /// <summary>
            /// which vehicle to
            /// </summary>
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag Vehicle;
            public StringId VariantName;
        }
    }
}

