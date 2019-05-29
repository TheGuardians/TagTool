using System.Collections.Generic;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public abstract class AnimationData
    {
        public List<List<RealQuaternion>> RotationFrames;
        public List<List<RealPoint3d>> TranslationFrames;
        public List<List<float>> ScaleFrames;
        public List<List<int>> RotationKeyframes;
        public List<List<int>> TranslationKeyframes;
        public List<List<int>> ScaleKeyframes;

        public abstract void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader);
        public abstract void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer);
    }
}