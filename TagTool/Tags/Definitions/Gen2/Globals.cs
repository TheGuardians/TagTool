using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x284)]
    public class Globals : TagStructure
    {
        [TagField(Length = 0xAC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public LanguageValue Language;
        public List<HavokCleanupResourcesBlock> HavokCleanupResources;
        public List<CollisionDamageBlock> CollisionDamage;
        public List<SoundGlobalsBlock> SoundGlobals;
        public List<AiGlobalsBlock> AiGlobals;
        public List<GameGlobalsDamageBlock> DamageTable;
        public List<GNullBlock> Unknown;
        public List<SoundBlock> Sounds;
        public List<CameraBlock> Camera;
        public List<PlayerControlBlock> PlayerControl;
        public List<DifficultyBlock> Difficulty;
        public List<GrenadesBlock> Grenades;
        public List<RasterizerDataBlock> RasterizerData;
        public List<InterfaceTagReferences> InterfaceTags;
        public List<CheatWeaponsBlock> WeaponListUpdateWeaponListEnumInGameGlobalsH;
        public List<CheatPowerupsBlock> CheatPowerups;
        public List<MultiplayerInformationBlock> MultiplayerInformation;
        public List<PlayerInformationBlock> PlayerInformation;
        public List<PlayerRepresentationBlock> PlayerRepresentation;
        public List<FallingDamageBlock> FallingDamage;
        public List<OldMaterialsBlock> OldMaterials;
        public List<MaterialsBlock> Materials;
        public List<MultiplayerUiBlock> MultiplayerUi;
        public List<MultiplayerColorBlock> ProfileColors;
        [TagField(ValidTags = new [] { "mulg" })]
        public CachedTag MultiplayerGlobals;
        public List<RuntimeLevelsDefinitionBlock> RuntimeLevelData;
        public List<UiLevelsDefinitionBlock> UiLevelData;
        [TagField(ValidTags = new [] { "gldf" })]
        public CachedTag DefaultGlobalLighting;
        [TagField(Length = 0xFC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        
        public enum LanguageValue : int
        {
            English,
            Japanese,
            German,
            French,
            Spanish,
            Italian,
            Korean,
            Chinese,
            Portuguese
        }
        
        [TagStructure(Size = 0x8)]
        public class HavokCleanupResourcesBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ObjectCleanupEffect;
        }
        
        [TagStructure(Size = 0x48)]
        public class CollisionDamageBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag CollisionDamage;
            /// <summary>
            /// 0-oo
            /// </summary>
            public float MinGameAccDefault;
            /// <summary>
            /// 0-oo
            /// </summary>
            public float MaxGameAccDefault;
            /// <summary>
            /// 0-1
            /// </summary>
            public float MinGameScaleDefault;
            /// <summary>
            /// 0-1
            /// </summary>
            public float MaxGameScaleDefault;
            /// <summary>
            /// 0-oo
            /// </summary>
            public float MinAbsAccDefault;
            /// <summary>
            /// 0-oo
            /// </summary>
            public float MaxAbsAccDefault;
            /// <summary>
            /// 0-1
            /// </summary>
            public float MinAbsScaleDefault;
            /// <summary>
            /// 0-1
            /// </summary>
            public float MaxAbsScaleDefault;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x24)]
        public class SoundGlobalsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sncl" })]
            public CachedTag SoundClasses;
            [TagField(ValidTags = new [] { "sfx+" })]
            public CachedTag SoundEffects;
            [TagField(ValidTags = new [] { "snmx" })]
            public CachedTag SoundMix;
            [TagField(ValidTags = new [] { "spk!" })]
            public CachedTag SoundCombatDialogueConstants;
            public int Unknown;
        }
        
        [TagStructure(Size = 0x168)]
        public class AiGlobalsBlock : TagStructure
        {
            public float DangerBroadlyFacing;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float DangerShootingNear;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float DangerShootingAt;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float DangerExtremelyClose;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            public float DangerShieldDamage;
            public float DangerExetendedShieldDamage;
            public float DangerBodyDamage;
            public float DangerExtendedBodyDamage;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            [TagField(ValidTags = new [] { "adlg" })]
            public CachedTag GlobalDialogueTag;
            public StringId DefaultMissionDialogueSoundEffect;
            [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            public float JumpDown; // wu/tick
            public float JumpStep; // wu/tick
            public float JumpCrouch; // wu/tick
            public float JumpStand; // wu/tick
            public float JumpStorey; // wu/tick
            public float JumpTower; // wu/tick
            public float MaxJumpDownHeightDown; // wu
            public float MaxJumpDownHeightStep; // wu
            public float MaxJumpDownHeightCrouch; // wu
            public float MaxJumpDownHeightStand; // wu
            public float MaxJumpDownHeightStorey; // wu
            public float MaxJumpDownHeightTower; // wu
            public Bounds<float> HoistStep; // wus
            public Bounds<float> HoistCrouch; // wus
            public Bounds<float> HoistStand; // wus
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            public Bounds<float> VaultStep; // wus
            public Bounds<float> VaultCrouch; // wus
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            public List<AiGlobalsGravemindBlock> GravemindProperties;
            [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
            /// <summary>
            /// A target of this scariness is offically considered scary (by combat dialogue, etc.)
            /// </summary>
            public float ScaryTargetThrehold;
            /// <summary>
            /// A weapon of this scariness is offically considered scary (by combat dialogue, etc.)
            /// </summary>
            public float ScaryWeaponThrehold;
            public float PlayerScariness;
            public float BerserkingActorScariness;
            
            [TagStructure(Size = 0xC)]
            public class AiGlobalsGravemindBlock : TagStructure
            {
                public float MinRetreatTime; // secs
                public float IdealRetreatTime; // secs
                public float MaxRetreatTime; // secs
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class GameGlobalsDamageBlock : TagStructure
        {
            public List<DamageGroupBlock> DamageGroups;
            
            [TagStructure(Size = 0xC)]
            public class DamageGroupBlock : TagStructure
            {
                public StringId Name;
                public List<ArmorModifierBlock> ArmorModifiers;
                
                [TagStructure(Size = 0x8)]
                public class ArmorModifierBlock : TagStructure
                {
                    public StringId Name;
                    public float DamageMultiplier;
                }
            }
        }
        
        [TagStructure()]
        public class GNullBlock : TagStructure
        {
        }
        
        [TagStructure(Size = 0x8)]
        public class SoundBlock : TagStructure
        {
            public CachedTag SoundObsolete;
        }
        
        [TagStructure(Size = 0x14)]
        public class CameraBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "trak" })]
            public CachedTag DefaultUnitCameraTrack;
            public float DefaultChangePause;
            public float FirstPersonChangePause;
            public float FollowingCameraChangePause;
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
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// -1..1, 0 is middle of the screen
            /// </summary>
            public RealPoint2d CrosshairLocation;
            /// <summary>
            /// how long you must be pegged before you start sprinting
            /// </summary>
            public float SecondsToStart;
            /// <summary>
            /// how long you must sprint before you reach top speed
            /// </summary>
            public float SecondsToFullSpeed;
            /// <summary>
            /// how fast being unpegged decays the timer (seconds per second)
            /// </summary>
            public float DecayRate;
            /// <summary>
            /// how much faster we actually go when at full sprint
            /// </summary>
            public float FullSpeedMultiplier;
            /// <summary>
            /// how far the stick needs to be pressed before being considered pegged
            /// </summary>
            public float PeggedMagnitude;
            /// <summary>
            /// how far off straight up (in degrees) we consider pegged
            /// </summary>
            public float PeggedAngularThreshold;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float LookDefaultPitchRate; // degrees
            public float LookDefaultYawRate; // degrees
            /// <summary>
            /// magnitude of yaw for pegged acceleration to kick in
            /// </summary>
            public float LookPegThreshold01;
            /// <summary>
            /// time for a pegged look to reach maximum effect
            /// </summary>
            public float LookYawAccelerationTime; // seconds
            /// <summary>
            /// maximum effect of a pegged look (scales last value in the look function below)
            /// </summary>
            public float LookYawAccelerationScale;
            /// <summary>
            /// time for a pegged look to reach maximum effect
            /// </summary>
            public float LookPitchAccelerationTime; // seconds
            /// <summary>
            /// maximum effect of a pegged look (scales last value in the look function below)
            /// </summary>
            public float LookPitchAccelerationScale;
            /// <summary>
            /// 1 is fast, 0 is none, >1 will probably be really fast
            /// </summary>
            public float LookAutolevellingScale;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public float GravityScale;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            /// <summary>
            /// amount of time player needs to move and not look up or down for autolevelling to kick in
            /// </summary>
            public short MinimumAutolevellingTicks;
            /// <summary>
            /// 0 means the vehicle's up vector is along the ground, 90 means the up vector is pointing straight up:degrees
            /// </summary>
            public Angle MinimumAngleForVehicleFlipping;
            public List<LookFunctionBlock> LookFunction;
            /// <summary>
            /// time that player needs to press ACTION to register as a HOLD
            /// </summary>
            public float MinimumActionHoldTime; // seconds
            
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
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
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
            /// <summary>
            /// difficulty-affecting values for vehicle driving/combat
            /// </summary>
            /// <summary>
            /// Chance of deciding to ram the player in a vehicle on easy difficulty
            /// </summary>
            public float EasyPlayerVehicleRamChance;
            /// <summary>
            /// Chance of deciding to ram the player in a vehicle on normal difficulty
            /// </summary>
            public float NormalPlayerVehicleRamChance;
            /// <summary>
            /// Chance of deciding to ram the player in a vehicle on hard difficulty
            /// </summary>
            public float HardPlayerVehicleRamChance;
            /// <summary>
            /// Chance of deciding to ram the player in a vehicle on impossible difficulty
            /// </summary>
            public float ImpossPlayerVehicleRamChance;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding7;
            [TagField(Length = 0x54, Flags = TagFieldFlags.Padding)]
            public byte[] Padding8;
        }
        
        [TagStructure(Size = 0x2C)]
        public class GrenadesBlock : TagStructure
        {
            public short MaximumCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ThrowingEffect;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Equipment;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
        }
        
        [TagStructure(Size = 0x108)]
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
            public CachedTag Gradients;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused2;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Glow;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused3;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused4;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<VertexShaderReferenceBlock> GlobalVertexShaders;
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
            public CachedTag Unused5;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused6;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused7;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused8;
            /// <summary>
            /// Used in cinematics.
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused9;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused10;
            [TagField(Length = 0x24, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// Used for layers that need to do something for other layers to work correctly if the layer is disabled, also used for
            /// active-camo, etc.
            /// </summary>
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag GlobalShader;
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
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
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Unused11;
            
            [TagStructure(Size = 0x8)]
            public class VertexShaderReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "vrtx" })]
                public CachedTag VertexShader;
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                TintEdgeDensity = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x98)]
        public class InterfaceTagReferences : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Obsolete1;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Obsolete2;
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
            public CachedTag Unknown;
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
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag MainmenuUiGlobals;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag SingleplayerUiGlobals;
            [TagField(ValidTags = new [] { "wgtz" })]
            public CachedTag MultiplayerUiGlobals;
        }
        
        [TagStructure(Size = 0x8)]
        public class CheatWeaponsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0x8)]
        public class CheatPowerupsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "eqip" })]
            public CachedTag Powerup;
        }
        
        [TagStructure(Size = 0x98)]
        public class MultiplayerInformationBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Flag;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unit;
            public List<VehiclesBlock> Vehicles;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag HillShader;
            [TagField(ValidTags = new [] { "shad" })]
            public CachedTag FlagShader;
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Ball;
            public List<SoundsBlock> Sounds;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag InGameText;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<GameEngineGeneralEventBlock> GeneralEvents;
            public List<GameEngineSlayerEventBlock> SlayerEvents;
            public List<GameEngineCtfEventBlock> CtfEvents;
            public List<GameEngineOddballEventBlock> OddballEvents;
            public List<GNullBlock> Unknown;
            public List<GameEngineKingEventBlock> KingEvents;
            
            [TagStructure(Size = 0x8)]
            public class VehiclesBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "vehi" })]
                public CachedTag Vehicle;
            }
            
            [TagStructure(Size = 0x8)]
            public class SoundsBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineGeneralEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    Kill,
                    Suicide,
                    KillTeammate,
                    Victory,
                    TeamVictory,
                    Unused1,
                    Unused2,
                    _1MinToWin,
                    Team1MinToWin,
                    _30SecsToWin,
                    Team30SecsToWin,
                    PlayerQuit,
                    PlayerJoined,
                    KilledByUnknown,
                    _30MinutesLeft,
                    _15MinutesLeft,
                    _5MinutesLeft,
                    _1MinuteLeft,
                    TimeExpired,
                    GameOver,
                    RespawnTick,
                    LastRespawnTick,
                    TeleporterUsed,
                    PlayerChangedTeam,
                    PlayerRejoined,
                    GainedLead,
                    GainedTeamLead,
                    LostLead,
                    LostTeamLead,
                    TiedLeader,
                    TiedTeamLeader,
                    RoundOver,
                    _30SecondsLeft,
                    _10SecondsLeft,
                    KillFalling,
                    KillCollision,
                    KillMelee,
                    SuddenDeath,
                    PlayerBootedPlayer,
                    KillFlagCarrier,
                    KillBombCarrier,
                    KillStickyGrenade,
                    KillSniper,
                    KillStMelee,
                    BoardedVehicle,
                    StartTeamNoti,
                    Telefrag,
                    _10SecsToWin,
                    Team10SecsToWin
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineSlayerEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    NewTarget
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineCtfEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    FlagTaken,
                    FlagDropped,
                    FlagReturnedByPlayer,
                    FlagReturnedByTimeout,
                    FlagCaptured,
                    FlagNewDefensiveTeam,
                    FlagReturnFaliure,
                    SideSwitchTick,
                    SideSwitchFinalTick,
                    SideSwitch30Seconds,
                    SideSwitch10Seconds,
                    FlagContested,
                    FlagCaptureFaliure
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineOddballEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    BallSpawned,
                    BallPickedUp,
                    BallDropped,
                    BallReset,
                    BallTick
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
            
            [TagStructure(Size = 0xA8)]
            public class GameEngineKingEventBlock : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding4;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public SoundResponseExtraSoundsStructBlock ExtraSounds;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding5;
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding6;
                public List<SoundResponseDefinitionBlock> SoundPermutations;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    QuantityMessage = 1 << 0
                }
                
                public enum EventValue : short
                {
                    GameStart,
                    HillControlled,
                    HillContested,
                    HillTick,
                    HillMove,
                    HillControlledTeam,
                    HillContestedTeam
                }
                
                public enum AudienceValue : short
                {
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam,
                    All
                }
                
                public enum RequiredFieldValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                public enum ExcludedAudienceValue : short
                {
                    None,
                    CausePlayer,
                    CauseTeam,
                    EffectPlayer,
                    EffectTeam
                }
                
                [Flags]
                public enum SoundFlagsValue : ushort
                {
                    AnnouncerSound = 1 << 0
                }
                
                [TagStructure(Size = 0x40)]
                public class SoundResponseExtraSoundsStructBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag JapaneseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag GermanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag FrenchSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag SpanishSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ItalianSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag KoreanSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag ChineseSound;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x50)]
                public class SoundResponseDefinitionBlock : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    [TagField(ValidTags = new [] { "snd!" })]
                    public CachedTag EnglishSound;
                    public SoundResponseExtraSoundsStructBlock ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x40)]
                    public class SoundResponseExtraSoundsStructBlock : TagStructure
                    {
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag JapaneseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag GermanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag FrenchSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag SpanishSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ItalianSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag KoreanSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag ChineseSound;
                        [TagField(ValidTags = new [] { "snd!" })]
                        public CachedTag PortugueseSound;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x11C)]
        public class PlayerInformationBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag Unused;
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float WalkingSpeed; // world units per second
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public float RunForward; // world units per second
            public float RunBackward; // world units per second
            public float RunSideways; // world units per second
            public float RunAcceleration; // world units per second squared
            public float SneakForward; // world units per second
            public float SneakBackward; // world units per second
            public float SneakSideways; // world units per second
            public float SneakAcceleration; // world units per second squared
            public float AirborneAcceleration; // world units per second squared
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            public RealPoint3d GrenadeOrigin;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
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
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            public Bounds<float> FirstPersonIdleTime; // seconds
            public float FirstPersonSkipFraction; // [0,1]
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding5;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag CoopRespawnEffect;
            public int BinocularsZoomCount;
            public Bounds<float> BinocularsZoomRange;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag BinocularsZoomInSound;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag BinocularsZoomOutSound;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding6;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag ActiveCamouflageOn;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag ActiveCamouflageOff;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag ActiveCamouflageError;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag ActiveCamouflageReady;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag FlashlightOn;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag FlashlightOff;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag IceCream;
        }
        
        [TagStructure(Size = 0xBC)]
        public class PlayerRepresentationBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag FirstPersonHands;
            [TagField(ValidTags = new [] { "mode" })]
            public CachedTag FirstPersonBody;
            [TagField(Length = 0x28, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x78, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "unit" })]
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
        }
        
        [TagStructure(Size = 0x68)]
        public class FallingDamageBlock : TagStructure
        {
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Bounds<float> HarmfulFallingDistance; // world units
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FallingDamage;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
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
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x24)]
        public class OldMaterialsBlock : TagStructure
        {
            public StringId NewMaterialName;
            public StringId NewGeneralMaterialName;
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
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag MeleeHitSound;
        }
        
        [TagStructure(Size = 0xB4)]
        public class MaterialsBlock : TagStructure
        {
            public StringId Name;
            public StringId ParentName;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public FlagsValue Flags;
            public OldMaterialTypeValue OldMaterialType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public StringId GeneralArmor;
            public StringId SpecificArmor;
            public MaterialPhysicsPropertiesStructBlock PhysicsProperties;
            [TagField(ValidTags = new [] { "mpdt" })]
            public CachedTag OldMaterialPhysics;
            [TagField(ValidTags = new [] { "bsdt" })]
            public CachedTag BreakableSurface;
            public MaterialsSweetenersStructBlock Sweeteners;
            [TagField(ValidTags = new [] { "foot" })]
            public CachedTag MaterialEffects;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                Flammable = 1 << 0,
                Biomass = 1 << 1
            }
            
            public enum OldMaterialTypeValue : short
            {
                Dirt,
                Sand,
                Stone,
                Snow,
                Wood,
                MetalHollow,
                MetalThin,
                MetalThick,
                Rubber,
                Glass,
                ForceField,
                Grunt,
                HunterArmor,
                HunterSkin,
                Elite,
                Jackal,
                JackalEnergyShield,
                EngineerSkin,
                EngineerForceField,
                FloodCombatForm,
                FloodCarrierForm,
                CyborgArmor,
                CyborgEnergyShield,
                HumanArmor,
                HumanSkin,
                Sentinel,
                Monitor,
                Plastic,
                Water,
                Leaves,
                EliteEnergyShield,
                Ice,
                HunterShield
            }
            
            [TagStructure(Size = 0x10)]
            public class MaterialPhysicsPropertiesStructBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float Friction;
                public float Restitution;
                public float Density; // kg/m^3
            }
            
            [TagStructure(Size = 0x74)]
            public class MaterialsSweetenersStructBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag SoundSweetenerSmall;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag SoundSweetenerMedium;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag SoundSweetenerLarge;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerRolling;
                [TagField(ValidTags = new [] { "lsnd" })]
                public CachedTag SoundSweetenerGrinding;
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag SoundSweetenerMelee;
                public CachedTag Unknown;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerSmall;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerMedium;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerLarge;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerRolling;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerGrinding;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag EffectSweetenerMelee;
                public CachedTag Unknown1;
                /// <summary>
                /// when a sweetener inheritance flag is set the sound\effect is not inherited from the parent material.  If you leave the
                /// sweetener blank and set the flag than no effect\sound will play
                /// </summary>
                public SweetenerInheritanceFlagsValue SweetenerInheritanceFlags;
                
                [Flags]
                public enum SweetenerInheritanceFlagsValue : uint
                {
                    SoundSmall = 1 << 0,
                    SoundMedium = 1 << 1,
                    SoundLarge = 1 << 2,
                    SoundRolling = 1 << 3,
                    SoundGrinding = 1 << 4,
                    SoundMelee = 1 << 5,
                    Unknown = 1 << 6,
                    EffectSmall = 1 << 7,
                    EffectMedium = 1 << 8,
                    EffectLarge = 1 << 9,
                    EffectRolling = 1 << 10,
                    EffectGrinding = 1 << 11,
                    EffectMelee = 1 << 12,
                    Unknown1 = 1 << 13
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class MultiplayerUiBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag RandomPlayerNames;
            public List<MultiplayerColorBlock> ObsoleteProfileColors;
            public List<MultiplayerColorBlock1> TeamColors;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag TeamNames;
            
            [TagStructure(Size = 0xC)]
            public class MultiplayerColorBlock : TagStructure
            {
                public RealRgbColor Color;
            }
            
            [TagStructure(Size = 0xC)]
            public class MultiplayerColorBlock1 : TagStructure
            {
                public RealRgbColor Color;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class MultiplayerColorBlock : TagStructure
        {
            public RealRgbColor Color;
        }
        
        [TagStructure(Size = 0x8)]
        public class RuntimeLevelsDefinitionBlock : TagStructure
        {
            public List<RuntimeCampaignLevelBlock> CampaignLevels;
            
            [TagStructure(Size = 0x108)]
            public class RuntimeCampaignLevelBlock : TagStructure
            {
                public int CampaignId;
                public int MapId;
                [TagField(Length = 256)]
                public string Path;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class UiLevelsDefinitionBlock : TagStructure
        {
            public List<UiCampaignBlock> Campaigns;
            public List<GlobalUiCampaignLevelBlock> CampaignLevels;
            public List<GlobalUiMultiplayerLevelBlock> MultiplayerLevels;
            
            [TagStructure(Size = 0xB44)]
            public class UiCampaignBlock : TagStructure
            {
                public int CampaignId;
                [TagField(Length = 0x240)]
                public byte[] Unknown;
                [TagField(Length = 0x900)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0xB50)]
            public class GlobalUiCampaignLevelBlock : TagStructure
            {
                public int CampaignId;
                public int MapId;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(Length = 0x240)]
                public byte[] Unknown;
                [TagField(Length = 0x900)]
                public byte[] Unknown1;
            }
            
            [TagStructure(Size = 0xC64)]
            public class GlobalUiMultiplayerLevelBlock : TagStructure
            {
                public int MapId;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag Bitmap;
                [TagField(Length = 0x240)]
                public byte[] Unknown;
                [TagField(Length = 0x900)]
                public byte[] Unknown1;
                [TagField(Length = 256)]
                public string Path;
                public int SortOrder;
                public FlagsValue Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public sbyte MaxTeamsNone;
                public sbyte MaxTeamsCtf;
                public sbyte MaxTeamsSlayer;
                public sbyte MaxTeamsOddball;
                public sbyte MaxTeamsKoth;
                public sbyte MaxTeamsRace;
                public sbyte MaxTeamsHeadhunter;
                public sbyte MaxTeamsJuggernaut;
                public sbyte MaxTeamsTerritories;
                public sbyte MaxTeamsAssault;
                public sbyte MaxTeamsStub10;
                public sbyte MaxTeamsStub11;
                public sbyte MaxTeamsStub12;
                public sbyte MaxTeamsStub13;
                public sbyte MaxTeamsStub14;
                public sbyte MaxTeamsStub15;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    Unlockable = 1 << 0
                }
            }
        }
    }
}

