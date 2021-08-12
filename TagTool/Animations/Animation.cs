using TagTool.Common;
using System.Numerics;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Animations.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TagTool.Animations
{
    public class Animation
    {
        public int NodeListChecksum { get; set; }

        public Node[] Nodes { get; set; }

        public Frame[] Frames { get; set; }

        public Animation(
          List<Node> defaultNodes,
          AnimationResourceData animationData)
        {
            this.NodeListChecksum = animationData.NodeListChecksum;
            this.Nodes = new Node[defaultNodes.Count];
            this.Frames = new Frame[animationData.FrameCount];
            for (int index = 0; index < defaultNodes.Count; ++index)
            {
                this.Nodes[index] = new Node();
                this.Nodes[index].Name = defaultNodes[index].Name;
                this.Nodes[index].ParentNode = defaultNodes[index].ParentNode;
                this.Nodes[index].FirstChildNode = defaultNodes[index].FirstChildNode;
                this.Nodes[index].NextSiblingNode = defaultNodes[index].NextSiblingNode;
                this.Nodes[index].Translation = defaultNodes[index].Translation;
                this.Nodes[index].Rotation = defaultNodes[index].Rotation;
                this.Nodes[index].Scale = defaultNodes[index].Scale;
                this.Nodes[index].Rotated = animationData.AnimatedRotatedNodeFlags[index];
                this.Nodes[index].Translated = animationData.AnimatedTranslatedNodeFlags[index];
                this.Nodes[index].Scaled = animationData.AnimatedScaledNodeFlags[index];
            }
            this.Process(animationData);
        }

        public void Process(AnimationResourceData animationData)
        {
            List<List<Quaternion>> QuaternionListList = this.Process<Quaternion>(animationData.StaticRotatedNodeFlags, animationData.AnimatedRotatedNodeFlags, animationData.Static_Data?.Rotations, animationData.Animation_Data.Rotations, animationData.Animation_Data.RotationKeyFrames);
            List<List<RealPoint3d>> RealPoint3dListList = this.Process<RealPoint3d>(animationData.StaticTranslatedNodeFlags, animationData.AnimatedTranslatedNodeFlags, animationData.Static_Data?.Translations, animationData.Animation_Data.Translations, animationData.Animation_Data.TranslationKeyFrames);
            List<List<float>> realListList = this.Process<float>(animationData.StaticScaledNodeFlags, animationData.AnimatedScaledNodeFlags, animationData.Static_Data?.Scales, animationData.Animation_Data.Scales, animationData.Animation_Data.ScaleKeyFrames);
            if (animationData.FrameInfoType > FrameInfoType.None)
            {
                RealPoint3d RealPoint3d = new RealPoint3d();
                RealEulerAngles3d angles = new RealEulerAngles3d();
                for (int index = 0; index < animationData.FrameCount; ++index)
                {
                    RealPoint3d += animationData.Movement_Data.Translations[index];
                    angles = new RealEulerAngles3d 
                    { 
                        YawValue = angles.YawValue + animationData.Movement_Data.Rotations[index].YawValue,
                        PitchValue = angles.PitchValue + animationData.Movement_Data.Rotations[index].PitchValue,
                        RollValue = angles.RollValue + animationData.Movement_Data.Rotations[index].RollValue,
                    };
                    RealPoint3dListList[0][index] += RealPoint3d;
                    QuaternionListList[0][index] = Quaternion.Conjugate(Quaternion.Conjugate(QuaternionListList[0][index]) * Quaternion.Conjugate(Quaternion.CreateFromYawPitchRoll(angles.Yaw.Degrees, angles.Pitch.Degrees, angles.Roll.Degrees)));
                }
            }
            for (int index1 = 0; index1 < this.Frames.Length; ++index1)
            {
                this.Frames[index1] = new Frame();
                this.Frames[index1].Nodes = new Node[this.Nodes.Length];
                for (int index2 = 0; index2 < this.Nodes.Length; ++index2)
                {
                    this.Frames[index1].Nodes[index2].Rotation = QuaternionListList[index2][index1];
                    this.Frames[index1].Nodes[index2].Translation = RealPoint3dListList[index2][index1];
                    this.Frames[index1].Nodes[index2].Scale = realListList[index2][index1];
                }
            }
        }

        private List<List<T>> Process<T>(
          BitArray staticFlags,
          BitArray animatedFlags,
          T[][] staticData,
          T[][] animatedData,
          List<List<int>> keyframes)
        {
            List<List<T>> objListList = new List<List<T>>(this.Nodes.Length);
            int index1 = 0;
            int index2 = 0;
            for (int index3 = 0; index3 < this.Nodes.Length; ++index3)
            {
                objListList.Add(new List<T>(this.Frames.Length));
                for (int f = 0; f < this.Frames.Length; f++)
                {
                    object field2;
                    if (staticFlags != null && staticFlags[index3])
                        field2 = staticData[index1][0];
                    else if (animatedFlags != null && animatedFlags[index3])
                    {
                        if (keyframes[index2].Contains(f))
                        {
                            int index4 = keyframes[index2].IndexOf(f);
                            field2 = animatedData[index2][index4];
                        }
                        else
                        {
                            int num1 = keyframes[index2].Last(q => q < f);
                            int num2 = keyframes[index2].First(q => q > f);
                            int index4 = keyframes[index2].IndexOf(num1);
                            int index5 = keyframes[index2].IndexOf(num2);
                            float time = (f - num1) / (num2 - num1);
                            field2 = this.Interpolate<T>(animatedData[index2][index4], animatedData[index2][index5], time);
                        }
                    }
                    else
                        field2 = this.GetNodeDefaultValue<T>(index3);
                    objListList[index3].Add((T)field2);
                }
                if (staticFlags != null && staticFlags[index3])
                    ++index1;
                if (animatedFlags != null && animatedFlags[index3])
                    ++index2;
            }
            return objListList;
        }

        private object Interpolate<T>(object start, object end, float time)
        {
            if (typeof(T) == typeof(Quaternion))
                return Quaternion.Normalize(Quaternion.Slerp((Quaternion)start, (Quaternion)end, time));
            if (typeof(T) == typeof(RealPoint3d))
            {
                RealPoint3d pa = (RealPoint3d)start;
                RealPoint3d pb = (RealPoint3d)end;
                return new RealPoint3d(pa.X + (pb.X - pa.X) * time, pa.Y + (pb.Y - pa.Y) * time, pa.Z + (pb.Z - pa.Z) * time);
            }
            if (typeof(T) == typeof(float))
            {
                float pa = (float)start;
                float pb = (float)end;
                return pa + (pb - pa) * time;
            }
            throw new NotSupportedException(string.Format("Interpolation operation not supported for type {0}.", typeof(T)));
        }

        private object GetNodeDefaultValue<T>(int nodeIndex)
        {
            if (typeof(T) == typeof(Quaternion))
                return this.Nodes == null || this.Nodes.Length <= nodeIndex ? Quaternion.Identity : this.Nodes[nodeIndex].Rotation;
            if (typeof(T) == typeof(RealPoint3d))
                return this.Nodes == null || this.Nodes.Length <= nodeIndex ? new RealPoint3d(0.0f, 0.0f, 0.0f) : new RealPoint3d(this.Nodes[nodeIndex].Translation.X * 100f, this.Nodes[nodeIndex].Translation.Y * 100f, this.Nodes[nodeIndex].Translation.Z * 100f);
            if (typeof(T) == typeof(float))
                return 1.0f;
            throw new NotSupportedException(string.Format("Default Node value not supported for type {0}.", typeof(T)));
        }

        public void InsertBaseFrame(Animation baseAnimation)
        {
            Frame[] frameArray = new Frame[this.Frames.Length + 1];
            frameArray[0] = baseAnimation.Frames[0];
            Array.Copy((Array)this.Frames, 0, (Array)frameArray, 1, this.Frames.Length);
            this.Frames = frameArray;
        }

        public void InsertBaseFrame()
        {
            Frame[] frameArray = new Frame[this.Frames.Length + 1];
            frameArray[0] = new Frame();
            frameArray[0].Nodes = new Node[this.Nodes.Length];
            for (int nodeIndex = 0; nodeIndex < this.Nodes.Length; ++nodeIndex)
            {
                frameArray[0].Nodes[nodeIndex].Rotation = (Quaternion)this.GetNodeDefaultValue<Quaternion>(nodeIndex);
                frameArray[0].Nodes[nodeIndex].Translation = (RealPoint3d)this.GetNodeDefaultValue<RealPoint3d>(nodeIndex);
                frameArray[0].Nodes[nodeIndex].Scale = (float)this.GetNodeDefaultValue<float>(nodeIndex);
            }
            Array.Copy((Array)this.Frames, 0, (Array)frameArray, 1, this.Frames.Length);
            this.Frames = frameArray;
        }

        public void Overlay(Animation baseAnimation)
        {
            for (int index1 = 0; index1 < this.Nodes.Length; ++index1)
            {
                for (int index2 = 0; index2 < this.Frames.Length; ++index2)
                {
                    this.Frames[index2].Nodes[index1].Rotation = !this.Nodes[index1].Rotated ? baseAnimation.Frames[0].Nodes[index1].Rotation : baseAnimation.Frames[0].Nodes[index1].Rotation * this.Frames[index2].Nodes[index1].Rotation;
                    this.Frames[index2].Nodes[index1].Translation = !this.Nodes[index1].Translated ? baseAnimation.Frames[0].Nodes[index1].Translation : baseAnimation.Frames[0].Nodes[index1].Translation + this.Frames[index2].Nodes[index1].Translation;
                    this.Frames[index2].Nodes[index1].Scale = !this.Nodes[index1].Scaled ? baseAnimation.Frames[0].Nodes[index1].Scale : baseAnimation.Frames[0].Nodes[index1].Scale + this.Frames[index2].Nodes[index1].Scale;
                }
            }
        }

        public void Overlay()
        {
            for (int nodeIndex = 0; nodeIndex < this.Nodes.Length; ++nodeIndex)
            {
                for (int index = 0; index < this.Frames.Length; ++index)
                {
                    this.Frames[index].Nodes[nodeIndex].Rotation = !this.Nodes[nodeIndex].Rotated ? (Quaternion)this.GetNodeDefaultValue<Quaternion>(nodeIndex) : (Quaternion)this.GetNodeDefaultValue<Quaternion>(nodeIndex) * this.Frames[index].Nodes[nodeIndex].Rotation;
                    this.Frames[index].Nodes[nodeIndex].Translation = !this.Nodes[nodeIndex].Translated ? (RealPoint3d)this.GetNodeDefaultValue<RealPoint3d>(nodeIndex) : (RealPoint3d)this.GetNodeDefaultValue<RealPoint3d>(nodeIndex) + this.Frames[index].Nodes[nodeIndex].Translation;
                    this.Frames[index].Nodes[nodeIndex].Scale = !this.Nodes[nodeIndex].Scaled ? (float)this.GetNodeDefaultValue<float>(nodeIndex) : (float)this.GetNodeDefaultValue<float>(nodeIndex) + this.Frames[index].Nodes[nodeIndex].Scale;
                }
            }
        }

        public void Replace(Animation baseAnimation)
        {
            for (int index1 = 0; index1 < this.Nodes.Length; ++index1)
            {
                for (int index2 = 0; index2 < this.Frames.Length; ++index2)
                {
                    if (!this.Nodes[index1].Rotated)
                        this.Frames[index2].Nodes[index1].Rotation = baseAnimation.Frames[0].Nodes[index1].Rotation;
                    if (!this.Nodes[index1].Translated)
                        this.Frames[index2].Nodes[index1].Translation = baseAnimation.Frames[0].Nodes[index1].Translation;
                    if (!this.Nodes[index1].Scaled)
                        this.Frames[index2].Nodes[index1].Scale = baseAnimation.Frames[0].Nodes[index1].Scale;
                }
            }
        }

        public void Export(string fileName)
        {
            using (TextWriter text = (TextWriter)File.CreateText(fileName))
            {
                text.WriteLine(16392);
                text.WriteLine(this.Frames.Length);
                text.WriteLine(30);
                text.WriteLine(1);
                text.WriteLine("unnamedActor");
                text.WriteLine(this.Nodes.Length);
                text.WriteLine(this.NodeListChecksum);
                foreach (Node node in this.Nodes)
                {
                    text.WriteLine(node.Name);
                    text.WriteLine((int)node.FirstChildNode);
                    text.WriteLine((int)node.NextSiblingNode);
                }
                for (int index1 = 0; index1 < this.Frames.Length; ++index1)
                {
                    for (int index2 = 0; index2 < this.Nodes.Length; ++index2)
                    {
                        Quaternion Quaternion = Quaternion.Conjugate(this.Frames[index1].Nodes[index2].Rotation);
                        RealPoint3d translation = this.Frames[index1].Nodes[index2].Translation;
                        float scale = this.Frames[index1].Nodes[index2].Scale;
                        text.WriteLine(string.Format("{0}\t{1}\t{2}", translation.X, translation.Y, translation.Z));
                        text.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", Quaternion.X, Quaternion.Y, Quaternion.Z, Quaternion.W));
                        text.WriteLine(scale);
                    }
                }
                text.Flush();
            }
        }

        public void Import(string fileName)
        {
            using (TextReader textReader = (TextReader)File.OpenText(fileName))
            {
                textReader.ReadLine();
                int length1 = textReader.Read();
                textReader.ReadLine();
                textReader.ReadLine();
                textReader.ReadLine();
                int length2 = textReader.Read();
                int num1 = textReader.Read();
                this.Frames = new Frame[length1];
                this.Nodes = new Node[length2];
                this.NodeListChecksum = num1;
                for (int index = 0; index < length2; ++index)
                {
                    this.Nodes[index] = new Node();
                    this.Nodes[index].Name = textReader.ReadLine();
                    this.Nodes[index].FirstChildNode = (short)textReader.Read();
                    this.Nodes[index].NextSiblingNode = (short)textReader.Read();
                }
                for (int index = 0; index < length1; ++index)
                {
                    this.Frames[index] = new Frame();
                    this.Frames[index].Nodes = new Node[length2];
                    int num2 = 0;
                    while (num2 < length2)
                        ++num2;
                }
            }
        }

        public string GetAnimationExtension(int type, int frameInfoType, bool worldRelative)
        {
            switch ((ModelAnimationGraph.FrameType)type)
            {
                case ModelAnimationGraph.FrameType.Base:
                    switch ((FrameInfoType)frameInfoType)
                    {
                        case FrameInfoType.None:
                            return worldRelative ? "JMW" : "JMM";
                        case FrameInfoType.DxDy:
                            return "JMA";
                        case FrameInfoType.DxDyDyaw:
                            return "JMT";
                        case FrameInfoType.DxDyDzDyaw:
                            return "JMZ";
                    }
                    break;
                case ModelAnimationGraph.FrameType.Overlay:
                    return "JMO";
                case ModelAnimationGraph.FrameType.Replacement:
                    return "JMR";
            }
            throw new NotSupportedException("Animation type not supported");
        }
    }
}
