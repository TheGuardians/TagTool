using TagTool.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using TagTool.Common;
using TagTool.Cache;

namespace TagTool.Animations.Codecs
{
    public class curve_codec : codec_base
    {
        public uint PayloadDataOffset { get; set; }

        public uint TotalCompressedSize { get; set; }

        public curve_codec(int framecount)
          : base(framecount)
        {
        }

        public override void Read(EndianReader reader)
        {
            uint position = (uint)reader.BaseStream.Position;
            base.Read(reader);
            this.TranslationDataOffset = reader.ReadUInt32();
            this.ScaleDataOffset = reader.ReadUInt32();
            this.PayloadDataOffset = reader.ReadUInt32();
            this.TotalCompressedSize = reader.ReadUInt32();
            int num1 = (int)reader.ReadUInt32();
            this.Rotations = new Quaternion[(int)this.RotatedNodeCount][];
            this.Translations = new RealPoint3d[(int)this.TranslatedNodeCount][];
            this.Scales = new float[(int)this.ScaledNodeCount][];
            this.RotationKeyFrames = new List<List<int>>((int)this.RotatedNodeCount);
            this.TranslationKeyFrames = new List<List<int>>((int)this.TranslatedNodeCount);
            this.ScaleKeyFrames = new List<List<int>>((int)this.ScaledNodeCount);
            uint[] numArray1 = new uint[(int)this.RotatedNodeCount];
            for (int index = 0; index < (int)this.RotatedNodeCount; ++index)
                numArray1[index] = reader.ReadUInt32();
            for (int index1 = 0; index1 < (int)this.RotatedNodeCount; ++index1)
            {
                List<int> intList = new List<int>();
                reader.BaseStream.Position = (long)(position + this.PayloadDataOffset + numArray1[index1]);
                int num2 = (int)reader.ReadUInt16();
                int keyCount = (int)reader.ReadUInt16();
                int num3 = (int)reader.ReadByte();
                int num4 = (int)reader.ReadByte();
                int num5 = (int)reader.ReadInt16();
                if ((num3 & 1) == 0)
                    intList = this.ReadKeyFrameData(keyCount, reader);
                this.RotationKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
                this.Rotations[index1] = new Quaternion[this.FrameCount];
                Quaternion p1 = new Quaternion();
                Quaternion p2 = new Quaternion();
                byte num6 = 0;
                byte num7 = 0;
                byte num8 = 0;
                byte num9 = 0;
                int num10 = 0;
                int index2 = 0;
                int num11 = 0;
                for (int index3 = 0; index3 < this.FrameCount; ++index3)
                {
                    Quaternion q;
                    if ((num3 & 1) == 1)
                    {
                        q = this.DecompressQuat((float)reader.ReadInt16() / (float)short.MaxValue, (float)reader.ReadInt16() / (float)short.MaxValue, (float)reader.ReadInt16() / (float)short.MaxValue);
                    }
                    else
                    {
                        if (intList[index2] == index3 && index3 < this.FrameCount - 1)
                        {
                            float i1 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float j1 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float w1 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            num6 = reader.ReadByte();
                            num7 = reader.ReadByte();
                            num8 = reader.ReadByte();
                            num9 = reader.ReadByte();
                            float i2 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float j2 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float w2 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            p1 = this.DecompressQuat(i1, j1, w1);
                            p2 = this.DecompressQuat(i2, j2, w2);
                            num10 = intList[index2];
                            num11 = intList[index2 + 1];
                            ++index2;
                            reader.BaseStream.Position -= 6L;
                        }
                        Quaternion tangent1 = this.CalculateTangent(((int)num6 >> 4) - 7, ((int)num7 >> 4) - 7, ((int)num8 >> 4) - 7, ((int)num9 >> 4) - 7, p1, p2);
                        Quaternion tangent2 = this.CalculateTangent(((int)num6 & 15) - 7, ((int)num7 & 15) - 7, ((int)num8 & 15) - 7, ((int)num9 & 15) - 7, p1, p2);
                        q = this.CalculateCurvePosition((float)Math.Round((double)(index3 - num10) / (double)(num11 - num10), 2), tangent1, tangent2, p1, p2);
                    }
                    this.Rotations[index1][index3] = Quaternion.Normalize(q);
                }
            }
            reader.BaseStream.Position = (long)(position + this.PayloadDataOffset + this.TranslationDataOffset);
            uint[] numArray2 = new uint[(int)this.TranslatedNodeCount];
            for (int index = 0; index < (int)this.TranslatedNodeCount; ++index)
                numArray2[index] = reader.ReadUInt32();
            for (int index1 = 0; index1 < (int)this.TranslatedNodeCount; ++index1)
            {
                List<int> intList = new List<int>();
                reader.BaseStream.Position = (long)(position + this.PayloadDataOffset + numArray2[index1]);
                int num2 = (int)reader.ReadUInt16();
                int keyCount = (int)reader.ReadUInt16();
                int num3 = (int)reader.ReadByte();
                int num4 = (int)reader.ReadByte();
                int num5 = (int)reader.ReadUInt16();
                float num6 = reader.ReadSingle();
                float num7 = reader.ReadSingle();
                float num8 = reader.ReadSingle();
                float num9 = reader.ReadSingle();
                if ((num3 & 1) == 0)
                    intList = this.ReadKeyFrameData(keyCount, reader);
                this.TranslationKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
                this.Translations[index1] = new RealPoint3d[this.FrameCount];
                RealPoint3d p1 = new RealPoint3d();
                RealPoint3d p2 = new RealPoint3d();
                byte num10 = 0;
                byte num11 = 0;
                byte num12 = 0;
                int num13 = 0;
                int index2 = 0;
                int num14 = 0;
                for (int index3 = 0; index3 < this.FrameCount; ++index3)
                {
                    RealPoint3d RealPoint3d2;
                    if ((num3 & 1) == 1)
                    {
                        double num15 = (double)reader.ReadInt16() / (double)short.MaxValue;
                        float num16 = (float)reader.ReadInt16() / (float)short.MaxValue;
                        float num17 = (float)reader.ReadInt16() / (float)short.MaxValue;
                        double num18 = (double)num16;
                        double num19 = (double)num17;
                        RealPoint3d2 = new RealPoint3d((float)num15, (float)num18, (float)num19);
                    }
                    else
                    {
                        if (intList[index2] == index3 && index3 < this.FrameCount - 1)
                        {
                            float x = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float y = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float z = (float)reader.ReadInt16() / (float)short.MaxValue;
                            num10 = reader.ReadByte();
                            num11 = reader.ReadByte();
                            num12 = reader.ReadByte();
                            double num15 = (double)reader.ReadInt16() / (double)short.MaxValue;
                            float num16 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            float num17 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            p1 = new RealPoint3d(x, y, z);
                            double num18 = (double)num16;
                            double num19 = (double)num17;
                            p2 = new RealPoint3d((float)num15, (float)num18, (float)num19);
                            num13 = intList[index2];
                            num14 = intList[index2 + 1];
                            ++index2;
                            reader.BaseStream.Position -= 6L;
                        }
                        RealPoint3d tangent1 = this.CalculateTangent(((int)num10 >> 4) - 7, ((int)num11 >> 4) - 7, ((int)num12 >> 4) - 7, p1, p2);
                        RealPoint3d tangent2 = this.CalculateTangent(((int)num10 & 15) - 7, ((int)num11 & 15) - 7, ((int)num12 & 15) - 7, p1, p2);
                        RealPoint3d2 = this.CalculateCurvePosition((float)Math.Round((double)(index3 - num13) / (double)(num14 - num13), 2), tangent1, tangent2, p1, p2);
                    }
                    RealPoint3d2.X = (float)(((double)num9 * (double)RealPoint3d2.X + (double)num6) * 100.0);
                    RealPoint3d2.Y = (float)(((double)num9 * (double)RealPoint3d2.Y + (double)num7) * 100.0);
                    RealPoint3d2.Z = (float)(((double)num9 * (double)RealPoint3d2.Z + (double)num8) * 100.0);
                    this.Translations[index1][index3] = RealPoint3d2;
                }
            }
            reader.BaseStream.Position = (long)(position + this.PayloadDataOffset + this.ScaleDataOffset);
            uint[] numArray3 = new uint[(int)this.ScaledNodeCount];
            for (int index = 0; index < (int)this.ScaledNodeCount; ++index)
                numArray3[index] = reader.ReadUInt32();
            for (int index1 = 0; index1 < (int)this.ScaledNodeCount; ++index1)
            {
                List<int> intList = new List<int>();
                reader.BaseStream.Position = (long)(position + this.PayloadDataOffset + numArray3[index1]);
                int num2 = (int)reader.ReadUInt16();
                int keyCount = (int)reader.ReadUInt16();
                int num3 = (int)reader.ReadByte();
                int num4 = (int)reader.ReadByte();
                int num5 = (int)reader.ReadUInt16();
                float num6 = reader.ReadSingle();
                float num7 = reader.ReadSingle();
                if ((num3 & 1) == 0)
                    intList = this.ReadKeyFrameData(keyCount, reader);
                this.ScaleKeyFrames.Add(Enumerable.Range(0, this.FrameCount).ToList<int>());
                this.Scales[index1] = new float[this.FrameCount];
                float real1 = 0.0f;
                float real2 = 0.0f;
                byte num8 = 0;
                int num9 = 0;
                int index2 = 0;
                int num10 = 0;
                for (int index3 = 0; index3 < this.FrameCount; ++index3)
                {
                    float real3;
                    if ((num3 & 1) == 1)
                    {
                        real3 = (float)reader.ReadInt16() / (float)short.MaxValue;
                    }
                    else
                    {
                        if (intList[index2] == index3 && index3 < this.FrameCount - 1)
                        {
                            float num11 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            num8 = reader.ReadByte();
                            float num12 = (float)reader.ReadInt16() / (float)short.MaxValue;
                            real1 = num11;
                            real2 = (float)num12;
                            num9 = intList[index2];
                            num10 = intList[index2 + 1];
                            ++index2;
                            reader.BaseStream.Position -= 2L;
                        }
                        float tangent1 = this.CalculateTangent(((int)num8 >> 4) - 7, real1, real2);
                        float tangent2 = this.CalculateTangent(((int)num8 & 15) - 7, real1, real2);
                        float time = (float)Math.Round((double)(index3 - num9) / (double)(num10 - num9), 2);
                        real3 = this.CalculateCurvePosition(time, tangent1, tangent2, real1, real2);
                    }
                    real3 = real3 * num7 + num6;
                    this.Scales[index1][index3] = real3;
                }
            }
            reader.BaseStream.Position = (long)(position + this.TotalCompressedSize);
        }

