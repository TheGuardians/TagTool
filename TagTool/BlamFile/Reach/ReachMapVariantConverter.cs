using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.BlamFile.Reach
{
    public class ReachMapVariantConverter
    {
        public HashSet<string> ExcludedTags = new HashSet<string>();
        public Dictionary<string, string> SubstitutedTags = new Dictionary<string, string>();

        public Blf Convert(Scenario sourceScenario, ReachBlfMapVariant sourceBlf)
        {
            var sourceMapVariant = sourceBlf.MapVariant;
            var metadata = new ContentItemMetadata();
            metadata.Name = sourceMapVariant.Name;
            metadata.Description = sourceMapVariant.Description;
            metadata.Author = sourceMapVariant.Metadata.ModificationHistory.AuthorName;
            if (string.IsNullOrEmpty(metadata.Author))
                metadata.Author = sourceMapVariant.Metadata.CreationHistory.AuthorName;
            metadata.GameId = sourceMapVariant.Metadata.GameId;
            metadata.ContentType = ContentItemType.SandboxMap;
            metadata.ContentSize = typeof(BlfMapVariant).GetSize();
            metadata.Timestamp = (ulong)DateTime.Now.ToFileTime();
            metadata.GameEngineType = GameEngineType.None;
            metadata.MapId = sourceMapVariant.MapId;
            metadata.Identifier = sourceMapVariant.Metadata.Uid;
            metadata.UserId = sourceMapVariant.Metadata.CreationHistory.AuthorUID;

            var mapVariant = ConvertMapVariant(sourceMapVariant, sourceScenario, out List<string> tagNames);
            mapVariant.Metadata = metadata;
            return GenerateBlf(mapVariant, tagNames);
        }

        private MapVariant ConvertMapVariant(ReachMapVariant sourceMapVariant, Scenario sourceScenario, out List<string> destTagNames)
        {
            var result = new MapVariant();
            result.Version = 12;
            result.MapId = sourceMapVariant.MapId;
            result.WorldBounds = sourceMapVariant.WorldBounds;
            result.MaximumBudget = sourceMapVariant.MaxBudget;
            result.SpentBudget = sourceMapVariant.SpentBudget;
            result.MapChecksum = sourceMapVariant.CacheCRC;

            var objectTypeMap = new ObjectTypeMap(sourceScenario);
            result.ObjectTypeStartIndex = objectTypeMap.CalculateObjetTypeStartIndices();
            result.ScenarioObjectCount = result.ObjectTypeStartIndex.Max();
            result.VariantObjectCount = result.ScenarioObjectCount;

            result.Objects = new VariantObjectDatum[640];
            result.Quotas = new VariantObjectQuota[256];

            for (int i = 0; i < result.Quotas.Length; i++)
                result.Quotas[i] = new VariantObjectQuota();

            for (int i = 0; i < result.ScenarioObjectCount; i++)
                result.Objects[i] = new VariantObjectDatum() { Flags = VariantObjectPlacementFlags.ScenarioObject | VariantObjectPlacementFlags.ScenarioObjectRemoved };
               

                var quotaBuilder = new VariantQuotaBuilder();
            for (int i = 0; i < sourceMapVariant.Objects.Count; i++)
            {
                var reachVariantObject = sourceMapVariant.Objects[i];

                if (!reachVariantObject.Flags.HasFlag(ReachVariantObjectDatum.ObjectPlacementFlags.OccupiedSlot))
                    continue;

                var paletteEntry = sourceMapVariant.GetPaletteEntry(sourceScenario, reachVariantObject.QuotaIndex);
                var sourceObjectTag = paletteEntry.Variants[reachVariantObject.VariantIndex].Object;
                if (ExcludedTags.Contains($"{sourceObjectTag.Name}.{ sourceObjectTag.Group.Tag}"))
                    continue;

                var variantObbject = ConvertVariantObject(reachVariantObject);
                result.Objects[result.VariantObjectCount++] = variantObbject;
                var index = quotaBuilder.FindObject(sourceObjectTag);
                var reachQuota = sourceMapVariant.Quotas[reachVariantObject.QuotaIndex];

                if (index == -1)
                {
                    index = quotaBuilder.AddObject(sourceObjectTag);
                    var newQuota = quotaBuilder[index];
                    newQuota.Tag = sourceObjectTag;
                    newQuota.MaxAllowed = paletteEntry.MaximumAllowed;
                    newQuota.MinimumCount = reachQuota.MinimumCount;
                    newQuota.MaximumCount = reachQuota.MaximumCount;
                }

                var quota = quotaBuilder[index];
                quota.PlacedOnMap++;
                variantObbject.QuotaIndex = index;
            }

            for (int i = 0; i < quotaBuilder.Count; i++)
            {
                result.Quotas[i].ObjectDefinitionIndex = 0;
                result.Quotas[i].MaximumCount = (byte)quotaBuilder[i].MinimumCount;
                result.Quotas[i].MaximumCount = (byte)quotaBuilder[i].MaximumCount;
                result.Quotas[i].PlacedOnMap = (byte)quotaBuilder[i].PlacedOnMap;
                result.Quotas[i].MaxAllowed = (byte)quotaBuilder[i].MaxAllowed;
            }

            destTagNames = new List<string>();
            for(int i = 0; i < quotaBuilder.Count; i++)
            {
                var quota = quotaBuilder[i];
                var name = $"{quota.Tag.Name}.{quota.Tag.Group.Tag}";
                if (SubstitutedTags.TryGetValue(name, out string sub))
                    name = sub;

                destTagNames.Add(name);
            }

            return result;
        }

        private static Blf GenerateBlf(MapVariant mapVariant, IList<string> tagnames)
        {
            var blf = new Blf(CacheVersion.HaloOnlineED, CachePlatform.Original);

            blf.StartOfFile = new BlfChunkStartOfFile()
            {
                Signature = new Tag("_blf"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkStartOfFile), blf.Version, blf.CachePlatform),
                MajorVersion = 1,
                MinorVersion = 2,
                ByteOrderMarker = -2
            };
            blf.ContentFlags |= BlfFileContentFlags.StartOfFile;

            blf.ContentHeader = new BlfContentHeader()
            {
                Signature = new Tag("chdr"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfContentHeader), blf.Version, blf.CachePlatform),
                MajorVersion = 9,
                MinorVersion = 3,
                BuildVersion = 0xffffa0d4,
                Metadata = mapVariant.Metadata
            };

            blf.ContentFlags |= BlfFileContentFlags.ContentHeader;

            blf.MapVariant = new BlfMapVariant()
            {
                Signature = new Tag("mapv"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfMapVariant), blf.Version, blf.CachePlatform),
                MajorVersion = 12,
                MinorVersion = 1,
                MapVariant = mapVariant,
                VariantVersion = 0
            };
            blf.ContentFlags |= BlfFileContentFlags.MapVariant;

            blf.MapVariantTagNames = new BlfMapVariantTagNames()
            {
                Signature = new Tag("tagn"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfMapVariantTagNames), blf.Version, blf.CachePlatform),
                MajorVersion = 1,
                MinorVersion = 0,
                Names = Enumerable.Range(0, 256).Select(x => new TagName() { Name = "" }).ToArray()
            };
            blf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;

            blf.MapVariantTagNames.Names = new TagName[256];
            for (int i = 0; i < tagnames.Count; i++)
                blf.MapVariantTagNames.Names[i] = new TagName() { Name = tagnames[i] };

            blf.EndOfFile = new BlfChunkEndOfFile()
            {
                Signature = new Tag("_eof"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkEndOfFile), blf.Version, blf.CachePlatform),
                MajorVersion = 1,
                MinorVersion = 1,
                AuthenticationDataSize = 0,
                AuthenticationType = BlfAuthenticationType.None
            };
            blf.ContentFlags |= BlfFileContentFlags.EndOfFile;

            return blf;
        }

        class VariantQuotaBuilder : IReadOnlyList<AuxVariantQuota>
        {
            private List<AuxVariantQuota> _quotas = new List<AuxVariantQuota>();
            private Dictionary<CachedTag, int> _tagToQuotaIndex = new Dictionary<CachedTag, int>();

            public AuxVariantQuota this[int index] => _quotas[index];
            public List<CachedTag> GetObjects() => _quotas.Select(x => x.Tag).ToList();
            public IReadOnlyList<AuxVariantQuota> Quotas => _quotas;

            public int Count => _quotas.Count;

            public int FindObject(CachedTag tag)
            {
                return _tagToQuotaIndex.TryGetValue(tag, out int index) ? index : -1;
            }

            public int AddObject(CachedTag tag)
            {
                var index = _quotas.Count;
                _tagToQuotaIndex.Add(tag, index);
                _quotas.Add(new AuxVariantQuota() { Tag = tag });
                return index;
            }

            public IEnumerator<AuxVariantQuota> GetEnumerator() => _quotas.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
        }

        public class AuxVariantQuota
        {
            public CachedTag Tag;
            public int MaxAllowed;
            public int MinimumCount;
            public int MaximumCount;
            public int MaximumAllowed = 255;
            public int PlacedOnMap;
        }


        private static VariantObjectDatum ConvertVariantObject(ReachVariantObjectDatum reachObject)
        {
            var result = new VariantObjectDatum();
            result.Flags = VariantObjectPlacementFlags.OccupiedSlot | VariantObjectPlacementFlags.Edited;
            result.Position = reachObject.Position;
            result.Forward = reachObject.Forward;
            result.Up = reachObject.Up;
            result.Properties = ConvertMultiplayerProperties(reachObject.Properties);
            return result;
        }

        private static VariantMultiplayerProperties ConvertMultiplayerProperties(ReachVarintMultiplayerObjectProperties reachProperties)
        {
            var result = new VariantMultiplayerProperties();
            result.EngineFlags = GameEngineSubTypeFlags.All;
            result.Flags = reachProperties.PlacementFlags.ConvertLexical<VariantPlacementFlags>();
            result.Team = reachProperties.Team;
            result.SpawnTime = (byte)reachProperties.SpawnTime;
            result.Type = reachProperties.Type.ConvertLexical<MultiplayerObjectType>();

            switch (reachProperties.Type)
            {
                case MultiplayerObjectTypeReach.Weapon:
                    result.SharedStorage = (byte)reachProperties.SpareClips;
                    break;
                case MultiplayerObjectTypeReach.Teleporter2way:
                case MultiplayerObjectTypeReach.TeleporterReceiver:
                case MultiplayerObjectTypeReach.TeleporterSender:
                    result.SharedStorage = (byte)reachProperties.TeleporterChannel;
                    break;
                default:
                    result.SharedStorage = (byte)reachProperties.SpawnOrder;
                    break;
            }

            result.Boundary.Type = reachProperties.Boundary.Shape;
            result.Boundary.WidthRadius = reachProperties.Boundary.WidthRadius;
            result.Boundary.BoxLength = reachProperties.Boundary.BoxLength;
            result.Boundary.PositiveHeight = reachProperties.Boundary.PositiveHeight;
            result.Boundary.NegativeHeight = reachProperties.Boundary.NegativeHeight;
            return result;
        }

        class ObjectTypeMap
        {
            public Dictionary<GameObjectTypeHalo3ODST, ObjectTypeInfo> ObjectTypes
                = new Dictionary<GameObjectTypeHalo3ODST, ObjectTypeInfo>();

            public ObjectTypeMap(Scenario scenario)
            {
                ObjectTypes.Add(GameObjectTypeHalo3ODST.Vehicle, new ObjectTypeInfo(scenario.Vehicles, scenario.VehiclePalette));
                ObjectTypes.Add(GameObjectTypeHalo3ODST.Weapon, new ObjectTypeInfo(scenario.Weapons, scenario.WeaponPalette));
                ObjectTypes.Add(GameObjectTypeHalo3ODST.Equipment, new ObjectTypeInfo(scenario.Equipment, scenario.EquipmentPalette));
                ObjectTypes.Add(GameObjectTypeHalo3ODST.Scenery, new ObjectTypeInfo(scenario.Scenery, scenario.SceneryPalette));
                ObjectTypes.Add(GameObjectTypeHalo3ODST.Crate, new ObjectTypeInfo(scenario.Crates, scenario.CratePalette));
            }

            public ObjectTypeInfo this[GameObjectTypeHalo3ODST type] => ObjectTypes[type];
            public ObjectTypeInfo this[GameObjectType type] => ObjectTypes[ConvertObjectType(type)];

            public GameObjectTypeHalo3ODST ConvertObjectType(GameObjectType objectType)
            {
                int index = (int)objectType.Halo3Retail;
                if (objectType.Halo3Retail > GameObjectTypeHalo3Retail.Equipment)
                    index++;

                return (GameObjectTypeHalo3ODST)index;
            }

            public short[] CalculateObjetTypeStartIndices()
            {
                var indices = new short[16];
                for (int i = 0; i < 16; i++)
                    indices[i] = -1;

                int placementCount = 0;
                foreach (var type in ObjectTypes)
                {
                    indices[(int)type.Key] = (short)placementCount;
                    placementCount += type.Value.Placements.Count;
                }
                return indices;
            }
        }

        class ObjectTypeInfo
        {
            public List<ScenarioInstance> Placements;
            public List<ScenarioPaletteEntry> Palette;

            public ObjectTypeInfo(IList placements, IList palette)
            {
                Placements = placements.Cast<ScenarioInstance>().ToList();
                Palette = palette.Cast<ScenarioPaletteEntry>().ToList();
            }
        }
    }
}
