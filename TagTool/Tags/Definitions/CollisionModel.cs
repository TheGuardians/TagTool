using System;
using System.Collections.Generic;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "collision_model", Tag = "coll", Size = 0x44, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "collision_model", Tag = "coll", Size = 0x50, MinVersion = CacheVersion.HaloReach)]
    public class CollisionModel : TagStructure
	{
        public int CollisionModelChecksum;

        [TagField(Flags = Padding, Length = 0xC)]
        public byte[] UnusedErrorsBlock = new byte[0xC];

        public CollisionModelFlags Flags;

        public List<Material> Materials;
        public List<Region> Regions;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Region> CookieCutters;

        public List<PathfindingSphere> PathfindingSpheres;
        public List<Node> Nodes;


        [TagStructure(Size = 0x4)]
        public class Material : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
        }

        [TagStructure(Size = 0x10, MinVersion = CacheVersion.Halo3Retail)]
        public class Region : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;

            public List<Permutation> Permutations;

            [TagStructure(Size = 0x28)]
            public class Permutation : TagStructure
			{
                [TagField(Flags = Label)]
                public StringId Name;

                public List<Bsp> Bsps;
                public List<CollisionBspPhysicsDefinition> BspPhysics;
                public List<TagHkpMoppCode> BspMoppCodes;

                [TagStructure(Size = 0x64, MaxVersion = CacheVersion.HaloOnline700123)]
                [TagStructure(Size = 0x70, MinVersion = CacheVersion.HaloReach)]
                public class Bsp : TagStructure
				{
                    public short NodeIndex;

                    [TagField(Flags = Padding, Length = 2)]
                    public byte[] Unused = new byte[2];

                    public CollisionGeometry Geometry;
                }
            }
        }

        [Flags]
        public enum PathfindingSphereFlags : ushort
        {
            None = 0,
            RemainsWhenOpen = 1 << 0,
            VehicleOnly = 1 << 1,
            WithSectors = 1 << 2
        }

        [TagStructure(Size = 0x14)]
        public class PathfindingSphere : TagStructure
		{
            public short Node;
            public PathfindingSphereFlags Flags;
            public RealPoint3d Center;
            public float Radius;
        }

        [Flags]
        public enum NodeFlags : ushort
        {
            None = 0,
            GenerateNavMesh = 1 << 0
        }

        [TagStructure(Size = 0xC)]
        public class Node : TagStructure
		{
            [TagField(Flags = Label)]
            public StringId Name;
            public NodeFlags Flags;
            public short ParentNode;
            public short NextSiblingNode;
            public short FirstChildNode;
        }
    }

    [Flags]
    public enum CollisionModelFlags : int
    {
        None = 0,
        ContainsOpenEdges = 1 << 0,
        PhysicsBuilt = 1 << 1,
        PhysicsInUse = 1 << 2,
        Processed = 1 << 3,
        HasTwoSidedSurfaces = 1 << 4
    }
}