        public override byte[] Write(GameCacheHaloOnlineBase CacheContext) => throw new NotImplementedException();

        protected List<int> ReadKeyFrameData(int keyCount, EndianReader reader)
        {
            List<int> intList = new List<int>(keyCount);
            intList.Add(0);
            int num = 0;
            for (int index = 0; index < keyCount; ++index)
            {
                num += (int)reader.ReadByte();
                intList.Add(num);
            }
            return intList;
        }

        protected Quaternion DecompressQuat(float i, float j, float w)
        {
            float num = (float)Math.Sqrt((double)Math.Max((float)(1.0 - (double)i * (double)i - (double)j * (double)j), 0.0f));
            if ((double)w < 0.0)
                num *= -1f;
            w = (float)((double)Math.Abs(w) + (double)Math.Abs(w) - 1.0);
            i *= (float)Math.Sqrt(1.0 - (double)w * (double)w);
            j *= (float)Math.Sqrt(1.0 - (double)w * (double)w);
            float k = num * (float)Math.Sqrt(1.0 - (double)w * (double)w);
            return new Quaternion(i, j, k, w);
        }

        protected float CalculateTangent(int tanComponent, float p1, float p2) => (float)((double)Math.Abs((float)tanComponent / 7f) * ((double)tanComponent / 7.0 * 0.300000011920929) + ((double)p2 - (double)p1));

