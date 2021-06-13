using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x100)]
    public class DecoratorSet : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag Base;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag Lod2;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag Lod3;
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag Lod4;
        public List<DecoratorSetInstanceNameBlock> RenderModelInstanceNames;
        public int RenderModelInstanceNameValidCount;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Texture;
        public DecoratorSetRenderFlags RenderFlags;
        public DecoratorSetRenderShaderEnum RenderShader;
        public DecoratorSetLightingSamplePatternEnum LightSamplingPattern;
        public byte Version;
        // postprocessed value
        public float TranslucencyA; // dont touch
        // postprocessed value
        public float TranslucencyB; // dont touch
        // postprocessed value
        public float TranslucencyC; // dont touch
        // how translucent the material is (0 = opaque, 1 = both sides same intensity), only affects dynamic lights
        public float Translucency; // [0..1]
        // direction and speed of wave through the world
        public float WavelengthX; // world units
        // direction and speed of wave through the world
        public float WavelengthY; // world units
        // waves per second through a point
        public float WaveSpeed; // per second
        // number of waves per world unit
        public float WaveFrequency; // per world unit
        // how dark is the dark side of a shaded decorator
        public float ShadedDark;
        // the bright side of a shaded decorator is this much brighter than the dark side
        public float ShadedBright;
        public float Unused1;
        public float Unused2;
        // cull vertices this percentage sooner than LOD3-nothing transition
        public float EarlyCull; // [0 - 1]
        // decorators are grouped into blocks to be culled in large batches, this determines how much ground each batch
        // covers.  Should be small if you expect to have very dense decorators, and large if you expect them to be sparse
        public float CullBlockSize; // [0.5-100]world units
        // 1 is default
        public float DecimationSpeed; // [0 - infinite]
        // [block size by default] decimation offset of the starting point
        public float DecimationStartDistance;
        // 0 means all the way removed. 100 means nothing
        public float DecimateTo;
        public float DecimationStart; // world units
        public float DecimationEnd; // world units
        public int MaxValidLod;
        public float StartPoint0;
        public float EndPoint0;
        public float Scale0;
        public float Offset0;
        public float StartPoint1;
        public float EndPoint1;
        public float Scale1;
        public float Offset1;
        public float StartPoint2;
        public float EndPoint2;
        public float Scale2;
        public float Offset2;
        public float StartPoint3;
        public float EndPoint3;
        public float Scale3;
        public float Offset3;
        public List<GlobalDecoratorTypeStruct> DecoratorTypes;
        
        [Flags]
        public enum DecoratorSetRenderFlags : byte
        {
            RenderTwoSided = 1 << 0,
            // takes twice as long to light
            DontSampleLightThroughGeometry = 1 << 1,
            UseDecimationMethodForRandomStructuredDecorators = 1 << 2,
            // or there could only be 10x10 blocks per cluster
            MoreStrictlyRespectBlockSize = 1 << 3,
            // only matters if the do not desaturate decorators checkbox is unchecked in the structure bsps block of the .scenario
            // tag
            DoNotDesaturate = 1 << 4
        }
        
        public enum DecoratorSetRenderShaderEnum : sbyte
        {
            BillboardWindDynamicLights,
            BillboardDynamicLights,
            SolidMeshDynamicLights,
            SolidMesh,
            UnderwaterDynamicLights,
            VolumetricBillboardDynamicLights,
            VolumetricBillboardWindDynamicLights
        }
        
        public enum DecoratorSetLightingSamplePatternEnum : sbyte
        {
            GroundDefault,
            Hanging
        }
        
        [TagStructure(Size = 0x4)]
        public class DecoratorSetInstanceNameBlock : TagStructure
        {
            public StringId Name;
        }
        
        [TagStructure(Size = 0x64)]
        public class GlobalDecoratorTypeStruct : TagStructure
        {
            public int Index;
            public int Mesh;
            public DecoratorTypeFlags Flags;
            public float ScaleMin; // [0.0 - 5.0]
            public float ScaleMax; // [0.0 - 5.0]
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
            public float PlacementWeight; // [0.0 - 1.0]
            public float PostprocessedWeight;
            
            [Flags]
            public enum DecoratorTypeFlags : uint
            {
                OnlyOnGround = 1 << 0,
                RandomRotation = 1 << 1,
                RotateXAxisDown = 1 << 2,
                AlignToNormal = 1 << 3,
                AlignRandom = 1 << 4
            }
        }
    }
}
