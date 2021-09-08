using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.BlamFile;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using static TagTool.Tags.Definitions.Scenario;

namespace TagTool.Commands.Scenarios
{
    public class GenerateCanvasCommand : Command
    {
        private GameCacheHaloOnlineBase Cache;

        public GenerateCanvasCommand(GameCacheHaloOnlineBase cache) :
            base(false,

                "GenerateCanvas",
                "Generates a mostly empty scenario.",

                "GenerateCanvas [default]",

                "Generates a mostly empty scenario for use with Forge. Use arg \"default\" to bypass input dialog.")
        {
            Cache = cache;
        }

        public class GeneratorParameters
        {
            public string MapName;
            public string MapDescription;
            public string MapAuthor;
            public int MapId;
            public string ScenarioPath;
            public RealRectangle3d WorldBounds;
        }

        public override object Execute(List<string> args)
        {
            var parameters = new GeneratorParameters()
            {
                MapName = "Canvas",
                MapDescription = "ElDewrito Canvas Map v1.0",
                MapAuthor = "ElDewrito",
                MapId = 9001,
                ScenarioPath = @"levels\eldewrito\canvas\canvas",
                WorldBounds = new RealRectangle3d(-1000, 1000, -1000, 1000, -1000, 1000)
            };
            // TODO: take input

            switch (args.Count)
            {
                case 0:
                    {
                        Console.WriteLine(@"Enter desired scenario tagname (e.g. levels\eldewrito\canvas\canvas):");
                        parameters.ScenarioPath = @Console.ReadLine();
                        if (parameters.ScenarioPath.Length < 5)
                            return new TagToolError(CommandError.CustomError, "Please put some effort into your scenario tagname.");

                        Console.WriteLine("Enter the map display name (4-15 characters):");
                        parameters.MapName = Console.ReadLine();
                        if (parameters.MapName.Length > 15 || parameters.MapName.Length < 4)
                            return new TagToolError(CommandError.CustomError, "Provided name must be between 4 and 15 characters.");

                        Console.WriteLine("Enter the map description: (<128 characters)");
                        parameters.MapDescription = Console.ReadLine();
                        if (parameters.MapDescription.Length > 127)
                            return new TagToolError(CommandError.CustomError, "Description exceeds 127 characters.");

                        Console.WriteLine("Enter the map author (4-15 characters):");
                        parameters.MapAuthor = Console.ReadLine();
                        if (parameters.MapAuthor.Length > 15 || parameters.MapAuthor.Length < 4)
                            return new TagToolError(CommandError.CustomError, "Author name must be between 4 and 15 characters.");

                        Console.WriteLine("Enter a mapID (integer) between 7000 and 65535:");
                        if (int.TryParse(Console.ReadLine(), out int result))
                        {
                            parameters.MapId = result;
                            if (parameters.MapId < 7001 || parameters.MapId > 65534)
                                return new TagToolError(CommandError.CustomError, "MapID must be between 7000 and 65535.");
                        }
                        else
                            return new TagToolError(CommandError.CustomError, "MapID must be an integer.");

                        break;
                    }
                case 1:
                    if (args[0].ToLower() == "default")
                        break;
                    else
                        return new TagToolError(CommandError.ArgInvalid);
                default:
                    return new TagToolError(CommandError.ArgCount);
            }

            GenerateCanvas(parameters);
            return true;
        }

        public void GenerateCanvas(GeneratorParameters parameters)
        {
            try
            {
                using (var stream = Cache.OpenCacheReadWrite())
                {
                    Console.WriteLine("Generating scenario...");
                    var scenarioTag = GenerateCanvas(stream, parameters.MapId, parameters.ScenarioPath, parameters.WorldBounds);
                    Console.WriteLine("Generating map file...");
                    GenerateMapFile(stream, Cache, scenarioTag, parameters.MapName, parameters.MapDescription, parameters.MapAuthor);

                    Console.WriteLine($"Finished.");
                    Console.WriteLine("---------------------------------------------------------");
                    Console.WriteLine($"Scenario: {scenarioTag.Name}");
                    Console.WriteLine($"Map ID: {parameters.MapId}");
                }
            }
            finally
            {
                Cache.SaveTagNames();
                Cache.SaveStrings();
            }
        }

