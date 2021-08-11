using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using System.Numerics;

namespace TagTool.Animations
{
    public struct Node
    {
        public string Name;
        public short ParentNode;
        public short FirstChildNode;
        public short NextSiblingNode;
        public RealPoint3d Translation;
        public Quaternion Rotation;
        public float Scale;
        public bool Rotated;
        public bool Translated;
        public bool Scaled;
    }
}
