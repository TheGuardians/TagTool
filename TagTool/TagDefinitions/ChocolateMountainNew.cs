using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "chocolate_mountain_new", Tag = "chmt", Size = 0xC)]
    public class ChocolateMountainNew
    {
        public List<LightingVariable> LightingVariables;

        [TagStructure(Size = 0x4)]
        public class LightingVariable
        {
            public float LightmapBrightnessOffset;
        }
    }
}
