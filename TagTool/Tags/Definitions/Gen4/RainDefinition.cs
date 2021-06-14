using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "rain_definition", Tag = "rain", Size = 0x140)]
    public class RainDefinition : TagStructure
    {
        // INTERNAL TAG USE ONLY DO NOT CHANGE
        public int Version;
        // fade control for all rain effects
        public float RainAmount; // [0,1]
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag Effect;
        [TagField(ValidTags = new [] { "sefc" })]
        public CachedTag ScreenEffect;
        [TagField(ValidTags = new [] { "cfxs" })]
        public CachedTag CameraFx;
        public RainParticleFlags Flags;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag DropTexture;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag SplashTexture;
        public float Density; // particles per square world unit
        // k -1 is down
        public RealVector3d Direction;
        public float FallSpeed; // world units per second
        // size of the rain particles
        public float Size; // world units
        // the max velocity in which we allow world-relative vertical rain motion.  over this velocity we clamp to
        // camera-relative vertical rain motion
        public float MaxVerticalCameraRelativeMotion; // world units per second
        // height to width ratio
        public float MaxAspectRatio;
        // intensity will scale inversely with aspect ratio, this clamps the aspect ratio before scaling
        public float MinAspectForIntensity;
        // tints the rain drops
        public RealRgbColor TintColor;
        // brightness of the rain drops
        public float Intensity;
        // transparency of the rain drops
        public float AlphaScale;
        // distance at which the drops fade out
        public float DropNearFadeDistance; // world units
        // depth range over which the particle will search for collisions with the depth buffer
        public float CollisionRange; // world units
        // length of a side of the splash particle card
        public float SplashSize; // world units
        // height off the ground of the center of the splash particle
        public float SplashHeight; // world units
        // how long the splash lasts
        public float SplashLifetime; // seconds
        // distance at which the splash fades out
        public float NearFadeDistance; // world units
        // tints the splashes
        public RealRgbColor SplashTint;
        // brightness of the splashes
        public float SplashIntensity;
        // transparency of the splashes
        public float SplashAlpha;
        // size of the ripple at impact
        public float RippleInitialSize;
        // maximum size of the ripple
        public float RippleMaxSize;
        // how long the ripples last
        public float RippleLifetime; // seconds
        // intensifies ripples
        public float RippleIntensity;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag RainSheetTexture;
        public float RainSheetSpeed;
        public float RainSheetIntervals;
        public float RainSheetMinimumDistance;
        public float RainSheetIntensity;
        public float RainTextureTileScale;
        public float RainSheetParallaxSpeed;
        public float RainSheetDepthFade;
        public float TransparentSortDistance;
        public float TransparentSortLayer;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag LightVolumeTexture;
        public float LightVolumeIntensity;
        public float LightVolumeTextureScale;
        public float RainDropParticleIntensity;
        public float FarthestRainParticleDistance;
        public float ClosestRainSheetDistance;
        // make it longer when it is far
        public float RainDropLengthCompensation;
        public float WetnessFadeInTime; // seconds
        public float WetnessFadeOutTime; // seconds
        public float DimOfPointLight;
        public float DimOfImposters;
        public float DimOfDecorators;
        
        [Flags]
        public enum RainParticleFlags : uint
        {
            AlphaBlend = 1 << 0,
            DisableCollision = 1 << 1
        }
    }
}
