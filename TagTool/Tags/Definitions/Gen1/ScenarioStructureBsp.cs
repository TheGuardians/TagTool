using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x288)]
    public class ScenarioStructureBsp : TagStructure
    {
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Lightmaps;
        /// <summary>
        /// height below which vehicles get pushed up by an unstoppable force
        /// </summary>
        public float VehicleFloor; // world units
        /// <summary>
        /// height above which vehicles get pushed down by an unstoppable force
        /// </summary>
        public float VehicleCeiling; // world units
        [TagField(Length = 0x14)]
        public byte[] Padding;
        public RealRgbColor DefaultAmbientColor;
        [TagField(Length = 0x4)]
        public byte[] Padding1;
        public RealRgbColor DefaultDistantLight0Color;
        public RealVector3d DefaultDistantLight0Direction;
        public RealRgbColor DefaultDistantLight1Color;
        public RealVector3d DefaultDistantLight1Direction;
        [TagField(Length = 0xC)]
        public byte[] Padding2;
        public RealArgbColor DefaultReflectionTint;
        public RealVector3d DefaultShadowVector;
        public RealRgbColor DefaultShadowColor;
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        public List<StructureCollisionMaterialsBlock> CollisionMaterials;
        public List<Bsp> CollisionBsp;
        public List<StructureBspNodeBlock> Nodes;
        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;
        public List<StructureBspLeafBlock> Leaves;
        public List<StructureBspSurfaceReferenceBlock> LeafSurfaces;
        public List<StructureBspSurfaceBlock> Surfaces;
        public List<StructureBspLightmapBlock> Lightmaps1;
        [TagField(Length = 0xC)]
        public byte[] Padding4;
        public List<StructureBspLensFlareBlock> LensFlares;
        public List<StructureBspLensFlareMarkerBlock> LensFlareMarkers;
        public List<StructureBspClusterBlock> Clusters;
        public byte[] ClusterData;
        public List<StructureBspClusterPortalBlock> ClusterPortals;
        [TagField(Length = 0xC)]
        public byte[] Padding5;
        public List<StructureBspBreakableSurfaceBlock> BreakableSurfaces;
        public List<StructureBspFogPlaneBlock> FogPlanes;
        public List<StructureBspFogRegionBlock> FogRegions;
        public List<StructureBspFogPaletteBlock> FogPalette;
        [TagField(Length = 0x18)]
        public byte[] Padding6;
        public List<StructureBspWeatherPaletteBlock> WeatherPalette;
        public List<StructureBspWeatherPolyhedronBlock> WeatherPolyhedra;
        [TagField(Length = 0x18)]
        public byte[] Padding7;
        public List<StructureBspPathfindingSurfacesBlock> PathfindingSurfaces;
        public List<StructureBspPathfindingEdgesBlock> PathfindingEdges;
        public List<StructureBspBackgroundSoundPaletteBlock> BackgroundSoundPalette;
        public List<StructureBspSoundEnvironmentPaletteBlock> SoundEnvironmentPalette;
        public byte[] SoundPasData;
        [TagField(Length = 0x18)]
        public byte[] Padding8;
        public List<StructureBspMarkerBlock> Markers;
        public List<StructureBspDetailObjectDataBlock> DetailObjects;
        public List<StructureBspRuntimeDecalBlock> RuntimeDecals;
        [TagField(Length = 0x8)]
        public byte[] Padding9;
        [TagField(Length = 0x4)]
        public byte[] Padding10;
        public List<GlobalMapLeafBlock> LeafMapLeaves;
        public List<GlobalLeafPortalBlock> LeafMapPortals;
        
        [TagStructure(Size = 0x14)]
        public class StructureCollisionMaterialsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "shdr" })]
            public CachedTag Shader;
            [TagField(Length = 0x4)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x60)]
        public class Bsp : TagStructure
        {
            public List<Bsp3dNode> Bsp3dNodes;
            public List<Plane> Planes;
            public List<Leaf> Leaves;
            public List<Bsp2dReference> Bsp2dReferences;
            public List<Bsp2dNode> Bsp2dNodes;
            public List<Surface> Surfaces;
            public List<Edge> Edges;
            public List<Vertex> Vertices;
            
            [TagStructure(Size = 0xC)]
            public class Bsp3dNode : TagStructure
            {
                public int Plane;
                public int BackChild;
                public int FrontChild;
            }
            
            [TagStructure(Size = 0x10)]
            public class Plane : TagStructure
            {
                public RealPlane3d Plane1;
            }
            
            [TagStructure(Size = 0x8)]
            public class Leaf : TagStructure
            {
                public FlagsValue Flags;
                public short Bsp2dReferenceCount;
                public int FirstBsp2dReference;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    ContainsDoubleSidedSurfaces = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class Bsp2dReference : TagStructure
            {
                public int Plane;
                public int Bsp2dNode;
            }
            
            [TagStructure(Size = 0x14)]
            public class Bsp2dNode : TagStructure
            {
                public RealPlane2d Plane;
                public int LeftChild;
                public int RightChild;
            }
            
            [TagStructure(Size = 0xC)]
            public class Surface : TagStructure
            {
                public int Plane;
                public int FirstEdge;
                public FlagsValue Flags;
                public sbyte BreakableSurface;
                public short Material;
                
                [Flags]
                public enum FlagsValue : byte
                {
                    TwoSided = 1 << 0,
                    Invisible = 1 << 1,
                    Climbable = 1 << 2,
                    Breakable = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class Edge : TagStructure
            {
                public int StartVertex;
                public int EndVertex;
                public int ForwardEdge;
                public int ReverseEdge;
                public int LeftSurface;
                public int RightSurface;
            }
            
            [TagStructure(Size = 0x10)]
            public class Vertex : TagStructure
            {
                public RealPoint3d Point;
                public int FirstEdge;
            }
        }
        
        [TagStructure(Size = 0x6)]
        public class StructureBspNodeBlock : TagStructure
        {
            [TagField(Length = 0x6)]
            public byte[] Unknown;
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspLeafBlock : TagStructure
        {
            [TagField(Length = 0x6)]
            public byte[] Unknown;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public short Cluster;
            public short SurfaceReferenceCount;
            public int SurfaceReferences;
        }
        
        [TagStructure(Size = 0x8)]
        public class StructureBspSurfaceReferenceBlock : TagStructure
        {
            public int Surface;
            public int Node;
        }
        
        [TagStructure(Size = 0x6)]
        public class StructureBspSurfaceBlock : TagStructure
        {
            [TagField(Length = 3)]
            public short[] Vertices;
        }
        
        [TagStructure(Size = 0x20)]
        public class StructureBspLightmapBlock : TagStructure
        {
            public short Bitmap;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            public List<StructureBspMaterialBlock> Materials;
            
            [TagStructure(Size = 0x100)]
            public class StructureBspMaterialBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "shdr" })]
                public CachedTag Shader;
                public short ShaderPermutation;
                public FlagsValue Flags;
                public int Surfaces;
                public int SurfaceCount;
                public RealPoint3d Centroid;
                public RealRgbColor AmbientColor;
                public short DistantLightCount;
                [TagField(Length = 0x2)]
                public byte[] Padding;
                public RealRgbColor DistantLight0Color;
                public RealVector3d DistantLight0Direction;
                public RealRgbColor DistantLight1Color;
                public RealVector3d DistantLight1Direction;
                [TagField(Length = 0xC)]
                public byte[] Padding1;
                public RealArgbColor ReflectionTint;
                public RealVector3d ShadowVector;
                public RealRgbColor ShadowColor;
                public RealPlane3d Plane;
                public short BreakableSurface;
                [TagField(Length = 0x2)]
                public byte[] Padding2;
                [TagField(Length = 2)]
                public VertexBuffersDatum[] VertexBuffers;
                public byte[] UncompressedVertices;
                public byte[] CompressedVertices;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Coplanar = 1 << 0,
                    FogPlane = 1 << 1
                }
                
                [TagStructure(Size = 0x14)]
                public class VertexBuffersDatum : TagStructure
                {
                    [TagField(Length = 0x4)]
                    public byte[] Padding;
                    public int Count;
                    public int Offset;
                    [TagField(Length = 0x8)]
                    public byte[] Padding1;
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspLensFlareBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "lens" })]
            public CachedTag LensFlare;
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspLensFlareMarkerBlock : TagStructure
        {
            public RealPoint3d Position;
            public sbyte DirectionIComponent;
            public sbyte DirectionJComponent;
            public sbyte DirectionKComponent;
            public sbyte LensFlareIndex;
        }
        
        [TagStructure(Size = 0x68)]
        public class StructureBspClusterBlock : TagStructure
        {
            public short Sky;
            public short Fog;
            public short BackgroundSound;
            public short SoundEnvironment;
            public short Weather;
            public short TransitionStructureBsp;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 0x18)]
            public byte[] Padding1;
            public List<PredictedResourceBlock> PredictedResources;
            public List<StructureBspSubclusterBlock> Subclusters;
            public short FirstLensFlareMarkerIndex;
            public short LensFlareMarkerCount;
            public List<StructureBspClusterSurfaceIndexBlock> SurfaceIndices;
            public List<StructureBspMirrorBlock> Mirrors;
            public List<StructureBspClusterPortalIndexBlock> Portals;
            
            [TagStructure(Size = 0x8)]
            public class PredictedResourceBlock : TagStructure
            {
                public TypeValue Type;
                public short ResourceIndex;
                public int TagIndex;
                
                public enum TypeValue : short
                {
                    Bitmap,
                    Sound
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class StructureBspSubclusterBlock : TagStructure
            {
                public Bounds<float> WorldBoundsX;
                public Bounds<float> WorldBoundsY;
                public Bounds<float> WorldBoundsZ;
                public List<StructureBspSubclusterSurfaceIndexBlock> SurfaceIndices;
                
                [TagStructure(Size = 0x4)]
                public class StructureBspSubclusterSurfaceIndexBlock : TagStructure
                {
                    public int Index;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class StructureBspClusterSurfaceIndexBlock : TagStructure
            {
                public int Index;
            }
            
            [TagStructure(Size = 0x40)]
            public class StructureBspMirrorBlock : TagStructure
            {
                public RealPlane3d Plane;
                [TagField(Length = 0x14)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "shdr" })]
                public CachedTag Shader;
                public List<StructureBspMirrorVertexBlock> Vertices;
                
                [TagStructure(Size = 0xC)]
                public class StructureBspMirrorVertexBlock : TagStructure
                {
                    public RealPoint3d Point;
                }
            }
            
            [TagStructure(Size = 0x2)]
            public class StructureBspClusterPortalIndexBlock : TagStructure
            {
                public short Portal;
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class StructureBspClusterPortalBlock : TagStructure
        {
            public short FrontCluster;
            public short BackCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public FlagsValue Flags;
            [TagField(Length = 0x18)]
            public byte[] Padding;
            public List<StructureBspClusterPortalVertexBlock> Vertices;
            
            [Flags]
            public enum FlagsValue : uint
            {
                AiCanTHearThroughThisShit = 1 << 0
            }
            
            [TagStructure(Size = 0xC)]
            public class StructureBspClusterPortalVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
        }
        
        [TagStructure(Size = 0x30)]
        public class StructureBspBreakableSurfaceBlock : TagStructure
        {
            public RealPoint3d Centroid;
            public float Radius;
            public int CollisionSurfaceIndex;
            [TagField(Length = 0x1C)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x20)]
        public class StructureBspFogPlaneBlock : TagStructure
        {
            public short FrontRegion;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public RealPlane3d Plane;
            public List<StructureBspFogPlaneVertexBlock> Vertices;
            
            [TagStructure(Size = 0xC)]
            public class StructureBspFogPlaneVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class StructureBspFogRegionBlock : TagStructure
        {
            [TagField(Length = 0x24)]
            public byte[] Padding;
            public short FogPalette;
            public short WeatherPalette;
        }
        
        [TagStructure(Size = 0x88)]
        public class StructureBspFogPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "fog " })]
            public CachedTag Fog;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string FogScaleFunction;
            [TagField(Length = 0x34)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0xF0)]
        public class StructureBspWeatherPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "rain" })]
            public CachedTag ParticleSystem;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string ParticleSystemScaleFunction;
            [TagField(Length = 0x2C)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "wind" })]
            public CachedTag Wind;
            public RealVector3d WindDirection;
            public float WindMagnitude;
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            [TagField(Length = 32)]
            public string WindScaleFunction;
            [TagField(Length = 0x2C)]
            public byte[] Padding3;
        }
        
        [TagStructure(Size = 0x20)]
        public class StructureBspWeatherPolyhedronBlock : TagStructure
        {
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            public List<StructureBspWeatherPolyhedronPlaneBlock> Planes;
            
            [TagStructure(Size = 0x10)]
            public class StructureBspWeatherPolyhedronPlaneBlock : TagStructure
            {
                public RealPlane3d Plane;
            }
        }
        
        [TagStructure(Size = 0x1)]
        public class StructureBspPathfindingSurfacesBlock : TagStructure
        {
            public sbyte Data;
        }
        
        [TagStructure(Size = 0x1)]
        public class StructureBspPathfindingEdgesBlock : TagStructure
        {
            public sbyte Midpoint;
        }
        
        [TagStructure(Size = 0x74)]
        public class StructureBspBackgroundSoundPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag BackgroundSound;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string ScaleFunction;
            [TagField(Length = 0x20)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x50)]
        public class StructureBspSoundEnvironmentPaletteBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            [TagField(ValidTags = new [] { "snde" })]
            public CachedTag SoundEnvironment;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x3C)]
        public class StructureBspMarkerBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
        }
        
        [TagStructure(Size = 0x40)]
        public class StructureBspDetailObjectDataBlock : TagStructure
        {
            public List<GlobalDetailObjectCellsBlock> Cells;
            public List<GlobalDetailObjectBlock> Instances;
            public List<GlobalDetailObjectCountsBlock> Counts;
            public List<GlobalZReferenceVectorBlock> ZReferenceVectors;
            [TagField(Length = 0x10)]
            public byte[] Padding;
            
            [TagStructure(Size = 0x20)]
            public class GlobalDetailObjectCellsBlock : TagStructure
            {
                public short Unknown;
                public short Unknown1;
                public short Unknown2;
                public short Unknown3;
                public int Unknown4;
                public int Unknown5;
                public int Unknown6;
                [TagField(Length = 0xC)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x6)]
            public class GlobalDetailObjectBlock : TagStructure
            {
                public sbyte Unknown;
                public sbyte Unknown1;
                public sbyte Unknown2;
                public sbyte Unknown3;
                public short Unknown4;
            }
            
            [TagStructure(Size = 0x2)]
            public class GlobalDetailObjectCountsBlock : TagStructure
            {
                public short Unknown;
            }
            
            [TagStructure(Size = 0x10)]
            public class GlobalZReferenceVectorBlock : TagStructure
            {
                public float Unknown;
                public float Unknown1;
                public float Unknown2;
                public float Unknown3;
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class StructureBspRuntimeDecalBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Unknown;
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalMapLeafBlock : TagStructure
        {
            public List<MapLeafFaceBlock> Faces;
            public List<MapLeafPortalIndexBlock> PortalIndices;
            
            [TagStructure(Size = 0x10)]
            public class MapLeafFaceBlock : TagStructure
            {
                public int NodeIndex;
                public List<MapLeafFaceVertexBlock> Vertices;
                
                [TagStructure(Size = 0x8)]
                public class MapLeafFaceVertexBlock : TagStructure
                {
                    public RealPoint2d Vertex;
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class MapLeafPortalIndexBlock : TagStructure
            {
                public int PortalIndex;
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class GlobalLeafPortalBlock : TagStructure
        {
            public int PlaneIndex;
            public int BackLeafIndex;
            public int FrontLeafIndex;
            public List<LeafPortalVertexBlock> Vertices;
            
            [TagStructure(Size = 0xC)]
            public class LeafPortalVertexBlock : TagStructure
            {
                public RealPoint3d Point;
            }
        }
    }
}

