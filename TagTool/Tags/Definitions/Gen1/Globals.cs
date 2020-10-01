using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x1AC)]
    public class Globals : TagStructure
    {
        [TagField(Length = 0xF8)]
        public byte[] Padding;
        public List<SoundBlock> Sounds;
        public List<CameraBlock> Camera;
        public List<PlayerControlBlock> PlayerControl;
        public List<DifficultyBlock> Difficulty;
        public List<GrenadesBlock> Grenades;
        public List<RasterizerDataBlock> RasterizerData;
        public List<InterfaceTagReferences> InterfaceBitmaps;
        public List<CheatWeaponsBlock> WeaponListUpdateWeaponListEnumInGameGlobalsH;
        public List<CheatPowerupsBlock> CheatPowerups;
        public List<MultiplayerInformationBlock> MultiplayerInformation;
        public List<PlayerInformationBlock> PlayerInformation;
        public List<FirstPersonInterfaceBlock> FirstPersonInterface;
        public List<FallingDamageBlock> FallingDamage;
        public List<MaterialsBlock> Materials;
        public List<PlaylistAutogenerateChoiceBlock> PlaylistMembers;
        
        [TagStructure(Size = 0x10)]
        public class SoundBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
        }
        
        [TagStructure(Size = 0x10)]
        public class CameraBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "trak" })]
            public CachedTag DefaultUnitCameraTrack;
        }
        
        [TagStructure(Size = 0x80)]
        public class PlayerControlBlock : TagStructure
        {
            /// <summary>
            /// how much the crosshair slows over enemies
            /// </summary>
            public float MagnetismFriction;
            /// <summary>
            /// how much the crosshair sticks to enemies
            /// </summary>
            public float MagnetismAdhesion;
            /// <summary>
            /// scales magnetism level for inconsequential targets like infection forms
            /// </summary>
            public float InconsequentialTargetScale;
            [TagField(Length = 0x34)]
            public byte[] Padding;
            /// <summary>
            /// time for a pegged look to reach maximum effect
            /// </summary>
            public float LookAccelerationTime; // seconds
            /// <summary>
            /// maximum effect of a pegged look (scales last value in the look function below)
            /// </summary>
            public float LookAccelerationScale;
            /// <summary>
            /// magnitude of yaw for pegged acceleration to kick in
            /// </summary>
            public float LookPegThreshold01;
            public float LookDefaultPitchRate; // degrees
            public float LookDefaultYawRate; // degrees
            /// <summary>
            /// 1 is fast, 0 is none, 1 will probably be really fast
            /// </summary>
            public float LookAutolevellingScale;
            [TagField(Length = 0x14)]
            public byte[] Padding1;
            /// <summary>
            /// amount of time player needs to hold down ACTION to pick up a new weapon
            /// </summary>
            public short MinimumWeaponSwapTicks;
            /// <summary>
            /// amount of time player needs to move and not look up or down for autolevelling to kick in
            /// </summary>
            public short MinimumAutolevellingTicks;
            /// <summary>
            /// 0 means the vehicle's up vector is along the ground, 90 means the up vector is pointing straight up:degrees
            /// </summary>
            public Angle MinimumAngleForVehicleFlipping;
            public List<LookFunctionBlock> LookFunction;
            
            [TagStructure(Size = 0x4)]
            public class LookFunctionBlock : TagStructure
            {
                public float Scale;
            }
        }
        
        [TagStructure(Size = 0x284)]
        public class DifficultyBlock : TagStructure
        {
            /// <summary>
            /// scale values for enemy health and damage settings
            /// </summary>
            /// <summary>
            /// enemy damage multiplier on easy difficulty
            /// </summary>
            public float EasyEnemyDamage;
            /// <summary>
            /// enemy damage multiplier on normal difficulty
            /// </summary>
            public float NormalEnemyDamage;
            /// <summary>
            /// enemy damage multiplier on hard difficulty
            /// </summary>
            public float HardEnemyDamage;
            /// <summary>
            /// enemy damage multiplier on impossible difficulty
            /// </summary>
            public float ImpossEnemyDamage;
            /// <summary>
            /// enemy maximum body vitality scale on easy difficulty
            /// </summary>
            public float EasyEnemyVitality;
            /// <summary>
            /// enemy maximum body vitality scale on normal difficulty
            /// </summary>
            public float NormalEnemyVitality;
            /// <summary>
            /// enemy maximum body vitality scale on hard difficulty
            /// </summary>
            public float HardEnemyVitality;
            /// <summary>
            /// enemy maximum body vitality scale on impossible difficulty
            /// </summary>
            public float ImpossEnemyVitality;
            /// <summary>
            /// enemy maximum shield vitality scale on easy difficulty
            /// </summary>
            public float EasyEnemyShield;
            /// <summary>
            /// enemy maximum shield vitality scale on normal difficulty
            /// </summary>
            public float NormalEnemyShield;
            /// <summary>
            /// enemy maximum shield vitality scale on hard difficulty
            /// </summary>
            public float HardEnemyShield;
            /// <summary>
            /// enemy maximum shield vitality scale on impossible difficulty
            /// </summary>
            public float ImpossEnemyShield;
            /// <summary>
            /// enemy shield recharge scale on easy difficulty
            /// </summary>
            public float EasyEnemyRecharge;
            /// <summary>
            /// enemy shield recharge scale on normal difficulty
            /// </summary>
            public float NormalEnemyRecharge;
            /// <summary>
            /// enemy shield recharge scale on hard difficulty
            /// </summary>
            public float HardEnemyRecharge;
            /// <summary>
            /// enemy shield recharge scale on impossible difficulty
            /// </summary>
            public float ImpossEnemyRecharge;
            /// <summary>
            /// friend damage multiplier on easy difficulty
            /// </summary>
            public float EasyFriendDamage;
            /// <summary>
            /// friend damage multiplier on normal difficulty
            /// </summary>
            public float NormalFriendDamage;
            /// <summary>
            /// friend damage multiplier on hard difficulty
            /// </summary>
            public float HardFriendDamage;
            /// <summary>
            /// friend damage multiplier on impossible difficulty
            /// </summary>
            public float ImpossFriendDamage;
            /// <summary>
            /// friend maximum body vitality scale on easy difficulty
            /// </summary>
            public float EasyFriendVitality;
            /// <summary>
            /// friend maximum body vitality scale on normal difficulty
            /// </summary>
            public float NormalFriendVitality;
            /// <summary>
            /// friend maximum body vitality scale on hard difficulty
            /// </summary>
            public float HardFriendVitality;
            /// <summary>
            /// friend maximum body vitality scale on impossible difficulty
            /// </summary>
            public float ImpossFriendVitality;
            /// <summary>
            /// friend maximum shield vitality scale on easy difficulty
            /// </summary>
            public float EasyFriendShield;
            /// <summary>
            /// friend maximum shield vitality scale on normal difficulty
            /// </summary>
            public float NormalFriendShield;
            /// <summary>
            /// friend maximum shield vitality scale on hard difficulty
            /// </summary>
            public float HardFriendShield;
            /// <summary>
            /// friend maximum shield vitality scale on impossible difficulty
            /// </summary>
            public float ImpossFriendShield;
            /// <summary>
            /// friend shield recharge scale on easy difficulty
            /// </summary>
            public float EasyFriendRecharge;
            /// <summary>
            /// friend shield recharge scale on normal difficulty
            /// </summary>
            public float NormalFriendRecharge;
            /// <summary>
            /// friend shield recharge scale on hard difficulty
            /// </summary>
            public float HardFriendRecharge;
            /// <summary>
            /// friend shield recharge scale on impossible difficulty
            /// </summary>
            public float ImpossFriendRecharge;
            /// <summary>
            /// toughness of infection forms (may be negative) on easy difficulty
            /// </summary>
            public float EasyInfectionForms;
            /// <summary>
            /// toughness of infection forms (may be negative) on normal difficulty
            /// </summary>
            public float NormalInfectionForms;
            /// <summary>
            /// toughness of infection forms (may be negative) on hard difficulty
            /// </summary>
            public float HardInfectionForms;
            /// <summary>
            /// toughness of infection forms (may be negative) on impossible difficulty
            /// </summary>
            public float ImpossInfectionForms;
            [TagField(Length = 0x10)]
            public byte[] Padding;
            /// <summary>
            /// difficulty-affecting values for enemy ranged combat settings
            /// </summary>
            /// <summary>
            /// enemy rate of fire scale on easy difficulty
            /// </summary>
            public float EasyRateOfFire;
            /// <summary>
            /// enemy rate of fire scale on normal difficulty
            /// </summary>
            public float NormalRateOfFire;
            /// <summary>
            /// enemy rate of fire scale on hard difficulty
            /// </summary>
            public float HardRateOfFire;
            /// <summary>
            /// enemy rate of fire scale on impossible difficulty
            /// </summary>
            public float ImpossRateOfFire;
            /// <summary>
            /// enemy projectile error scale, as a fraction of their base firing error. on easy difficulty
            /// </summary>
            public float EasyProjectileError;
            /// <summary>
            /// enemy projectile error scale, as a fraction of their base firing error. on normal difficulty
            /// </summary>
            public float NormalProjectileError;
            /// <summary>
            /// enemy projectile error scale, as a fraction of their base firing error. on hard difficulty
            /// </summary>
            public float HardProjectileError;
            /// <summary>
            /// enemy projectile error scale, as a fraction of their base firing error. on impossible difficulty
            /// </summary>
            public float ImpossProjectileError;
            /// <summary>
            /// enemy burst error scale; reduces intra-burst shot distance. on easy difficulty
            /// </summary>
            public float EasyBurstError;
            /// <summary>
            /// enemy burst error scale; reduces intra-burst shot distance. on normal difficulty
            /// </summary>
            public float NormalBurstError;
            /// <summary>
            /// enemy burst error scale; reduces intra-burst shot distance. on hard difficulty
            /// </summary>
            public float HardBurstError;
            /// <summary>
            /// enemy burst error scale; reduces intra-burst shot distance. on impossible difficulty
            /// </summary>
            public float ImpossBurstError;
            /// <summary>
            /// enemy new-target delay scale factor. on easy difficulty
            /// </summary>
            public float EasyNewTargetDelay;
            /// <summary>
            /// enemy new-target delay scale factor. on normal difficulty
            /// </summary>
            public float NormalNewTargetDelay;
            /// <summary>
            /// enemy new-target delay scale factor. on hard difficulty
            /// </summary>
            public float HardNewTargetDelay;
            /// <summary>
            /// enemy new-target delay scale factor. on impossible difficulty
            /// </summary>
            public float ImpossNewTargetDelay;
            /// <summary>
            /// delay time between bursts scale factor for enemies. on easy difficulty
            /// </summary>
            public float EasyBurstSeparation;
            /// <summary>
            /// delay time between bursts scale factor for enemies. on normal difficulty
            /// </summary>
            public float NormalBurstSeparation;
            /// <summary>
            /// delay time between bursts scale factor for enemies. on hard difficulty
            /// </summary>
            public float HardBurstSeparation;
            /// <summary>
            /// delay time between bursts scale factor for enemies. on impossible difficulty
            /// </summary>
            public float ImpossBurstSeparation;
            /// <summary>
            /// additional target tracking fraction for enemies. on easy difficulty
            /// </summary>
            public float EasyTargetTracking;
            /// <summary>
            /// additional target tracking fraction for enemies. on normal difficulty
            /// </summary>
            public float NormalTargetTracking;
            /// <summary>
            /// additional target tracking fraction for enemies. on hard difficulty
            /// </summary>
            public float HardTargetTracking;
            /// <summary>
            /// additional target tracking fraction for enemies. on impossible difficulty
            /// </summary>
            public float ImpossTargetTracking;
            /// <summary>
            /// additional target leading fraction for enemies. on easy difficulty
            /// </summary>
            public float EasyTargetLeading;
            /// <summary>
            /// additional target leading fraction for enemies. on normal difficulty
            /// </summary>
            public float NormalTargetLeading;
            /// <summary>
            /// additional target leading fraction for enemies. on hard difficulty
            /// </summary>
            public float HardTargetLeading;
            /// <summary>
            /// additional target leading fraction for enemies. on impossible difficulty
            /// </summary>
            public float ImpossTargetLeading;
            /// <summary>
            /// overcharge chance scale factor for enemies. on easy difficulty
            /// </summary>
            public float EasyOverchargeChance;
            /// <summary>
            /// overcharge chance scale factor for enemies. on normal difficulty
            /// </summary>
            public float NormalOverchargeChance;
            /// <summary>
            /// overcharge chance scale factor for enemies. on hard difficulty
            /// </summary>
            public float HardOverchargeChance;
            /// <summary>
            /// overcharge chance scale factor for enemies. on impossible difficulty
            /// </summary>
            public float ImpossOverchargeChance;
            /// <summary>
            /// delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on easy difficulty
            /// </summary>
            public float EasySpecialFireDelay;
            /// <summary>
            /// delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on normal difficulty
            /// </summary>
            public float NormalSpecialFireDelay;
            /// <summary>
            /// delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on hard difficulty
            /// </summary>
            public float HardSpecialFireDelay;
            /// <summary>
            /// delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on impossible difficulty
            /// </summary>
            public float ImpossSpecialFireDelay;
            /// <summary>
            /// guidance velocity scale factor for all projectiles targeted on a player. on easy difficulty
            /// </summary>
            public float EasyGuidanceVsPlayer;
            /// <summary>
            /// guidance velocity scale factor for all projectiles targeted on a player. on normal difficulty
            /// </summary>
            public float NormalGuidanceVsPlayer;
            /// <summary>
            /// guidance velocity scale factor for all projectiles targeted on a player. on hard difficulty
            /// </summary>
            public float HardGuidanceVsPlayer;
            /// <summary>
            /// guidance velocity scale factor for all projectiles targeted on a player. on impossible difficulty
            /// </summary>
            public float ImpossGuidanceVsPlayer;
            /// <summary>
            /// delay period added to all melee attacks, even when berserk. on easy difficulty
            /// </summary>
            public float EasyMeleeDelayBase;
            /// <summary>
            /// delay period added to all melee attacks, even when berserk. on normal difficulty
            /// </summary>
            public float NormalMeleeDelayBase;
            /// <summary>
            /// delay period added to all melee attacks, even when berserk. on hard difficulty
            /// </summary>
            public float HardMeleeDelayBase;
            /// <summary>
            /// delay period added to all melee attacks, even when berserk. on impossible difficulty
            /// </summary>
            public float ImpossMeleeDelayBase;
            /// <summary>
            /// multiplier for all existing non-berserk melee delay times. on easy difficulty
            /// </summary>
            public float EasyMeleeDelayScale;
            /// <summary>
            /// multiplier for all existing non-berserk melee delay times. on normal difficulty
            /// </summary>
            public float NormalMeleeDelayScale;
            /// <summary>
            /// multiplier for all existing non-berserk melee delay times. on hard difficulty
            /// </summary>
            public float HardMeleeDelayScale;
            /// <summary>
            /// multiplier for all existing non-berserk melee delay times. on impossible difficulty
            /// </summary>
            public float ImpossMeleeDelayScale;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            /// <summary>
            /// difficulty-affecting values for enemy grenade behavior
            /// </summary>
            /// <summary>
            /// scale factor affecting the desicions to throw a grenade. on easy difficulty
            /// </summary>
            public float EasyGrenadeChanceScale;
            /// <summary>
            /// scale factor affecting the desicions to throw a grenade. on normal difficulty
            /// </summary>
            public float NormalGrenadeChanceScale;
            /// <summary>
            /// scale factor affecting the desicions to throw a grenade. on hard difficulty
            /// </summary>
            public float HardGrenadeChanceScale;
            /// <summary>
            /// scale factor affecting the desicions to throw a grenade. on impossible difficulty
            /// </summary>
            public float ImpossGrenadeChanceScale;
            /// <summary>
            /// scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on easy
            /// difficulty
            /// </summary>
            public float EasyGrenadeTimerScale;
            /// <summary>
            /// scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on normal
            /// difficulty
            /// </summary>
            public float NormalGrenadeTimerScale;
            /// <summary>
            /// scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on hard
            /// difficulty
            /// </summary>
            public float HardGrenadeTimerScale;
            /// <summary>
            /// scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on
            /// impossible difficulty
            /// </summary>
            public float ImpossGrenadeTimerScale;
            [TagField(Length = 0x10)]
            public byte[] Padding2;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(Length = 0x10)]
            public byte[] Padding4;
            /// <summary>
            /// difficulty-affecting values for enemy placement
            /// </summary>
            /// <summary>
            /// fraction of actors upgraded to their major variant. on easy difficulty
            /// </summary>
            public float EasyMajorUpgradeNormal;
            /// <summary>
            /// fraction of actors upgraded to their major variant. on normal difficulty
            /// </summary>
            public float NormalMajorUpgradeNormal;
            /// <summary>
            /// fraction of actors upgraded to their major variant. on hard difficulty
            /// </summary>
            public float HardMajorUpgradeNormal;
            /// <summary>
            /// fraction of actors upgraded to their major variant. on impossible difficulty
            /// </summary>
            public float ImpossMajorUpgradeNormal;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = normal. on easy difficulty
            /// </summary>
            public float EasyMajorUpgradeFew;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = normal. on normal difficulty
            /// </summary>
            public float NormalMajorUpgradeFew;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = normal. on hard difficulty
            /// </summary>
            public float HardMajorUpgradeFew;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = normal. on impossible difficulty
            /// </summary>
            public float ImpossMajorUpgradeFew;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = many. on easy difficulty
            /// </summary>
            public float EasyMajorUpgradeMany;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = many. on normal difficulty
            /// </summary>
            public float NormalMajorUpgradeMany;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = many. on hard difficulty
            /// </summary>
            public float HardMajorUpgradeMany;
            /// <summary>
            /// fraction of actors upgraded to their major variant when mix = many. on impossible difficulty
            /// </summary>
            public float ImpossMajorUpgradeMany;
            [TagField(Length = 0x10)]
            public byte[] Padding5;
            [TagField(Length = 0x10)]
            public byte[] Padding6;
            [TagField(Length = 0x10)]
            public byte[] Padding7;
            [TagField(Length = 0x10)]
            public byte[] Padding8;
            [TagField(Length = 0x54)]
            public byte[] Padding9;
        }
        
        [TagStructure(Size = 0x44)]
        public class GrenadesBlock : TagStructure
        {
            public short MaximumCount;
            public short MpSpawnDefault;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ThrowingEffect;
            [TagField(ValidTags = new [] { "grhi" })]
            public CachedTag HudInterface;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Equipment;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
        }
        
        [TagStructure(Size = 0x1AC)]
        public class RasterizerDataBlock : TagStructure
        {
            /// <summary>
            /// Used internally by the rasterizer. (Do not change unless you know what you're doing!)
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DistanceAttenuation;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag VectorNormalization;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag AtmosphericFogDensity;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag PlanarFogDensity;
            /// <summary>
            /// used for shadows
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag LinearCornerFade;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ActiveCamouflageDistortion;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Glow;
            [TagField(Length = 0x3C)]
            public byte[] Padding;
            /// <summary>
            /// Used internally by the rasterizer - additive, multiplicative, detail, vector. (Do not change ever, period.)
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Default2d;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Default3d;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DefaultCubeMap;
            /// <summary>
            /// Used internally by the rasterizer. (Used by Bernie's experimental shaders.)
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Test0;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Test1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Test2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Test3;
            /// <summary>
            /// Used in cinematics.
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag VideoScanlineMap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag VideoNoiseMap;
            [TagField(Length = 0x34)]
            public byte[] Padding1;
            public FlagsValue Flags;
            [TagField(Length = 0x2)]
            public byte[] Padding2;
            public float RefractionAmount; // pixels
            public float DistanceFalloff;
            public RealRgbColor TintColor;
            public float HyperStealthRefraction; // pixels
            public float HyperStealthDistanceFalloff;
            public RealRgbColor HyperStealthTintColor;
            /// <summary>
            /// The PC can't use 3D textures, so we use this instead.
            /// </summary>
            /// <summary>
            /// the pc can't use 3d textures, so we use this instead
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DistanceAttenuation2d;
            
            public enum FlagsValue : ushort
            {
                TintEdgeDensity
            }
        }
        
        [TagStructure(Size = 0x130)]
        public class InterfaceTagReferences : TagStructure
        {
            [TagField(ValidTags = new [] { "font" })]
            public CachedTag FontSystem;
            [TagField(ValidTags = new [] { "font" })]
            public CachedTag FontTerminal;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag ScreenColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag HudColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag EditorColorTable;
            [TagField(ValidTags = new [] { "colo" })]
            public CachedTag DialogColorTable;
            [TagField(ValidTags = new [] { "hudg" })]
            public CachedTag HudGlobals;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorSweepBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorSweepBitmapMask;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MultiplayerHudBitmap;
            [TagField(ValidTags = new [] { "str#" })]
            public CachedTag Localization;
            [TagField(ValidTags = new [] { "hud#" })]
            public CachedTag HudDigitsDefinition;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag MotionSensorBlipBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag InterfaceGooMap3;
            [TagField(Length = 0x30)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x10)]
        public class CheatWeaponsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0x10)]
        public class CheatPowerupsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Powerup;
        }
        
        [TagStructure(Size = 0xA0)]
        public class MultiplayerInformationBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Flag;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            public List<VehiclesBlock> Vehicles;
            [TagField(ValidTags = new [] { "shdr" })]
            public CachedTag HillShader;
            [TagField(ValidTags = new [] { "shdr" })]
            public CachedTag FlagShader;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Ball;
            public List<SoundsBlock> Sounds;
            [TagField(Length = 0x38)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x10)]
            public class VehiclesBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "vehi" })]
                public CachedTag Vehicle;
            }
            
            [TagStructure(Size = 0x10)]
            public class SoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
            }
        }
        
        [TagStructure(Size = 0xF4)]
        public class PlayerInformationBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            [TagField(Length = 0x1C)]
            public byte[] Padding;
            public float WalkingSpeed; // world units per second
            public float DoubleSpeedMultiplier; // [1.0,+inf]
            public float RunForward; // world units per second
            public float RunBackward; // world units per second
            public float RunSideways; // world units per second
            public float RunAcceleration; // world units per second squared
            public float SneakForward; // world units per second
            public float SneakBackward; // world units per second
            public float SneakSideways; // world units per second
            public float SneakAcceleration; // world units per second squared
            public float AirborneAcceleration; // world units per second squared
            public float SpeedMultiplier; //  multiplayer only
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            public RealPoint3d GrenadeOrigin;
            [TagField(Length = 0xC)]
            public byte[] Padding2;
            /// <summary>
            /// 1.0 prevents moving while stunned
            /// </summary>
            public float StunMovementPenalty; // [0,1]
            /// <summary>
            /// 1.0 prevents turning while stunned
            /// </summary>
            public float StunTurningPenalty; // [0,1]
            /// <summary>
            /// 1.0 prevents jumping while stunned
            /// </summary>
            public float StunJumpingPenalty; // [0,1]
            /// <summary>
            /// all stunning damage will last for at least this long
            /// </summary>
            public float MinimumStunTime; // seconds
            /// <summary>
            /// no stunning damage will last for longer than this
            /// </summary>
            public float MaximumStunTime; // seconds
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            public Bounds<float> FirstPersonIdleTime; // seconds
            public float FirstPersonSkipFraction; // [0,1]
            [TagField(Length = 0x10)]
            public byte[] Padding4;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag CoopRespawnEffect;
            [TagField(Length = 0x2C)]
            public byte[] Padding5;
        }
        
        [TagStructure(Size = 0xC0)]
        public class FirstPersonInterfaceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mod2" })]
            public CachedTag FirstPersonHands;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BaseBitmap;
            [TagField(ValidTags = new [] { "metr" })]
            public CachedTag ShieldMeter;
            public Point2d ShieldMeterOrigin;
            [TagField(ValidTags = new [] { "metr" })]
            public CachedTag BodyMeter;
            public Point2d BodyMeterOrigin;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag NightVisionOffOnEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag NightVisionOnOffEffect;
            [TagField(Length = 0x58)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x98)]
        public class FallingDamageBlock : TagStructure
        {
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public Bounds<float> HarmfulFallingDistance; // world units
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FallingDamage;
            [TagField(Length = 0x8)]
            public byte[] Padding1;
            public float MaximumFallingDistance; // world units
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag DistanceDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag VehicleEnvironemtnCollisionDamageEffect;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag VehicleKilledUnitDamageEffect;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag VehicleCollisionDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FlamingDeathDamage;
            [TagField(Length = 0x1C)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x374)]
        public class MaterialsBlock : TagStructure
        {
            [TagField(Length = 0x64)]
            public byte[] Padding;
            [TagField(Length = 0x30)]
            public byte[] Padding1;
            /// <summary>
            /// the following fields modify the way a vehicle drives over terrain of this material type.
            /// </summary>
            /// <summary>
            /// fraction of original velocity parallel to the ground after one tick
            /// </summary>
            public float GroundFrictionScale;
            /// <summary>
            /// cosine of angle at which friction falls off
            /// </summary>
            public float GroundFrictionNormalK1Scale;
            /// <summary>
            /// cosine of angle at which friction is zero
            /// </summary>
            public float GroundFrictionNormalK0Scale;
            /// <summary>
            /// depth a point mass rests in the ground
            /// </summary>
            public float GroundDepthScale;
            /// <summary>
            /// fraction of original velocity perpendicular to the ground after one tick
            /// </summary>
            public float GroundDampFractionScale;
            [TagField(Length = 0x4C)]
            public byte[] Padding2;
            [TagField(Length = 0x1E0)]
            public byte[] Padding3;
            public float MaximumVitality;
            [TagField(Length = 0x8)]
            public byte[] Padding4;
            [TagField(Length = 0x4)]
            public byte[] Padding5;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag Effect;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
            [TagField(Length = 0x18)]
            public byte[] Padding6;
            public List<BreakableSurfaceParticleEffectBlock> ParticleEffects;
            [TagField(Length = 0x3C)]
            public byte[] Padding7;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag MeleeHitSound;
            
            [TagStructure(Size = 0x80)]
            public class BreakableSurfaceParticleEffectBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "part" })]
                public CachedTag ParticleType;
                public FlagsValue Flags;
                public float Density; // world units
                /// <summary>
                /// scales initial velocity
                /// </summary>
                public Bounds<float> VelocityScale;
                [TagField(Length = 0x4)]
                public byte[] Padding;
                public Bounds<Angle> AngularVelocity; // degrees per second
                [TagField(Length = 0x8)]
                public byte[] Padding1;
                /// <summary>
                /// particle radius
                /// </summary>
                public Bounds<float> Radius; // world units
                [TagField(Length = 0x8)]
                public byte[] Padding2;
                public RealArgbColor TintLowerBound;
                public RealArgbColor TintUpperBound;
                [TagField(Length = 0x1C)]
                public byte[] Padding3;
                
                public enum FlagsValue : uint
                {
                    InterpolateColorInHsv,
                    MoreColors
                }
            }
        }
        
        [TagStructure(Size = 0x94)]
        public class PlaylistAutogenerateChoiceBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string MapName;
            [TagField(Length = 32)]
            public string GameVariant;
            public int MinimumExperience;
            public int MaximumExperience;
            public int MinimumPlayerCount;
            public int MaximumPlayerCount;
            public int Rating;
            [TagField(Length = 0x40)]
            public byte[] Padding;
        }
    }
}

