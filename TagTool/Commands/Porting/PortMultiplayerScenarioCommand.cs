using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using static TagTool.Cache.CacheFile;
using static TagTool.Commands.Porting.PortTagCommand;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.Commands.Porting
{
    class PortMultiplayerScenarioCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }
        private PortTagCommand PortTag { get; }

        [Flags]
        public enum MultiplayerScenarioConversionFlags
        {
            None,
            // port scenario objects
            Objects,
            // port audio
            Audio,
            // use ms30 shaders
            Ms30,
            // attempt to add an invisible point
            SpawnPoint,
            // keep path finding data
            PathFinding,

            Default = Objects | Audio | Ms30 | SpawnPoint
        }

        public PortMultiplayerScenarioCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache, PortTagCommand portTag) :
            base(true,

                "PortMultiplayerScenario",
                "Builds a multiplayer map from one or more bsps within a given zone set",

                "PortMultiplayerScenario [options] [sbsp tag (leave blank for a wizard)]",

                "Builds a multiplayer map from one or more bsps within a given zone set")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            PortTag = portTag;
        }

        public override object Execute(List<string> args)
        {
            var blamCache = BlamCache;
            int zoneSetIndex = -1;
            BspFlags bspMask = 0;
            MultiplayerScenarioConversionFlags conversionFlags;

            var firstNonFlagArgumentIndex = ParseConversionFlags(args, out conversionFlags);
            args = args.Skip(firstNonFlagArgumentIndex).ToList();

            IndexItem blamScnrTag = BlamCache.IndexItems.First(x => x.GroupTag == "scnr");
            var blamScnr = BlamCache.Deserializer.Deserialize<Scenario>(
                new CacheSerializationContext(ref blamCache, blamScnrTag));

            if (args.Count < 1)
            {
                Dictionary<string, int> structureBspsByName = new Dictionary<string, int>();
                Dictionary<string, int> zoneSetsByName = new Dictionary<string, int>();

                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < blamScnr.ZoneSets.Count; i++)
                    Console.WriteLine($"{i}. {blamCache.Strings.GetString(blamScnr.ZoneSets[i].Name)}");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter the name or index of the zone set to use:");
                string zoneSetName = Console.ReadLine().Trim();


                for (int i = 0; i < blamScnr.StructureBsps.Count; i++)
                    structureBspsByName.Add(blamScnr.StructureBsps[i].StructureBsp.Name, i);

                if (zoneSetsByName.ContainsKey(zoneSetName))
                    zoneSetIndex = zoneSetsByName[zoneSetName];
                else
                {
                    if (!int.TryParse(zoneSetName, out zoneSetIndex))
                        zoneSetIndex = -1;
                }

                if (zoneSetIndex == -1)
                {
                    Console.WriteLine($"Zone set '{zoneSetName}' could not be found!\n");
                    return true;
                }

                var zoneSet = blamScnr.ZoneSets[zoneSetIndex];

                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < 32; i++)
                {
                    if ((zoneSet.LoadedBsps & (BspFlags)(1u << i)) != 0)
                        Console.WriteLine($"{i}. {blamScnr.StructureBsps[i].StructureBsp.Name}");
                }
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter the name or index of each bsp to include on a new line followed by a blank line:");

                for (string line; !string.IsNullOrWhiteSpace(line = Console.ReadLine());)
                {
                    var sbspName = line.Trim();
                    int bspIndex = -1;

                    if (structureBspsByName.ContainsKey(sbspName))
                        bspIndex = structureBspsByName[sbspName];
                    else
                    {
                        if (!int.TryParse(sbspName, out bspIndex))
                            bspIndex = -1;
                    }

                    if (bspIndex == -1)
                        Console.WriteLine($"Could not find bsp '{sbspName}'");

                    bspMask |= (BspFlags)(1u << bspIndex);
                }
            }
            else
            {
                var tagName = args[0].Trim();
                var bspIndex = blamScnr.StructureBsps.FindIndex(x => x.StructureBsp.Name == tagName);
                if (bspIndex == -1)
                {
                    Console.WriteLine($"Could not find bsp '{tagName}'");
                    return true;
                }

                bspMask = (BspFlags)(1u << bspIndex);
                for (int i = 0; i < blamScnr.ZoneSets.Count; i++)
                {
                    if ((blamScnr.ZoneSets[i].LoadedBsps & bspMask) != 0)
                    {
                        zoneSetIndex = i;
                        break;
                    }
                }

                if (zoneSetIndex == -1)
                {
                    Console.WriteLine($"No zone set includes bsp '{tagName}'!");
                    return true;
                }
            }

            Console.WriteLine("Converting...");
            Convert(zoneSetIndex, bspMask, conversionFlags);
            return true;
        }

        int ParseConversionFlags(List<string> args, out MultiplayerScenarioConversionFlags flags)
        {
            flags = MultiplayerScenarioConversionFlags.Default;
            var endIndex = 0;

            for (int i = 0; i < args.Count; i++)
            {
                var arg = args[i];

                bool not = false;
                string flagName = "";
                if (arg.Length > 1)
                {
                    not = arg[0] == '!';
                    flagName = arg.Substring(1);
                }

                MultiplayerScenarioConversionFlags flag;
                if (Enum.TryParse(flagName, true, out flag))
                {
                    endIndex++;

                    if (not)
                        flags &= ~flag;
                    else
                        flags |= flag;
                }
            }

            return endIndex;
        }

        private void Convert(int zoneSetIndex, BspFlags bspMask, MultiplayerScenarioConversionFlags conversionFlags)
        {
            var blamCache = BlamCache;
            var blamScnrTag = BlamCache.IndexItems.FirstOrDefault(x => x.GroupTag == "scnr");
            var blamScnr = BlamCache.Deserializer.Deserialize<Scenario>(new CacheSerializationContext(ref blamCache, blamScnrTag));

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                var resourceStreams = new Dictionary<ResourceLocation, Stream>();

                var defaultPortingFlags = PortingFlags.Default;
                if (!conversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Audio))
                    defaultPortingFlags &= ~PortingFlags.Audio;
                if (!conversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Ms30))
                    defaultPortingFlags &= ~PortingFlags.Ms30;

                if (blamScnr.Lightmap != null)
                {
                    PortTag.RemoveFlags(~defaultPortingFlags);
                    PortTag.SetFlags(defaultPortingFlags);
                    ConvertLightmap(cacheStream, resourceStreams,
                        blamCache.GetIndexItemFromID(blamScnr.Lightmap.Index), zoneSetIndex, bspMask);
                }

                var tagCollector = new MultiplayerScenarioTagCollector(blamCache, blamScnr, zoneSetIndex, bspMask, conversionFlags);
                tagCollector.Collect();

                PortTag.RemoveFlags(~defaultPortingFlags);
                PortTag.SetFlags(defaultPortingFlags);

                foreach (var tag in tagCollector.Tags)
                    PortTag.ConvertTag(cacheStream, resourceStreams, tag);

                PortTag.RemoveFlags(PortingFlags.Recursive);
                var scnrTag = PortTag.ConvertTag(cacheStream, resourceStreams, blamScnrTag);

                new MultiplayerScenarioFixup(cacheStream, CacheContext, blamScnrTag.Name, zoneSetIndex, bspMask, conversionFlags).Fixup();

                foreach (var entry in resourceStreams.Values)
                    entry.Close();
            }

            using (var stringIdCacheStream = CacheContext.OpenStringIdCacheReadWrite())
                CacheContext.StringIdCache.Save(stringIdCacheStream);

            CacheContext.SaveTagNames();
        }

        private void ConvertLightmap(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams,
            IndexItem blamLightmapTag, int zoneSetIndex, BspFlags bspMask)
        {
            var blamCache = BlamCache;
            var blamLightmap = BlamCache.Deserializer.Deserialize<ScenarioLightmap>(
                new CacheSerializationContext(ref blamCache, blamLightmapTag));

            if (blamCache.Version < CacheVersion.Halo3ODST)
            {
                var lightmapTag = CacheContext.AllocateTag<ScenarioLightmap>(blamLightmapTag.Name);
                var lightmap = blamLightmap;

                for (int i = 0, j = 0; i < blamLightmap.Lightmaps.Count; i++)
                {
                    if (((BspFlags)(1u << i) & bspMask) == 0)
                        continue;

                    var blamLbsp = blamLightmap.Lightmaps[i];
                    var Lbsp = (ScenarioLightmapBspData)PortTag.ConvertData(cacheStream, resourceStreams,
                        blamLbsp, blamLbsp, blamLightmapTag.Name);

                    Lbsp.Airprobes = new List<ScenarioLightmap.Airprobe>();
                    Lbsp.Airprobes.AddRange(blamLightmap.Airprobes);
                    Lbsp.BspIndex = (short)j++;

                    var LbspTag = CacheContext.AllocateTag<ScenarioLightmapBspData>($"{blamLightmapTag.Name}_{i}_data");
                    CacheContext.Serialize(cacheStream, LbspTag, Lbsp);

                    lightmap.LightmapDataReferences.Add(new ScenarioLightmap.LightmapDataReference() { LightmapData = LbspTag });
                }

                lightmap.Airprobes.Clear();
                CacheContext.Serialize(cacheStream, lightmapTag, lightmap);
            }
            else
            {
                var lightmapTag = CacheContext.AllocateTag<ScenarioLightmap>(blamLightmapTag.Name);
                var lightmap = blamLightmap;
                var newDataReferences = new List<ScenarioLightmap.LightmapDataReference>();

                for (int i = 0, j = 0; i < blamLightmap.LightmapDataReferences.Count; i++)
                {
                    if (((BspFlags)(1u << i) & bspMask) == 0)
                        continue;

                    var blamLbspTag = blamCache.GetIndexItemFromID(blamLightmap.LightmapDataReferences[i].LightmapData.Index);
                    var blamLbsp = blamCache.Deserializer.Deserialize<ScenarioLightmapBspData>(
                        new CacheSerializationContext(ref blamCache, blamLbspTag));

                    var Lbsp = (ScenarioLightmapBspData)PortTag.ConvertData(cacheStream, resourceStreams, blamLbsp, blamLbsp, blamLbspTag.Name);
                    Lbsp.BspIndex = (short)j++;

                    var LbspTag = CacheContext.AllocateTag<ScenarioLightmapBspData>(blamLbspTag.Name);
                    CacheContext.Serialize(cacheStream, LbspTag, Lbsp);

                    newDataReferences.Add(new ScenarioLightmap.LightmapDataReference() { LightmapData = LbspTag });
                }

                lightmap.LightmapDataReferences = newDataReferences;
                CacheContext.Serialize(cacheStream, lightmapTag, lightmap);
            }
        }

        class MultiplayerScenarioFixup
        {
            private HaloOnlineCacheContext CacheContext;
            private Stream CacheStream;
            private CachedTagInstance ScnrTag;
            private Scenario Scnr;
            private int DesiredZoneSetIndex;
            private BspFlags DesiredBsps;
            private MultiplayerScenarioConversionFlags ConversionFlags;
            private Dictionary<int, int> BspIndexRemapping;

            public MultiplayerScenarioFixup(
                Stream cacheStream, HaloOnlineCacheContext cacheContext, string scenarioTagName, int desiredZoneSetIndex,
                BspFlags desiredBsps, MultiplayerScenarioConversionFlags conversionFlags)
            {
                this.CacheContext = cacheContext;
                this.CacheStream = cacheStream;
                this.ScnrTag = cacheContext.GetTag<Scenario>(scenarioTagName);
                this.Scnr = cacheContext.Deserialize<Scenario>(new TagSerializationContext(cacheStream, cacheContext, this.ScnrTag));
                this.DesiredZoneSetIndex = desiredZoneSetIndex;
                this.DesiredBsps = desiredBsps;
                this.ConversionFlags = conversionFlags;
                this.BspIndexRemapping = new Dictionary<int, int>();
            }

            public void Fixup()
            {
                Scnr.MapType = ScenarioMapType.Multiplayer;
                Scnr.MapSubType = ScenarioMapSubType.None;
                Scnr.CampaignId = -1;

                FixupStructureBspReferences();
                FixupSkies();
                FixupZoneSets();
                FixupAllScenarioObjects();
                FixupScenarioClusterData();

                Scnr.BspAtlas.Clear();
                Scnr.CampaignPlayers.Clear();
                Scnr.SoftCeilings.Clear();
                Scnr.PlayerStartingProfile.Clear();
                Scnr.PlayerStartingLocations.Clear();
                Scnr.TriggerVolumes.Clear();
                Scnr.RecordedAnimations.Clear();
                Scnr.ZonesetSwitchTriggerVolumes.Clear();
                Scnr.Unknown32.Clear();
                Scnr.Unknown33.Clear();
                Scnr.Unknown34.Clear();
                Scnr.Unknown35.Clear();
                Scnr.Unknown36.Clear();
                Scnr.StylePalette.Clear();
                Scnr.SquadGroups.Clear();
                Scnr.Squads.Clear();
                Scnr.Zones.Clear();
                Scnr.SquadPatrols.Clear();
                Scnr.MissionScenes.Clear();
                Scnr.CharacterPalette.Clear();
                Scnr.Scripts.Clear();
                Scnr.ScriptingData.Clear();
                Scnr.ScriptStrings = null;
                Scnr.ScriptSourceFileReferences.Clear();
                Scnr.ScriptExternalFileReferences.Clear();
                Scnr.Scripts.Clear();
                Scnr.Globals.Clear();
                Scnr.CutsceneFlags.Clear();
                Scnr.CutsceneCameraPoints.Clear();
                Scnr.Cinematics.Clear();
                Scnr.CinematicLighting.Clear();
                Scnr.CutsceneTitles.Clear();
                Scnr.CustomObjectNameStrings = null;
                Scnr.ChapterTitleStrings = null;
                Scnr.UnitSeatsMapping.Clear();
                Scnr.ScenarioKillTriggers.Clear();
                Scnr.ScenarioSafeTriggers.Clear();
                Scnr.ScriptExpressions.Clear();
                Scnr.SubtitleStrings = null;
                Scnr.MissionDialogue.Clear();
                Scnr.ObjectiveStrings = null;
                Scnr.Interpolators.Clear();
                Scnr.SimulationDefinitionTable.Clear();
                Scnr.ObjectReferenceFrames.Clear();
                Scnr.AiObjectives.Clear();
                Scnr.DesignerZoneSets.Clear();
                Scnr.Unknown135.Clear();
                Scnr.ScenarioMetagame.Clear();
                Scnr.EditorFolders.Clear();

                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.PathFinding))
                    Scnr.AiPathfindingData.Clear();

                ScenarioInstance targetInstance = Scnr.Scenery.FirstOrDefault();
                if (targetInstance == null)
                    targetInstance = Scnr.Crates.FirstOrDefault();

                if (targetInstance != null)
                {
                    var position = targetInstance.Position;
                    position.Z += 0.5f;

                    if (ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.SpawnPoint))
                        AddRespawnPoint(position, targetInstance.Rotation);

                    AddPrematchCamera(position, targetInstance.Rotation);
                }

                Scnr.PlayerStartingProfile = new List<PlayerStartingProfileBlock>
                {
                    new PlayerStartingProfileBlock
                    {
                        Name = "start_assault",
                        PrimaryWeapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\assault_rifle\assault_rifle"),
                        PrimaryRoundsLoaded = 32,
                        PrimaryRoundsTotal = 96,
                        StartingFragGrenadeCount = 2,
                        Unknown3 = -1
                    }
                };

                CacheContext.Serialize(CacheStream, ScnrTag, Scnr);

                FixupInstancedGeometry();
            }

            private void FixupSkies()
            {
                BspShortFlags bspMask = 0;
                for(int i = 0, j =0; i < 32; i++)
                {
                    if ((DesiredBsps & (BspFlags)(1 << i)) != 0)
                    {
                        bspMask |= (BspShortFlags)(1u << j);
                        j++;
                    }
                }

                foreach(var sky in Scnr.SkyReferences)
                {
                    if (sky.SkyObject != null)
                        sky.ActiveBsps = bspMask;
                    else
                        sky.ActiveBsps = BspShortFlags.None;
                }
            }

            private void FixupStructureBspReferences()
            {
                var newStructureBsps = new List<StructureBspBlock>();
                for (int bspIndex = 0; bspIndex < Scnr.StructureBsps.Count; bspIndex++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << bspIndex)))
                    {
                        BspIndexRemapping[bspIndex] = newStructureBsps.Count;
                        newStructureBsps.Add(Scnr.StructureBsps[bspIndex]);
                    }
                }
                Scnr.StructureBsps = newStructureBsps;
            }

            private void FixupZoneSets()
            {
                var zoneSet = Scnr.ZoneSets[DesiredZoneSetIndex];
                var zoneSetAudibility = Scnr.ZoneSetAudibility[zoneSet.ScenarioBspAudibilityIndex];
                var zoneSetPvs = Scnr.ZoneSetPvs[zoneSet.PotentiallyVisibleSetIndex];

                uint newBspZoneFlags = 0;
                uint desiredBspPvsFlags = 0;
                for (int bspIndex = 0, newBspIndex = 0; bspIndex < 32; bspIndex++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << bspIndex)))
                    {
                        var bspPvsIndex = ScenarioPvsBspPvsIndexGet(zoneSet.LoadedBsps, bspIndex);
                        if (bspPvsIndex < 0)
                            throw new NotSupportedException($"bsp index {bspIndex} is not in zone set {DesiredZoneSetIndex}");

                        newBspZoneFlags |= (1u << newBspIndex);
                        desiredBspPvsFlags |= (1u << bspPvsIndex);
                        newBspIndex++;
                    }
                }

                Scnr.ZoneSets = new List<ZoneSet>()
                {
                    new ZoneSet()
                    {
                        Name = zoneSet.Name,
                        BspAtlasIndex = -1,
                        PotentiallyVisibleSetIndex = 0,
                        LoadedBsps = (BspFlags)newBspZoneFlags
                    }
                };

                zoneSetPvs.StructureBspMask = (BspFlags)newBspZoneFlags;
                Scnr.ZoneSetPvs = new List<ZoneSetPvsBlock>() { zoneSetPvs };
                Scnr.ZoneSetAudibility = new List<ZoneSetAudibilityBlock>() { zoneSetAudibility };

                ClearBspPvsBlockElements(zoneSetAudibility.BspClusterToRoomBoundsMappings, (ulong)DesiredBsps);
                ClearBspPvsBlockElements(zoneSetAudibility.GamePortalToDoorOccluderMappings, (ulong)DesiredBsps);

                ClearBspPvsBlockElements(zoneSetPvs.BspChecksums, desiredBspPvsFlags);
                ClearBspPvsBlockElements(zoneSetPvs.StructureBspPotentiallyVisibleSets, desiredBspPvsFlags);
                ClearBspPvsBlockElements(zoneSetPvs.PortalToDeviceMappings, (ulong)DesiredBsps);

                for (int i = 0; i < zoneSetPvs.StructureBspPotentiallyVisibleSets.Count; i++)
                {
                    var pvs = zoneSetPvs.StructureBspPotentiallyVisibleSets[i];
                    int clusterCount = pvs.Clusters.Count;
                    for (int clusterIndex = 0; clusterIndex < clusterCount; clusterIndex++)
                    {
                        ClearBspPvsBlockElements(pvs.Clusters[clusterIndex].BitVectors, desiredBspPvsFlags);
                        ClearBspPvsBlockElements(pvs.ClustersDoorsClosed[clusterIndex].BitVectors, desiredBspPvsFlags);
                        ClearBspPvsBlockElements(pvs.ClusterMappings[clusterIndex].Clusters, desiredBspPvsFlags);
                    }
                }
            }

            private void FixupLightmap()
            {
                CachedTagInstance lightmapTag;
                if (!CacheContext.TryGetTag<ScenarioLightmap>(Scnr.Lightmap.Name, out lightmapTag))
                    return;

                var lightmap = CacheContext.Deserialize<ScenarioLightmap>(new TagSerializationContext(CacheStream, CacheContext, lightmapTag));

                var newLightmapDataReference = new List<ScenarioLightmap.LightmapDataReference>();
                for (int i = 0; i < lightmap.LightmapDataReferences.Count; i++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << i)))
                        newLightmapDataReference.Add(lightmap.LightmapDataReferences[i]);
                }

                for (int i = 0; i < newLightmapDataReference.Count; i++)
                {
                    var lbspTag = newLightmapDataReference[i].LightmapData;
                    if (lbspTag == null)
                        continue;

                    var lbsp = CacheContext.Deserialize<ScenarioLightmapBspData>(CacheStream, lbspTag);
                    lbsp.BspIndex = (sbyte)i;

                    CacheContext.Serialize(CacheStream, lbspTag, lbsp);
                }

                lightmap.LightmapDataReferences = newLightmapDataReference;
                CacheContext.Serialize(CacheStream, lightmapTag, lightmap);
            }

            void FixupScenarioClusterData()
            {
                var newClusterData = new List<ScenarioClusterDatum>();
                for (int bspIndex = 0; bspIndex < Scnr.StructureBsps.Count; bspIndex++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << bspIndex)))
                        newClusterData.Add(Scnr.ScenarioClusterData[bspIndex]);
                }
                Scnr.ScenarioClusterData = newClusterData;
            }

            private void FixupInstancedGeometry()
            {
                for (var i = 0; i < Scnr.StructureBsps.Count; i++)
                {
                    var sbspTag = Scnr.StructureBsps[i].StructureBsp;
                    if (sbspTag == null) continue;

                    var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(CacheStream, sbspTag);

                    foreach (var instance in sbsp.InstancedGeometryInstances)
                    {
                        foreach (var def in instance.CollisionDefinitions)
                        {
                            def.BspIndex = (sbyte)i;
                        }
                    }

                    CacheContext.Serialize(CacheStream, sbspTag, sbsp);
                }
            }

            void ClearBspPvsBlockElements<T>(List<T> block, ulong keepMask)
            {
                var newBlock = new List<T>();
                for (int i = 0; i < block.Count; i++)
                {
                    if (((1u << i) & keepMask) != 0)
                        newBlock.Add(block[i]);
                }
                block.Clear();
                block.AddRange(newBlock);
            }

            int ScenarioPvsBspPvsIndexGet(BspFlags bspZoneFlags, int bspIndex)
            {
                for (int i = 0, k = 0; i < 32; i++)
                {
                    if (bspZoneFlags.HasFlag((BspFlags)(1u << i)))
                    {
                        if (i == bspIndex)
                            return k;
                        k++;
                    }
                }
                return -1;
            }

            private void FixupAllScenarioObjects()
            {
                FixupScenarioObjects(Scnr.Scenery, Scnr.SceneryPalette);
                FixupScenarioObjects(Scnr.Bipeds, Scnr.BipedPalette);
                FixupScenarioObjects(Scnr.Vehicles, Scnr.VehiclePalette);
                FixupScenarioObjects(Scnr.Equipment, Scnr.EquipmentPalette);
                FixupScenarioObjects(Scnr.Weapons, Scnr.WeaponPalette);
                FixupScenarioObjects(Scnr.Machines, Scnr.MachinePalette);
                FixupScenarioObjects(Scnr.Terminals, Scnr.TerminalPalette);
                FixupScenarioObjects(Scnr.Controls, Scnr.ControlPalette);
                FixupScenarioObjects(Scnr.SoundScenery, Scnr.SoundSceneryPalette);
                FixupScenarioObjects(Scnr.Giants, Scnr.GiantPalette);
                FixupScenarioObjects(Scnr.EffectScenery, Scnr.EffectSceneryPalette);
                FixupScenarioObjects(Scnr.LightVolumes, Scnr.LightVolumePalette);
                FixupScenarioObjects(Scnr.Crates, Scnr.CratePalette);
                FixupScenarioObjects(Scnr.Creatures, Scnr.CreaturePalette);
            }

            private void FixupScenarioObjects<T>(List<T> objects, List<ScenarioPaletteEntry> palette)
                where T : ScenarioInstance
            {
                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Objects))
                {
                    objects.Clear();
                    palette.Clear();
                    return;
                }

                var visitedPaletteObjects = new HashSet<CachedTagInstance>();
                var newPalette = new List<ScenarioPaletteEntry>();
                var newObjects = new List<T>();
                var keepPaletteIndices = new HashSet<int>();

                for (int placementIndex = 0; placementIndex < objects.Count; placementIndex++)
                {
                    var obj = objects[placementIndex];

                    if (obj.OriginBspIndex < 0)
                        continue;
                    if (!DesiredBsps.HasFlag((BspFlags)(1 << (obj.OriginBspIndex))))
                        continue;
                    if (obj.PaletteIndex < 0 || obj.PaletteIndex >= palette.Count)
                        continue;

                    keepPaletteIndices.Add(obj.PaletteIndex);

                    obj.OriginBspIndex = (short)BspIndexRemapping[obj.OriginBspIndex];
                    obj.AllowedZoneSets = (1 << 0);

                    newObjects.Add(obj);

                    if (obj.NameIndex >= 0 && obj.NameIndex < Scnr.ObjectNames.Count)
                    {
                        var name = Scnr.ObjectNames[obj.NameIndex];
                        name.PlacementIndex = (short)(newObjects.Count - 1);
                    }
                }

                for (int i = 0; i < palette.Count; i++)
                {
                    if (!keepPaletteIndices.Contains(i))
                        palette[i].Object = null;
                }

                objects.Clear();
                objects.AddRange(newObjects);
            }

            private void AddPrematchCamera(RealPoint3d position, RealEulerAngles3d rotation)
            {
                Scnr.CutsceneCameraPoints.Add(new CutsceneCameraPoint()
                {
                    Position = position,
                    Orientation = rotation,
                    Flags = CutsceneCameraPointFlags.PrematchCameraHack,
                    Name = "prematch_camera",
                });
            }

            private void AddRespawnPoint(RealPoint3d position, RealEulerAngles3d rotation)
            {
                var instance = new SceneryInstance();
                instance.PaletteIndex = (short)Scnr.SceneryPalette.Count;
                instance.NameIndex = -1;
                instance.Position = position;
                instance.Rotation = rotation;
                instance.ObjectType = new ScenarioObjectType() { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
                instance.Source = ScenarioInstance.SourceValue.Editor;
                instance.BspPolicy = ScenarioInstance.BspPolicyValue.Default;
                instance.OriginBspIndex = 0;
                instance.AllowedZoneSets = (1 << 0);
                instance.Team = SceneryInstance.TeamValue.Neutral;
                Scnr.Scenery.Add(instance);

                Scnr.SceneryPalette.Add(new ScenarioPaletteEntry()
                {
                    Object = CacheContext.GetTag(@"objects\multi\spawning\respawn_point_invisible.scenery")
                });
            }
        }

        class MultiplayerScenarioTagCollector
        {
            private CacheFile BlamCache;
            private Scenario BlamScnr;
            private BspFlags DesiredBsps;
            private int DesiredZoneSetIndex;
            private MultiplayerScenarioConversionFlags ConversionFlags;
            public List<IndexItem> Tags;

            public MultiplayerScenarioTagCollector(CacheFile blamCache, Scenario blamScnr,
                int desiredZoneSetIndex, BspFlags desiredBsps, MultiplayerScenarioConversionFlags conversionFlags)
            {
                this.BlamCache = blamCache;
                this.BlamScnr = blamScnr;
                this.DesiredZoneSetIndex = desiredZoneSetIndex;
                this.DesiredBsps = desiredBsps;
                this.ConversionFlags = conversionFlags;
                this.Tags = new List<IndexItem>();
            }

            private void Add(CachedTagInstance tag)
            {
                if (tag != null) Tags.Add(BlamCache.GetIndexItemFromID(tag.Index));
            }

            public void Collect()
            {
                for (int i = 0; i < BlamScnr.StructureBsps.Count; i++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << i)))
                    {
                        Add(BlamScnr.StructureBsps[i].StructureBsp);
                        Add(BlamScnr.StructureBsps[i].Design);
                        Add(BlamScnr.StructureBsps[i].Lighting);
                        Add(BlamScnr.StructureBsps[i].Wind);
                        Add(BlamScnr.StructureBsps[i].Cubemap);
                    }
                }

                CollectSkies();
                CollectAllScenarioObjects();
                CollectFlocks();
                CollectDecals();

                Add(BlamScnr.DefaultScreenFx);
                Add(BlamScnr.DefaultCameraFx);
                Add(BlamScnr.SkyParameters);
                Add(BlamScnr.GlobalLighting);
                Add(BlamScnr.PerformanceThrottles);
            }

            private void CollectSkies()
            {
                var zoneSet = BlamScnr.ZoneSets[DesiredZoneSetIndex];
                for (int i = 0; i < BlamScnr.SkyReferences.Count; i++)
                {
                    var skyReference = BlamScnr.SkyReferences[i];
                    if ((skyReference.ActiveBsps & (BspShortFlags)zoneSet.LoadedBsps) != 0)
                        Add(skyReference.SkyObject);
                }
            }

            private void CollectAllScenarioObjects()
            {
                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Objects))
                    return;

                CollectScenarioObjects(BlamScnr.Scenery, BlamScnr.SceneryPalette);
                CollectScenarioObjects(BlamScnr.Bipeds, BlamScnr.BipedPalette);
                CollectScenarioObjects(BlamScnr.Vehicles, BlamScnr.VehiclePalette);
                CollectScenarioObjects(BlamScnr.Equipment, BlamScnr.EquipmentPalette);
                CollectScenarioObjects(BlamScnr.Weapons, BlamScnr.WeaponPalette);
                CollectScenarioObjects(BlamScnr.Machines, BlamScnr.MachinePalette);
                CollectScenarioObjects(BlamScnr.Terminals, BlamScnr.TerminalPalette);
                CollectScenarioObjects(BlamScnr.Controls, BlamScnr.ControlPalette);
                CollectScenarioObjects(BlamScnr.SoundScenery, BlamScnr.SoundSceneryPalette);
                CollectScenarioObjects(BlamScnr.Giants, BlamScnr.GiantPalette);
                CollectScenarioObjects(BlamScnr.EffectScenery, BlamScnr.EffectSceneryPalette);
                CollectScenarioObjects(BlamScnr.LightVolumes, BlamScnr.LightVolumePalette);
                CollectScenarioObjects(BlamScnr.Crates, BlamScnr.CratePalette);
                CollectScenarioObjects(BlamScnr.Creatures, BlamScnr.CreaturePalette);
            }

            private void CollectScenarioObjects(IEnumerable<ScenarioInstance> objects, List<ScenarioPaletteEntry> palette)
            {
                if (objects == null || palette == null)
                    return;

                foreach (var obj in objects.Where(x => DesiredBsps.HasFlag((BspFlags)(1 << (x.OriginBspIndex))) && x.PaletteIndex != -1))
                {
                    if (obj.PaletteIndex < 0 || obj.PaletteIndex >= palette.Count)
                        continue;

                    var paletteEntry = palette[obj.PaletteIndex];
                    if (paletteEntry.Object != null)
                        Add(palette[obj.PaletteIndex].Object);
                }
            }

            private void CollectFlocks()
            {
                foreach (var flock in BlamScnr.Flocks.Where(x => DesiredBsps.HasFlag((BspFlags)(1 << (x.BspIndex)))))
                {
                    if (flock.FlockPaletteIndex >= 0 && flock.FlockPaletteIndex < BlamScnr.CreaturePalette.Count)
                    {
                        var flockTag = BlamScnr.FlockPalette[flock.FlockPaletteIndex];
                        if (flockTag != null && flockTag.Instance != null)
                            Add(flockTag.Instance);
                    }

                    if (flock.CreaturePaletteIndex >= 0 && flock.CreaturePaletteIndex < BlamScnr.CreaturePalette.Count)
                    {
                        var creatureTag = BlamScnr.CreaturePalette[flock.CreaturePaletteIndex];
                        if (creatureTag != null && creatureTag.Object != null)
                            Add(creatureTag.Object);
                    }
                }
            }

            private void CollectDecals()
            {
                var visitedPaletteEntries = new HashSet<int>();
                for (int i = 0; i < BlamScnr.StructureBsps.Count; i++)
                {
                    var sbspRef = BlamScnr.StructureBsps[i];
                    if (((BspFlags)(1 << i) & DesiredBsps) == 0)
                        continue;

                    CacheFile blamCache = BlamCache;
                    var sbsp = BlamCache.Deserializer.Deserialize<ScenarioStructureBsp>(
                        new CacheSerializationContext(ref blamCache, BlamCache.GetIndexItemFromID(sbspRef.StructureBsp.Index)));
       
                    for (int j = 0; j < BlamScnr.Decals.Count; j++)
                    {
                        var decal = BlamScnr.Decals[j];
                        if (decal.DecalPaletteIndex == -1)
                            continue;

                        if (visitedPaletteEntries.Contains(decal.DecalPaletteIndex))
                            continue;

                        if (decal.Position.X >= sbsp.WorldBoundsX.Lower && decal.Position.X <= sbsp.WorldBoundsX.Upper &&
                            decal.Position.Y >= sbsp.WorldBoundsY.Lower && decal.Position.Y <= sbsp.WorldBoundsY.Upper &&
                            decal.Position.Z >= sbsp.WorldBoundsZ.Lower && decal.Position.Z <= sbsp.WorldBoundsZ.Upper)
                        {
                            Add(BlamScnr.DecalPalette[decal.DecalPaletteIndex].Instance);
                            visitedPaletteEntries.Add(decal.DecalPaletteIndex);
                        }
                    }
                }
            }
        }
    }
}
