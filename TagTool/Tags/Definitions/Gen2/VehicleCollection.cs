using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vehicle_collection", Tag = "vehc", Size = 0x10)]
    public class VehicleCollection : TagStructure
    {
        public List<VehiclePermutationDefinition> VehiclePermutations;
        public short SpawnTimeInSeconds0Default;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        
        [TagStructure(Size = 0x18)]
        public class VehiclePermutationDefinition : TagStructure
        {
            public float Weight; // relatively how likely this vehicle will be chosen
            public CachedTag Vehicle; // which vehicle to 
            public StringId VariantName;
        }
    }
}

