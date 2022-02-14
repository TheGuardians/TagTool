using TagTool.Cache;
using TagTool.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_animation_definition", Tag = "chad", Size = 0x5C)]
    public class ChudAnimationDefinition : TagStructure
	{
        public ChudAnimationFlags Flags;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;

        public List<PositionBlock> Position;
        public List<RotationBlock> Rotation;
        public List<ScaleBlock> Scale;
        public List<ColorBlock> Color;
        public List<ScalarBlock> Alpha;
        public List<ScalarBlock> Flash;
        public List<TextureBlock> Texture;
        public int RuntimePeriodMsec;

        [Flags]
        public enum ChudAnimationFlags : ushort
        {
            Spline = 1 << 0,
            Loop = 1 << 1,
            LoopSeesaw = 1 << 2,
            DoNotCorrectTranslation = 1 << 3
        }

        [TagStructure(Size = 0x20)]
        public class PositionBlock : TagStructure
		{
            public List<AnimationBlock> Keyframes;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x10)]
            public class AnimationBlock : TagStructure
			{
                public int TimeOffset; // milliseconds
                public RealPoint3d Position;
            }
        }

        [TagStructure(Size = 0x20)]
        public class RotationBlock : TagStructure
		{
            public List<AnimationBlock> Keyframes;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x10)]
            public class AnimationBlock : TagStructure
			{
                public int FrameNumber;
                public RealEulerAngles3d Angles;
            }
        }

        [TagStructure(Size = 0x20)]
        public class ScaleBlock : TagStructure
		{
            public List<AnimationBlock> Keyframes;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0xC)]
            public class AnimationBlock : TagStructure
			{
                public int TimeOffset; // milliseconds
                public RealPoint2d Scale;
            }
        }

        [TagStructure(Size = 0x20)]
        public class ColorBlock : TagStructure
		{
            public List<AnimationBlock> Keyframes;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            public class AnimationBlock : TagStructure
			{
                public int TimeOffset; // milliseconds

                [TagField(MinVersion = CacheVersion.HaloOnlineED)]
                [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public ChudKeyframeColorSourceEnum ColorSource;

                [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED)]
                [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
                public byte[] XYJAZZQJ;

                public enum ChudKeyframeColorSourceEnum : short
                {
                    Taco
                }
            }
        }

        [TagStructure(Size = 0x20)]
        public class ScalarBlock : TagStructure
		{
            public List<ChudAnimationScalarBlock> Keyframes;
            public TagFunction DefaultFunction = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x8)]
            public class ChudAnimationScalarBlock : TagStructure
			{
                public int TimeOffset; // milliseconds
                public float ScalarValue;
            }
        }

        [TagStructure(Size = 0x20)]
        public class TextureBlock : TagStructure
		{
            public List<AnimationBlock> Keyframes;
            public TagFunction DefaultFunction = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x14)]
            public class AnimationBlock : TagStructure
			{
                public int TimeOffset; // milliseconds
                public RealPoint2d TextureScale;
                public RealPoint2d TextureOffset;
            }
        }
    }
}