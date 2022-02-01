using System.Collections.Generic;
using TagTool.Cache.Interops;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Cache.Resources
{
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x220, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "cache_file_resource_gestalt", Tag = "zone", Size = 0x214, MaxVersion = CacheVersion.HaloReach11883, Platform = CachePlatform.Original)]
    public class ResourceGestalt : TagStructure
	{
        [TagField(MaxVersion = CacheVersion.Halo3Retail)]
        public MapTypeHalo3RetailValue MapTypeHalo3Retail;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public MapTypeHalo3OdstValue MapTypeHalo3Odst;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        public MapSubTypeHalo3OdstValue MapSubTypeHalo3Odst;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public MapTypeHalo3RetailValue MapSubTypeHaloReach;

        public ScenarioFlags Flags;

        public List<ResourceDefinition> ResourceDefinitions;
        public List<InteropDefinition> InteropDefinitions;

        public ResourceLayoutTable LayoutTable = new ResourceLayoutTable();

        public List<ResourceData> TagResources;

        public List<ZoneManifest> DesignerZoneManifests;
        public List<ZoneManifest> GlobalZoneManifests;
        public List<ZoneManifest> HsZoneManifests;
        public List<ZoneManifest> UnattachedDesignerZoneManifests;
        public List<ZoneManifest> DvdForbiddenZoneManifests;
        public List<ZoneManifest> DvdAlwaysStreamingZoneManifests;
        public List<ZoneManifest> DefaultBspZoneManifests;
        public List<ZoneManifest> StaticBspZoneManifests;
        public List<ZoneManifest> DynamicBspZoneManifests;
        public List<ZoneManifest> CinematicZoneManifests;

        public List<ZoneManifest> ZonesOnlyZoneSetManifests;
        public List<ZoneManifest> ExpectedZoneManifests;
        public List<ZoneManifest> FullyPopulatedZoneManifests;

        public List<ZoneSetZoneUsage> ZoneSetZoneUsages;

        public List<CachedTag> BspReferences;
        public List<TagReferenceBlock> TagReferences;

        public List<ModelVariantUsage> ModelVariantUsages;
        public List<CharacterUsage> CharacterUsages;

        public byte[] DefinitionData;

        public uint MinimumCompletePageableDataSize;
        public uint MinimumRequiredPageableDataSize;
        public uint MinimumRequiredDvdDataSize;

        public uint GlobalPageableDataSize;
        public uint OptionalControlDataSize;

        public List<ZoneResourceUsage> GlobalResourceUsage;

        [TagField(Flags = TagFieldFlags.Padding, Length = 96)]
        public byte[] Unused = new byte[96];

        public List<PredictionQuantum> PredictionQuanta;
        public List<PredictionAtom> PredictionAtoms;
        public List<PredictionMoleculeAtom> PredictionMoleculeAtoms;
        public List<PredictionMolecule> PredictionMolecules;
        public List<PredictionMoleculeKey> PredictionMoleculeKeys;

        public int CampaignId;
        public int MapId;

        public enum MapTypeHalo3RetailValue : short
        {
            SinglePlayer,
            Multiplayer,
            MainMenu
        }
        
        public enum MapTypeHalo3OdstValue : sbyte
        {
            SinglePlayer,
            Multiplayer,
            MainMenu
        }

        public enum MapSubTypeHalo3OdstValue : sbyte
        {
            None,
            Hub,
            Level,
            Scene,
            Cinematic
        }
    }
}