using TagTool.Cache;
using TagTool.Cache.Monolithic;
using TagTool.Common;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Beta)]
    public class TagResourceReference : TagStructure
    {

        [TagField(Gen = CacheGeneration.Second)]
        public uint Gen2ResourceAddress;

        /// <summary>
        /// ID is an index in ResourceGestalt.TagResources
        /// </summary>
        [TagField(MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.Halo3ODST)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DatumHandle Gen3ResourceID;

        /// <summary>
        /// PageableResource structure (as a pointer)
        /// </summary>
        [TagField(Gen = CacheGeneration.HaloOnline, Flags = TagFieldFlags.Pointer)]
        public PageableResource HaloOnlinePageableResource;

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public int Unused;

        // TODO: consider making storing in the existing PageableResource field
        [TagField(Flags = TagFieldFlags.Runtime)]
        public TagResourceXSyncState XSyncState;
    }
}
