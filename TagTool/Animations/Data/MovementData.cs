using TagTool.IO;
using TagTool.Tags;
using TagTool.Common;
using System.Numerics;
using System;

namespace TagTool.Animations.Data
{
    public class MovementData
    {
        public FrameInfoType Type { get; set; }

        public int FrameCount { get; set; }

        public RealPoint3d[] Translations { get; set; }

        public RealEulerAngles3d[] Rotations { get; set; }

        public MovementData()
        {
        }

        public MovementData(FrameInfoType type, int frameCount)
        {
            this.Type = type;
            this.FrameCount = frameCount;
        }

        public void Read(EndianReader reader)
        {
            this.Translations = new RealPoint3d[this.FrameCount];
            this.Rotations = new RealEulerAngles3d[this.FrameCount];
            for (int index = 0; index < this.FrameCount; ++index)
            {
                float num1 = 0.0f;
                float y = 0.0f;
                float num2 = reader.ReadSingle();
                float num3 = reader.ReadSingle();
                if (this.Type == FrameInfoType.DxDyDyaw)
                    y = reader.ReadSingle();
                else if (this.Type == FrameInfoType.DxDyDzDyaw)
                {
                    num1 = reader.ReadSingle();
                    y = reader.ReadSingle();
                }
                this.Translations[index] = new RealPoint3d(num2 * 100f, num3 * 100f, num1 * 100f);
                this.Rotations[index] = new RealEulerAngles3d(Angle.FromDegrees(y), Angle.FromDegrees(0.0f), Angle.FromDegrees(0.0f));
            }
        }

        public void Write(EndianWriter writer) => throw new NotImplementedException();
    }
}
