using System.Collections.Generic;
using TagTool.Common;
using TagTool.Tags.Definitions;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0x30)]
    public class StructureBspCacheFileTagResources : TagStructure
	{
        public TagBlock<ScenarioStructureBsp.SurfacesPlanes> SurfacePlanes;
        public TagBlock<ScenarioStructureBsp.Plane> Planes;
        public TagBlock<ScenarioStructureBsp.UnknownRaw7th> UnknownRaw7ths;
        public List<PathfindingDatum> PathfindingData;

        [TagStructure(Size = 0x94)]
        public class PathfindingDatum : TagStructure
		{
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Sector> Sectors;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Link> Links;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Reference> References;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Bsp2dNode> Bsp2dNodes;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Vertex> Vertices;
            public List<ObjectReference> ObjectReferences;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.PathfindingHint> PathfindingHints;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.InstancedGeometryReference> InstancedGeometryReferences;
            public int StructureChecksum;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.GiantPathfindingBlock> GiantPathfinding;
            public List<Seam> Seams;
            public List<JumpSeam> JumpSeams;
            public TagBlock<ScenarioStructureBsp.PathfindingDatum.Door> Doors;

            [TagStructure(Size = 0x18)]
            public class ObjectReference : TagStructure
            {
                public ushort Flags;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unused = new byte[2];

                public List<BspReference> Bsps;

                public DatumIndex ObjectHandle;
                public short OriginBspIndex;
                public ScenarioObjectType ObjectType;
                public Scenario.ScenarioInstance.SourceValue Source;

                [TagStructure(Size = 0x18)]
                public class BspReference : TagStructure
                {
                    public DatumIndex BspHandle;
                    public short NodeIndex;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];

                    public TagBlock<ScenarioStructureBsp.PathfindingDatum.ObjectReference.BspReference.Bsp2dRef> Bsp2dRefs;

                    public int VertexOffset;
                }
            }

            [TagStructure(Size = 0xC)]
            public class Seam : TagStructure
			{
                public TagBlock<ScenarioStructureBsp.PathfindingDatum.Seam.LinkIndexBlock> LinkIndices;
            }

            [TagStructure(Size = 0x14)]
            public class JumpSeam : TagStructure
			{
                public short UserJumpIndex;
                public byte DestOnly;

                [TagField(Flags = Padding, Length = 1)]
                public byte[] Unused = new byte[1];

                public float Length;

                public TagBlock<ScenarioStructureBsp.PathfindingDatum.JumpSeam.JumpIndexBlock> JumpIndices;
            }
        }
    }
}