        private CachedTag GenerateCanvas(Stream stream, int mapId, string scenarioPath, RealRectangle3d worldBounds)
        {
            var scnrTag = Cache.TagCache.AllocateTag<Scenario>(scenarioPath);
            var sbspTag = Cache.TagCache.AllocateTag<ScenarioStructureBsp>($"{scenarioPath}_bsp_0");
            var lbspTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap_bsp_data_0");
            var sldtTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap");

            Cache.TagCache.TryGetTag<Scenery>(@"levels\multi\riverworld\sky\riverworld", out var skySceneryTag);
            Cache.TagCache.TryGetTag<Wind>(@"levels\multi\riverworld\wind_riverworld", out var windTag);
            Cache.TagCache.TryGetTag<Bitmap>(@"levels\multi\riverworld\riverworld_riverworld_cubemaps", out var cubemapsTag);
            Cache.TagCache.TryGetTag<CameraFxSettings>(@"levels\multi\riverworld\riverworld", out var cfxsTag);
            Cache.TagCache.TryGetTag<SkyAtmParameters>(@"levels\multi\riverworld\sky\riverworld", out var skyaTag);
            Cache.TagCache.TryGetTag<ChocolateMountainNew>(@"levels\multi\riverworld\riverworld", out var chmtTag);
            Cache.TagCache.TryGetTag<ChocolateMountainNew>(@"levels\multi\riverworld\riverworld", out var perfTag);

            Scenario scnr = GenerateScenario(mapId);
            ScenarioStructureBsp sbsp = GenerateStructureBsp(worldBounds);

            var lbsp = new ScenarioLightmapBspData();
            lbsp.BspIndex = 0;

            var sldt = new ScenarioLightmap();
            sldt.LightmapDataReferences = new List<CachedTag>() { lbspTag };

            scnr.Lightmap = sldtTag;
            scnr.StructureBsps = new List<StructureBspBlock>() {
                    new StructureBspBlock() {
                        StructureBsp = sbspTag,
                        Cubemap = cubemapsTag,
                        Wind = windTag,
                        Unknown5 = 32,
                        Unknown6 = -1
                    }
                };

            if (skySceneryTag != null)
            {
                scnr.SkyReferences[0].ActiveBsps |= BspShortFlags.Bsp0;
                scnr.SkyReferences[0].SkyObject = skySceneryTag;
            }

            scnr.DefaultCameraFx = cfxsTag;
            scnr.SkyParameters = skyaTag;
            scnr.GlobalLighting = chmtTag;
            scnr.PerformanceThrottles = perfTag;

            Cache.Serialize(stream, sbspTag, sbsp);
            Cache.Serialize(stream, lbspTag, lbsp);
            Cache.Serialize(stream, sldtTag, sldt);
            Cache.Serialize(stream, scnrTag, scnr);
            return scnrTag;
        }

