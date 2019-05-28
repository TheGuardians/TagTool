using TagTool.IO;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public abstract class AnimationData
    {
        public abstract void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader);
        public abstract void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer);
    }
}