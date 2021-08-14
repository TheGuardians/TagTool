using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0x20)]
    public class FacialAnimationResourceDefinition : TagStructure
    {
        public TagBlock<FacialAnimationPermutationBlockStruct> FacialAnimationPermutations;
        public TagData CompressedFacialAnimationCurveData;
        
        [TagStructure(Size = 0x1C)]
        public class FacialAnimationPermutationBlockStruct : TagStructure
        {
            public float StartTime;
            public float EndTime;
            public float BlendIn;
            public float BlendOut;
            public List<FacialAnimationCurveBlockStruct> FacialAnimationCurves;
            
            [TagStructure(Size = 0xC)]
            public class FacialAnimationCurveBlockStruct : TagStructure
            {
                public FacialAnimationTrackEnum FacialAnimationTrack;
                public int FacialAnimationCurveDataOffset;
                public int FacialAnimationCurveDataSize;
                
                public enum FacialAnimationTrackEnum : int
                {
                    Silence,
                    Eat,
                    Earth,
                    If,
                    Ox,
                    Oat,
                    Wet,
                    Size,
                    Church,
                    Fave,
                    Though,
                    Told,
                    Bump,
                    New,
                    Roar,
                    Cage,
                    EyebrowRaise,
                    Blink,
                    OrientationHeadPitch,
                    OrientationHeadRoll,
                    OrientationHeadYaw,
                    EmphasisHeadPitch,
                    EmphasisHeadRoll,
                    EmphasisHeadYaw,
                    GazeEyePitch,
                    GazeEyeYaw,
                    Happy,
                    Sad,
                    Angry,
                    Disgusted,
                    Scared,
                    Surprised,
                    Pain,
                    Shout
                }
            }
        }
    }
}
