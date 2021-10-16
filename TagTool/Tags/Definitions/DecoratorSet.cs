using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x80)]
    public class DecoratorSet : TagStructure
	{
        public CachedTag RenderModel;
        public List<StringId> RenderModelInstanceNames;
        public int ValidRenderModelInstanceNameCount;
        public CachedTag Texture;

        public DecoratorFlags RenderFlags;
        public DecoratorShader RenderShader;
        public LightSamplingPatternValue LightSamplingPattern;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x1)]
        public byte[] Padding;

        public RealRgbColor TranslucencyABC;
        public float Translucency; // [0-1]

        // wind/underwater properties
        public Bounds<float> WavelengthXY; // direction and speed of wave through the world (world units)
        public float WaveSpeed; // waves per second through a point
        public float WaveFrequency; // (per world unit)

        public float ShadedDark; // dark side darkness
        public float ShadedBright; // bright side brightness

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x8)]
        public byte[] Padding1;

        public DecoratorLodTransition LodSettings;
        public List<GlobalDecoratorType> DecoratorTypes;

        [TagStructure(Size = 0x5C)]
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
            RenderTwoSided = 1 << 0,
            // takes twice as long to light
            DontSampleLightThroughGeometry = 1 << 1 // unchecked will render regardless of detail quality
        }

        public enum DecoratorShader : byte
        {
            WindDynamicLights,
            StillDynamicLights,
            StillNoLights,
            StillSunLightOnly,
            WavyDynamicLights,
            ShadedDynamicLights
        }

        public enum LightSamplingPatternValue : byte
        {
            GroundDefault,
            Hanging
        }

        [TagStructure(Size = 0x10)]
        public class DecoratorLodTransition : TagStructure
        {
            public float BeginFadeDistance; // decorators will start to fade at this distance
            public float FinishFadeDistance; // decorators will fade completely and be culled at this distance
            public float EarlyCull; // cull vertices this percentage sooner than end fade [0-1]
            public float CullBlockSize; // decorators are grouped into blocks to be culled in large batches
                                        // this determines how much ground each batch covers.
                                        // Should be small if you expect to have very dense decorators,
                                        // and large if you expect them to be sparse
        }
    }
}
