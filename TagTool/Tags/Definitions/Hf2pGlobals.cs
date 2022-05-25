using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "hf2p_globals", Tag = "hf2p", Size = 0x10, MinVersion = CacheVersion.HaloOnline498295)]
    public class Hf2pGlobals : TagStructure
	{
        public List<Armor> RaceArmors;

        [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloOnline498295)]
        public class Armor : TagStructure
		{
            public List<Gender> Genders;

            [TagStructure(Size = 0xC, MinVersion = CacheVersion.HaloOnline498295)]
            public class Gender : TagStructure
			{
                public List<ArmorObject> ArmorObjects;

                [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloOnline498295)]
                public class ArmorObject : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Name;
                    public CachedTag Armor;
                    public StringId Variant;
                }
            }
        }
    }
}