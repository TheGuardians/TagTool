using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Animations.Codecs
{
    public class QuantizedRotationOnlyData : AnimationCodecData
    {
        public override void Read(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianReader reader)
        {
            RotationKeyframes = new List<List<int>>();
            Rotations = new List<List<Common.RealQuaternion>>();

            for (var i = 0; i < header.RotationNodeCount; i++)
            {
                var rotations = new List<RealQuaternion>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                {
                    rotations.Add(
                        new RealQuaternion(
                            reader.ReadSingle(TagFieldCompression.Int16),
                            reader.ReadSingle(TagFieldCompression.Int16),
                            reader.ReadSingle(TagFieldCompression.Int16),
                            reader.ReadSingle(TagFieldCompression.Int16)));
                }

                RotationKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());
                Rotations.Add(rotations);
            }

            TranslationKeyframes = new List<List<int>>();
            Translations = new List<List<RealPoint3d>>();

            for (var i = 0; i < header.TranslationNodeCount; i++)
            {
                var translations = new List<RealPoint3d>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                {
                    translations.Add(
                        new RealPoint3d(
                            reader.ReadSingle() * 100.0f,
                            reader.ReadSingle() * 100.0f,
                            reader.ReadSingle() * 100.0f));
                }

                TranslationKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());
                Translations.Add(translations);
            }

            ScaleKeyframes = new List<List<int>>();
            Scales = new List<List<float>>();

            for (var i = 0; i < header.ScaleNodeCount; i++)
            {
                var scales = new List<float>();

                for (var j = 0; j < groupMember.FrameCount; j++)
                {
                    scales.Add(reader.ReadSingle());
                }

                ScaleKeyframes.Add(Enumerable.Range(0, groupMember.FrameCount).ToList());
                Scales.Add(scales);
            }
        }

        public override void Write(ModelAnimationTagResource.GroupMember groupMember, AnimationCodecHeader header, EndianWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
