using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0xC, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0x10, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0xC, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0x10, MinVersion = CacheVersion.HaloReach)]
    public class AreaScreenEffect : TagStructure
	{
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public GlobalFlagBits GlobalFlags;
        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
        public HiddenFlagBits GlobalHiddenFlags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public GlobalFlagBits GlobalFlagsReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public HiddenFlagBits GlobalHiddenFlagsReach;

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
        [TagStructure(Size = 0x9C, MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0xF0, MinVersion = CacheVersion.HaloReach)]
        public class ScreenEffectBlock : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            [TagField(MaxVersion = CacheVersion.Halo3Retail)]
            public ushort Unknown;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public FlagBits Flags;
            [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
            public FlagBits_HO Flags_HO;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public FlagBits FlagsReach;

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
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DelayReach;

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
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId InputVariableReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId RangeVariableReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public TagFunction ObjectFalloffReach = new TagFunction { Data = new byte[0] };

            public float LightIntensity;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Darkness;
            public float PrimaryHue; ///degrees [0-360] shifts hue R>G>B
            public float SecondaryHue; ///degrees [0-360] shifts hue R>G>B
            public float Saturation; ///[0-1]#increases saturation
            public float Desaturation; ///[0-1]#decreases saturation
            public float GammaIncrease; ///[0-10]#gamma increase
            public float GammaDecrease; ///[0-10]#gamma decrease
            public float ShadowBrightness;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float BrightNoise;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DarkNoise;
            public RealRgbColor ColorFilter; ///#this color is multiplied on top
            public RealRgbColor ColorFloor; ///#this color is subtracted

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Tron; ///tron:[0-1]#adds a full-screen tron effect
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TronReach; ///tron:[0-1]#adds a full-screen tron effect

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float RadialBlur; ///{motion suck}:[-2, 2]#adds motion-blur towards or away from this screen effect
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public RealVector3d RadialBlurDirection; ///{motion suck direction}!:[do not edit]#do not edit" />
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RadialBlurReach; ///{motion suck}:[-2, 2]#adds motion-blur towards or away from this screen effect
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealVector3d RadialBlurDirectionReach; ///{motion suck direction}!:[do not edit]#do not edit" />

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HorizontalBlur; ///[0-10] expensive#blurs the entire screen
            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float VerticalBlur; ///[0-10] expensive#blurs the entire screen
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float HorizontalBlurReach; ///[0-10] expensive#blurs the entire screen
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float VerticalBlurReach; ///[0-10] expensive#blurs the entire screen

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
            public float Vision; ///[0-1]#turns on the global vision mode

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float Unknown4;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float Unknown4Reach;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float HudTransparency; ///[0-1]#fades the chud
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float HudTransparencyReach;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float FovIn;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FovInReach;

            [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST)]
            public float FovOut;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FovOutReach;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public float ScreenShake;

            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public CachedTag ScreenShader; ///#applies this shader to the entire screen

            [Flags]
            public enum FlagBits_HO : ushort
            {
                None = 0,
                DebugDisable = 1 << 0,
                AllowEffectOutsideRadius = 1 << 1,
                FirstPersonOnly = 1 << 2,
                ThirdPersonOnly = 1 << 3,
                DisableCinematicCameraFalloffs = 1 << 4,
                OnlyAffectsAttachedObject = 1 << 5,
                DrawPreciselyOne = 1 << 6,
                UsePlayerTravelDirection = 1 << 7,
                Bit8 = 1 << 8,
                UseNameAsStringIDInput = 1 << 9, //these last two are custom flags to allow for stringid control of sefc
                InvertStringIDInput = 1 << 10
            }

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
                UsePlayerTravelDirection = 1 << 8,
            }
        }
    }
}