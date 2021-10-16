using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF4, MinVersion = CacheVersion.HaloOnlineED)]
    public class DamageEffect : TagStructure
	{
        public Bounds<float> Radius; // world units
        public float CutoffScale; // [0-1]
        public DamageEffectFlags EffectFlags;
        public DamageSideEffects SideEffect;
        public DamageCategories Category;
        public DamageFlags Flags;
        public float AreaOfEffectCoreRadius; // if this is area of effect damage
        public float DamageLowerBound;
        public Bounds<float> DamageUpperBound;
        public Angle DamageInnerConeAngle;
        public Angle DamageOuterConeAngle;
        public float ActiveCamoflageDamage; // how much more visible this damage makes a player who is active camouflaged [0,1]
        public float Stun; // amount of stun added to damaged unit [0,1]
        public float MaximumStun; // damaged unit's stun will never exceed this amount [0,1]
        public float StunDuration; // duration of stun due to this damage (seconds)
        public float InstantaneousAcceleration; // [0,+inf]
        public float RiderDirectDamageScale;
        public float RiderMaxTransferDamageScale;
        public float RiderMinTransferDamageScale;
        public StringId GeneralDamage;
        public StringId SpecificDamage;
        public StringId CustomResponseLabel;
        public float AiStunRadius; // world units
        public Bounds<float> AiStunBounds; // (0-1)
        public float ShakeRadius;
        public float EmpRadius;
        public float AOESpikeRadius;
        public float AOESpikeDamageBump;

        [TagField(MinVersion = CacheVersion.HaloOnlineED)]
        public float Unknown_HO = 1.0f;

        public List<PlayerResponseBlock> PlayerResponses;
        public CachedTag DamageResponse;
        public CameraImpulseStruct CameraImpulse;
        public CameraShakeStruct CameraShake;
        public CachedTag Sound;
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
            AffectsModelTargets = 1 << 2
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
            Shotgun
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
            CauseMagicDeceleration = 1 << 22
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

            public float CameraImpulseDuration; // seconds
            public FunctionType CameraImpulseFadeFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;

            public Angle CameraImpulseRotation; // degrees
            public float CameraImpulsePushback; // world units
            public Bounds<float> CameraImpulseJitter; // world units
        }

        [TagStructure(Size = 0x1C)]
        public class CameraShakeStruct : TagStructure
        {
            public float CameraShakeDuration; // seconds
            public FunctionType CameraShakeFalloffFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;

            public float CameraShakeRandomTranslation; // random translation in all directions (world units)
            public Angle CameraShakeRandomRotation; // random rotation in all directions (degrees)
            public GlobalPeriodicFunctionType CameraShakeWobbleFunction;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;

            public float CameraShakeWobbleFunctionPeriod; // seconds
            public float CameraShakeWobbleWeight; // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be felt when the wobble function's value is zero.
        }

        [TagStructure(Size = 0x18)]
        public class BreakingEffectStruct : TagStructure
        {
            public float BreakingEffectForwardVelocity; // world units per second
            public float BreakingEffectForwardRadius; // world units
            public float BreakingEffectForwardExponent;
            public float BreakingEffectOutwardVelocity; // world units per second
            public float BreakingEffectOutwardRadius; // world units
            public float BreakingEffectOutwardExponent;
        }
    }
}