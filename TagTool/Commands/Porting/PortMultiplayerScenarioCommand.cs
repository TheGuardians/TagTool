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

namespace TagTool.Commands.Porting
{
    class PortMultiplayerScenarioCommand : Command
    {
        private GameCacheHaloOnlineBase CacheContext { get; }
        private GameCache BlamCache { get; }
        private PortTagCommand PortTag { get; }

        [Flags]
        public enum MultiplayerScenarioConversionFlags
        {
            None = (1 << 0),
            // port scenario objects
            Objects = (1 << 1),
            // port device objects
            DeviceObjects = (1 << 2),
            // port audio
            Audio = (1 << 3),
            // use ms30 shaders
            Ms30 = (1 << 4),
            // attempt to add an invisible point
            SpawnPoint = (1 << 5),
            // keep path finding data
            PathFinding = (1 << 6),

            Default = Objects | DeviceObjects | Audio | Ms30 | SpawnPoint
        }

        public PortMultiplayerScenarioCommand(GameCacheHaloOnlineBase cacheContext, GameCache blamCache, PortTagCommand portTag) :
            base(true,

                "PortMultiplayerScenario",
                "Builds a multiplayer map from one or more bsps within a given zone set",

                "PortMultiplayerScenario [options]",

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
            BspFlags desiredBspMask = 0;
            MultiplayerScenarioConversionFlags conversionFlags;
            string scenarioPath = null;
            int mapId = -1;

            var firstNonFlagArgumentIndex = ParseConversionFlags(args, out conversionFlags);
            args = args.Skip(firstNonFlagArgumentIndex).ToList();

            CachedTag blamScnrTag = BlamCache.TagCache.NonNull().First(x => x.Group.Tag == "scnr");

            using(var blamStream = BlamCache.OpenCacheRead())
            using (var cacheStream = CacheContext.OpenCacheReadWrite())
            {
                var blamScnr = BlamCache.Deserialize<Scenario>(blamStream, blamScnrTag);
      
                Dictionary<string, int> structureBspsByName = new Dictionary<string, int>();
                Dictionary<string, int> zoneSetsByName = new Dictionary<string, int>();

                Console.WriteLine("Enter the scenario name:");
                var scenarioName = Console.ReadLine();
                if (!Regex.IsMatch(scenarioName, "[a-z0-9_]+"))
                    return new TagToolError(CommandError.CustomMessage, "Scenario name must consist of lowercase alphanumeric characters and underscores");

                scenarioPath = $@"levels\custom\{scenarioName}\{scenarioName}";

                //
                // try to parse the map id, if not use the randomly generated one unless it's actually invalid/out of range
                //

                const int kMinMapId = 7000;
                const int kMaxMapId = ushort.MaxValue-1;

                // generate a map id
                mapId = new Random(Guid.NewGuid().GetHashCode()).Next(kMinMapId, kMaxMapId+1);

                Console.WriteLine($"Enter map id in the range [{kMinMapId}, {kMaxMapId}] (or blank for {mapId}):");

                // try to parse one from input
                var tmpMapId = -1;
                var mapIdInput = Console.ReadLine();
                if (int.TryParse(mapIdInput, out tmpMapId))
                {
                    if (tmpMapId < kMinMapId || tmpMapId > kMaxMapId)
                        return new TagToolError(CommandError.CustomMessage, "Map id out of range");
                }
                else
                {
                    tmpMapId = -1;
                }

                if (tmpMapId != -1)
                    mapId = tmpMapId;

                Console.WriteLine("Enter the map name (for display):");
                var mapName = Console.ReadLine();
                if(mapName.Length >= 4 && mapName.Length > 15)
                    return new TagToolError(CommandError.CustomMessage, "Map name must be at 4 to 15 characters");

                Console.WriteLine("Enter the map description:");
                var mapDescription = Console.ReadLine();
                if (mapDescription.Length > 127)
                    return new TagToolError(CommandError.CustomMessage, "Map description must be no longer than 127 characters");

                Console.WriteLine("-----------------------------------------");
                for (int i = 0; i < blamScnr.ZoneSets.Count; i++)
                {
                    // map the zone sets by name for easy lookup
                    zoneSetsByName.Add(blamCache.StringTable.GetString(blamScnr.ZoneSets[i].Name), i);
                    Console.WriteLine($"{i}. {blamCache.StringTable.GetString(blamScnr.ZoneSets[i].Name)}");
                }
                  
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine("Enter the name or index of the zone set to use:");

                // read the zone set name from input, trim it so we don't have to worry about spaces
                string zoneSetName = Console.ReadLine().Trim();
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

                for (string line; !string.IsNullOrWhiteSpace(line = Console.ReadLine());)
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

                // perofrm the conversion
                Convert(cacheStream, scenarioPath, mapId, blamScnr, blamScnrTag, blamStream, zoneSetIndex, desiredBspMask, conversionFlags);

                // generate the .map file
                GenerateMapFile(cacheStream, CacheContext.TagCache.GetTag($"{scenarioPath}.scnr"), mapName, mapDescription);

                // finish up
                CacheContext.SaveStrings();
                CacheContext.SaveTagNames();

                Console.WriteLine("Done.");
                return true;
            }
        }

