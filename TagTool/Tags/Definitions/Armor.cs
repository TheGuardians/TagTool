using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "armor", Tag = "armr", Size = 0x28, MinVersion = CacheVersion.HaloOnline430475)]
    public class Armor : GameObject
    {
        public CachedTagInstance ParentModel;
        public CachedTagInstance FirstPersonModel;
        public CachedTagInstance ThirdPersonModel;

        [TagField(Flags = TagFieldFlags.Padding, Length = 4)]
        public byte[] Unused4;
    }
}