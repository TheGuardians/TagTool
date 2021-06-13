using System;
using System.Collections.Generic;
using System.IO;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using static TagTool.BlamFile.MapVariantGenerator;
using static TagTool.Tags.Definitions.Scenario;
using static TagTool.Tags.Definitions.Scenario.MultiplayerObjectProperties;

namespace TagTool.Commands.Scenarios
{
    class ImportMapVariantCommand : Command
    {
        private GameCache Cache;
        private CachedTag Tag;
        private Scenario Definition;

        public ImportMapVariantCommand(GameCacheHaloOnlineBase cache, CachedTag Tag, Scenario definition) :
            base(false,

                "ImportMapVariant",
                "Imports a map variant into the current scenario",
                "ImportMapVariant [MapFile] <Path>",
                "If optional argument MapFile is specified, the map variant will instead be stored directly in the .map file and become the default map.")
        {
            Cache = cache;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1)
                return new TagToolError(CommandError.ArgCount);

            bool importingIntoMapFile  = false;

            if(args.Count > 0)
            {
                if(args[0].ToLower() == "mapfile")
                {
                    importingIntoMapFile = true;
                    args.RemoveAt(0);
                }
            }

            var sandboxMapFile = new FileInfo(args[0]);
            using (var stream = sandboxMapFile.OpenRead())
            {
                var reader = new EndianReader(stream);
                var blf = new Blf(Cache.Version);
                if (!blf.Read(reader))
                    return new TagToolError(CommandError.FileType, "Not a valid sandbox.map file");

                if (importingIntoMapFile)
                    ImportIntoMapFile(blf);
                else
                {
                    if (blf.MapVariant.MapVariant.MapId != Definition.MapId)
                        throw new InvalidOperationException("Tried to import a map variant into a scenario with a different map id");
                    ImportIntoScenario(blf);
                }
            }

            Console.WriteLine("Done.");
            return true;
        }

        private void ImportIntoMapFile(Blf mapVariantBlf)
        {
            if (Cache is GameCacheModPackage modCache)
            {
                var mapFileIndex = modCache.BaseModPackage.MapIds.IndexOf(Definition.MapId);
                if (mapFileIndex == -1)
                    throw new InvalidOperationException("Map not found in in mod package");

                InjectMapVariantIntoMapFile(modCache.BaseModPackage.MapFileStreams[mapFileIndex], mapVariantBlf);
            }
            else
            {
                var file = FindMapFileInDirectory(Definition.MapId, Cache.Directory);
                if (file == null)
                    throw new InvalidOperationException("Map not found in in cache directory");

                using (var stream = file.Open(FileMode.Open, FileAccess.ReadWrite))
                    InjectMapVariantIntoMapFile(stream, mapVariantBlf);
            }
        }

        private void ImportIntoScenario(Blf mapVariantBlf)
        {
            using (var cacheStream = Cache.OpenCacheRead())
            {
                var importer = new MapVariantImporter(cacheStream, Cache, Definition, mapVariantBlf);
                importer.Import();
            }
        }

        private FileInfo FindMapFileInDirectory(int mapId, DirectoryInfo directory)
        {
            foreach (var file in directory.GetFiles("*.map"))
            {
                using (var reader = new EndianReader(file.OpenRead()))
                {
                    reader.BaseStream.Position = 0x2DEC;
                    if (reader.ReadInt32() == mapId)
                        return file;
                }
            }

            return null;
        }

        private void InjectMapVariantIntoMapFile(Stream mapFileStream, Blf mapVariantBlf)
        {
            var reader = new EndianReader(mapFileStream);
            var writer = new EndianWriter(mapFileStream);

            var mapFile = new MapFile();
            mapFile.Read(reader);

            if (mapFile.MapFileBlf == null)
                throw new InvalidOperationException("Not a valid map file. Missing blf data");

            mapVariantBlf.MapVariant.MapVariant.MapId = Definition.MapId;
            if (mapFile.MapFileBlf.Scenario != null)
            {
                UpdateMetadata(mapVariantBlf.ContentHeader.Metadata, mapFile.MapFileBlf.Scenario);
                UpdateMetadata(mapVariantBlf.MapVariant.MapVariant.Metadata, mapFile.MapFileBlf.Scenario);
            }
            
            mapFile.MapFileBlf.MapVariant = mapVariantBlf.MapVariant;     
            mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariant;
            mapFile.MapFileBlf.MapVariantTagNames = mapVariantBlf.MapVariantTagNames;
            mapFile.MapFileBlf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;

            writer.BaseStream.Position = 0;
            mapFile.Write(writer);
        }

        private void UpdateMetadata(ContentItemMetadata metadata, BlfScenario blfScenario)
        {
            metadata.MapId = Definition.MapId;
            metadata.Name = blfScenario.Names[0].Name;
            metadata.Description = blfScenario.Descriptions[0].Name;
        }

