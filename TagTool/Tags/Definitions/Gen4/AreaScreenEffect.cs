using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "area_screen_effect", Tag = "sefc", Size = 0x10)]
    public class AreaScreenEffect : TagStructure
    {
        public AreaScreenEffectGlobalFlags GlobalFlags;
        public AreaScreenEffectGlobalHiddenFlags GlobalHiddenFlags;
        public List<SingleScreenEffect> ScreenEffects;
        
        [Flags]
        public enum AreaScreenEffectGlobalFlags : ushort
        {
            PlaySequentiallyIgnoreDelaySettings = 1 << 0,
            DebugThisScreenEffect = 1 << 1,
            ForceDeleteWhenAttachedToDeadObject = 1 << 2,
            ForceMaxOnePerObject = 1 << 3,
            ForceLooping = 1 << 4,
            OnlySpawnKillcamVersionDuringKillcam = 1 << 5
        }
        
        [Flags]
        public enum AreaScreenEffectGlobalHiddenFlags : ushort
        {
            UpdateThread = 1 << 0,
            RenderThread = 1 << 1
        }
        
        [TagStructure(Size = 0x118)]
        public class SingleScreenEffect : TagStructure
        {
            public StringId Name;
            public AreaScreenEffectFlags Flags;
            public AreaScreenEffectHiddenFlags HiddenFlags;
            // the maximum distance this screen effect will affect
            public float MaximumDistance; // world units
            public ScreenEffectScalarFunctionStruct DistanceFalloff;
            // the effect will start after this many seconds (ignored if the play sequentially flag is set)
            public float Delay; // seconds
            // the effect is destroyed after this many seconds (0 = never dies)
            public float Lifetime; // seconds
            public ScreenEffectScalarFunctionStruct TimeFalloff;
            public ScreenEffectScalarFunctionStruct AngleFalloff;
            public ScreenEffectScalarObjectFunctionStruct ObjectFalloff;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ColorGradingLookUpTexture;
            // do not edit
            public float RuntimeColorGradingStrength; // [do not edit]
            // increase in exposure
            public float ExposureBoost; // stops
            // decrease in exposure
            public float ExposureDeboost; // stops
            // shifts hue R->G->B
            public float HueLeft; // degrees [0-360]
            // shifts hue B->G->R
            public float HueRight; // degrees [0-360]
            // increases saturation
            public float Saturation; // [0-1]
            // decreases saturation
            public float Desaturation; // [0-1]
            // contrast increase
            public float ContrastEnhance; // [0-1]
            // gamma increase
            public float GammaEnhance; // [0-10]
            // gamma increase
            public float GammaReduce; // [0-10]
            // add bright noise contribution, 0 turns it off, 1
            public float BrightNoise; // [0-1]
            // add dark noise contribution, 0 turns it off, 1
            public float DarkNoise; // [0-1]
            // this color is multiplied on top
            public RealRgbColor ColorFilter;
            // this color is subtracted
            public RealRgbColor ColorFloor;
            // used to blend in the color replace below
            public float ColorReplaceStrength; // [0-1]
            // this color is blended in place of the screen's color
            public RealRgbColor ColorReplace;
            // adds a full-screen tron effect
            public float Tron; // [0-1]
            // adds motion-blur towards or away from this screen effect
            public float MotionSuck; // [-2, 2]
            // adds cheap bloom buffer motion-blur towards or away from this screen effect
            public float BloomBufferMotionSuck; // [-2, 2]
            // do not edit
            public RealVector3d MotionSuckDirection; // [do not edit]
            // blurs the entire screen
            public float HorizontalBlur; // [0-10] expensive
            // blurs the entire screen
            public float VerticalBlur; // [0-10] expensive
            // turns on the global vision mode
            public float VisionMode; // [0-1]
            // fades the chud
            public float HudFade; // [0-1]
            // zooms in the field of view
            public float FovIn; // [0-1]
            // zooms out the field of view
            public float FovOut; // [0-1]
            // shakes the entire screen
            public float ScreenShake; // [0-1]
            // applies this shader to the entire screen
            [TagField(ValidTags = new [] { "mat " })]
            public CachedTag ShaderEffect;
            
            [Flags]
            public enum AreaScreenEffectFlags : ushort
            {
                DebugDisable = 1 << 0,
                AllowEffectOutsideRadius = 1 << 1,
                Unattached = 1 << 2,
                FirstPerson = 1 << 3,
                ThirdPerson = 1 << 4,
                // disables distance and angle falloffs
                DisableCameraFalloffs = 1 << 5,
                OnlyAffectsAttachedObject = 1 << 6,
                DrawPreciselyOne = 1 << 7
            }
            
            [Flags]
            public enum AreaScreenEffectHiddenFlags : ushort
            {
                UpdateThread = 1 << 0,
                RenderThread = 1 << 1
            }
            
            [TagStructure(Size = 0x14)]
            public class ScreenEffectScalarFunctionStruct : TagStructure
            {
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class ScreenEffectScalarObjectFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
