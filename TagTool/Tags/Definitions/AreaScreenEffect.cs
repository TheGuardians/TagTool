using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0xC, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0xC, MinVersion = CacheVersion.HaloOnline106708)]
    public class AreaScreenEffect
    {
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public GlobalFlagBits GlobalFlags;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public HiddenFlagBits GlobalHiddenFlags;

        public List<ScreenEffectBlock> ScreenEffects;

        [TagField(Padding = true, Length = 12, MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused;

        [Flags]
        public enum GlobalFlagBits : ushort
        {
            None,
            PlaySequentially = 1 << 0,
            DebugThisScreenEffect = 1 << 1,
            ForceDeleteWhenAttachedToDeadObject = 1 << 2,
            ForceMaxOnePerObject = 1 << 3,
            ForceLooping = 1 << 4
        }

        [Flags]
        public enum HiddenFlagBits : ushort
        {
            None = 0,
            UpdateThread = 1 << 0,
            RenderThread = 1 << 1
        }

        [TagStructure(Size = 0x84, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0xE8, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x9C, MinVersion = CacheVersion.HaloOnline106708)]
        public class ScreenEffectBlock
        {
            [TagField(Label = true)]
            public StringId Name;
            public FlagBits Flags;
            public HiddenFlagBits HiddenFlags;

            //
            //  DISTANCE FALLOFF:
            //      Controls the maximum distance and the distance falloff of this effect.
            //      NOTE: Not used for scenario global effects
            //

            /// <summary>
            /// The maximum distance this screen effect will affect.
            /// </summary>
            public float MaximumDistance;

            /// <summary>
            /// The function data of the distance falloff.
            /// </summary>
            public TagFunction DistanceFalloffFunction = new TagFunction { Data = new byte[0] };

            //
            //  TIME EVOLUTION:
            //      Controls the lifetime and time falloff of this effect.
            //      NOTE: Not used for scenario global effects
            //

            /// <summary>
            /// The effect is destroyed after this many seconds. (0 = never dies)
            /// </summary>
            public float Duration;

            /// <summary>
            /// The function data of the time evolution.
            /// </summary>
            public TagFunction TimeEvolutionFunction = new TagFunction { Data = new byte[0] };

            //
            //  ANGLE FALLOFF:
            //      Controls the falloff of this effect based on how close you are to looking directly at it.
            //      NOTE: not used for scenario global effects
            //

            /// <summary>
            /// The function data of the angle falloff.
            /// </summary>
            public TagFunction AngleFalloffFunction = new TagFunction { Data = new byte[0] };

            public float LightIntensity;
            public float PrimaryHue;
            public float SecondaryHue;
            public float Saturation;
            public float Desaturation;
            public float GammaIncrease;
            public float GammaDecrease;
            public float ShadowBrightness;
            public RealRgbColor ColorFilter;
            public RealRgbColor ColorFloor;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Holograph;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Drunk;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown1;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown2;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown3;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HorizontalBlur;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float VerticalBlur;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Tracing;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HudTransparency;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float ZoomIn;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float ZoomOut;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Turbulence;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTagInstance ScreenShader;

            [Flags]
            public enum FlagBits : ushort
            {
                None = 0,
                DebugDisable = 1 << 0,
                AllowEffectOutsideRadius = 1 << 1,
                Unattached = 1 << 2,
                FirstPersonOnly = 1 << 3,
                ThirdPersonOnly = 1 << 4,
                DisableCinematicCameraFalloffs = 1 << 5,
                OnlyAffectsAttachedObject = 1 << 6,
                DrawPreciselyOne = 1 << 7,
                UsePlayerTravelDirection = 1 << 8
            }
        }
    }
}