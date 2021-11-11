using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0x18, MinVersion = CacheVersion.HaloOnlineED)]
    public class DamageResponseDefinition : TagStructure
	{
        public List<DamageResponseClass> Classes;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED)]
        public byte[] Unused;

        [TagStructure(Size = 0xC0)]
        public class DamageResponseClass : TagStructure
        {
            public DamageResponseClassTypeEnum Type;
            public DamageResponseClassFlags Flags;
            public DamageResponseScreenFlashStruct ScreenFlash;
            public DamageResponseMotionBlurStruct MotionBlur;
            public DamageResponseDirectionalFlashStruct DirectionalFlash;
            public DamageResponseRumbleStruct Rumble;
            public DamageResponseCameraImpulseStruct CameraImpulse;
            public DamageResponseCameraShakeStruct CameraShake;

            public enum DamageResponseClassTypeEnum : short
            {
                Shielded,
                Unshielded,
                All
            }

            [Flags]
            public enum DamageResponseClassFlags : ushort
            {
                IgnoreOnNoDamage = 1 << 0
            }

            [TagStructure(Size = 0x20)]
            public class DamageResponseScreenFlashStruct : TagStructure
            {
                public ScreenFlashTypeEnum Type;
                public ScreenFlashPriorityEnum Priority;
                public float Duration;
                public GlobalReverseTransitionFunctionsEnum FadeFunction;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                public float MaxIntensity;
                public RealArgbColor FlashColor;

                public enum ScreenFlashTypeEnum : short
                {
                    Add,
                    Multiply,
                    AlphaBlend,
                    DoubleMultiply,
                    Max,
                    MultiplyAdd,
                    InvAlphaBlend
                }

                public enum ScreenFlashPriorityEnum : short
                {
                    Low,
                    Medium,
                    High
                }

                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }

            [TagStructure(Size = 0x14)]
            public class DamageResponseMotionBlurStruct : TagStructure
            {
                public float Duration;
                public ScreenFlashPriorityEnum Priority;
                public GlobalReverseTransitionFunctionsEnum FadeFunction;
                public float MotionBlurEffectScale;
                public float MotionBlurCapScale;
                public float MotionBlurCenterFalloffScale;

                public enum ScreenFlashPriorityEnum : short
                {
                    Low,
                    Medium,
                    High
                }

                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }

            [TagStructure(Size = 0x24)]
            public class DamageResponseDirectionalFlashStruct : TagStructure
            {
                public float Duration;
                public GlobalReverseTransitionFunctionsEnum FadeFunction;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                public float Size;
                public float InnerScale;
                public float OuterScale;
                public RealArgbColor FlashColor;

                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }

            [TagStructure(Size = 0x30)]
            public class DamageResponseRumbleStruct : TagStructure
            {
                public DamageResponseRumbleFrequencyStruct LowFrequencyRumble;
                public DamageResponseRumbleFrequencyStruct HighFrequencyRumble;

                [TagStructure(Size = 0x18)]
                public class DamageResponseRumbleFrequencyStruct : TagStructure
                {
                    public float Duration; // seconds
                    public MappingFunction DirtyRumble;

                    [TagStructure(Size = 0x14)]
                    public class MappingFunction : TagStructure
                    {
                        public byte[] Data;
                    }
                }
            }

            [TagStructure(Size = 0x18)]
            public class DamageResponseCameraImpulseStruct : TagStructure
            {
                public float Duration; // seconds
                public GlobalReverseTransitionFunctionsEnum FadeFunction;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                public Angle Rotation; // degrees
                public float Pushback; // world units
                public Bounds<float> Jitter; // world units

                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }
            }

            [TagStructure(Size = 0x1C)]
            public class DamageResponseCameraShakeStruct : TagStructure
            {
                // the effect will last for this duration.
                public float ShakeDuration; // seconds
                // a function to envelope the effect's magnitude over time
                public GlobalReverseTransitionFunctionsEnum FalloffFunction;
                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;
                // random translation in all directions
                public float RandomTranslation; // world units
                // random rotation in all directions
                public Angle RandomRotation; // degrees
                // a function to perturb the effect's behavior over time
                public GlobalPeriodicFunctionsEnum WobbleFunction;

                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding1;

                public float WobbleFunctionPeriod; // seconds
                // a value of 0.0 signifies that the wobble function has no effect; a value of 1.0 signifies that the effect will not be felt when the wobble function's value is zero.
                public float WobbleWeight;

                public enum GlobalReverseTransitionFunctionsEnum : short
                {
                    Linear,
                    Late,
                    VeryLate,
                    Early,
                    VeryEarly,
                    Cosine,
                    Zero,
                    One
                }

                public enum GlobalPeriodicFunctionsEnum : short
                {
                    One,
                    Zero,
                    Cosine,
                    CosinevariablePeriod,
                    DiagonalWave,
                    DiagonalWavevariablePeriod,
                    Slide,
                    SlidevariablePeriod,
                    Noise,
                    Jitter,
                    Wander,
                    Spark
                }
            }
        }
    }
}