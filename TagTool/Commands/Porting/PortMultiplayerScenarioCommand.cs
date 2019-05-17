using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    class PortMultiplayerScenarioCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }
        private PortTagCommand PortTag { get; }

        public PortMultiplayerScenarioCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache, PortTagCommand portTag) :
            base(true,
                
                "PortMultiplayerScenario",
                "Builds a multiplayer map from a single campaign map scenario_structure_bsp.",
                
                "PortMultiplayerScenario <Tag>",

                "Builds a multiplayer map from a single campaign map scenario_structure_bsp.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            PortTag = portTag;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var tagName = args[0];
            CacheFile.IndexItem scnrInstance = null;

            var blamCache = BlamCache;

            //
            // Look up the map's scenario tag
            //

            foreach (var instance in blamCache.IndexItems)
            {
                if (instance == null)
                    continue;

                if (instance.IsInGroup<Scenario>())
                {
                    scnrInstance = instance;
                    break;
                }
            }

            if (scnrInstance == null)
            {
                Console.WriteLine("ERROR: No scenario tag found!");
                return true;
            }

            var scnr = blamCache.Deserializer.Deserialize<Scenario>(
                new CacheSerializationContext(ref blamCache, scnrInstance));

            //
            // Look up the bsp index
            //

            var bspIndex = -1;
            var removeStructureBsps = new HashSet<int>();

            for (var i = 0; i < scnr.StructureBsps.Count; i++)
            {
                var reference = scnr.StructureBsps[i];

                if (reference.StructureBsp == null)
                    continue;

                if (reference.StructureBsp.Name == tagName)
                    bspIndex = i;
                else if (!removeStructureBsps.Contains(i))
                    removeStructureBsps.Add(i);
            }

            if (bspIndex == -1)
            {
                Console.WriteLine($"ERROR: Invalid bsp name: {tagName}");
                return true;
            }

            var sbsp = blamCache.Deserializer.Deserialize<ScenarioStructureBsp>(
                new CacheSerializationContext(
                    ref blamCache,
                    blamCache.GetIndexItemFromID(scnr.StructureBsps[bspIndex].StructureBsp.Index)));

            var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            sbsp.SeamIdentifiers.Clear();

            //
            // Rebuild scenario zone set pvs
            //

            var newPvs = new Scenario.ZoneSetPvsBlock
            {
                StructureBspMask = Scenario.BspFlags.Bsp0,
                Version = 9,
                BspChecksums = new List<Scenario.ZoneSetPvsBlock.BspChecksum>
                {
                    new Scenario.ZoneSetPvsBlock.BspChecksum
                    {
                        Checksum = sbsp.BspChecksum
                    }
                },
                StructureBspPotentiallyVisibleSets = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet>
                {
                    new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet
                    {
                        Clusters = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster>(),
                        ClustersDoorsClosed = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster>(),
                        ClusterSkies = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky>(),
                        ClusterVisibleSkies = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky>(),
                        Unknown = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock>
                        {
                            new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock()
                        },
                        Unknown2 = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock>
                        {
                            new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock()
                        },
                        Clusters2 = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster2>()
                    }
                },
                PortalToDeviceMappings = new List<Scenario.ZoneSetPvsBlock.PortalToDeviceMapping>
                {
                    new Scenario.ZoneSetPvsBlock.PortalToDeviceMapping()
                }
            };

            for (var i = 0; i < scnr.ZoneSetPvs.Count; i++)
            {
                var oldPvs = scnr.ZoneSetPvs[i];

                for (int j = 0, k = 0; j < 32; j++)
                {
                    if ((oldPvs.StructureBspMask & (Scenario.BspFlags)(1 << j)) == 0)
                        continue;

                    if (j == bspIndex)
                    {
                        var oldSet = oldPvs.StructureBspPotentiallyVisibleSets[k];
                        var newSet = newPvs.StructureBspPotentiallyVisibleSets[0];

                        for (var clusterIndex = 0; clusterIndex < sbsp.Clusters.Count; clusterIndex++)
                        {
                            while (clusterIndex >= newSet.Clusters.Count)
                                newSet.Clusters.Add(new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster
                                {
                                    BitVectors = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector>
                                    {
                                        new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector
                                        {
                                            Bits = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit>
                                            {
                                                new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit()
                                            }
                                        }
                                    }
                                });

                            var oldCluster = oldSet.Clusters[clusterIndex];
                            var newCluster = newSet.Clusters[clusterIndex];

                            for (int l = 0; l < 32; l++)
                                if ((oldCluster.BitVectors[k].Bits[0].Allow & (Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags)(1 << l)) != 0)
                                    newCluster.BitVectors[0].Bits[0].Allow |= (Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags)(1 << l);
                        }

                        for (var clusterIndex = 0; clusterIndex < sbsp.Clusters.Count; clusterIndex++)
                        {
                            while (clusterIndex >= newSet.ClustersDoorsClosed.Count)
                                newSet.ClustersDoorsClosed.Add(new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster
                                {
                                    BitVectors = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector>
                                    {
                                        new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector
                                        {
                                            Bits = new List<Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit>
                                            {
                                                new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit()
                                            }
                                        }
                                    }
                                });

                            var oldCluster = oldSet.ClustersDoorsClosed[clusterIndex];
                            var newCluster = newSet.ClustersDoorsClosed[clusterIndex];

                            for (int l = 0; l < 32; l++)
                                if ((oldCluster.BitVectors[k].Bits[0].Allow & (Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags)(1 << l)) != 0)
                                    newCluster.BitVectors[0].Bits[0].Allow |= (Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags)(1 << l);
                        }

                        for (var clusterIndex = 0; clusterIndex < sbsp.Clusters.Count; clusterIndex++)
                        {
                            while (clusterIndex >= newSet.ClusterSkies.Count)
                                newSet.ClusterSkies.Add(new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky
                                {
                                    SkyIndex = -1
                                });

                            var oldClusterSky = oldSet.ClusterSkies[clusterIndex];
                            var newClusterSky = newSet.ClusterSkies[clusterIndex];

                            if (oldClusterSky.SkyIndex != -1)
                                newClusterSky.SkyIndex = oldClusterSky.SkyIndex;
                        }

                        for (var clusterIndex = 0; clusterIndex < sbsp.Clusters.Count; clusterIndex++)
                        {
                            while (clusterIndex >= newSet.ClusterVisibleSkies.Count)
                                newSet.ClusterVisibleSkies.Add(new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky
                                {
                                    SkyIndex = -1
                                });

                            var oldClusterSky = oldSet.ClusterVisibleSkies[clusterIndex];
                            var newClusterSky = newSet.ClusterVisibleSkies[clusterIndex];

                            if (oldClusterSky.SkyIndex != -1)
                                newClusterSky.SkyIndex = oldClusterSky.SkyIndex;
                        }

                        for (var clusterIndex = 0; clusterIndex < sbsp.Clusters.Count; clusterIndex++)
                        {
                            while (clusterIndex >= newSet.Clusters2.Count)
                                newSet.Clusters2.Add(new Scenario.ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster2());
                        }
                    }

                    k++;
                }
            }

            scnr.ZoneSetPvs = new List<Scenario.ZoneSetPvsBlock> { newPvs };

            //
            // Rebuild scenario zone set audibility
            //

            var newZoneSetAudibility = new List<Scenario.ZoneSetAudibilityBlock>();

            foreach (var audibility in scnr.ZoneSetAudibility)
            {
                var newAudibility = audibility.DeepClone();

                newAudibility.BspClusterToRoomBoundsMappings = new List<Scenario.ZoneSetAudibilityBlock.BspClusterToRoomBoundsMapping>
                {
                    audibility.BspClusterToRoomBoundsMappings[bspIndex].DeepClone()
                };

                newAudibility.GamePortalToDoorOccluderMappings = new List<Scenario.ZoneSetAudibilityBlock.GamePortalToDoorOccluderMapping>
                {
                    audibility.GamePortalToDoorOccluderMappings[bspIndex].DeepClone()
                };

                newZoneSetAudibility.Add(newAudibility);
            }

            scnr.ZoneSetAudibility = newZoneSetAudibility;

            //
            // Rebuild the scenario zone sets
            //

            scnr.ZoneSets = new List<Scenario.ZoneSet>
            {
                new Scenario.ZoneSet
                {
                    LoadedBsps = Scenario.BspFlags.Bsp0
                }
            };

            scnr.BspAtlas = new List<Scenario.BspAtlasBlock>
            {
                new Scenario.BspAtlasBlock
                {
                    Bsp = Scenario.BspFlags.Bsp0
                }
            };

            //
            // Rebuild the scenario scenery placements
            //

            var newScenery = new List<Scenario.SceneryInstance>();

            foreach (var scenery in scnr.Scenery)
            {
                if (scenery.OriginBspIndex == bspIndex)
                    newScenery.Add(scenery);
                else if (scenery.PaletteIndex != -1)
                    scnr.SceneryPalette[scenery.PaletteIndex].Object = null;
            }

            foreach (var Scenery in newScenery)
                Scenery.OriginBspIndex = 0;

            scnr.Scenery = newScenery;

            //
            // Rebuild the scenario biped placements
            //

            var newBipeds = new List<Scenario.BipedInstance>();

            foreach (var biped in scnr.Bipeds)
            {
                if (biped.OriginBspIndex == bspIndex)
                    newBipeds.Add(biped);
                else if (biped.PaletteIndex != -1)
                    scnr.BipedPalette[biped.PaletteIndex].Object = null;
            }

            foreach (var Biped in newBipeds)
                Biped.OriginBspIndex = 0;

            scnr.Bipeds = newBipeds;

            //
            // Rebuild the scenario vehicle placements
            //

            var newVehicles = new List<Scenario.VehicleInstance>();

            foreach (var vehicle in scnr.Vehicles)
            {
                if (vehicle.OriginBspIndex == bspIndex)
                    newVehicles.Add(vehicle);
                else if (vehicle.PaletteIndex != -1)
                    scnr.VehiclePalette[vehicle.PaletteIndex].Object = null;
            }

            foreach (var Vehicle in newVehicles)
                Vehicle.OriginBspIndex = 0;

            scnr.Vehicles = newVehicles;

            //
            // Rebuild the scenario equipment placements
            //

            var newEquipment = new List<Scenario.EquipmentInstance>();

            foreach (var equipment in scnr.Equipment)
            {
                if (equipment.OriginBspIndex == bspIndex)
                    newEquipment.Add(equipment);
                else if (equipment.PaletteIndex != -1)
                    scnr.EquipmentPalette[equipment.PaletteIndex].Object = null;
            }

            foreach (var Equipment in newEquipment)
                Equipment.OriginBspIndex = 0;

            scnr.Equipment = newEquipment;

            //
            // Rebuild the scenario weapon placements
            //

            var newWeapons = new List<Scenario.WeaponInstance>();

            foreach (var weapon in scnr.Weapons)
            {
                if (weapon.OriginBspIndex == bspIndex)
                    newWeapons.Add(weapon);
                else if (weapon.PaletteIndex != -1)
                    scnr.WeaponPalette[weapon.PaletteIndex].Object = null;
            }

            foreach (var Weapon in newWeapons)
                Weapon.OriginBspIndex = 0;

            scnr.Weapons = newWeapons;

            //
            // Rebuild the scenario machine placements
            //

            var newMachines = new List<Scenario.MachineInstance>();

            foreach (var machine in scnr.Machines)
            {
                if (machine.OriginBspIndex == bspIndex)
                    newMachines.Add(machine);
                else if (machine.PaletteIndex != -1)
                    scnr.MachinePalette[machine.PaletteIndex].Object = null;
            }

            foreach (var Machine in newMachines)
                Machine.OriginBspIndex = 0;

            scnr.Machines = newMachines;

            //
            // Rebuild the scenario terminal placements
            //

            var newTerminals = new List<Scenario.TerminalInstance>();

            foreach (var terminal in scnr.Terminals)
            {
                if (terminal.OriginBspIndex == bspIndex)
                    newTerminals.Add(terminal);
                else if (terminal.PaletteIndex != -1)
                    scnr.TerminalPalette[terminal.PaletteIndex].Object = null;
            }

            foreach (var Terminal in newTerminals)
                Terminal.OriginBspIndex = 0;

            scnr.Terminals = newTerminals;

            //
            // Rebuild the scenario alternateRealityDevice placements
            //

            var newAlternateRealityDevices = new List<Scenario.AlternateRealityDeviceInstance>();

            foreach (var alternateRealityDevice in scnr.AlternateRealityDevices)
            {
                if (alternateRealityDevice.OriginBspIndex == bspIndex)
                    newAlternateRealityDevices.Add(alternateRealityDevice);
                else if (alternateRealityDevice.PaletteIndex != -1)
                    scnr.AlternateRealityDevicePalette[alternateRealityDevice.PaletteIndex].Object = null;
            }

            foreach (var AlternateRealityDevice in newAlternateRealityDevices)
                AlternateRealityDevice.OriginBspIndex = 0;

            scnr.AlternateRealityDevices = newAlternateRealityDevices;

            //
            // Rebuild the scenario control placements
            //

            var newControls = new List<Scenario.ControlInstance>();

            foreach (var control in scnr.Controls)
            {
                if (control.OriginBspIndex == bspIndex)
                    newControls.Add(control);
                else if (control.PaletteIndex != -1)
                    scnr.ControlPalette[control.PaletteIndex].Object = null;
            }

            foreach (var Control in newControls)
                Control.OriginBspIndex = 0;

            scnr.Controls = newControls;

            //
            // Rebuild the scenario soundScenery placements
            //

            var newSoundScenery = new List<Scenario.SoundSceneryInstance>();

            foreach (var soundScenery in scnr.SoundScenery)
            {
                if (soundScenery.OriginBspIndex == bspIndex)
                    newSoundScenery.Add(soundScenery);
                else if (soundScenery.PaletteIndex != -1)
                    scnr.SoundSceneryPalette[soundScenery.PaletteIndex].Object = null;
            }

            foreach (var SoundScenery in newSoundScenery)
                SoundScenery.OriginBspIndex = 0;

            scnr.SoundScenery = newSoundScenery;

            //
            // Rebuild the scenario giant placements
            //

            var newGiants = new List<Scenario.GiantInstance>();

            foreach (var giant in scnr.Giants)
            {
                if (giant.OriginBspIndex == bspIndex)
                    newGiants.Add(giant);
                else if (giant.PaletteIndex != -1)
                    scnr.GiantPalette[giant.PaletteIndex].Object = null;
            }

            foreach (var Giant in newGiants)
                Giant.OriginBspIndex = 0;

            scnr.Giants = newGiants;

            //
            // Rebuild the scenario effectScenery placements
            //

            var newEffectScenery = new List<Scenario.EffectSceneryInstance>();

            foreach (var effectScenery in scnr.EffectScenery)
            {
                if (effectScenery.OriginBspIndex == bspIndex)
                    newEffectScenery.Add(effectScenery);
                else if (effectScenery.PaletteIndex != -1)
                    scnr.EffectSceneryPalette[effectScenery.PaletteIndex].Object = null;
            }

            foreach (var EffectScenery in newEffectScenery)
                EffectScenery.OriginBspIndex = 0;

            scnr.EffectScenery = newEffectScenery;

            //
            // Rebuild the scenario lightVolume placements
            //

            var newLightVolumes = new List<Scenario.LightVolumeInstance>();

            foreach (var lightVolume in scnr.LightVolumes)
            {
                if (lightVolume.OriginBspIndex == bspIndex)
                    newLightVolumes.Add(lightVolume);
                else if (lightVolume.PaletteIndex != -1)
                    scnr.LightVolumePalette[lightVolume.PaletteIndex].Object = null;
            }

            foreach (var LightVolume in newLightVolumes)
                LightVolume.OriginBspIndex = 0;

            scnr.LightVolumes = newLightVolumes;

            //
            // Rebuild the scenario crate placements
            //

            var newCrates = new List<Scenario.CrateInstance>();

            foreach (var crate in scnr.Crates)
            {
                if (crate.OriginBspIndex == bspIndex)
                    newCrates.Add(crate);
                else if (crate.PaletteIndex != -1)
                    scnr.CratePalette[crate.PaletteIndex].Object = null;
            }

            foreach (var Crate in newCrates)
                Crate.OriginBspIndex = 0;

            scnr.Crates = newCrates;

            //
            // Rebuild the scenario flock placements
            //

            var newFlocks = new List<Scenario.Flock>();
            var flockPaletteIndices = new List<short>();

            foreach (var flock in scnr.Flocks)
            {
                if (flock.BspIndex == bspIndex)
                    newFlocks.Add(flock);
                else if (flock.CreaturePaletteIndex != -1)
                    flockPaletteIndices.Add(flock.CreaturePaletteIndex);
            }

            foreach (var Flock in newFlocks)
                Flock.BspIndex = (short)bspIndex;

            scnr.Flocks = newFlocks;

            //
            // Rebuild the scenario creature placements
            //

            var newCreatures = new List<Scenario.CreatureInstance>();

            foreach (var creature in scnr.Creatures)
            {
                if (creature.OriginBspIndex == bspIndex && flockPaletteIndices.Contains(creature.PaletteIndex))
                    newCreatures.Add(creature);
                else if (creature.PaletteIndex != -1)
                    scnr.CreaturePalette[creature.PaletteIndex].Object = null;
            }

            foreach (var Creature in newCreatures)
                Creature.OriginBspIndex = 0;

            for (short x = 0; x < scnr.CreaturePalette.Count; x++)
            {
                if (!flockPaletteIndices.Contains(x))
                {
                    scnr.CreaturePalette[x].Object = null;
                    continue;
                }
            }

            scnr.Creatures = newCreatures;

            //
            // Rebuild the scenario cluster data
            //

            scnr.ScenarioClusterData = new List<Scenario.ScenarioClusterDatum> { scnr.ScenarioClusterData[bspIndex] };

            //
            // Final cleanup to the scenario definition
            //

            scnr.MapType = ScenarioMapType.Multiplayer;
            scnr.MapSubType = ScenarioMapSubType.None;
            scnr.CampaignId = -1;

            scnr.StructureBsps = new List<Scenario.StructureBspBlock> { scnr.StructureBsps[bspIndex] };

            scnr.PlayerStartingProfile.Clear();

            var initialPoint = newScenery.Where(x => x.OriginBspIndex != -1).First().Position;

            scnr.PlayerStartingLocations = new List<Scenario.PlayerStartingLocation>
            {
                new Scenario.PlayerStartingLocation
                {
                    Position = initialPoint
                }
            };

            scnr.CutsceneCameraPoints = new List<Scenario.CutsceneCameraPoint>
            {
                new Scenario.CutsceneCameraPoint
                {
                    Flags = Scenario.CutsceneCameraPointFlags.PrematchCameraHack,
                    Position = initialPoint
                }
            };

            scnr.SoftCeilings = new List<Scenario.SoftCeiling>();
            scnr.TriggerVolumes = new List<Scenario.TriggerVolume>();
            scnr.RecordedAnimations = new List<Scenario.RecordedAnimation>();
            scnr.ZonesetSwitchTriggerVolumes = new List<Scenario.ZoneSetSwitchTriggerVolume>();
            scnr.Unknown32 = new List<Scenario.UnknownBlock>();
            scnr.Unknown33 = new List<Scenario.UnknownBlock>();
            scnr.Unknown34 = new List<Scenario.UnknownBlock>();
            scnr.Unknown35 = new List<Scenario.UnknownBlock>();
            scnr.Unknown36 = new List<Scenario.UnknownBlock>();
            scnr.StylePalette = new List<TagReferenceBlock>();
            scnr.SquadGroups = new List<Scenario.SquadGroup>();
            scnr.Squads = new List<Scenario.Squad>();
            scnr.Zones = new List<Scenario.Zone>();
            scnr.SquadPatrols = new List<Scenario.SquadPatrol>();
            scnr.MissionScenes = new List<Scenario.MissionScene>();
            scnr.CharacterPalette = new List<TagReferenceBlock>();
            scnr.AiPathfindingData = new List<Scenario.AiPathfindingDatum> { new Scenario.AiPathfindingDatum() };
            scnr.Scripts = new List<Scripting.Script>();
            scnr.Globals = new List<Scripting.ScriptGlobal>();
            scnr.ScriptingData = new List<Scenario.ScriptingDatum> { new Scenario.ScriptingDatum() };
            scnr.CutsceneFlags = new List<Scenario.CutsceneFlag>();
            scnr.CutsceneTitles = new List<Scenario.CutsceneTitle>();
            scnr.CustomObjectNameStrings = null;
            scnr.ChapterTitleStrings = null;
            scnr.UnitSeatsMapping = new List<Scenario.UnitSeatsMappingBlock>();
            scnr.ScenarioKillTriggers = new List<Scenario.ScenarioKillTrigger>();
            scnr.ScenarioSafeTriggers = new List<Scenario.ScenarioSafeTrigger>();
            scnr.ScriptExpressions = new List<Scripting.ScriptExpression>();
            scnr.SubtitleStrings = null;
            scnr.MissionDialogue = new List<TagReferenceBlock>();
            scnr.ObjectiveStrings = null;
            scnr.Interpolators = new List<Scenario.Interpolator>();
            scnr.SimulationDefinitionTable = new List<Scenario.SimulationDefinitionTableBlock>();
            scnr.AiObjectIds = new List<Scenario.AiObjectId>();
            scnr.AiObjectives = new List<Scenario.AiObjective>();
            scnr.DesignerZoneSets = new List<Scenario.DesignerZoneSet>();
            scnr.Unknown135 = new List<Scenario.UnknownBlock5>();
            scnr.Cinematics = new List<TagReferenceBlock>();
            scnr.CinematicLighting = new List<Scenario.CinematicLightingBlock>();
            scnr.ScenarioMetagame = new List<Scenario.ScenarioMetagameBlock>();

            var sLdT = blamCache.Deserializer.Deserialize<ScenarioLightmap>(
                new CacheSerializationContext(
                    ref blamCache, blamCache.GetIndexItemFromID(scnr.Lightmap.Index)));

            if (blamCache.Version < CacheVersion.Halo3ODST)
                sLdT.Lightmaps = new List<ScenarioLightmapBspData> { sLdT.Lightmaps[bspIndex] };
            else
                sLdT.LightmapDataReferences = new List<ScenarioLightmap.LightmapDataReference> { sLdT.LightmapDataReferences[bspIndex] };

            using (var cacheStream = CacheContext.OpenTagCacheReadWrite())
            {
                PortTag.SetFlags(PortTagCommand.PortingFlags.Default);
                PortTag.RemoveFlags(PortTagCommand.PortingFlags.Ms30 | PortTagCommand.PortingFlags.ForgePalette);

                CacheContext.Serialize(cacheStream,
                    CacheContext.AllocateTag<ScenarioStructureBsp>(scnr.StructureBsps[0].StructureBsp.Name),
                    (ScenarioStructureBsp)PortTag.ConvertData(cacheStream, resourceStreams, sbsp, sbsp, scnr.StructureBsps[0].StructureBsp.Name));

                CacheContext.Serialize(cacheStream,
                    CacheContext.AllocateTag<ScenarioLightmap>(scnr.Lightmap.Name),
                    (ScenarioLightmap)PortTag.ConvertData(cacheStream, resourceStreams, sLdT, sLdT, scnr.Lightmap.Name));

                var newScnrTag = CacheContext.AllocateTag<Scenario>(scnrInstance.Name);
                scnr = (Scenario)PortTag.ConvertData(cacheStream, resourceStreams, scnr, scnr, scnrInstance.Name);
                
                scnr.PlayerStartingProfile = new List<Scenario.PlayerStartingProfileBlock>
                {
                    new Scenario.PlayerStartingProfileBlock
                    {
                        Name = "start_assault",
                        PrimaryWeapon = CacheContext.GetTag<Weapon>(@"objects\weapons\rifle\assault_rifle\assault_rifle"),
                        PrimaryRoundsLoaded = 32,
                        PrimaryRoundsTotal = 96,
                        StartingFragGrenadeCount = 2,
                        Unknown3 = -1
                    }
                };

                CacheContext.Serialize(cacheStream, newScnrTag, scnr);
            }

            return true;
        }
    }
}
