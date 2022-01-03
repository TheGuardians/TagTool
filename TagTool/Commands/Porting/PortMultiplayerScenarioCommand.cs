using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using static TagTool.Commands.Porting.PortTagCommand;
using static TagTool.Tags.Definitions.Scenario;
using System.Collections;
using TagTool.Tags;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Commands.Porting
{
    class PortMultiplayerScenarioCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }
        private GameCache BlamCache { get; }

        [Flags]
        public enum MultiplayerScenarioConversionFlags
        {
            [Description("Preserve object placements")]
            Objects = (1 << 1),
            [Description("Preserve device objects (control, machine)")]
            DeviceObjects = (1 << 2),
			[Description("Keep biped palette and placements")]
			Bipeds = (1 << 3),
			[Description("Add a spawn point at (0,0,0)")]
            SpawnPoint = (1 << 5),
            [Description("Keep path finding data")]
            PathFinding = (1 << 6),

			Default = Objects | DeviceObjects | SpawnPoint
        }

        public PortMultiplayerScenarioCommand(GameCacheHaloOnlineBase cacheContext, GameCache blamCache, PortTagCommand portTag) :
            base(true,

                "PortMultiplayerScenario",
                "Builds a multiplayer map from one or more bsps within a given zone set",

                "PortMultiplayerScenario [Conversion Flags | Porting Flags]",

                BuildHelpText())
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
        }

        private static string BuildHelpText()
        {
            var buff = new StringBuilder();
            buff.AppendLine("Builds a multiplayer map from one or more bsps within a given zone set.");
            buff.AppendLine();
            buff.AppendLine("Prepending the flag with ! will disable it. e.g !Objects will ignore all object placements");
            buff.AppendLine("You may also use porting flags such as !Audio to not port sounds.");
            buff.AppendLine();
            buff.AppendLine($"Conversion Flags:");
            var type = typeof(MultiplayerScenarioConversionFlags);
            var names = Enum.GetNames(typeof(MultiplayerScenarioConversionFlags));
            foreach(var name in names)
            {
                if (name == "Default")
                    continue;

                var descriptionAttrib = type.GetField(name).GetCustomAttribute<DescriptionAttribute>();
                var helpText = descriptionAttrib?.Description ?? "";
                buff.AppendLine($"  {name,-20} - { helpText}");
            }
            return buff.ToString();
        }

        public override object Execute(List<string> args)
        {
            int zoneSetIndex = -1;
            BspFlags desiredBspMask = 0;
            string scenarioPath = null;
            int mapId = -1;

            var firstNonFlagArgumentIndex = ParseConversionFlags(args, out MultiplayerScenarioConversionFlags conversionFlags, out PortingFlags portingFlags);
            args = args.Skip(firstNonFlagArgumentIndex).ToList();

            CachedTag blamScnrTag = BlamCache.TagCache.NonNull().First(x => x.Group.Tag == "scnr");

            using (var blamStream = BlamCache.OpenCacheRead())
            using (var cacheStream = CacheContext.OpenCacheReadWrite())
            {
                var blamScnr = BlamCache.Deserialize<Scenario>(blamStream, blamScnrTag);

                Dictionary<string, int> structureBspsByName = new Dictionary<string, int>();
                Dictionary<string, int> zoneSetsByName = new Dictionary<string, int>();

                Console.WriteLine("Enter the scenario name:");
                var scenarioName = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables);
                if (!Regex.IsMatch(scenarioName, "[a-z0-9_]+"))
                    return new TagToolError(CommandError.CustomMessage, "Scenario name must consist of lowercase alphanumeric characters and underscores");

                scenarioPath = $@"levels\custom\{scenarioName}\{scenarioName}";

                //
                // try to parse the map id, if not use the randomly generated one unless it's actually invalid/out of range
                //

                const int kMinMapId = 7000;
                const int kMaxMapId = ushort.MaxValue - 1;

                // generate a map id
                mapId = new Random(Guid.NewGuid().GetHashCode()).Next(kMinMapId, kMaxMapId + 1);

                Console.WriteLine($"Enter map id in the range [{kMinMapId}, {kMaxMapId}] (or blank for {mapId}):");

                // try to parse one from input
                var tmpMapId = -1;
                var mapIdInput = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables);
                if (int.TryParse(mapIdInput, out tmpMapId))
                {
                    if (tmpMapId < kMinMapId || tmpMapId > kMaxMapId)
                        return new TagToolError(CommandError.CustomMessage, "Map ID out of range");
				}
                else
                {
                    tmpMapId = -1;
                }

                if (tmpMapId != -1)
                    mapId = tmpMapId;

                Console.WriteLine("Enter the map name (for display):");
                var mapName = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables);
                if (mapName.Length >= 4 && mapName.Length > 15)
                    return new TagToolError(CommandError.CustomMessage, "Map name must be at 4 to 15 characters");

                Console.WriteLine("Enter the map description:");
                var mapDescription = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables);
                if (mapDescription.Length > 127)
                    return new TagToolError(CommandError.CustomMessage, "Map description must be no longer than 127 characters");

                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < blamScnr.ZoneSets.Count; i++)
                {
                    // map the zone sets by name for easy lookup
                    zoneSetsByName.Add(BlamCache.StringTable.GetString(blamScnr.ZoneSets[i].Name), i);
                    Console.WriteLine($"{i}. {BlamCache.StringTable.GetString(blamScnr.ZoneSets[i].Name)}");
                }

                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter the name or index of the zone set to use:");

                // read the zone set name from input, trim it so we don't have to worry about spaces
                string zoneSetName = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables);
                // if they entered a valid name, use that index it is mapped to
                if (zoneSetsByName.ContainsKey(zoneSetName))
                {
                    zoneSetIndex = zoneSetsByName[zoneSetName];
                }
                else
                {
                    // otherwise try to parse the index
                    if (!int.TryParse(zoneSetName, out zoneSetIndex))
                        zoneSetIndex = -1;
                }

                if (zoneSetIndex == -1)
                    return new TagToolError(CommandError.CustomMessage, $"Zone set '{zoneSetName}' could not be found!\n");

                var zoneSet = blamScnr.ZoneSets[zoneSetIndex];

                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < 32; i++)
                {
                    if ((zoneSet.LoadedBsps & (BspFlags)(1u << i)) != 0)
                    {
                        // map the bsps by name for easy lookup
                        structureBspsByName.Add(blamScnr.StructureBsps[i].StructureBsp.Name, i);
                        Console.WriteLine($"{i}. {blamScnr.StructureBsps[i].StructureBsp.Name}");
                    }
                }
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter the name or index of each bsp to include on a new line followed by a blank line (leave blank for all):");

                //
                // keep reading lines from the console and parsing either a bsp name or index
                // setting the bit corresponding to the bsp index of the desired bsp mask
                //

                for (string line; !string.IsNullOrWhiteSpace(line = CommandRunner.ApplyUserVars(Console.ReadLine().Trim(), IgnoreArgumentVariables));)
                {
                    var sbspName = line.Trim();
                    int bspIndex = -1;

                    // if they entered a valid name, use that index it is mapped to
                    if (structureBspsByName.ContainsKey(sbspName))
                    {
                        bspIndex = structureBspsByName[sbspName];
                    }
                    else
                    {
                        // otherwise try to parse the index
                        if (!int.TryParse(sbspName, out bspIndex))
                            bspIndex = -1;
                    }

                    if (bspIndex == -1)
                        Console.WriteLine($"Could not find bsp '{sbspName}'");

                    desiredBspMask |= (BspFlags)(1u << bspIndex);
                }

                if (desiredBspMask == 0)
                    desiredBspMask = blamScnr.ZoneSets[zoneSetIndex].LoadedBsps;

                // perform the conversion
                Convert(cacheStream, CacheContext, blamStream, BlamCache, scenarioPath, mapId, blamScnr, blamScnrTag, zoneSetIndex, (uint)desiredBspMask, conversionFlags, portingFlags);

                // generate the .map file
                GenerateMapFile(cacheStream, CacheContext, CacheContext.TagCache.GetTag($"{scenarioPath}.scnr"), mapName, mapDescription);

                // finish up
                CacheContext.SaveStrings();
                CacheContext.SaveTagNames();

                Console.WriteLine("Done.");
                return true;
            }
        }

        int ParseConversionFlags(List<string> args, out MultiplayerScenarioConversionFlags conversionFlags, out PortingFlags portingFlags)
        {
            conversionFlags = MultiplayerScenarioConversionFlags.Default;
            portingFlags = PortingFlags.Default;

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


                PortingFlags portingFlag;
                MultiplayerScenarioConversionFlags conversionFlag;
                if (Enum.TryParse(flagName, true, out conversionFlag))
                {
                    endIndex++;

                    if (not)
                        conversionFlags &= ~conversionFlag;
                    else
                        conversionFlags |= conversionFlag;
                }
                else if (Enum.TryParse(flagName, true, out portingFlag))
                {
                    endIndex++;

                    if (not)
                        portingFlags &= ~portingFlag;
                    else
                        portingFlags |= portingFlag;
                }
            }

            return endIndex;
        }

        private RealPoint3d FindPointToPlaceRespawnPoint(Stream stream, GameCache cache, Scenario scnr, uint includeBspMask)
        {
            //
            // tries to find a safe place to place a respawn point where the player won't be killed instantly
            // not the smartest way to do it but will work for now. Note: this assumes that other fixups have
            // already been performed such as culling the structure bsp block.
            //

            foreach (var zone in scnr.Zones)
            {
                foreach (var firingPosition in zone.FiringPositions)
                {
                    if (((includeBspMask >> firingPosition.BspIndex) & 1) == 0)
                        continue;

                    if (firingPosition.ReferenceFrame != -1) continue;

                    return firingPosition.Position;
                }
            }

            foreach (var scriptingDatum in scnr.ScriptingData)
            {
                foreach (var pointSet in scriptingDatum.PointSets)
                {
                    if (((includeBspMask >> pointSet.BspIndex) & 1) == 0)
                        continue;

                    foreach (var point in pointSet.Points)
                    {
                        if (point.SurfaceIndex == -1)
                            continue;
                        if (IsSafePointToSpawn(scnr, point.Position))
                            return point.Position;
                    }
                }
            }

            // get the list of all potential objects we can use for a position
            var potentialScenarioObjects = new List<(IList instances, IList<ScenarioPaletteEntry> palette)>();
            potentialScenarioObjects.Add((scnr.Weapons, scnr.WeaponPalette));
            potentialScenarioObjects.Add((scnr.Equipment, scnr.EquipmentPalette));
            potentialScenarioObjects.Add((scnr.Vehicles, scnr.VehiclePalette));
            potentialScenarioObjects.Add((scnr.Crates, scnr.CratePalette));
            potentialScenarioObjects.Add((scnr.Scenery, scnr.SceneryPalette));
     
            foreach(var type in potentialScenarioObjects)
            {
                // check each object
                foreach (ScenarioInstance obj in type.instances)
                {
                    if (obj.ParentNameIndex != -1 || obj.PaletteIndex < 0)
                        continue;

                    if (IsSafePointToSpawn(scnr, obj.Position))
                    {
                        var palette = type.palette;
                        if (palette[obj.PaletteIndex].Object == null)
                            continue;

                        var objectDef = cache.Deserialize<GameObject>(stream, palette[obj.PaletteIndex].Object);

                        // move it up just a bit so we're not clipped into it hopefully
                        RealPoint3d point = obj.Position;
                        point.Z += objectDef.BoundingRadius;
                        return point;
                    }
                }
            }


            //
            // for each bsp check if any safe zones are within the world bounds
            //

            var bspWorldCenterPoints = new List<RealPoint3d>();
            for (int i = 0; i < scnr.StructureBsps.Count; i++)
            {
                if (((includeBspMask >> i) & 1) == 0)
                    continue;

                var sbsp = cache.Deserialize<ScenarioStructureBsp>(stream, scnr.StructureBsps[i].StructureBsp);

                foreach(var instance in sbsp.InstancedGeometryInstances)
                {
                    if (IsSafePointToSpawn(scnr, instance.WorldBoundingSphereCenter))
                        return instance.WorldBoundingSphereCenter;
                }

                foreach (var safeTrigger in scnr.ScenarioSafeTriggers)
                {
                    var volume = scnr.TriggerVolumes[safeTrigger.TriggerVolume];

                    // add the world center point as a fallback if we can't find a safe volume
                    var worldCenterPoint = new RealPoint3d()
                    {
                        X = (sbsp.WorldBoundsX.Lower + sbsp.WorldBoundsX.Upper) * 0.5f,
                        Y = (sbsp.WorldBoundsY.Lower + sbsp.WorldBoundsY.Upper) * 0.5f,
                        Z = (sbsp.WorldBoundsZ.Lower + sbsp.WorldBoundsZ.Upper) * 0.5f
                    };
                    bspWorldCenterPoints.Add(worldCenterPoint);

                    // check the volume position is within the bsp world bounds
                    if (volume.Position.X >= sbsp.WorldBoundsX.Lower && volume.Position.X <= sbsp.WorldBoundsX.Upper &&
                        volume.Position.Y >= sbsp.WorldBoundsY.Lower && volume.Position.Y <= sbsp.WorldBoundsY.Upper &&
                        volume.Position.Z >= sbsp.WorldBoundsZ.Lower && volume.Position.Z <= sbsp.WorldBoundsZ.Upper)
                    {
                        return volume.Position;
                    }
                }
            }

            // as a final fallback use one of the bsp world center points we stored
            if (bspWorldCenterPoints.Count > 0)
                return bspWorldCenterPoints[0];

            return new RealPoint3d(0, 0, 0);
        }

        private bool IsSafePointToSpawn(Scenario scnr, RealPoint3d point)
        {
            foreach (var killTrigger in scnr.ScenarioKillTriggers)
            {
                if (killTrigger.TriggerVolume != -1 && TriggerVolumeTestPoint(scnr, killTrigger.TriggerVolume, point))
                    return false;
            }

            return true;
        }

        public bool TriggerVolumeTestPoint(Scenario scnr, int index, RealPoint3d point)
        {
            var volume = scnr.TriggerVolumes[index];
            // get the vector from the volume center to the point
            var delta = point - volume.Position;
            // check the vector head is within the extents
            return
                Math.Abs(delta.X) < volume.Extents.X &&
                Math.Abs(delta.Y) < volume.Extents.Y &&
                Math.Abs(delta.Z) < volume.Extents.Z;
        }

        public void Convert(Stream destStream, GameCacheHaloOnlineBase destCache, Stream srcStream, GameCache srcCache,
            string scenarioPath, int mapId, Scenario scnr, CachedTag scnrTag, int zoneSetIndex, uint includeBspMask,
            MultiplayerScenarioConversionFlags conversionFlags, PortingFlags portingFlags)
        {
            var resourceStreams = new Dictionary<TagTool.Common.ResourceLocation, Stream>();
            
            using (var tagRenamer = new TagRenamerScope())
            {
                var porttag = new PortTagCommand(destCache, srcCache);
                porttag.SetFlags(portingFlags);
                porttag.InitializeSoundConverter();

                var sldtTag = scnr.Lightmap;
                tagRenamer.Rename(sldtTag, $"{scenarioPath}_faux_lightmap");
                var sldt = (ScenarioLightmap)srcCache.Deserialize(srcStream, sldtTag);
                ConvertLightmap(srcCache.Version, srcStream, sldt, includeBspMask);
                sldt = (ScenarioLightmap)porttag.ConvertData(destStream, srcStream, resourceStreams, sldt, sldt, sldtTag.Name);
                sldt = porttag.ConvertScenarioLightmap(destStream, srcStream, resourceStreams, sldtTag.Name, sldt);

                FixupLightmapBpsData(destCache, destStream, sldt);
                sldtTag = CreateOrReplaceTag<Scenario>(destCache, sldtTag.Name);
                destCache.Serialize(destStream, sldtTag, sldt);

                var spawnPoint = FindPointToPlaceRespawnPoint(srcStream, srcCache, scnr, includeBspMask);

                // Convert the scenario to MP
                new MultiplayerScenarioConverter(scnr, zoneSetIndex, includeBspMask, conversionFlags).Convert();

                tagRenamer.Rename(scnrTag, scenarioPath);
                for (int i = 0; i < scnr.StructureBsps.Count; i++)
                    tagRenamer.Rename(scnr.StructureBsps[i].StructureBsp, $"{scenarioPath}_bsp_{i}");

                scnr.Lightmap = null;
                scnr = (Scenario)porttag.ConvertData(destStream, srcStream, resourceStreams, scnr, scnr, scnrTag.Name);
                scnr = porttag.ConvertScenario(destStream, srcStream, resourceStreams, scnr, scnrTag.Name);
                scnrTag = CreateOrReplaceTag<Scenario>(destCache, scnrTag.Name);
                scnr.MapId = mapId;
                scnr.MapType = ScenarioMapType.Multiplayer;
                scnr.MapSubType = ScenarioMapSubType.None;
                scnr.CampaignId = -1;
                scnr.Lightmap = sldtTag;

                // Fixup the bsps and determine the spawn point while we are at it
                for (int i = 0; i < scnr.StructureBsps.Count; i++)
                {
                    var sbspReference = scnr.StructureBsps[i];
                    var sbsp = destCache.Deserialize<ScenarioStructureBsp>(destStream, sbspReference.StructureBsp);
                    FixupStructureBsp(sbsp, (short)i);
                    destCache.Serialize(destStream, sbspReference.StructureBsp, sbsp);
                }

                // add the spawn point placement if required
                if (conversionFlags.HasFlag(MultiplayerScenarioConversionFlags.SpawnPoint))
                    AddRespawnPoint(scnr, 0, spawnPoint, new RealEulerAngles3d());

                // add the prematch camera
                AddPrematchCamera(scnr, spawnPoint + new RealPoint3d(0, 0, 0.62f), new RealEulerAngles3d());

                // add generic player starting profile
                AddPlayerStartingProfile(scnr);

                // finalize the scenario
                destCache.Serialize(destStream, scnrTag, scnr);
            }
    
            foreach (var pair in resourceStreams)
                pair.Value.Close();
        }

        void GenerateMapFile(Stream cacheStream, GameCache cache, CachedTag scenarioTag, string mapName, string mapDescription)
        {
            var scenarioName = Path.GetFileName(scenarioTag.Name);

            MapFile map = new MapFile();
            var header = new CacheFileHeaderGenHaloOnline();

            var scnr = cache.Deserialize<Scenario>(cacheStream, scenarioTag);

            map.CachePlatform = cache.Platform;
            map.Version = cache.Version;
            map.EndianFormat = EndianFormat.LittleEndian;
            map.MapVersion = CacheFileVersion.HaloOnline;

            header.HeaderSignature = new Tag("head");
            header.FooterSignature = new Tag("foot");
            header.FileVersion = map.MapVersion;
            header.Build = CacheVersionDetection.GetBuildName(cache.Version, cache.Platform);

            switch (scnr.MapType)
            {
                case ScenarioMapType.MainMenu:
                    header.CacheType = CacheFileType.MainMenu;
                    break;
                case ScenarioMapType.SinglePlayer:
                    header.CacheType = CacheFileType.Campaign;
                    break;
                case ScenarioMapType.Multiplayer:
                    header.CacheType = CacheFileType.Multiplayer;
                    break;
            }
            header.SharedCacheType = CacheFileSharedType.None;

            header.MapId = scnr.MapId;
            header.ScenarioTagIndex = scenarioTag.Index;
            header.Name = scenarioTag.Name.Split('\\').Last();
            header.ScenarioPath = scenarioTag.Name;

            map.Header = header;

            header.FileLength = 0x3390;

            map.MapFileBlf = new Blf(cache.Version, cache.Platform);
            map.MapFileBlf.StartOfFile = new BlfChunkStartOfFile() { Signature = "_blf", Length = 0x30, MajorVersion = 1, MinorVersion = 2, ByteOrderMarker = -2, };
            map.MapFileBlf.Scenario = new BlfScenario() { Signature = "levl", Length = 0x98C0, MajorVersion = 3, MinorVersion = 1 };
            map.MapFileBlf.EndOfFile = new BlfChunkEndOfFile() { Signature = "_eof", Length = 0x11, MajorVersion = 1, MinorVersion = 2 };

            var scnrBlf = map.MapFileBlf.Scenario;
            scnrBlf.MapId = scnr.MapId;
            scnrBlf.Names = new NameUnicode32[12];
            for (int i = 0; i < scnrBlf.Names.Length; i++)
                scnrBlf.Names[i] = new NameUnicode32() { Name = "" };

            scnrBlf.Descriptions = new NameUnicode128[12];
            for (int i = 0; i < scnrBlf.Descriptions.Length; i++)
                scnrBlf.Descriptions[i] = new NameUnicode128() { Name = "" };

            scnrBlf.Names[0] = new NameUnicode32() { Name = mapName };
            scnrBlf.Descriptions[0] = new NameUnicode128() { Name = mapDescription };

            scnrBlf.MapName = scenarioName;
            scnrBlf.ImageName = $"m_{scenarioName}";
            scnrBlf.UnknownTeamCount1 = 2;
            scnrBlf.UnknownTeamCount2 = 6;
            scnrBlf.GameEngineTeamCounts = new byte[11] { 00, 02, 08, 08, 08, 08, 08, 08, 04, 02, 08 };

            scnrBlf.MapFlags = BlfScenarioFlags.GeneratesFilm | BlfScenarioFlags.IsMainmenu | BlfScenarioFlags.IsDlc;

            map.MapFileBlf.ContentFlags |= BlfFileContentFlags.StartOfFile | BlfFileContentFlags.Scenario | BlfFileContentFlags.EndOfFile;

            if (cache is GameCacheModPackage)
            {
                var mapStream = new MemoryStream();
                var writer = new EndianWriter(mapStream, leaveOpen: true);
                map.Write(writer);

                var modPackCache = cache as GameCacheModPackage;
                modPackCache.AddMapFile(mapStream, header.MapId);
            }
            else
            {
                var mapFile = new FileInfo(Path.Combine(cache.Directory.FullName, $"{scenarioName}.map"));

                Console.WriteLine($"Generating map file '{mapFile.Name}'...");

                using (var mapFileStream = mapFile.Create())
                {
                    map.Write(new EndianWriter(mapFileStream));
                }
            }
        }

        private void AddPrematchCamera(Scenario scnr, RealPoint3d position, RealEulerAngles3d rotation)
        {
            scnr.CutsceneCameraPoints.Add(new CutsceneCameraPoint()
            {
                Position = position,
                Orientation = rotation,
                Flags = CutsceneCameraPointFlags.PrematchCameraHack,
                Name = "prematch_camera",
            });
        }

        private void AddRespawnPoint(Scenario scnr, int bspIndex, RealPoint3d position, RealEulerAngles3d rotation)
        {
            var instance = new SceneryInstance();
            instance.PaletteIndex = (short)scnr.SceneryPalette.Count;
            instance.NameIndex = -1;
            instance.EditorFolderIndex = -1;
            instance.ParentNameIndex = -1;
            instance.Position = position;
            instance.Rotation = rotation;
            instance.ObjectType = new ScenarioObjectType() { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
            instance.Source = ScenarioInstance.SourceValue.Editor;
            instance.BspPolicy = ScenarioInstance.BspPolicyValue.Default;
            instance.UniqueHandle = new DatumHandle(0xffffffff);
            instance.OriginBspIndex = -1;
            instance.AllowedZoneSets = (ushort)(1u << bspIndex);
            instance.Multiplayer = new MultiplayerObjectProperties();
            instance.Multiplayer.Team = MultiplayerTeamDesignator.Neutral;
            scnr.Scenery.Add(instance);

            scnr.SceneryPalette.Add(new ScenarioPaletteEntry()
            {
                Object = CacheContext.TagCache.GetTag(@"objects\multi\spawning\respawn_point", "scen")
            });
        }

        private void AddPlayerStartingProfile(Scenario scnr)
        {
            scnr.PlayerStartingProfile.Add(new PlayerStartingProfileBlock()
            {
                Name = "start_assault",
                PrimaryWeapon = CacheContext.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\assault_rifle", "weap"),
                PrimaryRoundsLoaded = 32,
                PrimaryRoundsTotal = 108,
                StartingFragGrenadeCount = 2
            });
        }

        private CachedTag CreateOrReplaceTag<T>(GameCache cache, string name) where T : TagStructure
        {
            if (cache.TagCache.TryGetTag<T>(name, out CachedTag tag))
                return tag;
            return cache.TagCache.AllocateTag<Scenario>(name);
        }

        private static void FixupLightmapBpsData(GameCacheHaloOnlineBase destCache, Stream destStream, ScenarioLightmap sldt)
        {
            for (short i = 0; i < sldt.LightmapDataReferences.Count; i++)
            {
                var lbsp = destCache.Deserialize<ScenarioLightmapBspData>(destStream, sldt.LightmapDataReferences[i]);
                lbsp.BspIndex = i;
                destCache.Serialize(destStream, sldt.LightmapDataReferences[i], lbsp);
            }
        }

        private void ConvertLightmap(CacheVersion version, Stream srcStream, ScenarioLightmap lightmap, uint includeBspMask)
        {
            if(lightmap.LightmapDataReferences.Count > 0)
            {
                var newLightmapDataReference = new List<CachedTag>();
                for (int test_index = 0; test_index < 32; test_index++)
                {
                    if ((includeBspMask & (1 << test_index)) > 0)
                    {
                        foreach (var lbspTag in lightmap.LightmapDataReferences)
                        {
                            if (lbspTag == null)
                                continue;
                            var test_lbsp = BlamCache.Deserialize<ScenarioLightmapBspData>(srcStream, lbspTag);

                            if (test_index == test_lbsp.BspIndex)
                            {
                                newLightmapDataReference.Add(lbspTag);
                                break;
                            }
                        }
                    }
                }
                lightmap.LightmapDataReferences = newLightmapDataReference;
            }
            else
            {
                var newLightmapBspData = new List<ScenarioLightmapBspData>();
                for (int test_index = 0; test_index < 32; test_index++)
                {
                    if ((includeBspMask & (1 << test_index)) > 0)
                    {
                        foreach (var lightmapdata in lightmap.Lightmaps)
                        {
                            if (lightmapdata.BspIndex == test_index)
                            {
                                newLightmapBspData.Add(lightmapdata);
                                break;
                            }
                        }
                    }
                }
                lightmap.Lightmaps = newLightmapBspData;
            }
        }

        private static void FixupStructureBsp(ScenarioStructureBsp sbsp, short index)
        {
            foreach (var instance in sbsp.InstancedGeometryInstances)
            {
                foreach (var bspPhysics in instance.BspPhysics)
                    bspPhysics.GeometryShape.BspIndex = (sbyte)index;
            }
        }

        private static void RemoveBlockElementsNotInMask(IList block, uint mask)
        {
            for (int i = block.Count - 1; i >= 0; i--)
            {
                if (((mask >> i) & 1) == 0)
                    block.RemoveAt(i);
            }
        }

        class MultiplayerScenarioConverter
        {
            private Scenario Scenario;
            private int ZoneSetIndex;
            private uint IncludeBspMask;
            private Dictionary<short, short> BspIndexRemapping;
            private MultiplayerScenarioConversionFlags ConversionFlags;
            private uint NewBspMask;

            public MultiplayerScenarioConverter(Scenario scenario, int zoneSetIndex, uint includeBspMask, MultiplayerScenarioConversionFlags flags)
            {
                Scenario = scenario;
                ZoneSetIndex = zoneSetIndex;
                IncludeBspMask = includeBspMask;
                BspIndexRemapping = new Dictionary<short, short>();
                ConversionFlags = flags;
            }

            public void Convert()
            {
                NewBspMask = CreateBspMask();
                RemoveBlockElementsNotInMask(Scenario.StructureBsps, IncludeBspMask);
                ConvertSkies();
                ConvertZoneSet();
                RemoveBlockElementsNotInMask(Scenario.ScenarioClusterData, IncludeBspMask);
                ConvertScenarioObjects();

                Scenario.BspAtlas?.Clear();
                Scenario.CampaignPlayers?.Clear();
                //Scenario.SoftCeilings?.Clear();
                Scenario.PlayerStartingProfile?.Clear();
                Scenario.PlayerStartingLocations?.Clear();
                //Scenario.TriggerVolumes?.Clear();
                Scenario.RecordedAnimations?.Clear();
                Scenario.ZonesetSwitchTriggerVolumes?.Clear();
                Scenario.Unknown32?.Clear();
                Scenario.Unknown33?.Clear();
                Scenario.Unknown34?.Clear();
                Scenario.Unknown35?.Clear();
                Scenario.Unknown36?.Clear();
                Scenario.StylePalette?.Clear();
                Scenario.SquadGroups?.Clear();
                Scenario.Squads?.Clear();
                Scenario.Zones?.Clear();
                Scenario.SquadPatrols?.Clear();
                Scenario.MissionScenes?.Clear();
                Scenario.CharacterPalette?.Clear();
                Scenario.Scripts?.Clear();
                Scenario.ScriptingData?.Clear();
                Scenario.ScriptStrings = null;
                Scenario.ScriptSourceFileReferences?.Clear();
                Scenario.ScriptExternalFileReferences?.Clear();
                Scenario.Scripts?.Clear();
                Scenario.Globals?.Clear();
                Scenario.CutsceneFlags?.Clear();
                Scenario.CutsceneCameraPoints?.Clear();
                Scenario.Cinematics?.Clear();
                Scenario.CinematicLighting?.Clear();
                Scenario.CutsceneTitles?.Clear();
                Scenario.CustomObjectNameStrings = null;
                Scenario.ChapterTitleStrings = null;
                Scenario.UnitSeatsMapping?.Clear();
                //Scenario.ScenarioKillTriggers?.Clear();
                //Scenario.ScenarioSafeTriggers?.Clear();
                Scenario.ScriptExpressions?.Clear();
                Scenario.SubtitleStrings = null;
                Scenario.MissionDialogue?.Clear();
                Scenario.ObjectiveStrings = null;
                Scenario.Interpolators?.Clear();
                Scenario.SimulationDefinitionTable?.Clear();
                Scenario.ObjectReferenceFrames?.Clear();
                Scenario.AiObjectives?.Clear();
                Scenario.DesignerZoneSets?.Clear();
                Scenario.Unknown135?.Clear();
                Scenario.ScenarioMetagame?.Clear();
                Scenario.EditorFolders?.Clear();

                // if we're not interested in path finding data clear that out too
                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.PathFinding))
                    Scenario.AiPathfindingData.Clear();
            }

            private void ConvertSkies()
            {
                for (int i = 0; i < Scenario.SkyReferences.Count; i++)
                {
                    var skyReference = Scenario.SkyReferences[i];

                    uint newActiveBsps = 0;
                    int k = 0;
                    for (int j = 0; j < 32; j++)
                    {
                        if ((((uint)skyReference.ActiveBsps >> j) & 1) != 0)
                        {
                            newActiveBsps |= (1u << k);
                            k++;
                        }
                    }
                    skyReference.ActiveBsps = (BspShortFlags)newActiveBsps;
                }
            }

            private void ConvertZoneSet()
            {
                var zoneSet = Scenario.ZoneSets[ZoneSetIndex];
                var zoneSetPvs = Scenario.ZoneSetPvs[zoneSet.PotentiallyVisibleSetIndex];

                uint newBspPvsMask = CreateBspPvsMask((uint)zoneSetPvs.StructureBspMask);
                zoneSetPvs.StructureBspMask = (BspFlags)NewBspMask;
                RemoveBlockElementsNotInMask(zoneSetPvs.BspChecksums, newBspPvsMask);
                RemoveBlockElementsNotInMask(zoneSetPvs.StructureBspPotentiallyVisibleSets, newBspPvsMask);
                RemoveBlockElementsNotInMask(zoneSetPvs.PortalToDeviceMappings, newBspPvsMask);
                for (int i = 0; i < zoneSetPvs.StructureBspPotentiallyVisibleSets.Count; i++)
                {
                    var pvs = zoneSetPvs.StructureBspPotentiallyVisibleSets[i];
                    for (int clusterIndex = 0; clusterIndex < pvs.Clusters.Count; clusterIndex++)
                    {
                        RemoveBlockElementsNotInMask(pvs.Clusters[clusterIndex].BitVectors, newBspPvsMask);
                        RemoveBlockElementsNotInMask(pvs.ClustersDoorsClosed[clusterIndex].BitVectors, newBspPvsMask);
                        RemoveBlockElementsNotInMask(pvs.ClusterMappings[clusterIndex].Clusters, newBspPvsMask);
                    }
                }
                Scenario.ZoneSetPvs = new List<ZoneSetPvsBlock>() { zoneSetPvs };
                Scenario.ZoneSets = new List<ZoneSet>()
                {
                    new ZoneSet()
                    {
                        Name = zoneSet.Name,
                        BspAtlasIndex = -1,
                        PotentiallyVisibleSetIndex = 0,
                        LoadedBsps = (BspFlags)NewBspMask
                    }
                };

                var zoneSetAudibility = Scenario.ZoneSetAudibility[zoneSet.ScenarioBspAudibilityIndex];
                RemoveBlockElementsNotInMask(zoneSetAudibility.BspClusterToRoomBoundsMappings, newBspPvsMask);
                RemoveBlockElementsNotInMask(zoneSetAudibility.GamePortalToDoorOccluderMappings, newBspPvsMask);
                Scenario.ZoneSetAudibility = new List<ZoneSetAudibilityBlock>() { zoneSetAudibility };
            }

            private void ConvertScenarioObjects()
            {
                ConvertScenarioObject(Scenario.Scenery, Scenario.SceneryPalette);
                ConvertScenarioObject(Scenario.Vehicles, Scenario.VehiclePalette);
                ConvertScenarioObject(Scenario.Equipment, Scenario.EquipmentPalette);
                ConvertScenarioObject(Scenario.Weapons, Scenario.WeaponPalette);

                if (ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.DeviceObjects))
                {
                    ConvertScenarioObject(Scenario.Machines, Scenario.MachinePalette);
                    ConvertScenarioObject(Scenario.Controls, Scenario.ControlPalette);
                }
				else
                {
                    Scenario.Machines.Clear();
                    Scenario.MachinePalette.Clear();
                    Scenario.Controls.Clear();
                    Scenario.ControlPalette.Clear();
                    Scenario.DeviceGroups.Clear();
                }
				if (ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Bipeds))
				{
					ConvertScenarioObject(Scenario.Bipeds, Scenario.BipedPalette);
				}
				else
				{
					Scenario.Bipeds.Clear();
					Scenario.BipedPalette.Clear();
				}

				ConvertScenarioObject(Scenario.Terminals, Scenario.TerminalPalette);
                ConvertScenarioObject(Scenario.SoundScenery, Scenario.SoundSceneryPalette);
                ConvertScenarioObject(Scenario.Giants, Scenario.GiantPalette);
                ConvertScenarioObject(Scenario.EffectScenery, Scenario.EffectSceneryPalette);
                ConvertScenarioObject(Scenario.LightVolumes, Scenario.LightVolumePalette);
                ConvertScenarioObject(Scenario.Crates, Scenario.CratePalette);
                ConvertScenarioObject(Scenario.Creatures, Scenario.CreaturePalette);
            }

            private void ConvertScenarioObject<T>(List<T> placements, List<ScenarioPaletteEntry> palette) where T : ScenarioInstance
            {
                // if we're not interested in objects just clear both blocks and return
                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Objects))
                {
                    placements.Clear();
                    palette.Clear();
                    return;
                }

                var newPlacements = new List<T>();
                var keepPaletteIndices = new HashSet<int>();

                // loop through placements placements and determine which objects we are going to keep
                for (int placementIndex = 0; placementIndex < placements.Count; placementIndex++)
                {
                    var placement = placements[placementIndex];

                    // check if the object is supposed to be spawned on any of the desired bsps
                    if (((placement.AllowedZoneSets & IncludeBspMask) == 0))
                        continue;
                    // check the palette index is valid
                    if (placement.PaletteIndex < 0 || placement.PaletteIndex >= palette.Count)
                        continue;
                    // check if the palette entry has a valid tag instance
                    if (palette[placement.PaletteIndex].Object == null)
                        continue;

                    keepPaletteIndices.Add(placement.PaletteIndex);

                    // fixup the origin bsp index
                    if (placement.OriginBspIndex >= 0 && (IncludeBspMask & (1u << placement.OriginBspIndex)) != 0)
                    {
                        placement.OriginBspIndex = (short)BspIndexRemapping[placement.OriginBspIndex];
                    }
                    else
                    {
                        placement.OriginBspIndex = -1;
                    }

                    // fixup the name placement index (for children)
                    if (placement.NameIndex >= 0 && placement.NameIndex < Scenario.ObjectNames.Count)
                    {
                        var name = Scenario.ObjectNames[placement.NameIndex];
                        name.PlacementIndex = (short)(newPlacements.Count);
                    }

                    uint mask = 0;
                    for (int i = 0, j = 0; i < 32; i++)
                    {
                        if (((IncludeBspMask >> i) & 1) != 0)
                            mask |= 1u << j++;
                    }

                    // fixup the allowed bsps
                    placement.AllowedZoneSets = (ushort)mask;
                    // tell it to always spawn
                    placement.PlacementFlags &= ~ObjectPlacementFlags.NotAutomatically;

                    // add the placement to the new list
                    newPlacements.Add(placement);
                }

                // TODO: run another pass over the placements and fixup the palette index, 
                // so we can just remove them rather than nulling the tag instance
                for (int i = 0; i < palette.Count; i++)
                {
                    if (!keepPaletteIndices.Contains(i))
                        palette[i].Object = null;
                }

                // add the new placements (in-place)
                placements.Clear();
                placements.AddRange(newPlacements);
            }

            private uint CreateBspMask()
            {
                uint mask = 0;
                short index = 0;
                for (short i = 0; i < 32; i++)
                {
                    if (((IncludeBspMask >> i) & 1) != 0)
                    {
                        BspIndexRemapping[i] = index;
                        mask |= (1u << index);
                        index++;
                    }
                }
                return mask;
            }

            private uint CreateBspPvsMask(uint bspPvsMask)
            {
                uint mask = 0;
                for (int i = 0; i < 32; i++)
                {
                    if (((IncludeBspMask >> i) & 1) != 0)
                    {
                        var pvsIndex = ScenarioPvsBspPvsIndexGet(bspPvsMask, i);
                        if (pvsIndex == -1)
                            continue;
                        mask |= (1u << pvsIndex);
                    }
                }
                return mask;
            }

            int ScenarioPvsBspPvsIndexGet(uint mask, int index)
            {
                for (int i = 0, k = 0; i < 32; i++)
                {
                    if (((mask >> i) & 1) != 0)
                    {
                        if (i == index)
                            return k;
                        k++;
                    }
                }
                return -1;
            }
        }

        class TagRenamerScope : IDisposable
        {
            private Dictionary<CachedTag, string> originalNames = new Dictionary<CachedTag, string>();

            public void Dispose()
            {
                foreach (var namePair in originalNames)
                    namePair.Key.Name = namePair.Value;
            }

            public void Rename(CachedTag tag, string name)
            {
                originalNames[tag] = tag.Name;
                tag.Name = name;
            }
        }
    }
}
