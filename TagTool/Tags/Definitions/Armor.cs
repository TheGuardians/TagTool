using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "armor", Tag = "armr", Size = 0x28, MinVersion = CacheVersion.HaloOnline430475)]
    public class Armor : GameObject
    {
        public CachedTag ParentModel;
        public CachedTag FirstPersonModel;
        public CachedTag ThirdPersonModel;

        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unused4;
    }
}