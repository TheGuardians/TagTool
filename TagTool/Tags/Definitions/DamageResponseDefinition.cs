using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0xC, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0x18, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "damage_response_definition", Tag = "drdf", Size = 0x18, MinVersion = CacheVersion.HaloReach)]
    public class DamageResponseDefinition : TagStructure
    {
        public List<DamageResponseClass> Classes;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<AreaControlBlockStruct> AreaControl;

        [TagField(Flags = Padding, Length = 12, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused;

        [TagStructure(Size = 0xAC, MaxVersion = CacheVersion.Halo3Beta)]
        [TagStructure(Size = 0xC0, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x84, MinVersion = CacheVersion.HaloReach)]
        public class DamageResponseClass : TagStructure
        {
            public DamageResponseClassTypeEnum Type;
            public DamageResponseClassFlags Flags;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public DamageResponseScreenFlashStruct ScreenFlash;
            [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
            public DamageResponseMotionBlurStruct MotionBlur;

            public DamageResponseDirectionalFlashStruct DirectionalFlash;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public DamageResponseMotionSensorPing MotionSensorPing;
            [TagField(ValidTags = new[] { "rmbl" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag RumbleReference;
            [TagField(ValidTags = new[] { "csdt" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag CameraShakeReachReference;
            [TagField(ValidTags = new[] { "sidt" }, MinVersion = CacheVersion.HaloReach)]
            public CachedTag SimulatedInput;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public Rumble Rumble;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public CameraShake CameraShake;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<DamageResponseGlobalSoundEffectBlockStruct> GlobalSoundEffect;

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

                [TagField(Length = 2, Flags = Padding, MinVersion = CacheVersion.Halo3Retail)]
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

            [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
            public class DamageResponseDirectionalFlashStruct : TagStructure
            {
                public float Duration;

                public GlobalReverseTransitionFunctionsEnum FadeFunction;
                [TagField(Length = 2, Flags = Padding)]
                public byte[] Padding0;

                [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
                public float Size;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float CenterSize;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float OffscreenSize;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float CenterAlpha;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                public float OffscreenAlpha;

                public float InnerScale;
                public float OuterScale;
                public RealArgbColor FlashColor;

                [TagField(MinVersion = CacheVersion.HaloReach)]
                public RealArgbColor ArrowColor;

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
        }

        [TagStructure(Size = 0x4)]
        public class DamageResponseMotionSensorPing : TagStructure
        {
            public short PingDuration; // ticks
            public short PingScale;
        }

        [TagStructure(Size = 0x18)]
        public class DamageResponseGlobalSoundEffectBlockStruct : TagStructure
        {
            public StringId EffectName;
            public TagFunction ScaleDuration; // seconds
        }

        [TagStructure(Size = 0x4C)]
        public class AreaControlBlockStruct : TagStructure
        {
            public AreaControlFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the maximum distance this player feedback will affect
            public float MaximumDistance; // world units
            public AreaControlScalarFunctionStruct DistanceFalloff;
            public AreaControlScalarFunctionStruct AngleFalloff;
            public AreaControlScalarObjectFunctionStruct ObjectFalloff;

            [Flags]
            public enum AreaControlFlags : ushort
            {
                DistanceFalloff = 1 << 0,
                AngleFalloff = 1 << 1,
                ObjectFunctionFalloff = 1 << 2,
                // use the head position and facing vector of the unit instead of the player camera
                UseUnitPosition = 1 << 3
            }

            [TagStructure(Size = 0x14)]
            public class AreaControlScalarFunctionStruct : TagStructure
            {
                public TagFunction Mapping;
            }

            [TagStructure(Size = 0x1C)]
            public class AreaControlScalarObjectFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public TagFunction Mapping;
            }
        }
    }
}