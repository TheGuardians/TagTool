using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decorator_set", Tag = "dctr", Size = 0x80)]
    public class DecoratorSet : TagStructure
	{
        public CachedTag Model;
        public List<StringId> ValidRenderModelInstanceNames;
        public int ValidRenderModelInstances;
        public CachedTag Texture;

        public DecoratorFlags RenderFlags;
        public DecoratorShader RenderShader;
        public LightSamplingPatternValue LightSamplingPattern;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x1)]
        public byte[] Unused1;

        public RealRgbColor TranslucencyABC;
        public float MaterialTranslucency;

        // wind/underwater properties
        public Bounds<float> WavelengthXY;
        public float WaveSpeed;
        public float WaveFrequency;

        public float Darkness;                // dark side darkness
        public float Brightness;              // bright side brightness

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x8)]
        public byte[] Unused2;

        public DecoratorLodTransition LodSettings;

        // this is either decorator types or padding, need to find code to confirm
        [TagField(Flags = TagFieldFlags.Padding, Length = 0xC)]
        public byte[] Unused3;

        [Flags]
        public enum DecoratorFlags : byte 
        {
            TwoSided,
            bit1, // unused?
            EnabledViaPreferences // unchecked will render regardless of detail quality
        }

        public enum DecoratorShader : byte
        {
            Default,
            NoWind,
            Static,
            Sun,
            Wavy,
            Shaded
        }

        public enum LightSamplingPatternValue : byte
        {
            Default,
            Hanging
        }

        [TagStructure(Size = 0x10)]
        public class DecoratorLodTransition : TagStructure
        {
            public float StartDistance;
            public float EndDistance;
            public float Scale;
            public float Offset;
        }
    }
}
