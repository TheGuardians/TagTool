using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Scripting;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;
using TagTool.Tags.Resources;
using TagTool.Geometry;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Scenario CurrentScenario = null;

        private static readonly byte[] DefaultScenarioFxFunction = new byte[] { 0x01, 0x34, 0x00, 0x00, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x80, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        public Scenario ConvertScenario(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Scenario scnr, string tagName)
        {
            CurrentScenario = scnr;

            if (CacheVersionDetection.GetGameTitle(BlamCache.Version) == GameTitle.Halo3)
                scnr.Flags |= ScenarioFlags.H3Compatibility;

            foreach (var zoneset in scnr.ZoneSets)
            {
                // cex_ff_halo references bsps that don't exist, remove them
                zoneset.Bsps &= (Scenario.BspFlags)(scnr.StructureBsps.Count >= 32 ? uint.MaxValue : ~(-1u << scnr.StructureBsps.Count));
                if (scnr.BspAtlas == null || scnr.BspAtlas.Count == 0)
                    zoneset.HintPreviousZoneSet = -1;
            }
               

            //
            // Halo 3 scenario ai data
            //

            if (BlamCache.Version == CacheVersion.Halo3Retail &&
                scnr.MapType == ScenarioMapType.SinglePlayer &&
                Flags.HasFlag(PortingFlags.Recursive))
            {
                var pathfindingBsps = new List<StructureBspCacheFileTagResources>();

                Console.Write("Loading pathfinding bsps: ");

                for (var bspIndex = 0; bspIndex < scnr.StructureBsps.Count; bspIndex++)
                {
                    Console.Write($"{bspIndex}, ");

                    var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(cacheStream, scnr.StructureBsps[bspIndex].StructureBsp);

                    StructureBspCacheFileTagResources pathfindingBsp = null;

                    if (sbsp.PathfindingResource != null)
                    {
                        pathfindingBsp = CacheContext.ResourceCache.GetStructureBspCacheFileTagResources(sbsp.PathfindingResource);

                        if (!sbsp.PathfindingResource.HaloOnlinePageableResource.GetLocation(out var location))
                            throw new NullReferenceException();

                    }
                    pathfindingBsps.Add(pathfindingBsp);
                }

                Console.WriteLine("done.");

                foreach (var squad in scnr.Squads)
                {
                    squad.EditorFolderIndexNew = squad.EditorFolderIndexOld;
                    squad.SpawnFormations = new List<Scenario.Squad.SpawnFormation>();
                    squad.SpawnPoints = new List<Scenario.Squad.SpawnPoint>();
                    squad.SquadTemplate = null;
                    squad.TemplatedFireteams = new List<Scenario.Squad.Fireteam>();
                    squad.DesignerFireteams = new List<Scenario.Squad.Fireteam>();

                    for (var i = 0; i < squad.Fireteams.Count; i++)
                    {
                        var fireteam = squad.Fireteams[i];

                        fireteam.Name = StringId.Invalid;

                        if (fireteam.CharacterTypeIndex != -1)
                        {
                            fireteam.CharacterType = new List<Scenario.Squad.Fireteam.CharacterTypeBlock>
                            {
                                new Scenario.Squad.Fireteam.CharacterTypeBlock
                                {
                                    CharacterTypeIndex = fireteam.CharacterTypeIndex,
                                    Chance = 1
                                }
                            };

                            //
                            // Determine if the character is a giant to enable zone flags
                            //

                            if (squad.InitialZoneIndex != -1)
                            {
                                var unitIndex = -1;
                                var entry = scnr.CharacterPalette[fireteam.CharacterTypeIndex];

                                if (entry.Instance != null)
                                {
                                    using (var reader = new EndianReader(cacheStream, true))
                                    {
                                        cacheStream.Position = entry.Instance.DefinitionOffset + entry.Instance.DefinitionOffset + 32;
                                        unitIndex = reader.ReadInt32();
                                    }
                                }

                                if (unitIndex != -1 && (CacheContext.TagCache.GetTag(unitIndex)?.IsInGroup("gint") ?? false))
                                    scnr.Zones[squad.InitialZoneIndex].FlagsNew |= Scenario.Zone.ZoneFlagsNew.GiantsZone;
                            }
                        }

                        if (fireteam.InitialPrimaryWeaponIndex != -1)
                        {
                            fireteam.InitialPrimaryWeapon = new List<Scenario.Squad.Fireteam.ItemTypeBlock>
                            {
                                new Scenario.Squad.Fireteam.ItemTypeBlock
                                {
                                    ItemTypeIndex = fireteam.InitialPrimaryWeaponIndex,
                                    Probability = 1
                                }
                            };
                        }

                        if (fireteam.InitialSecondaryWeaponIndex != -1)
                        {
                            fireteam.InitialSecondaryWeapon = new List<Scenario.Squad.Fireteam.ItemTypeBlock>
                            {
                                new Scenario.Squad.Fireteam.ItemTypeBlock
                                {
                                    ItemTypeIndex = fireteam.InitialSecondaryWeaponIndex,
                                    Probability = 1
                                }
                            };
                        }

                        if (fireteam.InitialEquipmentIndex != -1)
                        {
                            fireteam.InitialEquipment = new List<Scenario.Squad.Fireteam.ItemTypeBlock>
                            {
                                new Scenario.Squad.Fireteam.ItemTypeBlock
                                {
                                    ItemTypeIndex = fireteam.InitialEquipmentIndex,
                                    Probability = 1
                                }
                            };
                        }

                        fireteam.VehicleVariant = ConvertStringId(fireteam.VehicleVariant);
                        fireteam.ActivityName = ConvertStringId(fireteam.ActivityName);

                        foreach (var point in fireteam.PatrolPoints)
                            point.ActivityName = ConvertStringId(point.ActivityName);

                        foreach (var spawnPoint in fireteam.StartingLocations)
                        {
                            spawnPoint.Name = ConvertStringId(spawnPoint.Name);
                            spawnPoint.FireteamIndex = (short)i;
                            spawnPoint.InitialEquipmentIndexNew = spawnPoint.InitialEquipmentIndexOld;
                            spawnPoint.ActorVariant = ConvertStringId(spawnPoint.ActorVariant);
                            spawnPoint.VehicleVariant = ConvertStringId(spawnPoint.VehicleVariant);

                            spawnPoint.InitialMovementModeNew = spawnPoint.InitialMovementModeOld;

                            spawnPoint.ActivityName = ConvertStringId(spawnPoint.ActivityName);

                            foreach (var point in spawnPoint.PatrolPoints)
                                point.ActivityName = ConvertStringId(point.ActivityName);

                            squad.SpawnPoints.Add(spawnPoint);
                        }

                        squad.DesignerFireteams.Add(fireteam);
                    }
                }

                foreach (var pathfindingdata in scnr.AiUserHintData)
                {
                    foreach (var cookieCutter in pathfindingdata.CookieCutters)
                    {
                        cookieCutter.SectorPoints = new List<Scenario.TriggerVolume.SectorPoint>
                        {
                            new Scenario.TriggerVolume.SectorPoint
                            {
                                Position = new RealPoint3d(
                                    cookieCutter.Position.X - 0.5f,
                                    cookieCutter.Position.Y - 0.5f,
                                    cookieCutter.Position.Z),
                                Normal = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.TriggerVolume.SectorPoint
                            {
                                Position = new RealPoint3d(
                                    cookieCutter.Position.X + 0.5f,
                                    cookieCutter.Position.Y - 0.5f,
                                    cookieCutter.Position.Z),
                                Normal = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.TriggerVolume.SectorPoint
                            {
                                Position = new RealPoint3d(
                                    cookieCutter.Position.X + 0.5f,
                                    cookieCutter.Position.Y + 0.5f,
                                    cookieCutter.Position.Z),
                                Normal = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.TriggerVolume.SectorPoint
                            {
                                Position = new RealPoint3d(
                                    cookieCutter.Position.X - 0.5f,
                                    cookieCutter.Position.Y + 0.5f,
                                    cookieCutter.Position.Z),
                                Normal = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            }
                        };
                    }

                    foreach (var block in pathfindingdata.Unknown9)
                        block.Unknown1 = block.UnknownH3;
                }

                var zoneAreaSectors = new List<List<List<(short, short)>>>();

                foreach (var zone in scnr.Zones)
                {
                    foreach (var firingPosition in zone.FiringPositions)
                    {
                        if (firingPosition.BspIndex != -1)
                            zone.BspFlags |= (ushort)(1 << firingPosition.BspIndex);

                        if (firingPosition.SectorBspIndex != -1)
                            zone.BspFlags |= (ushort)(1 << firingPosition.SectorBspIndex);
                    }

                    var areaSectors = new List<List<(short, short)>>();

                    for (var areaIndex = 0; areaIndex < zone.Areas.Count; areaIndex++)
                    {
                        var area = zone.Areas[areaIndex];

                        area.ManualReferenceFrameNew = area.ManualReferenceFrameOld;
                        //This is definitely a bsp index, not an area type
                        area.BSPIndex = zone.ManualBspIndex;
                        area.Points = new List<Scenario.Zone.Area.Point>()
                        {
                            new Scenario.Zone.Area.Point
                            {
                                Position = new RealPoint3d(
                                    area.RuntimeRelativeMeanPoint.X - area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Y - area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Z),
                                ReferenceFrame = -1,
                                BspIndex = area.BSPIndex, // TODO: find the proper bsp index
                                Facing = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.Zone.Area.Point
                            {
                                Position = new RealPoint3d(
                                    area.RuntimeRelativeMeanPoint.X + area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Y - area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Z),
                                ReferenceFrame = -1,
                                BspIndex = area.BSPIndex, // TODO: find the proper bsp index
                                Facing = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.Zone.Area.Point
                            {
                                Position = new RealPoint3d(
                                    area.RuntimeRelativeMeanPoint.X + area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Y + area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Z),
                                ReferenceFrame = -1,
                                BspIndex = area.BSPIndex, // TODO: find the proper bsp index
                                Facing = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            },
                            new Scenario.Zone.Area.Point
                            {
                                Position = new RealPoint3d(
                                    area.RuntimeRelativeMeanPoint.X - area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Y + area.RuntimeStandardDeviation,
                                    area.RuntimeRelativeMeanPoint.Z),
                                ReferenceFrame = -1,
                                BspIndex = area.BSPIndex, // TODO: find the proper bsp index
                                Facing = new RealEulerAngles2d(Angle.FromDegrees(0.0f), Angle.FromDegrees(90.0f))
                            }
                        };

                        var sectors = new List<(short, short)>();

                        foreach (var firingPosition in zone.FiringPositions)
                        {
                            if ((firingPosition.AreaIndex != areaIndex) || (firingPosition.SectorBspIndex == -1) || (firingPosition.SectorIndex == -1))
                                continue;

                            if (sectors.Contains((firingPosition.SectorBspIndex, firingPosition.SectorIndex)))
                                continue;

                            sectors.Add((firingPosition.SectorBspIndex, firingPosition.SectorIndex));
                        }

                        areaSectors.Add(sectors);
                    }

                    zoneAreaSectors.Add(areaSectors);
                }

                //
                // TODO: thoroughly check and possibly refactor the ai objective code below
                //

                foreach (var aiObjective in scnr.AiObjectives)
                {
                    foreach (var task in aiObjective.Tasks)
                    {
                        task.RuntimeFlags = Scenario.AiObjective.Task.RuntimeFlagsValue.AreaConnectivityValid;

                        if (!Enum.TryParse(task.Filter.Halo3Retail.ToString(), out task.Filter.Halo3Odst))
                            throw new NotSupportedException(task.Filter.Halo3Retail.ToString());

                        foreach (var taskArea in task.Areas)
                        {
                            if (taskArea.ZoneIndex == -1 || taskArea.AreaIndex == -1)
                                continue;

                            foreach (var entry1 in zoneAreaSectors[taskArea.ZoneIndex][taskArea.AreaIndex])
                            {
                                if (entry1.Item1 >= pathfindingBsps.Count || entry1.Item2 >= pathfindingBsps[entry1.Item1].PathfindingData[0].Sectors.Count)
                                {
                                    new TagToolWarning("Invalid zone area sector data!");
                                    continue;
                                }

                                var pathfinding = pathfindingBsps[entry1.Item1].PathfindingData[0];
                                var sector = pathfinding.Sectors[entry1.Item2];
                                var link = pathfinding.Links[sector.FirstLink];

                                while (true)
                                {
                                    if (link.LeftSector == entry1.Item2)
                                    {
                                        if (link.RightSector != -1)
                                        {
                                            for (var areaIndex = 0; areaIndex < task.Areas.Count; areaIndex++)
                                            {
                                                if (areaIndex == task.Areas.IndexOf(taskArea) || task.Areas[areaIndex].ZoneIndex != taskArea.ZoneIndex)
                                                    continue;

                                                foreach (var entry2 in zoneAreaSectors[task.Areas[areaIndex].ZoneIndex][task.Areas[areaIndex].AreaIndex])
                                                {
                                                    if (entry1.Item1 != entry2.Item1)
                                                        continue;

                                                    if (entry2.Item2 == link.LeftSector || entry2.Item2 == link.RightSector)
                                                        taskArea.ConnectivityBitVector |= 1 << areaIndex;
                                                }
                                            }
                                        }

                                        if (link.ForwardLink == sector.FirstLink)
                                            break;
                                        else
                                            link = pathfinding.Links[link.ForwardLink];
                                    }
                                    else if (link.RightSector == entry1.Item2)
                                    {
                                        if (link.LeftSector != -1)
                                        {
                                            for (var areaIndex = 0; areaIndex < task.Areas.Count; areaIndex++)
                                            {
                                                if (areaIndex == task.Areas.IndexOf(taskArea) || task.Areas[areaIndex].ZoneIndex != taskArea.ZoneIndex)
                                                    continue;

                                                foreach (var entry2 in zoneAreaSectors[task.Areas[areaIndex].ZoneIndex][task.Areas[areaIndex].AreaIndex])
                                                {
                                                    if (entry1.Item1 != entry2.Item1)
                                                        continue;

                                                    if (entry2.Item2 == link.LeftSector || entry2.Item2 == link.RightSector)
                                                        taskArea.ConnectivityBitVector |= 1 << areaIndex;
                                                }
                                            }
                                        }

                                        if (link.ReverseLink == sector.FirstLink)
                                            break;
                                        else
                                            link = pathfinding.Links[link.ReverseLink];
                                    }
                                }
                            }
                        }

                        foreach (var direction in task.Direction)
                            direction.Points = direction.Points_H3.ToList();
                    }
                }
            }

            AddPrematchCamera(cacheStream, scnr, tagName);

            //
            // Convert PlayerStartingProfiles
            //

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                if (scnr.PlayerStartingProfile.Count >= 4)
                {
                    var profile_0 = scnr.PlayerStartingProfile[0];
                    var profile_1 = scnr.PlayerStartingProfile[1];
                    var profile_2 = scnr.PlayerStartingProfile[2];
                    var profile_3 = scnr.PlayerStartingProfile[3];

                    scnr.PlayerStartingProfile.Insert(1, profile_2);
                    scnr.PlayerStartingProfile.Insert(2, profile_2); 
                    scnr.PlayerStartingProfile.Insert(3, profile_2); 

                    scnr.PlayerStartingProfile.Insert(4, profile_0);
                    scnr.PlayerStartingProfile.Insert(5, profile_2);
                    scnr.PlayerStartingProfile.Insert(6, profile_2);
                    scnr.PlayerStartingProfile.Insert(7, profile_2);

                    scnr.PlayerStartingProfile.Insert(8, profile_1);
                    scnr.PlayerStartingProfile.Insert(9, profile_3);
                    scnr.PlayerStartingProfile.Insert(10, profile_3);
                    scnr.PlayerStartingProfile.Insert(11, profile_3);

                    scnr.PlayerStartingProfile.Insert(12, profile_1);
                    scnr.PlayerStartingProfile.Insert(13, profile_3);
                    scnr.PlayerStartingProfile.Insert(14, profile_3);
                    scnr.PlayerStartingProfile.Insert(15, profile_3);

                    scnr.PlayerStartingProfile.RemoveAt(16);
                    scnr.PlayerStartingProfile.RemoveAt(16);
                    scnr.PlayerStartingProfile.RemoveAt(16);
                }
            }

            //
            // Convert scripts
            //

            if (FlagIsSet(PortingFlags.Scripts))
            {
                foreach (var global in scnr.Globals)
                {
                    ConvertHsType(global.Type);
                }

                foreach (var script in scnr.Scripts)
                {
                    ConvertHsType(script.ReturnType);

                    foreach (var parameter in script.Parameters)
                        ConvertHsType(parameter.Type);
                }

                foreach (var expr in scnr.ScriptExpressions)
                {
                    ConvertScriptExpression(cacheStream, blamCacheStream, resourceStreams, scnr, expr);
                }

                AdjustScripts(scnr, tagName);
            }
            else
            {
                scnr.Globals = new List<HsGlobal>();
                scnr.Scripts = new List<HsScript>();
                scnr.ScriptExpressions = new List<HsSyntaxNode>();
            }

            //
            // Remove functions from default screen fx
            //

            if (scnr.DefaultScreenFx != null)
            {
                var defaultSefc = CacheContext.Deserialize<AreaScreenEffect>(cacheStream, scnr.DefaultScreenFx);
                foreach (var screenEffect in defaultSefc.ScreenEffects)
                {
                    screenEffect.AngleFalloffFunction.Data = DefaultScenarioFxFunction;
                    screenEffect.DistanceFalloffFunction.Data = DefaultScenarioFxFunction;
                    screenEffect.TimeEvolutionFunction.Data = DefaultScenarioFxFunction;
                }
                CacheContext.Serialize(cacheStream, scnr.DefaultScreenFx, defaultSefc);
            }

            //
            // Reach fixups
            //

            if(BlamCache.Version >= CacheVersion.HaloReach)
            {
                for(int i = 0; i < scnr.TriggerVolumes.Count; i++)
                    scnr.TriggerVolumes[i] = ConvertTriggerVolumeReach(scnr.TriggerVolumes[i]);
            }

            if (BlamCache.Version >= CacheVersion.HaloReach && Flags.HasFlag(PortingFlags.Recursive))
            {
                // convert structure design

                if (scnr.StructureDesigns.Count > 0)
                {
                    if (scnr.StructureDesigns.Count > 1)
                    {
                        new TagToolWarning("Multiple structure designs currently not supported.");
                    }
                    else
                    {
                        var sddtTag = ConvertTag(cacheStream, blamCacheStream, resourceStreams, scnr.StructureDesigns[0].StructureDesign);
                        for (int i = 0; i < scnr.StructureBsps.Count; i++)
                            scnr.StructureBsps[i].Design = sddtTag;
                    }
                }

                var lightmap = CacheContext.Deserialize<ScenarioLightmap>(cacheStream, scnr.Lightmap);

                for (int i = 0; i < scnr.StructureBsps.Count; i++)
                {
                    if (scnr.StructureBsps[i].StructureBsp == null)
                        continue;

                    var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(cacheStream, scnr.StructureBsps[i].StructureBsp);


                    // Reach doesn't have these blocks in sbsp anymore, move it back
                    sbsp.CameraFxPalette = scnr.CameraFx;
                    sbsp.AtmospherePalette = scnr.Atmosphere;
                    sbsp.AcousticsPalette = scnr.AcousticsPalette;


                    // Rebuild reach instanced geometry instance per pixel data
                    //
                    // Prior to reach the "LodDataIndex" was the index of the instanced geometry instance per pixel data.
                    // In reach this is the mesh index, the per pixel data is indexed by instance index instead, 
                    // with a -1 vertex buffer index for instances that do not have per pixel data

                    if (lightmap.PerPixelLightmapDataReferences[i].LightmapBspData != null)
                    {
                        var Lbsp = CacheContext.Deserialize<ScenarioLightmapBspData>(cacheStream, lightmap.PerPixelLightmapDataReferences[i].LightmapBspData);
                        var newPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();
                        for (int instanceIndex = 0; instanceIndex < sbsp.InstancedGeometryInstances.Count; instanceIndex++)
                        {
                            var lightingElement = Lbsp.Geometry.InstancedGeometryPerPixelLighting[instanceIndex];
                            if (lightingElement.VertexBufferIndex != -1)
                            {
                                sbsp.InstancedGeometryInstances[instanceIndex].LightmapTexcoordBlockIndex = (short)newPerPixelLighting.Count;
                                newPerPixelLighting.Add(lightingElement);
                            }
                            else
                            {
                                sbsp.InstancedGeometryInstances[instanceIndex].LightmapTexcoordBlockIndex = -1;
                            }
                        }
                        Lbsp.Geometry.InstancedGeometryPerPixelLighting = newPerPixelLighting;

                        // Fixup foliage material
                        foreach (var mesh in Lbsp.Geometry.Meshes)
                        {
                            foreach (var part in mesh.Parts)
                            {
                                if (part.MaterialIndex != -1 && 
                                    sbsp.Materials[part.MaterialIndex].RenderMethod != null &&
                                    sbsp.Materials[part.MaterialIndex].RenderMethod.Group.Tag == "rmfl")
                                {
                                    part.FlagsNew |= Part.PartFlagsNew.PreventBackfaceCulling;
                                }
                            }
                        }

                        CacheContext.Serialize(cacheStream, lightmap.PerPixelLightmapDataReferences[i].LightmapBspData, Lbsp);
                    }

                    // Fixup instance bsp physics
                    for (int instanceIndex = 0; instanceIndex < sbsp.InstancedGeometryInstances.Count; instanceIndex++)
                    {
                        var instance = sbsp.InstancedGeometryInstances[instanceIndex];
                        foreach(var bspPhysics in instance.BspPhysics)
                        {
                            bspPhysics.GeometryShape.BspIndex = (sbyte)i;
                            bspPhysics.GeometryShape.CollisionGeometryShapeKey = (ushort)instanceIndex;
                        }
                    }

                    CacheContext.Serialize(cacheStream, scnr.StructureBsps[i].StructureBsp, sbsp);
                }

                // Generate skya from fog parameters

                if (!CacheContext.TagCache.TryGetTag(tagName + ".skya", out scnr.SkyParameters))
                {
                    SkyAtmParameters skya = new SkyAtmParameters();
                    skya.AtmosphereSettings = new List<SkyAtmParameters.AtmosphereProperty>();
                    skya.UnderwaterSettings = new List<SkyAtmParameters.UnderwaterBlock>();

                    List<string> convertedWaterFog = new List<string>();

                    // Convert atmosphere globals

                    if (scnr.AtmosphereGlobals != null)
                    {
                        var atgf = BlamCache.Deserialize<AtmosphereGlobals>(blamCacheStream, scnr.AtmosphereGlobals);

                        skya.Flags = SkyAtmParameters.SkyAtmFlags.None;
                        skya.FogBitmap = atgf.FogBitmap != null ? (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, atgf.FogBitmap, null, atgf.FogBitmap.Name) : null;
                        skya.TextureRepeatRate = atgf.TextureRepeatRate;
                        skya.DistanceBetweenSheets = atgf.DistanceBetweenSheets;
                        skya.DepthFadeFactor = atgf.DepthFadeFactor;
                        skya.ClusterSearchRadius = 25.0f;
                        skya.TransparentSortDistance = atgf.TransparentSortDistance;
                        skya.TransparentSortLayer = atgf.TransparentSortLayer;

                        foreach (var underwaterSetting in atgf.UnderwaterSettings)
                        {
                            var unwt = new SkyAtmParameters.UnderwaterBlock
                            {
                                Name = (StringId)ConvertData(cacheStream, blamCacheStream, resourceStreams, underwaterSetting.Name, null, null),
                                Murkiness = underwaterSetting.Murkiness,
                                FogColor = underwaterSetting.FogColor
                            };

                            skya.UnderwaterSettings.Add(unwt);

                            convertedWaterFog.Add(CacheContext.StringTable.GetString(unwt.Name));
                        }
                    }

                    // Convert underwater fog

                    foreach (var sDesign in scnr.StructureDesigns)
                    {
                        if (sDesign.StructureDesign != null)
                        {
                            var blamSddt = BlamCache.Deserialize<StructureDesign>(blamCacheStream, BlamCache.TagCache.GetTag<StructureDesign>(sDesign.StructureDesign.Name));

                            foreach (var waterInstance in blamSddt.WaterInstances)
                            {
                                var waterNameId = (StringId)ConvertData(cacheStream, blamCacheStream, resourceStreams, blamSddt.WaterGroups[waterInstance.WaterNameIndex].Name, null, null);
                                var waterName = CacheContext.StringTable.GetString(waterNameId);

                                if (!convertedWaterFog.Contains(waterName))
                                {
                                    var unwt = new SkyAtmParameters.UnderwaterBlock
                                    {
                                        Name = waterNameId,
                                        Murkiness = waterInstance.FogMurkiness,
                                        FogColor = waterInstance.FogColor
                                    };

                                    skya.UnderwaterSettings.Add(unwt);

                                    convertedWaterFog.Add(waterName);
                                }
                            }
                        }
                    }

                    // Convert atmospheres

                    foreach (var atmospherePalette in scnr.Atmosphere)
                    {
                        while (atmospherePalette.AtmosphereSettingIndex >= skya.AtmosphereSettings.Count)
                            skya.AtmosphereSettings.Add(new SkyAtmParameters.AtmosphereProperty());

                        if (atmospherePalette.AtmosphereFog != null)
                        {
                            var fogg = BlamCache.Deserialize<AtmosphereFog>(blamCacheStream, atmospherePalette.AtmosphereFog);

                            var atmosphereSettings = skya.AtmosphereSettings[atmospherePalette.AtmosphereSettingIndex];

                            atmosphereSettings.Flags |= SkyAtmParameters.AtmosphereProperty.AtmosphereFlags.EnableAtmosphere;
                            atmosphereSettings.Name = (StringId)ConvertData(cacheStream, blamCacheStream, resourceStreams, atmospherePalette.Name, null, null);

                            // Patchy Fog

                            if (fogg.Flags.HasFlag(AtmosphereFog.AtmosphereFogFlags.PatchyFogEnabled))
                                atmosphereSettings.Flags |= SkyAtmParameters.AtmosphereProperty.AtmosphereFlags.PatchyFog;

                            atmosphereSettings.SheetDensity = fogg.PatchyFog.SheetDensity;
                            atmosphereSettings.FullIntensityHeight = fogg.PatchyFog.FullIntensityHeight;
                            atmosphereSettings.HalfIntensityHeight = fogg.PatchyFog.HalfIntensityHeight;
                            atmosphereSettings.WindDirection = fogg.PatchyFog.WindDirection;

                            if (fogg.WeatherEffect != null)
                                atmosphereSettings.WeatherEffect = (CachedTag)ConvertData(cacheStream, blamCacheStream, resourceStreams, fogg.WeatherEffect, null, null);

                            // Scattering
                            // TODO: proper conversion for fog.

                            AtmosphereFog.FogSettings fogSettings = null;

                            if (fogg.Flags.HasFlag(AtmosphereFog.AtmosphereFogFlags.GroundFogEnabled))
                                fogSettings = fogg.GroundFog;
                            else if (fogg.Flags.HasFlag(AtmosphereFog.AtmosphereFogFlags.SkyFogEnabled))
                                fogSettings = fogg.SkyFog;

                            if (fogSettings != null)
                            {
                                atmosphereSettings.Flags |= SkyAtmParameters.AtmosphereProperty.AtmosphereFlags.OverrideRealSunValues;
                                atmosphereSettings.Color = fogSettings.FogColor;
                                atmosphereSettings.Intensity = 3.0f; // tweak?
                                atmosphereSettings.SunAnglePitch = 0.0f;
                                atmosphereSettings.SunAngleYaw = 0.0f;

                                // Test for direction
                                //if (scnr.SkyReferences.Count > 0 && scnr.SkyReferences[0].SkyObject != null)
                                //{
                                //    var skyObje = CacheContext.Deserialize<GameObject>(cacheStream, scnr.SkyReferences[0].SkyObject);
                                //    var hlmt = CacheContext.Deserialize<Model>(cacheStream, skyObje.Model);
                                //    var mode = CacheContext.Deserialize<RenderModel>(cacheStream, hlmt.RenderModel);
                                //
                                //    if (mode.LightgenLights.Count > 0)
                                //    {
                                //        var direction = mode.LightgenLights.Last().Direction;
                                //
                                //        atmosphereSettings.SunAnglePitch = (float)(Math.Acos(direction.K) / Math.PI) * 180.0f;
                                //        atmosphereSettings.SunAngleYaw = (float)(Math.Asin(direction.J / Math.Sin(atmosphereSettings.SunAnglePitch)) / Math.PI) * 180.0f;
                                //    }
                                //}

                                atmosphereSettings.SeaLevel = fogSettings.BaseHeight; // WU, lowest height of scenario
                                atmosphereSettings.RayleignHeightScale = fogSettings.FogHeight; // WU, height above sea where atmo 30% thick
                                atmosphereSettings.MieHeightScale = fogSettings.FogHeight; // WU, height above sea where atmo 30% thick

                                atmosphereSettings.MaxFogThickness = fogSettings.FogThickness * 10000.0f;
                            }

                            atmosphereSettings.RayleighMultiplier = 0.0f; // scattering amount, small
                            atmosphereSettings.MieMultiplier = 0.0f; // scattering amount, large

                            atmosphereSettings.SunPhaseFunction = 0.2f;
                            atmosphereSettings.Desaturation = 0.0f;
                            atmosphereSettings.DistanceBias = fogg.DistanceBias;

                            // placeholder for now

                            atmosphereSettings.BetaM = new RealVector3d(0.0002946603f, 0.0005024257f, 0.001058603f);
                            atmosphereSettings.BetaP = new RealVector3d(0.001434321f, 0.001849472f, 0.002627869f);
                            atmosphereSettings.BetaMThetaPrefix = new RealVector3d(1.788872E-05f, 3.050209E-05f, 6.426741E-05f);
                            atmosphereSettings.BetaPThetaPrefix = new RealVector3d(0.0003334733f, 0.0004336488f, 0.0006244543f);
                        }
                    }

                    CachedTag skyTag = CacheContext.TagCache.AllocateTag<SkyAtmParameters>(tagName);
                    CacheContext.Serialize(cacheStream, skyTag, skya);

                    scnr.SkyParameters = skyTag;
                }

                // set Game Object Reset Height

                scnr.SpawnData = new List<Scenario.SpawnDatum> { new Scenario.SpawnDatum { GameObjectResetHeight = -20f } };

                // gametype object processing

                if (scnr.MapType == ScenarioMapType.Multiplayer)
                    AddGametypeObjects(scnr);
            }

            //
            // Misc multiplayer fixups
            //

            if (scnr.PlayerStartingProfile == null || scnr.PlayerStartingProfile.Count == 0)
            {
                scnr.PlayerStartingProfile = new List<Scenario.PlayerStartingProfileBlock>() {
                    new Scenario.PlayerStartingProfileBlock() {
                        Name = "start_assault",
                        PrimaryWeapon = CacheContext.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\assault_rifle", "weap"),
                        PrimaryRoundsLoaded = 32,
                        PrimaryRoundsTotal = 108,
                        StartingFragGrenadeCount = 2
                    }
                };
            }
            else if (scnr.MapType == ScenarioMapType.Multiplayer)
            {
                if (string.IsNullOrEmpty(scnr.PlayerStartingProfile[0].Name))
                    scnr.PlayerStartingProfile[0].Name = "start_assault";

                if (scnr.PlayerStartingProfile[0].PrimaryWeapon == null)
                    scnr.PlayerStartingProfile[0].PrimaryWeapon = CacheContext.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\assault_rifle", "weap");
            }

            if (scnr.MapType == ScenarioMapType.Multiplayer && BlamCache.Version == CacheVersion.Halo3Retail)
            {
                var spawnpoint = -1;
                for (int i = 0; i < scnr.SceneryPalette.Count; i++)
                {
                    if (scnr.SceneryPalette[i].Object?.Name == @"objects\multi\spawning\respawn_point")
                    {
                        spawnpoint = i;
                        break;
                    }
                }

                if (spawnpoint != -1)
                {
                    for (int i = 0; i < scnr.Scenery.Count; i++)
                    {
                        if (scnr.Scenery[i].PaletteIndex == spawnpoint)
                            scnr.Scenery[i].Multiplayer.Team = MultiplayerTeamDesignator.Neutral;
                    }
                }
            }

            return scnr;
        }

        private Scenario.TriggerVolume ConvertTriggerVolumeReach(Scenario.TriggerVolume volume)
        {
            RealVector3d ProjectPointOnPlane(RealPlane3d plane, RealVector3d point)
            {
                var o = new RealVector3d(plane.I * plane.D, plane.J * plane.D, plane.K * plane.D);
                var v = point - o;
                var n = plane.Normal;
                float dist = (n.I * v.I) + (n.J * v.J) + (n.K * v.K);
                return point - dist * n;
            }


            if (volume.Type == Scenario.TriggerVolumeType.Sector)
            {
                foreach (var triangle in volume.RuntimeTriangles)
                {
                    triangle.BoundsX0 = triangle.BoundsY0 = triangle.BoundsZ0 = float.MaxValue;
                    triangle.BoundsX1 = triangle.BoundsY1 = triangle.BoundsZ1 = -float.MaxValue;

                    var points = new[] { triangle.Vertex0, triangle.Vertex1, triangle.Vertex2 };
                    for (int i = 0; i < points.Length; i++)
                    {
                        var proj = ProjectPointOnPlane(triangle.Plane0, new RealVector3d(points[i].X, points[i].Y, 0));
                        triangle.BoundsX0 = Math.Min(triangle.BoundsX0, proj.I);
                        triangle.BoundsX1 = Math.Max(triangle.BoundsX1, proj.I);
                        triangle.BoundsY0 = Math.Min(triangle.BoundsY0, proj.J);
                        triangle.BoundsY1 = Math.Max(triangle.BoundsY1, proj.J);
                        triangle.BoundsZ0 = Math.Min(triangle.BoundsZ0, proj.K);
                        triangle.BoundsZ1 = Math.Max(triangle.BoundsZ1, proj.K);
                    }

                    triangle.Plane1.Normal *= -1;
                    triangle.Plane1.D *= -1;
                }
            }

            return volume;
        }

        public void AddGametypeObjects(Scenario scnr)
        {
            scnr.SceneryPalette = scnr.SceneryPalette ?? new List<Scenario.ScenarioPaletteEntry>();
            scnr.CratePalette = scnr.CratePalette ?? new List<Scenario.ScenarioPaletteEntry>();

            if (scnr.CratePalette.Count > 0)
            {
                scnr.CratePalette.AddRange(new List<Scenario.ScenarioPaletteEntry>
                    {
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\assault\assault_bomb_goal_area", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\assault\assault_bomb_spawn_point", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\ctf\ctf_flag_spawn_point", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\infection\infection_haven_static", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\juggernaut\juggernaut_destination_static", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\vip\vip_destination_static", "bloc") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\territories\territory_static", "bloc") }
                    });

                ProcessMegaloLabels(scnr.CratePalette, scnr.Crates);

                // Teleporters must be neutral.

                for (int i = 0; i < scnr.CratePalette.Count(); i++)
                {
                    var obj = scnr.CratePalette[i].Object;
                    if (obj != null && obj.Name.Contains("teleporter"))
                    {
                        foreach (var instance in scnr.Crates.Where(n => n.PaletteIndex == i))
                            instance.Multiplayer.Team = MultiplayerTeamDesignator.Neutral;
                    }
                }

                // Reach uses a unified CTF spawn and return object. A duplicate of these instances will be used for flag spawn locations

                short flagSpawnIndex = GetPaletteIndex(scnr.CratePalette, @"objects\multi\ctf\ctf_flag_return_area");
                List<Scenario.CrateInstance> flagSpawns = new List<Scenario.CrateInstance>();
                
                foreach (var bloc in scnr.Crates.Where(n => n.PaletteIndex == flagSpawnIndex))
                    flagSpawns.Add(bloc.DeepClone());

                foreach (var flagSpawn in flagSpawns)
                {
                    flagSpawn.PaletteIndex = GetPaletteIndex(scnr.CratePalette, @"objects\multi\ctf\ctf_flag_spawn_point");
                    flagSpawn.NameIndex = -1;
                }

                scnr.Crates.AddRange(flagSpawns);
            }

            if (scnr.SceneryPalette.Count > 0)
            {
                scnr.SceneryPalette.AddRange(new List<Scenario.ScenarioPaletteEntry>
                    {
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\assault\assault_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\ctf\ctf_flag_at_home_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\ctf\ctf_flag_away_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\ctf\ctf_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\infection\infection_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\koth\koth_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\oddball\oddball_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\territories\territories_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\vip\vip_respawn_zone", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\assault\assault_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\ctf\ctf_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\infection\infection_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\koth\koth_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\oddball\oddball_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\territories\territories_initial_spawn_point", "scen") },
                        new Scenario.ScenarioPaletteEntry { Object = CacheContext.TagCache.GetTag(@"objects\multi\vip\vip_initial_spawn_point", "scen") }
                    });

                ProcessMegaloLabels(scnr.SceneryPalette, scnr.Scenery);
            }
        }

        private void ProcessMegaloLabels<T>(List<Scenario.ScenarioPaletteEntry> palette, List<T> instanceList)
        {
            foreach (var instance in instanceList)
            {
                var mpProperties = (Scenario.MultiplayerObjectProperties)(instance.GetType().GetField("Multiplayer").GetValue(instance));
                var permutationInstance = (instance as Scenario.PermutationInstance);
                var newPaletteIndex = permutationInstance.PaletteIndex;
                var ctfReturnIndex = GetPaletteIndex(palette, @"objects\multi\ctf\ctf_flag_return_area");
                switch (mpProperties.MegaloLabel)
                {
                    case "ctf_res_zone_away":
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\ctf\ctf_flag_away_respawn_zone") : -1);
                        break;
                    case "ctf_res_zone":
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\ctf\ctf_flag_at_home_respawn_zone") : -1);
                        break;
                    case "ctf_flag_return":
                        {
                            newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\ctf\ctf_flag_return_area") : -1);
                            //if (mpProperties.Team == MultiplayerTeamDesignator.Neutral)
                            //    newPaletteIndex = -1;
                        }
                        break;
                    case "terr_object":
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\territories\territory_static");
                        break;
                    case "as_goal": // assault plant point
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\assault\assault_bomb_goal_area") : -1);
                        break;
                    case "as_bomb": // assault bomb spawn
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\assault\assault_bomb_spawn_point") : -1);
                        break;
                    case "stp_goal": // substitute stockpile goal for juggernaut destination
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\juggernaut\juggernaut_destination_static");
                        break;
                    case "stp_flag": // substitute stockpile flag spawn for VIP destination
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\vip\vip_destination_static");
                        break;
                    case "assault":
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\assault\assault_respawn_zone") : -1);
                        break;
                    case "inf_spawn":
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\infection\infection_initial_spawn_point");
                        break;
                    case "ffa_only":
                        newPaletteIndex = -1;
                        break;
                    case "inf_haven":
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\infection\infection_respawn_zone");
                        break;
                    case "stockpile":
                        newPaletteIndex = GetPaletteIndex(palette, @"objects\multi\vip\vip_initial_spawn_point");
                        break;
                    case "ctf":
                        newPaletteIndex = (short)(CheckTeamValue(permutationInstance) ? GetPaletteIndex(palette, @"objects\multi\ctf\ctf_initial_spawn_point") : -1);
                        break;
                    case "oddball_ball":
                    case "koth_hill":
                    case "team_only":
                    case "hh_drop_point":
                    case "none":
                        break;
                    case "inv_objective":
                    case "inv_obj_flag":
                    case "invasion":
                        newPaletteIndex = -1;
                        break;
                    default:
                        if (!string.IsNullOrEmpty(mpProperties.MegaloLabel))
                            new TagToolWarning($"unknown megalo label: {mpProperties.MegaloLabel}");
                        break;
                }

                permutationInstance.PaletteIndex = newPaletteIndex;
            }
        }

        public short GetPaletteIndex(List<Scenario.ScenarioPaletteEntry> palette, string name)
        {
            var itemIndex = palette.FindIndex(x => (x.Object != null && x.Object.Name == name));

            return (short)itemIndex;
        }

        public bool CheckTeamValue(Scenario.PermutationInstance instance)
        {
            bool validforRvB = false;
            var validTeams = new List<string> { "Red", "Blue", "Neutral" };

            if (instance is Scenario.CrateInstance && validTeams.Contains((instance as Scenario.CrateInstance).Multiplayer.Team.ToString()))
                validforRvB = true;
            else if (instance is Scenario.SceneryInstance && validTeams.Contains((instance as Scenario.SceneryInstance).Multiplayer.Team.ToString()))
                validforRvB = true;

            return validforRvB;
        }

        private void AddPrematchCamera(Stream cacheStream, Scenario scnr, string tagName)
        {
            //
            // Add prematch camera position
            //

            var existingCameraPoint = scnr.CutsceneCameraPoints.FirstOrDefault(cameraPoint => cameraPoint.Name == "prematch_camera");
            if (existingCameraPoint != null)
            {
                // if we already have one, just add the flag for HO
                existingCameraPoint.Flags |= Scenario.CutsceneCameraPointFlags.PrematchCameraHack;
                return;
            }  

            var createPrematchCamera = false;

            var position = new RealPoint3d();
            var orientation = new RealEulerAngles3d();

            if (FlagIsSet(PortingFlags.Recursive))
            {
                switch (tagName)
                {
                    case @"levels\multi\construct\construct":
                        createPrematchCamera = true;
                        position = new RealPoint3d(4.5471f, 1.8711f, 13.4354f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-126.7653f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\multi\isolation\isolation":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-14.4359f, -10.9502f, -5.2309f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(30f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\multi\salvation\salvation":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-0.0762f, -0.1681f, 7.1527f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(90f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\multi\snowbound\snowbound":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-7.1015f, 17.7492f, 3.9918f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-90f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\armory\armory":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-16.8807f, 0.0363f, -8.6872f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\chillout\chillout":
                        createPrematchCamera = true;
                        position = new RealPoint3d(0.7120f, -10.8107f, 5.3540f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(90f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\descent\descent":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-19.9727f, -0.0140f, -17.3611f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\docks\docks":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-28.5603f, 22.1670f, -3.9043f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\fortress\fortress":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-33.9909f, 3.4858f, -18.9907f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\ghosttown\ghosttown":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-10.6792f, 10.8319f, 5.6487f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\lockout\lockout":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-19.9729f, 0.4024f, -5.3355f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\midship\midship":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-5.7814f, 4.7866f, 4.5577f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\sidewinder\sidewinder":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-35.8092f, 42.7776f, 2.6463f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\spacecamp\spacecamp":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-8.7606f, 17.2195f, -0.3308f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\warehouse\warehouse":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-11.2818f, 0.2725f, 4.1475f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\dlc\sandbox\sandbox":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-24.9556f, -9.8958f, -17.2465f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(0f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\multi\guardian\guardian":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-3.856011f, -1.605904f, 22.34261f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-58.089f), Angle.FromDegrees(-6.839594f), Angle.FromDegrees(10.82678f));
                        break;

                    case @"levels\multi\riverworld\riverworld":
                        createPrematchCamera = true;
                        position = new RealPoint3d(80f, -115f, 8f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-72f), Angle.FromDegrees(0f), Angle.FromDegrees(0f));
                        break;

                    case @"levels\multi\s3d_avalanche\s3d_avalanche":
                        createPrematchCamera = true;
                        position = new RealPoint3d(39.68156f, 52.96737f, 13.24531f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-101.3976f), Angle.FromDegrees(1.840378f), Angle.FromDegrees(9.051623f));
                        break;

                    case @"levels\multi\s3d_turf\s3d_turf":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-11.1375f, 10.65022f, 3.68083f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-1.106702f), Angle.FromDegrees(-6.048638f), Angle.FromDegrees(0.1166338f));
                        break;

                    case @"levels\multi\cyberdyne\cyberdyne":
                        createPrematchCamera = true;
                        position = new RealPoint3d(16.48399f, -0.2954462f, 5.926272f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(148.4995f), Angle.FromDegrees(10.94987f), Angle.FromDegrees(-6.639596f));
                        break;

                    case @"levels\multi\chill\chill":
                        createPrematchCamera = true;
                        position = new RealPoint3d(0.1023328f, 13.20142f, 67.24016f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-91.86741f), Angle.FromDegrees(0.5627626f), Angle.FromDegrees(16.76527f));
                        break;

                    case @"levels\dlc\bunkerworld\bunkerworld":
                        createPrematchCamera = true;
                        position = new RealPoint3d(1.919771f, 39.41721f, 14.75777f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-74.90959f), Angle.FromDegrees(-0.6069012f), Angle.FromDegrees(2.249499f));
                        break;

                    case @"levels\multi\zanzibar\zanzibar":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-0.5595548f, 8.776897f, 12.80816f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-70.32931f), Angle.FromDegrees(-4.318761f), Angle.FromDegrees(11.89593f));
                        break;

                    case @"levels\multi\deadlock\deadlock":
                        createPrematchCamera = true;
                        position = new RealPoint3d(-7.903993f, -4.081663f, 17.2834f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(74.76313f), Angle.FromDegrees(-4.653013f), Angle.FromDegrees(-16.58442f));
                        break;

                    case @"levels\multi\shrine\shrine":
                        createPrematchCamera = true;
                        position = new RealPoint3d(31.19498f, 20.94002f, -6.859918f);
                        orientation = new RealEulerAngles3d(Angle.FromDegrees(-137.8311f), Angle.FromDegrees(16.69542f), Angle.FromDegrees(15.16735f));
                        break;

                    default:
                        if (scnr.MapType == ScenarioMapType.Multiplayer)
                        {
                            var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(cacheStream, scnr.StructureBsps[0].StructureBsp);

                            createPrematchCamera = true;
                            position = new RealPoint3d(
                                (sbsp.WorldBoundsX.Lower + sbsp.WorldBoundsX.Upper) / 2.0f,
                                (sbsp.WorldBoundsY.Lower + sbsp.WorldBoundsY.Upper) / 2.0f,
                                (sbsp.WorldBoundsZ.Lower + sbsp.WorldBoundsZ.Upper) / 2.0f);
                            orientation = new RealEulerAngles3d(scnr.LocalNorth, Angle.FromDegrees(0.0f), Angle.FromDegrees(0.0f));
                        }
                        break;
                }
            }

            if (createPrematchCamera)
                scnr.CutsceneCameraPoints = new List<Scenario.CutsceneCameraPoint>() { MultiplayerPrematchCamera(position, orientation) };
        }

        /// <summary>
        /// Given the position and the yaw/pitch given by the camera coordinates, create a CutsceneCameraPoint block. Note that roll is always 0 in the coordinates.
        /// </summary>
        public Scenario.CutsceneCameraPoint MultiplayerPrematchCamera(RealPoint3d position, RealEulerAngles3d orientation)
        {
            var camera = new Scenario.CutsceneCameraPoint()
            {
                Flags = Scenario.CutsceneCameraPointFlags.PrematchCameraHack,
                Type = Scenario.CutsceneCameraPointType.Normal,
                Name = "prematch_camera",
                Position = position,
                Orientation = orientation

            };
            return camera;
        }

        /// <summary>
        /// Convert from the Tait-Bryan XYZ to ZYX coordinate, assuming input Z = 0.
        /// </summary>
        public RealEulerAngles3d ConvertXYZtoZYX(RealEulerAngles3d angle)
        {
            var x1 = angle.Yaw.Radians;
            var y1 = angle.Pitch.Radians;

            var y2 = (float)Math.Asin(Math.Cos(x1) * Math.Sin(y1));
            var z2 = (float)Math.Acos(Math.Cos(y1) / Math.Cos(y2));
            var x2 = x1;

            return new RealEulerAngles3d(Angle.FromRadians(x2), Angle.FromRadians(y2), Angle.FromRadians(z2));
        }

        public void ConvertScriptExpression(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, Scenario scnr, HsSyntaxNode expr)
        {
            if (expr.Opcode == 0xBABA)
                return;

            ConvertHsType(expr.ValueType);

            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Expression:
                case HsSyntaxNodeFlags.Group:
                case HsSyntaxNodeFlags.GlobalsReference:
                case HsSyntaxNodeFlags.ParameterReference:
                    if (ScriptExpressionIsValue(expr))
                        ConvertScriptValueOpcode(expr);
                    else if (!ConvertScriptUsingPresets(cacheStream, scnr, expr))
                        ConvertScriptExpressionOpcode(scnr, expr);
                    break;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                    break;

                default:
                    break;
            }

            ConvertScriptExpressionData(cacheStream, blamCacheStream, resourceStreams, expr);
        }

        public bool ScriptExpressionIsValue(HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.ParameterReference:
                case HsSyntaxNodeFlags.GlobalsReference:
                    return true;

                case HsSyntaxNodeFlags.Expression:
                    if ((int)expr.ValueType.HaloOnline > 0x4)
                        return true;
                    else
                        return false;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses, so ignore
                case HsSyntaxNodeFlags.Group:
                    return false;

                default:
                    return false;
            }
        }

        public void ConvertHsType(HsType type)
        {
            if (!Enum.TryParse(
                BlamCache.Version == CacheVersion.Halo3Retail ?
                    type.Halo3Retail.ToString() :
                    type.Halo3ODST.ToString(),
                out type.HaloOnline))
            {
                throw new NotImplementedException(
                    BlamCache.Version == CacheVersion.Halo3Retail ?
                        type.Halo3Retail.ToString() :
                        type.Halo3ODST.ToString());
            }

            if ((int)type.HaloOnline == 255)
                type.HaloOnline = HsType.HaloOnlineValue.Invalid;
        }

        public void ConvertScriptValueOpcode(HsSyntaxNode expr)
        {
            if (expr.Opcode == 0xFFFF || expr.Opcode == 0xBABA || expr.Opcode == 0x0000)
                return;

            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Expression:
                case HsSyntaxNodeFlags.Group:
                case HsSyntaxNodeFlags.GlobalsReference:
                case HsSyntaxNodeFlags.ParameterReference:
                    break;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                    return;

                default:
                    return;
            }

            // Some script expressions use opcode as a script reference. Only continue if it is a reference
            if (!ScriptInfo.ValueTypes[(BlamCache.Version, BlamCache.Platform)].ContainsKey(expr.Opcode))
            {
                new TagToolError(CommandError.CustomError, $"not in {BlamCache.Version} opcode table 0x{expr.Opcode:X3}.");
                return;
            }

            var blamValueTypeName = ScriptInfo.ValueTypes[(BlamCache.Version, BlamCache.Platform)][expr.Opcode];

            foreach (var valueType in ScriptInfo.ValueTypes[(CacheContext.Version, CacheContext.Platform)])
            {
                if (valueType.Value == blamValueTypeName)
                {
                    expr.Opcode = (ushort)valueType.Key;
                    break;
                }
            }
        }

        public void ConvertScriptExpressionUnsupportedOpcode(HsSyntaxNode expr)
        {
            if (expr.Opcode == 0xBABA || expr.Opcode == 0xCDCD)
                return;

            expr.Opcode = 0x000; // begin
        }

        public void ConvertScriptExpressionOpcode(Scenario scnr, HsSyntaxNode expr)
        {
            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Expression:
                    if (expr.Flags == HsSyntaxNodeFlags.ScriptReference)
                        return;

                    // If the previous scriptExpr is a scriptRef, don't convert. The opcode is the script reference. They always come in pairs.
                    if (scnr.ScriptExpressions[scnr.ScriptExpressions.IndexOf(expr) - 1].Flags == HsSyntaxNodeFlags.ScriptReference)
                        return;

                    break;

                case HsSyntaxNodeFlags.Group:
                    break;

                case HsSyntaxNodeFlags.ScriptReference: // The opcode is the tagblock index of the script it uses. Don't convert opcode
                case HsSyntaxNodeFlags.GlobalsReference: // The opcode is the tagblock index of the global it uses. Don't convert opcode
                case HsSyntaxNodeFlags.ParameterReference: // Probably as above
                    return;

                default:
                    return;
            }

            if (!ScriptInfo.Scripts[(BlamCache.Version, BlamCache.Platform)].ContainsKey(expr.Opcode))
            {
                new TagToolError(CommandError.CustomError, $"not in {BlamCache.Version} opcode table: 0x{expr.Opcode:X3}. (ConvertScriptExpressionOpcode)");
                return;
            }

            var blamScript = ScriptInfo.Scripts[(BlamCache.Version, BlamCache.Platform)][expr.Opcode];

            bool match;

            foreach (var entry in ScriptInfo.Scripts[(CacheContext.Version, CacheContext.Platform)])
            {
                var newOpcode = entry.Key;
                var newScript = entry.Value;

                if (newScript.Name != blamScript.Name)
                    continue;

                if (newScript.Parameters.Count != blamScript.Parameters.Count)
                    continue;

                match = true;

                for (var k = 0; k < newScript.Parameters.Count; k++)
                {
                    if (newScript.Parameters[k].Type != blamScript.Parameters[k].Type)
                    {
                        match = false;
                        break;
                    }
                }

                if (match)
                {
                    expr.Opcode = (ushort)newOpcode;
                    return;
                }
            }

            //
            // If no match was found, the opcode is currently unsupported.
            //

            new TagToolWarning($"No equivalent script op was found for '{ScriptInfo.Scripts[(BlamCache.Version, BlamCache.Platform)][expr.Opcode].Name}' (0x{expr.Opcode:X3}, expr {scnr.ScriptExpressions.IndexOf(expr)})");

            ConvertScriptExpressionUnsupportedOpcode(expr);
        }

        public void ConvertScriptExpressionData(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, HsSyntaxNode expr)
        {
            if (expr.Flags == HsSyntaxNodeFlags.Expression)
                switch (expr.ValueType.HaloOnline)
                {
                    case HsType.HaloOnlineValue.Sound:
                    case HsType.HaloOnlineValue.Effect:
                    case HsType.HaloOnlineValue.Damage:
                    case HsType.HaloOnlineValue.LoopingSound:
                    case HsType.HaloOnlineValue.AnimationGraph:
                    case HsType.HaloOnlineValue.DamageEffect:
                    case HsType.HaloOnlineValue.ObjectDefinition:
                    case HsType.HaloOnlineValue.Bitmap:
                    case HsType.HaloOnlineValue.Shader:
                    case HsType.HaloOnlineValue.RenderModel:
                    case HsType.HaloOnlineValue.StructureDefinition:
                    case HsType.HaloOnlineValue.LightmapDefinition:
                    case HsType.HaloOnlineValue.CinematicDefinition:
                    case HsType.HaloOnlineValue.CinematicSceneDefinition:
                    case HsType.HaloOnlineValue.BinkDefinition:
                    case HsType.HaloOnlineValue.AnyTag:
                    case HsType.HaloOnlineValue.AnyTagNotResolving:
                        ConvertScriptTagReferenceExpressionData(cacheStream, blamCacheStream, resourceStreams, expr);
                        return;

                    case HsType.HaloOnlineValue.AiLine when BitConverter.ToInt32(expr.Data, 0) != -1:
                    case HsType.HaloOnlineValue.StringId:
                        ConvertScriptStringIdExpressionData(cacheStream, blamCacheStream, resourceStreams, expr);
                        return;

                    default:
                        break;
                }

            var dataSize = 4;

            switch (expr.Flags)
            {
                case HsSyntaxNodeFlags.Expression:
                    switch (expr.ValueType.HaloOnline)
                    {
                        case HsType.HaloOnlineValue.Object:
                        case HsType.HaloOnlineValue.Unit:
                        case HsType.HaloOnlineValue.Vehicle:
                        case HsType.HaloOnlineValue.Weapon:
                        case HsType.HaloOnlineValue.Device:
                        case HsType.HaloOnlineValue.Scenery:
                        case HsType.HaloOnlineValue.EffectScenery:
                            dataSize = 2; // definitely not 4
                            break;

                        case HsType.HaloOnlineValue.Ai: // int
                            break;

                        default:
                            dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                            break;
                    }
                    break;

                case HsSyntaxNodeFlags.GlobalsReference:
                    if (BlamCache.Version == CacheVersion.Halo3Retail)
                    {
                        dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                    }
                    else if (BlamCache.Version == CacheVersion.Halo3ODST)
                    {
                        switch (expr.ValueType.HaloOnline)
                        {
                            case HsType.HaloOnlineValue.Long:
                                dataSize = 2; // definitely not 4
                                break;
                            default:
                                dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                                break;
                        }
                    }
                    break;

                default:
                    dataSize = ScriptInfo.GetScriptExpressionDataLength(expr);
                    break;
            }

#if DEBUG
            if (expr.Flags == HsSyntaxNodeFlags.Expression && BitConverter.ToInt32(expr.Data, 0) != -1)
            {
                //
                // Breakpoint any case statement below to examine different types of "object" expression data
                //

                switch (expr.ValueType.HaloOnline)
                {
                    case HsType.HaloOnlineValue.Object: break;
                    case HsType.HaloOnlineValue.Unit: break;
                    case HsType.HaloOnlineValue.Vehicle: break;
                    case HsType.HaloOnlineValue.Weapon: break;
                    case HsType.HaloOnlineValue.Device: break;
                    case HsType.HaloOnlineValue.Scenery: break;
                    case HsType.HaloOnlineValue.EffectScenery: break;
                }
            }
#endif

            if(!CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform))
                Array.Reverse(expr.Data, 0, dataSize);

            if (expr.Flags == HsSyntaxNodeFlags.GlobalsReference)
            {
                if (expr.Data[2] == 0xFF && expr.Data[3] == 0xFF)
                {
                    var opcode = BitConverter.ToUInt16(expr.Data, 0) & ~0x8000;
                    var name = ScriptInfo.Globals[(BlamCache.Version, BlamCache.Platform)][opcode];
                    opcode = ScriptInfo.Globals[(CacheContext.Version, CacheContext.Platform)].First(p => p.Value == name).Key | 0x8000;
                    var bytes = BitConverter.GetBytes(opcode);
                    expr.Data[0] = bytes[0];
                    expr.Data[1] = bytes[1];
                }
            }
            else if (expr.Flags == HsSyntaxNodeFlags.Expression && expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Ai)
            {
                var value = BitConverter.ToUInt32(expr.Data, 0);

                if (value != uint.MaxValue && BlamCache.Version == CacheVersion.Halo3Retail)
                {
                    var aiValueType = (value >> 29) & 0x7;
                    var squadIndex = (value >> 16) & 0x1FFF;
                    var taskIndex = (value >> 16) & 0x1FFF;
                    var fireTeamIndex = (value >> 8) & 0xFF;
                    var spawnPointIndex = value & 0xFF;

                    switch (aiValueType)
                    {
                        case 1: // squad index
                        case 2: // squad group index
                        case 3: // actor datum_index
                            value = (uint)(((aiValueType & 0x7) << 29) | (value & 0xFFFF));
                            break;

                        case 4: // starting location
                            value = (uint)(((aiValueType & 0x7) << 29) | ((squadIndex & 0x1FFF) << 16) | (GetSpawnPointIndex(squadIndex, fireTeamIndex, spawnPointIndex) & 0xFF));
                            break;

                        case 5: // objective task
                            aiValueType++; // odst added spawn_formation
                            value = (uint)(((aiValueType & 0x7) << 29) | ((taskIndex & 0x1FFF) << 16) | (value & 0xFFFF));
                            break;

                        default:
                            throw new NotSupportedException($"0x{value:X8}");
                    }

                    expr.Data = BitConverter.GetBytes(value);
                }
            }
        }

        public uint GetSpawnPointIndex(uint squadIndex, uint fireTeamIndex, uint spawnPointIndex)
        {
            var prevSpawnPoints = 0;

            for (var i = 0; i < fireTeamIndex; i++)
                prevSpawnPoints += CurrentScenario.Squads[(int)squadIndex].Fireteams[i].StartingLocations.Count;

            return (uint)(prevSpawnPoints + spawnPointIndex);
        }

        public void ConvertScriptTagReferenceExpressionData(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, HsSyntaxNode expr)
        {
            if (!FlagIsSet(PortingFlags.Recursive))
                return;

            uint tagIndex;
            if (CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform))
                tagIndex = BitConverter.ToUInt32(expr.Data, 0);
            else
                tagIndex = BitConverter.ToUInt32(expr.Data.Reverse().ToArray(), 0);

            var tag = ConvertTag(cacheStream, blamCacheStream, resourceStreams, BlamCache.TagCache.GetTag(tagIndex));

            if (tag == null)
                return;

            expr.Data = BitConverter.GetBytes(tag?.Index ?? -1).ToArray();
        }

        public void ConvertScriptStringIdExpressionData(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, HsSyntaxNode expr)
        {
            uint blamStringId;
            if (CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform))
                blamStringId = BitConverter.ToUInt32(expr.Data, 0);
            else
                blamStringId =  BitConverter.ToUInt32(expr.Data.Reverse().ToArray(), 0);

            var value = BlamCache.StringTable.GetString(new StringId(blamStringId));

            if (value == null)
                return;

            if (!CacheContext.StringTable.Contains(value))
                ConvertStringId(new StringId(blamStringId));

            var edStringId = CacheContext.StringTable.GetStringId(value);
            expr.Data = BitConverter.GetBytes(edStringId.Value).ToArray();
        }

        public bool ConvertScriptUsingPresets(Stream cacheStream, Scenario scnr, HsSyntaxNode expr)
        {
            // Return false to convert normally.
            var blamScripts = ScriptInfo.Scripts[(BlamCache.Version, BlamCache.Platform)];
            if (BlamCache.Platform == CachePlatform.MCC)
            {
                var opName = blamScripts[expr.Opcode].Name;
                switch (opName)
                {
                    case "vehicle_test_seat_list":
                        expr.Opcode = 0x114;
                        if (expr.Flags == HsSyntaxNodeFlags.Group &&
                            expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Boolean)
                        {
                            UpdateAiTestSeat(cacheStream, scnr, expr);
                        }
                        return true;

                    case "vehicle_test_seat":
                        expr.Opcode = 0x115; // -> vehicle_test_seat_unit
                        if (expr.Flags == HsSyntaxNodeFlags.Group &&
                            expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Boolean)
                        {
                            UpdateAiTestSeat(cacheStream, scnr, expr);
                        }
                        return true;

                    case "campaign_metagame_award_primary_skull":
                        expr.Opcode = 0x1E5; // ^
                        return true;

                    case "campaign_metagame_award_secondary_skull":
                        expr.Opcode = 0x1E6; // ^
                        return true;

                    case "player_action_test_cinematic_skip":
                        expr.Opcode = 0x2F5; // player_action_test_jump
                        return true;

                    case "cinematic_object_get_unit":
                    case "cinematic_object_get_scenery":
                    case "cinematic_object_get_effect_scenery":
                        expr.Opcode = 0x391; // -> cinematic_object_get
                        return true;
                    case "cinematic_scripting_create_object":
                        expr.Opcode = 0x6A2;
                        return true;
                    case "cinematic_scripting_start_animation":
                        expr.Opcode = 0x6A1;
                        return true;
                    case "cinematic_scripting_destroy_object":
                        expr.Opcode = 0x6A6;
                        return true;
                    case "cinematic_scripting_create_and_animate_object":
                        expr.Opcode = 0x6A3;
                        return true;
                    case "cinematic_scripting_create_and_animate_cinematic_object":
                        expr.Opcode = 0x6A5;
                        return true;
                    case "cinematic_scripting_create_and_animate_object_no_animation":
                        expr.Opcode = 0x6A4;
                        return true;
                    case "chud_show_weapon_stats":
                        expr.Opcode = 0x423; // -> chud_show_crosshair
                        return true;
                    case "objectives_secondary_show":
                        expr.Opcode = 0x4AE; // -> objectives_show
                        return true;
                    case "objectives_secondary_unavailable":
                        expr.Opcode = 0x4B2; // -> objectives_show
                        return true;
                    case "mp_wake_script":
                        expr.Opcode = 0x6A7; // ^
                        UpdateMpWakeScript(cacheStream, scnr, expr);
                        return true;

                    default:
                        return false;
                }
            }        

            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                switch (expr.Opcode)
                {
                    case 0x0B3: // texture_camera_set_aspect_ratio
                        expr.Opcode = 0x0B9;
                        // Change the player appearance aspect ratio
                        if (scnr.MapId == 0x10231971 && // mainmenu map id
                            expr.Flags == HsSyntaxNodeFlags.Group &&
                            expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Void)
                        {
                            var exprIndex = scnr.ScriptExpressions.IndexOf(expr) + 1;
                            for (var n = 1; n < 2; n++)
                                exprIndex = scnr.ScriptExpressions[exprIndex].NextExpressionHandle.Index;

                            var expr2 = scnr.ScriptExpressions[exprIndex];
                            expr2.Data = BitConverter.GetBytes(1.777f).Reverse().ToArray();
                        }
                        return true;

                    case 0x0F9: // vehicle_test_seat_list
                        expr.Opcode = 0x114;
                        if (expr.Flags == HsSyntaxNodeFlags.Group &&
                            expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Boolean)
                        {
                            UpdateAiTestSeat(cacheStream, scnr, expr);
                        }
                        return true;

                    case 0x0FA: // vehicle_test_seat
                        expr.Opcode = 0x115; // -> vehicle_test_seat_unit
                        if (expr.Flags == HsSyntaxNodeFlags.Group &&
                            expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Boolean)
                        {
                            UpdateAiTestSeat(cacheStream, scnr, expr);
                        }
                        return true;

                    case 0x1B7: // campaign_metagame_award_primary_skull
                        expr.Opcode = 0x1E5; // ^
                        return true;

                    case 0x1B8: //campaign_metagame_award_secondary_skull
                        expr.Opcode = 0x1E6; // ^
                        return true;

                    case 0x2D2: // player_action_test_cinematic_skip
                        expr.Opcode = 0x2F5; // player_action_test_jump
                        return true;

                    case 0x33C: // cinematic_object_get_unit
                    case 0x33D: // cinematic_object_get_scenery
                    case 0x33E: // cinematic_object_get_effect_scenery
                        expr.Opcode = 0x391; // -> cinematic_object_get
                        return true;

                    //case 0x34D: // cinematic_scripting_destroy_object; remove last argument
                    //    expr.Opcode = 0x3A0;
                    //    return true;
                    //
                    //case 0x353: // cinematic_scripting_create_and_animate_cinematic_object
                    //    expr.Opcode = 0x3A6;
                    //    // Remove the additional H3 argument
                    //    if (expr.Flags == HsSyntaxNodeFlags.Group &&
                    //        expr.ValueType.HaloOnline == HsType.HaloOnlineValue.Void)
                    //    {
                    //        var exprIndex = scnr.ScriptExpressions.IndexOf(expr) + 1;
                    //        for (var n = 1; n < 4; n++)
                    //            exprIndex = scnr.ScriptExpressions[exprIndex].NextExpressionHandle.Index;
                    //
                    //        var expr2 = scnr.ScriptExpressions[exprIndex];
                    //        var expr3 = scnr.ScriptExpressions[expr2.NextExpressionHandle.Index];
                    //
                    //        expr2.NextExpressionHandle = expr3.NextExpressionHandle;
                    //    }
                    //    return true;
                    //
                    //case 0x354: //cinematic_scripting_create_and_animate_object_no_animation
                    //    expr.Opcode = 0x3A7; // ^
                    //    return true;

                    case 0x34A:
                        expr.Opcode = 0x6A2;
                        return true;
                    case 0x34C:
                        expr.Opcode = 0x6A1;
                        return true;
                    case 0x34D:
                        expr.Opcode = 0x6A6;
                        return true;
                    case 0x352:
                        expr.Opcode = 0x6A3;
                        return true;
                    case 0x353:
                        expr.Opcode = 0x6A5;
                        return true;
                    case 0x354:
                        expr.Opcode = 0x6A4;
                        return true;

                    case 0x3CD: // chud_show_weapon_stats
                        expr.Opcode = 0x423; // -> chud_show_crosshair
                        return true;

                    case 0x44D: // objectives_secondary_show
                        expr.Opcode = 0x4AE; // -> objectives_show
                        return true;

                    case 0x44F: // objectives_secondary_unavailable
                    case 0x44E: // objectives_primary_unavailable
                        expr.Opcode = 0x4B2; // -> objectives_show
                        return true;
                    case 0x118: // unit_add_equipment
                        expr.Opcode = 0x126; // ^
                        UpdateUnitAddEquipmentScript(cacheStream, scnr, expr);
                        return true;

                    default:
                        return false;
                }
            }
            else if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                switch (expr.Opcode)
                {
                    case 0x3A3: // cinematic_scripting_create_and_animate_cinematic_object
                        expr.Opcode = 0x3A6; // cinematic_scripting_create_and_animate_cinematic_object // example
                        return true;

                    default:
                        return false;
                }
            }
            else
                return false;
        }

        private void UpdateUnitAddEquipmentScript(Stream cacheStream, Scenario scnr, HsSyntaxNode expr)
        {
            if (BlamCache.Version == CacheVersion.Halo3Retail)
            {
                var exprIndex = scnr.ScriptExpressions.IndexOf(expr);
                var profileExpr = scnr.ScriptExpressions[exprIndex + 3]; // <StartingProfile> parameter

                if (profileExpr.StringAddress != 0)
                {
                    if (profileExpr.ValueType.Halo3Retail.ToString() != "StartingProfile")
                        return;

                    using (var scriptStringStream = new MemoryStream(scnr.ScriptStrings))
                    using (var scriptStringReader = new BinaryReader(scriptStringStream))
                    {
                        var profileName = "";
                        scriptStringReader.BaseStream.Position = profileExpr.StringAddress;
                        for (char c; (c = scriptStringReader.ReadChar()) != 0x00; profileName += c) ;

                        var startingProfileIndex = scnr.PlayerStartingProfile.FindIndex(sp => sp.Name == profileName);

                        if (startingProfileIndex == -1)
                        {
                            new TagToolWarning($"StartingProfile reference could not be converted {profileName}");
                            return;
                        }

                        profileExpr.ValueType.Halo3Retail = HsType.Halo3RetailValue.StartingProfile;
                        Array.Copy(BitConverter.GetBytes((short)startingProfileIndex), expr.Data, 2);
                        return;
                    }
                }
            }
        }

        private void UpdateMpWakeScript(Stream cacheStream, Scenario scnr, HsSyntaxNode expr)
        {
            var exprIndex = scnr.ScriptExpressions.IndexOf(expr) + 1;
            
            var stringExpr = scnr.ScriptExpressions[exprIndex]; // <string> parameter

            if(stringExpr.StringAddress != 0)
            {
                using (var scriptStringStream = new MemoryStream(scnr.ScriptStrings))
                using (var scriptStringReader = new BinaryReader(scriptStringStream))
                {
                    var scriptName = "";
                    scriptStringReader.BaseStream.Position = stringExpr.StringAddress;
                    for (char c; (c = scriptStringReader.ReadChar()) != 0x00; scriptName += c);

                    for(var i = 0; i < scnr.Scripts.Count; i++)
                    {
                        var script = scnr.Scripts[i];
                        if(scriptName == script.ScriptName)
                        {

                            stringExpr.ValueType.Halo3Retail = HsType.Halo3RetailValue.Script;
                            stringExpr.Data = BitConverter.GetBytes(i).ToArray();
                            return;
                        }
                    }
                }
            }
        }

        private void UpdateAiTestSeat(Stream cacheStream, Scenario scnr, HsSyntaxNode expr)
        {
            var exprIndex = scnr.ScriptExpressions.IndexOf(expr) + 1;
            for (var n = 1; n < 2; n++)
                exprIndex = scnr.ScriptExpressions[exprIndex].NextExpressionHandle.Index;

            var vehicleExpr = scnr.ScriptExpressions[exprIndex]; // <vehicle> parameter
            var seatMappingExpr = scnr.ScriptExpressions[vehicleExpr.NextExpressionHandle.Index]; // <string_id> parameter

            StringId seatMappingStringId;
            if(CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform))
                seatMappingStringId = new StringId(BitConverter.ToUInt32(seatMappingExpr.Data, 0));
            else
                seatMappingStringId = new StringId(BitConverter.ToUInt32(seatMappingExpr.Data.Reverse().ToArray(), 0));

            var seatMappingString = BlamCache.StringTable.GetString(seatMappingStringId);
            var seatMappingIndex = (int)-1;

            if (vehicleExpr.Flags == HsSyntaxNodeFlags.Group &&
                seatMappingStringId != StringId.Invalid)
            {
                if (vehicleExpr.Opcode == (BlamCache.Platform == CachePlatform.MCC ? 0x194 : 0x193)) // ai_vehicle_get_from_starting_location
                {
                    var expr3 = scnr.ScriptExpressions[++exprIndex]; // function name
                    var expr4 = scnr.ScriptExpressions[expr3.NextExpressionHandle.Index]; // <ai> parameter

                    uint value;
                    if(CacheVersionDetection.IsLittleEndian(BlamCache.Version, BlamCache.Platform))
                        value = BitConverter.ToUInt32(expr4.Data.ToArray(), 0);
                    else
                        value = BitConverter.ToUInt32(expr4.Data.Reverse().ToArray(), 0);

                    if (value != uint.MaxValue)
                    {
                        var squadIndex = (value >> 16) & 0x1FFF;
                        var fireTeamIndex = (value >> 8) & 0xFF;

                        var fireTeam = scnr.Squads[(int)squadIndex].Fireteams[(int)fireTeamIndex];

                        var unitInstance = scnr.VehiclePalette[fireTeam.VehicleTypeIndex].Object;

                        if(unitInstance.Index == -1)
                        {
                            new TagToolWarning($"Unit tag reference invalid in script in UpdateAiTestSeat! squads index {squadIndex} fireteam index {fireTeamIndex}");
                            return;
                        }

                        var unitDefinition = (Unit)CacheContext.Deserialize<Vehicle>(cacheStream, unitInstance);

                        var variantName = CacheContext.StringTable.GetString(unitDefinition.DefaultModelVariant);

                        if (fireTeam.VehicleVariant != StringId.Invalid)
                            variantName = CacheContext.StringTable.GetString(fireTeam.VehicleVariant);

                        if (unitDefinition.Model.Index == -1)
                        {
                            new TagToolWarning($"Unit model tag reference invalid in UpdateAiTestSeat! Unit {unitInstance.Name}");
                            return;
                        }

                        var modelDefinition = CacheContext.Deserialize<Model>(cacheStream, unitDefinition.Model);
                        var modelVariant = default(Model.Variant);

                        foreach (var variant in modelDefinition.Variants)
                        {
                            if (variantName == CacheContext.StringTable.GetString(variant.Name))
                            {
                                modelVariant = variant;
                                break;
                            }
                        }

                        var seats1 = (Scenario.UnitSeatFlags)(0);
                        var seats2 = (Scenario.UnitSeatFlags)(0);

                        for (var seatIndex = 0; seatIndex < unitDefinition.Seats.Count; seatIndex++)
                        {
                            var seat = unitDefinition.Seats[seatIndex];
                            var seatName = CacheContext.StringTable.GetString(seat.Label);

                            if (seatMappingString == seatName)
                            {
                                if (seatIndex < 32)
                                    seats1 = (Scenario.UnitSeatFlags)(1 << seatIndex);
                                else
                                    seats2 = (Scenario.UnitSeatFlags)(1 << (seatIndex - 32));
                                break;
                            }
                        }

                        if (seats1 == Scenario.UnitSeatFlags.None && seats2 == Scenario.UnitSeatFlags.None)
                        {
                            foreach (var obj in modelVariant.Objects)
                            {
                                if (obj.ChildObject == null || !obj.ChildObject.IsInGroup("unit"))
                                    continue;

                                var definition = (Unit)CacheContext.Deserialize(cacheStream, obj.ChildObject);

                                for (var seatIndex = 0; seatIndex < definition.Seats.Count; seatIndex++)
                                {
                                    var seat = definition.Seats[seatIndex];
                                    var seatName = CacheContext.StringTable.GetString(seat.Label);

                                    if (seatMappingString == seatName)
                                    {
                                        if (seatIndex < 32)
                                            seats1 = (Scenario.UnitSeatFlags)(1 << seatIndex);
                                        else
                                            seats2 = (Scenario.UnitSeatFlags)(1 << (seatIndex - 32));
                                        break;
                                    }
                                }

                                if (seats1 != Scenario.UnitSeatFlags.None || seats2 != Scenario.UnitSeatFlags.None)
                                {
                                    unitInstance = obj.ChildObject;
                                    unitDefinition = definition;
                                    break;
                                }
                            }
                        }

                        if (seats1 == Scenario.UnitSeatFlags.None && seats2 == Scenario.UnitSeatFlags.None)
                        {
                            for (var i = 0; i < unitDefinition.Seats.Count; i++)
                            {
                                if (i < 32)
                                    seats1 |= (Scenario.UnitSeatFlags)(1 << i);
                                else
                                    seats2 |= (Scenario.UnitSeatFlags)(1 << (i - 32));
                            }
                        }

                        for (var mappingIndex = 0; mappingIndex < scnr.UnitSeatsMapping.Count; mappingIndex++)
                        {
                            var mapping = scnr.UnitSeatsMapping[mappingIndex];

                            if (mapping.Unit == unitInstance && mapping.Seats1 == seats1 && mapping.Seats2 == seats2)
                            {
                                seatMappingIndex = mappingIndex;
                                break;
                            }
                        }

                        if (seatMappingIndex == -1)
                        {
                            seatMappingIndex = scnr.UnitSeatsMapping.Count;

                            scnr.UnitSeatsMapping.Add(new Scenario.UnitSeatsMappingBlock
                            {
                                Unit = unitInstance,
                                Seats1 = seats1,
                                Seats2 = seats2
                            });
                        }
                    }
                }
            }

            seatMappingExpr.Opcode = 0x00C; // -> unit_seat_mapping
            seatMappingExpr.ValueType.Halo3Retail = HsType.Halo3RetailValue.UnitSeatMapping;
            seatMappingExpr.Data = BitConverter.GetBytes((seatMappingIndex & ushort.MaxValue) | (1 << 16)).Reverse().ToArray();
            //all four bytes need to be FF for the argument to be "none"
            if (seatMappingStringId == StringId.Invalid)
                seatMappingExpr.Data = BitConverter.GetBytes(-1);
        }

        public void AdjustScripts(Scenario scnr, string tagName)
        {
            var mapName = tagName.Split("\\".ToCharArray()).Last();

            if (mapName == "mainmenu" && BlamCache.Version == CacheVersion.Halo3ODST)
                mapName = "mainmenu_odst";

            if (!DisabledScriptsString.ContainsKey(mapName))
                return;

            foreach (var line in DisabledScriptsString[mapName])
            {
                var items = line.Split(",".ToCharArray());

                int scriptIndex = Convert.ToInt32(items[0]);

                uint.TryParse(items[2], NumberStyles.HexNumber, null, out uint NextExpressionHandle);
                ushort.TryParse(items[3], NumberStyles.HexNumber, null, out ushort Opcode);
                byte.TryParse(items[4].Substring(0, 2), NumberStyles.HexNumber, null, out byte data0);
                byte.TryParse(items[4].Substring(2, 2), NumberStyles.HexNumber, null, out byte data1);
                byte.TryParse(items[4].Substring(4, 2), NumberStyles.HexNumber, null, out byte data2);
                byte.TryParse(items[4].Substring(6, 2), NumberStyles.HexNumber, null, out byte data3);

                scnr.ScriptExpressions[scriptIndex].NextExpressionHandle = new DatumHandle(NextExpressionHandle);
                scnr.ScriptExpressions[scriptIndex].Opcode = Opcode;
                scnr.ScriptExpressions[scriptIndex].Data[0] = data0;
                scnr.ScriptExpressions[scriptIndex].Data[1] = data1;
                scnr.ScriptExpressions[scriptIndex].Data[2] = data2;
                scnr.ScriptExpressions[scriptIndex].Data[3] = data3;
                scnr.ScriptExpressions[scriptIndex].Flags = (HsSyntaxNodeFlags)Enum.Parse(typeof(HsSyntaxNodeFlags), items[5]);
            }
        }

        private static Dictionary<string, List<string>> DisabledScriptsString = new Dictionary<string, List<string>>
        {
            // Format: Script expression tagblock index (dec), expression handle (salt + tagblock index), next expression handle (salt + tagblock index), opcode, data, 
            // expression type, value type, script expression name, original value, comment
            // Ideally this should use a dictionary with a list of script expressions per map name. I'm using a simple text format as this is how I dump scripts and modify them currently.

            ["mainmenu_odst"] = new List<string>
            {
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode, //default:E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,, //default:E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode, //default:E8A80535",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin, //default:E9790606",
            },

            ["sc110"] = new List<string>
            {
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E disable pda_breadcrumbs",
                "00018320,AB044790,AA6446F0,0000,00000000,Expression,FunctionName,begin,// AA4046CC disable waypoints temp",

                // autoremove:
                "00000892,E6EF037C,E6F50382,0016,00000000,Expression,FunctionName,// E6F0037D was pointing at E6F0037D",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,// E93405C1 was pointing at E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,// E95405E1 was pointing at E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,// E9FF068C was pointing at E9FF068C",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,// EA11069E was pointing at EA11069E",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,// EB1C07A9 was pointing at EB1C07A9",
                "00002164,EBE70874,EBFC0889,0000,00000000,Expression,FunctionName,// EBF70884 was pointing at EBF70884",
                "00005201,F7C41451,F7A81435,0000,00000000,Expression,FunctionName,// F7A2142F was pointing at F7A2142F",
                "00005173,F7A81435,FFFFFFFF,0014,3614A9F7,Group,Void,// F7BD144A was pointing at F7BD144A",
                "00018193,AA854711,AAA0472C,0627,124786AA,Group,Void,// AA9B4727 was pointing at AA9B4727",
                "00023201,BE155AA1,BE1B5AA7,0014,A25A16BE,Group,Void,// BE185AA4 was pointing at BE185AA4",
            },

            ["c200"] = new List<string>
            {
                "00000293,E4980125,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E4860113",
                "00000482,E55501E2,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E52F01BC",
                "00000532,E5870214,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E56201EF",
                "00000603,E5CE025B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5970224",
                "00000693,E62802B5,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E5E1026E",
                "00000802,E6950322,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E63E02CB",
                "00000852,E6C70354,E6D0035D,03F4,5503C8E6,Group,Void,sound_impulse_start,// E6CC0359",
                "00000884,E6E70374,FFFFFFFF,03F4,7503E8E6,Group,Void,sound_impulse_start,// E6EC0379",
                "00001043,E7860413,E79B0428,0112,140487E7,Group,Void,unit_enable_vision_mode,// E78B0418",
                "00001064,E79B0428,E7AD043A,0009,29049CE7,ScriptReference,Void,,// E79D042A",
                "00001328,E8A30530,E8B80545,0112,3105A4E8,Group,Void,unit_enable_vision_mode,// E8A80535",
                "00001572,E9970624,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// E9790606"
            },

            ["sc140"] = new List<string>
            {
                "00018790,ACDA4966,ACDB4967,0044,70010000,GlobalsReference,Vehicle,Value,//makes phantom fill up, allows level to finish",
                "00025501,C711639D,FFFFFFFF,0006,0000003C,Expression,Real,real,value," // convert default near_clip value (0.04 -> 0.0078125)
            },

            ["h100"] = new List<string>
            {
                "00016022,A20A3E96,A21F3EAB,0014,973E0BA2,Group,Void,sleep,", //get rid of f_l100_look_training
                "00017763,A8D74563,A94545D1,03EA,6445D8A8,Group,Void,game_save_cancel,", //get rid of all pda training
                "00016088,A24C3ED8,A2653EF1,0000,00000000,Expression,FunctionName,begin,", //get rid of health and vision training
            },

            ["l300"] = new List<string>
            {
                "00000891,E6EE037B,E6F50382,0016,7C03EFE6,Group,Void,sleep_until,// E6F60383",
                "00001060,E7970424,E7A50432,030F,250498E7,Group,Void,unit_action_test_reset,// E79C0429",
                "00001111,E7CA0457,E7D80465,030F,5804CBE7,Group,Void,unit_action_test_reset,// E7CF045C",
                "00001125,E7D80465,E7E0046D,0667,6604D9E7,Group,Void,chud_show_navpoint,// FFFFFFFF",
                "00001164,E7FF048C,E80D049A,030F,8D0400E8,Group,Void,unit_action_test_reset,// E8040491",
                "00001178,E80D049A,E81F04AC,0000,9B040EE8,Group,Void,begin,// FFFFFFFF",
                "00001238,E84904D6,E85704E4,030F,D7044AE8,Group,Void,unit_action_test_reset,// E84E04DB",
                "00001252,E85704E4,E87104FE,0000,E50458E8,Group,Void,begin,// FFFFFFFF",
                "00001331,E8A60533,E8B40541,030F,3405A7E8,Group,Void,unit_action_test_reset,// E8AB0538",
                "00001345,E8B40541,E8D60563,0000,4205B5E8,Group,Void,begin,// FFFFFFFF",
                "00001468,E92F05BC,E93805C5,03F4,BD0530E9,Group,Void,sound_impulse_start,// E93405C1",
                "00001500,E94F05DC,FFFFFFFF,03F4,DD0550E9,Group,Void,sound_impulse_start,// E95405E1",
                "00001659,E9EE067B,EA030690,0112,7C06EFE9,Group,Void,unit_enable_vision_mode,// E9F30680",
                "00001680,EA030690,EA1506A2,0012,910604EA,ScriptReference,Void,,// EA050692",
                "00001944,EB0B0798,EB2007AD,0112,99070CEB,Group,Void,unit_enable_vision_mode,// EB10079D",
                "00002188,EBFF088C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EBE1086E",
                "00005201,F7C41451,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// F7A2142F",
                "00020425,B33D4FC9,B3494FD5,0004,CA4F3EB3,Group,Void,set,// B3434FCF",
                "00020521,B39D5029,B3A75033,0169,2A509EB3,Group,Void,ai_cannot_die,// B3A1502D",
                "00020550,B3BA5046,B3C65052,0004,4750BBB3,Group,Void,set,// B3C0504C",
                "00020646,B41A50A6,B42450B0,0169,A7501BB4,Group,Void,ai_cannot_die,// B41E50AA",
                "00020675,B43750C3,B44350CF,0004,C45038B4,Group,Void,set,// B43D50C9",
                "00020771,B4975123,B4A1512D,0169,245198B4,Group,Void,ai_cannot_die,// B49B5127",
                "00020800,B4B45140,B4C0514C,0004,4151B5B4,Group,Void,set,// B4BA5146",
                "00020896,B51451A0,B51E51AA,0169,A15115B5,Group,Void,ai_cannot_die,// B51851A4",
                "00020925,B53151BD,B53D51C9,0004,BE5132B5,Group,Void,set,// B53751C3",
                "00021021,B591521D,B59B5227,0169,1E5292B5,Group,Void,ai_cannot_die,// B5955221",
                "00021050,B5AE523A,B5BA5246,0004,3B52AFB5,Group,Void,set,// B5B45240",
                "00021146,B60E529A,B61852A4,0169,9B520FB6,Group,Void,ai_cannot_die,// B612529E",
                "00021175,B62B52B7,B63752C3,0004,B8522CB6,Group,Void,set,// B63152BD",
                "00021271,B68B5317,B6955321,0169,18538CB6,Group,Void,ai_cannot_die,// B68F531B",
                "00021300,B6A85334,B6B45340,0004,3553A9B6,Group,Void,set,// B6AE533A",
                "00021396,B7085394,B712539E,0169,955309B7,Group,Void,ai_cannot_die,// B70C5398",
                "00021425,B72553B1,B73153BD,0004,B25326B7,Group,Void,set,// B72B53B7",
                "00021517,B781540D,B78B5417,0169,0E5482B7,Group,Void,ai_cannot_die,// B7855411",
                "00021546,B79E542A,B7AA5436,0004,2B549FB7,Group,Void,set,// B7A45430",
                "00021638,B7FA5486,B8045490,0169,8754FBB7,Group,Void,ai_cannot_die,// B7FE548A",
                "00021753,B86D54F9,B8775503,0169,FA546EB8,Group,Void,ai_cannot_die,// B87154FD",
                "00021795,B8975523,FFFFFFFF,0169,245598B8,Group,Void,ai_cannot_die,// B89B5527",
                "00022403,BAF75783,BB03578F,0004,8457F8BA,Group,Void,set,// BAFD5789",
                "00022523,BB6F57FB,BB795805,0169,FC5770BB,Group,Void,ai_cannot_die,// BB7357FF",
                "00022689,BC1558A1,BC1C58A8,017F,A25816BC,Group,Void,ai_allegiance,// BC1958A5",
                "00023456,BF145BA0,FFFFFFFF,01B1,A15B15BF,Group,Void,ai_set_objective,// BF185BA4",
                "00027973,D0B96D45,D0C36D4F,0169,466DBAD0,Group,Void,ai_cannot_die,// D0BD6D49",
                "00028614,D33A6FC6,D3486FD4,0016,C76F3BD3,Group,Void,sleep_until,// D3426FCE",
                "00028649,D35D6FE9,D3676FF3,0169,EA6F5ED3,Group,Void,ai_cannot_die,// D3616FED",
                "00028675,D3777003,D381700D,0192,047078D3,Group,Void,ai_force_active,// D37B7007",
                "00029030,D4DA7166,D4EF717B,0017,6771DBD4,Group,Void,wake,// D4DD7169",
                "00029086,D512719E,D52771B3,0166,9F7113D5,Group,Void,ai_place,// D51571A1",
                "00034658,EAD68762,EAEF877B,0006,6387D7EA,Group,Boolean,or,// EAE98775",
                "00034789,EB5987E5,EABD8749,0000,00000000,Expression,FunctionName,begin,// EABD8749 disable the whole thing for now",
                "00035083,EC7F890B,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EB5F87EB disable the whole thing for now",
                "00035212,ED00898C,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EC858911 disable the whole thing for now",
                "00035397,EDB98A45,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// ED068992 disable the whole thing for now",
                "00035767,EF2B8BB7,FFFFFFFF,0000,00000000,Expression,FunctionName,begin,// EF238BAF",
                "00035910,EFBA8C46,EFC08C4C,0333,478CBBEF,Group,Void,switch_zone_set,// EFBD8C49",
            },

            ["005_intro"] = new List<string>
            {
                "00002585,ED8C0A19,ED920A1F,0424,1A0A8DED,Group,Void,,chud_show_shield,ED8F0A1C",
                "00003019,EF3E0BCB,EF440BD1,0424,CC0B3FEF,Group,Void,chud_show_shield,", // to fix
                "00002221,EC2008AD,EC2F08BC,0053,AE0821EC,ScriptReference,Void,", // disable g_player_training as it freezes scripts
            },

            ["040_voi"] = new List<string>
            {         
                "00001611,E9BE064B,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
                "00009478,887A2506,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
                "00012297,937D3009,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,C:player_disable_movement",
            },

            ["070_waste"] = new List<string>
            {
                // Default script lines:
                "00001611,E9BE064B,FFFFFFFF,0005,01FFFFFF,Expression,Boolean,boolean,",
                "00009478,887A2506,887B2507,0013,3C000040,Expression,Ai,ai,",
                "00012297,937D3009,937E300A,0002,00000000,Expression,FunctionName,,if",
            },
        };
    }
}