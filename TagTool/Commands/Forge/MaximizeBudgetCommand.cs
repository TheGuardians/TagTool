using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using static TagTool.BlamFile.MapVariantGenerator;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.Commands.Forge
{
    class MaximizeBudgetCommand : Command
    {
        private GameCacheHaloOnlineBase Cache;
        private ForgeGlobalsDefinition Definition;
        private HashSet<CachedTag> ForgePalette = new HashSet<CachedTag>();

        public MaximizeBudgetCommand(GameCacheHaloOnlineBase cache, ForgeGlobalsDefinition definition) : base(true,
            "MaximizeBudget",
            "Moves placements for objects that are in the global forge palette into a map variant to maximize the number of objects that can be placed",

            "MaximizeBudget",

            "")
        {
            Cache = cache;
            Definition = definition;
            ForgePalette = new HashSet<CachedTag>(definition.Palette.Where(x => x.CategoryIndex != -1).Select(x => x.Object));
        }

        public override object Execute(List<string> args)
        {
            if (Cache is GameCacheModPackage modCache)
            {
                foreach (var stream in modCache.BaseModPackage.MapFileStreams)
                    MaximizeMapForgeBudget(stream);
            }
            else if (Cache is GameCacheHaloOnline hoCache)
            {
                foreach (var file in hoCache.Directory.GetFiles("*.map"))
                {
                    using (var mapFileStream = file.Open(FileMode.Open, FileAccess.ReadWrite))
                        MaximizeMapForgeBudget(mapFileStream);
                }
            }

            return true;
        }

        private void MaximizeMapForgeBudget(Stream mapFileStream)
        {
            var reader = new EndianReader(mapFileStream);
            var writer = new EndianWriter(mapFileStream);

            var mapFile = new MapFile();
            mapFile.Read(reader);

            if (mapFile.MapFileBlf == null || mapFile.MapFileBlf.MapVariant != null)
                return;

            MaximizeMapForgeBudget(mapFile);

            mapFileStream.Position = 0;
            mapFile.Write(writer);
        }

        private void MaximizeMapForgeBudget(MapFile mapFile)
        {
            var scenarioTag = Cache.TagCache.GetTag<Scenario>(mapFile.Header.GetScenarioPath());

            Console.WriteLine($"Maximizing budget for scenario '{scenarioTag.Name}'...");

            using (var cacheStream = Cache.OpenCacheReadWrite())
            {
                var scenario = Cache.Deserialize<Scenario>(cacheStream, scenarioTag);

                var metadata = new ContentItemMetadata()
                {
                    Name = mapFile.MapFileBlf.Scenario.Names[0].Name,
                    Description = mapFile.MapFileBlf.Scenario.Descriptions[0].Name,
                    Author = "Bungie",
                    ContentType = 0xA,
                    ContentSize = typeof(BlfMapVariant).GetSize(),
                    Timestamp = (ulong)DateTime.Now.ToFileTime(),
                    CampaignId = -1,
                    MapId = scenario.MapId,
                    GameEngineType = (int)GameEngineType.None,
                    CampaignDifficulty = -1,
                    CampaignInsertionPoint = 0,
                    IsSurvival = false,
                    MapChecksum = BlamCrc32.CrcChecksum(((CacheFileHeaderGenHaloOnline)mapFile.Header).RSASignature)
                };

                var generator = new MapVariantGenerator();
                generator.ObjectTypeMask |= 
                    (1 << (int)GameObjectTypeHalo3ODST.Machine) |
                    (1 << (int)GameObjectTypeHalo3ODST.Control) |
                    (1 << (int)GameObjectTypeHalo3ODST.EffectScenery);

                // Generate a map variant from the current scenario first
                var oldBlf = generator.Generate(cacheStream, Cache, scenario, metadata);
                // Generate a list of placements for objects that are in the forge palette
                var newUserPlacements = GetForgeablePlacements(oldBlf.MapVariant.MapVariant);
                // Perform the culling
                CullForgeObjectsFromScenario(scenario);
                // Regenerate the map variant from the culled scenario
                var blf = generator.Generate(cacheStream, Cache, scenario, metadata);
                // Convert the culled scenario placements to user placements and add them to the new map variant
                ConvertScenarioPlacements(blf.MapVariant.MapVariant, oldBlf.MapVariant.MapVariant.Palette, newUserPlacements);
                // Update the tag names
                RebuildTagNameChunk(blf);
                // Assign the new blf chunks to the map file
                mapFile.MapFileBlf.MapVariant = blf.MapVariant;
                mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariant;
                mapFile.MapFileBlf.MapVariantTagNames = blf.MapVariantTagNames;
                mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;
                // Finally serialize the scenario
                Cache.Serialize(cacheStream, scenarioTag, scenario);

                var numCulled = oldBlf.MapVariant.MapVariant.ScenarioObjectCount - blf.MapVariant.MapVariant.ScenarioObjectCount;
                var numAvailable = blf.MapVariant.MapVariant.Placements.Length - blf.MapVariant.MapVariant.ScenarioObjectCount;
                Console.WriteLine($"Culled {numCulled} placements, Availabel: {numAvailable}");
            }
        }

        private void RebuildTagNameChunk(Blf blf)
        {
            var mapVariant = blf.MapVariant.MapVariant;
            for (int i = 0; i < mapVariant.Palette.Length; i++)
            {
                if (mapVariant.Palette[i].TagIndex == -1)
                    continue;

                var tag = Cache.TagCache.GetTag(mapVariant.Palette[i].TagIndex);
                blf.MapVariantTagNames.Names[i] = new TagName() { Name = $"{tag.Name}.{tag.Group.Tag}" };
            }
        }

        private static void ConvertScenarioPlacements(MapVariant mapVariant, IList<MapVariantPaletteItem> palette, IList<MapVariantPlacement> placements)
        {
            foreach (var placement in placements)
            {
                var paletteEntry = palette[placement.PaletteIndex];

                var newPaletteIndex = -1;
                for (int i = 0; i < mapVariant.Palette.Length; i++)
                {
                    if (mapVariant.Palette[i].TagIndex == paletteEntry.TagIndex)
                    {
                        newPaletteIndex = i;
                        break;
                    }
                }

                if (newPaletteIndex == -1)
                {
                    newPaletteIndex = mapVariant.PaletteItemCount;
                    mapVariant.Palette[mapVariant.PaletteItemCount++] = paletteEntry;
                }

                placement.PaletteIndex = newPaletteIndex;
                placement.PlacementFlags = (placement.PlacementFlags & ~PlacementFlags.FromScenario) | PlacementFlags.Touched;
                paletteEntry.CountOnMap++;
                paletteEntry.RuntimeMax++;
                mapVariant.Placements[mapVariant.PlacedObjectCount++] = placement;
            }
        }

        private List<MapVariantPlacement> GetForgeablePlacements(MapVariant mapVariant)
        {
            var newUserPlacements = new List<MapVariantPlacement>();

            for (int i = 0; i < mapVariant.Placements.Length; i++)
            {
                var placement = mapVariant.Placements[i];
                if (!placement.PlacementFlags.HasFlag(PlacementFlags.Valid))
                    continue;
                var paletteEntry = mapVariant.Palette[placement.PaletteIndex];

                if (ForgePalette.Contains(Cache.TagCache.GetTag(paletteEntry.TagIndex)))
                    newUserPlacements.Add(placement);
            }

            return newUserPlacements;
        }

        private void CullForgeObjectsFromScenario(Scenario scenario)
        {
            var objectTypes = new Dictionary<GameObjectTypeHalo3ODST, ObjectTypeDefinition>();
            objectTypes.Add(GameObjectTypeHalo3ODST.Biped, new ObjectTypeDefinition(scenario.Bipeds, scenario.BipedPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Vehicle, new ObjectTypeDefinition(scenario.Vehicles, scenario.VehiclePalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Weapon, new ObjectTypeDefinition(scenario.Weapons, scenario.WeaponPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Equipment, new ObjectTypeDefinition(scenario.Equipment, scenario.EquipmentPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.AlternateRealityDevice, new ObjectTypeDefinition(scenario.AlternateRealityDevices, scenario.AlternateRealityDevicePalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Terminal, new ObjectTypeDefinition(scenario.Terminals, scenario.TerminalPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Scenery, new ObjectTypeDefinition(scenario.Scenery, scenario.SceneryPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Machine, new ObjectTypeDefinition(scenario.Machines, scenario.MachinePalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Control, new ObjectTypeDefinition(scenario.Controls, scenario.ControlPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.SoundScenery, new ObjectTypeDefinition(scenario.SoundScenery, scenario.SoundSceneryPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Crate, new ObjectTypeDefinition(scenario.Crates, scenario.CratePalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Creature, new ObjectTypeDefinition(scenario.Creatures, scenario.CreaturePalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.Giant, new ObjectTypeDefinition(scenario.Giants, scenario.GiantPalette));
            objectTypes.Add(GameObjectTypeHalo3ODST.EffectScenery, new ObjectTypeDefinition(scenario.EffectScenery, scenario.EffectSceneryPalette));

            foreach (var pair in objectTypes)
            {
                var objectType = pair.Key;
                var objectTypeDef = pair.Value;

                var newInstances = new List<ScenarioInstance>();
                var newPalette = new List<ScenarioPaletteEntry>();
                var oldToNewInstanceMapping = new Dictionary<int, int>();

                // Generate the new instances block, keeping track of the old to new indices
                for (int i = 0; i < objectTypeDef.Instances.Count; i++)
                {
                    var instance = objectTypeDef.Instances[i] as ScenarioInstance;
                    if (instance.PaletteIndex == -1)
                        continue;

                    var paletteEntry = objectTypeDef.Palette[instance.PaletteIndex] as ScenarioPaletteEntry;

                    // we only want to leave objects that are not in the forge palette left in the scenario
                    if (ForgePalette.Contains(paletteEntry.Object))
                        continue;

                    // try to find an existing palette entry, if not add one to the new palette block and use that index
                    var paletteIndex = newPalette.IndexOf(paletteEntry);
                    if (paletteIndex == -1)
                    {
                        paletteIndex = newPalette.Count;
                        newPalette.Add(paletteEntry);
                    }

                    instance.PaletteIndex = (short)paletteIndex;

                    oldToNewInstanceMapping[i] = newInstances.Count;
                    newInstances.Add(instance);
                }

                // Assign the new instances block
                objectTypeDef.Instances.Clear();
                foreach (var instance in newInstances)
                    objectTypeDef.Instances.Add(instance);

                // Assign the new palette block
                objectTypeDef.Palette.Clear();
                foreach (var entry in newPalette)
                    objectTypeDef.Palette.Add(entry);
            }

            // Clear out legacy sandbox palettes
            scenario.SandboxEquipment = new List<SandboxObject>();
            scenario.SandboxVehicles = new List<SandboxObject>();
            scenario.SandboxEquipment = new List<SandboxObject>();
            scenario.SandboxGoalObjects = new List<SandboxObject>();
            scenario.SandboxScenery = new List<SandboxObject>();
            scenario.SandboxTeleporters = new List<SandboxObject>();
            scenario.SandboxSpawning = new List<SandboxObject>();
            scenario.SandboxWeapons = new List<SandboxObject>();
        }

        class BlamCrc32
        {
            private static uint[] _table;

            static BlamCrc32()
            {
                _table = new uint[256];

                for (int i = 0; i < _table.Length; i++)
                {
                    uint value = (uint)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((value & 1) != 0)
                            value = (value >> 1) ^ 0xEDB88320;
                        else
                            value >>= 1;
                    }
                    _table[i] = value;
                }
            }

            public static uint CrcChecksum(byte[] data)
            {
                uint value = 0xFFFFFFFF;
                for (int i = 0; i < data.Length; i++)
                {
                    value = _table[(value ^ data[i]) & 0xFF] ^ (value >> 8);
                }
                return value;
            }
        }
    }
}
