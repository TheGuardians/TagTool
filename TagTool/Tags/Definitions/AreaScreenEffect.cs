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
    public class AreaScreenEffect : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public GlobalFlagBits GlobalFlags;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public HiddenFlagBits GlobalHiddenFlags;

        public List<ScreenEffectBlock> ScreenEffects;

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
        public class ScreenEffectBlock : TagStructure
		{
            [TagField(Label = true)]
            public StringId Name;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public ushort Unknown;

            public FlagBits Flags;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
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

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Delay;

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


            ///OBJECT FALLOFF 
            ///applies a falloff based on an object function - ignored if the effect is not attached to an object
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public StringId InputVariable;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public StringId RangeVariable;
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public TagFunction ObjectFalloff = new TagFunction { Data = new byte[0] };
            
            public float LightIntensity;
            public float PrimaryHue; ///degrees [0-360] shifts hue R>G>B
            public float SecondaryHue; ///degrees [0-360] shifts hue R>G>B
            public float Saturation; ///[0-1]#increases saturation
            public float Desaturation; ///[0-1]#decreases saturation
            public float GammaIncrease; ///[0-10]#gamma increase
            public float GammaDecrease; ///[0-10]#gamma decrease
            public float ShadowBrightness;
            public RealRgbColor ColorFilter; ///#this color is multiplied on top
            public RealRgbColor ColorFloor; ///#this color is subtracted

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Tron; ///tron:[0-1]#adds a full-screen tron effect

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float RadialBlur; ///{motion suck}:[-2, 2]#adds motion-blur towards or away from this screen effect
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public RealVector3d RadialBlurDirection; ///{motion suck direction}!:[do not edit]#do not edit" />

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HorizontalBlur; ///[0-10] expensive#blurs the entire screen
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float VerticalBlur; ///[0-10] expensive#blurs the entire screen

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float Vision; ///[0-1]#turns on the global vision mode

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown4;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HudTransparency; ///[0-1]#fades the chud

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float FovIn;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float FovOut;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float ScreenShake;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTagInstance ScreenShader; ///#applies this shader to the entire screen

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