        void GenerateMapFile(Stream cacheStream, CachedTag scenarioTag, string mapName, string mapDescription)
        {
            var scenarioName = Path.GetFileName(scenarioTag.Name);

            var blf = new BlamFile.Blf(CacheVersion.HaloOnline106708);

            MapFile map = new MapFile();
            var header = new CacheFileHeader();

            var scnr = CacheContext.Deserialize<Scenario>(cacheStream, scenarioTag);

            map.Version = CacheContext.Version;
            map.EndianFormat = EndianFormat.LittleEndian;
            map.MapVersion = CacheFileVersion.HaloOnline;

            header.HeaderSignature = new Tag("head");
            header.FooterSignature = new Tag("foot");
            header.FileVersion = map.MapVersion;
            header.Build = CacheVersionDetection.GetBuildName(CacheContext.Version);

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
            header.SharedType = CacheFileSharedType.None;

            header.MapId = scnr.MapId;
            header.ScenarioTagIndex = scenarioTag.Index;
            header.Name = scenarioTag.Name.Split('\\').Last();
            header.ScenarioPath = scenarioTag.Name;

            map.Header = header;

            header.FileLength = 0x3390;

            map.MapFileBlf = new Blf(CacheContext.Version);
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

            if (CacheContext is GameCacheModPackage)
            {
                var mapStream = new MemoryStream();
                var writer = new EndianWriter(mapStream, leaveOpen: true);
                map.Write(writer);

                var modPackCache = CacheContext as GameCacheModPackage;
                modPackCache.AddMapFile(mapStream, map.Header.MapId);
            }
            else
            {
                var mapFile = new FileInfo(Path.Combine(CacheContext.Directory.FullName, $"{scenarioName}.map"));

                Console.WriteLine($"Generating map file '{mapFile.Name}'...");

                using (var mapFileStream = mapFile.Create())
                {
                    map.Write(new EndianWriter(mapFileStream));
                }
            }
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

        private void Convert(Stream cacheStream, string newScenarioPath, int newMapId, Scenario blamScnr, CachedTag blamScnrTag, Stream blamStream, int zoneSetIndex, BspFlags bspMask, MultiplayerScenarioConversionFlags conversionFlags)
        {
            var blamCache = BlamCache;

            var resourceStreams = new Dictionary<ResourceLocation, Stream>();

            var defaultPortingFlags = PortingFlags.Default;
            if (!conversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Audio))
                defaultPortingFlags &= ~PortingFlags.Audio;
            if (!conversionFlags.HasFlag(MultiplayerScenarioConversionFlags.Ms30))
                defaultPortingFlags &= ~PortingFlags.Ms30;

            Console.WriteLine("Collecting tags...");
            var tagCollector = new MultiplayerScenarioTagCollector(blamCache, blamScnr, blamStream, zoneSetIndex, bspMask, conversionFlags);
            tagCollector.Collect();

            PortTag.RemoveFlags(~defaultPortingFlags);
            PortTag.SetFlags(defaultPortingFlags);

            var oldScenarioPath = blamScnrTag.Name;
            var scenarioDir = Path.GetDirectoryName(blamScnrTag.Name);
            var newScenarioDir = Path.GetDirectoryName(newScenarioPath);

            var renames = new List<KeyValuePair<CachedTag, string>>();

            // rename the tags we are gong to modify first so they don't conflict when porting
            foreach (var tag in tagCollector.Tags)
            {
                switch(tag.Group.ToString())
                {
                    case "sbsp":
                    case "sLdT":
                    case "Lbsp":
                        renames.Add(new KeyValuePair<CachedTag, string>(tag, tag.Name));
                        tag.Name = tag.Name.Replace(scenarioDir, newScenarioDir);
                        break;
                };
            }
            renames.Add(new KeyValuePair<CachedTag, string>(blamScnrTag, blamScnrTag.Name));
            blamScnrTag.Name = newScenarioPath;

            Console.WriteLine("Converting...");

            // port the tags
            foreach (var tag in tagCollector.Tags)
                PortTag.ConvertTag(cacheStream, blamStream, resourceStreams, tag);

            PortTag.RemoveFlags(PortingFlags.Recursive | PortingFlags.Scripts);

            PortTag.ConvertTag(cacheStream, blamStream, resourceStreams, blamScnrTag);

            Console.WriteLine("Performing fixups...");
            new MultiplayerScenarioFixup(cacheStream, CacheContext, blamScnrTag.Name, zoneSetIndex, bspMask, conversionFlags).Fixup();

            // change the map id
            var scnrTag = CacheContext.TagCache.GetTag($"{blamScnrTag}");
            var newScenario = CacheContext.Deserialize<Scenario>(cacheStream, scnrTag);
            newScenario.MapId = newMapId;
            CacheContext.Serialize(cacheStream, scnrTag, newScenario);

            // restore the tag names
            foreach(var tagNamePair in renames)
                tagNamePair.Key.Name = tagNamePair.Value;

            foreach (var entry in resourceStreams.Values)
                entry.Close();
        }