        private ScenarioStructureBsp GenerateStructureBsp(RealRectangle3d worldBounds)
        {
            var sbsp = new ScenarioStructureBsp();
            sbsp.Leaves = new List<ScenarioStructureBsp.Leaf>() { new ScenarioStructureBsp.Leaf() { ClusterNew = 0 } };
            sbsp.WorldBoundsX = new Bounds<float>(worldBounds.X0, worldBounds.X1);
            sbsp.WorldBoundsY = new Bounds<float>(worldBounds.Y0, worldBounds.Y1);
            sbsp.WorldBoundsZ = new Bounds<float>(worldBounds.Z0, worldBounds.Z1);
            sbsp.Clusters = new List<ScenarioStructureBsp.Cluster>();
            sbsp.Clusters.Add(new ScenarioStructureBsp.Cluster()
            {
                BoundsX = sbsp.WorldBoundsX,
                BoundsY = sbsp.WorldBoundsY,
                BoundsZ = sbsp.WorldBoundsZ,
                ScenarioSkyIndex = -1,
                CameraEffectIndex = -1,
                BackgroundSoundEnvironmentIndex = -1,
                SoundClustersAIndex = 0,
                Unknown3 = -1,
                Unknown4 = -1,
                Unknown5 = -1,
                RuntimeDecalStartIndex = -1,
                MeshIndex = 0,
            });
            sbsp.UnknownSoundClustersA = new List<ScenarioStructureBsp.UnknownSoundClustersBlock>() {
                    new ScenarioStructureBsp.UnknownSoundClustersBlock() {
                        BackgroundSoundEnvironmentIndex = -1,
                    }
                };
            sbsp.CollisionMoppCodes = new List<TagHkpMoppCode>() {
                    new TagHkpMoppCode() {
                        ArrayBase = new HkArrayBase() {
                            Size = 1,
                            CapacityAndFlags = 1 + HkArrayFlags.DONT_DEALLOCATE_FLAG
                        },
                        Data = new TagBlock<byte>(CacheAddressType.Definition, new List<byte> { 0 }) // nop
                    }
                };

            sbsp.CollisionWorldBoundsLower = new RealPoint3d(sbsp.WorldBoundsX.Lower, sbsp.WorldBoundsY.Lower, sbsp.WorldBoundsZ.Lower);
            sbsp.CollisionWorldBoundsUpper = new RealPoint3d(sbsp.WorldBoundsX.Upper, sbsp.WorldBoundsY.Upper, sbsp.WorldBoundsZ.Upper);
            sbsp.Geometry = CreateEmptyRenderGeometry(Cache);
            sbsp.CollisionMaterials = new List<ScenarioStructureBsp.CollisionMaterial>() { new ScenarioStructureBsp.CollisionMaterial() { SeamMappingIndex = -1, RuntimeGlobalMaterialIndex = 0 } };
            sbsp.CollisionBspResource = Cache.ResourceCache.CreateStructureBspResource(CreateStructureBspTagResources(CreateEmptyCollisionGeometry(CacheAddressType.Data)));
            sbsp.PathfindingResource = Cache.ResourceCache.CreateStructureBspCacheFileResource(CreateStructureBspCacheFileTagResources());
            sbsp.Unknown86 = 1; // use CollisionBspResource
            return sbsp;
        }

        private Scenario GenerateScenario(int mapId)
        {
            var scnr = new Scenario();
            scnr.MapType = ScenarioMapType.Multiplayer;
            scnr.CampaignId = -1;
            scnr.MapId = mapId;
            scnr.SoftSurfaces = new List<SoftSurfaceBlock>() { new SoftSurfaceBlock() { Unknown1 = 0 } };
            scnr.ZoneSets = new List<ZoneSet>() {
                        new ZoneSet() {
                        Name = Cache.StringTable.GetStringId("default"),
                        PotentiallyVisibleSetIndex = 0,
                        ImportLoadedBsps = 0,
                        LoadedBsps = BspFlags.Bsp0,
                        ScenarioBspAudibilityIndex = -1
                    }
                };

            scnr.SkyReferences = new List<SkyReference>() {
                new SkyReference()
                {
                    NameIndex = -1,
                }
            };

            scnr.ZoneSetPvs = new List<ZoneSetPvsBlock>()
                {
                    new ZoneSetPvsBlock()
                    {
                        StructureBspMask = BspFlags.Bsp0,
                        StructureBspPotentiallyVisibleSets = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet>() {
                           new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet() {
                               Clusters = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster>() {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster() {
                                       BitVectors = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector>() {
                                           new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector() {
                                               Bits = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit>() {
                                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit () {
                                                        Allow = ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags.Bit0
                                                   }
                                               }
                                           }
                                        }
                                    }
                                },
                               ClustersDoorsClosed = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster>() {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster() {
                                       BitVectors = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector>() {
                                           new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector() {
                                               Bits = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit>() {
                                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit () {
                                                        Allow = ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Cluster.BitVector.Bit.AllowFlags.Bit0
                                                   }
                                               }
                                           }
                                        }
                                    }
                                },
                               ClusterSkies = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky>() {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky()  {
                                       SkyIndex = 0
                                   }
                               },
                               ClusterVisibleSkies = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky>()  {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.Sky() {
                                       SkyIndex = 0
                                   }
                               },
                               Unknown2 = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock>() {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.UnknownBlock() {
                                       Unknown = 0
                                   }
                               },
                               ClusterMappings = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.BspSeamClusterMapping>() {
                                   new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.BspSeamClusterMapping() {
                                       Clusters = new List<ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.BspSeamClusterMapping.ClusterReference>() {
                                           new ZoneSetPvsBlock.StructureBspPotentiallyVisibleSet.BspSeamClusterMapping.ClusterReference() {
                                               ClusterIndex = 0
                                           }
                                       }
                                   }
                               }
                           },
                        },
                        PortalToDeviceMappings = new List<ZoneSetPvsBlock.PortalToDeviceMapping>() {
                            new ZoneSetPvsBlock.PortalToDeviceMapping() {
                                DevicePortalAssociations = new List<ZoneSetPvsBlock.PortalToDeviceMapping.DevicePortalAssociation>(),
                                GamePortalToPortalMappings = new List<ZoneSetPvsBlock.PortalToDeviceMapping.GamePortalToPortalMapping>()
                            }
                        }
                    }
                };

            scnr.PlayerStartingProfile = new List<PlayerStartingProfileBlock>()  {
                 new PlayerStartingProfileBlock() {
                    Name = "start_assault",
                    PrimaryWeapon = Cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\assault_rifle", "weap"),
                    PrimaryRoundsLoaded = 32,
                    PrimaryRoundsTotal = 108,
                    StartingFragGrenadeCount = 2
                }
            };

            return scnr;
        }

