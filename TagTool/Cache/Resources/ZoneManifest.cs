using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Size = 0x78, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0xA0, MinVersion = CacheVersion.HaloReach)]
    public class ZoneManifest : TagStructure
    {
        public TagBlockBitVector RequiredResourcesBitVector;
        public TagBlockBitVector UnusedResourcesBitVector;
        public TagBlockBitVector OptionalResourcesBitVector;
        public TagBlockBitVector StreamedResourcesBitVector;

        public ZoneResourceUsage OverallUsage;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public StringId Name;

        public List<ZoneResourceUsage> ResourceUsage;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ZoneResourceUsage> BudgetUsage;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<ZoneResourceUsage> UniqueBudgetUsage;

        public TagBlockBitVector ActiveResourceOwners;
        public TagBlockBitVector TopLevelResourceOwners;

        public TagBlock<ZoneResourceVisitNode> VisitationHeirarchy;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint ActiveBspMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint TouchedBspMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint DesignerZoneMask;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint CinematicZoneMask;
    }
}