        protected Quaternion CalculateTangent(
          int iTanComponent,
          int jTanComponent,
          int kTanComponent,
          int wTanComponent,
          Quaternion p1,
          Quaternion p2)
        {
            double tangent1 = (double)this.CalculateTangent(iTanComponent, p1.X, p2.X);
            float tangent2 = this.CalculateTangent(jTanComponent, p1.Y, p2.Y);
            float tangent3 = this.CalculateTangent(kTanComponent, p1.Z, p2.Z);
            float tangent4 = this.CalculateTangent(wTanComponent, p1.W, p2.W);
            double num1 = (double)tangent2;
            double num2 = (double)tangent3;
            double num3 = (double)tangent4;
            return new Quaternion((float)tangent1, (float)num1, (float)num2, (float)num3);
        }

        protected RealPoint3d CalculateTangent(
          int xTanComponent,
          int yTanComponent,
          int zTanComponent,
          RealPoint3d p1,
          RealPoint3d p2)
        {
            double tangent1 = (double)this.CalculateTangent(xTanComponent, p1.X, p2.X);
            float tangent2 = this.CalculateTangent(yTanComponent, p1.Y, p2.Y);
            float tangent3 = this.CalculateTangent(zTanComponent, p1.Z, p2.Z);
            double num1 = (double)tangent2;
            double num2 = (double)tangent3;
            return new RealPoint3d((float)tangent1, (float)num1, (float)num2);
        }

