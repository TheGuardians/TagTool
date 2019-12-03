using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.IO;

namespace TagTool.Animations
{
    public class AnimationMovementData
    {
        public AnimationMovementDataType Type { get; set; }
        public int FrameCount { get; set; }
        public RealPoint3d[] Translations { get; set; }
        public RealEulerAngles3d[] Rotations { get; set; }

		public AnimationMovementData()
		{
		}

		public AnimationMovementData(AnimationMovementDataType type, int frameCount)
		{
			Type = type;
			FrameCount = frameCount;
		}

        public void Read(EndianReader reader)
        {
            Translations = new RealPoint3d[FrameCount];
            Rotations = new RealEulerAngles3d[FrameCount];

            for (int i = 0; i < FrameCount; i++)
            {
                var yaw = 0f;
                var x = reader.ReadSingle();
                var y = reader.ReadSingle();
                var z = 0f;

                switch (Type)
                {
                    case AnimationMovementDataType.DxDyDyaw:
                        yaw = reader.ReadSingle();
                        break;

                    case AnimationMovementDataType.DxDyDzDyaw:
                        z = reader.ReadSingle();
                        yaw = reader.ReadSingle();
                        break;
                }

                Translations[i] = new RealPoint3d(x * 100f, y * 100f, z * 100f);
                Rotations[i] = new RealEulerAngles3d(Angle.FromRadians(yaw), Angle.FromRadians(0f), Angle.FromRadians(0f));
            }
        }

        public void Write(EndianWriter writer)
        {
            for (int i = 0; i < FrameCount; i++)
            {
                writer.Write(Translations[i].X / 100f);
                writer.Write(Translations[i].Y / 100f);

                switch (Type)
                {
                    case AnimationMovementDataType.DxDyDyaw:
                        writer.Write(Rotations[i].Yaw.Radians);
                        break;

                    case AnimationMovementDataType.DxDyDzDyaw:
                        writer.Write(Translations[i].Z / 100f);
                        writer.Write(Rotations[i].Yaw.Radians);
                        break;
                }
            }
        }
    }
}