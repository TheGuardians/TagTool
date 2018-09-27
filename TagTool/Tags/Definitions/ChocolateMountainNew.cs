using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chocolate_mountain_new", Tag = "chmt", Size = 0xC)]
    public class ChocolateMountainNew : TagStructure
	{
        public List<LightingVariable> LightingVariables;

        [TagStructure(Size = 0x4)]
        public class LightingVariable : TagStructure
		{
            public float LightmapBrightnessOffset;
        }
    }
}
