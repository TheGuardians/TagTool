using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
	[TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xE8, MaxVersion = CacheVersion.Halo3Beta)]
	[TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF4, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xE8, MinVersion = CacheVersion.HaloReach)]
    public class DamageEffect : TagStructure
	{
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0-1]
        public DamageEffectFlags EffectFlags;
        public DamageSideEffects SideEffect;
        public DamageCategories Category;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DamageDeathVocalizations DeathVocalization;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public DamageFlags Flags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DamageFlagsReach FlagsReach;
        public float AreaOfEffectCoreRadius; // if this is area of effect damage
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public TagFunction DamageFalloffFunction;
        public Angle DamageInnerConeAngle;
        public Angle DamageOuterConeAngle;
        public float ActiveCamoflageDamage; // how much more visible this damage makes a player who is active camouflaged [0,1]
        public float Stun; // amount of stun added to damaged unit [0,1]
        public float MaximumStun; // damaged unit's stun will never exceed this amount [0,1]
        public float StunDuration; // duration of stun due to this damage (seconds)
        // how long we stun recovering current body damage
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int DamageStun; // ticks
        public float InstantaneousAcceleration; // [0,+inf]
        public float RiderDirectDamageScale;
        public float RiderMaxTransferDamageScale;
        public float RiderMinTransferDamageScale;
        // The maximum amount to apply the pain screen pose overlay
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float SoftPingPainScreenScale; // [0,1]
        public StringId GeneralDamage;
        public StringId SpecificDamage;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public StringId CustomResponseLabel;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<CustomDamageResponseLabelBlock> CustomResponseLabels;
        public float AiStunRadius; // world units
        public Bounds<float> AiStunBounds; // (0-1)
        public float ShakeRadius;
        public float EmpRadius;   
		[TagField(MinVersion = CacheVersion.Halo3Retail)]
        public float AoeSpikeRadius;
		[TagField(MinVersion = CacheVersion.Halo3Retail)]
		public float AoeSpikeDamageBump;   
        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float ShieldRenderEffectsScale = 1.0f; 
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<PlayerResponseBlock> PlayerResponses;

        [TagField(ValidTags = new[] { "drdf" })]
        public CachedTag DamageResponse;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CameraImpulseStruct CameraImpulse;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public CameraShakeStruct CameraShake;

        [TagField(ValidTags = new[] { "scmb", "snd!" })]
        public CachedTag Sound;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<DamageEffectSoundBlockStruct> DamageSounds;

        public BreakingEffectStruct BreakingEffect;

        [Flags]
        public enum DamageEffectFlags : uint
        {
            None = 0,
            DontScaleDamageByDistance = 1 << 0,
            /// <summary>
            /// area of effect damage only affects players
            /// </summary>
            AreaDamagePlayersOnly = 1 << 1,
            AffectsModelTargets = 1 << 2,
        }

        public enum DamageSideEffects : short
        {
            None,
            Harmless,
            LethalToTheUnsuspecting,
            Emp
        }

        public enum DamageCategories : short
        {
            None,
            Falling,
            Bullet,
            Grenade,
            HighExplosive,
            Sniper,
            Melee,
            Flame,
            MountedWeapon,
            Vehicle,
            Plasma,
            Needle,
            Shotgun,
            Assassinated
        }

        public enum DamageDeathVocalizations : int
        {
            // uses code to figure out what vocalization to use
            Default,
            Dth,
            DthFall,
            DthMjr,
            DthSlw,
            DthHdsht,
            DthSlnt,
            DthDrama,
            DthReanimated,
            Thrwn,
            DieSpace,
            DieAss
        }

        [Flags]
        public enum DamageFlags : uint
        {
            None = 0,
            DoesNotHurtOwner = 1 << 0,
            CanCauseHeadshots = 1 << 1,
            PingsResistantUnits = 1 << 2,
            DoesNotHurtFriends = 1 << 3,
            DoesNotPingUnits = 1 << 4,
            DetonatesExplosives = 1 << 5,
            OnlyHurtsShields = 1 << 6,
            CausesFlamingDeath = 1 << 7,
            DamageIndicatorsAlwaysPointDown = 1 << 8,
            SkipsShields = 1 << 9,
            OnlyHurtsOneInfectionForm = 1 << 10,
            TransferDamageAlwaysUsesMinimum = 1 << 11,
            InfectionFormPop = 1 << 12,
            IgnoreSeatScaleForDirDmg = 1 << 13,
            ForcesHardPing = 1 << 14,
            DoesNotHurtPlayers = 1 << 15,
            DoesNotOvercombine = 1 << 16,
            EnablesSpecialDeath = 1 << 17,
            CannotCauseBetrayals = 1 << 18,
            UsesOldEmpBehavior = 1 << 19,
            IgnoresDamageResistance = 1 << 20,
            ForceSKillOnDeath = 1 << 21,
            CauseMagicDeceleration = 1 << 22,
            InhibitsMeleeAttacks = 1 << 23, // ODST
            Bit24 = 1 << 24 // HO concussive blast
        }

        [Flags]
        public enum DamageFlagsReach : uint
        {
            None = 0,
            DoesNotHurtOwner = 1 << 0,
            CanCauseHeadshots = 1 << 1,
            IgnoresHeadshotObstructions = 1 << 2, // arms, held weapons, attachments
            PingsResistantUnits = 1 << 3,
            DoesNotHurtFriends = 1 << 4,
            DoesNotPingUnits = 1 << 5,
            DetonatesExplosives = 1 << 6,
            OnlyHurtsShields = 1 << 7,
            CausesFlamingDeath = 1 << 8,
            DamageIndicatorsAlwaysPointDown = 1 << 9,
            SkipsShields = 1 << 10,
            TransferDmgAlwaysUsesMin = 1 << 11,
            IgnoreSeatScaleForDirDmg = 1 << 12,
            ForcesHardPingIfBodyDmg = 1 << 13,
            ForcesHardPingAlways = 1 << 14,
            DoesNotHurtPlayers = 1 << 15,
            EnablesSpecialDeath = 1 << 16,
            CannotCauseBetrayals = 1 << 17,
            UsesOldEmpBehavior = 1 << 18,
            IgnoresDamageResistance = 1 << 19,
            ForceSKillOnDeath = 1 << 20,
            CauseMagicDeceleration = 1 << 21,
            AoeSkipObstructionTest = 1 << 22,
            DoesNotSpillOver = 1 << 23,
            DoesNotHurtBoarders = 1 << 24,
            DoesNotCauseBipedAoeEffect = 1 << 25
        }

        [TagStructure(Size = 0x70)]
        public class PlayerResponseBlock : TagStructure
		{
            public ResponseTypeValue ResponseType;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public ScreenFlashStruct ScreenFlash;
            public RumbleDefinitionStruct Rumble;
            public DamageEffectSoundEffectDefinition SoundEffect;

            [TagStructure(Size = 0x20)]
            public class ScreenFlashStruct : TagStructure
            {
                public ScreenFlashTypes Type;
                public ScreenFlashPriorities Priority;
                public float Duration; // seconds
                public FunctionType FadeFunction;

                [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;

                public float MaximumIntensity;
                public RealArgbColor Color;
            }

            [TagStructure(Size = 0x30)]
            public class RumbleDefinitionStruct : TagStructure
            {
                public float LowFrequencyRumbleDuration;
                public TagFunction LowFrequencyRumbleFunction = new TagFunction { Data = new byte[0] };
                public float HighFrequencyRumbleDuration;
                public TagFunction HighFrequencyRumbleFunction = new TagFunction { Data = new byte[0] };
            }

            [TagStructure(Size = 0x1C)]
            public class DamageEffectSoundEffectDefinition : TagStructure
            {
                public StringId EffectName;
                public float Duration;
                public TagFunction EffectScaleFunction = new TagFunction { Data = new byte[0] };
            }

            public enum ResponseTypeValue : short
            {
                Shielded,
                Unshielded,
                All
            }

            public enum ScreenFlashTypes : short
            {
                None,
                Lighten,
                Darken,
                Max,
                Min,
                Invert,
                Tint
            }

            public enum ScreenFlashPriorities : short
            {
                Low,
                Medium,
                High
            }

        }

        public enum GlobalPeriodicFunctionType : short
        {
            One,
            Zero,
            Cosine,
            CosineVariablePeriod,
            DiagonalWave,
            DiagonalWaveVariablePeriod,
            Slide,
            SlideVariablePeriod,
            Noise,
            Jitter,
            Wander,
            Spark
        }

        [TagStructure(Size = 0x18)]
        public class CameraImpulseStruct : TagStructure
        {

            public float Duration; // seconds
            public FunctionType FadeFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public Angle Rotation; // degrees
            public float Pushback; // world units
            public Bounds<float> Jitter; // world units
        }

        [TagStructure(Size = 0x1C)]
        public class CameraShakeStruct : TagStructure
        {
            public float Duration; // seconds
            public FunctionType FalloffFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;

            public float RandomTranslation; // random translation in all directions (world units)
            public Angle RandomRotation; // random rotation in all directions (degrees)
            public GlobalPeriodicFunctionType WobbleFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;

            public float WobbleFunctionPeriod; // seconds
            public float WobbleWeight; // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be felt when the wobble function's value is zero.
        }

        [TagStructure(Size = 0x18)]
        public class BreakingEffectStruct : TagStructure
        {
            public float ForwardVelocity; // world units per second
            public float ForwardRadius; // world units
            public float ForwardExponent;
            public float OutwardVelocity; // world units per second
            public float OutwardRadius; // world units
            public float OutwardExponent;
        }

        [TagStructure(Size = 0x4)]
        public class CustomDamageResponseLabelBlock : TagStructure
        {
            // label used to control what damage response will fire.^
            public StringId CustomLabel;
        }

        [TagStructure(Size = 0x14)]
        public class DamageEffectSoundBlockStruct : TagStructure
        {
            [TagField(ValidTags = new[] { "scmb", "sndo", "snd!" })]
            public CachedTag Sound;
            public DamageEffectSoundTypeFlags DamageTypes;
            public ObjectTypeEnum ObjectTypes;

            [Flags]
            public enum DamageEffectSoundTypeFlags : ushort
            {
                None = 1 << 0,
                // headshots and assassinations
                LethalInstantaneous = 1 << 1,
                // excludes headshots and assassinations
                Lethal = 1 << 2,
                NonLethal = 1 << 3
            }

            [Flags]
            public enum ObjectTypeEnum : ushort
            {
                Biped = 1 << 0,
                Vehicle = 1 << 1,
                Weapon = 1 << 2,
                Equipment = 1 << 3,
                Terminal = 1 << 4,
                Projectile = 1 << 5,
                Scenery = 1 << 6,
                Machine = 1 << 7,
                Control = 1 << 8,
                Dispenser = 1 << 9,
                SoundScenery = 1 << 10,
                Crate = 1 << 11,
                Creature = 1 << 12,
                Giant = 1 << 13,
                EffectScenery = 1 << 14,
                Spawner = 1 << 15
            }
        }
    }
}