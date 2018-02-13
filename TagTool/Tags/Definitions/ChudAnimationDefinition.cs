using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "chud_animation_definition", Tag = "chad", Size = 0x5C)]
    public class ChudAnimationDefinition
    {
        public ushort Flags;
        public short Unknown;
        public List<PositionBlock> Position;
        public List<RotationBlock> Rotation;
        public List<SizeBlock> Size;
        public List<ColorBlock> Color;
        public List<AlphaBlock> Alpha;
        public List<AlphaUnknownBlock> AlphaUnknown;
        public List<BitmapBlock> Bitmap;
        public int NumberOfFrames;

        [TagStructure(Size = 0x20)]
        public class PositionBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x10)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public RealPoint3d Position;
            }
        }

        [TagStructure(Size = 0x20)]
        public class RotationBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x10)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public RealEulerAngles3d Angles;
            }
        }

        [TagStructure(Size = 0x20)]
        public class SizeBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0xC)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public RealPoint2d Stretch;
            }
        }

        [TagStructure(Size = 0x20)]
        public class ColorBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x8, MinVersion = CacheVersion.HaloOnline106708)]
            public class AnimationBlock
            {
                public int FrameNumber;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public uint Unknown;
            }
        }

        [TagStructure(Size = 0x20)]
        public class AlphaBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x8)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public float Alpha;
            }
        }

        [TagStructure(Size = 0x20)]
        public class AlphaUnknownBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x8)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public float Alpha;
            }
        }

        [TagStructure(Size = 0x20)]
        public class BitmapBlock
        {
            public List<AnimationBlock> Animation;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            [TagStructure(Size = 0x14)]
            public class AnimationBlock
            {
                public int FrameNumber;
                public RealPoint2d Movement1;
                public RealPoint2d Movement2;
            }
        }
    }
}