using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x80, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x100, MinVersion = CacheVersion.HaloReach)]
    public class DecoratorSet : TagStructure
	{
        [TagField(ValidTags = new[] { "mode" })]
        public CachedTag RenderModel; // Base
        [TagField(ValidTags = new[] { "mode" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag Lod2;
        [TagField(ValidTags = new[] { "mode" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag Lod3;
        [TagField(ValidTags = new[] { "mode" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag Lod4;

        public List<StringId> RenderModelInstanceNames;
        public int ValidRenderModelInstanceNameCount;
        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTag Texture;

        public DecoratorFlags RenderFlags;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public DecoratorShader RenderShader;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public DecoratorShaderReach RenderShaderReach;

        public LightSamplingPatternValue LightSamplingPattern;
        public byte Version;

        public RealRgbColor TranslucencyABC;
        public float Translucency; // [0-1]

        // wind/underwater properties
        public Bounds<float> WavelengthXY; // direction and speed of wave through the world (world units)
        public float WaveSpeed; // waves per second through a point
        public float WaveFrequency; // (per world unit)

        public float ShadedDark; // dark side darkness
        public float ShadedBright; // bright side brightness
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x8)]
        public byte[] Padding1; // the above 2 fields and these 8 bytes are supplied directly to shaders as a 4d vector.

        public DecoratorLodTransition LodSettings;

        public List<GlobalDecoratorType> DecoratorTypes;

        [TagStructure(Size = 0x5C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)] // verify
        public class GlobalDecoratorType : TagStructure
        {
            public int Index;
            public int Mesh;
            public DecoratorTypeFlagsDefinition Flags;
            public float ScaleMin; // [0.0 - 2.0]
            public float ScaleMax; // [0.0 - 2.0]
            public float TiltMin; // degrees
            public float TiltMax; // degrees
            public float WindMin; // [0.0 - 1.0]
            public float WindMax; // [0.0 - 1.0]
            public RealRgbColor Color0;
            public RealRgbColor Color1;
            public RealRgbColor Color2;
            public float GroundTintMin; // [0.0 - 1.0]
            public float GroundTintMax; // [0.0 - 1.0]
            public float HoverMin; // [-1.0 - 1.0]
            public float HoverMax; // [-1.0 - 1.0]
            public float MinimumDistanceBetweenDecorators; // world units

            // verify these
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float PlacementWeight; // [0.0 - 1.0]
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float PostprocessedWeight;

            [Flags]
            public enum DecoratorTypeFlagsDefinition : uint
            {
                OnlyOnGround = 1 << 0,
                RandomRotation = 1 << 1,
                RotateXAxisDown = 1 << 2,
                AlignToNormal = 1 << 3,
                AlignRandom = 1 << 4
            }
        }

        [Flags]
        public enum DecoratorFlags : byte 
        {
            RenderTwoSided = 1 << 0, // takes twice as long to light
            DontSampleLightThroughGeometry = 1 << 1,
            EnabledViaPreferences = 1 << 2, // unchecked will render regardless of detail quality
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7
        }

        public enum DecoratorShader : sbyte
        {
            WindDynamicLights,                   // rasterizer\shaders\decorator_default
            StillDynamicLights,                  // rasterizer\shaders\decorator_no_wind
            StillNoLights,                       // rasterizer\shaders\decorator_static
            StillSunLightOnly,                   // rasterizer\shaders\decorator_sun   
            WavyDynamicLights,                   // rasterizer\shaders\decorator_wavy
            ShadedDynamicLights                  // rasterizer\shaders\decorator_shaded
        }

        public enum DecoratorShaderReach : sbyte
        {
            BillboardWindDynamicLights,           // rasterizer\shaders\decorator_wind_billboard
            BillboardDynamicLights,               // rasterizer\shaders\decorator_static_billboard
            SolidMeshDynamicLights,               // rasterizer\shaders\decorator_mesh
            SolidMesh,                            // rasterizer\shaders\decorator_far_object
            UnderwaterDynamicLights,              // rasterizer\shaders\decorator_underwater
            VolumetricBillboardDynamicLights,     // rasterizer\shaders\decorator_volume
            VolumetricBillboardWindDynamicLights  // rasterizer\shaders\decorator_wind_volume
        }

        public enum LightSamplingPatternValue : byte
        {
            GroundDefault,
            Hanging
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x60, MinVersion = CacheVersion.HaloReach)]
        public class DecoratorLodTransition : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float StartFade; // decorators will start to fade at this distance
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float EndFade; // decorators will fade completely and be culled at this distance

            public float EarlyCull; // cull vertices this percentage sooner than end fade [0-1]
            public float CullBlockSize; // decorators are grouped into blocks to be culled in large batches
                                        // this determines how much ground each batch covers.
                                        // Should be small if you expect to have very dense decorators,
                                        // and large if you expect them to be sparse

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DecimationSpeed;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DecimationStartDistance;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DecimateTo;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DecimationStart;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float DecimationEnd;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public int MaxValidLod;
            [TagField(Length = 4, MinVersion = CacheVersion.HaloReach)]
            public ReachLodTransition[] TransitionsReach;
        }

        [TagStructure(Size = 0x10)]
        public class ReachLodTransition : TagStructure
        {
            public float StartPoint;
            public float EndPoint;
            public float Scale;
            public float Offset;
        }
    }
}
