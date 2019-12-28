using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0x30)]
    public class StructureBspCacheFileTagResourcesTest : TagStructure
	{
        public List<ScenarioStructureBsp.SurfacesPlanes> SurfacePlanes;
        public List<ScenarioStructureBsp.Plane> Planes;
        public List<ScenarioStructureBsp.EdgeToSeamMapping> EdgeToSeams;
        public List<PathfindingDatum> PathfindingData;

        [TagStructure(Size = 0x94)]
        public class PathfindingDatum : TagStructure
		{
            public List<ScenarioStructureBsp.PathfindingDatum.Sector> Sectors;
            public List<ScenarioStructureBsp.PathfindingDatum.Link> Links;
            public List<ScenarioStructureBsp.PathfindingDatum.Reference> References;
            public List<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode> Bsp2dNodes;
            public List<ScenarioStructureBsp.PathfindingDatum.Vertex> Vertices;
            public List<ObjectReference> ObjectReferences;
            public List<ScenarioStructureBsp.PathfindingDatum.PathfindingHint> PathfindingHints;
            public List<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference> InstancedGeometryReferences;
            public int StructureChecksum;
            public List<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock> GiantPathfinding;
            public List<Seam> Seams;
            public List<JumpSeam> JumpSeams;
            public List<ScenarioStructureBsp.PathfindingDatum.Door> Doors;

            [TagStructure(Size = 0x18)]
            public class ObjectReference : TagStructure
            {
                public ushort Flags;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused = new byte[2];

                public List<BspReference> Bsps;

                public int ObjectUniqueID;
                public short OriginBspIndex;
                public ScenarioObjectType ObjectType;
                public Scenario.ScenarioInstance.SourceValue Source;

                [TagStructure(Size = 0x18)]
                public class BspReference : TagStructure
                {
                    public int BspIndex;
                    public short NodeIndex;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];

                    public List<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef> Bsp2dRefs;

                    public int VertexOffset;
                }
            }

            [TagStructure(Size = 0xC)]
            public class Seam : TagStructure
			{
                public List<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock> LinkIndices;
            }

            [TagStructure(Size = 0x14)]
            public class JumpSeam : TagStructure
			{
                public short UserJumpIndex;
                public byte DestOnly;

                [TagField(Flags = Padding, Length = 1)]
                public byte[] Unused = new byte[1];

                public float Length;

                public List<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock> JumpIndices;
            }
        }
    }
}