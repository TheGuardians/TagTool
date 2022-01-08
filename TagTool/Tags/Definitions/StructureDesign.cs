using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Definitions.Common;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x40, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x44, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "structure_design", Tag = "sddt", Size = 0x154, MinVersion = CacheVersion.HaloReach)]
    public class StructureDesign : TagStructure
	{
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StructureManifestBuildIdentifier BuildIdentifier;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StructureManifestBuildIdentifier ParentBuildIdentifier;

        public int Version;

        public List<TagHkpMoppCode> SoftCeilingMoppCodes;
        public List<SoftCeiling> SoftCeilings;
        public List<TagHkpMoppCode> WaterMoppCodes;
        public List<WaterGroup> WaterGroups;
        public List<WaterInstance> WaterInstances;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public PlanarFogSetStruct PlanarFogSet;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RenderGeometry RainRenderGeometry;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<InstancedGeometryBlock> InstancedGeometriesDefinitions;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<InstancedGeometryInstanceBlock> InstancedGeometryInstances;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<RenderMaterial> Materials;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagHkpMoppCode> RainBlockerMoppCodeBlock;

        [TagField(MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
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

        [TagStructure(Size = 0x2C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
        public class WaterInstance : TagStructure
		{
            public short WaterNameIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused = new byte[2];

            public RealVector3d FlowVelocity;
            public float FlowForce;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealRgbColor FogColor;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float FogMurkiness;

            public List<WaterPlane> WaterPlanes;
            public List<WaterDebugTriangle> WaterDebugTriangles;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealRectangle3d Bounds;
        }

        [TagStructure(Size = 0x4)]
        public class InstancedGeometryInstanceBlock : InstancedGeometryInstance
        {
            public StringId InstanceName;
        }

        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
        public class PlanarFogSetStruct : TagStructure
        {
            public List<PlanarFogDefinitionBlock> PlanarFogs;
            public List<TagHkpMoppCode> MoppCode;

            [TagStructure(Size = 0x3C)]
            public class PlanarFogDefinitionBlock : TagStructure
            {
                public StringId Name;
                [TagField(ValidTags = new[] { "pfpt" })]
                public CachedTag AppearanceSettings;
                public List<PlanarFogVertexBlock> Vertices;
                public List<PlanarFogTriangleBlock> Triangles;
                public float Depth;
                public RealVector3d Normal;

                [TagStructure(Size = 0xC)]
                public class PlanarFogVertexBlock : TagStructure
                {
                    public RealPoint3d Position;
                }

                [TagStructure(Size = 0xC)]
                public class PlanarFogTriangleBlock : TagStructure
                {
                    public List<PlanarFogTrianglePlanesBlock> Planes;

                    [TagStructure(Size = 0x10)]
                    public class PlanarFogTrianglePlanesBlock : TagStructure
                    {
                        public RealPlane3d Plane;
                    }
                }
            }
        }
    }
}