using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0xD4, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0xE0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0x1B0, MinVersion = CacheVersion.HaloOnlineED)]
    public class Equipment : Item
    {
        public float UseDuration;
        public float ActivationDelay;
        public short NumberOfUses;
        public EquipmentFlagBits EquipmentFlags;
        public float AiDangerRadius;
        public float AiMinDeployDistance;
        public float AiAwarenessDelay;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<OptionalUnitCameraBlock> OverrideCamera;

        public List<SuperShieldBlock> SuperShield;
        public List<MultiplayerPowerupBlock> MultiplayerPowerup;
        public List<SpawnerBlock> Spawner;
        public List<ProximityMineBlock> ProximityMine;
        public List<MotionTrackerNoiseBlock> MotionTrackerNoise;

        //Probably a unused tagblock
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Unused;

        public List<InvisibilityBlock> Invisibility;
        public List<InvincibilityBlock> Invincibility;
        public List<RegeneratorBlock> Regenerator;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<HealthPackBlock> HealthPack;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<ForcedReloadBlock> ForcedReload;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<ConcussiveBlastBlock> ConcussiveBlast;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<TankModeBlock> TankMode;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<MagPulseBlock> MagPulse;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<HologramBlock> Hologram;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<ReactiveArmorBlock> ReactiveArmor;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<BombRunBlock> BombRun;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<ArmorLockBlock> ArmorLock;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<AdrenalineBlock> Adrenaline;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<LightningStrikeBlock> LightningStrike;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<ScramblerBlock> Scrambler;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<WeaponJammerBlock> WeaponJammer;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<AmmoPackBlock> AmmoPack;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public List<VisionBlock> Vision;

        public CachedTag HudInterface;
        public CachedTag PickupSound;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public CachedTag EmptySound;

        public CachedTag ActivationEffect;
        public CachedTag ActiveEffect;
        public CachedTag DeactivationEffect;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public StringId EnterAnimation;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public StringId IdleAnimation;
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public StringId ExitAnimation;

        [Flags]
        public enum EquipmentFlagBits : ushort
        {
            None,
            PathfindingObstacle = 1 << 0,
            EquipmentIsDangerousToAi = 1 << 1,
            NeverDroppedByAi = 1 << 2,
            ProtectsParentFromAoe = 1 << 3,
            ThirdPersonCameraAlways = 1 << 4,
            UseForcedPrimaryChangeColor = 1 << 5,
            UseForcedSecondaryChangeColor = 1 << 6,
            CanNotBePickedUpByPlayer = 1 << 7,
            IsRemovedFromWorldOnDeactivation = 1 << 8,
            NotDroppedByPlayer = 1 << 9,
            IsDroppedByAi = 1 << 10
        }

        [TagStructure(Size = 0x3C)]
        public class OptionalUnitCameraBlock : TagStructure
		{
            public FlagBits Flags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<CameraTrack> CameraTracks;

            public Angle Unknown2;
            public Angle Unknown3;
            public Angle Unknown4;

            public List<CameraAccelerationBlock> CameraAcceleration;

            [Flags]
            public enum FlagBits : ushort
            {
                None,
                PitchBoundsAbsoluteSpace = 1 << 0,
                OnlyCollidesWithEnvironment = 1 << 1,
                HidesPlayerUnitFromCamera = 1 << 2,
                UseAimingVectorInsteadOfMarkerForward = 1 << 3
            }

            [TagStructure(Size = 0x10)]
            public class CameraTrack : TagStructure
			{
                public CachedTag Track;
            }

            [TagStructure(Size = 0x4C)]
            public class CameraAccelerationBlock : TagStructure
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
        public class SuperShieldBlock : TagStructure
		{
            public uint Unused1;
            public uint Unused2;
            public float ShieldVitality;
            public CachedTag Unused3;
            public CachedTag Unused4;
            public CachedTag Unused5;
        }

        [TagStructure(Size = 0x4)]
        public class MultiplayerPowerupBlock : TagStructure
		{
            public FlavorValue Flavor;

            public enum FlavorValue : int
            {
                RedPowerup,
                BluePowerup,
                YellowPowerup,
                CustomPowerup
            }
        }

        [TagStructure(Size = 0x34)]
        public class SpawnerBlock : TagStructure
		{
            public CachedTag SpawnedObject;
            public CachedTag SpawnedEffect;
            public float SpawnRadius;
            public float SpawnZOffset;
            public float SpawnAreaRadius;
            public float SpawnVelocity;
            public TypeValue Type;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public enum TypeValue : short
            {
                AlongAimingVector,
                CameraPosZPlane,
                FootPosZPlane
            }
        }

        [TagStructure(Size = 0x30)]
        public class ProximityMineBlock : TagStructure
		{
            public CachedTag ExplosionEffect;
            public CachedTag ExplosionDamageEffect;
            public float ArmTime;
            public float SelfDestructTime;
            public float TriggerTime;
            public float TriggerVelocity;
        }

        [TagStructure(Size = 0x10)]
        public class MotionTrackerNoiseBlock : TagStructure
		{
            public float ArmTime;
            public float NoiseRadius;
            public int NoiseCount;
            public float FlashRadius;
        }

        [TagStructure(Size = 0x8)]
        public class InvisibilityBlock : TagStructure
		{
            public float InvisibilityDuration;
            public float InvisibilityFadeTime;
        }

        [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.HaloOnlineED)]
        public class InvincibilityBlock : TagStructure
		{
            public StringId NewPlayerMaterial;
            public short NewPlayerMaterialGlobalIndex;
            public short Unknown;
            public float Unknown2;
            public CachedTag ActivationEffect;
            public CachedTag ActiveEffect;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public CachedTag DeactivationEffect;
        }

        [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x10, MinVersion = CacheVersion.HaloOnlineED)]
        public class RegeneratorBlock : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown;

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            public CachedTag RegeneratingEffect;
        }

        [TagStructure(Size = 0x8)]
        public class HealthPackBlock : TagStructure
		{
            public float HealthGiven;
            public float ShieldsGiven;
        }

        [TagStructure(Size = 0x14)]
        public class ForcedReloadBlock : TagStructure
		{
            public CachedTag Effect;
            public float AmmoPenalty;
        }

        [TagStructure(Size = 0x20)]
        public class ConcussiveBlastBlock : TagStructure
		{
            public CachedTag SecondaryActivationEffect;
            public CachedTag SecondaryDamageEffect;
        }

        [TagStructure(Size = 0x28)]
        public class TankModeBlock : TagStructure
		{
            public StringId NewPlayerMaterial;
            public uint Unknown;
            public float Unknown2;
            public float Unknown3;
            public float Unknown4;
            public uint Unknown5;
            public CachedTag ActiveHud;
        }

        [TagStructure(Size = 0x34)]
        public class MagPulseBlock : TagStructure
		{
            public CachedTag Unknown;
            public CachedTag Unknown2;
            public CachedTag Unknown3;
            public uint Unknown4;
        }

        [TagStructure(Size = 0x6C)]
        public class HologramBlock : TagStructure
		{
            public float Unknown;
            public CachedTag ActiveEffect;
            public CachedTag Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;
            public CachedTag DeathEffect;
            public float Unknown6;
            public float Unknown7;
            public byte[] Function;
            public CachedTag NavPointHud;
        }

        [TagStructure(Size = 0x4C)]
        public class ReactiveArmorBlock : TagStructure
		{
            public float Unknown;
            public float DamageReflectionRatio;
            public uint Unknown3;
            public CachedTag ActivationEffect;
            public CachedTag Unknown5;
            public CachedTag MeleeImpactEffect;
            public CachedTag Unknown7;
        }

        [TagStructure(Size = 0x34)]
        public class BombRunBlock : TagStructure
		{
            public int GrenadeCount;
            public float VelocityBoundsA;
            public float VelocityBoundsB;
            public float HorizontalRandomness;
            public float VerticalRandomness;
            public CachedTag Projectile;
            public CachedTag ThrowSound;
        }

        [TagStructure(Size = 0x20)]
        public class ArmorLockBlock : TagStructure
		{
            public CachedTag CollisionDamage;
            public CachedTag UnknownCollisionDamage;
        }

        [TagStructure(Size = 0x24)]
        public class AdrenalineBlock : TagStructure
		{
            public float SprintRestored;
            public CachedTag ActivationEffect;
            public CachedTag ActiveEffect;
        }

        [TagStructure(Size = 0x14)]
        public class LightningStrikeBlock : TagStructure
		{
            public float MeleeTimeReduction;
            public CachedTag UnknownEffect;
        }

        [TagStructure(Size = 0x24)]
        public class ScramblerBlock : TagStructure
		{
            public uint Unknown;
            public CachedTag Unknown2;
            public int Unknown3;
            public int Unknown4;
            public int Unknown5;
            public int Unknown6;
        }

        [TagStructure(Size = 0x24)]
        public class WeaponJammerBlock : TagStructure
		{
            public uint Unknown;
            public CachedTag Unknown2;
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
            public CachedTag Unknown7;

            [TagStructure(Size = 0x18)]
            public class Weapon : TagStructure
			{
                public StringId Name;
                public CachedTag WeaponObject;
                public int Unknown;
            }
        }

        [TagStructure(Size = 0x20)]
        public class VisionBlock : TagStructure
		{
            public CachedTag ScreenEffect;
            public CachedTag DamageResponse;
        }
    }
}