        private StructureBspCacheFileTagResources CreateStructureBspCacheFileTagResources()
        {
            var definition = new StructureBspCacheFileTagResources();
            definition.Planes = new TagBlock<PlaneReference>(CacheAddressType.Data);
            definition.SurfacePlanes = new TagBlock<SurfacesPlanes>(CacheAddressType.Data);
            definition.EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data) { new EdgeToSeamMapping() { SeamIndex = -1, SeamIdentifierIndexEdgeMappingIndex = -1 } };
            definition.PathfindingData = new TagBlock<Pathfinding.ResourcePathfinding>(CacheAddressType.Data);
            return definition;
        }

        private StructureBspTagResources CreateStructureBspTagResources(CollisionGeometry collisionGeometry)
        {
            var definition = new StructureBspTagResources();
            definition.CollisionBsps = new TagBlock<CollisionGeometry>(CacheAddressType.Definition) { collisionGeometry };
            return definition;
        }

        private CollisionGeometry CreateEmptyCollisionGeometry(CacheAddressType addressType)
        {
            var collisionGoemetry = new CollisionGeometry()
            {
                Bsp2dNodes = new TagBlock<Bsp2dNode>(addressType),
                Bsp2dReferences = new TagBlock<Bsp2dReference>(addressType),
                Bsp3dNodes = new TagBlock<Bsp3dNode>(addressType),
                Edges = new TagBlock<Edge>(addressType),
                Leaves = new TagBlock<Leaf>(addressType),
                Planes = new TagBlock<Plane>(addressType),
                Surfaces = new TagBlock<Surface>(addressType),
                Vertices = new TagBlock<Vertex>(addressType)
            };

            collisionGoemetry.Bsp3dNodes.Add(new Bsp3dNode() { Value = 0x8000020000010000 }); // left child 2, right child 1, plane 0
            collisionGoemetry.Bsp3dNodes.Add(new Bsp3dNode() { Value = 0x8000018000000001 });  // left child 1, right child 0, plane 1
            collisionGoemetry.Planes.Add(new Plane() { Value = new RealPlane3d(new RealVector3d(0, 0, 0), 0) });
            collisionGoemetry.Planes.Add(new Plane() { Value = new RealPlane3d(new RealVector3d(0, 0, 0), 0) });
            collisionGoemetry.Leaves.Add(new Leaf() { Flags = LeafFlags.ContainsDoubleSidedSurfaces, Bsp2dReferenceCount = 2, FirstBsp2dReference = 0 });
            collisionGoemetry.Leaves.Add(new Leaf() { Flags = LeafFlags.ContainsDoubleSidedSurfaces, Bsp2dReferenceCount = 1, FirstBsp2dReference = 2 });
            collisionGoemetry.Leaves.Add(new Leaf() { Flags = LeafFlags.ContainsDoubleSidedSurfaces, Bsp2dReferenceCount = 1, FirstBsp2dReference = 3 });
            collisionGoemetry.Bsp2dReferences.Add(new Bsp2dReference() { PlaneIndex = -32767, Bsp2dNodeIndex = -32765 });
            collisionGoemetry.Bsp2dReferences.Add(new Bsp2dReference() { PlaneIndex = -32768, Bsp2dNodeIndex = -32767 });
            collisionGoemetry.Bsp2dReferences.Add(new Bsp2dReference() { PlaneIndex = 1, Bsp2dNodeIndex = -32766 });
            collisionGoemetry.Bsp2dReferences.Add(new Bsp2dReference() { PlaneIndex = 0, Bsp2dNodeIndex = -32768 });
            collisionGoemetry.Surfaces.Add(new Surface() { Plane = 0, FirstEdge = 0, MaterialIndex = 0, BreakableSurfaceSet = -1, BreakableSurfaceIndex = -1, Flags = SurfaceFlags.TwoSided });
            collisionGoemetry.Surfaces.Add(new Surface() { Plane = 32768, FirstEdge = 0, MaterialIndex = 0, BreakableSurfaceSet = -1, BreakableSurfaceIndex = -1, Flags = SurfaceFlags.TwoSided });
            collisionGoemetry.Surfaces.Add(new Surface() { Plane = 1, FirstEdge = 4, MaterialIndex = 0, BreakableSurfaceSet = -1, BreakableSurfaceIndex = -1, Flags = SurfaceFlags.TwoSided });
            collisionGoemetry.Surfaces.Add(new Surface() { Plane = 32769, FirstEdge = 4, MaterialIndex = 0, BreakableSurfaceSet = -1, BreakableSurfaceIndex = -1, Flags = SurfaceFlags.TwoSided });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 0, EndVertex = 1, ForwardEdge = 1, ReverseEdge = 3, LeftSurface = 0, RightSurface = 1 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 1, EndVertex = 2, ForwardEdge = 2, ReverseEdge = 0, LeftSurface = 0, RightSurface = 1 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 2, EndVertex = 3, ForwardEdge = 3, ReverseEdge = 1, LeftSurface = 0, RightSurface = 1 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 3, EndVertex = 0, ForwardEdge = 0, ReverseEdge = 2, LeftSurface = 0, RightSurface = 1 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 4, EndVertex = 5, ForwardEdge = 5, ReverseEdge = 7, LeftSurface = 2, RightSurface = 3 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 5, EndVertex = 6, ForwardEdge = 6, ReverseEdge = 4, LeftSurface = 2, RightSurface = 3 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 6, EndVertex = 7, ForwardEdge = 7, ReverseEdge = 5, LeftSurface = 2, RightSurface = 3 });
            collisionGoemetry.Edges.Add(new Edge() { StartVertex = 7, EndVertex = 4, ForwardEdge = 4, ReverseEdge = 6, LeftSurface = 2, RightSurface = 3 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 0, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 1, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 2, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 3, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 4, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 5, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 6, Sink = 0 });
            collisionGoemetry.Vertices.Add(new Vertex() { Point = new RealPoint3d(0, 0, 0), FirstEdge = 7, Sink = 0 });
            return collisionGoemetry;
        }

        private RenderGeometry CreateEmptyRenderGeometry(GameCache cache)
        {
            var resourceDef = new RenderGeometryApiResourceDefinition();
            resourceDef.VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition);
            resourceDef.IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition);

            return CreateEmptyRenderGeometry(resourceDef);
        }

        private RenderGeometry CreateEmptyRenderGeometry(RenderGeometryApiResourceDefinition resourceDef)
        {
            var renderGeometry = new RenderGeometry();

            renderGeometry.Meshes = new List<Mesh>();
            renderGeometry.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();
            renderGeometry.Meshes.Add(new Mesh()
            {
                Type = VertexType.World,
                RigidNodeIndex = -1,
                VertexBufferIndices = new short[8] { -1, -1, -1, -1, -1, -1, -1, -1 },
                IndexBufferIndices = new short[2] { -1, -1 },
            });
            renderGeometry.SetResourceBuffers(resourceDef);

            return renderGeometry;
        }

        private void GenerateMapFile(Stream cacheStream, GameCache cache, CachedTag scenarioTag, string mapName, string mapDescription, string author)
        {
            // TODO: move this map generation logic somewhere so it can be reused.
            // ConvertMultiplayerScenario and UpdateMapFiles are doing more or less the same thing.

            var scenarioName = Path.GetFileName(scenarioTag.Name);

            MapFile map = new MapFile();
            var header = new CacheFileHeaderGenHaloOnline();

            var scnr = cache.Deserialize<Scenario>(cacheStream, scenarioTag);

            map.CachePlatform = cache.Platform;
            map.Version = cache.Version;
            map.EndianFormat = EndianFormat.LittleEndian;
            map.MapVersion = CacheFileVersion.HaloOnline;

            header.HeaderSignature = new Tag("head");
            header.FooterSignature = new Tag("foot");
            header.FileVersion = map.MapVersion;
            header.Build = CacheVersionDetection.GetBuildName(cache.Version, cache.Platform);

            switch (scnr.MapType)
            {
                case ScenarioMapType.MainMenu:
                    header.CacheType = CacheFileType.MainMenu;
                    break;
                case ScenarioMapType.SinglePlayer:
                    header.CacheType = CacheFileType.Campaign;
                    break;
                case ScenarioMapType.Multiplayer:
                    header.CacheType = CacheFileType.Multiplayer;
                    break;
            }
            header.SharedCacheType = CacheFileSharedType.None;

            header.MapId = scnr.MapId;
            header.ScenarioTagIndex = scenarioTag.Index;
            header.Name = scenarioTag.Name.Split('\\').Last();
            header.ScenarioPath = scenarioTag.Name;

            map.Header = header;

            header.FileLength = 0x3390;

            map.MapFileBlf = new Blf(cache.Version, cache.Platform);
            map.MapFileBlf.StartOfFile = new BlfChunkStartOfFile() { Signature = "_blf", Length = 0x30, MajorVersion = 1, MinorVersion = 2, ByteOrderMarker = -2, };
            map.MapFileBlf.Scenario = new BlfScenario() { Signature = "levl", Length = 0x98C0, MajorVersion = 3, MinorVersion = 1 };
            map.MapFileBlf.EndOfFile = new BlfChunkEndOfFile() { Signature = "_eof", Length = 0x11, MajorVersion = 1, MinorVersion = 2 };

            var scnrBlf = map.MapFileBlf.Scenario;
            scnrBlf.MapId = scnr.MapId;
            scnrBlf.Names = new NameUnicode32[12];
            for (int i = 0; i < scnrBlf.Names.Length; i++)
                scnrBlf.Names[i] = new NameUnicode32() { Name = "" };

            scnrBlf.Descriptions = new NameUnicode128[12];
            for (int i = 0; i < scnrBlf.Descriptions.Length; i++)
                scnrBlf.Descriptions[i] = new NameUnicode128() { Name = "" };

            scnrBlf.Names[0] = new NameUnicode32() { Name = mapName };
            scnrBlf.Descriptions[0] = new NameUnicode128() { Name = mapDescription };

            scnrBlf.MapName = scenarioName;
            scnrBlf.ImageName = $"m_{scenarioName}";
            scnrBlf.UnknownTeamCount1 = 2;
            scnrBlf.UnknownTeamCount2 = 6;
            scnrBlf.GameEngineTeamCounts = new byte[11] { 00, 02, 08, 08, 08, 08, 08, 08, 04, 02, 08 };

            scnrBlf.MapFlags = BlfScenarioFlags.GeneratesFilm | BlfScenarioFlags.IsMainmenu | BlfScenarioFlags.IsDlc;

            map.MapFileBlf.ContentFlags |= BlfFileContentFlags.StartOfFile | BlfFileContentFlags.Scenario | BlfFileContentFlags.EndOfFile;

            if (cache is GameCacheModPackage)
            {
                var mapStream = new MemoryStream();
                var writer = new EndianWriter(mapStream, leaveOpen: true);
                map.Write(writer);

                var modPackCache = cache as GameCacheModPackage;
                modPackCache.AddMapFile(mapStream, scnr.MapId);
            }
            else
            {
                var mapFile = new FileInfo(Path.Combine(cache.Directory.FullName, $"{scenarioName}.map"));

                using (var mapFileStream = mapFile.Create())
                {
                    map.Write(new EndianWriter(mapFileStream));
                }
            }
        }
    }
}
