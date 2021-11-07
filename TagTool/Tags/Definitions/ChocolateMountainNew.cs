using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chocolate_mountain_new", Tag = "chmt", Size = 0xC, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "chocolate_mountain_new", Tag = "chmt", Size = 0x10, MinVersion = CacheVersion.HaloReach)]
    public class ChocolateMountainNew : TagStructure
	{
		[TagField(MinVersion = CacheVersion.HaloReach)]
		public sbyte Unknown1;
		
		[TagField(MinVersion = CacheVersion.HaloReach)]
		public sbyte Unknown2;
		
		[TagField(MinVersion = CacheVersion.HaloReach)]
		public sbyte Unknown3;
		
		[TagField(MinVersion = CacheVersion.HaloReach)]
		public sbyte Unknown4;
		
        public List<PerObjectTypeRelativeMinLuminanceBlock> PerObjectTypeSettings;

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x14, MinVersion = CacheVersion.HaloReach)]
        public class PerObjectTypeRelativeMinLuminanceBlock : TagStructure
		{
            public float MinimumLuminance;
			
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float LightmapDarknessOffset;
			
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float Unknown;
			
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float DynamicLightmapBrightnessOffset;
			
			[TagField(MinVersion = CacheVersion.HaloReach)]
			public float DynamicLightmapDarknessOffset;
        }
    }
}
