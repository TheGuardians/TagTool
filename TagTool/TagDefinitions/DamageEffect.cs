using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF0, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "damage_effect", Tag = "jpt!", Size = 0xF4, MinVersion = CacheVersion.HaloOnline106708)]
    public class DamageEffect
    {
        public float RadiusMin;
        public float RadiusMax;
        public float CutoffScale;
        public uint Flags;
        public SideEffectValue SideEffect;
        public CategoryValue Category;
        public uint Flags2;
        public float AreaOfEffectCoreRadius;
        public float DamageLowerBound;
        public float DamageUpperBoundMin;
        public float DamageUpperBoundMax;
        public Angle DamageInnerConeAngle;
        public Angle DamageOuterConeAngle;
        public float ActiveCamoflageDamage;
        public float Stun;
        public float MaxStun;
        public float StunTime;
        public float InstantaneousAcceleration;
        public float RiderDirectDamageScale;
        public float RiderMaxTransferDamageScale;
        public float RiderMinTransferDamageScale;
        public StringId GeneralDamage;
        public StringId SpecificDamage;
        public StringId SpecialDamage;
        public float AiStunRadius;
        public float AiStunBoundsMin;
        public float AiStunBoundsMax;
        public float ShakeRadius;
        public float EmpRadius;
        public float Unknown1;
        public float Unknown2;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public float Unknown3;

        public List<PlayerResponseBlock> PlayerResponses;
        public CachedTagInstance DamageResponse;
        public float Duration;
        public FunctionType FadeFunction;
        public short Unknown4;
        public Angle Rotation;
        public float Pushback;
        public float JitterMin;
        public float JitterMax;
        public float Duration2;
        public FunctionType FalloffFunction;
        public short Unknown5;
        public float RandomTranslation;
        public Angle RandomRotation;
        public WobbleFunctionValue WobbleFunction;
        public short Unknown6;
        public float WobbleFunctionPeriod;
        public float WobbleWeight;
        public CachedTagInstance Sound;
        public float ForwardVelocity;
        public float ForwardRadius;
        public float ForwardExponent;
        public float OutwardVelocity;
        public float OutwardRadius;
        public float OutwardExponent;

        [TagField(Padding = true, Length = 4, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;
        
        public enum SideEffectValue : short
        {
            None,
            Harmless,
            LethalToTheUnsuspecting,
            Emp,
        }

        public enum CategoryValue : short
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
        }

        [TagStructure(Size = 0x70)]
        public class PlayerResponseBlock
        {
            public ResponseTypeValue ResponseType;
            public short Unknown;
            public TypeValue Type;
            public PriorityValue Priority;
            public float Duration;
            public FunctionType FadeFunction;
            public short Unknown2;
            public float MaximumIntensity;
            public float ColorAlpha;
            public float ColorRed;
            public float ColorGreen;
            public float ColorBlue;
            public float LowFrequencyVibrationDuration;
            public TagFunction LowFrequencyVibrationFunction = new TagFunction { Data = new byte[0] };
            public float HighFrequencyVibrationDuration;
            public TagFunction HighFrequencyVibrationFunction = new TagFunction { Data = new byte[0] };
            public StringId EffectName;
            public float Duration2;
            public TagFunction EffectScaleFunction = new TagFunction { Data = new byte[0] };

            public enum ResponseTypeValue : short
            {
                Shielded,
                Unshielded,
                All,
            }

            public enum TypeValue : short
            {
                None,
                Lighten,
                Darken,
                Max,
                Min,
                Invert,
                Tint,
            }

            public enum PriorityValue : short
            {
                Low,
                Medium,
                High,
            }
        }

        public enum WobbleFunctionValue : short
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
            Spark,
        }
    }
}
