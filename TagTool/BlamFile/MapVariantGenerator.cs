using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.BlamFile
{
    /// <summary>
    /// Generates a map variant blf given a scenario and metadata
    /// </summary>
    public class MapVariantGenerator
    {
        // TODO: add a definition cache

        private GameCache _cache;
        private Scenario _scenario;
        private ScenarioStructureBsp _structureBsp;
        private Stream _cacheStream;
        private ForgeGlobalsDefinition _forgeGlobals;
        private HashSet<CachedTag> _forgePalette;
        public Dictionary<GameObjectTypeHalo3ODST, ObjectTypeDefinition> ObjectTypes;
        public uint ObjectTypeMask =
            (1 << (int)GameObjectTypeHalo3ODST.Vehicle) |
            (1 << (int)GameObjectTypeHalo3ODST.Weapon) |
            (1 << (int)GameObjectTypeHalo3ODST.Equipment) |
            (1 << (int)GameObjectTypeHalo3ODST.Scenery) |
            (1 << (int)GameObjectTypeHalo3ODST.Crate);

        public Blf Generate(Stream cacheStream, GameCache cache, Scenario scenario, ContentItemMetadata metadata)
        {
            _cache = cache;
            _cacheStream = cacheStream;
            _scenario = scenario;
            _structureBsp = cache.Deserialize<ScenarioStructureBsp>(cacheStream, scenario.StructureBsps[0].StructureBsp);
            _forgeGlobals = cache.Deserialize<ForgeGlobalsDefinition>(cacheStream, _cache.TagCache.GetTag("*.forg"));
            _forgePalette = new HashSet<CachedTag>(_forgeGlobals.Palette.Where(x => x.Object != null).Select(x => x.Object));

            ObjectTypes = new Dictionary<GameObjectTypeHalo3ODST, ObjectTypeDefinition>();
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Biped, new ObjectTypeDefinition(_scenario.Bipeds, _scenario.BipedPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Vehicle, new ObjectTypeDefinition(_scenario.Vehicles, _scenario.VehiclePalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Weapon, new ObjectTypeDefinition(_scenario.Weapons, _scenario.WeaponPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Equipment, new ObjectTypeDefinition(_scenario.Equipment, _scenario.EquipmentPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.AlternateRealityDevice, new ObjectTypeDefinition(_scenario.AlternateRealityDevices, _scenario.AlternateRealityDevicePalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Terminal, new ObjectTypeDefinition(_scenario.Terminals, _scenario.TerminalPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Scenery, new ObjectTypeDefinition(_scenario.Scenery, _scenario.SceneryPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Machine, new ObjectTypeDefinition(_scenario.Machines, _scenario.MachinePalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Control, new ObjectTypeDefinition(_scenario.Controls, _scenario.ControlPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.SoundScenery, new ObjectTypeDefinition(_scenario.SoundScenery, _scenario.SoundSceneryPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Crate, new ObjectTypeDefinition(_scenario.Crates, _scenario.CratePalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Creature, new ObjectTypeDefinition(_scenario.Creatures, _scenario.CreaturePalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.Giant, new ObjectTypeDefinition(_scenario.Giants, _scenario.GiantPalette));
            ObjectTypes.Add(GameObjectTypeHalo3ODST.EffectScenery, new ObjectTypeDefinition(_scenario.EffectScenery, _scenario.EffectSceneryPalette));

            for (var i = GameObjectTypeHalo3ODST.Biped; i <= GameObjectTypeHalo3ODST.EffectScenery; i++)
            {
                if (((ObjectTypeMask >> (int)i) & 1) == 0)
                    ObjectTypes.Remove(i);
            }

            return GenerateBlf(metadata, GenerateMapVariant(metadata));
        }

        private Blf GenerateBlf(ContentItemMetadata metadata, MapVariant mapVariant)
        {
            var blf = new Blf(CacheVersion.HaloOnlineED, CachePlatform.Original);

            blf.StartOfFile = new BlfChunkStartOfFile()
            {
                Signature = new Tag("_blf"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkStartOfFile), _cache.Version, _cache.Platform),
                MajorVersion = 1,
                MinorVersion = 2,
                ByteOrderMarker = -2
            };
            blf.ContentFlags |= BlfFileContentFlags.StartOfFile;

            blf.ContentHeader = new BlfContentHeader()
            {
                Signature = new Tag("chdr"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfContentHeader), _cache.Version, _cache.Platform),
                MajorVersion = 9,
                MinorVersion = 3,
                BuildVersion = 0xffffa0d4,
                Metadata = metadata
            };

            blf.ContentFlags |= BlfFileContentFlags.ContentHeader;

            blf.MapVariant = new BlfMapVariant()
            {
                Signature = new Tag("mapv"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfMapVariant), _cache.Version, _cache.Platform),
                MajorVersion = 12,
                MinorVersion = 1,
                MapVariant = mapVariant,
                VariantVersion = 0
            };
            blf.ContentFlags |= BlfFileContentFlags.MapVariant;

            blf.MapVariantTagNames = new BlfMapVariantTagNames()
            {
                Signature = new Tag("tagn"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfMapVariantTagNames), _cache.Version, _cache.Platform),
                MajorVersion = 1,
                MinorVersion = 0,
                Names = Enumerable.Range(0, 256).Select(x => new TagName() { Name = "" }).ToArray()
            };
            blf.ContentFlags |= BlfFileContentFlags.MapVariantTagNames;

            for (int i = 0; i < mapVariant.Palette.Length; i++)
            {
                if (mapVariant.Palette[i].TagIndex == -1)
                    continue;

                var tag = _cache.TagCache.GetTag(mapVariant.Palette[i].TagIndex);
                blf.MapVariantTagNames.Names[i] = new TagName() { Name = $"{tag.Name}.{tag.Group.Tag}" };
            }

            blf.EndOfFile = new BlfChunkEndOfFile()
            {
                Signature = new Tag("_eof"),
                Length = (int)TagStructure.GetStructureSize(typeof(BlfChunkEndOfFile), _cache.Version, _cache.Platform),
                MajorVersion = 1,
                MinorVersion = 1,
                AuthenticationDataSize = 0,
                AuthenticationType = BlfAuthenticationType.None
            };
            blf.ContentFlags |= BlfFileContentFlags.EndOfFile;

            return blf;
        }

        private MapVariant GenerateMapVariant(ContentItemMetadata metadata)
        {
            var mapVariant = CreateEmptyMapVariant(metadata);

            //
            // Compute the scenario datum offsets
            //

            short scenarioDatumsOffset = 0;
            for (var objectType = GameObjectTypeHalo3ODST.Biped; objectType <= GameObjectTypeHalo3ODST.EffectScenery; objectType++)
            {
                if (ObjectTypes.TryGetValue(objectType, out ObjectTypeDefinition objectTypeDefinition))
                {
                    mapVariant.ScenarioDatumIndices[(int)objectType] = scenarioDatumsOffset;
                    scenarioDatumsOffset += (short)objectTypeDefinition.Instances.Count;
                }
                else
                {
                    mapVariant.ScenarioDatumIndices[(int)objectType] = -1;
                }
            }

            //
            // Generate the placements
            //

            for (var objectType = GameObjectTypeHalo3ODST.Biped; objectType <= GameObjectTypeHalo3ODST.EffectScenery; objectType++)
            {
                if (!ObjectTypes.TryGetValue(objectType, out ObjectTypeDefinition objectTypeDefinition))
                    continue;

                var instances = objectTypeDefinition.Instances.Cast<ScenarioInstance>().ToList();
                var palette = objectTypeDefinition.Palette.Cast<ScenarioPaletteEntry>().ToList();

                for (int i = 0; i < instances.Count; i++)
                {
                    var instance = instances[i];
                    if (instance.PaletteIndex < 0 || instance.PaletteIndex >= palette.Count)
                        continue;

                    var paletteEntry = palette[instance.PaletteIndex];

                    if (!ObjectIsForgeable(paletteEntry.Object))
                        continue;

                    var mapvPaletteIndex = GetOrAddPaletteEntry(mapVariant, paletteEntry.Object);
                    if (mapvPaletteIndex < 0)
                        throw new Exception("Pallete overflow!");

                    var mapvPlacementIndex = mapVariant.ScenarioDatumIndices[(int)objectType] + i;
                    var mapvPlacement = mapVariant.Placements[mapvPlacementIndex];

                    InitPlacement(mapvPlacement, objectType, instance, mapvPaletteIndex);

                    // Update the cached pallete entry information
                    mapVariant.Palette[mapvPaletteIndex].CountOnMap++;
                    mapVariant.Palette[mapvPaletteIndex].RuntimeMax++;
                }
            }

            // Update the cached information
            mapVariant.PlacedObjectCount = (short)(scenarioDatumsOffset);
            mapVariant.ScenarioObjectCount = scenarioDatumsOffset;
            mapVariant.PaletteItemCount = 0;

            return mapVariant;
        }

        protected virtual int GetOrAddPaletteEntry(MapVariant mapVariant, CachedTag tag)
        {
            var firstEmptyIndex = -1;
            for (int i = 0; i < mapVariant.Palette.Length; i++)
            {
                if (firstEmptyIndex == -1 && mapVariant.Palette[i].TagIndex == -1)
                    firstEmptyIndex = i;

                if (mapVariant.Palette[i].TagIndex == tag.Index)
                    return i;
            }

            var paletteEntry = mapVariant.Palette[firstEmptyIndex];
            paletteEntry.TagIndex = tag.Index;
            paletteEntry.CountOnMap = 0;
            paletteEntry.RuntimeMax = paletteEntry.CountOnMap;
            paletteEntry.PlacedOnMapMax = 255;
            mapVariant.PaletteItemCount++;
            return firstEmptyIndex;
        }

        private void InitPlacement(
            MapVariantPlacement mapvPlacement, 
            GameObjectTypeHalo3ODST objectType, 
            ScenarioInstance instance, 
            int mapvPaletteIndex)
        {
            var paletteEntry = ObjectTypes[objectType].Palette[instance.PaletteIndex] as ScenarioPaletteEntry;
            var obje = _cache.Deserialize(_cacheStream, paletteEntry.Object) as GameObject;

            mapvPlacement.PlacementFlags = PlacementFlags.Valid | PlacementFlags.FromScenario;
            mapvPlacement.PaletteIndex = mapvPaletteIndex;
            mapvPlacement.Position = instance.Position;
            Vectors3dFromEulerAngles(instance.Rotation, out mapvPlacement.Forward, out mapvPlacement.Up);

            var multiplayerInstance = instance as IMultiplayerInstance;
            if (multiplayerInstance != null && multiplayerInstance.Multiplayer.AttachedNameIndex != -1)
                AttachToParent(mapvPlacement, paletteEntry.Object, multiplayerInstance.Multiplayer.AttachedNameIndex);

            InitMultiplayerProperties(mapvPlacement.Properties, instance, obje);

            // not entirely sure what this is for
            switch (objectType)
            {
                case GameObjectTypeHalo3ODST.Crate:
                case GameObjectTypeHalo3ODST.Equipment:
                case GameObjectTypeHalo3ODST.Weapon:
                case GameObjectTypeHalo3ODST.Vehicle:
                    if (obje.MultiplayerObject[0].Type < GameObject.MultiplayerObjectBlock.TypeValue.Teleporter2way)
                        mapvPlacement.PlacementFlags |= PlacementFlags.UnknownBit7;
                    break;
            }
        }

        private void AttachToParent(MapVariantPlacement mapvPlacement, CachedTag objectTag, int parentNameIndex)
        {
            //
            // Handle parent attachments
            //

            if (parentNameIndex < 0 || parentNameIndex >= _scenario.ObjectNames.Count)
            {
                new TagToolWarning($"Parent object #{parentNameIndex} not found!");
                return;
            }

            var parentName = _scenario.ObjectNames[parentNameIndex];
            var parentScrnInstanceIndex = parentName.PlacementIndex;
            var parentScnrInstance = ObjectTypes[parentName.ObjectType.Halo3ODST].Instances[parentScrnInstanceIndex] as ScenarioInstance;
            var parrentScnrPaletteEntry = ObjectTypes[parentName.ObjectType.Halo3ODST].Palette[parentScnrInstance.PaletteIndex] as ScenarioPaletteEntry;

            // Setup up the parent object identifier
            mapvPlacement.ParentScenarioObject = new ObjectIdentifier()
            {
                BspIndex = parentScnrInstance.OriginBspIndex,
                Source = (sbyte)parentScnrInstance.Source,
                Type = (sbyte)parentScnrInstance.ObjectType.Halo3ODST,
                UniqueID = parentScnrInstance.UniqueHandle
            };

            // Set the attached flags
            mapvPlacement.PlacementFlags |= PlacementFlags.Attached;
            if (ObjectIsFixedOrPhased(objectTag) && ObjectIsEarlyMover(parrentScnrPaletteEntry.Object))
                mapvPlacement.PlacementFlags |= PlacementFlags.AttachedFixed;

            RealVector3d parentForward, parentUp;
            Vectors3dFromEulerAngles(
               parentScnrInstance.Rotation,
               out parentForward,
               out parentUp);

            // Transform the from world space to parent space
            var parentToChildPos = VectorFromPoints3d(parentScnrInstance.Position, mapvPlacement.Position);
            var invParentToChildPos = InverseTransformVector3d(parentForward, parentUp, parentToChildPos);
            mapvPlacement.Forward = InverseTransformVector3d(parentForward, parentUp, mapvPlacement.Forward);
            mapvPlacement.Up = InverseTransformVector3d(parentForward, parentUp, mapvPlacement.Up);
            mapvPlacement.Position = new RealPoint3d(invParentToChildPos.I, invParentToChildPos.J, invParentToChildPos.K);
        }

        private static void InitMultiplayerProperties(MapVariantPlacementProperty properties, ScenarioInstance instance, GameObject obje)
        {
            //
            // Set multiplayer object properties
            //

            var multiplayerInstance = instance as IMultiplayerInstance;
            if (multiplayerInstance == null)
                return;

            var scrnMultiplayerProperties = multiplayerInstance.Multiplayer;


            var objeMultiplayerProperties = obje.MultiplayerObject[0];
            properties.SpawnTime = (byte)objeMultiplayerProperties.SpawnTime;
            properties.ObjectType = (MultiplayerObjectType)objeMultiplayerProperties.Type;
            properties.Shape = new MultiplayerObjectBoundaryShape()
            {
                Type = (MultiplayerObjectShapeType)objeMultiplayerProperties.BoundaryShape,
                Bottom = objeMultiplayerProperties.BoundaryBottom,
                Length = objeMultiplayerProperties.BoundaryLength,
                Top = objeMultiplayerProperties.BoundaryTop,
                Width = objeMultiplayerProperties.BoundaryWidth
            };

            if (objeMultiplayerProperties.EngineFlags != 0)
                properties.EngineFlags = (GameEngineFlags)objeMultiplayerProperties.EngineFlags;

            if (((scrnMultiplayerProperties.MultiplayerFlags >> 6) & 1) != 0)
                properties.MultiplayerFlags |= MultiplayerObjectFlags.Unknown;

            else if (((scrnMultiplayerProperties.MultiplayerFlags >> 7) & 1) != 0)
                properties.MultiplayerFlags |= MultiplayerObjectFlags.PlacedAtStart;

            properties.MultiplayerFlags = MultiplayerObjectFlags.Symmetric | MultiplayerObjectFlags.Asymmetric;
            if (scrnMultiplayerProperties.Symmetry == MultiplayerObjectProperties.SymmetryValue.Symmetric)
                properties.MultiplayerFlags &= ~MultiplayerObjectFlags.Asymmetric;
            if (scrnMultiplayerProperties.Symmetry == MultiplayerObjectProperties.SymmetryValue.Asymmetric)
                properties.MultiplayerFlags &= ~MultiplayerObjectFlags.Symmetric;

            if (scrnMultiplayerProperties.SpawnTime != 0)
                properties.SpawnTime = (byte)scrnMultiplayerProperties.SpawnTime;

            properties.TeamAffiliation = (MapVariantGameTeam)scrnMultiplayerProperties.Team;

            if (scrnMultiplayerProperties.Shape > MultiplayerObjectProperties.ShapeValue.None)
            {
                properties.Shape.Type = (MultiplayerObjectShapeType)scrnMultiplayerProperties.Shape;
                properties.Shape.Bottom = scrnMultiplayerProperties.Bottom;
                properties.Shape.Top = scrnMultiplayerProperties.Top;
                properties.Shape.Width = scrnMultiplayerProperties.WidthRadius;
                properties.Shape.Length = scrnMultiplayerProperties.Depth;
            }

            if (scrnMultiplayerProperties.EngineFlags != 0)
                properties.EngineFlags = (GameEngineFlags)scrnMultiplayerProperties.EngineFlags;


            //
            // Handle Weapon spare clips
            //

            if (obje.ObjectType.Halo3ODST == GameObjectTypeHalo3ODST.Weapon)
            {
                properties.SharedStorage = (byte)ComputeSpareClips(properties, instance as WeaponInstance, obje as Weapon);
            }
            else
            {
                if(scrnMultiplayerProperties.TeleporterChannel != 0)
                    properties.SharedStorage = (byte)scrnMultiplayerProperties.TeleporterChannel;
                else
                    properties.SharedStorage = (byte)multiplayerInstance.Multiplayer.SpawnSequence;
            }
        }

        private static int ComputeSpareClips(MapVariantPlacementProperty properties, WeaponInstance instance, Weapon weap)
        {
            // not entirely sure whether this is correct. the names don't appear to be correct
            // however, in the limited tests i've done, it produces the correct result.

            if (weap.Magazines.Count > 0)
            {
                var magazine = weap.Magazines[0];

                var initial = magazine.RoundsTotalLoadedMaximum;
                var total = magazine.RoundsTotalInitial;

                // Override from scenario
                if (instance.RoundsLoaded != 0)
                    initial = instance.RoundsLoaded;
                if (instance.RoundsLeft != 0)
                    total = instance.RoundsLeft;

                if (initial > 0)
                    return total / initial - 1;
            }

            return 0;
        }

        private MapVariant CreateEmptyMapVariant(ContentItemMetadata metadata)
        {
            //
            // Initialize an empty map variant
            //

            var mapVariant = new MapVariant();
            mapVariant.Metadata = metadata;
            mapVariant.Version = 12;
            mapVariant.ScenarioObjectCount = 0;
            mapVariant.PlacedObjectCount = 0;
            mapVariant.PaletteItemCount = 0;
            mapVariant.MapId = _scenario.MapId;
            mapVariant.WorldBounds = new RealRectangle3d(
              _structureBsp.WorldBoundsX.Lower, _structureBsp.WorldBoundsX.Upper,
              _structureBsp.WorldBoundsY.Lower, _structureBsp.WorldBoundsY.Upper,
              _structureBsp.WorldBoundsZ.Lower, _structureBsp.WorldBoundsZ.Upper
              );
            mapVariant.GameType = (GameEngineType)0xA;
            mapVariant.MaximumBudget = _scenario.SandboxBudget;
            mapVariant.SpentBudget = 0;
            mapVariant.RuntimeShowHelpers = true;
            mapVariant.MapChecksum = (uint)(metadata.MapChecksum);
            mapVariant.Placements = Enumerable.Range(0, 640).Select(x => CreateDefaultPlacement()).ToArray();
            mapVariant.ScenarioDatumIndices = Enumerable.Range(0, 16).Select(x => (short)0).ToArray();
            mapVariant.Palette = Enumerable.Range(0, 256).Select(x => CreateDefaultPaletteItem()).ToArray();
            mapVariant.SimulationEntities = Enumerable.Range(0, 80).Select(x => -1).ToArray();
            return mapVariant;
        }

        private MapVariantPaletteItem CreateDefaultPaletteItem()
        {
            return new MapVariantPaletteItem() { TagIndex = -1 };
        }

        private MapVariantPlacement CreateDefaultPlacement()
        {
            return new MapVariantPlacement()
            {
                ObjectIndex = -1,
                EditorObjectIndex = -1,
                PaletteIndex = -1,
                Properties = new MapVariantPlacementProperty()
                {
                    EngineFlags =
                    GameEngineFlags.Ctf |
                    GameEngineFlags.Slayer |
                    GameEngineFlags.Oddball |
                    GameEngineFlags.Koth |
                    GameEngineFlags.Sandbox |
                    GameEngineFlags.Vip |
                    GameEngineFlags.Juggernaut |
                    GameEngineFlags.Territories |
                    GameEngineFlags.Assault |
                    GameEngineFlags.Infection,
                    MultiplayerFlags = (MultiplayerObjectFlags)0xC
                },
                ParentScenarioObject = new ObjectIdentifier()
                {
                    BspIndex = -1,
                    Type = -1,
                    Source = -1,
                    UniqueID = DatumHandle.None 
                }
            };
        }

        private bool ObjectIsEarlyMover(CachedTag tag)
        {
            var obje = _cache.Deserialize(_cacheStream, tag) as GameObject;
            return obje.ObjectFlags.HasFlag(GameObjectFlags.EarlyMoverLocalizedPhysics);
        }

        public bool ObjectIsForgeable(CachedTag tag)
        {
            if (!_forgePalette.Contains(tag))
                return false;

            var obje = _cache.Deserialize(_cacheStream, tag) as GameObject;
            return obje.MultiplayerObject != null && obje.MultiplayerObject.Count > 0;
        }

        private bool ObjectIsFixedOrPhased(CachedTag tag)
        {
            var obje = _cache.Deserialize(_cacheStream, tag) as GameObject;
            var hlmt = _cache.Deserialize<Model>(_cacheStream, obje.Model);
            var phmo = _cache.Deserialize<PhysicsModel>(_cacheStream, hlmt.PhysicsModel);

            foreach (var rigidBody in phmo.RigidBodies)
            {
                if (rigidBody.MotionType != PhysicsModel.RigidBody.MotionTypeValue.Fixed &&
                    rigidBody.MotionType != PhysicsModel.RigidBody.MotionTypeValue.Keyframed)
                    return false;
            }

            return true;
        }

        #region Math
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

        private static RealVector3d InverseTransformVector3d(RealVector3d forward, RealVector3d up, RealVector3d vector)
        {
            var left = RealVector3d.CrossProduct(up, forward);

            return new RealVector3d
            (
                (float)((forward.J * vector.J) + (forward.I * vector.I)) + (forward.K * vector.K),
                (float)((left.J * vector.J) + (left.I * vector.I)) + (left.K * vector.K),
                (float)((up.J * vector.J) + (up.I * vector.I)) + (up.K * vector.K)
            );
        }

        private static RealVector3d VectorFromPoints3d(RealPoint3d p0, RealPoint3d p1)
        {
            return new RealVector3d(p0.X - p1.X, p0.Y - p1.Y, p0.Z - p1.Z);
        }
        #endregion

        public class ObjectTypeDefinition
        {
            public IList Instances;
            public IList Palette;

            public ObjectTypeDefinition(IList instances, IList palette)
            {
                Instances = instances;
                Palette = palette;
            }
        }
    }
}