        class MapVariantImporter
        {
            private Stream _cacheStream;
            private GameCache _cache;
            private readonly Scenario _scenario;
            private readonly Blf _blf;
            private readonly MapVariant _mapVariant;
            private readonly Dictionary<GameObjectTypeHalo3ODST, ObjectTypeDefinition> _objectTypes
                = new Dictionary<GameObjectTypeHalo3ODST, ObjectTypeDefinition>();
            private Dictionary<GameObjectTypeHalo3ODST, HashSet<int>> _deletedPlacements = new Dictionary<GameObjectTypeHalo3ODST, HashSet<int>>();

            public MapVariantImporter(Stream cacheStream, GameCache cache, Scenario scenario, Blf blf)
            {
                _cacheStream = cacheStream;
                _cache = cache;
                _scenario = scenario;
                _blf = blf;
                _mapVariant = blf.MapVariant.MapVariant;

                _objectTypes.Add(GameObjectTypeHalo3ODST.Vehicle, new ObjectTypeDefinition(_scenario.Vehicles, _scenario.VehiclePalette));
                _objectTypes.Add(GameObjectTypeHalo3ODST.Weapon, new ObjectTypeDefinition(_scenario.Weapons, _scenario.WeaponPalette));
                _objectTypes.Add(GameObjectTypeHalo3ODST.Equipment, new ObjectTypeDefinition(_scenario.Equipment, _scenario.EquipmentPalette));
                _objectTypes.Add(GameObjectTypeHalo3ODST.Scenery, new ObjectTypeDefinition(_scenario.Scenery, _scenario.SceneryPalette));
                _objectTypes.Add(GameObjectTypeHalo3ODST.Crate, new ObjectTypeDefinition(_scenario.Crates, _scenario.CratePalette));
            }

            public void Import()
            {
                VerifyCreatedOnScenario();
                ImportPlacements();
                HandleDeletedPlacements();
            }

            private void VerifyCreatedOnScenario()
            {
                int scenarioDatumsOffset = 0;
                foreach (var pair in _objectTypes)
                {
                    if (_mapVariant.ScenarioDatumIndices[(int)pair.Key] != scenarioDatumsOffset)
                        throw new InvalidOperationException("Cannot import map variant on a different scenario to that which it was created on");

                    scenarioDatumsOffset += pair.Value.Instances.Count;
                }
            }

            private void ImportPlacements()
            {
                ImportScenarioPlacements();
                ImportUserPlacements();
            }

            private void ImportUserPlacements()
            {
                for (int i = 0; i < _mapVariant.Placements.Length; i++)
                {
                    var placement = _mapVariant.Placements[i];
                    if (!placement.PlacementFlags.HasFlag(PlacementFlags.Valid) || placement.PlacementFlags.HasFlag(PlacementFlags.FromScenario))
                        continue;

                    if (placement.PaletteIndex == -1)
                        continue;

                    var tagPath = _blf.MapVariantTagNames.Names[placement.PaletteIndex].Name;
                    var tag = _cache.TagCache.GetTag(tagPath);
                    var obje = _cache.Deserialize(_cacheStream, tag) as GameObject;

                    var instance = NewScenarioInstance(obje.ObjectType.Halo3ODST, tag);
                    UpdateScenarioInstanceFromPlacement(instance, placement);
                }
            }

            private void ImportScenarioPlacements()
            {
                foreach (var pair in _objectTypes)
                {
                    var instances = pair.Value.Instances;
                    for (int i = 0; i < instances.Count; i++)
                    {
                        var placementIndex = _mapVariant.ScenarioDatumIndices[(int)pair.Key] + i;
                        var placement = _mapVariant.Placements[placementIndex];

                        if (!placement.PlacementFlags.HasFlag(PlacementFlags.Valid) || !placement.PlacementFlags.HasFlag(PlacementFlags.FromScenario))
                            continue;

                        if (placement.PlacementFlags.HasFlag(PlacementFlags.Deleted))
                        {
                            DeletePlacement(pair.Key, i);
                        }
                        else if (placement.PlacementFlags.HasFlag(PlacementFlags.Touched))
                        {
                            UpdateScenarioInstanceFromPlacement(instances[i] as ScenarioInstance, _mapVariant.Placements[placementIndex]);
                        }
                    }
                }
            }

            private void DeletePlacement(GameObjectTypeHalo3ODST type, int index)
            {
                if (!_deletedPlacements.TryGetValue(type, out HashSet<int> deletedSet))
                    _deletedPlacements[type] = deletedSet = new HashSet<int>();

                deletedSet.Add(index);
            }

