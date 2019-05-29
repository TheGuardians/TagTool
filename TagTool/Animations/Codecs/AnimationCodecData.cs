using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public abstract class AnimationCodecData
    {
        public List<List<int>> RotationKeyframes;
        public List<List<RealQuaternion>> Rotations;

        public List<List<int>> TranslationKeyframes;
        public List<List<RealPoint3d>> Translations;

        public List<List<int>> ScaleKeyframes;
        public List<List<float>> Scales;

        public abstract void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader);
        public abstract void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer);
    }
}