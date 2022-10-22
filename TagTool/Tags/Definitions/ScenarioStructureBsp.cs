using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using System;
using System.Collections.Generic;
using TagTool.Tags.Resources;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Pathfinding;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x3A0, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x388, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x394, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x3AC, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x3B8, MinVersion = CacheVersion.HaloOnline604673, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "scenario_structure_bsp", Tag = "sbsp", Size = 0x51C, MinVersion = CacheVersion.HaloReach)]
    public class ScenarioStructureBsp : TagStructure
    {
        [TagField(Flags = TagFieldFlags.Padding, Length = 12, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused1 = new byte[12];

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StructureBuildIdentifier BuildIdentifier;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StructureBuildIdentifier ParentBuildIdentifier;

        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public int ImportInfoChecksum;
        [TagField(MinVersion = CacheVersion.Halo3Beta)]
        public int ImportVersion;

        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
        public byte[] Unused4;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline106708)]
        public StructureBspCompatibilityValue CompatibilityFlags;

        [TagField(Length = 4, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagField(Length = 4, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        public byte[] Unused5;
        [TagField(Length = 4, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        public byte[] Unused6;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public FlagsValueReach FlagsReach;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ContentPolicyFlagsValue ContentPolicyFlags;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public ContentPolicyFlagsValue FailedContentPolicyFlags;
        [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] Padding1;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<SeamIdentifier> SeamIdentifiers;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<EdgeToSeamMapping> EdgeToSeamEdge;

        public List<CollisionMaterial> CollisionMaterials;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<CollisionGeometry> CollisionBsp;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float VehicleFloorWorldUnits;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public float VehicleCeilingWorldUnits;

        [TagField(Flags = TagFieldFlags.Padding, Length = 8, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused2 = new byte[8];

        public List<Leaf> Leaves;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.ScenarioStructureBsp.StructureSuperNodeAabbsBlock> SuperAabbs;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.ScenarioStructureBsp.SuperNodeMappingsBlock> SuperNodeParentMappings;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.ScenarioStructureBsp.SuperNodeRecursableMasksBlock> SuperNodeRecursableMasks;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.ScenarioStructureBsp.StructureSuperNodeTraversalGeometryBlock> SuperNodeTraversalGeometry;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public CollisionKdHierarchyStatic InstanceKdHierarchy;

       

        public Bounds<float> WorldBoundsX;
        public Bounds<float> WorldBoundsY;
        public Bounds<float> WorldBoundsZ;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<SurfaceReference> StructureSurfacesH2;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        [TagField(MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        public List<StructureSurface> StructureSurfaces;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        public List<LargeStructureSurface> LargeStructureSurfaces;

        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public List<StructureSurfaceToTriangleMapping> StructureSurfaceToTriangleMapping;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] ClusterData;

        public List<ClusterPortal> ClusterPortals;

        [TagField(MaxVersion = CacheVersion.Halo2Vista)]
        public List<int> FogPlanes;

        [TagField(Flags = TagFieldFlags.Padding, Length = 24, MaxVersion = CacheVersion.Halo2Vista)]
        public byte[] Unused3 = new byte[24];

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<WeatherPaletteEntry> WeatherPalette;

        [TagField(MinVersion = CacheVersion.Halo3Beta, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<StructureBspAtmospherePaletteBlock> AtmospherePalette;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<StructureBspCameraFxPaletteBlock> CameraFxPalette;

        [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
        public List<LocationNameBlock> LocationNames;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NullBlock> WeatherPolyhedra; // use WeatherPolyhedron when tags are fixed

        public List<DetailObject> DetailObjects;
        public List<Cluster> Clusters;
        public List<RenderMaterial> Materials;
        public List<SkyOwnerClusterBlock> SkyOwnerCluster;
        public List<ConveyorSurface> ConveyorSurfaces;
        public List<BreakableSurfaceBits> BreakableSurfaces;
        public List<TagPathfinding> PathfindingData;
        public List<StructureBspPathfindingEdgesBlock> PathfindingEdges;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<Gen4.ScenarioStructureBsp.StructureCookieCutterDefinition> CookieCutters;

        public List<AcousticsPaletteBlock> AcousticsPalette;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NullBlock> BackgroundSoundPalette;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NullBlock> SoundEnvironmentPalette;

        public byte[] SoundPASData;

        public List<Marker> Markers;
        public List<TagReferenceBlock> MarkerLightPalette;
        public List<MarkerLightIndex> MarkerLightPaletteIndex;
        public List<RuntimeDecal> RuntimeDecals;

        public List<EnvironmentObjectPaletteBlock> EnvironmentObjectPalette;
        public List<EnvironmentObject> EnvironmentObjects;

        [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
        public byte[] Unused7;

        public List<NullBlock> LeafMapLeaves;
        public List<NullBlock> LeafMapConnections;
        public List<NullBlock> Errors;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagHkpMoppCode> ClusterToInstanceGroupMopps;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<TagHkpMoppCode> InstanceGroupToInstanceMopps;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<StructureInstanceClusterDefinition> ClusterToInstanceGroupSpheres;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<StructureInstanceGroupDefinition> InstanceGroupToInstanceSpheres;

        public List<InstancedGeometryInstance> InstancedGeometryInstances;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<InstancedGeometryInstanceName> InstancedGeometryInstanceNames;
        [TagField(ValidTags = new[] { "iimz" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag InstanceImposters;
        public List<TagReferenceBlock> Decorators;
        public RenderGeometry DecoratorGeometry;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<BspPreplacedDecalSetReferenceBlock> PreplacedDecalSets;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<BspPreplacedDecalReferenceBlock> PreplacedDecals;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public RenderGeometry PreplacedDecalGeometry;

      

        public List<StructureBspSoundClusterBlock> AcousticsSoundClusters;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<StructureBspSoundClusterBlock> AmbienceSoundClusters;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<StructureBspSoundClusterBlock> ReverbSoundClusters;

        public List<TransparentPlane> TransparentPlanes;
        public List<NullBlock> DebugInfo;

        public StructurePhysics Physics = new StructurePhysics();

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public List<NullBlock> Audability;
        
        public List<NullBlock> ObjectFakeLightprobes;
        public RenderGeometry Geometry;

        public List<WidgetReferenceBlock> WidgetReferences;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<NullBlock> CheapLightReferences;

        public List<StructureBspTagResources> RawResources;

        public TagResourceReference CollisionBspResource;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public TagResourceReference PathfindingResource;

        [TagField(MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline106708)]
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public int UseResourceItems;

        // probably padding
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown87;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown88;
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown89;

        [TagField(MinVersion = CacheVersion.HaloOnline301003, MaxVersion = CacheVersion.HaloOnline700123)]
        public uint Unknown90;

        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloOnline700123)]
        public class LocationNameBlock : TagStructure
        {
            public StringId Name;
        }

        [TagStructure(Size = 0x18)]
        public class StructureBuildIdentifier : TagStructure
        {
            [TagField(Length = 4)]
            public int[] ManifestId = new int[4];
            public int BuildIndex;
            public int StructureImporterVersion;
        }

        [Flags]
        public enum FlagsValueReach : ushort
        {
            None = 0,
            HasInstanceGroups = 1 << 0,
            SurfaceToTriangleMappingRemapped = 1 << 1,
            ExternalReferencesConvertedToIo = 1 << 2,
            StructureMoppNeedsRebuilt = 1 << 3,
            StructurePrefabMaterialsNeedPostprocessing = 1 << 4,
            SerializedHavokDataConvertedToTargetPlatform = 1 << 5
        }

        [Flags]
        public enum ContentPolicyFlagsValue : ushort
        {
            None = 0,
            HasWorkingPathfinding = 1 << 0,
            ConvexDecompositionEnabled = 1 << 1
        }

        [Flags]
        public enum StructureBspCompatibilityValue : ushort
        {
            None = 0,
            UseMoppIndexPatch = 1 << 0,
            Reach = 1 << 1
        }

        [TagStructure(Size = 0x28)]
        public class SeamIdentifier : TagStructure
        {
            [TagField(Length = 4)]
            public int[] SeamIDs;
            public List<Edge> EdgeMapping;
            public List<Cluster> ClusterMapping;

            [TagStructure(Size = 0x4)]
            public class Edge : TagStructure
            {
                public int StructureEdgeIndex;
            }

            [TagStructure(Size = 0x10)]
            public class Cluster : TagStructure
            {
                public int ClusterIndex;
                public RealPoint3d ClusterCenter;
            }
        }

        [TagStructure(Size = 0x14, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.Halo3Retail)]
        public class CollisionMaterial : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label, MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTag OldShader;

            [TagField(Flags = TagFieldFlags.Label, MinVersion = CacheVersion.Halo3Retail)]
            public CachedTag RenderMethod;

            [TagField(Flags = TagFieldFlags.GlobalMaterial)]
            public short RuntimeGlobalMaterialIndex;
            public short ConveyorSurfaceIndex;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTag NewShader;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public short SeamMappingIndex;

            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public FlagsValue Flags;

            [Flags]
            public enum FlagsValue : ushort
            {
                None = 0,
                IsSeam = 1 << 0
            }
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x1, MinVersion = CacheVersion.Halo3Retail)]
        public class Leaf : TagStructure
        {
            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short ClusterOld;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public byte ClusterNew;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public short SurfaceReferenceCount;

            [TagField(MaxVersion = CacheVersion.Halo2Vista)]
            public int FirstSurfaceReferenceIndex;

            public override void PostConvert(CacheVersion from, CacheVersion to)
            {
                if (from <= CacheVersion.Halo2Vista && to >= CacheVersion.Halo3Retail)
                    ClusterNew = (byte)ClusterOld;
            }
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x50, MinVersion = CacheVersion.HaloReach)]
        public class ClusterPortal : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public OrientedBoundsStruct OrientedBounds;
            public short BackCluster;
            public short FrontCluster;
            public int PlaneIndex;
            public RealPoint3d Centroid;
            public float BoundingRadius;
            public FlagsValue Flags;
            public List<Vertex> Vertices;

            [Flags]
            public enum FlagsValue : uint
            {
                None = 0,
                AiCantHearThroughThisShit = 1 << 0,
                OneWay = 1 << 1,
                Door = 1 << 2,
                NoWay = 1 << 3,
                OneWayReversed = 1 << 4,
                NoOneCanHearThroughThis = 1 << 5
            }

            [TagStructure(Size = 0xC)]
            public class Vertex : TagStructure
            {
                public RealPoint3d Position;
            }

            [TagStructure(Size = 0x28)]
            public class OrientedBoundsStruct : TagStructure
            {
                public RealPoint3d Center;
                public RealVector3d Extents;
                public RealQuaternion Orientation;
            }
        }

        [TagStructure(Size = 0x8, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x18, MinVersion = CacheVersion.HaloReach)]
        public class StructureBspAtmospherePaletteBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;

            public short AtmosphereSettingIndex;
            [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
            public byte[] Padding0;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag AtmosphereFog;
        }

        [TagStructure(Size = 0x88, MinVersion = CacheVersion.Halo2Xbox, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.Halo3Beta)]
        public class WeatherPaletteEntry : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;

            [TagField(ValidTags = new[] { "weat" }, MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTag WeatherSystem;

            [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Unused1 = new byte[2];

            [TagField(Flags = TagFieldFlags.Padding, Length = 2)]
            public byte[] Unused2 = new byte[2];

            [TagField(Flags = TagFieldFlags.Padding, Length = 32)]
            public byte[] Unused3 = new byte[32];

            [TagField(ValidTags = new[] { "wind" }, MaxVersion = CacheVersion.Halo2Vista)]
            public CachedTag Wind;

            public RealVector3d WindDirection;
            public float WindMagnitude;

            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Unused4;

            public short RuntimeWindGlobalScenarioFunctionIndex;

            [TagField(Length = 32)]
            public string WindScaleFunction;
        }

        [TagStructure(Size = 0x18, MaxVersion = CacheVersion.Halo2Vista)]
        [TagStructure(Size = 0x1C, MinVersion = CacheVersion.Halo3Beta)]
        public class WeatherPolyhedron : TagStructure
        {
            public RealPoint3d BoundingSphereCenter;
            public float BoundingSphereRadius;
            public List<PlaneBlock> Planes;

            [TagStructure(Size = 0x10)]
            public class PlaneBlock : TagStructure
            {
                public RealPlane3d Plane;
            }
        }

        [TagStructure(Size = 0x30)]
        public class StructureBspCameraFxPaletteBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;
            // if empty, uses default
            public CachedTag ClusterCameraFxTag;
            public CameraFxPaletteFlags Flags;
            [TagField(Length = 3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // the target exposure (ONLY USED WHEN FORCE EXPOSURE IS CHECKED)
            public float ForcedExposure; // stops
            // how bright you want the screen to be (ONLY USED WHEN FORCE AUTO EXPOSURE IS CHECKED)
            public float ForcedAutoexposureScreenBrightness; // [0.0001-1]
            public float ExposureMin; // stops
            public float ExposureMax; // stops
            public float InherentBloom;
            public float BloomIntensity;

            [Flags]
            public enum CameraFxPaletteFlags : byte
            {
                ForceExposure = 1 << 0,
                ForceAutoexposure = 1 << 1,
                OverrideExposureBounds = 1 << 2,
                OverrideInherentBloom = 1 << 3,
                OverrideBloomIntensity = 1 << 4
            }
        }

        [TagStructure(Size = 0x34)]
        public class DetailObject : TagStructure
        {
            public List<GlobalDetailObjectCellsBlock> Cells;
            public List<GlobalDetailObjectBlock> Instances;
            public List<GlobalDetailObjectCountsBlock> Counts;
            public List<GlobalZReferenceVectorBlock> ZReferenceVectors;
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Unused;

            [TagStructure(Size = 0x20)]
            public class GlobalDetailObjectCellsBlock : TagStructure
            {
                public short CellX;
                public short CellY;
                public short CellZ;
                public short OffsetZ;
                public int ValidLayersFlags;
                public int StartIndex;
                public int CountIndex;
                [TagField(Length = 12, Flags = TagFieldFlags.Padding)]
                public byte[] Unused;
            }

            [TagStructure(Size = 0x6)]
            public class GlobalDetailObjectBlock : TagStructure
            {
                public sbyte PositionX;
                public sbyte PositionY;
                public sbyte PositionZ;
                public sbyte Data;
                public short Color;
            }

            [TagStructure(Size = 0x2)]
            public class GlobalDetailObjectCountsBlock : TagStructure
            {
                public short Count;
            }

            [TagStructure(Size = 0x10)]
            public class GlobalZReferenceVectorBlock : TagStructure
            {
                public float ZReferenceI;
                public float ZReferenceJ;
                public float ZReferenceK;
                public float ZReferenceL;
            }
        }

        [TagStructure(Size = 0x118, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
        [TagStructure(Size = 0xDC, MaxVersion = CacheVersion.HaloOnline106708, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0xE0, MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public class Cluster : TagStructure
        {
            public Bounds<float> BoundsX;
            public Bounds<float> BoundsY;
            public Bounds<float> BoundsZ;

            public sbyte ScenarioSkyIndex; // not used in reach
            public sbyte AtmosphereIndex;
            public sbyte CameraFxIndex;
            public sbyte WeatherIndex;
            public short BackgroundSoundEnvironmentIndex; // acoustics index
            public short AcousticsSoundClusterIndex;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown3;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown4;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown5;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown6;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown7;

            [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
            public short LocationNameIndex;
            [TagField(MinVersion = CacheVersion.HaloOnline700123, MaxVersion = CacheVersion.HaloOnline700123)]
            public short Ms30Unknown1;

            public short RuntimeDecalStartIndex;
            public short RuntimeDecalCount;

            public ClusterFlags Flags;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding1;

            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] PredictedResourcesPadding = new byte[0xC]; // predicted resources block
            public List<Portal> Portals;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public InstancedGeometryPhysicsData InstancedGeometryPhysics;
            public short MeshIndex;

            [TagField(Length = 2, Flags = TagFieldFlags.Padding, MaxVersion = CacheVersion.HaloOnline700123)]
            public byte[] Padding2;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public short InstanceImposterClusterMoppIndex;

            public List<Seam> SeamIndices;
            public List<DecoratorGrid> DecoratorGrids;

            [TagField(MinVersion = CacheVersion.HaloReach)]
            public List<CheapLightMarkerRefBlock> CheapLightMarkerRefs;

            public List<PvsBoundObjectIdentifiersBlock> PvsBoundObjectIdentifiers;
            public List<PvsBoundObjectReferencesBlock> PvsBoundObjectReferences;
            public List<StructureClusterCubemap> ClusterCubemaps;

            [TagStructure(Size = 0x2)]
            public class Portal : TagStructure
            {
                public short PortalIndex;
            }

            [Flags]
            public enum ClusterFlags : ushort
            {
                None = 0,
                OneWayPortal = 1 << 0,
                DoorPortal = 1 << 1,
                PostProcessedGeometry = 1 << 2,
                IsTheSky = 1 << 3,
                DecoratorsAreLit = 1 << 4
            }

            [TagStructure(Size = 0x60, Platform = CachePlatform.MCC)]
            [TagStructure(Size = 0x3C, Platform = CachePlatform.Original)]
            public class InstancedGeometryPhysicsData : HkpShapeCollection
            {
                public CachedTag StructureBsp;
                public int ClusterIndex;
                [TagField(Align = 4, Platform = CachePlatform.Original)]
                [TagField(Align = 8, Platform = CachePlatform.MCC)]
                public HkpMoppBvTreeShape Shape;
                public List<TagHkpMoppCode> MoppCodes;
                [TagField(Length = 0x4, Platform = CachePlatform.MCC)]
                public byte[] Padding6;
            }

            [TagStructure(Size = 0x1)]
            public class Seam : TagStructure
            {
                public sbyte SeamIndex;
            }

            [TagStructure(Size = 0x3C, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
            [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x34, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            [TagStructure(Size = 0x30, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
            public class DecoratorGrid : TagStructure
            {
                public short Amount;

                [TagField(Gen = CacheGeneration.Third)]
                public Gen3DecoratorInfo Gen3Info;

                [TagField(Gen = CacheGeneration.HaloOnline)]
                public HaloOnlineDecoratorInfo HaloOnlineInfo;

                public int VertexBufferOffset;
                public RealPoint3d Position;
                public float Radius;
                public RealPoint3d GridSize;
                public RealPoint3d BoundingSphereOffset;

                [TagField(Platform = CachePlatform.MCC)]
                public List<short> ModelStartIndex;

                [TagField(Flags = TagFieldFlags.Runtime)]
                public List<TinyPositionVertex> Vertices;

                [TagStructure(Size = 0x2)]
                public class Gen3DecoratorInfo : TagStructure
                {
                    public sbyte PaletteIndex;
                    public byte VertexBufferIndex;
                }

                [TagStructure(Size = 0x6)]
                public class HaloOnlineDecoratorInfo : TagStructure
                {
                    public short PaletteIndex;
                    public short Variant;
                    public short VertexBufferIndex;
                }
            }

            [TagStructure(Size = 0x8)]
            public class PvsBoundObjectIdentifiersBlock : TagStructure
            {
                public int UniqueId;
                public short OriginBspIndex;
                public GameObjectType8 Type;
                public Scenario.ObjectSource Source;
            }

            [TagStructure(Size = 0x4)]
            public class PvsBoundObjectReferencesBlock : TagStructure
            {
                public GameObjectType16 ObjectType;
                public short PlacementIndex;
            }

            [TagStructure(Size = 0x10)]
            public class StructureClusterCubemap : TagStructure
            {
                [TagField(MaxVersion = CacheVersion.Halo3ODST)]
                [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline700123)]
                public RealPoint3d Position;
                public short ScenarioCubemapIndex;
                public short CubemapBitmapIndex;
                [TagField(MinVersion = CacheVersion.HaloReach)]
                [TagField(Version = CacheVersion.HaloOnlineED)]
                public List<CubemapReferencePointsBlock> ReferencePoints;

                [TagStructure(Size = 0x10)]
                public class CubemapReferencePointsBlock : TagStructure
                {
                    public RealPoint3d ReferencePoint;
                    public int PointIndex;
                }
            }

            [TagStructure(Size = 0x4)]
            public class CheapLightMarkerRefBlock : TagStructure
            {
                public short CheapLightReferenceReference;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }

        [TagStructure(Size = 0x2)]
        public class SkyOwnerClusterBlock : TagStructure
        {
            public short Value;
        }

        [TagStructure(Size = 0x18)]
        public class ConveyorSurface : TagStructure
        {
            public RealVector3d U;
            public RealVector3d V;
        }



        [TagStructure(Size = 0x54, MaxVersion = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.Halo3ODST, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x78, MinVersion = CacheVersion.HaloReach)]
        public class AcousticsPaletteBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;
            public CachedTag SoundEnvironment;
            [TagField(MinVersion = CacheVersion.Halo3ODST)]
            public SoundEnvironmentType Type;
            public float ReverbCutoffDistance;
            public float ReverbInterpolationSpeed;
            public CachedTag AmbienceBackgroundSound;
            public CachedTag AmbienceInsideClusterSound;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag EntrySound;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public CachedTag ExitSound;
            public float AmbienceCutoffDistance;
            public BackgroundSoundScaleFlags AmbienceScaleFlags;
            public float AmbienceInteriorScale;
            public float AmbiencePortalScale;
            public float AmbienceExteriorScale;
            public float AmbienceInterpolationSpeed;
        }

        public enum SoundEnvironmentType : int
        {
            Default,
            InteriorNarrow,
            InteriorSmall,
            InteriorMedium,
            InteriorLarge,
            ExteriorSmall,
            ExteriorMedium,
            ExteriorLarge,
            ExteriorHalfOpen,
            ExteriorOpen
        }

        [Flags]
        public enum BackgroundSoundScaleFlags : int
        {
            None = 0,
            OverrideDefaultScale = 1 << 0,
            UseAdjacentClusterAsPortalScale = 1 << 1,
            UseAdjacentClusterAsExteriorScale = 1 << 2,
            ScaleWithWeatherIntensity = 1 << 3
        }

        [TagStructure(Size = 0x3C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x40, MinVersion = CacheVersion.HaloReach)]
        public class Marker : TagStructure
        {
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StructureMarkerTypeEnum MarkerType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
            public byte[] Padding1;

            [TagField(Length = 32, Flags = TagFieldFlags.Label)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;

            public enum StructureMarkerTypeEnum : sbyte
            {
                None,
                CheapLight,
                FallingLeafGenerator,
                Light,
                Sky,
                Model
            }
        }

        [TagStructure(Size = 0x2)]
        public class MarkerLightIndex : TagStructure
        {
            public short PaletteIndex;
        }

        [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach)]
        public class RuntimeDecal : TagStructure
        {
            public short PaletteIndex;
            public sbyte Yaw;
            public sbyte Pitch;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public float Scale;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public RealVector2d ScaleReach; // x, y
        }

        [TagStructure(Size = 0x24)]
        public class EnvironmentObjectPaletteBlock : TagStructure
        {
            public CachedTag Definition;
            public CachedTag Model;
            public ObjectTypeValue ObjectType;

            [Flags]
            public enum ObjectTypeValue : int
            {
                None,
                Biped,
                Vehicle,
                Weapon = 4,
                Equipment = 8,
                ArgDevice = 16,
                Terminal = 32,
                Projectile = 64,
                Scenery = 128,
                Machine = 256,
                Control = 512,
                SoundScenery = 1024,
                Crate = 2048,
                Creature = 4096,
                Giant = 8192,
                EffectScenery = 16384
            }
        }

        [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x54, MinVersion = CacheVersion.HaloReach)]
        public class EnvironmentObject : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealQuaternion Rotation;
            public RealPoint3d Position;
            public float Scale;
            public short PaletteIndex;
            public FlagsValue Flags;
            [TagField(Flags = TagFieldFlags.Padding, Length = 1)]
            public byte[] Padding1 = new byte[1];
            public int UniqueId;
            public Tag ExportedObjectType;
            [TagField(Length = 32, MaxVersion = CacheVersion.HaloOnline700123)]
            public string ScenarioObjectName;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId ScenarioObjectNameReach;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public StringId VariantName;

            [Flags]
            public enum FlagsValue : byte
            {
                None,
                ScriptsAlwaysRun = 1 << 0
            }
        }

        [TagStructure(Size = 0x4, MinVersion = CacheVersion.HaloReach)]
        public class InstancedGeometryInstanceName : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Label)]
            public StringId Name;
        }

        [TagStructure(Size = 0x1C)]
        public class StructureBspSoundClusterBlock : TagStructure
        {
            public short PaletteIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public List<PortalDesignatorBlock> EnclosingPortalDesignators;
            public List<InteriorClusterIndexBlock> InteriorClusterIndices;

            [TagStructure(Size = 0x2)]
            public class PortalDesignatorBlock : TagStructure
            {
                public short PortalDesignator;
            }

            [TagStructure(Size = 0x2)]
            public class InteriorClusterIndexBlock : TagStructure
            {
                public short InteriorClusterIndex;
            }
        }

        [TagStructure(Size = 0x14)]
        public class TransparentPlane : TagStructure
        {
            public short MeshIndex;
            public short PartIndex;
            public RealPlane3d Plane;
        }

        [TagStructure(Size = 0x20)]
        public class BreakableSurfaceKeyTableBlock : TagStructure
        {
            public short InstancedGeometryIndex;
            public sbyte BreakableSurfaceIndex;
            public byte BreakableSurfaceSubIndex;
            public int SeedSurfaceIndex;
            public Bounds<float> X;
            public Bounds<float> Y;
            public Bounds<float> Z;
        }

        [TagStructure(Size = 0x14)]
        public class WidgetReferenceBlock : TagStructure
        {
            public short MarkerIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new[] { "lswd" })]
            public CachedTag WidgetRef;
        }

        [TagStructure(Size = 0x1C)]
        public class BspPreplacedDecalSetReferenceBlock : TagStructure
        {
            [TagField(Flags = TagFieldFlags.Short)]
            public CachedTag DecalDefinitionIndex;
            public sbyte LocationBsp0;
            public byte LocationCluster0;
            public sbyte LocationBsp1;
            public byte LocationCluster1;
            public sbyte LocationBsp2;
            public byte LocationCluster2;
            public sbyte LocationBsp3;
            public byte LocationCluster3;
            public RealPoint3d Center;
            public short FirstDecalRefIndex;
            public short DecalRefCount;
        }

        [TagStructure(Size = 0x1C)]
        public class BspPreplacedDecalReferenceBlock : TagStructure
        {
            public short IndexStart;
            public short IndexCount;
            public short VertexStart;
            public short VertexCount;
            public short DefinitionBlockIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public RealPoint2d SpiritCorner;
            public RealVector2d SpiritSize;
        }

        [TagStructure(Size = 0x10)]
        public class StructureInstanceClusterDefinition : TagStructure
        {
            public StructureInstanceClusterFlags Flags;
            public List<IndexListBlock> InstanceGroupIndices;

            [Flags]
            public enum StructureInstanceClusterFlags : uint
            {
                OptimizedMopp = 1 << 0
            }

            [TagStructure(Size = 0x2)]
            public class IndexListBlock : TagStructure
            {
                public ushort Index;
            }
        }

        [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloReach, MaxVersion = CacheVersion.HaloReach11883)]
        [TagStructure(Size = 0x2C, MinVersion = CacheVersion.Halo4)]
        public class StructureInstanceGroupDefinition : TagStructure
        {
            public RealPoint3d Center;
            public float Radius;
            public short ClusterCount;
            public StructureInstanceGroupFlags Flags;
            public float MaximumImposterDistance;
            public float MinimumCentrifugalDistanceFromGroupCenter;
            [TagField(MinVersion = CacheVersion.Halo4)]
            public float MinimumImposterDistanceSquared;
            public List<IndexListBlock> InstanceIndices;

            [Flags]
            public enum StructureInstanceGroupFlags : ushort
            {
                ContainsCardImposters = 1 << 0,
                ContainsPolyImposters = 1 << 1,
                IsDecoratorType = 1 << 2,
                OptimizedMopp = 1 << 3
            }

            [TagStructure(Size = 0x2)]
            public class IndexListBlock : TagStructure
            {
                public ushort Index;
            }
        }

        [TagStructure(Size = 0x40)]
        public class StructurePhysics : TagStructure
        {
            public List<TagHkpMoppCode> CollisionMoppCodes;
            [TagField(Length = 4, Flags = TagFieldFlags.Padding)]
            public byte[] Unused;
            public RealPoint3d MoppBoundsMin;
            public RealPoint3d MoppBoundsMax;
            public List<TagHkpMoppCode> BreakableSurfaceMoppCodes;
            public List<BreakableSurfaceKeyTableBlock> BreakableSurfaceKeyTable;

            [TagStructure(Size = 0x20)]
            public class BreakableSurfaceKeyTableBlock : TagStructure
            {
                public short InstancedGeometryIndex;
                public sbyte BreakableSurfaceIndex;
                public byte BreakableSurfaceSubIndex;
                public int SeedSurfaceIndex;
                public Bounds<float> X;
                public Bounds<float> Y;
                public Bounds<float> Z;
            }
        }
    }
}
