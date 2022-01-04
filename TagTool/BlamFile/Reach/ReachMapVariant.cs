using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags.Definitions.Common;

namespace TagTool.BlamFile.Reach
{
    public class ReachBlfMapVariant
    {
        public byte[] Hash;
        public int Size;
        public ReachMapVariant MapVariant;

        public void Decode(Stream stream)
        {
            var reader = new ReachMapVariantReader(stream);
            Decode(reader);
        }

        public void Decode(ReachMapVariantReader reader)
        {
            Hash = reader.ReadBytes(0x14);
            Size = (int)reader.ReadUnsigned(32);
            MapVariant = new ReachMapVariant();
            MapVariant.Decode(reader);
        }
    }

    public class ReachMapVariant
    {
        public const int MaxObjects = 651;
        public const int MaxQuota = 256;

        public ReachContentItemMetadata Metadata;
        public string Name;
        public string Description;
        public int Version;
        public uint CacheCRC;
        public uint ScenarioPaletteCRC;
        public int PlaceableObjectQuota;
        public int MapId;
        public bool BuiltIn;
        public bool BuiltFromXml;
        public RealRectangle3d WorldBounds;
        public int MaxBudget;
        public int SpentBudget;
        public MegaloStringTable StringTable;
        public List<ReachVariantObjectDatum> Objects;
        public List<ReachVariantQuota> Quotas;

        public Scenario.MapVariantPaletteBlock.MapVariantPaletteEntryBlock GetPaletteEntry(Scenario scenario, int quotaIndex)
        {
            GetPaletteIndices(scenario, quotaIndex, out int paletteIndex, out int entryIndex);
            return scenario.MapVariantPalettes[paletteIndex].Entries[entryIndex];
        }

        public bool GetPaletteIndices(Scenario scenario, int quotaIndex, out int paletteIndex, out int entryIndex)
        {
            paletteIndex = -1;
            entryIndex = -1;

            var absoluteIndex = quotaIndex;
            for (int i = 0; i < scenario.MapVariantPalettes.Count; i++)
            {
                var palette = scenario.MapVariantPalettes[i];
                
                if (absoluteIndex < palette.Entries.Count)
                {
                    paletteIndex = i;
                    entryIndex = absoluteIndex;
                    return true;
                }
                absoluteIndex -= palette.Entries.Count;
            }
            return false;
        }

        public void Decode(ReachMapVariantReader reader)
        {
            Metadata = new ReachContentItemMetadata();
            Metadata.Decode(reader);
            Name = reader.ReadUnicodeString(128);
            Description = reader.ReadUnicodeString(128);
            Version = (int)reader.ReadUnsigned(8);
            Debug.Assert(Version == 31);
            CacheCRC = reader.ReadUnsigned(32);
            ScenarioPaletteCRC = reader.ReadUnsigned(32);
            PlaceableObjectQuota = (int)reader.ReadUnsigned(9);
            Debug.Assert(PlaceableObjectQuota >= 0 && PlaceableObjectQuota <= MaxQuota);
            MapId = (int)reader.ReadUnsigned(32);
            BuiltIn = reader.ReadBool();
            BuiltFromXml = reader.ReadBool();

            WorldBounds = new RealRectangle3d
            (
                ReinterpretCastFloat(reader.ReadUnsigned(32)),
                ReinterpretCastFloat(reader.ReadUnsigned(32)),
                ReinterpretCastFloat(reader.ReadUnsigned(32)),
                ReinterpretCastFloat(reader.ReadUnsigned(32)),
                ReinterpretCastFloat(reader.ReadUnsigned(32)),
                ReinterpretCastFloat(reader.ReadUnsigned(32))
            );

            MaxBudget = (int)reader.ReadUnsigned(32);
            SpentBudget = (int)reader.ReadUnsigned(32);

            StringTable = new MegaloStringTable();
            StringTable.Decode(reader);

            Objects = new List<ReachVariantObjectDatum>();
            for (int i = 0; i < MaxObjects; i++)
            {
                var datum = new ReachVariantObjectDatum();
                datum.Decode(reader, WorldBounds);
                Objects.Add(datum);
            }

            Quotas = new List<ReachVariantQuota>();
            for (int i = 0; i < MaxQuota; i++)
            {
                var quota = new ReachVariantQuota();
                quota.Decode(reader);
                Quotas.Add(quota);
            }
        }

