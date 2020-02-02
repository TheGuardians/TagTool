using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x44, MinVersion = CacheVersion.Halo3ODST)]
    public class StructureDesign : TagStructure
	{
        public int Version;

        public List<TagHkpMoppCode> SoftCeilingMoppCodes;
        public List<SoftCeiling> SoftCeilings;
        public List<TagHkpMoppCode> WaterMoppCodes;
        public List<WaterGroup> WaterGroups;
        public List<WaterInstance> WaterInstances;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public uint Unknown2;
        
        public enum SoftCeilingType : short
        {
            Acceleration,
            SoftKill,
            SlipSurface
        }

        [TagStructure(Size = 0x44)]
        public class SoftCeilingTriangle : TagStructure
		{
            public RealPlane3d Plane;
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public RealPoint3d Point1;
            public RealPoint3d Point2;
            public RealPoint3d Point3;
        }

        [TagStructure(Size = 0x14)]
        public class SoftCeiling : TagStructure
		{
            public StringId Name;
            public SoftCeilingType Type;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public List<SoftCeilingTriangle> SoftCeilingTriangles;
        }

        [TagStructure(Size = 0x4)]
        public class WaterGroup : TagStructure
		{
            public StringId Name;
        }

        [TagStructure(Size = 0x10)]
        public class WaterPlane : TagStructure
		{
            public RealPlane3d Plane;
        }

        [TagStructure(Size = 0x24)]
        public class WaterDebugTriangle : TagStructure
		{
            public RealPoint3d Point1;
            public RealPoint3d Point2;
            public RealPoint3d Point3;
        }

        [TagStructure(Size = 0x2C)]
        public class WaterInstance : TagStructure
		{
            public short WaterNameIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public RealVector3d FlowVelocity;
            public float FlowForce;

            public List<WaterPlane> WaterPlanes;
            public List<WaterDebugTriangle> WaterDebugTriangles;
        }
    }
}