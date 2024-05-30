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

                "GenerateCanvas [worldtype] [quick]",

                "Generates a mostly empty scenario for use with Forge. Use arg 'quick' to bypass custom parameter dialog.\n" +
                "Worldtypes include 'default' (empty) and 'water' (very large plane of bsp water).\n" +
                "Using the command without args will begin a dialog for a default worldtype scenario.")
        {
            Cache = cache;
        }

        public enum WorldType
        {
            Default,
            Water
        }

        public class GeneratorParameters
        {
            public string MapName;
            public string MapDescription;
            public string MapAuthor;
            public int MapId;
            public string ScenarioPath;
            public RealRectangle3d WorldBounds;
            public WorldType WorldType;
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
                WorldBounds = new RealRectangle3d(-200, 200, -200, 200, -200, 200)
            };

            bool quick = false;

            if (args.Count > 0)
            {
                switch (args[0].ToLower())
                {
                    case "default":
                        break;
                    case "water":
                        parameters.WorldType = WorldType.Water;
                        break;
                    case "quick":
                        quick = true;
                        break;
                    default:
                        return new TagToolError(CommandError.ArgInvalid);
                }

                if (args.Count == 2 && args[1].ToLower() == "quick")
                    quick = true;

                if (args.Count > 2)
                    return new TagToolError(CommandError.ArgCount);
            }

            if (quick || AskForParameterInput(parameters))
                GenerateCanvas(parameters);

            return true;
        }

        private bool AskForParameterInput(GeneratorParameters parameters)
        {
            if (!RequestBoundedInput("Enter desired scenario tagname (e.g. levels\\eldewrito\\canvas\\canvas):",
                out parameters.ScenarioPath))
                return false;

            var fullName = $"{parameters.ScenarioPath}.scnr";
            if (Cache.TagCache.TagExists(fullName))
            {
                new TagToolError(CommandError.CustomError, "A scenario tag with this name already exists.");
                return false;
            }
            else if (!Cache.TagCache.IsTagPathValid(fullName))
            {
                new TagToolError(CommandError.CustomError, $"Malformed target tag path '{parameters.ScenarioPath}'");
                return false;
            }

            if (!RequestBoundedInput("Enter the map display name (<16 characters):",
                out parameters.MapName, 15))
                return false;

            if (!RequestBoundedInput("Enter the map description: (<128 characters)",
                out parameters.MapDescription, 127))
                return false;

            if (!RequestBoundedInput("Enter the map author (<16 characters):",
                out parameters.MapDescription, 15))
                return false;

            if (!RequestBoundedInput("Enter a mapID (integer) between 7000 and 65535:",
                out string mapIdString, 5))
                return false;

            if (int.TryParse(mapIdString, out int result))
            {
                parameters.MapId = result;
                if (parameters.MapId < 7001 || parameters.MapId > 65534)
                {
                    new TagToolError(CommandError.CustomError, "MapID must be between 7000 and 65535.");
                    return false;
                }
            }
            else
            {
                new TagToolError(CommandError.CustomError, "MapID must be an integer.");
                return false;
            }

            return true;
        }

        private bool RequestBoundedInput(string prompt, out string value, int upperBound = 0, int lowerBound = 0)
        {
            Console.WriteLine(prompt);
            string input = CommandRunner.ApplyUserVars(@Console.ReadLine().ToLower(), IgnoreArgumentVariables);

            if (InputIsValid(input, upperBound, lowerBound))
            {
                value = input;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        private bool InputIsValid(string input, int upperBound = 0, int lowerBound = 0)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                new TagToolError(CommandError.CustomError, $"Input is null or empty.");
                return false;
            }

            if (upperBound > 0 && input.Length > upperBound)
            {
                new TagToolError(CommandError.CustomError, $"Input exceeds {upperBound} characters.");
                return false;
            }

            if (input.Contains("|"))
            {
                new TagToolError(CommandError.CustomError, $"Input contains invalid characters.");
                return false;
            }

            return true;
        }

        public void GenerateCanvas(GeneratorParameters parameters)
        {
            try
            {
                using (var stream = Cache.OpenCacheReadWrite())
                {
                    Console.WriteLine("\nGenerating scenario...");
                    var scenarioTag = GenerateCanvas(stream, parameters.WorldType, parameters.MapId, parameters.ScenarioPath, parameters.WorldBounds);
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

        private CachedTag GenerateCanvas(Stream stream, WorldType type, int mapId, string scenarioPath, RealRectangle3d worldBounds)
        {
            var scnrTag = Cache.TagCache.AllocateTag<Scenario>(scenarioPath);
            var sbspTag = Cache.TagCache.AllocateTag<ScenarioStructureBsp>($"{scenarioPath}_bsp_0");
            var lbspTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap_bsp_data_0");
            var sldtTag = Cache.TagCache.AllocateTag<ScenarioLightmapBspData>($"{scenarioPath}_faux_lightmap");
            var cfxsTag = Cache.TagCache.AllocateTag<CameraFxSettings>($"{scenarioPath}");

            var cfxs = Cache.Deserialize<CameraFxSettings>(stream, Cache.TagCache.GetTag<CameraFxSettings>(@"globals\default"));

            Cache.TagCache.TryGetTag<Scenery>(@"levels\multi\riverworld\sky\riverworld", out var skySceneryTag);
            Cache.TagCache.TryGetTag<Wind>(@"levels\multi\riverworld\wind_riverworld", out var windTag);
            Cache.TagCache.TryGetTag<Bitmap>(@"levels\multi\riverworld\riverworld_riverworld_cubemaps", out var cubemapsTag);
            Cache.TagCache.TryGetTag<SkyAtmParameters>(@"levels\multi\riverworld\sky\riverworld", out var skyaTag);
            Cache.TagCache.TryGetTag<ChocolateMountainNew>(@"levels\multi\riverworld\riverworld", out var chmtTag);
            Cache.TagCache.TryGetTag<PerformanceThrottles>(@"levels\multi\riverworld\riverworld", out var perfTag);

            Scenario scnr = GenerateScenario(mapId);
            ScenarioStructureBsp sbsp = GenerateStructureBsp(worldBounds);

            var lbsp = new ScenarioLightmapBspData();
            lbsp.BspIndex = 0;

            var sldt = new ScenarioLightmap();
            sldt.PerPixelLightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>() { new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = lbspTag } };

            scnr.Lightmap = sldtTag;
            scnr.StructureBsps = new List<StructureBspBlock>() {
                    new StructureBspBlock() {
                        StructureBsp = sbspTag,
                        Cubemap = cubemapsTag,
                        Wind = windTag,
                        Flags = (Scenario.StructureBspBlock.StructureBspFlags)32,
                        DefaultSkyIndex = -1
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

            if (type == WorldType.Water)
            {
                var waterWorldParams = new WaterWorldParameters()
                {
                    Shader = Cache.TagCache.GetTag(@"levels\multi\riverworld\shaders\riverworld_water_rough.shader_water"),
                    CellSize = 20,
                    Tesselation = 20,
                    Opacity = 0.9f,
                    Z = 0
                };
                var waterGeometry = GenerateWaterWorld(sbsp, waterWorldParams);
                lbsp.Geometry = waterGeometry;
                sbsp.Geometry = waterGeometry;
                // temp hack to ensure render geo is visible
                sbsp.CompatibilityFlags |= ScenarioStructureBsp.StructureBspCompatibilityValue.Reach;
            }

            Cache.Serialize(stream, cfxsTag, cfxs);
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
                AtmosphereIndex = -1,
                CameraFxIndex = -1,
                BackgroundSoundEnvironmentIndex = -1,
                AcousticsSoundClusterIndex = 0,
                Unknown3 = -1,
                Unknown4 = -1,
                Unknown5 = -1,
                RuntimeDecalStartIndex = -1,
                MeshIndex = 0,
            });
            sbsp.AcousticsSoundClusters = new List<ScenarioStructureBsp.StructureBspSoundClusterBlock>() {
                    new ScenarioStructureBsp.StructureBspSoundClusterBlock() {
                        PaletteIndex = -1,
                    }
                };
            sbsp.Physics.CollisionMoppCodes = new List<TagHkpMoppCode>() {
                    new TagHkpMoppCode() {
                        ArrayBase = new HkArrayBase() {
                            Size = 1,
                            CapacityAndFlags = 1 + HkArrayFlags.DONT_DEALLOCATE_FLAG
                        },
                        Data = new TagBlock<byte>(CacheAddressType.Definition, new List<byte> { 0 }) // nop
                    }
                };

            sbsp.Physics.MoppBoundsMin = new RealPoint3d(sbsp.WorldBoundsX.Lower, sbsp.WorldBoundsY.Lower, sbsp.WorldBoundsZ.Lower);
            sbsp.Physics.MoppBoundsMax = new RealPoint3d(sbsp.WorldBoundsX.Upper, sbsp.WorldBoundsY.Upper, sbsp.WorldBoundsZ.Upper);
            sbsp.Geometry = CreateEmptyRenderGeometry(Cache);
            sbsp.CollisionMaterials = new List<ScenarioStructureBsp.CollisionMaterial>() { new ScenarioStructureBsp.CollisionMaterial() { SeamMappingIndex = -1, RuntimeGlobalMaterialIndex = 0 } };
            sbsp.CollisionBspResource = Cache.ResourceCache.CreateStructureBspResource(CreateStructureBspTagResources(CreateEmptyCollisionGeometry(CacheAddressType.Data)));
            sbsp.PathfindingResource = Cache.ResourceCache.CreateStructureBspCacheFileResource(CreateStructureBspCacheFileTagResources());
            sbsp.UseResourceItems = 1; // use CollisionBspResource
            return sbsp;
        }

        private Scenario GenerateScenario(int mapId)
        {
            var scnr = new Scenario();
            scnr.MapType = ScenarioMapType.Multiplayer;
            scnr.CampaignId = -1;
            scnr.MapId = mapId;
            scnr.SoftSurfaces = new List<SoftSurfaceBlock>() { new SoftSurfaceBlock() { Padding0 = new byte[4] } };
            scnr.ZoneSets = new List<ZoneSet>() {
                        new ZoneSet() {
                        Name = Cache.StringTable.GetStringId("default"),
                        PvsIndex = 0,
                        Flags = ZoneSet.ZoneSetFlags.None,
                        Bsps = BspFlags.Bsp0,
                        AudibilityIndex = -1
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
                        StructureBspPvs = new List<ZoneSetPvsBlock.BspPvsBlock>() {
                           new ZoneSetPvsBlock.BspPvsBlock() {
                               ClusterPvs = new List<ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock>() {
                                   new ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock() {
                                       ClusterPvsBitVectors = new List<ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock>() {
                                           new ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock() {
                                               Bits = new List<ZoneSetPvsBlock.BitVectorDword>() {
                                                   new ZoneSetPvsBlock.BitVectorDword() { Bits = ZoneSetPvsBlock.BitVectorDword.DwordBits.Bit0 }
                                               }
                                           }
                                        }
                                    }
                                },
                               ClusterPvsDoorsClosed = new List<ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock>() {
                                   new ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock() {
                                       ClusterPvsBitVectors = new List<ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock>() {
                                           new ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock() {
                                               Bits = new List<ZoneSetPvsBlock.BitVectorDword>() {
                                                   new ZoneSetPvsBlock.BitVectorDword() { Bits = ZoneSetPvsBlock.BitVectorDword.DwordBits.Bit0 }
                                               }
                                           }
                                        }
                                    }
                                },
                               AttachedSkyIndices = new List<ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock>() {
                                   new ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock()  {
                                       SkyIndex = 0
                                   }
                               },
                               VisibleSkyIndices = new List<ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock>()  {
                                   new ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock() {
                                       SkyIndex = 0
                                   }
                               },
                               ClusterAudioBitvector = new List<ZoneSetPvsBlock.BitVectorDword>() {
                                   new ZoneSetPvsBlock.BitVectorDword() {
                                       Bits =  ZoneSetPvsBlock.BitVectorDword.DwordBits.None
                                   }
                               },
                               ClusterMappings = new List<ZoneSetPvsBlock.BspPvsBlock.BspSeamClusterMapping>() {
                                   new ZoneSetPvsBlock.BspPvsBlock.BspSeamClusterMapping() {
                                       Clusters = new List<ZoneSetPvsBlock.BspPvsBlock.BspSeamClusterMapping.ClusterReference>() {
                                           new ZoneSetPvsBlock.BspPvsBlock.BspSeamClusterMapping.ClusterReference() {
                                               ClusterIndex = 0
                                           }
                                       }
                                   }
                               }
                           },
                        },
                        PortaldeviceMapping = new List<ZoneSetPvsBlock.PortalDeviceMappingBlock>() {
                            new ZoneSetPvsBlock.PortalDeviceMappingBlock() {
                                DevicePortalAssociations = new List<ZoneSetPvsBlock.PortalDeviceMappingBlock.DevicePortalAssociation>(),
                                GamePortalToPortalMap = new List<ZoneSetPvsBlock.PortalDeviceMappingBlock.GamePortalToPortalMapping>()
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
            definition.Planes = new TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Data);
            definition.SurfacePlanes = new TagBlock<StructureSurface>(CacheAddressType.Data);
            definition.EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data) { new EdgeToSeamMapping() { SeamIndex = -1, SeamEdgeIndex = -1 } };
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
                Leaves = new TagBlock<TagTool.Geometry.BspCollisionGeometry.Leaf>(addressType),
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
            renderGeometry.SetResourceBuffers(resourceDef, false);

            return renderGeometry;
        }

        private void GenerateMapFile(Stream cacheStream, GameCache cache, CachedTag scenarioTag, string mapName, string mapDescription, string author)
        {
            var scenarioName = Path.GetFileName(scenarioTag.Name);
            var scnr = cache.Deserialize<Scenario>(cacheStream, scenarioTag);

            var mapBuilder = new MapFileBuilder(cache.Version);
            mapBuilder.MapName = mapName;
            mapBuilder.MapDescription = mapDescription;
            MapFile map = mapBuilder.Build(scenarioTag, scnr);
 
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

        class WaterWorldParameters
        {
            public CachedTag Shader;
            public float Tesselation;
            public float Opacity;
            public float CellSize;
            public float Z;
        }

        private RenderGeometry GenerateWaterWorld(ScenarioStructureBsp sbsp, WaterWorldParameters parameters)
        {
            sbsp.Materials = new List<RenderMaterial>() { new RenderMaterial() { RenderMethod = parameters.Shader } };
            sbsp.CollisionMaterials = new List<ScenarioStructureBsp.CollisionMaterial>()
            { 
                new ScenarioStructureBsp.CollisionMaterial()
                {
                    RenderMethod = parameters.Shader,
                    ConveyorSurfaceIndex = -1,
                    SeamMappingIndex = -1,
                    RuntimeGlobalMaterialIndex = 0
                }
            };

            float cellSize = parameters.CellSize;
            int xCells = (int)Math.Ceiling(sbsp.WorldBoundsX.Length / cellSize);
            int yCells = (int)Math.Ceiling(sbsp.WorldBoundsZ.Length / cellSize);
            GenerateGridMesh(xCells, yCells, cellSize, out WorldVertex[] worldVertices, out ushort[] indices);

            var origin = new RealPoint3d(-sbsp.WorldBoundsX.Length / 2, -sbsp.WorldBoundsZ.Length / 2, parameters.Z);
            foreach (var vertex in worldVertices)
                vertex.Position = new RealQuaternion(vertex.Position.I + origin.X, vertex.Position.J + origin.Y, vertex.Position.K + origin.Z);

            var worldWaterVertices = GenerateWorldWaterVertices(worldVertices, indices);
            var waterParams = GenerateWaterParams(indices, parameters.Tesselation, parameters.Opacity);

            var part = new Part()
            {
                MaterialIndex = 0,
                TransparentSortingIndex = -1,
                FirstIndex = 0,
                IndexCount = indices.Length,
                FlagsNew = Part.PartFlagsNew.IsWaterSurface

            };
            var mesh = new Mesh()
            {
                Type = VertexType.World,
                RigidNodeIndex = -1,
                Parts = new List<Part>() { part },
                Water = new List<Mesh.WaterBlock>() { new Mesh.WaterBlock() { Value = 0 } },
                VertexBufferIndices = new short[] { -1, -1, -1, -1, -1, -1, -1, -1 },
                IndexBufferIndices = new short[] { -1, -1 },
                IndexBufferType = PrimitiveType.TriangleList
            };

            var indexBuffer = new IndexBufferDefinition();
            var worldBuffer = new VertexBufferDefinition();
            var worldWaterBuffer = new VertexBufferDefinition();
            var waterParamsBuffer = new VertexBufferDefinition();

            WriteIndices(indexBuffer, indices, IndexBufferFormat.TriangleList);
            WriteWorldVertices(worldBuffer, worldVertices);
            WriteWorldWaterVertices(worldWaterBuffer, worldWaterVertices);
            WriteUnknown1BVertices(waterParamsBuffer, waterParams);

            var resourceDefinition = new RenderGeometryApiResourceDefinition();
            resourceDefinition.IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(CacheAddressType.Definition)
            {
                new D3DStructure<IndexBufferDefinition>() { AddressType = CacheAddressType.Definition, Definition = indexBuffer }
            };

            resourceDefinition.VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(CacheAddressType.Definition)
            {
                new D3DStructure<VertexBufferDefinition>() { AddressType = CacheAddressType.Definition, Definition = worldBuffer },
                new D3DStructure<VertexBufferDefinition>() { AddressType = CacheAddressType.Definition, Definition = worldWaterBuffer },
                new D3DStructure<VertexBufferDefinition>() { AddressType = CacheAddressType.Definition, Definition = waterParamsBuffer },
            };

            mesh.IndexBufferIndices[0] = 0;
            mesh.VertexBufferIndices[0] = 0;
            mesh.VertexBufferIndices[6] = 1;
            mesh.VertexBufferIndices[7] = 2;


            var geometry = new RenderGeometry();
            geometry.Meshes = new List<Mesh>() { mesh };
            geometry.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();
            geometry.SetResourceBuffers(resourceDefinition, false);
            geometry.Resource = Cache.ResourceCache.CreateRenderGeometryApiResource(resourceDefinition);

            return geometry;
        }

        private static WaterTesselatedParameters[] GenerateWaterParams(ushort[] indices, float tessellation, float opacity)
        {
            return indices.Select(x => new WaterTesselatedParameters() 
            {
                LocalInfo = new RealVector2d(tessellation, opacity)
            }).ToArray();
        }

        private static WorldWaterVertex[] GenerateWorldWaterVertices(WorldVertex[] worldVertices, ushort[] indices)
        {
            var worldWaterVertices = new List<WorldWaterVertex>();

            for (int i = 0; i < indices.Length; i++)
            {
                var vertex = worldVertices[indices[i]];
                var waterVertex = new WorldWaterVertex()
                {
                    Position = vertex.Position,
                    Binormal = vertex.Binormal,
                    Normal = vertex.Normal,
                    Tangent = vertex.Tangent,
                    Texcoord = vertex.Texcoord,
                    StaticPerPixel = new RealVector2d(0, 0)
                };

                worldWaterVertices.Add(waterVertex);
            }

            return worldWaterVertices.ToArray();
        }

        private void GenerateGridMesh(int xCells, int yCells, float cellSize, out WorldVertex[] outVertices, out ushort[] outIndices)
        {
            var vertices = new List<WorldVertex>();
            var indices = new List<ushort>();

            for (int x = 0; x < xCells; x++)
            {
                for (int y = 0; y < yCells; y++)
                {
                    indices.Add((ushort)(vertices.Count + 0));
                    indices.Add((ushort)(vertices.Count + 1));
                    indices.Add((ushort)(vertices.Count + 2));
                    indices.Add((ushort)(vertices.Count + 2));
                    indices.Add((ushort)(vertices.Count + 3));
                    indices.Add((ushort)(vertices.Count + 0));

                    vertices.Add(new WorldVertex()
                    {
                        Position = new RealQuaternion((x + 1) * cellSize, y * cellSize, 0),
                        Normal = new RealVector3d(0, 0, 1),
                        Binormal = new RealVector3d(1, 0, 0),
                        Tangent = new RealQuaternion(0, 1, 0),
                        Texcoord = new RealVector2d(x + 1, y + 1)
                    });
                    vertices.Add(new WorldVertex()
                    {
                        Position = new RealQuaternion((x + 1) * cellSize, (y + 1) * cellSize, 0),
                        Normal = new RealVector3d(0, 0, 1),
                        Binormal = new RealVector3d(1, 0, 0),
                        Tangent = new RealQuaternion(0, 1, 0),
                        Texcoord = new RealVector2d(x + 0, y + 1)
                    });
                    vertices.Add(new WorldVertex()
                    {
                        Position = new RealQuaternion(x * cellSize, (y + 1) * cellSize, 0),
                        Normal = new RealVector3d(0, 0, 1),
                        Binormal = new RealVector3d(1, 0, 0),
                        Tangent = new RealQuaternion(0, 1, 0),
                        Texcoord = new RealVector2d(x + 0, y + 0)
                    });
                    vertices.Add(new WorldVertex()
                    {
                        Position = new RealQuaternion(x * cellSize, y * cellSize, 0),
                        Normal = new RealVector3d(0, 0, 1),
                        Binormal = new RealVector3d(1, 0, 0),
                        Tangent = new RealQuaternion(0, 1, 0),
                        Texcoord = new RealVector2d(x + 1, y + 0)
                    });
                }
            }

            outVertices = vertices.ToArray();
            outIndices = indices.ToArray();
        }

        void WriteIndices(IndexBufferDefinition def, ushort[] indices, IndexBufferFormat format)
        {
            using (var outputStream = new MemoryStream())
            {
                var indexBufferStream = new IndexBufferStream(outputStream);
                indexBufferStream.WriteIndices(indices);
                def.Data = new TagData(outputStream.ToArray());
                def.Format = format;
            }
        }

        void WriteWorldVertices(VertexBufferDefinition def, WorldVertex[] vertices)
        {
            using (var outputStream = new MemoryStream())
            {
                var vertexBufferStream = VertexStreamFactory.Create(CacheVersion.HaloOnlineED, CachePlatform.Original, outputStream);
                foreach (var vertex in vertices)
                    vertexBufferStream.WriteWorldVertex(vertex);

                def.Data = new TagData(outputStream.ToArray());
                def.Count = vertices.Length;
                def.Format = VertexBufferFormat.World;
                def.VertexSize = 0x38;
            }
        }

        void WriteWorldWaterVertices(VertexBufferDefinition def, WorldWaterVertex[] vertices)
        {
            using (var outputStream = new MemoryStream())
            {
                var vertexBufferStream = VertexStreamFactory.Create(CacheVersion.HaloOnlineED, CachePlatform.Original, outputStream);
                foreach (var vertex in vertices)
                    vertexBufferStream.WriteWorldWaterVertex(vertex);

                def.Data = new TagData(outputStream.ToArray());
                def.Count = vertices.Length;
                def.Format = VertexBufferFormat.World;
                def.VertexSize = 0x38; // incorrect
            }
        }

        void WriteUnknown1BVertices(VertexBufferDefinition def, WaterTesselatedParameters[] vertices)
        {
            using (var outputStream = new MemoryStream())
            {
                var vertexBufferStream = VertexStreamFactory.Create(CacheVersion.HaloOnline106708, CachePlatform.Original, outputStream);

                foreach (var vertex in vertices)
                    vertexBufferStream.WriteWaterTesselatedParameters(vertex);

                def.Data = new TagData(outputStream.ToArray());
                def.Count = vertices.Length;
                def.Format = VertexBufferFormat.TesselatedWaterParameters;
                def.VertexSize = 0x24;
            }
        }
    }
}