        class MultiplayerScenarioFixup
        {
            private GameCache CacheContext;
            private Stream CacheStream;
            private CachedTag ScnrTag;
            private Scenario Scnr;
            private int DesiredZoneSetIndex;
            private BspFlags DesiredBsps;
            private BspFlags NewBsps;
            private MultiplayerScenarioConversionFlags ConversionFlags;
            private Dictionary<int, int> BspIndexRemapping;

            public MultiplayerScenarioFixup(
                Stream cacheStream, GameCache cacheContext, string scenarioTagName, int desiredZoneSetIndex,
                BspFlags desiredBsps, MultiplayerScenarioConversionFlags conversionFlags)
            {
                this.CacheContext = cacheContext;
                this.CacheStream = cacheStream;
                this.ScnrTag = cacheContext.TagCache.GetTag<Scenario>(scenarioTagName);
                this.Scnr = cacheContext.Deserialize<Scenario>(cacheStream, ScnrTag);
                this.DesiredZoneSetIndex = desiredZoneSetIndex;
                this.DesiredBsps = desiredBsps;
                this.ConversionFlags = conversionFlags;
                this.BspIndexRemapping = new Dictionary<int, int>();

                // create mask, setting the bits of the remapped bsp indices
                for(int i = 0, j = 0; i < 32; i++)
                {
                    if(desiredBsps.HasFlag((BspFlags)(1 << i)))
                        NewBsps |= (BspFlags)(1 << j++);
                }
            }

            public void Fixup()
            {
                Scnr.MapType = ScenarioMapType.Multiplayer;
                Scnr.MapSubType = ScenarioMapSubType.None;
                Scnr.CampaignId = -1;

                //
                // fixup any bsp indices across the different blocks and tags
                // 

                FixupStructureBspReferences();
                FixupSkies();
                FixupZoneSets();
                FixupAllScenarioObjects();
                FixupScenarioClusterData();
                FixupLightmap();
                FixupInstancedGeometry();

                // find a good place to point the respawn point and prematch camera
                var spawnPoint = FindPointToPlaceRespawnPoint();

                //
                // clear out the blocks we're not interested in
                //
                Scnr.BspAtlas.Clear();
                Scnr.CampaignPlayers.Clear();
                //Scnr.SoftCeilings.Clear();
                Scnr.PlayerStartingProfile.Clear();
                Scnr.PlayerStartingLocations.Clear();
                //Scnr.TriggerVolumes.Clear();
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
                //Scnr.ScenarioKillTriggers.Clear();
                //Scnr.ScenarioSafeTriggers.Clear();
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

                // if we're not interested in path finding data clear that out too
                if (!ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.PathFinding))
                    Scnr.AiPathfindingData.Clear();

