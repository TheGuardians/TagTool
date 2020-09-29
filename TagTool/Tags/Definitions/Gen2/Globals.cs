using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "globals", Tag = "matg", Size = 0x2F8)]
    public class Globals : TagStructure
    {
        [TagField(Flags = Padding, Length = 172)]
        public byte[] Padding1;
        public LanguageValue Language;
        public List<GameGlobalsHavokCleanupResources> HavokCleanupResources;
        public List<GameGlobalsCollisionDamage> CollisionDamage;
        public List<SoundGlobalsDefinition> SoundGlobals;
        public List<AiGlobalsDefinition> AiGlobals;
        public List<DamageGlobalsDefinition> DamageTable;
        public List<GNullBlock> Unknown1;
        public List<TagReference> Sounds;
        public List<GameGlobalsCamera> Camera;
        public List<GameGlobalsPlayerControl> PlayerControl;
        public List<GameGlobalsDifficultyInformation> Difficulty;
        public List<GameGlobalsGrenade> Grenades;
        public List<GameGlobalsRasterizerData> RasterizerData;
        public List<GameGlobalsInterfaceTagReferences> InterfaceTags;
        public List<GameGlobalsTagReference> WeaponListUpdateWeaponListEnumInGameGlobalsH;
        public List<GameGlobalsTagReference> CheatPowerups;
        public List<GameGlobalsMultiplayerInformation> MultiplayerInformation;
        public List<GameGlobalsPlayerInformation> PlayerInformation;
        public List<GameGlobalsPlayerRepresentation> PlayerRepresentation;
        public List<GameGlobalsFallingDamage> FallingDamage;
        public List<MaterialDefinition> OldMaterials;
        public List<GlobalMaterialDefinition> Materials;
        public List<GameGlobalsMultiplayerUi> MultiplayerUi;
        public List<RealRgbColor> ProfileColors;
        public CachedTag MultiplayerGlobals;
        public List<RuntimeLevelsDefinition> RuntimeLevelData;
        public List<UiLevelsDefinition> UiLevelData;
        /// <summary>
        /// Default global lighting
        /// </summary>
        public CachedTag DefaultGlobalLighting;
        [TagField(Flags = Padding, Length = 252)]
        public byte[] Padding2;
        
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
        
        [TagStructure(Size = 0x10)]
        public class GameGlobalsHavokCleanupResources : TagStructure
        {
            public CachedTag ObjectCleanupEffect;
        }
        
        [TagStructure(Size = 0x50)]
        public class GameGlobalsCollisionDamage : TagStructure
        {
            public CachedTag CollisionDamage;
            public float MinGameAccDefault; // 0-oo
            public float MaxGameAccDefault; // 0-oo
            public float MinGameScaleDefault; // 0-1
            public float MaxGameScaleDefault; // 0-1
            public float MinAbsAccDefault; // 0-oo
            public float MaxAbsAccDefault; // 0-oo
            public float MinAbsScaleDefault; // 0-1
            public float MaxAbsScaleDefault; // 0-1
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x44)]
        public class SoundGlobalsDefinition : TagStructure
        {
            public CachedTag SoundClasses;
            public CachedTag SoundEffects;
            public CachedTag SoundMix;
            public CachedTag SoundCombatDialogueConstants;
            public int Unknown1;
        }
        
        [TagStructure(Size = 0x174)]
        public class AiGlobalsDefinition : TagStructure
        {
            public float DangerBroadlyFacing;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float DangerShootingNear;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float DangerShootingAt;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding3;
            public float DangerExtremelyClose;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding4;
            public float DangerShieldDamage;
            public float DangerExetendedShieldDamage;
            public float DangerBodyDamage;
            public float DangerExtendedBodyDamage;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding5;
            public CachedTag GlobalDialogueTag;
            public StringId DefaultMissionDialogueSoundEffect;
            [TagField(Flags = Padding, Length = 20)]
            public byte[] Padding6;
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
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding7;
            public Bounds<float> VaultStep; // wus
            public Bounds<float> VaultCrouch; // wus
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding8;
            public List<AiGlobalsGravemindDefinition> GravemindProperties;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Padding9;
            public float ScaryTargetThrehold; // A target of this scariness is offically considered scary (by combat dialogue, etc.)
            public float ScaryWeaponThrehold; // A weapon of this scariness is offically considered scary (by combat dialogue, etc.)
            public float PlayerScariness;
            public float BerserkingActorScariness;
            
            [TagStructure(Size = 0xC)]
            public class AiGlobalsGravemindDefinition : TagStructure
            {
                public float MinRetreatTime; // secs
                public float IdealRetreatTime; // secs
                public float MaxRetreatTime; // secs
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class DamageGlobalsDefinition : TagStructure
        {
            public List<DamageGroupDefinition> DamageGroups;
            
            [TagStructure(Size = 0x10)]
            public class DamageGroupDefinition : TagStructure
            {
                public StringId Name;
                public List<ArmorModifierDefinition> ArmorModifiers;
                
                [TagStructure(Size = 0x8)]
                public class ArmorModifierDefinition : TagStructure
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
        
        [TagStructure(Size = 0x10)]
        public class TagReference : TagStructure
        {
            public CachedTag SoundObsolete;
        }
        
        [TagStructure(Size = 0x1C)]
        public class GameGlobalsCamera : TagStructure
        {
            public CachedTag DefaultUnitCameraTrack;
            public float DefaultChangePause;
            public float FirstPersonChangePause;
            public float FollowingCameraChangePause;
        }
        
        [TagStructure(Size = 0x84)]
        public class GameGlobalsPlayerControl : TagStructure
        {
            public float MagnetismFriction; // how much the crosshair slows over enemies
            public float MagnetismAdhesion; // how much the crosshair sticks to enemies
            public float InconsequentialTargetScale; // scales magnetism level for inconsequential targets like infection forms
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding1;
            /// <summary>
            /// crosshair
            /// </summary>
            public RealPoint2d CrosshairLocation; // -1..1, 0 is middle of the screen
            /// <summary>
            /// sprinting
            /// </summary>
            public float SecondsToStart; // how long you must be pegged before you start sprinting
            public float SecondsToFullSpeed; // how long you must sprint before you reach top speed
            public float DecayRate; // how fast being unpegged decays the timer (seconds per second)
            public float FullSpeedMultiplier; // how much faster we actually go when at full sprint
            public float PeggedMagnitude; // how far the stick needs to be pressed before being considered pegged
            public float PeggedAngularThreshold; // how far off straight up (in degrees) we consider pegged
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            /// <summary>
            /// looking
            /// </summary>
            public float LookDefaultPitchRate; // degrees
            public float LookDefaultYawRate; // degrees
            public float LookPegThreshold01; // magnitude of yaw for pegged acceleration to kick in
            public float LookYawAccelerationTime; // seconds
            public float LookYawAccelerationScale; // maximum effect of a pegged look (scales last value in the look function below)
            public float LookPitchAccelerationTime; // seconds
            public float LookPitchAccelerationScale; // maximum effect of a pegged look (scales last value in the look function below)
            public float LookAutolevellingScale; // 1 is fast, 0 is none, >1 will probably be really fast
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding3;
            public float GravityScale;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding4;
            public short MinimumAutolevellingTicks; // amount of time player needs to move and not look up or down for autolevelling to kick in
            public Angle MinimumAngleForVehicleFlipping; // 0 means the vehicle's up vector is along the ground, 90 means the up vector is pointing straight up:degrees
            public List<Real> LookFunction;
            public float MinimumActionHoldTime; // seconds
            
            [TagStructure(Size = 0x4)]
            public class Real : TagStructure
            {
                public float Scale;
            }
        }
        
        [TagStructure(Size = 0x284)]
        public class GameGlobalsDifficultyInformation : TagStructure
        {
            /// <summary>
            /// health
            /// </summary>
            /// <remarks>
            /// scale values for enemy health and damage settings
            /// </remarks>
            public float EasyEnemyDamage; // enemy damage multiplier on easy difficulty
            public float NormalEnemyDamage; // enemy damage multiplier on normal difficulty
            public float HardEnemyDamage; // enemy damage multiplier on hard difficulty
            public float ImpossEnemyDamage; // enemy damage multiplier on impossible difficulty
            public float EasyEnemyVitality; // enemy maximum body vitality scale on easy difficulty
            public float NormalEnemyVitality; // enemy maximum body vitality scale on normal difficulty
            public float HardEnemyVitality; // enemy maximum body vitality scale on hard difficulty
            public float ImpossEnemyVitality; // enemy maximum body vitality scale on impossible difficulty
            public float EasyEnemyShield; // enemy maximum shield vitality scale on easy difficulty
            public float NormalEnemyShield; // enemy maximum shield vitality scale on normal difficulty
            public float HardEnemyShield; // enemy maximum shield vitality scale on hard difficulty
            public float ImpossEnemyShield; // enemy maximum shield vitality scale on impossible difficulty
            public float EasyEnemyRecharge; // enemy shield recharge scale on easy difficulty
            public float NormalEnemyRecharge; // enemy shield recharge scale on normal difficulty
            public float HardEnemyRecharge; // enemy shield recharge scale on hard difficulty
            public float ImpossEnemyRecharge; // enemy shield recharge scale on impossible difficulty
            public float EasyFriendDamage; // friend damage multiplier on easy difficulty
            public float NormalFriendDamage; // friend damage multiplier on normal difficulty
            public float HardFriendDamage; // friend damage multiplier on hard difficulty
            public float ImpossFriendDamage; // friend damage multiplier on impossible difficulty
            public float EasyFriendVitality; // friend maximum body vitality scale on easy difficulty
            public float NormalFriendVitality; // friend maximum body vitality scale on normal difficulty
            public float HardFriendVitality; // friend maximum body vitality scale on hard difficulty
            public float ImpossFriendVitality; // friend maximum body vitality scale on impossible difficulty
            public float EasyFriendShield; // friend maximum shield vitality scale on easy difficulty
            public float NormalFriendShield; // friend maximum shield vitality scale on normal difficulty
            public float HardFriendShield; // friend maximum shield vitality scale on hard difficulty
            public float ImpossFriendShield; // friend maximum shield vitality scale on impossible difficulty
            public float EasyFriendRecharge; // friend shield recharge scale on easy difficulty
            public float NormalFriendRecharge; // friend shield recharge scale on normal difficulty
            public float HardFriendRecharge; // friend shield recharge scale on hard difficulty
            public float ImpossFriendRecharge; // friend shield recharge scale on impossible difficulty
            public float EasyInfectionForms; // toughness of infection forms (may be negative) on easy difficulty
            public float NormalInfectionForms; // toughness of infection forms (may be negative) on normal difficulty
            public float HardInfectionForms; // toughness of infection forms (may be negative) on hard difficulty
            public float ImpossInfectionForms; // toughness of infection forms (may be negative) on impossible difficulty
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            /// <summary>
            /// ranged fire
            /// </summary>
            /// <remarks>
            /// difficulty-affecting values for enemy ranged combat settings
            /// </remarks>
            public float EasyRateOfFire; // enemy rate of fire scale on easy difficulty
            public float NormalRateOfFire; // enemy rate of fire scale on normal difficulty
            public float HardRateOfFire; // enemy rate of fire scale on hard difficulty
            public float ImpossRateOfFire; // enemy rate of fire scale on impossible difficulty
            public float EasyProjectileError; // enemy projectile error scale, as a fraction of their base firing error. on easy difficulty
            public float NormalProjectileError; // enemy projectile error scale, as a fraction of their base firing error. on normal difficulty
            public float HardProjectileError; // enemy projectile error scale, as a fraction of their base firing error. on hard difficulty
            public float ImpossProjectileError; // enemy projectile error scale, as a fraction of their base firing error. on impossible difficulty
            public float EasyBurstError; // enemy burst error scale; reduces intra-burst shot distance. on easy difficulty
            public float NormalBurstError; // enemy burst error scale; reduces intra-burst shot distance. on normal difficulty
            public float HardBurstError; // enemy burst error scale; reduces intra-burst shot distance. on hard difficulty
            public float ImpossBurstError; // enemy burst error scale; reduces intra-burst shot distance. on impossible difficulty
            public float EasyNewTargetDelay; // enemy new-target delay scale factor. on easy difficulty
            public float NormalNewTargetDelay; // enemy new-target delay scale factor. on normal difficulty
            public float HardNewTargetDelay; // enemy new-target delay scale factor. on hard difficulty
            public float ImpossNewTargetDelay; // enemy new-target delay scale factor. on impossible difficulty
            public float EasyBurstSeparation; // delay time between bursts scale factor for enemies. on easy difficulty
            public float NormalBurstSeparation; // delay time between bursts scale factor for enemies. on normal difficulty
            public float HardBurstSeparation; // delay time between bursts scale factor for enemies. on hard difficulty
            public float ImpossBurstSeparation; // delay time between bursts scale factor for enemies. on impossible difficulty
            public float EasyTargetTracking; // additional target tracking fraction for enemies. on easy difficulty
            public float NormalTargetTracking; // additional target tracking fraction for enemies. on normal difficulty
            public float HardTargetTracking; // additional target tracking fraction for enemies. on hard difficulty
            public float ImpossTargetTracking; // additional target tracking fraction for enemies. on impossible difficulty
            public float EasyTargetLeading; // additional target leading fraction for enemies. on easy difficulty
            public float NormalTargetLeading; // additional target leading fraction for enemies. on normal difficulty
            public float HardTargetLeading; // additional target leading fraction for enemies. on hard difficulty
            public float ImpossTargetLeading; // additional target leading fraction for enemies. on impossible difficulty
            public float EasyOverchargeChance; // overcharge chance scale factor for enemies. on easy difficulty
            public float NormalOverchargeChance; // overcharge chance scale factor for enemies. on normal difficulty
            public float HardOverchargeChance; // overcharge chance scale factor for enemies. on hard difficulty
            public float ImpossOverchargeChance; // overcharge chance scale factor for enemies. on impossible difficulty
            public float EasySpecialFireDelay; // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on easy difficulty
            public float NormalSpecialFireDelay; // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on normal difficulty
            public float HardSpecialFireDelay; // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on hard difficulty
            public float ImpossSpecialFireDelay; // delay between special-fire shots (overcharge, banshee bombs) scale factor for enemies. on impossible difficulty
            public float EasyGuidanceVsPlayer; // guidance velocity scale factor for all projectiles targeted on a player. on easy difficulty
            public float NormalGuidanceVsPlayer; // guidance velocity scale factor for all projectiles targeted on a player. on normal difficulty
            public float HardGuidanceVsPlayer; // guidance velocity scale factor for all projectiles targeted on a player. on hard difficulty
            public float ImpossGuidanceVsPlayer; // guidance velocity scale factor for all projectiles targeted on a player. on impossible difficulty
            public float EasyMeleeDelayBase; // delay period added to all melee attacks, even when berserk. on easy difficulty
            public float NormalMeleeDelayBase; // delay period added to all melee attacks, even when berserk. on normal difficulty
            public float HardMeleeDelayBase; // delay period added to all melee attacks, even when berserk. on hard difficulty
            public float ImpossMeleeDelayBase; // delay period added to all melee attacks, even when berserk. on impossible difficulty
            public float EasyMeleeDelayScale; // multiplier for all existing non-berserk melee delay times. on easy difficulty
            public float NormalMeleeDelayScale; // multiplier for all existing non-berserk melee delay times. on normal difficulty
            public float HardMeleeDelayScale; // multiplier for all existing non-berserk melee delay times. on hard difficulty
            public float ImpossMeleeDelayScale; // multiplier for all existing non-berserk melee delay times. on impossible difficulty
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding2;
            /// <summary>
            /// grenades
            /// </summary>
            /// <remarks>
            /// difficulty-affecting values for enemy grenade behavior
            /// </remarks>
            public float EasyGrenadeChanceScale; // scale factor affecting the desicions to throw a grenade. on easy difficulty
            public float NormalGrenadeChanceScale; // scale factor affecting the desicions to throw a grenade. on normal difficulty
            public float HardGrenadeChanceScale; // scale factor affecting the desicions to throw a grenade. on hard difficulty
            public float ImpossGrenadeChanceScale; // scale factor affecting the desicions to throw a grenade. on impossible difficulty
            public float EasyGrenadeTimerScale; // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on easy difficulty
            public float NormalGrenadeTimerScale; // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on normal difficulty
            public float HardGrenadeTimerScale; // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on hard difficulty
            public float ImpossGrenadeTimerScale; // scale factor affecting the delay period between grenades thrown from the same encounter (lower is more often). on impossible difficulty
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding5;
            /// <summary>
            /// placement
            /// </summary>
            /// <remarks>
            /// difficulty-affecting values for enemy placement
            /// </remarks>
            public float EasyMajorUpgradeNormal; // fraction of actors upgraded to their major variant. on easy difficulty
            public float NormalMajorUpgradeNormal; // fraction of actors upgraded to their major variant. on normal difficulty
            public float HardMajorUpgradeNormal; // fraction of actors upgraded to their major variant. on hard difficulty
            public float ImpossMajorUpgradeNormal; // fraction of actors upgraded to their major variant. on impossible difficulty
            public float EasyMajorUpgradeFew; // fraction of actors upgraded to their major variant when mix = normal. on easy difficulty
            public float NormalMajorUpgradeFew; // fraction of actors upgraded to their major variant when mix = normal. on normal difficulty
            public float HardMajorUpgradeFew; // fraction of actors upgraded to their major variant when mix = normal. on hard difficulty
            public float ImpossMajorUpgradeFew; // fraction of actors upgraded to their major variant when mix = normal. on impossible difficulty
            public float EasyMajorUpgradeMany; // fraction of actors upgraded to their major variant when mix = many. on easy difficulty
            public float NormalMajorUpgradeMany; // fraction of actors upgraded to their major variant when mix = many. on normal difficulty
            public float HardMajorUpgradeMany; // fraction of actors upgraded to their major variant when mix = many. on hard difficulty
            public float ImpossMajorUpgradeMany; // fraction of actors upgraded to their major variant when mix = many. on impossible difficulty
            /// <summary>
            /// vehicles
            /// </summary>
            /// <remarks>
            /// difficulty-affecting values for vehicle driving/combat
            /// </remarks>
            public float EasyPlayerVehicleRamChance; // Chance of deciding to ram the player in a vehicle on easy difficulty
            public float NormalPlayerVehicleRamChance; // Chance of deciding to ram the player in a vehicle on normal difficulty
            public float HardPlayerVehicleRamChance; // Chance of deciding to ram the player in a vehicle on hard difficulty
            public float ImpossPlayerVehicleRamChance; // Chance of deciding to ram the player in a vehicle on impossible difficulty
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding6;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding7;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding8;
            [TagField(Flags = Padding, Length = 84)]
            public byte[] Padding9;
        }
        
        [TagStructure(Size = 0x44)]
        public class GameGlobalsGrenade : TagStructure
        {
            public short MaximumCount;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public CachedTag ThrowingEffect;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding2;
            public CachedTag Equipment;
            public CachedTag Projectile;
        }
        
        [TagStructure(Size = 0x1AC)]
        public class GameGlobalsRasterizerData : TagStructure
        {
            /// <summary>
            /// function textures
            /// </summary>
            /// <remarks>
            /// Used internally by the rasterizer. (Do not change unless you know what you're doing!)
            /// </remarks>
            public CachedTag DistanceAttenuation;
            public CachedTag VectorNormalization;
            public CachedTag Gradients;
            public CachedTag Unused;
            public CachedTag Unused1;
            public CachedTag Unused2;
            public CachedTag Glow;
            public CachedTag Unused3;
            public CachedTag Unused4;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding1;
            public List<VertexShaderReference> GlobalVertexShaders;
            /// <summary>
            /// default textures
            /// </summary>
            /// <remarks>
            /// Used internally by the rasterizer - additive, multiplicative, detail, vector. (Do not change ever, period.)
            /// </remarks>
            public CachedTag Default2d;
            public CachedTag Default3d;
            public CachedTag DefaultCubeMap;
            /// <summary>
            /// experimental textures
            /// </summary>
            /// <remarks>
            /// Used internally by the rasterizer. (Used by Bernie's experimental shaders.)
            /// </remarks>
            public CachedTag Unused5;
            public CachedTag Unused6;
            public CachedTag Unused7;
            public CachedTag Unused8;
            /// <summary>
            /// video effect textures
            /// </summary>
            /// <remarks>
            /// Used in cinematics.
            /// </remarks>
            public CachedTag Unused9;
            public CachedTag Unused10;
            [TagField(Flags = Padding, Length = 36)]
            public byte[] Padding2;
            /// <summary>
            /// global shader
            /// </summary>
            /// <remarks>
            /// Used for layers that need to do something for other layers to work correctly if the layer is disabled, also used for active-camo, etc.
            /// </remarks>
            public CachedTag GlobalShader;
            /// <summary>
            /// active camouflage
            /// </summary>
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            public float RefractionAmount; // pixels
            public float DistanceFalloff;
            public RealRgbColor TintColor;
            public float HyperStealthRefraction; // pixels
            public float HyperStealthDistanceFalloff;
            public RealRgbColor HyperStealthTintColor;
            /// <summary>
            /// PC textures
            /// </summary>
            /// <remarks>
            /// The PC can't use 3D textures, so we use this instead.
            /// </remarks>
            public CachedTag Unused11;
            
            [TagStructure(Size = 0x10)]
            public class VertexShaderReference : TagStructure
            {
                public CachedTag VertexShader;
            }
            
            [Flags]
            public enum FlagsValue : ushort
            {
                TintEdgeDensity = 1 << 0
            }
        }
        
        [TagStructure(Size = 0x130)]
        public class GameGlobalsInterfaceTagReferences : TagStructure
        {
            public CachedTag Obsolete1;
            public CachedTag Obsolete2;
            public CachedTag ScreenColorTable;
            public CachedTag HudColorTable;
            public CachedTag EditorColorTable;
            public CachedTag DialogColorTable;
            public CachedTag HudGlobals;
            public CachedTag MotionSensorSweepBitmap;
            public CachedTag MotionSensorSweepBitmapMask;
            public CachedTag MultiplayerHudBitmap;
            public CachedTag Unknown1;
            public CachedTag HudDigitsDefinition;
            public CachedTag MotionSensorBlipBitmap;
            public CachedTag InterfaceGooMap1;
            public CachedTag InterfaceGooMap2;
            public CachedTag InterfaceGooMap3;
            public CachedTag MainmenuUiGlobals;
            public CachedTag SingleplayerUiGlobals;
            public CachedTag MultiplayerUiGlobals;
        }
        
        [TagStructure(Size = 0x10)]
        public class GameGlobalsTagReference : TagStructure
        {
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0xE8)]
        public class GameGlobalsMultiplayerInformation : TagStructure
        {
            public CachedTag Flag;
            public CachedTag Unit;
            public List<GameGlobalsTagReference> Vehicles;
            public CachedTag HillShader;
            public CachedTag FlagShader;
            public CachedTag Ball;
            public List<GameGlobalsTagReference> Sounds;
            public CachedTag InGameText;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding1;
            public List<MultiplayerEventResponseDefinition> GeneralEvents;
            public List<MultiplayerEventResponseDefinition> SlayerEvents;
            public List<MultiplayerEventResponseDefinition> CtfEvents;
            public List<MultiplayerEventResponseDefinition> OddballEvents;
            public List<GNullBlock> Unknown1;
            public List<MultiplayerEventResponseDefinition> KingEvents;
            
            [TagStructure(Size = 0x10)]
            public class GameGlobalsTagReference : TagStructure
            {
                public CachedTag Vehicle;
            }
            
            [TagStructure(Size = 0xF4)]
            public class MultiplayerEventResponseDefinition : TagStructure
            {
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public EventValue Event;
                public AudienceValue Audience;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
                public StringId DisplayString;
                public RequiredFieldValue RequiredField;
                public ExcludedAudienceValue ExcludedAudience;
                public StringId PrimaryString;
                public int PrimaryStringDuration; // seconds
                public StringId PluralDisplayString;
                [TagField(Flags = Padding, Length = 28)]
                public byte[] Padding4;
                public float SoundDelayAnnouncerOnly;
                public SoundFlagsValue SoundFlags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding5;
                public CachedTag Sound;
                public _1 ExtraSounds;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding6;
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding7;
                public List<MultiplayerEventSoundResponseDefinition> SoundPermutations;
                
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
                
                [TagStructure(Size = 0x80)]
                public class _1 : TagStructure
                {
                    public CachedTag JapaneseSound;
                    public CachedTag GermanSound;
                    public CachedTag FrenchSound;
                    public CachedTag SpanishSound;
                    public CachedTag ItalianSound;
                    public CachedTag KoreanSound;
                    public CachedTag ChineseSound;
                    public CachedTag PortugueseSound;
                }
                
                [TagStructure(Size = 0x98)]
                public class MultiplayerEventSoundResponseDefinition : TagStructure
                {
                    public SoundFlagsValue SoundFlags;
                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Padding1;
                    public CachedTag EnglishSound;
                    public _1 ExtraSounds;
                    public float Probability;
                    
                    [Flags]
                    public enum SoundFlagsValue : ushort
                    {
                        AnnouncerSound = 1 << 0
                    }
                    
                    [TagStructure(Size = 0x80)]
                    public class _1 : TagStructure
                    {
                        public CachedTag JapaneseSound;
                        public CachedTag GermanSound;
                        public CachedTag FrenchSound;
                        public CachedTag SpanishSound;
                        public CachedTag ItalianSound;
                        public CachedTag KoreanSound;
                        public CachedTag ChineseSound;
                        public CachedTag PortugueseSound;
                    }
                }
            }
            
            [TagStructure()]
            public class GNullBlock : TagStructure
            {
            }
        }
        
        [TagStructure(Size = 0x174)]
        public class GameGlobalsPlayerInformation : TagStructure
        {
            public CachedTag Unused;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding1;
            public float WalkingSpeed; // world units per second
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding2;
            public float RunForward; // world units per second
            public float RunBackward; // world units per second
            public float RunSideways; // world units per second
            public float RunAcceleration; // world units per second squared
            public float SneakForward; // world units per second
            public float SneakBackward; // world units per second
            public float SneakSideways; // world units per second
            public float SneakAcceleration; // world units per second squared
            public float AirborneAcceleration; // world units per second squared
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding3;
            public RealPoint3d GrenadeOrigin;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding4;
            public float StunMovementPenalty; // [0,1]
            public float StunTurningPenalty; // [0,1]
            public float StunJumpingPenalty; // [0,1]
            public float MinimumStunTime; // seconds
            public float MaximumStunTime; // seconds
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding5;
            public Bounds<float> FirstPersonIdleTime; // seconds
            public float FirstPersonSkipFraction; // [0,1]
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding6;
            public CachedTag CoopRespawnEffect;
            public int BinocularsZoomCount;
            public Bounds<float> BinocularsZoomRange;
            public CachedTag BinocularsZoomInSound;
            public CachedTag BinocularsZoomOutSound;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding7;
            public CachedTag ActiveCamouflageOn;
            public CachedTag ActiveCamouflageOff;
            public CachedTag ActiveCamouflageError;
            public CachedTag ActiveCamouflageReady;
            public CachedTag FlashlightOn;
            public CachedTag FlashlightOff;
            public CachedTag IceCream;
        }
        
        [TagStructure(Size = 0xD4)]
        public class GameGlobalsPlayerRepresentation : TagStructure
        {
            public CachedTag FirstPersonHands;
            public CachedTag FirstPersonBody;
            [TagField(Flags = Padding, Length = 40)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 120)]
            public byte[] Padding2;
            public CachedTag ThirdPersonUnit;
            public StringId ThirdPersonVariant;
        }
        
        [TagStructure(Size = 0x98)]
        public class GameGlobalsFallingDamage : TagStructure
        {
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;
            public Bounds<float> HarmfulFallingDistance; // world units
            public CachedTag FallingDamage;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding2;
            public float MaximumFallingDistance; // world units
            public CachedTag DistanceDamage;
            public CachedTag VehicleEnvironemtnCollisionDamageEffect;
            public CachedTag VehicleKilledUnitDamageEffect;
            public CachedTag VehicleCollisionDamage;
            public CachedTag FlamingDeathDamage;
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding3;
        }
        
        [TagStructure(Size = 0x2C)]
        public class MaterialDefinition : TagStructure
        {
            public StringId NewMaterialName;
            public StringId NewGeneralMaterialName;
            /// <summary>
            /// vehicle terrain parameters
            /// </summary>
            /// <remarks>
            /// the following fields modify the way a vehicle drives over terrain of this material type.
            /// </remarks>
            public float GroundFrictionScale; // fraction of original velocity parallel to the ground after one tick
            public float GroundFrictionNormalK1Scale; // cosine of angle at which friction falls off
            public float GroundFrictionNormalK0Scale; // cosine of angle at which friction is zero
            public float GroundDepthScale; // depth a point mass rests in the ground
            public float GroundDampFractionScale; // fraction of original velocity perpendicular to the ground after one tick
            public CachedTag MeleeHitSound;
        }
        
        [TagStructure(Size = 0x13C)]
        public class GlobalMaterialDefinition : TagStructure
        {
            public StringId Name;
            public StringId ParentName;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public FlagsValue Flags;
            public OldMaterialTypeValue OldMaterialType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public StringId GeneralArmor;
            public StringId SpecificArmor;
            public MaterialPhysicsProperties PhysicsProperties;
            public CachedTag OldMaterialPhysics;
            public CachedTag BreakableSurface;
            public GlobalMaterialSweetenersDefinition Sweeteners;
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
            public class MaterialPhysicsProperties : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public float Friction;
                public float Restitution;
                public float Density; // kg/m^3
            }
            
            [TagStructure(Size = 0xE4)]
            public class GlobalMaterialSweetenersDefinition : TagStructure
            {
                public CachedTag SoundSweetenerSmall;
                public CachedTag SoundSweetenerMedium;
                public CachedTag SoundSweetenerLarge;
                public CachedTag SoundSweetenerRolling;
                public CachedTag SoundSweetenerGrinding;
                public CachedTag SoundSweetenerMelee;
                public CachedTag Unknown1;
                public CachedTag EffectSweetenerSmall;
                public CachedTag EffectSweetenerMedium;
                public CachedTag EffectSweetenerLarge;
                public CachedTag EffectSweetenerRolling;
                public CachedTag EffectSweetenerGrinding;
                public CachedTag EffectSweetenerMelee;
                public CachedTag Unknown2;
                /// <summary>
                /// sweetener inheritance flags
                /// </summary>
                /// <remarks>
                /// when a sweetener inheritance flag is set the sound\effect is not inherited from the parent material.  If you leave the sweetener blank and set the flag than no effect\sound will play
                /// </remarks>
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
                    Bit6 = 1 << 6,
                    EffectSmall = 1 << 7,
                    EffectMedium = 1 << 8,
                    EffectLarge = 1 << 9,
                    EffectRolling = 1 << 10,
                    EffectGrinding = 1 << 11,
                    EffectMelee = 1 << 12,
                    Bit13 = 1 << 13
                }
            }
        }
        
        [TagStructure(Size = 0x38)]
        public class GameGlobalsMultiplayerUi : TagStructure
        {
            public CachedTag RandomPlayerNames;
            public List<RealRgbColor> ObsoleteProfileColors;
            public List<RealRgbColor> TeamColors;
            public CachedTag TeamNames;
            
            [TagStructure(Size = 0xC)]
            public class RealRgbColor : TagStructure
            {
                public RealRgbColor Color;
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class RealRgbColor : TagStructure
        {
            public RealRgbColor Color;
        }
        
        [TagStructure(Size = 0xC)]
        public class RuntimeLevelsDefinition : TagStructure
        {
            public List<CampaignRuntimeLevelDefinition> CampaignLevels;
            
            [TagStructure(Size = 0x108)]
            public class CampaignRuntimeLevelDefinition : TagStructure
            {
                public int CampaignId;
                public int MapId;
                [TagField(Length = 256)]
                public string Path;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class UiLevelsDefinition : TagStructure
        {
            public List<CampaignDefinition> Campaigns;
            public List<CampaignUiLevelDefinition> CampaignLevels;
            public List<MultiplayerUiLevelDefinition> MultiplayerLevels;
            
            [TagStructure(Size = 0xB44)]
            public class CampaignDefinition : TagStructure
            {
                public int CampaignId;
                [TagField(Flags = Padding, Length = 576)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2304)]
                public byte[] Unknown2;
            }
            
            [TagStructure(Size = 0xB58)]
            public class CampaignUiLevelDefinition : TagStructure
            {
                public int CampaignId;
                public int MapId;
                public CachedTag Bitmap;
                [TagField(Flags = Padding, Length = 576)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2304)]
                public byte[] Unknown2;
            }
            
            [TagStructure(Size = 0xC6C)]
            public class MultiplayerUiLevelDefinition : TagStructure
            {
                public int MapId;
                public CachedTag Bitmap;
                [TagField(Flags = Padding, Length = 576)]
                public byte[] Unknown1;
                [TagField(Flags = Padding, Length = 2304)]
                public byte[] Unknown2;
                [TagField(Length = 256)]
                public string Path;
                public int SortOrder;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 3)]
                public byte[] Padding1;
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

