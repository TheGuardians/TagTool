using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "armor", Tag = "armr", Size = 0x28, MinVersion = CacheVersion.HaloOnline430475)]
    public class Armor : GameObject
    {
        public CachedTagInstance ParentModel;
        public CachedTagInstance FirstPersonModel;
        public CachedTagInstance ThirdPersonModel;

        [TagField(Padding = true, Length = 4)]
        public byte[] Unused4;
    }
}