            private void UpdateScenarioInstanceFromPlacement(ScenarioInstance instance, MapVariantPlacement placement)
            {
                var multiplayerInstance = instance as IMultiplayerInstance;

                // update position / rotation

                if (placement.ParentScenarioObject.UniqueID != DatumHandle.None)
                {
                    throw new NotSupportedException("Attached placements are not supported currently.");

                    var objectName = _scenario.ObjectNames[multiplayerInstance.Multiplayer.AttachedNameIndex];
                    var parentInstanceIndex = objectName.PlacementIndex;
                    var parentInstance = _objectTypes[objectName.ObjectType.Halo3ODST].Instances[objectName.PlacementIndex] as ScenarioInstance;

                    Vectors3dFromEulerAngles(parentInstance.Rotation, out RealVector3d parentForward, out RealVector3d parentUp);

                    var parentSpaceFoward = TransformVector3d(parentForward, parentUp, placement.Forward);
                    var parentSpaceUp = TransformVector3d(parentForward, parentUp, placement.Up);
                    instance.Rotation = EulerAngles3dFromVectors3d(parentSpaceFoward, parentSpaceUp);
                    instance.Position = TransformPoint3d(parentInstance.Position, parentForward, parentUp, placement.Position);
                }
                else
                {

                    instance.Rotation = EulerAngles3dFromVectors3d(placement.Forward, placement.Up);
                    instance.Position = placement.Position;
                }

                // update multiplayer properties

                var multiplayer = multiplayerInstance.Multiplayer;
                var properties = placement.Properties;

                multiplayer.Team = (TeamValue)properties.TeamAffiliation;
                multiplayer.SpawnTime = placement.Properties.SpawnTime;
                multiplayer.EngineFlags = (ushort)placement.Properties.EngineFlags;

                if (placement.Properties.Shape.Type != MultiplayerObjectShapeType.None)
                {
                    multiplayer.Shape = (ShapeValue)properties.Shape.Type;
                    multiplayer.WidthRadius = properties.Shape.Width;
                    multiplayer.Depth = properties.Shape.Length;
                    multiplayer.Top = properties.Shape.Top;
                    multiplayer.Bottom = properties.Shape.Bottom;
                }

                if (!placement.Properties.MultiplayerFlags.HasFlag(MultiplayerObjectFlags.PlacedAtStart))
                    multiplayer.MultiplayerFlags |= (1 << 7);

                if (placement.Properties.MultiplayerFlags.HasFlag(MultiplayerObjectFlags.Unknown))
                    multiplayer.MultiplayerFlags |= (1 << 6);

                if (properties.MultiplayerFlags.HasFlag(MultiplayerObjectFlags.Symmetric | MultiplayerObjectFlags.Asymmetric))
                    multiplayer.Symmetry = SymmetryValue.Both;
                else if (properties.MultiplayerFlags.HasFlag(MultiplayerObjectFlags.Symmetric))
                    multiplayer.Symmetry = SymmetryValue.Symmetric;
                else if (properties.MultiplayerFlags.HasFlag(MultiplayerObjectFlags.Asymmetric))
                    multiplayer.Symmetry = SymmetryValue.Asymmetric;

                switch (placement.Properties.ObjectType)
                {
                    case MultiplayerObjectType.Weapon:
                        break;
                    case MultiplayerObjectType.Teleporter2Way:
                    case MultiplayerObjectType.TeleporterReceiver:
                    case MultiplayerObjectType.TeleporterSender:
                        multiplayer.TeleporterChannel = (sbyte)placement.Properties.SharedStorage;
                        break;
                    default:
                        multiplayer.SpawnSequence = (sbyte)placement.Properties.SharedStorage;
                        break;
                }

                // handle instanced geometry

                if (instance is SceneryInstance)
                {
                    var tag = (_objectTypes[instance.ObjectType.Halo3ODST].Palette[instance.PaletteIndex] as ScenarioPaletteEntry).Object;
                    var scenery = _cache.Deserialize(_cacheStream, tag) as Scenery;
                    var model = _cache.Deserialize(_cacheStream, scenery.Model) as Model;
                    if (model.CollisionModel != null && model.PhysicsModel == null)
                        instance.Scale = properties.Shape.Width;
                }
            }

            private void HandleDeletedPlacements()
            {
                foreach (var pair in _deletedPlacements)
                {
                    var objectType = pair.Key;
                    var deletedIndices = pair.Value;

                    var oldToNewInstance = new Dictionary<int, int>();
                    var newInstances = new List<ScenarioInstance>();

                    var objectTypeDef = _objectTypes[objectType];
                    for (int i = 0; i < objectTypeDef.Instances.Count; i++)
                    {
                        if (!deletedIndices.Contains(i))
                        {
                            if (i != newInstances.Count)
                                oldToNewInstance[i] = newInstances.Count;

                            newInstances.Add(objectTypeDef.Instances[i] as ScenarioInstance);
                        }
                    }

                    objectTypeDef.Instances.Clear();
                    foreach (var instance in newInstances)
                        objectTypeDef.Instances.Add(instance);

                    FixupScenarioInstances(objectType, oldToNewInstance);
                }
            }

