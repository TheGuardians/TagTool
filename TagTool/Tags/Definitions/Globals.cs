using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x600, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x608, MaxVersion = CacheVersion.HaloOnline449175)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x618, MinVersion = CacheVersion.HaloOnline498295, MaxVersion = CacheVersion.HaloOnline571627)]
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x614, MinVersion = CacheVersion.HaloOnline700123)]
    public class Globals : TagStructure
	{
        [TagField(Flags = Padding, Length = 172)]
        public byte[] Unused;

        public GameLanguage Language;

        public List<TagReferenceBlock> HavokObjectCleanupEffects;

        public List<SoundGlobalsDefinition> SoundGlobals;

        public List<AiGlobalsDatum> AiGlobalsOld;

        [TagField(ValidTags = new[] { "aigl" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag AiGlobals;

        public List<DamageTableBlock> DamageTable;

        public uint Unknown45;
        public uint Unknown46;
        public uint Unknown47;

        public List<TagReferenceBlock> SoundsOld;

        public List<CameraGlobalsDefinition> Camera;

        public List<PlayerControlBlock> PlayerControl;

        public List<DifficultyBlock> Difficulty;

        public List<Grenade> Grenades;

        public uint Unknown48;
        public uint Unknown49;
        public uint Unknown50;

        public List<InterfaceTagsBlock> InterfaceTags;

        public uint Unknown51;
        public uint Unknown52;
        public uint Unknown53;

        public uint Unknown54;
        public uint Unknown55;
        public uint Unknown56;

        public List<PlayerInformationBlock> PlayerInformation;

        public List<PlayerRepresentationBlock> PlayerRepresentation;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<PlayerCharacterType> PlayerCharacterTypes;

        public List<FallingDamageBlock> FallingDamage;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public List<UnknownBlock> Unknown60;

        public List<Material> Materials;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public List<ColorBlock> Colors;

        [TagField(ValidTags = new[] { "mulg" })]
        public CachedTag MultiplayerGlobals;

        [TagField(ValidTags = new[] { "smdt" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag SurvivalGlobals;

        [TagField(MinVersion = CacheVersion.HaloOnline498295)]
        public CachedTag ArmorGlobals;

        public List<CinematicAnchorBlock> CinematicAnchors;
        public List<MetagameGlobal> MetagameGlobals;

        [TagField(Length = 12)]
        public LocaleGlobalsBlock[] LocaleGlobals = new LocaleGlobalsBlock[12];

        [TagField(ValidTags = new[] { "rasg" })]
        public CachedTag RasterizerGlobals;

        [TagField(ValidTags = new[] { "cfxs" })]
        public CachedTag DefaultCameraEffect;

        [TagField(ValidTags = new[] { "pdm!" }, MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTag PodiumDefinition;

        [TagField(ValidTags = new[] { "wind" })]
        public CachedTag DefaultWind;

        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag DefaultDamageEffect;

        [TagField(ValidTags = new[] { "cddf" })]
        public CachedTag DefaultCollisionDamage;

        public StringId DefaultWaterMaterial;

        public short UnknownGlobalMaterialIndex;
        public short Unknown265;

        [TagField(ValidTags = new[] { "effg" })]
        public CachedTag EffectGlobals;
        
        [TagField(ValidTags = new[] { "gpdt" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag GameProgressionGlobals;

        [TagField(ValidTags = new[] { "achi" }, MinVersion = CacheVersion.Halo3ODST)]
        public CachedTag AchievementGlobals;
        
        [TagField(ValidTags = new[] { "inpg" }, MinVersion = CacheVersion.HaloOnline106708)]
        public CachedTag InputGlobals;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown266;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown267;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown268;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown269;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagFunction Unknown270 = new TagFunction { Data = new byte[0] };
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown271;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown272;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown273;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown274;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown275;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public List<DamageReportingType> DamageReportingTypes;

        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline571627)]
        public uint Unknown276;
        
		[TagStructure(Size = 0xC)]
		public class DamageTableBlock : TagStructure
		{
			public List<DamageGroup> DamageGroups;

			[TagStructure(Size = 0x10)]
			public class DamageGroup : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;
				public List<ArmorModifier> ArmorModifiers;

				[TagStructure(Size = 0x8)]
				public class ArmorModifier : TagStructure
				{
                    [TagField(Flags = Label)]
                    public StringId Name;
					public float DamageMultiplier;
				}
			}
		}
        
        [TagStructure(Size = 0x88, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x70, MaxVersion = CacheVersion.HaloOnline449175)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloOnline498295)]
        public class PlayerControlBlock : TagStructure
		{
            public float MagnetismFriction;
            public float MagnetismAdhesion;
            public float InconsequentialTargetScale;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown1;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown3;

            [TagField(MaxVersion = CacheVersion.HaloOnline449175)]
            public RealPoint2d CrosshairLocation;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public List<CrosshairLocationBlock> CrosshairLocations;

            //Sprinting

            public float SecondsToStart;
            public float SecondsToFullSpeed;
            public float DecayRate;
            public float FullSpeedMultiplier;
            public float PeggedMagnitude;
            public float PeggedAngularThreshold;
            public float Unknown4;
            public float Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnline498295)]
            public uint Unknown6;

            //Looking

            public float LookDefaultPitchRate;
            public float LookDefaultYawRate;
            public float LookPegThreshold;
            public float LookYawAccelerationTime;
            public float LookYawAccelerationScale;
            public float LookPitchAccelerationTime;
            public float LookPitchAccelerationScale;
            public float LookAutolevelingScale;
            public float Unknown7;
            public float Unknown8;
            public float GravityScale;
            public short Unknown9;
            public short MinimumAutolevelingTicks;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float MinVehicleFlipAngle;
            public List<LookFunctionBlock> LookFunction;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float MinimumActionHoldTime;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown10;

            [TagStructure(Size = 0x24)]
            public class CrosshairLocationBlock : TagStructure
			{
                public float DefaultCrosshairLocationY;
                public uint Unknown;
                public float CenteredCrosshairLocationY;
                public uint Unknown2;
                public uint Unknown3;
                public uint Unknown4;
                public uint Unknown5;
                public uint Unknown6;
                public uint Unknown7;
            }

            [TagStructure]
            public class LookFunctionBlock : TagStructure
			{
                public float Scale;
            }
        }

        [TagStructure(Size = 0x284)]
        public class DifficultyBlock : TagStructure
		{
            public float EasyEnemyDamage;
            public float NormalEnemyDamage;
            public float HardEnemyDamage;
            public float ImpossibleEnemyDamage;
            public float EasyEnemyVitality;
            public float NormalEnemyVitality;
            public float HardEnemyVitality;
            public float ImpossibleEnemyVitality;
            public float EasyEnemyShield;
            public float NormalEnemyShield;
            public float HardEnemyShield;
            public float ImpossibleEnemyShield;
            public float EasyEnemyRecharge;
            public float NormalEnemyRecharge;
            public float HardEnemyRecharge;
            public float ImpossibleEnemyRecharge;
            public float EasyFriendDamage;
            public float NormalFriendDamage;
            public float HardFriendDamage;
            public float ImpossibleFriendDamage;
            public float EasyFriendVitality;
            public float NormalFriendVitality;
            public float HardFriendVitality;
            public float ImpossibleFriendVitality;
            public float EasyFriendShield;
            public float NormalFriendShield;
            public float HardFriendShield;
            public float ImpossibleFriendShield;
            public float EasyFriendRecharge;
            public float NormalFriendRecharge;
            public float HardFriendRecharge;
            public float ImpossibleFriendRecharge;
            public float EasyInfectionForms;
            public float NormalInfectionForms;
            public float HardInfectionForms;
            public float ImpossibleInfectionForms;
            public float EasyUnknown;
            public float NormalUnknown;
            public float HardUnknown;
            public float ImpossibleUnknown;
            public float EasyRateOfFire;
            public float NormalRateOfFire;
            public float HardRateOfFire;
            public float ImpossibleRateOfFire;
            public float EasyProjectileError;
            public float NormalProjectileError;
            public float HardProjectileError;
            public float ImpossibleProjectileError;
            public float EasyBurstError;
            public float NormalBurstError;
            public float HardBurstError;
            public float ImpossibleBurstError;
            public float EasyTargetDelay;
            public float NormalTargetDelay;
            public float HardTargetDelay;
            public float ImpossibleTargetDelay;
            public float EasyBurstSeparation;
            public float NormalBurstSeparation;
            public float HardBurstSeparation;
            public float ImpossibleBurstSeparation;
            public float EasyTargetTracking;
            public float NormalTargetTracking;
            public float HardTargetTracking;
            public float ImpossibleTargetTracking;
            public float EasyTargetLeading;
            public float NormalTargetLeading;
            public float HardTargetLeading;
            public float ImpossibleTargetLeading;
            public float EasyOverchargeChance;
            public float NormalOverchargeChance;
            public float HardOverchargeChance;
            public float ImpossibleOverchargeChance;
            public float EasySpecialFireDelay;
            public float NormalSpecialFireDelay;
            public float HardSpecialFireDelay;
            public float ImpossibleSpecialFireDelay;
            public float EasyGuidanceVsPlayer;
            public float NormalGuidanceVsPlayer;
            public float HardGuidanceVsPlayer;
            public float ImpossibleGuidanceVsPlayer;
            public float EasyMeleeDelayBase;
            public float NormalMeleeDelayBase;
            public float HardMeleeDelayBase;
            public float ImpossibleMeleeDelayBase;
            public float EasyMeleeDelayScale;
            public float NormalMeleeDelayScale;
            public float HardMeleeDelayScale;
            public float ImpossibleMeleeDelayScale;
            public float EasyUnknown2;
            public float NormalUnknown2;
            public float HardUnknown2;
            public float ImpossibleUnknown2;
            public float EasyGrenadeChanceScale;
            public float NormalGrenadeChanceScale;
            public float HardGrenadeChanceScale;
            public float ImpossibleGrenadeChanceScale;
            public float EasyGrenadeTimerScale;
            public float NormalGrenadeTimerScale;
            public float HardGrenadeTimerScale;
            public float ImpossibleGrenadeTimerScale;
            public float EasyUnknown3;
            public float NormalUnknown3;
            public float HardUnknown3;
            public float ImpossibleUnknown3;
            public float EasyUnknown4;
            public float NormalUnknown4;
            public float HardUnknown4;
            public float ImpossibleUnknown4;
            public float EasyUnknown5;
            public float NormalUnknown5;
            public float HardUnknown5;
            public float ImpossibleUnknown5;
            public float EasyMajorUpgradeNormal;
            public float NormalMajorUpgradeNormal;
            public float HardMajorUpgradeNormal;
            public float ImpossibleMajorUpgradeNormal;
            public float EasyMajorUpgradeFew;
            public float NormalMajorUpgradeFew;
            public float HardMajorUpgradeFew;
            public float ImpossibleMajorUpgradeFew;
            public float EasyMajorUpgradeMany;
            public float NormalMajorUpgradeMany;
            public float HardMajorUpgradeMany;
            public float ImpossibleMajorUpgradeMany;
            public float EasyPlayerVehicleRamChance;
            public float NormalPlayerVehicleRamChance;
            public float HardPlayerVehicleRamChance;
            public float ImpossiblePlayerVehicleRamChance;
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
            public uint Unknown20;
            public uint Unknown21;
            public uint Unknown22;
            public uint Unknown23;
            public uint Unknown24;
            public uint Unknown25;
            public uint Unknown26;
            public uint Unknown27;
            public uint Unknown28;
            public uint Unknown29;
            public uint Unknown30;
            public uint Unknown31;
            public uint Unknown32;
            public uint Unknown33;
        }

        [TagStructure(Size = 0x44)]
        public class Grenade : TagStructure
		{
            public short MaximumCount;
            public short Unknown;

            /// <summary>
            /// The effect produced by the grenade when a biped throws it.
            /// </summary>
            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag ThrowingEffect;

            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
            public uint Unknown5;

            /// <summary>
            /// The equipment tag associated with the grenade.
            /// </summary>
            [TagField(Flags = Label, ValidTags = new[] { "eqip" })]
            public CachedTag Equipment;

            /// <summary>
            /// The projectile tag associated with the grenade.
            /// </summary>
            [TagField(ValidTags = new[] { "proj" })]
            public CachedTag Projectile;
        }

        [TagStructure(Size = 0x120, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x12C, MinVersion = CacheVersion.HaloOnline106708)]
        public class InterfaceTagsBlock : TagStructure
		{
            public CachedTag Spinner;
            public CachedTag Obsolete;
            public CachedTag ScreenColorTable;
            public CachedTag HudColorTable;
            public CachedTag EditorColorTable;
            public CachedTag DialogColorTable;
            public CachedTag MotionSensorSweepBitmap;
            public CachedTag MotionSensorSweepBitmapMask;
            public CachedTag MultiplayerHudBitmap;
            public CachedTag HudDigitsDefinition;
            public CachedTag MotionSensorBlipBitmap;
            public CachedTag InterfaceGooMap1;
            public CachedTag InterfaceGooMap2;
            public CachedTag InterfaceGooMap3;
            public CachedTag MainMenuUiGlobals;
            public CachedTag SinglePlayerUiGlobals;
            public CachedTag MultiplayerUiGlobals;
            public CachedTag HudGlobals;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public List<GfxUiString> GfxUiStrings;

            [TagStructure(Size = 0x30)]
            public class GfxUiString : TagStructure
			{
                [TagField(Flags = Label, Length = 32)]
                public string Name;
                public CachedTag Strings;
            }
        }

        [TagStructure(Size = 0xC0, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xC4, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0xCC, MinVersion = CacheVersion.HaloOnline106708)]
        public class PlayerInformationBlock : TagStructure
		{
            public float WalkingSpeed;
            public float RunForward;
            public float RunBackward;
            public float RunSideways;
            public float RunAcceleration;
            public float SneakForward;
            public float SneakBackward;
            public float SneakSideways;
            public float SneakAcceleration;
            public float AirborneAcceleration;
            public RealPoint3d GrenadeOrigin;
            public float StunMovementPenalty;
            public float StunTurningPenalty;
            public float StunJumpingPenalty;
            public Bounds<float> StunTimeRange;
            public Bounds<float> FirstPersonIdleTimeRange;
            public float FirstPersonSkipFraction;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown1;

            public CachedTag Unknown2;
            public CachedTag Unknown3;
            public CachedTag Unknown4;
            public int BinocularsZoomCount;
            public Bounds<float> BinocularZoomRange;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown5;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown6;

            public CachedTag FlashlightOn;
            public CachedTag FlashlightOff;
            public CachedTag DefaultDamageResponse;
        }

        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x6C, MinVersion = CacheVersion.Halo3ODST)]
        public class PlayerRepresentationBlock : TagStructure
		{
            [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST)]
            public StringId Name;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Flags;

            public CachedTag FirstPersonHands;
            public CachedTag FirstPersonBody;
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
            public CachedTag BinocularsZoomInSound;
            public CachedTag BinocularsZoomOutSound;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag CombatDialogue;
        }

        [TagStructure(Size = 0xC)]
        public class PlayerCharacterType : TagStructure
        {
            [TagField(Flags = Label)]
            public StringId Name;

            public FlagsValue Flags;
            public sbyte PlayerInformation;
            public sbyte PlayerControl;
            public sbyte CampaignRepresentation;
            public sbyte MultiplayerRepresentation;
            public sbyte MultiplayerArmorCustomization;
            public sbyte ChudGlobals;
            public sbyte FirstPersonInterface;

            [Flags]
            public enum FlagsValue : byte
            {
                None,
                CanSprint = 1 << 0
            }
        }

        [TagStructure(Size = 0x78)]
        public class FallingDamageBlock : TagStructure
		{
            public Bounds<float> HarmfulFallingDistanceBounds;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag FallingDamage;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag Unknown;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag SoftLanding;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag HardLanding;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag ScriptDamage;

            public float TerminalVelocity;

            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag DistanceDamage;

            public uint Unknown2;
            public uint Unknown3;
            public uint Unknown4;
        }

        [TagStructure(Size = 0xC)]
        public class UnknownBlock : TagStructure //Single block
		{
            public uint Unknown1; //100
            public uint Unknown2; //1
            public uint Unknown3; //1
        }

        [Flags]
        public enum MaterialFlags : ushort
        {
            None = 0,
            Flammable = 1 << 0,
            Biomass = 1 << 1,
            RadXferInterior = 1 << 2
        }
        
        [Flags]
        public enum SweetenerInheritanceFlags : int
        {
            None = 0,
            SoundSmall = 1 << 0,
            SoundMedium = 1 << 1,
            SoundLarge = 1 << 2,
            SoundRolling = 1 << 3,
            SoundGrinding = 1 << 4,
            SoundMeleeSmall = 1 << 5,
            SoundMeleeMedium = 1 << 6,
            SoundMeleeLarge = 1 << 7,
            EffectSmall = 1 << 8,
            EffectMedium = 1 << 9,
            EffectLarge = 1 << 10,
            EffectRolling = 1 << 11,
            EffectGrinding = 1 << 12,
            EffectMelee = 1 << 13,
            WaterRippleSmall = 1 << 14,
            WaterRippleMedium = 1 << 15,
            WaterRippleLarge = 1 << 16
        }
        
        [TagStructure(Size = 0x170, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x178, MinVersion = CacheVersion.Halo3ODST)]
        public class Material : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public StringId ParentName;
            public short RuntimeMaterialIndex;
            public MaterialFlags Flags;

            public StringId GeneralArmor;
            public StringId SpecificArmor;

            public int PhysicsFlags;
            public float Friction;
            public float Restitution;
            public float Density;

            public List<WaterDragProperty> WaterDragProperties;

            [TagField(ValidTags = new[] { "bsdt" })]
            public CachedTag BreakableSurface;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerSmall;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerMedium;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerLarge;

            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag SoundSweetenerRolling;

            [TagField(ValidTags = new[] { "lsnd" })]
            public CachedTag SoundSweetenerGrinding;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerMeleeSmall;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerMeleeMedium;

            [TagField(ValidTags = new[] { "snd!" })]
            public CachedTag SoundSweetenerMeleeLarge;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerSmall;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerMedium;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerLarge;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerRolling;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerGrinding;

            [TagField(ValidTags = new[] { "effe" })]
            public CachedTag EffectSweetenerMelee;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleSmall;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleMedium;

            [TagField(ValidTags = new[] { "rwrd" })]
            public CachedTag WaterRippleLarge;

            public SweetenerInheritanceFlags InheritanceFlags;

            [TagField(ValidTags = new[] { "foot" })]
            public CachedTag MaterialEffects;

            public List<UnderwaterProxy> UnderwaterProxies;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)] public uint Unknown2;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)] public short Unknown3;
            [TagField(MinVersion = CacheVersion.HaloOnline106708)] public short Unknown4;

            [TagStructure(Size = 0x28)]
            public class WaterDragProperty : TagStructure
			{
                public float Unknown;
                public float Unknown2;
                public float Unknown3;
                public float Unknown4;
                public float Unknown5;
                public float Unknown6;
                public float Unknown7;
                public float Unknown8;
                public float Unknown9;
                public float Unknown10;
            }

            [TagStructure(Size = 0xC)]
            public class UnderwaterProxy : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId SurfaceName;
                public StringId SubmergedName;
                public short SurfaceIndex;
                public short SubmergedIndex;
            }
        }

        [TagStructure(Size = 0xC)]
        public class ColorBlock : TagStructure
		{
            public RealRgbColor Color;
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3ODST)]
        public class CinematicAnchorBlock : TagStructure
		{
            public CachedTag CinematicAnchor;

            public float Unknown1;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;
        }

        [TagStructure(Size = 0x48, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x98, MinVersion = CacheVersion.Halo3ODST)]
        public class MetagameGlobal : TagStructure
		{
            public List<Medal> Medals;
            public List<MultiplierBlock> Difficulty;
            public List<MultiplierBlock> PrimarySkulls;
            public List<MultiplierBlock> SecondarySkulls;
            public int Unknown;
            public int DeathPenalty;
            public int BetrayalPenalty;
            public int Unknown2;
            public float MultikillWindow;
            public float EmpWindow;
            
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown3;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown4;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public uint Unknown5;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int FirstWeaponSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int SecondWeaponSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int KillingSpree;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int KillingFrenzy;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int RunningRiot;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Rampage;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Untouchable;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Invincible;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int DoubleKill;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int TripleKill;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Overkill;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killtacular;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killtrocity;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killimanjaro;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killtastrophe;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killpocalypse;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public int Killionaire;

            [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
            public class Medal : TagStructure
			{ 
                public float Multiplier;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int AwardedPoints;

                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public int MedalUptime;

                [TagField(Flags = Label, MinVersion = CacheVersion.Halo3ODST)]
                public StringId EventName;
            }

            [TagStructure(Size = 0x4)]
            public class MultiplierBlock : TagStructure
			{
                public float Multiplier;
            }
        }

        [TagStructure(Size = 0x44)]
        public class LocaleGlobalsBlock : TagStructure
		{
            public uint Unknown1;
            public uint Unknown2;

            public int StringCount;
            public int LocaleTableSize;
            public uint LocaleIndexTableAddress;
            public uint LocaleDataIndexAddress;

            [TagField(Length = 20)]
            public byte[] IndexTableHash;

            [TagField(Length = 20)]
            public byte[] StringDataHash;

            public uint Unknown3;
        }

        [TagStructure(Size = 0x24)]
        public class DamageReportingType : TagStructure
		{
            public short Index;
            public short Version;
            [TagField(Flags = Label, Length = 32)]
            public string Name;
        }
    }
}