        static float ReinterpretCastFloat(uint value)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
        }
    }

    public class ReachVariantQuota
    {
        public int MinimumCount;
        public int MaximumCount;
        public int PlacedOnMap;

        public void Decode(BitStream bitstream)
        {
            MinimumCount = (int)bitstream.ReadUnsigned(8);
            MaximumCount = (int)bitstream.ReadUnsigned(8);
            PlacedOnMap = (int)bitstream.ReadUnsigned(8);
        }
    }

    public class ReachVariantObjectDatum
    {
        public ObjectPlacementFlags Flags;
        public int QuotaIndex = -1;
        public int VariantIndex = -1;
        public RealPoint3d Position;
        public RealVector3d Forward;
        public RealVector3d Up;
        public int SpawnRelativeToIndex = -1;
        public ReachVarintMultiplayerObjectProperties Properties;

        public void Decode(ReachMapVariantReader reader, RealRectangle3d worldBounds)
        {
            if (!reader.ReadBool())
                return;

            Flags = (ObjectPlacementFlags)reader.ReadUnsigned(2);

            if (reader.ReadBool())
                QuotaIndex = -1;
            else
                QuotaIndex = (int)reader.ReadUnsigned(8);

            if (reader.ReadBool())
                VariantIndex = -1;
            else
                VariantIndex = (int)reader.ReadUnsigned(5);

            Position = reader.ReadPosition(21, worldBounds);
            reader.ReadAxes(14, 20, out Forward, out Up);

            SpawnRelativeToIndex = (int)reader.ReadUnsigned(10) - 1;

            Properties = new ReachVarintMultiplayerObjectProperties();
            Properties.Decode(reader);
        }

        [Flags]
        public enum ObjectPlacementFlags
        {
            None = 0,
            OccupiedSlot = 1 << 0
        }
    }

    public class ReachVarintMultiplayerObjectProperties
    {
        public ReachMultiplayerObjectBoundary Boundary;
        public int SpawnOrder;
        public int SpawnTime;
        public MultiplayerObjectType Type;
        public int MegaloLabelIndex;
        public VariantPlacementFlags PlacementFlags;
        public MultiplayerTeamDesignator Team = MultiplayerTeamDesignator.Neutral;
        public int PrimaryChangeColorIndex = -1;
        public int SpareClips;
        public int TeleporterChannel;
        public TeleporterPassabilityFlags TeleporterPassability;
        public int LocationNameIndex = -1;

        public void Decode(ReachMapVariantReader reader)
        {
            Boundary = new ReachMultiplayerObjectBoundary();
            Boundary.Decode(reader);

            SpawnOrder = (int)reader.ReadUnsigned(8);
            SpawnTime = (int)reader.ReadUnsigned(8);
            Type = (MultiplayerObjectType)(int)reader.ReadUnsigned(5);

            if (reader.ReadBool())
                MegaloLabelIndex = -1;
            else
                MegaloLabelIndex = (int)reader.ReadUnsigned(8);

            PlacementFlags = (VariantPlacementFlags)reader.ReadUnsigned(8);
            Team = (MultiplayerTeamDesignator)((int)reader.ReadUnsigned(4) - 1);

            if (reader.ReadBool())
                PrimaryChangeColorIndex = -1;
            else
                PrimaryChangeColorIndex = (int)reader.ReadUnsigned(3);

            if (Type == MultiplayerObjectType.Weapon)
            {
                SpareClips = (int)reader.ReadUnsigned(8);
            }
            else
            {
                if (Type <= MultiplayerObjectType.Device)
                    return;
                if (Type <= MultiplayerObjectType.TeleporterReceiver)
                {
                    TeleporterChannel = (int)reader.ReadUnsigned(5);
                    TeleporterPassability = (TeleporterPassabilityFlags)reader.ReadUnsigned(5);
                }
                if (Type != MultiplayerObjectType.NamedLocationArea)
                    return;

                LocationNameIndex = (int)reader.ReadUnsigned(8) - 1;
            }
        }

        public enum MultiplayerObjectType
        {
            Ordinary,
            Weapon,
            Grenade,
            Projectile,
            Powerup,
            Equipment,
            AmmoPack,
            LightLandVehicle,
            HeavyLandVehicle,
            FlyingVehicle,
            Turret,
            Device,
            Teleporter2way,
            TeleporterSender,
            TeleporterReceiver,
            PlayerSpawnLocation,
            PlayerRespawnZone,
            SecondaryObjective,
            PrimaryObjective,
            NamedLocationArea,
            DangerZone,
            Fireteam1RespawnZone,
            Fireteam2RespawnZone,
            Fireteam3RespawnZone,
            Fireteam4RespawnZone,
            SafeVolume,
            KillVolume,
            CinematicCameraPosition,
            MoshEnemySpawnLocation,
            OrdnanceDropPoint,
            TraitZone,
            InitialOrdnanceDropPoint,
            RandomOrdnanceDropPoint,
            ObjectiveOrdnanceDropPoint,
            PersonalOrdnanceDropPoint
        }


        [Flags]
        public enum VariantPlacementFlags
        {
            UniqueSpawn  = 1 << 0,
            NotInitiallyPlaced  = 1 << 1,
            SymmetricPlacement = 1 << 2,
            AsymmetricPlacement = 1 << 3,
            IsShortcut = 1 << 4,
            HideUnlessRequired = 1 << 5,
            PhysicsFixed = 1 << 6, // create at rest
            PhysicsPhased = 1 << 7
        }
    }

    public class ReachMultiplayerObjectBoundary
    {
        public float WidthRadius;
        public float BoxLength;
        public float PositiveHeight;
        public float NegativeHeight;
        public MultiplayerObjectBoundaryShape Shape;

        public bool Decode(BitStream bitstream)
        {
            Shape = (MultiplayerObjectBoundaryShape)bitstream.ReadUnsigned(2);
            if (Shape == MultiplayerObjectBoundaryShape.Sphere)
            {
                WidthRadius = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
            }
            else
            {
                if (Shape == MultiplayerObjectBoundaryShape.Cylinder)
                {
                    WidthRadius = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
                }
                else
                {
                    if (Shape != MultiplayerObjectBoundaryShape.Box)
                    {
                        Debug.Assert(Shape == 0);
                        return true;
                    }


                    WidthRadius = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
                    BoxLength = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
                }

                PositiveHeight = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
                NegativeHeight = bitstream.ReadQuantizedReal(0, 200.0f, 11, false, true);
            }
            return true;
        }
    }
}
