using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0xD4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0xE0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0x1B0, MinVersion = CacheVersion.HaloOnline106708)]
    public class Equipment : Item
    {
        public float UseDuration;
        public uint Unknown8;
        public short NumberOfUses;
        public ushort Flags3;
        public float Unknown9;
        public float Unknown10;
        public float Unknown11;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<EquipmentCameraBlock> EquipmentCamera;

        public List<HealthPackBlock> HealthPack;
        public List<PowerupBlock> Powerup;
        public List<ObjectCreationBlock> ObjectCreation;
        public List<DestructionBlock> Destruction;
        public List<RadarManipulationBlock> RadarManipulation;
        //Probably a unused tagblock
        public uint Unknown12;
        public uint Unknown13;
        public uint Unknown14;
        public List<InvisibilityBlock> Invisibility;
        public List<InvincibilityBlock> Invincibility;
        public List<RegeneratorBlock> Regenerator;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<NewHealthPackBlock> NewHealthPack;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ForcedReloadBlock> ForcedReload;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ConcussiveBlastBlock> ConcussiveBlast;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<TankModeBlock> TankMode;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<MagPulseBlock> MagPulse;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<HologramBlock> Hologram;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ReactiveArmorBlock> ReactiveArmor;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<BombRunBlock> BombRun;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ArmorLockBlock> ArmorLock;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<AdrenalineBlock> Adrenaline;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<LightningStrikeBlock> LightningStrike;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<ScramblerBlock> Scrambler;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<WeaponJammerBlock> WeaponJammer;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<AmmoPackBlock> AmmoPack;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<VisionBlock> Vision;

        public CachedTagInstance HudInterface;
        public CachedTagInstance PickupSound;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTagInstance EmptySound;

        public CachedTagInstance ActivationEffect;
        public CachedTagInstance ActiveEffect;
        public CachedTagInstance DeactivationEffect;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId EnterAnimation;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId IdleAnimation;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public StringId ExitAnimation;

        [TagStructure(Size = 0x3C)]
        public class EquipmentCameraBlock : TagStructure
		{
            public short Flags;
            public short Unknown;
            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Angle PitchRangeMin;
            public Angle PitchRangeMax;
            public List<CameraTrack> CameraTracks;
            public Angle Unknown2;
            public Angle Unknown3;
            public Angle Unknown4;
            public List<UnknownBlock> Unknown5;

            [TagStructure(Size = 0x10)]
            public class CameraTrack : TagStructure
			{
                public CachedTagInstance Track;
            }

            [TagStructure(Size = 0x4C)]
            public class UnknownBlock : TagStructure
			{
                public uint Unknown;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
                public uint Unknown8;
                public uint Unknown9;
                public uint Unknown10;
                public uint Unknown11;
                public uint Unknown12;
                public uint Unknown13;
                public uint Unknown14;
                public uint Unknown15;
                public uint Unknown16;
                public uint Unknown17;
                public uint Unknown18;
                public uint Unknown19;
            }
        }

        [TagStructure(Size = 0x3C)]
        public class HealthPackBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public float ShieldsGiven;
            public CachedTagInstance Unknown3;
            public CachedTagInstance Unknown4;
            public CachedTagInstance Unknown5;
        }

        [TagStructure(Size = 0x4)]
        public class PowerupBlock : TagStructure
		{
            public PowerupTraitSetValue PowerupTraitSet;

            public enum PowerupTraitSetValue : int
            {
                Red,
                Blue,
                Yellow,
            }
        }

        [TagStructure(Size = 0x34)]
        public class ObjectCreationBlock : TagStructure
		{
            public CachedTagInstance Object;
            public CachedTagInstance Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public float ObjectForce;
            public uint Unknown5;
        }

        [TagStructure(Size = 0x30)]
        public class DestructionBlock : TagStructure
		{
            public CachedTagInstance DestroyEffect;
            public CachedTagInstance DestroyDamageEffect;
            public uint Unknown;
            public float SelfDestructionTime;
            public uint Unknown2;
            public uint Unknown3;
        }

        [TagStructure(Size = 0x10)]
        public class RadarManipulationBlock : TagStructure
		{
            public uint Unknown;
            public float FakeBlipRadius;
            public int FakeBlipCount;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x8)]
        public class InvisibilityBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
        }

        [TagStructure(Size = 0x2C)]
        public class InvincibilityBlock : TagStructure
		{
            public StringId NewPlayerMaterial;
            public short NewPlayerMaterialGlobalIndex;
            public short Unknown;
            public uint Unknown2;
            public CachedTagInstance Unknown3;
            public CachedTagInstance Unknown4;
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloOnline106708)]
        public class RegeneratorBlock : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance RegeneratingEffect;
        }

        [TagStructure(Size = 0x8)]
        public class NewHealthPackBlock : TagStructure
		{
            public float Unknown1;
            public float Unknown2;
        }

        [TagStructure(Size = 0x14)]
        public class ForcedReloadBlock : TagStructure
		{
            public CachedTagInstance Effect;
            public uint Unknown;
        }

        [TagStructure(Size = 0x20)]
        public class ConcussiveBlastBlock : TagStructure
		{
            public CachedTagInstance Unknown;
            public CachedTagInstance Unknown2;
        }

        [TagStructure(Size = 0x28)]
        public class TankModeBlock : TagStructure
		{
            public StringId NewPlayerMaterial;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public CachedTagInstance ActiveHud;
        }

        [TagStructure(Size = 0x34)]
        public class MagPulseBlock : TagStructure
		{
            public CachedTagInstance Unknown;
            public CachedTagInstance Unknown2;
            public CachedTagInstance Unknown3;
            public uint Unknown4;
        }

        [TagStructure(Size = 0x6C)]
        public class HologramBlock : TagStructure
		{
            public uint Unknown;
            public CachedTagInstance ActiveEffect;
            public CachedTagInstance Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public CachedTagInstance DeathEffect;
            public uint Unknown6;
            public uint Unknown7;
            public byte[] Function;
            public CachedTagInstance NavPointHud;
        }

        [TagStructure(Size = 0x4C)]
        public class ReactiveArmorBlock : TagStructure
		{
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public CachedTagInstance Unknown4;
            public CachedTagInstance Unknown5;
            public CachedTagInstance Unknown6;
            public CachedTagInstance Unknown7;
        }

        [TagStructure(Size = 0x34)]
        public class BombRunBlock : TagStructure
		{
            public int Unknown;
            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public CachedTagInstance Projectile;
            public CachedTagInstance ThrowSound;
        }

        [TagStructure(Size = 0x20)]
        public class ArmorLockBlock : TagStructure
		{
            public CachedTagInstance Unknown;
            public CachedTagInstance Unknown2;
        }

        [TagStructure(Size = 0x24)]
        public class AdrenalineBlock : TagStructure
		{
            public uint Unknown;
            public CachedTagInstance Unknown2;
            public CachedTagInstance Unknown3;
        }

        [TagStructure(Size = 0x14)]
        public class LightningStrikeBlock : TagStructure
		{
            public uint Unknown;
            public CachedTagInstance Unknown2;
        }

        [TagStructure(Size = 0x24)]
        public class ScramblerBlock : TagStructure
		{
            public uint Unknown;
            public CachedTagInstance Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
        }

        [TagStructure(Size = 0x24)]
        public class WeaponJammerBlock : TagStructure
		{
            public uint Unknown;
            public CachedTagInstance Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
        }

        [TagStructure(Size = 0x34)]
        public class AmmoPackBlock : TagStructure
		{
            public uint Unknown;
            public int Unknown2;
            public int Unknown3;
            public uint Unknown4;
            public int Unknown5;
            public int Unknown6;
            public List<Weapon> Weapons;
            public CachedTagInstance Unknown7;

            [TagStructure(Size = 0x18)]
            public class Weapon : TagStructure
			{
                public StringId Name;
                public CachedTagInstance WeaponObject;
                public int Unknown;
            }
        }

        [TagStructure(Size = 0x20)]
        public class VisionBlock : TagStructure
		{
            public CachedTagInstance ScreenEffect;
            public CachedTagInstance Unknown;
        }
    }
}