        protected float CalculateCurvePosition(
          float time,
          float tan1,
          float tan2,
          float p1,
          float p2)
        {
            double num1 = 2.0 * Math.Pow((double)time, 3.0) - 3.0 * Math.Pow((double)time, 2.0) + 1.0;
            float num2 = (float)Math.Pow((double)time, 3.0) - (float)(2.0 * Math.Pow((double)time, 2.0)) + time;
            float num3 = (float)(3.0 * Math.Pow((double)time, 2.0) - 2.0 * Math.Pow((double)time, 3.0));
            float num4 = (float)Math.Pow((double)time, 3.0) - (float)Math.Pow((double)time, 2.0);
            double num5 = (double)p1;
            return (float)(num1 * num5 + (double)num2 * (double)tan1 + (double)num3 * (double)p2 + (double)num4 * (double)tan2);
        }

        protected Quaternion CalculateCurvePosition(
          float time,
          Quaternion tan1,
          Quaternion tan2,
          Quaternion p1,
          Quaternion p2)
        {
            double curvePosition1 = (double)this.CalculateCurvePosition(time, tan1.X, tan2.X, p1.X, p2.X);
            float curvePosition2 = this.CalculateCurvePosition(time, tan1.Y, tan2.Y, p1.Y, p2.Y);
            float curvePosition3 = this.CalculateCurvePosition(time, tan1.Z, tan2.Z, p1.Z, p2.Z);
            float curvePosition4 = this.CalculateCurvePosition(time, tan1.W, tan2.W, p1.W, p2.W);
            double num1 = (double)curvePosition2;
            double num2 = (double)curvePosition3;
            double num3 = (double)curvePosition4;
            return new Quaternion((float)curvePosition1, (float)num1, (float)num2, (float)num3);
        }

        protected RealPoint3d CalculateCurvePosition(
          float time,
          RealPoint3d tan1,
          RealPoint3d tan2,
          RealPoint3d p1,
          RealPoint3d p2)
        {
            double curvePosition1 = (double)this.CalculateCurvePosition(time, tan1.X, tan2.X, p1.X, p2.X);
            float curvePosition2 = this.CalculateCurvePosition(time, tan1.Y, tan2.Y, p1.Y, p2.Y);
            float curvePosition3 = this.CalculateCurvePosition(time, tan1.Z, tan2.Z, p1.Z, p2.Z);
            double num1 = (double)curvePosition2;
            double num2 = (double)curvePosition3;
            return new RealPoint3d((float)curvePosition1, (float)num1, (float)num2);
        }
    }
}