                // add the spawn point placement if required
                if (ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.SpawnPoint))
                    AddRespawnPoint(spawnPoint, new RealEulerAngles3d());

                // add the prematch camera
                AddPrematchCamera(spawnPoint + new RealPoint3d(0,0,0.62f), new RealEulerAngles3d());

                // add a starting profile (can't remember if this is actually needed. i'm going to say no, it's not)
                Scnr.PlayerStartingProfile = new List<PlayerStartingProfileBlock>
                {
                    new PlayerStartingProfileBlock
                    {
                        Name = "start_assault",
                        PrimaryWeapon = CacheContext.TagCache.GetTag<Weapon>(@"objects\weapons\rifle\assault_rifle\assault_rifle"),
                        PrimaryRoundsLoaded = 32,
                        PrimaryRoundsTotal = 96,
                        StartingFragGrenadeCount = 2,
                        Unknown3 = -1
                    }
                };

                CacheContext.Serialize(CacheStream, ScnrTag, Scnr);
            }

            private RealPoint3d FindPointToPlaceRespawnPoint()
            {
                //
                // tries to find a safe place to place a respawn point where the player won't be killed instantly
                // not the smartest way to do it but will work for now. Note: this assumes that other fixups have
                // already been performed such as culling the structure bsp block.
                //

                foreach (var scriptingDatum in Scnr.ScriptingData)
                {
                    foreach (var pointSet in scriptingDatum.PointSets)
                    {
                        if (!DesiredBsps.HasFlag((BspFlags)(1 << pointSet.BspIndex)))
                            continue;

                        foreach (var point in pointSet.Points)
                        {
                            if (point.BspIndex != pointSet.BspIndex)
                                continue;
                            if (point.SurfaceIndex == -1)
                                continue;
                            if (IsSafePointToSpawn(point.Position))
                                return point.Position;
                        }
                    }
                }

                foreach (var zone in Scnr.Zones)
                {
                    foreach (var firingPosition in zone.FiringPositions)
                    {
                        if (!DesiredBsps.HasFlag((BspFlags)(1 << firingPosition.BspIndex)))
                            continue;

                        if (firingPosition.ReferenceFrame != -1) continue;

                        return firingPosition.Position;
                    }
                }

                // get the list of all potential objects we can use for a position
                var potentialScenarioObjects =
                    Scnr.Weapons.Cast<ScenarioInstance>()
                    .Concat(Scnr.Equipment)
                    .Concat(Scnr.Vehicles)
                    .Concat(Scnr.Crates)
                    .Concat(Scnr.Scenery);

                // check each object
                foreach (var obj in potentialScenarioObjects)
                {
                    if (obj.ParentNameIndex != -1 || obj.PaletteIndex < 0)
                        continue;

                    if (IsSafePointToSpawn(obj.Position))
                    {
                        var palette = GetPaletteBlock(obj);
                        if (palette[obj.PaletteIndex].Object == null)
                            continue;

                        var objectDef = CacheContext.Deserialize<GameObject>(CacheStream, palette[obj.PaletteIndex].Object);

                        // move it up just a bit so we're not clipped into it hopefully
                        RealPoint3d point = obj.Position;
                        point.Z += objectDef.BoundingRadius;
                        return point;
                    }
                }

                //
                // for each bsp check if any safe zones are within the world bounds
                //

                var bspWorldCenterPoints = new List<RealPoint3d>();
                for (int i = 0; i < Scnr.StructureBsps.Count; i++)
                {
                    var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(CacheStream, Scnr.StructureBsps[i].StructureBsp);

                    foreach (var safeTrigger in Scnr.ScenarioSafeTriggers)
                    {
                        var volume = Scnr.TriggerVolumes[safeTrigger.TriggerVolume];

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

            private List<Scenario.ScenarioPaletteEntry> GetPaletteBlock(ScenarioInstance instance)
            {
                switch(instance)
                {
                    case BipedInstance _: return Scnr.BipedPalette;
                    case VehicleInstance _: return Scnr.VehiclePalette;
                    case WeaponInstance _: return Scnr.WeaponPalette;
                    case EquipmentInstance _: return Scnr.EquipmentPalette;
                    case ControlInstance _: return Scnr.ControlPalette;
                    case TerminalInstance _: return Scnr.TerminalPalette;
                    case MachineInstance _: return Scnr.MachinePalette;
                    case CrateInstance _: return Scnr.CratePalette;
                    case SceneryInstance _: return Scnr.SceneryPalette;
                    case SoundSceneryInstance _: return Scnr.SoundSceneryPalette;
                    case EffectSceneryInstance _:return Scnr.EffectSceneryPalette;
                    case LightVolumeInstance _: return Scnr.LightVolumePalette;
                    case GiantInstance _: return Scnr.GiantPalette;
                    default: throw new NotSupportedException();
                }
            }

            private bool IsSafePointToSpawn(RealPoint3d point)
            {
                foreach (var killTrigger in Scnr.ScenarioKillTriggers)
                {
                    if (killTrigger.TriggerVolume != -1 && TriggerVolumeTestPoint(killTrigger.TriggerVolume, point))
                        return false;
                }

                return true;
            }

            private bool TriggerVolumeTestPoint(int index, RealPoint3d point)
            {
                var volume = Scnr.TriggerVolumes[index];
                // get the vector from the volume center to the point
                var delta = point - volume.Position;
                // check the vector head is within the extents
                return 
                    Math.Abs(delta.X) < volume.Extents.X &&
                    Math.Abs(delta.Y) < volume.Extents.Y &&
                    Math.Abs(delta.Z) < volume.Extents.Z;
            }

            private void FixupSkies()
            {
                //
                // fixup the scenario sky reference block
                //

                foreach (var sky in Scnr.SkyReferences)
                {
                    uint newMask = 0;
                    for (int i = 0; i < 32; i++)
                    {
                        if (sky.ActiveBsps.HasFlag((BspShortFlags)(1 << i)) && DesiredBsps.HasFlag((BspFlags)(1 << i)))
                            newMask |= (1u << BspIndexRemapping[i]);
                    }

                    sky.ActiveBsps = (BspShortFlags)newMask;
                }
            }

            private void FixupStructureBspReferences()
            {
                //
                // fixup the scenario structure bsps block
                //

                var newStructureBsps = new List<StructureBspBlock>();
                for (int bspIndex = 0; bspIndex < Scnr.StructureBsps.Count; bspIndex++)
                {
                    // remove any we're not interested in
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
                //
                // zoneset/zoneset pvs fixup is a little bit involved. At a basic level: the blocks are either indexed by bsp or cluster
                // we're not interested in bsps that are not in our desired bsp mask so the goal is to just remove those 
                // leaving the order intact (matching the new bsp mask).
                //

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

                // we only want one zone set for multiplayer as we are not going to be switching bsps
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

                //
                // fixup the pvs (potentially visible set) data
                //

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
                //
                // to fixup the lightmap we need to remove any bsp data reference datums whos index (bsp index)
                // is not a set bit in our desired bsp mask. the Lbsp also has a bsp index field which needs to be fixed up
                //

                CachedTag lightmapTag;
                if (!CacheContext.TagCache.TryGetTag<ScenarioLightmap>(Scnr.Lightmap.Name, out lightmapTag))
                    return;

                var lightmap = CacheContext.Deserialize<ScenarioLightmap>(CacheStream, lightmapTag);

                var newLightmapDataReference = new List<CachedTag>();
                for (int i = 0; i < lightmap.LightmapDataReferences.Count; i++)
                {
                    if (DesiredBsps.HasFlag((BspFlags)(1 << i)))
                        newLightmapDataReference.Add(lightmap.LightmapDataReferences[i]);
                }

                for (int i = 0; i < newLightmapDataReference.Count; i++)
                {
                    var lbspTag = newLightmapDataReference[i];
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
                //
                // only indlude scenario cluster datums that are in our desired bsp set.
                //

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
                //
                // there is a bsp index in the instance collision definitions 
                // that just needs to be fixed up with the new bsp index.
                //

                for (var i = 0; i < Scnr.StructureBsps.Count; i++)
                {
                    var sbspTag = Scnr.StructureBsps[i].StructureBsp;
                    if (sbspTag == null) continue;

                    var sbsp = CacheContext.Deserialize<ScenarioStructureBsp>(CacheStream, sbspTag);

                    foreach (var instance in sbsp.InstancedGeometryInstances)
                    {
                        foreach (var def in instance.BspPhysics)
                            def.GeometryShape.BspIndex = (sbyte)i;
                    }

                    CacheContext.Serialize(CacheStream, sbspTag, sbsp);
                }
            }

            void ClearBspPvsBlockElements<T>(List<T> block, ulong keepMask)
            {
                //
                // Removes block elements whos indices are not set bits of the keepMask
                // 

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

            private void FixupScenarioObjects<T>(List<T> placements, List<ScenarioPaletteEntry> palette)
                where T : ScenarioInstance
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
                    if (((uint)placement.AllowedZoneSets & (int)DesiredBsps) == 0)
                        continue;
                    // check the palette index is valid
                    if (placement.PaletteIndex < 0 || placement.PaletteIndex >= palette.Count)
                        continue;
                    // check if the palette entry has a valid tag instance
                    if (palette[placement.PaletteIndex].Object == null)
                        continue;

                    keepPaletteIndices.Add(placement.PaletteIndex);

                    // fixup the origin bsp index
                    if (placement.OriginBspIndex >= 0 && DesiredBsps.HasFlag((BspFlags)(1 << placement.OriginBspIndex)))
                    {
                        placement.OriginBspIndex = (short)BspIndexRemapping[placement.OriginBspIndex];
                    }
                    else
                    {
                        placement.OriginBspIndex = -1;
                    }

                    // fixup the name placement index (for children)
                    if (placement.NameIndex >= 0 && placement.NameIndex < Scnr.ObjectNames.Count)
                    {
                        var name = Scnr.ObjectNames[placement.NameIndex];
                        name.PlacementIndex = (short)(newPlacements.Count);
                    }

                    // fixup the allowed bsps
                    placement.AllowedZoneSets = (ushort)NewBsps;
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
                instance.EditorFolderIndex = -1;
                instance.ParentNameIndex = -1;
                instance.Position = position;
                instance.Rotation = rotation;
                instance.ObjectType = new ScenarioObjectType() { Halo3ODST = GameObjectTypeHalo3ODST.Scenery };
                instance.Source = ScenarioInstance.SourceValue.Editor;
                instance.BspPolicy = ScenarioInstance.BspPolicyValue.Default;
                instance.UniqueHandle = new DatumHandle(0xffffffff);
                instance.OriginBspIndex = -1;
                instance.AllowedZoneSets = (ushort)NewBsps;
                instance.Multiplayer = new MultiplayerObjectProperties();
                instance.Multiplayer.AttachedNameIndex = -1;
                instance.Multiplayer.Team = MultiplayerObjectProperties.TeamValue.Neutral;
                Scnr.Scenery.Add(instance);

                Scnr.SceneryPalette.Add(new ScenarioPaletteEntry()
                {
                    Object = CacheContext.TagCache.GetTag(@"objects\multi\spawning\respawn_point", "scen")
                });
            }
        }

        class MultiplayerScenarioTagCollector
        {
            private GameCache BlamCache;
            private Stream BlamStream;
            private Scenario BlamScnr;
            private BspFlags DesiredBsps;
            private int DesiredZoneSetIndex;
            private MultiplayerScenarioConversionFlags ConversionFlags;
            public List<CachedTag> Tags;

            public MultiplayerScenarioTagCollector(GameCache blamCache, Scenario blamScnr, Stream blamStream,
                int desiredZoneSetIndex, BspFlags desiredBsps, MultiplayerScenarioConversionFlags conversionFlags)
            {
                this.BlamCache = blamCache;
                this.BlamScnr = blamScnr;
                this.DesiredZoneSetIndex = desiredZoneSetIndex;
                this.DesiredBsps = desiredBsps;
                this.ConversionFlags = conversionFlags;
                this.Tags = new List<CachedTag>();
                BlamStream = blamStream;
            }

            private void Add(CachedTag tag)
            {
                if (tag != null) Tags.Add(tag);
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

                Add(BlamScnr.Lightmap);

                CollectSkies();
                CollectAllScenarioObjects();
                CollectFlocks();
                CollectDecals(BlamStream);

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
                //
                // the objects commented below work, but they have the potential to make porting times take longer than needed
                // the majority of the time they won't be visible.
                //

                CollectScenarioObjects(BlamScnr.Scenery, BlamScnr.SceneryPalette);
                //CollectScenarioObjects(BlamScnr.Bipeds, BlamScnr.BipedPalette);
                CollectScenarioObjects(BlamScnr.Vehicles, BlamScnr.VehiclePalette);
                CollectScenarioObjects(BlamScnr.Equipment, BlamScnr.EquipmentPalette);
                CollectScenarioObjects(BlamScnr.Weapons, BlamScnr.WeaponPalette);

                if (ConversionFlags.HasFlag(MultiplayerScenarioConversionFlags.DeviceObjects))
                {
                    CollectScenarioObjects(BlamScnr.Machines, BlamScnr.MachinePalette);
                    CollectScenarioObjects(BlamScnr.Terminals, BlamScnr.TerminalPalette);
                    CollectScenarioObjects(BlamScnr.Controls, BlamScnr.ControlPalette);
                }

                CollectScenarioObjects(BlamScnr.SoundScenery, BlamScnr.SoundSceneryPalette);
                //CollectScenarioObjects(BlamScnr.Giants, BlamScnr.GiantPalette); 
                CollectScenarioObjects(BlamScnr.EffectScenery, BlamScnr.EffectSceneryPalette);
                CollectScenarioObjects(BlamScnr.LightVolumes, BlamScnr.LightVolumePalette);
                CollectScenarioObjects(BlamScnr.Crates, BlamScnr.CratePalette);
                CollectScenarioObjects(BlamScnr.Creatures, BlamScnr.CreaturePalette);
            }

            private void CollectScenarioObjects(IEnumerable<ScenarioInstance> objects, List<ScenarioPaletteEntry> palette)
            {
                if (objects == null || palette == null)
                    return;

                foreach (var obj in objects)
                {
                    // check if the object object should be spawned in any of the desired bsps
                    if (((uint)obj.AllowedZoneSets & (int)DesiredBsps) == 0)
                        continue;
                    // check the palette index is valid
                    if (obj.PaletteIndex < 0 || obj.PaletteIndex >= palette.Count)
                        continue;

                    var paletteEntry = palette[obj.PaletteIndex];

                    // check the palette entry tag instance is valid and add it to the list of tags to be ported
                    if (paletteEntry.Object != null)
                        Add(palette[obj.PaletteIndex].Object);
                }
            }

            private void CollectFlocks()
            {
                foreach (var flock in BlamScnr.Flocks.Where(x => DesiredBsps.HasFlag((BspFlags)(1 << (x.BspIndex)))))
                {
                    // check the flock palette index is valid
                    if (flock.FlockPaletteIndex >= 0 && flock.FlockPaletteIndex < BlamScnr.CreaturePalette.Count)
                    {
                        var flockTag = BlamScnr.FlockPalette[flock.FlockPaletteIndex];
                        // check the flock palette entry tag instance is valid and add it to the list of tags to be ported
                        if (flockTag != null && flockTag.Instance != null)
                            Add(flockTag.Instance);
                    }

                    // check the create palette index is valid
                    if (flock.CreaturePaletteIndex >= 0 && flock.CreaturePaletteIndex < BlamScnr.CreaturePalette.Count)
                    {
                        var creatureTag = BlamScnr.CreaturePalette[flock.CreaturePaletteIndex];
                        // check the create palette entry tag instance is valid and add it to the list of tags to be ported
                        if (creatureTag != null && creatureTag.Object != null)
                            Add(creatureTag.Object);
                    }
                }
            }

            private void CollectDecals(Stream blamStream)
            {
                //
                // iterate through the decal adding any any decal systems that have decals that are within the bsp world bounds
                // to the list of the tags to be ported
                //

                var visitedPaletteEntries = new HashSet<int>();
                for (int i = 0; i < BlamScnr.StructureBsps.Count; i++)
                {
                    var sbspRef = BlamScnr.StructureBsps[i];

                    // only check bsps we are interested in
                    if (((BspFlags)(1 << i) & DesiredBsps) == 0)
                        continue;

                    // deserialize the structure bsp or the given index
                    var sbsp = BlamCache.Deserialize<ScenarioStructureBsp>(blamStream, sbspRef.StructureBsp);
                    
                    for (int j = 0; j < BlamScnr.Decals.Count; j++)
                    {
                        var decal = BlamScnr.Decals[j];

                        // check the decal palette index is valid
                        if (decal.DecalPaletteIndex == -1)
                            continue;
                        // check we haven't already added this decal system
                        if (visitedPaletteEntries.Contains(decal.DecalPaletteIndex))
                            continue;

                        // if the deal position is inside the world bounds AABB, add it to the list of tags to be ported
                        // and mark it as visited so we don't check it again
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