            private void FixupScenarioInstances(GameObjectTypeHalo3ODST type, Dictionary<int, int> remapping)
            {
                // fixup object names
                for (int i = 0; i < _scenario.ObjectNames.Count; i++)
                {
                    var objectName = _scenario.ObjectNames[i];
                    if (remapping.ContainsKey(objectName.PlacementIndex))
                        objectName.PlacementIndex = (short)remapping[objectName.PlacementIndex];
                }
            }

            private int GetOrAddScenarioPaletteEntry(GameObjectTypeHalo3ODST type, CachedTag tag)
            {
                var palette = _objectTypes[type].Palette;
                for (int i = 0; i < palette.Count; i++)
                {
                    var entry = palette[i] as ScenarioPaletteEntry;
                    if (entry.Object == tag)
                        return i;
                }

                return palette.Add(new ScenarioPaletteEntry() { Object = tag });
            }

            private ScenarioInstance NewScenarioInstance(GameObjectTypeHalo3ODST type, CachedTag tag)
            {
                var instance = Activator.CreateInstance(_objectTypes[type].Instances.GetType().GetGenericArguments()[0]) as ScenarioInstance;
                instance.PaletteIndex = (short)GetOrAddScenarioPaletteEntry(type, tag);
                instance.NameIndex = -1;
                instance.UniqueHandle = DatumHandle.None;
                instance.ObjectType = new ScenarioObjectType() { Halo3ODST = type };
                instance.Source = ScenarioInstance.SourceValue.Editor;
                instance.ParentNameIndex = -1;
                instance.AllowedZoneSets = 1;

                var multiplayerInstance = instance as IMultiplayerInstance;
                var multiplayer = multiplayerInstance.Multiplayer = new MultiplayerObjectProperties();
                multiplayer.Team = MultiplayerObjectProperties.TeamValue.Neutral;
                multiplayer.AttachedNameIndex = -1;
                _objectTypes[type].Instances.Add(instance);

                return instance;
            }

            #region Math
            private static RealEulerAngles3d EulerAngles3dFromVectors3d(RealVector3d forward, RealVector3d up)
            {
                var left = RealVector3d.CrossProduct(up, forward);

                float i;
                float j = -(float)Math.Asin(up.I);
                float k;

                float n = (float)Math.Cos(j);
                if (n > 0.0001f)
                {
                    float d = -up.J / n;
                    float d2 = up.K / n;
                    k = (float)Math.Atan2(d, d2);

                    float f = -left.I / n;
                    float f2 = forward.I / n;
                    i = (float)Math.Atan2(f, f2);
                }
                else
                {
                    k = 0;
                    i = (float)Math.Atan2(forward.J, left.J);
                }

                return new RealEulerAngles3d(Angle.FromRadians(i), Angle.FromRadians(j), Angle.FromRadians(k));
            }

            private static void Vectors3dFromEulerAngles(RealEulerAngles3d angles, out RealVector3d forward, out RealVector3d up)
            {
                float sy = (float)Math.Sin(angles.Yaw.Radians);
                float sp = (float)Math.Sin(angles.Pitch.Radians);
                float sr = (float)Math.Sin(angles.Roll.Radians);
                float cy = (float)Math.Cos(angles.Yaw.Radians);
                float cp = (float)Math.Cos(angles.Pitch.Radians);
                float cr = (float)Math.Cos(angles.Roll.Radians);

                forward = new RealVector3d(cy * cp, (sy * cr) - (sp * sr * cy), (sp * cr * cy) + (sy * sr));
                up = new RealVector3d(-sp, -(cp * sr), cp * cr);
            }


            private static RealVector3d TransformVector3d(RealVector3d forward, RealVector3d up, RealVector3d vector)
            {
                var left = RealVector3d.CrossProduct(up, forward);

                return new RealVector3d(
                    ((left.I * vector.J) + (forward.I * vector.I)) + up.I * vector.K,
                    ((left.J * vector.J) + (forward.J * vector.I)) + up.J * vector.K,
                    ((left.K * vector.J) + (forward.K * vector.I)) + up.K * vector.K);
            }

            private static RealPoint3d TransformPoint3d(RealPoint3d origin, RealVector3d forward, RealVector3d up, RealPoint3d point)
            {
                var v = new RealVector3d(point.X - origin.X, point.Y - origin.Y, point.Z - origin.Z);
                TransformVector3d(forward, up, v);
                return new RealPoint3d(v.I, v.J, v.K);
            }
            #endregion
        }
    }
}
