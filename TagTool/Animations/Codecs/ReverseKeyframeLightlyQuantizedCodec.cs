using TagTool.IO;
using System;
using TagTool.Cache;

namespace TagTool.Animations.Codecs
{
    public class ReverseKeyframeLightlyQuantizedCodec : KeyframeLightlyQuantizedCodec
    {
        public ReverseKeyframeLightlyQuantizedCodec(int framecount, int keysize)
          : base(framecount, keysize)
        {
        }

        public override void Read(EndianReader reader)
        {
            base.Read(reader);
            this.RotationKeyFrames.Reverse();
            this.TranslationKeyFrames.Reverse();
            this.ScaleKeyFrames.Reverse();
            Array.Reverse((Array)this.Rotations);
            Array.Reverse((Array)this.Translations);
            Array.Reverse((Array)this.Scales);
        }

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();
    }
}
