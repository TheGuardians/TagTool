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
    class ExportMultiplayerScriptCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private CacheFile BlamCache { get; }

        public ExportMultiplayerScriptCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,
                
                "ExportMultiplayerScript",
                "Exports a script to build a multiplayer map from a single campaign map scenario_structure_bsp.",
                
                "ExportMultiplayerScript <Tag> <File>",

                "Exports a script to build a multiplayer map from a single campaign map scenario_structure_bsp.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
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

            //
            // Update scenario zone set pvs
            //

            var removePvsBspChecksums = new Dictionary<int, HashSet<int>>();
            var removePvsBsps = new Dictionary<int, HashSet<int>>();
            var removePvsPortals = new Dictionary<int, HashSet<int>>();

            for (var i = 0; i < scnr.ZoneSetPvs.Count; i++)
            {
                var pvs = scnr.ZoneSetPvs[i];

                if (!removePvsBspChecksums.ContainsKey(i))
                    removePvsBspChecksums[i] = new HashSet<int>();

                if (!removePvsBsps.ContainsKey(i))
                    removePvsBsps[i] = new HashSet<int>();

                if (!removePvsPortals.ContainsKey(i))
                    removePvsPortals[i] = new HashSet<int>();

                for (int j = 0, k = 0; j < 32; j++)
                {
                    if ((pvs.StructureBspMask & (Scenario.BspFlags)(1 << j)) != 0)
                    {
                        if (j != bspIndex)
                        {
                            if (!removePvsBspChecksums[i].Contains(k))
                                removePvsBspChecksums[i].Add(k);

                            if (!removePvsBsps[i].Contains(k))
                                removePvsBsps[i].Add(k);
                        }
                        k++;
                    }
                }

                for (var j = 0; j < scnr.StructureBsps.Count; j++)
                {
                    if (j != bspIndex && !removePvsPortals[i].Contains(j))
                        removePvsPortals[i].Add(j);
                }
            }

            //
            // Update scenario zone set audibility
            //

            var removeAudibilityBsps = new Dictionary<int, HashSet<int>>();
            var removeAudibilityBspClusters = new Dictionary<int, HashSet<int>>();

            for (var i = 0; i < scnr.ZoneSetAudibility.Count; i++)
            {
                if (!removeAudibilityBsps.ContainsKey(i))
                    removeAudibilityBsps[i] = new HashSet<int>();

                if (!removeAudibilityBspClusters.ContainsKey(i))
                    removeAudibilityBspClusters[i] = new HashSet<int>();

                for (var j = 0; j < scnr.StructureBsps.Count; j++)
                {
                    if (j != bspIndex)
                    {
                        if (!removeAudibilityBsps[i].Contains(j))
                            removeAudibilityBsps[i].Add(j);

                        if (!removeAudibilityBspClusters[i].Contains(j))
                            removeAudibilityBspClusters[i].Add(j);
                    }
                }
            }

            //
            // Update the scenario scenery placements
            //

            var removeScenery = new HashSet<int>();
            var keepSceneryPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Scenery.Count; i++)
            {
                var scenery = scnr.Scenery[i];

                if (removeScenery.Contains(i))
                    continue;
                else if (scenery.OriginBspIndex != bspIndex)
                    removeScenery.Add(i);
                else if (scenery.PaletteIndex != -1 && !keepSceneryPalettes.Contains(scenery.PaletteIndex))
                    keepSceneryPalettes.Add(scenery.PaletteIndex);
            }

            //
            // Update the scenario biped placements
            //

            var removeBipeds = new HashSet<int>();
            var keepBipedPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Bipeds.Count; i++)
            {
                var biped = scnr.Bipeds[i];

                if (removeBipeds.Contains(i))
                    continue;
                else if (biped.OriginBspIndex != bspIndex)
                    removeBipeds.Add(i);
                else if (biped.PaletteIndex != -1 && !keepBipedPalettes.Contains(biped.PaletteIndex))
                    keepBipedPalettes.Add(biped.PaletteIndex);
            }

            //
            // Update the scenario vehicle placements
            //

            var removeVehicles = new HashSet<int>();
            var keepVehiclePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Vehicles.Count; i++)
            {
                var vehicle = scnr.Vehicles[i];

                if (removeVehicles.Contains(i))
                    continue;
                else if (vehicle.OriginBspIndex != bspIndex)
                    removeVehicles.Add(i);
                else if (vehicle.PaletteIndex != -1 && !keepVehiclePalettes.Contains(vehicle.PaletteIndex))
                    keepVehiclePalettes.Add(vehicle.PaletteIndex);
            }

            //
            // Update the scenario equipment placements
            //

            var removeEquipment = new HashSet<int>();
            var keepEquipmentPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Equipment.Count; i++)
            {
                var equipment = scnr.Equipment[i];

                if (removeEquipment.Contains(i))
                    continue;
                else if (equipment.OriginBspIndex != bspIndex)
                    removeEquipment.Add(i);
                else if (equipment.PaletteIndex != -1 && !keepEquipmentPalettes.Contains(equipment.PaletteIndex))
                    keepEquipmentPalettes.Add(equipment.PaletteIndex);
            }

            //
            // Update the scenario weapon placements
            //

            var removeWeapons = new HashSet<int>();
            var keepWeaponPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Weapons.Count; i++)
            {
                var weapon = scnr.Weapons[i];

                if (removeWeapons.Contains(i))
                    continue;
                else if (weapon.OriginBspIndex != bspIndex)
                    removeWeapons.Add(i);
                else if (weapon.PaletteIndex != -1 && !keepWeaponPalettes.Contains(weapon.PaletteIndex))
                    keepWeaponPalettes.Add(weapon.PaletteIndex);
            }

            //
            // Update the scenario machine placements
            //

            var removeMachines = new HashSet<int>();
            var keepMachinePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Machines.Count; i++)
            {
                var machine = scnr.Machines[i];

                if (removeMachines.Contains(i))
                    continue;
                else if (machine.OriginBspIndex != bspIndex)
                    removeMachines.Add(i);
                else if (machine.PaletteIndex != -1 && !keepMachinePalettes.Contains(machine.PaletteIndex))
                    keepMachinePalettes.Add(machine.PaletteIndex);
            }

            //
            // Update the scenario terminal placements
            //

            var removeTerminals = new HashSet<int>();
            var keepTerminalPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Terminals.Count; i++)
            {
                var terminal = scnr.Terminals[i];

                if (removeTerminals.Contains(i))
                    continue;
                else if (terminal.OriginBspIndex != bspIndex)
                    removeTerminals.Add(i);
                else if (terminal.PaletteIndex != -1 && !keepTerminalPalettes.Contains(terminal.PaletteIndex))
                    keepTerminalPalettes.Add(terminal.PaletteIndex);
            }

            //
            // Update the scenario argDevice placements
            //

            var removeAlternateRealityDevices = new HashSet<int>();
            var keepAlternateRealityDevicePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.AlternateRealityDevices.Count; i++)
            {
                var argDevice = scnr.AlternateRealityDevices[i];

                if (removeAlternateRealityDevices.Contains(i))
                    continue;
                else if (argDevice.OriginBspIndex != bspIndex)
                    removeAlternateRealityDevices.Add(i);
                else if (argDevice.PaletteIndex != -1 && !keepAlternateRealityDevicePalettes.Contains(argDevice.PaletteIndex))
                    keepAlternateRealityDevicePalettes.Add(argDevice.PaletteIndex);
            }

            //
            // Update the scenario control placements
            //

            var removeControls = new HashSet<int>();
            var keepControlPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Controls.Count; i++)
            {
                var control = scnr.Controls[i];

                if (removeControls.Contains(i))
                    continue;
                else if (control.OriginBspIndex != bspIndex)
                    removeControls.Add(i);
                else if (control.PaletteIndex != -1 && !keepControlPalettes.Contains(control.PaletteIndex))
                    keepControlPalettes.Add(control.PaletteIndex);
            }

            //
            // Update the scenario soundSoundScenery placements
            //

            var removeSoundScenery = new HashSet<int>();
            var keepSoundSceneryPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.SoundScenery.Count; i++)
            {
                var soundScenery = scnr.SoundScenery[i];

                if (removeSoundScenery.Contains(i))
                    continue;
                else if (soundScenery.OriginBspIndex != bspIndex)
                    removeSoundScenery.Add(i);
                else if (soundScenery.PaletteIndex != -1 && !keepSoundSceneryPalettes.Contains(soundScenery.PaletteIndex))
                    keepSoundSceneryPalettes.Add(soundScenery.PaletteIndex);
            }

            //
            // Update the scenario giant placements
            //

            var removeGiants = new HashSet<int>();
            var keepGiantPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Giants.Count; i++)
            {
                var giant = scnr.Giants[i];

                if (removeGiants.Contains(i))
                    continue;
                else if (giant.OriginBspIndex != bspIndex)
                    removeGiants.Add(i);
                else if (giant.PaletteIndex != -1 && !keepGiantPalettes.Contains(giant.PaletteIndex))
                    keepGiantPalettes.Add(giant.PaletteIndex);
            }

            //
            // Update the scenario effectEffectScenery placements
            //

            var removeEffectScenery = new HashSet<int>();
            var keepEffectSceneryPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.EffectScenery.Count; i++)
            {
                var effectScenery = scnr.EffectScenery[i];

                if (removeEffectScenery.Contains(i))
                    continue;
                else if (effectScenery.OriginBspIndex != bspIndex)
                    removeEffectScenery.Add(i);
                else if (effectScenery.PaletteIndex != -1 && !keepEffectSceneryPalettes.Contains(effectScenery.PaletteIndex))
                    keepEffectSceneryPalettes.Add(effectScenery.PaletteIndex);
            }

            //
            // Update the scenario lightVolume placements
            //

            var removeLightVolumes = new HashSet<int>();
            var keepLightVolumePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.LightVolumes.Count; i++)
            {
                var lightVolume = scnr.LightVolumes[i];

                if (removeLightVolumes.Contains(i))
                    continue;
                else if (lightVolume.OriginBspIndex != bspIndex)
                    removeLightVolumes.Add(i);
                else if (lightVolume.PaletteIndex != -1 && !keepLightVolumePalettes.Contains(lightVolume.PaletteIndex))
                    keepLightVolumePalettes.Add(lightVolume.PaletteIndex);
            }

            //
            // Update the scenario crate placements
            //

            var removeCrates = new HashSet<int>();
            var keepCratePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Crates.Count; i++)
            {
                var crate = scnr.Crates[i];

                if (removeCrates.Contains(i))
                    continue;
                else if (crate.OriginBspIndex != bspIndex)
                    removeCrates.Add(i);
                else if (crate.PaletteIndex != -1 && !keepCratePalettes.Contains(crate.PaletteIndex))
                    keepCratePalettes.Add(crate.PaletteIndex);
            }

            //
            // Update the scenario creature placements
            //

            var removeCreatures = new HashSet<int>();
            var keepCreaturePalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Creatures.Count; i++)
            {
                var creature = scnr.Creatures[i];

                if (removeCreatures.Contains(i))
                    continue;
                else if (creature.OriginBspIndex != bspIndex)
                    removeCreatures.Add(i);
                else if (creature.PaletteIndex != -1 && !keepCreaturePalettes.Contains(creature.PaletteIndex))
                    keepCreaturePalettes.Add(creature.PaletteIndex);
            }

            //
            // Update the scenario flock placements
            //

            var removeFlocks = new HashSet<int>();
            var keepFlockPalettes = new HashSet<int>();

            for (var i = 0; i < scnr.Flocks.Count; i++)
            {
                var flock = scnr.Flocks[i];

                if (removeFlocks.Contains(i))
                    continue;
                else if (flock.BspIndex != bspIndex)
                    removeFlocks.Add(i);
                else
                {
                    if (flock.FlockPaletteIndex != -1 && !keepFlockPalettes.Contains(flock.FlockPaletteIndex))
                        keepFlockPalettes.Add(flock.FlockPaletteIndex);

                    if (flock.CreaturePaletteIndex != -1 && !keepCreaturePalettes.Contains(flock.CreaturePaletteIndex))
                        keepCreaturePalettes.Add(flock.CreaturePaletteIndex);
                }
            }

            //
            // Update the scenario cluster data
            //

            var removeClusterData = new HashSet<int>();

            for (var i = 0; i < scnr.StructureBsps.Count; i++)
            {
                if (removeClusterData.Contains(i))
                    continue;

                if (i != bspIndex)
                    removeClusterData.Add(i);
            }

            //
            // Export the script file
            //

            var scriptFile = new FileInfo(args[1]);

            if (!scriptFile.Directory.Exists)
                scriptFile.Directory.Create();

            using (var writer = scriptFile.CreateText())
            {
                writer.WriteLine($"OpenCacheFile \"{BlamCache.File.FullName}\"");
                writer.WriteLine();
                writer.WriteLine($"PortTag {scnr.StructureBsps[bspIndex].StructureBsp.Name}.scenario_structure_bsp");
                writer.WriteLine($"PortTag {scnr.StructureBsps[bspIndex].Design.Name}.structure_design");
                writer.WriteLine($"PortTag {scnr.StructureBsps[bspIndex].Cubemap.Name}.bitmap");
                writer.WriteLine();
                writer.WriteLine($"PortTag {scnr.SkyReferences[0].SkyObject.Name}.{scnr.SkyReferences[0].SkyObject.Group.Tag}");
                writer.WriteLine();

                foreach (var index in keepSceneryPalettes)
                {
                    var instance = scnr.SceneryPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepBipedPalettes)
                {
                    var instance = scnr.BipedPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepVehiclePalettes)
                {
                    var instance = scnr.VehiclePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepEquipmentPalettes)
                {
                    var instance = scnr.EquipmentPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepWeaponPalettes)
                {
                    var instance = scnr.WeaponPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepMachinePalettes)
                {
                    var instance = scnr.MachinePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepTerminalPalettes)
                {
                    var instance = scnr.TerminalPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepAlternateRealityDevicePalettes)
                {
                    var instance = scnr.AlternateRealityDevicePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepControlPalettes)
                {
                    var instance = scnr.ControlPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepSoundSceneryPalettes)
                {
                    var instance = scnr.SoundSceneryPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepGiantPalettes)
                {
                    var instance = scnr.GiantPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepEffectSceneryPalettes)
                {
                    var instance = scnr.EffectSceneryPalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepLightVolumePalettes)
                {
                    var instance = scnr.LightVolumePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepCratePalettes)
                {
                    var instance = scnr.CratePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                foreach (var index in keepCreaturePalettes)
                {
                    var instance = scnr.CreaturePalette[index].Object;
                    if (instance != null)
                        writer.WriteLine($"PortTag {instance.Name}.{instance.Group.Tag}");
                }

                writer.WriteLine();

                if (scnr.DefaultCameraFx != null)
                    writer.WriteLine($"PortTag {scnr.DefaultCameraFx.Name}.camera_fx_settings");

                if (scnr.DefaultScreenFx != null)
                    writer.WriteLine($"PortTag {scnr.DefaultScreenFx.Name}.area_screen_effect");

                if (scnr.SkyParameters != null)
                    writer.WriteLine($"PortTag {scnr.SkyParameters.Name}.sky_atm_parameters");

                if (scnr.GlobalLighting != null)
                    writer.WriteLine($"PortTag {scnr.GlobalLighting.Name}.chocolate_mountain_new");

                if (scnr.Lightmap != null)
                {
                    if (blamCache.Version < CacheVersion.Halo3ODST)
                    {
                        writer.WriteLine($"PortTag {scnr.Lightmap.Name}.scenario_lightmap");
                    }
                    else
                    {
                        var sldt = blamCache.Deserializer.Deserialize<ScenarioLightmap>(
                            new CacheSerializationContext(ref blamCache, blamCache.GetIndexItemFromID(scnr.Lightmap.Index)));

                        writer.WriteLine($"PortTag {sldt.LightmapDataReferences[bspIndex].LightmapData.Name}.scenario_lightmap_bsp_data");
                        writer.WriteLine($"PortTag Single {scnr.Lightmap.Name}.scenario_lightmap");
                    }
                }

                if (scnr.PerformanceThrottles != null)
                    writer.WriteLine($"PortTag {scnr.PerformanceThrottles.Name}.performance_throttles");

                writer.WriteLine();
                writer.WriteLine($"PortTag Single {scnrInstance.Name}.scenario");
                writer.WriteLine("Exit");
                writer.WriteLine();

                if (scnr.Lightmap != null)
                {
                    writer.WriteLine($"EditTag {scnr.Lightmap.Name}.scenario_lightmap");

                    for (var i = scnr.StructureBsps.Count - 1; i >= 0; i--)
                        if (i != bspIndex)
                            writer.WriteLine($"RemoveBlockElements LightmapDataReferences {i} 1");

                    writer.WriteLine("SaveTagChanges");
                    writer.WriteLine("Exit");
                    writer.WriteLine();
                }

                writer.WriteLine();
                writer.WriteLine($"EditTag {scnrInstance.Name}.scenario");
                writer.WriteLine();

                foreach (var entry in removePvsBspChecksums)
                    foreach (var index in entry.Value.Reverse())
                        writer.WriteLine($"RemoveBlockElements ZoneSetPvs[{entry.Key}].BspChecksums {index} 1");

                writer.WriteLine();

                foreach (var entry in removePvsBsps)
                    foreach (var index in entry.Value.Reverse())
                        writer.WriteLine($"RemoveBlockElements ZoneSetPvs[{entry.Key}].StructureBspPotentiallyVisibleSets {index} 1");

                writer.WriteLine();

                foreach (var entry in removePvsPortals)
                    foreach (var index in entry.Value.Reverse())
                        writer.WriteLine($"RemoveBlockElements ZoneSetPvs[{entry.Key}].PortalToDeviceMappings {index} 1");

                writer.WriteLine();
                writer.WriteLine("ForEach ZoneSetPvs SetField StructureBspMask Bsp0");

                foreach (var entry in removePvsBspChecksums)
                {
                    var pvs = scnr.ZoneSetPvs[entry.Key];

                    foreach (var set in pvs.StructureBspPotentiallyVisibleSets)
                    {
                        var setIndex = pvs.StructureBspPotentiallyVisibleSets.IndexOf(set);

                        foreach (var cluster in set.Clusters)
                        {
                            var clusterIndex = set.Clusters.IndexOf(cluster);

                            foreach (var index in entry.Value.Reverse())
                                writer.WriteLine($"RemoveBlockElements ZoneSetPvs[{entry.Key}].StructureBspPotentiallyVisibleSets[{setIndex}].Clusters[{clusterIndex}].BitVectors {index} 1");
                        }

                        foreach (var cluster in set.ClustersDoorsClosed)
                        {
                            var clusterIndex = set.ClustersDoorsClosed.IndexOf(cluster);

                            foreach (var index in entry.Value.Reverse())
                                writer.WriteLine($"RemoveBlockElements ZoneSetPvs[{entry.Key}].StructureBspPotentiallyVisibleSets[{setIndex}].ClustersDoorsClosed[{clusterIndex}].BitVectors {index} 1");
                        }
                    }
                }

                writer.WriteLine();

                foreach (var entry in removeAudibilityBsps)
                    foreach (var index in entry.Value.Reverse())
                        writer.WriteLine($"RemoveBlockElements ZoneSetAudibility[{entry.Key}].GamePortalToDoorOccluderMappings {index} 1");

                writer.WriteLine();

                foreach (var entry in removeAudibilityBspClusters)
                    foreach (var index in entry.Value.Reverse())
                        writer.WriteLine($"RemoveBlockElements ZoneSetAudibility[{entry.Key}].BspClusterToRoomBoundsMappings {index} 1");

                writer.WriteLine();
                writer.WriteLine("RemoveBlockElements ZoneSets 0 *");
                writer.WriteLine("AddBlockElements ZoneSets");
                writer.WriteLine("EditBlock ZoneSets[*]");
                writer.WriteLine("SetField Name default");
                writer.WriteLine("SetField LoadedBsps Bsp0");
                writer.WriteLine("SetField BspAtlasIndex -1");
                writer.WriteLine("Exit");
                writer.WriteLine();
                writer.WriteLine("RemoveBlockElements BspAtlas 0 *");
                writer.WriteLine("RemoveBlockElements CampaignPlayers 0 *");
                writer.WriteLine();

                foreach (var index in removeScenery.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Scenery {index} 1");

                for (var i = 0; i < scnr.SceneryPalette.Count; i++)
                    if (!keepSceneryPalettes.Contains(i))
                        writer.WriteLine($"SetField SceneryPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Scenery SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Scenery SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeBipeds.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Bipeds {index} 1");

                for (var i = 0; i < scnr.BipedPalette.Count; i++)
                    if (!keepBipedPalettes.Contains(i))
                        writer.WriteLine($"SetField BipedPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Bipeds SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Bipeds SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeVehicles.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Vehicles {index} 1");

                for (var i = 0; i < scnr.VehiclePalette.Count; i++)
                    if (!keepVehiclePalettes.Contains(i))
                        writer.WriteLine($"SetField VehiclePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Vehicles SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Vehicles SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeEquipment.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Equipment {index} 1");

                for (var i = 0; i < scnr.EquipmentPalette.Count; i++)
                    if (!keepEquipmentPalettes.Contains(i))
                        writer.WriteLine($"SetField EquipmentPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Equipment SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Equipment SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeWeapons.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Weapons {index} 1");

                for (var i = 0; i < scnr.WeaponPalette.Count; i++)
                    if (!keepWeaponPalettes.Contains(i))
                        writer.WriteLine($"SetField WeaponPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Weapons SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Weapons SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeMachines.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Machines {index} 1");

                for (var i = 0; i < scnr.MachinePalette.Count; i++)
                    if (!keepMachinePalettes.Contains(i))
                        writer.WriteLine($"SetField MachinePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Machines SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Machines SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeTerminals.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Terminals {index} 1");

                for (var i = 0; i < scnr.TerminalPalette.Count; i++)
                    if (!keepTerminalPalettes.Contains(i))
                        writer.WriteLine($"SetField TerminalPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Terminals SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Terminals SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeAlternateRealityDevices.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements AlternateRealityDevices {index} 1");

                for (var i = 0; i < scnr.AlternateRealityDevicePalette.Count; i++)
                    if (!keepAlternateRealityDevicePalettes.Contains(i))
                        writer.WriteLine($"SetField AlternateRealityDevicePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach AlternateRealityDevices SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach AlternateRealityDevices SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeControls.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Controls {index} 1");

                for (var i = 0; i < scnr.ControlPalette.Count; i++)
                    if (!keepControlPalettes.Contains(i))
                        writer.WriteLine($"SetField ControlPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Controls SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Controls SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeSoundScenery.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements SoundScenery {index} 1");

                for (var i = 0; i < scnr.SoundSceneryPalette.Count; i++)
                    if (!keepSoundSceneryPalettes.Contains(i))
                        writer.WriteLine($"SetField SoundSceneryPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach SoundScenery SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach SoundScenery SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeGiants.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Giants {index} 1");

                for (var i = 0; i < scnr.GiantPalette.Count; i++)
                    if (!keepGiantPalettes.Contains(i))
                        writer.WriteLine($"SetField GiantPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Giants SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Giants SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeEffectScenery.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements EffectScenery {index} 1");

                for (var i = 0; i < scnr.EffectSceneryPalette.Count; i++)
                    if (!keepEffectSceneryPalettes.Contains(i))
                        writer.WriteLine($"SetField EffectSceneryPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach EffectScenery SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach EffectScenery SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeLightVolumes.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements LightVolumes {index} 1");

                for (var i = 0; i < scnr.LightVolumePalette.Count; i++)
                    if (!keepLightVolumePalettes.Contains(i))
                        writer.WriteLine($"SetField LightVolumePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach LightVolumes SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach LightVolumes SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeCrates.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Crates {index} 1");

                for (var i = 0; i < scnr.CratePalette.Count; i++)
                    if (!keepCratePalettes.Contains(i))
                        writer.WriteLine($"SetField CratePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Crates SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Crates SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeCreatures.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Creatures {index} 1");

                for (var i = 0; i < scnr.CreaturePalette.Count; i++)
                    if (!keepCreaturePalettes.Contains(i))
                        writer.WriteLine($"SetField CreaturePalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Creatures SetField OriginBspIndex 0");
                writer.WriteLine($"ForEach Creatures SetField AllowedZoneSets 1");
                writer.WriteLine();

                foreach (var index in removeFlocks.Reverse())
                    writer.WriteLine($"echo RemoveBlockElements Flocks {index} 1");

                for (var i = 0; i < scnr.FlockPalette.Count; i++)
                    if (!keepFlockPalettes.Contains(i))
                        writer.WriteLine($"SetField FlockPalette[{i}].Object null");

                writer.WriteLine();
                writer.WriteLine($"ForEach Flocks SetField BspIndex 0");
                writer.WriteLine();
                writer.WriteLine("RemoveBlockElements SoftCeilings 0 *");
                writer.WriteLine("RemoveBlockElements PlayerStartingProfile 0 *");
                writer.WriteLine("RemoveBlockElements PlayerStartingLocations 0 *");
                writer.WriteLine("RemoveBlockElements TriggerVolumes 0 *");
                writer.WriteLine("RemoveBlockElements RecordedAnimations 0 *");
                writer.WriteLine("RemoveBlockElements ZonesetSwitchTriggerVolumes 0 *");
                writer.WriteLine("RemoveBlockElements Unknown32 0 *");
                writer.WriteLine("RemoveBlockElements Unknown33 0 *");
                writer.WriteLine("RemoveBlockElements Unknown34 0 *");
                writer.WriteLine("RemoveBlockElements Unknown35 0 *");
                writer.WriteLine("RemoveBlockElements Unknown36 0 *");
                writer.WriteLine("RemoveBlockElements StylePalette 0 *");
                writer.WriteLine("RemoveBlockElements SquadGroups 0 *");
                writer.WriteLine("RemoveBlockElements Squads 0 *");
                writer.WriteLine("RemoveBlockElements Zones 0 *");
                writer.WriteLine("RemoveBlockElements SquadPatrols 0 *");
                writer.WriteLine("RemoveBlockElements MissionScenes 0 *");
                writer.WriteLine("RemoveBlockElements CharacterPalette 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].LineSegments 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].Parallelograms 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].JumpHints 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].ClimbHints 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].WellHints 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].FlightHints 0 *");
                writer.WriteLine("RemoveBlockElements AiPathfindingData[0].CookieCutters 0 *");
                writer.WriteLine("RemoveBlockElements Scripts 0 *");
                writer.WriteLine("RemoveBlockElements Globals 0 *");
                writer.WriteLine("RemoveBlockElements ScriptingData[0].PointSets 0 *");
                writer.WriteLine("RemoveBlockElements CutsceneFlags 0 *");
                writer.WriteLine("RemoveBlockElements CutsceneCameraPoints 0 *");
                writer.WriteLine("RemoveBlockElements CutsceneTitles 0 *");
                writer.WriteLine("SetField CustomObjectNameStrings null");
                writer.WriteLine("SetField ChapterTitleStrings null");
                writer.WriteLine("RemoveBlockElements UnitSeatsMapping 0 *");
                writer.WriteLine("RemoveBlockElements ScenarioKillTriggers 0 *");
                writer.WriteLine("RemoveBlockElements ScenarioSafeTriggers 0 *");
                writer.WriteLine("RemoveBlockElements ScriptExpressions 0 *");
                writer.WriteLine();

                foreach (var index in removeClusterData.Reverse())
                    writer.WriteLine($"RemoveBlockElements ScenarioClusterData {index} 1");

                writer.WriteLine();
                writer.WriteLine("RemoveBlockElements SubtitleStrings 0 *");
                writer.WriteLine("RemoveBlockElements MissionDialogue 0 *");
                writer.WriteLine("SetField ObjectiveStrings null");
                writer.WriteLine("RemoveBlockElements Interpolators 0 *");
                writer.WriteLine("RemoveBlockElements SimulationDefinitionTable 0 *");
                writer.WriteLine("RemoveBlockElements AiObjectIds 0 *");
                writer.WriteLine("RemoveBlockElements AiObjectives 0 *");
                writer.WriteLine("RemoveBlockElements DesignerZoneSets 0 *");
                writer.WriteLine("RemoveBlockElements Unknown135 0 *");
                writer.WriteLine("RemoveBlockElements ScenarioMetagame 0 *");
                writer.WriteLine();
                writer.WriteLine("SaveTagChanges");
                writer.WriteLine("Exit");
                writer.WriteLine();
            }

            return true;
        }
    }
}
