using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Geometry.Utils;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using static TagTool.Commands.Porting.Gen2.Gen2BspGeometryConverter;

namespace TagTool.Commands.Porting.Gen2
{
    partial class PortTagGen2Command : Command
    {
        List<List<Gen2BSPResourceMesh>> bspMeshes = new List<List<Gen2BSPResourceMesh>>();

        private void ConvertNetgameFlags(TagTool.Tags.Definitions.Gen2.Scenario rawgen2tag, Scenario newScenario)
        {
            if (newScenario.MapType == ScenarioMapType.Multiplayer)
            {
                // TODO make new tags that are equivalent to these but don't have a render model
                // TODO fix teleporters

                foreach (var NetgameFlags in rawgen2tag.NetgameFlags)
                {
                    Scenario.CrateInstance crate = new Scenario.CrateInstance();

                    CachedTag objectiveItem = null;
                    switch (NetgameFlags.Type)
                    {
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.OddballSpawn:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\oddball\oddball_ball_spawn_point", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.CtfFlagSpawn:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\ctf\ctf_flag_spawn_point", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.CtfFlagReturn:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\ctf\ctf_flag_return_area", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.AssaultBombSpawn:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\assault\assault_bomb_spawn_point", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.AssaultBombReturn:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\assault\assault_bomb_goal_area", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.TeleporterSrc:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\teleporter_sender\teleporter_sender", out objectiveItem);
                            break;
                        case TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetpointsBlock.TypeValue.TeleporterDest:
                            Cache.TagCache.TryGetTag<Crate>(@"objects\multi\teleporter_reciever\teleporter_reciever", out objectiveItem);
                            break;
                    }

                    if (objectiveItem == null)
                        continue;

                    crate.PaletteIndex = (short)GetOrAddCrateItem(newScenario, objectiveItem);
                    crate.Position = NetgameFlags.Position;
                    crate.Rotation.YawValue = NetgameFlags.Facing.Radians;

                    crate.Multiplayer = new Scenario.MultiplayerObjectProperties();
                    crate.Multiplayer.Team = (TagTool.Tags.Definitions.Common.MultiplayerTeamDesignator)NetgameFlags.TeamDesignator;
                    crate.ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Crate };
                    crate.Source = Scenario.ScenarioInstance.SourceValue.Editor;
                    crate.EditorFolder = -1;
                    crate.ParentId = new ScenarioObjectParentStruct() { NameIndex = -1 };
                    crate.UniqueHandle = new DatumHandle(0xffffffff);
                    crate.OriginBspIndex = -1;
                    crate.CanAttachToBspFlags = (ushort)(1u << 0);

                    // TODO: figure out how to handle symemtric and asymmetric placements (Flags in Netgame flags)

                    newScenario.Crates.Add(crate);
                }
            }
        }

        private int GetOrAddCrateItem(Scenario scnr, CachedTag crate)
        {
            int findIndex = scnr.CratePalette.FindIndex(c => c.Object == crate);
            if (findIndex != -1)
                return findIndex;
            else
            {
                scnr.CratePalette.Add(new Scenario.ScenarioPaletteEntry { Object = crate });
                return scnr.CratePalette.Count - 1;
            }
        }

        public TagStructure ConvertScenario(TagTool.Tags.Definitions.Gen2.Scenario gen2Tag, TagTool.Tags.Definitions.Gen2.Scenario rawgen2Tag, string scenarioPath
            , Stream cacheStream, Stream gen2CacheStream, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            Scenario newScenario = new Scenario();
            AutoConverter.InitTagBlocks(newScenario);

            //default values for now, pulled from valhalla
            Cache.TagCache.TryGetTag<Wind>(@"levels\multi\riverworld\wind_riverworld", out var windTag);
            Cache.TagCache.TryGetTag<Bitmap>(@"levels\multi\riverworld\riverworld_riverworld_cubemaps", out var cubemapsTag);
            Cache.TagCache.TryGetTag<CameraFxSettings>(@"levels\multi\riverworld\riverworld", out var cfxsTag);
            Cache.TagCache.TryGetTag<SkyAtmParameters>(@"levels\multi\riverworld\sky\riverworld", out var skyaTag);
            Cache.TagCache.TryGetTag<ChocolateMountainNew>(@"levels\multi\riverworld\riverworld", out var chmtTag);
            Cache.TagCache.TryGetTag<PerformanceThrottles>(@"levels\multi\riverworld\riverworld", out var perfTag);

            newScenario.DefaultCameraFx = cfxsTag;
            newScenario.SkyParameters = skyaTag;
            newScenario.GlobalLighting = chmtTag;
            newScenario.PerformanceThrottles = perfTag;

            //mapid and type
            newScenario.MapType = (ScenarioMapType)gen2Tag.Type;
            switch (newScenario.MapType)
            {
                case ScenarioMapType.Multiplayer:
                    newScenario.MapId = gen2Tag.LevelData[0].Multiplayer[0].MapId;
                    newScenario.CampaignId = -1;
                    break;
                case ScenarioMapType.SinglePlayer:
                    newScenario.MapId = gen2Tag.LevelData[0].CampaignLevelData[0].MapId;
                    newScenario.CampaignId = gen2Tag.LevelData[0].CampaignLevelData[0].CampaignId;
                    break;
            }


            // Starting Profiles
            AutoConverter.TranslateList(gen2Tag.PlayerStartingProfile, newScenario.PlayerStartingProfile);

            //soft surfaces
            newScenario.SoftSurfaces = new List<Scenario.SoftSurfaceBlock> { new Scenario.SoftSurfaceBlock() };


            for (var i = 0; i < gen2Tag.StructureBsps.Count; i++)
            {
                newScenario.ZoneSets.Add(new Scenario.ZoneSet
                {
                    Name = Cache.StringTable.GetStringId("default"),
                    AudibilityIndex = -1
                });

                ScenarioStructureBsp currentbsp = Cache.Deserialize<ScenarioStructureBsp>(cacheStream, gen2Tag.StructureBsps[i].StructureBsp);

                //bsps
                newScenario.StructureBsps.Add(new Scenario.StructureBspBlock
                {
                    StructureBsp = gen2Tag.StructureBsps[i].StructureBsp,
                    Flags = 32,
                    DefaultSkyIndex = -1,
                    Cubemap = cubemapsTag,
                    Wind = windTag
                });

                //zoneset pvs
                newScenario.ZoneSetPvs.Add(new Scenario.ZoneSetPvsBlock());
                newScenario.ZoneSetPvs[i].StructureBspMask = (Scenario.BspFlags)(((int)newScenario.ZoneSetPvs[0].StructureBspMask) | (1 << i));
                newScenario.ZoneSetPvs[i].StructureBspPvs = new List<Scenario.ZoneSetPvsBlock.BspPvsBlock>();
                newScenario.ZoneSetPvs[i].StructureBspPvs.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock());
                AutoConverter.InitTagBlocks(newScenario.ZoneSetPvs[i].StructureBspPvs[0]);

                int pvsbits = 0;
                for (var k = 0; k < currentbsp.Clusters.Count; k++)
                {
                    pvsbits |= 1 << k;
                }

                for (var j = 0; j < currentbsp.Clusters.Count; j++)
                {
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].ClusterPvs.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock
                    {
                        ClusterPvsBitVectors = new List<Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock>
                        {
                            new Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock
                            {
                                Bits = new List<Scenario.ZoneSetPvsBlock.BitVectorDword>
                                {
                                    new Scenario.ZoneSetPvsBlock.BitVectorDword
                                    {
                                        Bits = (Scenario.ZoneSetPvsBlock.BitVectorDword.DwordBits)pvsbits
                                    }
                                }
                            }
                        }
                    });
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].ClusterPvsDoorsClosed.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock
                    {
                        ClusterPvsBitVectors = new List<Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock>
                        {
                            new Scenario.ZoneSetPvsBlock.BspPvsBlock.ClusterPvsBlock.CluserPvsBitVectorBlock
                            {
                                Bits = new List<Scenario.ZoneSetPvsBlock.BitVectorDword>
                                {
                                    new Scenario.ZoneSetPvsBlock.BitVectorDword
                                    {
                                        Bits = (Scenario.ZoneSetPvsBlock.BitVectorDword.DwordBits)pvsbits
                                    }
                                }
                            }
                        }
                    });
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].AttachedSkyIndices.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock());
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].VisibleSkyIndices.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock.SkyIndicesBlock());
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].ClusterMappings.Add(new Scenario.ZoneSetPvsBlock.BspPvsBlock.BspSeamClusterMapping());
                    newScenario.ZoneSetPvs[i].StructureBspPvs[0].ClusterAudioBitvector.Add(new Scenario.ZoneSetPvsBlock.BitVectorDword());
                }

                //zonesets
                newScenario.ZoneSets[i].Bsps = (Scenario.BspFlags)(1 << i);
                newScenario.ZoneSetPvs[i].PortaldeviceMapping = new List<Scenario.ZoneSetPvsBlock.PortalDeviceMappingBlock>
                {
                    new Scenario.ZoneSetPvsBlock.PortalDeviceMappingBlock()
                };
            }

            //skies
            int bspbits = 0;
            for (var b = 0; b < gen2Tag.StructureBsps.Count; b++)
            {
                bspbits |= 1 << b;
            }
            foreach (var gen2sky in rawgen2Tag.Skies)
            {
                if (gen2sky.Sky != null)
                {
                    string skytagname = gen2sky.Sky.Name;

                    var gen2skytag = Gen2Cache.Deserialize<TagTool.Tags.Definitions.Gen2.Sky>(gen2CacheStream, gen2sky.Sky);
                    CachedTag skymodetag = null;
                    if (gen2skytag.RenderModel != null)
                    {
                        skymodetag = ConvertTag(cacheStream, gen2CacheStream, resourceStreams, gen2skytag.RenderModel);

                        //fixup skymodetag with gen2 sky render model scale
                        RenderModel skymode = Cache.Deserialize<RenderModel>(cacheStream, skymodetag);

                        foreach (var comp in skymode.Geometry.Compression)
                        {
                            float size_x = comp.X.Upper - comp.X.Lower;
                            float size_y = comp.Y.Upper - comp.Y.Lower;
                            float size_z = comp.Z.Upper - comp.Z.Lower;
                            comp.X.Upper = (comp.X.Upper - (size_x / 2)) + (size_x / 2) * gen2skytag.RenderModelScale;
                            comp.X.Lower = (comp.X.Lower + (size_x / 2)) - (size_x / 2) * gen2skytag.RenderModelScale;
                            comp.Y.Upper = (comp.Y.Upper - (size_y / 2)) + (size_y / 2) * gen2skytag.RenderModelScale;
                            comp.Y.Lower = (comp.Y.Lower + (size_y / 2)) - (size_y / 2) * gen2skytag.RenderModelScale;
                            comp.Z.Upper = (comp.Z.Upper - (size_z / 2)) + (size_z / 2) * gen2skytag.RenderModelScale;
                            comp.Z.Lower = (comp.Z.Lower + (size_z / 2)) - (size_z / 2) * gen2skytag.RenderModelScale;
                        };
                        Cache.Serialize(cacheStream, skymodetag, skymode);
                    }

                    var newmodel = new Model
                    {
                        RenderModel = skymodetag
                    };
                    CachedTag newmodeltag = Cache.TagCache.AllocateTag<Model>($"{skytagname}");
                    Cache.Serialize(cacheStream, newmodeltag, newmodel);
                    var newscen = new Scenery
                    {
                        BoundingRadius = 5555.0f,
                        ObjectType = new GameObjectType16 { Halo3ODST = GameObjectTypeHalo3ODST.Scenery },
                        Model = newmodeltag
                    };
                    CachedTag newscentag = Cache.TagCache.AllocateTag<Scenery>($"{skytagname}");
                    Cache.Serialize(cacheStream, newscentag, newscen);
                    newScenario.SkyReferences.Add(new Scenario.SkyReference
                    {
                        SkyObject = newscentag,
                        NameIndex = -1,
                        ActiveBsps = (Scenario.BspShortFlags)bspbits
                    });
                }
            }

            ConvertScenarioPlacements(gen2Tag, rawgen2Tag, newScenario, gen2CacheStream, cacheStream, resourceStreams);

            newScenario.Lightmap = ConvertLightmap(rawgen2Tag, newScenario, scenarioPath, cacheStream, gen2CacheStream);

            return newScenario;
        }

        public TagStructure ConvertStructureBSP(TagTool.Tags.Definitions.Gen2.ScenarioStructureBsp gen2Tag)
        {
            ScenarioStructureBsp newSbsp = new ScenarioStructureBsp();
            newSbsp.UseResourceItems = 1; // use CollisionBspResource
            newSbsp.ImportVersion = 7;

            AutoConverter.InitTagBlocks(newSbsp);

            //materials
            foreach (var material in gen2Tag.Materials)
            {
                newSbsp.Materials.Add(new RenderMaterial
                {
                    RenderMethod = material.Shader == null ? Cache.TagCache.GetTag(@"shaders\invalid.shader") : material.Shader
                });
            }

            //collision materials
            foreach (var material in gen2Tag.CollisionMaterials)
            {
                newSbsp.CollisionMaterials.Add(new ScenarioStructureBsp.CollisionMaterial
                {
                    RenderMethod = material.NewShader == null ? Cache.TagCache.GetTag(@"shaders\invalid.shader") : material.NewShader
                });
            }

            //RENDER GEO RESOURCE
            //begin building render geo resource
            var builder = new RenderModelBuilder(Cache);
            builder.BeginRegion(StringId.Invalid);
            builder.BeginPermutation(StringId.Invalid);

            //COLLISION RESOURCE
            //create new collisionresource and populate values from tag
            StructureBspTagResources CollisionResource = new StructureBspTagResources();

            //main collision geometry
            CollisionResource.CollisionBsps = new TagBlock<CollisionGeometry>(CacheAddressType.Definition);

            int collisionEdgeCount = 0;
            foreach (var bsp in gen2Tag.CollisionBsp)
            {
                var newBsp = ConvertCollisionGeometry(bsp);
                newBsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
                newBsp.Planes.AddressType = CacheAddressType.Data;
                newBsp.Leaves.AddressType = CacheAddressType.Data;
                newBsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
                newBsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
                newBsp.Surfaces.AddressType = CacheAddressType.Data;
                newBsp.Edges.AddressType = CacheAddressType.Data;
                newBsp.Vertices.AddressType = CacheAddressType.Data;
                collisionEdgeCount = newBsp.Edges.Count;
                CollisionResource.CollisionBsps.Add(newBsp);
            }

            //structure physics
            newSbsp.Physics = new ScenarioStructureBsp.StructurePhysics
            {
                MoppBoundsMin = gen2Tag.StructurePhysics.MoppBoundsMin,
                MoppBoundsMax = gen2Tag.StructurePhysics.MoppBoundsMax
            };

            byte[] moppdata = gen2Tag.StructurePhysics.MoppCode;
            newSbsp.Physics.CollisionMoppCodes = ConvertH2MOPP(moppdata);

            //world bounds
            newSbsp.WorldBoundsX = gen2Tag.WorldBoundsX;
            newSbsp.WorldBoundsY = gen2Tag.WorldBoundsY;
            newSbsp.WorldBoundsZ = gen2Tag.WorldBoundsZ;

            //leaves
            foreach (var leaf in gen2Tag.Leaves)
            {
                newSbsp.Leaves.Add(new ScenarioStructureBsp.Leaf
                {
                    ClusterNew = (byte)leaf.Cluster
                });
            };

            //transparent planes
            foreach (var plane in gen2Tag.TransparentPlanes)
            {
                newSbsp.TransparentPlanes.Add(new ScenarioStructureBsp.TransparentPlane
                {
                    MeshIndex = plane.SectionIndex,
                    PartIndex = plane.PartIndex,
                    Plane = plane.Plane
                });
            }

            //acoustic sound clusters (needed to prevent crash)
            newSbsp.AcousticsSoundClusters = new List<ScenarioStructureBsp.StructureBspSoundClusterBlock>() {
                    new ScenarioStructureBsp.StructureBspSoundClusterBlock() {
                        PaletteIndex = -1,
                    }
                };

            //cluster portals
            foreach (var portal in gen2Tag.ClusterPortals)
            {
                var newportal = new ScenarioStructureBsp.ClusterPortal
                {
                    BackCluster = portal.BackCluster,
                    FrontCluster = portal.FrontCluster,
                    PlaneIndex = portal.PlaneIndex,
                    Centroid = portal.Centroid,
                    BoundingRadius = portal.BoundingRadius,
                    Flags = (ScenarioStructureBsp.ClusterPortal.FlagsValue)portal.Flags,
                    Vertices = new List<ScenarioStructureBsp.ClusterPortal.Vertex>()
                };
                foreach (var vertex in portal.Vertices)
                {
                    newportal.Vertices.Add(new ScenarioStructureBsp.ClusterPortal.Vertex
                    {
                        Position = vertex.Point
                    });
                }
                newSbsp.ClusterPortals.Add(newportal);
            }

            List<Gen2BSPResourceMesh> Gen2Meshes = new List<Gen2BSPResourceMesh>();

            //cluster data
            foreach (var cluster in gen2Tag.Clusters)
            {
                List<Gen2BSPResourceMesh> clustermeshes = new List<Gen2BSPResourceMesh>();
                if (cluster.SectionInfo.TotalVertexCount > 0)
                {
                    //render geometry
                    var compressor = new VertexCompressor(
                        cluster.SectionInfo.Compression.Count > 0 ?
                            cluster.SectionInfo.Compression[0] :
                            new RenderGeometryCompression
                            {
                                X = new Bounds<float>(0.0f, 1.0f),
                                Y = new Bounds<float>(0.0f, 1.0f),
                                Z = new Bounds<float>(0.0f, 1.0f),
                                U = new Bounds<float>(0.0f, 1.0f),
                                V = new Bounds<float>(0.0f, 1.0f),
                                U2 = new Bounds<float>(0.0f, 1.0f),
                                V2 = new Bounds<float>(0.0f, 1.0f),
                            });
                    clustermeshes = ReadResourceMeshes(Gen2Cache, cluster.GeometryBlockInfo,
                    cluster.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)cluster.SectionInfo.GeometryCompressionFlags,
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)cluster.SectionInfo.SectionLightingFlags, compressor);

                    if (clustermeshes.Count > 1)
                    {
                        new TagToolWarning("cluster had >1 render mesh! Culling extras...");
                        clustermeshes = new List<Gen2BSPResourceMesh> { clustermeshes.First() };
                    }
                }

                int newmeshindex = -1;
                if (clustermeshes.Count > 0)
                {
                    BuildMeshes(builder, clustermeshes, (RenderGeometryClassification)cluster.SectionInfo.GeometryClassification,
                        cluster.SectionInfo.OpaqueMaxNodesVertex, 0);

                    //fixup mesh part fields
                    var newmesh = builder.Meshes.Last();
                    for (var i = 0; i < newmesh.Mesh.Parts.Count; i++)
                    {
                        newmesh.Mesh.Parts[i].FirstSubPartIndex = clustermeshes[0].Parts[i].FirstSubPartIndex;
                        newmesh.Mesh.Parts[i].SubPartCount = clustermeshes[0].Parts[i].SubPartCount;
                        newmesh.Mesh.Parts[i].TypeNew = (Part.PartTypeNew)clustermeshes[0].Parts[i].TypeOld;
                    }
                    Gen2Meshes.AddRange(clustermeshes);
                    newmeshindex = Gen2Meshes.Count - 1;
                }

                //block values
                var newcluster = new ScenarioStructureBsp.Cluster
                {
                    //mesh that was just built
                    MeshIndex = (short)newmeshindex,
                    BoundsX = cluster.BoundsX,
                    BoundsY = cluster.BoundsY,
                    BoundsZ = cluster.BoundsZ,
                    AtmosphereIndex = -1,
                    CameraFxIndex = -1,
                    BackgroundSoundEnvironmentIndex = -1,
                    AcousticsSoundClusterIndex = 0,
                    Unknown3 = -1,
                    Unknown4 = -1,
                    Unknown5 = -1,
                    RuntimeDecalStartIndex = -1,
                    Portals = new List<ScenarioStructureBsp.Cluster.Portal>(),
                    InstancedGeometryPhysics = new ScenarioStructureBsp.Cluster.InstancedGeometryPhysicsData
                    {
                        ClusterIndex = gen2Tag.Clusters.IndexOf(cluster),
                        MoppCodes = cluster.CollisionMoppCode.Length > 0 ? ConvertH2MOPP(cluster.CollisionMoppCode) : null
                    }
                };

                //cluster portal indices
                foreach (var portal in cluster.Portals)
                {
                    newcluster.Portals.Add(new ScenarioStructureBsp.Cluster.Portal
                    {
                        PortalIndex = portal.PortalIndex
                    });
                };

                newSbsp.Clusters.Add(newcluster);
            }

            //instanced geometry definitions
            CollisionResource.InstancedGeometry = new TagBlock<InstancedGeometryBlock>(CacheAddressType.Definition);
            foreach (var instanced in gen2Tag.InstancedGeometriesDefinitions)
            {
                List<Gen2BSPResourceMesh> instancemeshes = new List<Gen2BSPResourceMesh>();
                if (instanced.RenderInfo.SectionInfo.TotalVertexCount > 0)
                {
                    //render geometry
                    var compressor = new VertexCompressor(
                        instanced.RenderInfo.SectionInfo.Compression.Count > 0 ?
                            instanced.RenderInfo.SectionInfo.Compression[0] :
                            new RenderGeometryCompression
                            {
                                X = new Bounds<float>(0.0f, 1.0f),
                                Y = new Bounds<float>(0.0f, 1.0f),
                                Z = new Bounds<float>(0.0f, 1.0f),
                                U = new Bounds<float>(0.0f, 1.0f),
                                V = new Bounds<float>(0.0f, 1.0f),
                                U2 = new Bounds<float>(0.0f, 1.0f),
                                V2 = new Bounds<float>(0.0f, 1.0f),
                            });
                    instancemeshes = ReadResourceMeshes(Gen2Cache, instanced.RenderInfo.GeometryBlockInfo,
                    instanced.RenderInfo.SectionInfo.TotalVertexCount, (RenderGeometryCompressionFlags)instanced.RenderInfo.SectionInfo.GeometryCompressionFlags,
                    (TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags)instanced.RenderInfo.SectionInfo.SectionLightingFlags, compressor);

                    if (instancemeshes.Count > 1)
                    {
                        new TagToolWarning("instance had >1 render mesh! Culling extras...");
                        instancemeshes = new List<Gen2BSPResourceMesh> { instancemeshes.First() };
                    }
                }

                int newmeshindex = -1;
                if (instancemeshes.Count > 0)
                {
                    BuildMeshes(builder, instancemeshes, (RenderGeometryClassification)instanced.RenderInfo.SectionInfo.GeometryClassification,
                    instanced.RenderInfo.SectionInfo.OpaqueMaxNodesVertex, 0);

                    //fixup mesh part fields
                    var newmesh = builder.Meshes.Last();
                    for (var i = 0; i < newmesh.Mesh.Parts.Count; i++)
                    {
                        newmesh.Mesh.Parts[i].FirstSubPartIndex = instancemeshes[0].Parts[i].FirstSubPartIndex;
                        newmesh.Mesh.Parts[i].SubPartCount = instancemeshes[0].Parts[i].SubPartCount;
                        newmesh.Mesh.Parts[i].TypeNew = (Part.PartTypeNew)instancemeshes[0].Parts[i].TypeOld;
                    }
                    Gen2Meshes.AddRange(instancemeshes);
                    newmeshindex = Gen2Meshes.Count - 1;
                }

                //block values
                var newinstance = new InstancedGeometryBlock
                {
                    Checksum = instanced.Checksum,
                    BoundingSphereOffset = instanced.BoundingSphereCenter,
                    BoundingSphereRadius = instanced.BoundingSphereRadius,
                    //index of mesh just builts
                    MeshIndex = (short)newmeshindex,
                };

                var bsp = ConvertCollisionGeometry(instanced.CollisionInfo);
                bsp.Bsp3dNodes.AddressType = CacheAddressType.Data;
                bsp.Planes.AddressType = CacheAddressType.Data;
                bsp.Leaves.AddressType = CacheAddressType.Data;
                bsp.Bsp2dReferences.AddressType = CacheAddressType.Data;
                bsp.Bsp2dNodes.AddressType = CacheAddressType.Data;
                bsp.Surfaces.AddressType = CacheAddressType.Data;
                bsp.Edges.AddressType = CacheAddressType.Data;
                bsp.Vertices.AddressType = CacheAddressType.Data;
                newinstance.CollisionInfo = bsp;

                //build mopp codes from collision info and add
                if (instanced.BspPhysics != null && instanced.BspPhysics.Count > 0)
                {
                    var mopps = ConvertH2MOPP(instanced.BspPhysics[0].MoppCodeData);
                    newinstance.CollisionMoppCodes = new TagBlock<TagHkpMoppCode>(CacheAddressType.Definition, mopps);
                }

                CollisionResource.InstancedGeometry.Add(newinstance);
            }

            //instanced geometry instances
            foreach (var instanced in gen2Tag.InstancedGeometryInstances)
            {
                var newinstance = new InstancedGeometryInstance
                {
                    Scale = instanced.Scale,
                    Matrix = new RealMatrix4x3(instanced.Forward.I, instanced.Forward.J, instanced.Forward.K,
                    instanced.Left.I, instanced.Left.J, instanced.Left.K,
                    instanced.Up.I, instanced.Up.J, instanced.Up.K,
                    instanced.Position.X, instanced.Position.Y, instanced.Position.Z),
                    DefinitionIndex = instanced.InstanceDefinition,
                    LightmapTexcoordBlockIndex = -1,
                    Name = instanced.Name,
                    WorldBoundingSphereCenter = instanced.WorldBoundingSphereCenter,
                    BoundingSphereRadiusBounds = new Bounds<float>(instanced.BoundingSphereRadius, instanced.BoundingSphereRadius),
                    PathfindingPolicy = (Scenery.PathfindingPolicyValue)instanced.PathfindingPolicy,
                    LightmappingPolicy = ((int)instanced.LightmappingPolicy) == 0 ? 
                    InstancedGeometryInstance.InstancedGeometryLightmappingPolicy.PerPixelShared :
                    InstancedGeometryInstance.InstancedGeometryLightmappingPolicy.PerVertex,
                    LightmapResolutionScale = 1.0f
                };

                //make sure there is a bsp physics block in the instance def
                var instancedef = gen2Tag.InstancedGeometriesDefinitions[instanced.InstanceDefinition];
                if (instancedef.BspPhysics != null && instancedef.BspPhysics.Count > 0)
                {
                    newinstance.Flags |= InstancedGeometryInstance.InstancedGeometryFlags.Collidable;
                    newinstance.BspPhysics = new List<InstancedGeometryPhysics>
                    {
                        new InstancedGeometryPhysics
                        {
                            MoppBvTreeShape = new CMoppBvTreeShape
                            {
                                Scale = 1.0f,
                                Type = 27
                            },
                            GeometryShape = new CollisionGeometryShape
                            {
                                AABB_Center = instancedef.BspPhysics[0].AABB_Center,
                                AABB_Half_Extents = instancedef.BspPhysics[0].AABB_Half_Extents,
                                Scale = 1.0f
                            }
                        }
                    };
                }

                newSbsp.InstancedGeometryInstances.Add(newinstance);
            }

            //close out render geo resource
            builder.EndPermutation();
            builder.EndRegion();
            RenderModel meshbuild = builder.Build(Cache.Serializer);

            //create empty pathfinding resource
            var pathfindingresource = new StructureBspCacheFileTagResources();
            pathfindingresource.Planes = new TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Data);
            pathfindingresource.SurfacePlanes = new TagBlock<StructureSurface>(CacheAddressType.Data);
            pathfindingresource.EdgeToSeams = new TagBlock<EdgeToSeamMapping>(CacheAddressType.Data);
            for (int i = 0; i < collisionEdgeCount; i++)
                pathfindingresource.EdgeToSeams.Add(new EdgeToSeamMapping() { SeamIndex = -1, SeamEdgeIndex = -1 });

            pathfindingresource.PathfindingData = new TagBlock<Pathfinding.ResourcePathfinding>(CacheAddressType.Data);

            //write pathfinding resource
            newSbsp.PathfindingResource = Cache.ResourceCache.CreateStructureBspCacheFileResource(pathfindingresource);

            //write meshes and render model resource
            newSbsp.Geometry = meshbuild.Geometry;

            //add empty meshes for clusters and instances with no mesh
            foreach (var cluster in newSbsp.Clusters)
            {
                if (cluster.MeshIndex == -1)
                {
                    Gen2Meshes.Add(new Gen2BSPResourceMesh());
                    newSbsp.Geometry.Meshes.Add(new Mesh()
                    {
                        Type = VertexType.World,
                        RigidNodeIndex = -1,
                        VertexBufferIndices = new short[8] { -1, -1, -1, -1, -1, -1, -1, -1 },
                        IndexBufferIndices = new short[2] { -1, -1 },
                    });
                    cluster.MeshIndex = (short)(newSbsp.Geometry.Meshes.Count - 1);
                }
            }
            foreach (var instance in CollisionResource.InstancedGeometry)
            {
                if (instance.MeshIndex == -1)
                {
                    Gen2Meshes.Add(new Gen2BSPResourceMesh());
                    newSbsp.Geometry.Meshes.Add(new Mesh()
                    {
                        Type = VertexType.World,
                        RigidNodeIndex = -1,
                        VertexBufferIndices = new short[8] { -1, -1, -1, -1, -1, -1, -1, -1 },
                        IndexBufferIndices = new short[2] { -1, -1 },
                    });
                    instance.MeshIndex = (short)(newSbsp.Geometry.Meshes.Count - 1);
                }
            }

            //write collision resource
            newSbsp.CollisionBspResource = Cache.ResourceCache.CreateStructureBspResource(CollisionResource);

            //fixup per mesh visibility mopp
            newSbsp.Geometry.MeshClusterVisibility = new List<RenderGeometry.PerMeshMoppBlock>();
            newSbsp.Geometry.PerMeshSubpartVisibility = new List<RenderGeometry.PerMeshSubpartVisibilityBlock>();
            for (var i = 0; i < Gen2Meshes.Count; i++)
            {
                Gen2BSPResourceMesh gen2mesh = Gen2Meshes[i];
                if (gen2mesh.VisibilityMoppCodeData == null || gen2mesh.VisibilityBounds == null)
                    continue;
                //mesh visibility mopp and mopp reorder table
                if (gen2mesh.VisibilityMoppCodeData.Length > 0 && gen2mesh.MoppReorderTable != null)
                {
                    newSbsp.Geometry.MeshClusterVisibility.Add(new RenderGeometry.PerMeshMoppBlock
                    {
                        MoppCode = ConvertH2MoppData(gen2mesh.VisibilityMoppCodeData),
                        MoppReorderTable = gen2mesh.MoppReorderTable.Select(m => m.Index).ToList()
                    });
                }
                //visibility bounds (approximate conversion)
                for (var j = 0; j < gen2mesh.VisibilityBounds.Count; j++)
                {
                    newSbsp.Geometry.PerMeshSubpartVisibility.Add(new RenderGeometry.PerMeshSubpartVisibilityBlock
                    {
                        BoundingSpheres = new List<RenderGeometry.BoundingSphere> { new RenderGeometry.BoundingSphere
                    {
                        Position = gen2mesh.VisibilityBounds[j].Position,
                        Radius = gen2mesh.VisibilityBounds[j].Radius,
                        NodeIndices = new sbyte[]{ (sbyte)gen2mesh.VisibilityBounds[j].NodeIndex, 0, 0, 0}
                    } }
                    });
                }

            }

            bspMeshes.Add(Gen2Meshes);

            InstanceBucketGenerator.Generate(newSbsp, CollisionResource);
            ConvertGen2EnvironmentMopp(newSbsp);
            return newSbsp;
        }

        public void ConvertScenarioPlacements(TagTool.Tags.Definitions.Gen2.Scenario gen2Tag, TagTool.Tags.Definitions.Gen2.Scenario rawgen2tag,
            Scenario newScenario, Stream gen2CacheStream, Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            // Object names
            foreach (var objname in gen2Tag.ObjectNames)
            {
                newScenario.ObjectNames.Add(new Scenario.ObjectName {
                    Name = objname.Name,
                    ObjectType = objname.ObjectType,
                    PlacementIndex = objname.PlacementIndex
                });
            }

            // Device Groups
            foreach (var devgroup in gen2Tag.DeviceGroups)
            {
                newScenario.DeviceGroups.Add(new Scenario.DeviceGroup
                {
                    Name = devgroup.Name,
                    InitialValue = devgroup.InitialValue,
                    Flags = (Scenario.DeviceGroupFlags)devgroup.Flags
                });
            }

            // Device machines
            foreach (var macpal in gen2Tag.MachinePalette)
            {
                newScenario.MachinePalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = macpal.Name
                });
            }
            for (var machobjindex = 0; machobjindex < gen2Tag.Machines.Count; machobjindex++)
            {
                var machobj = gen2Tag.Machines[machobjindex];
                newScenario.Machines.Add(new Scenario.MachineInstance
                {
                    PaletteIndex = machobj.Type,
                    NameIndex = machobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)machobj.ObjectData.PlacementFlags,
                    Position = machobj.ObjectData.Position,
                    Rotation = machobj.ObjectData.Rotation,
                    Scale = machobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)machobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)machobj.ObjectData.ManualBspFlags,

                    CanAttachToBspFlags = (ushort)(machobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.ScenarioInstance.SourceValue)machobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)machobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Machine },
                    //extra device/machine flags
                    DeviceFlags = (Scenario.ScenarioDeviceFlags)machobj.DeviceData.Flags,
                    MachineFlags = (Scenario.MachineInstance.ScenarioMachineFlags)machobj.MachineData.Flags,
                    PowerGroup = machobj.DeviceData.PowerGroup,
                    PositionGroup = machobj.DeviceData.PositionGroup
                });
            }

            // Device controls
            foreach (var ctrlpal in gen2Tag.ControlPalette)
            {
                newScenario.ControlPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = ctrlpal.Name
                });
            }
            for (var ctrlobjindex = 0; ctrlobjindex < gen2Tag.Controls.Count; ctrlobjindex++)
            {
                var ctrlobj = gen2Tag.Controls[ctrlobjindex];
                newScenario.Controls.Add(new Scenario.ControlInstance
                {
                    PaletteIndex = ctrlobj.Type,
                    NameIndex = ctrlobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)ctrlobj.ObjectData.PlacementFlags,
                    Position = ctrlobj.ObjectData.Position,
                    Rotation = ctrlobj.ObjectData.Rotation,
                    Scale = ctrlobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)ctrlobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)ctrlobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(ctrlobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.ScenarioInstance.SourceValue)ctrlobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)ctrlobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Control },
                    ControlFlags = (Scenario.ControlInstance.ScenarioControlFlags)ctrlobj.ControlData.Flags,
                    DeviceFlags = (Scenario.ScenarioDeviceFlags)ctrlobj.DeviceData.Flags,
                    PowerGroup = ctrlobj.DeviceData.PowerGroup,
                    PositionGroup = ctrlobj.DeviceData.PositionGroup
                });
            }

            // Crates
            foreach (var blocpal in gen2Tag.CratesPalette)
            {
                newScenario.CratePalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = blocpal.Name
                });
            }
            for (var blocobjindex = 0; blocobjindex < gen2Tag.Crates.Count; blocobjindex++)
            {
                var crateobj = gen2Tag.Crates[blocobjindex];
                newScenario.Crates.Add(new Scenario.CrateInstance
                {
                    PaletteIndex = crateobj.Type,
                    NameIndex = crateobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)crateobj.ObjectData.PlacementFlags,
                    Position = crateobj.ObjectData.Position,
                    Rotation = crateobj.ObjectData.Rotation,
                    Scale = crateobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)crateobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)crateobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(crateobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.ScenarioInstance.SourceValue)crateobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)crateobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Crate },
                });
            }

            // Scenery
            foreach (var scenpal in gen2Tag.SceneryPalette)
            {
                newScenario.SceneryPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = scenpal.Name
                });
            }
            for (var scenobjindex = 0; scenobjindex < gen2Tag.Scenery.Count; scenobjindex++)
            {
                var scenobj = gen2Tag.Scenery[scenobjindex];
                var scenery = new Scenario.SceneryInstance
                {
                    PaletteIndex = scenobj.Type,
                    NameIndex = scenobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)scenobj.ObjectData.PlacementFlags,
                    Position = scenobj.ObjectData.Position,
                    Rotation = scenobj.ObjectData.Rotation,
                    Scale = scenobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)scenobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)scenobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(scenobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.ScenarioInstance.SourceValue)scenobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)scenobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Scenery },
                    Multiplayer = new Scenario.MultiplayerObjectProperties(),
                };
                newScenario.Scenery.Add(scenery);
                AutoConverter.TranslateEnum(gen2Tag.Scenery[scenobjindex].SceneryData.ValidMultiplayerGames, out scenery.Multiplayer.EngineFlags, scenery.Multiplayer.EngineFlags.GetType());
            }

            // Bipeds
            foreach (var bipdpal in gen2Tag.BipedPalette)
            {
                newScenario.BipedPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = bipdpal.Name
                });
            }
            for (var bipdobjindex = 0; bipdobjindex < gen2Tag.Bipeds.Count; bipdobjindex++)
            {
                var bipdobj = gen2Tag.Bipeds[bipdobjindex];
                var biped = new Scenario.BipedInstance
                {
                    PaletteIndex = bipdobj.Type,
                    NameIndex = bipdobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)bipdobj.ObjectData.PlacementFlags,
                    Position = bipdobj.ObjectData.Position,
                    Rotation = bipdobj.ObjectData.Rotation,
                    Scale = bipdobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)bipdobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)bipdobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(bipdobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.BipedInstance.SourceValue)bipdobj.ObjectData.ObjectId.Source,
                    
                    UniqueHandle = new DatumHandle((uint)bipdobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Biped },
                    Variant = bipdobj.PermutationData.VariantName,
                    ActiveChangeColors = (Scenario.BipedInstance.ScenarioObjectActiveChangeColorFlags)bipdobj.PermutationData.ActiveChangeColors,

                    BodyVitalityFraction = bipdobj.UnitData.BodyVitality,
                    Flags = (Scenario.BipedInstance.ScenarioUnitDatumFlags)bipdobj.UnitData.Flags
                };
                newScenario.Bipeds.Add(biped);
            }

            // Weapons
            foreach (var weappal in gen2Tag.WeaponPalette)
            {
                newScenario.WeaponPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = weappal.Name
                });
            }
            for (var weapobjindex = 0; weapobjindex < gen2Tag.Weapons.Count; weapobjindex++)
            {
                var weapobj = gen2Tag.Weapons[weapobjindex];
                var weapon = new Scenario.WeaponInstance
                {
                    PaletteIndex = weapobj.Type,
                    NameIndex = weapobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)weapobj.ObjectData.PlacementFlags,
                    Position = weapobj.ObjectData.Position,
                    Rotation = weapobj.ObjectData.Rotation,
                    Scale = weapobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)weapobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)weapobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(weapobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.BipedInstance.SourceValue)weapobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)weapobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Weapon },
                    WeaponFlags = (Scenario.WeaponInstance.ScenarioWeaponDatumFlags)weapobj.WeaponData.Flags,
                };
                newScenario.Weapons.Add(weapon);
            }

            // Equipment
            foreach (var eqippal in gen2Tag.EquipmentPalette)
            {
                newScenario.EquipmentPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = eqippal.Name
                });
            }
            for (var eqipobjindex = 0; eqipobjindex < gen2Tag.Equipment.Count; eqipobjindex++)
            {
                var eqipobj = gen2Tag.Equipment[eqipobjindex];
                var equipment = new Scenario.EquipmentInstance
                {
                    PaletteIndex = eqipobj.Type,
                    NameIndex = eqipobj.Name,
                    PlacementFlags = (Scenario.ObjectPlacementFlags)eqipobj.ObjectData.PlacementFlags,
                    Position = eqipobj.ObjectData.Position,
                    Rotation = eqipobj.ObjectData.Rotation,
                    Scale = eqipobj.ObjectData.Scale,
                    BspPolicy = (Scenario.ScenarioInstance.BspPolicyValue)eqipobj.ObjectData.BspPolicy,
                    OriginBspIndex = (short)eqipobj.ObjectData.ManualBspFlags,
                    CanAttachToBspFlags = (ushort)(eqipobj.ObjectData.ManualBspFlags + 1),
                    Source = (Scenario.BipedInstance.SourceValue)eqipobj.ObjectData.ObjectId.Source,
                    UniqueHandle = new DatumHandle((uint)eqipobj.ObjectData.ObjectId.UniqueId),
                    EditorFolder = -1,
                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Equipment },
                };
                newScenario.Equipment.Add(equipment);
            }

            // Player starting locations
            foreach (var startlocation in gen2Tag.PlayerStartingLocations)
            {
                newScenario.PlayerStartingLocations.Add(new Scenario.PlayerStartingLocation
                {
                    Position = startlocation.Position,
                    Facing = new RealEulerAngles2d(startlocation.Facing, Angle.FromDegrees(0.0f)),
                    EditorFolderIndex = -1
                });
            }

            // Spawn points from starting locations
            if (newScenario.MapType == ScenarioMapType.Multiplayer || newScenario.MapType == ScenarioMapType.SinglePlayer)
            {
                newScenario.SceneryPalette.Add(new Scenario.ScenarioPaletteEntry
                {
                    Object = Cache.TagCache.GetTag<Scenery>(@"objects\multi\spawning\respawn_point_invisible")
                });
                bool prematchcameraset = false;
                foreach (var startlocation in newScenario.PlayerStartingLocations)
                {
                    newScenario.Scenery.Add(new Scenario.SceneryInstance
                    {
                        PaletteIndex = (short)(newScenario.SceneryPalette.Count - 1),
                        NameIndex = -1,
                        Position = startlocation.Position,
                        Rotation = new RealEulerAngles3d(startlocation.Facing.Yaw, Angle.FromDegrees(0.0f), Angle.FromDegrees(0.0f)),
                        ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Scenery },
                        Source = Scenario.ScenarioInstance.SourceValue.Editor,
                        EditorFolder = -1,
                        ParentId = new ScenarioObjectParentStruct() { NameIndex = -1 },
                        UniqueHandle = new DatumHandle(0xffffffff),
                        OriginBspIndex = -1,
                        CanAttachToBspFlags = (ushort)(1u << 0),
                        Multiplayer = new Scenario.MultiplayerObjectProperties() { Team = TagTool.Tags.Definitions.Common.MultiplayerTeamDesignator.Neutral },
                    });
                    if (!prematchcameraset)
                    {
                        newScenario.CutsceneCameraPoints.Add(new Scenario.CutsceneCameraPoint()
                        {
                            Position = startlocation.Position,
                            Orientation = new RealEulerAngles3d(startlocation.Facing.Yaw, Angle.FromDegrees(0.0f), Angle.FromDegrees(0.0f)),
                            Flags = Scenario.CutsceneCameraPointFlags.PrematchCameraHack,
                            Name = "prematch_camera",
                        });
                        prematchcameraset = true;
                    }
                }
            }

            //Spawn Zones
            AutoConverter.TranslateList(gen2Tag.SpawnData, newScenario.SpawnData);

            // Item Collection -> Weapon/Vehicle Placement
            byte uniqueid = 0;
            CachedTag paletteTag;
            TagTool.Tags.Definitions.Gen2.ItemCollection itemlayout = null;
            TagTool.Tags.Definitions.Gen2.VehicleCollection vehilayout = null;
            foreach (var NetgameEquipment in rawgen2tag.NetgameEquipment)
            {
                TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags EngineFlags = 0;
                ConvertH2GametypeFlags(ref EngineFlags, NetgameEquipment);

                if (NetgameEquipment.ItemVehicleCollection != null)
                {
                    object itemdef = Gen2Cache.Deserialize(gen2CacheStream, NetgameEquipment.ItemVehicleCollection);

                    // Port tags referenced in item/vehicle collection
                    ConvertData(cacheStream, gen2CacheStream, resourceStreams, null, null, NetgameEquipment.ItemVehicleCollection);

                    switch (NetgameEquipment.ItemVehicleCollection.Group.ToString())
                    {
                        case "vehc":
                            vehilayout = (TagTool.Tags.Definitions.Gen2.VehicleCollection)itemdef;
                            if (vehilayout.VehiclePermutations[0].Vehicle != null)
                            {
                                if (!Cache.TagCache.TryGetTag<Vehicle>(vehilayout.VehiclePermutations[0].Vehicle.Name, out paletteTag))
                                    break;
                                var palette_index = newScenario.VehiclePalette.FindIndex(v => (v.Object == null ? "" : v.Object.Name) == vehilayout.VehiclePermutations[0].Vehicle.Name);
                                if (palette_index == -1)
                                {
                                    newScenario.VehiclePalette.Add(new Scenario.ScenarioPaletteEntry
                                    {
                                        Object = paletteTag
                                    });
                                    palette_index = newScenario.VehiclePalette.Count - 1;
                                }
                                newScenario.Vehicles.Add(new Scenario.VehicleInstance
                                {
                                    PaletteIndex = (short)palette_index,
                                    NameIndex = -1,
                                    PlacementFlags = Scenario.ObjectPlacementFlags.None,
                                    Position = NetgameEquipment.Position,
                                    Rotation = NetgameEquipment.Orientation.Orientation,
                                    Scale = 1,
                                    BspPolicy = Scenario.ScenarioInstance.BspPolicyValue.Default,
                                    OriginBspIndex = 1,
                                    CanAttachToBspFlags = 1,
                                    Source = Scenario.ScenarioInstance.SourceValue.Editor,
                                    UniqueHandle = new DatumHandle(uniqueid),
                                    EditorFolder = -1,
                                    ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Vehicle },
                                    Multiplayer = new Scenario.MultiplayerObjectProperties
                                    {
                                        SpawnTime = NetgameEquipment.SpawnTimeInSeconds0Default,
                                        AbandonTime = NetgameEquipment.RespawnOnEmptyTime,
                                        EngineFlags = EngineFlags
                                    }
                                });
                            }
                            break;
                        default:
                            itemlayout = (TagTool.Tags.Definitions.Gen2.ItemCollection)itemdef;
                            if (itemlayout.ItemPermutations[0].Item != null)
                            {
                                if (itemlayout.ItemPermutations[0].Item.Group.ToString().Equals("weap"))
                                {
                                    // Convert weapon flags
                                    var WeaponFlags = Scenario.WeaponInstance.ScenarioWeaponDatumFlags.DoesAcceleratemovesDueToExplosions;
                                    if (NetgameEquipment.Flags.HasFlag(TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.FlagsValue.Levitate))
                                    {
                                        WeaponFlags |= Scenario.WeaponInstance.ScenarioWeaponDatumFlags.InitiallyAtRestdoesntFall;
                                    }
                                    if (!Cache.TagCache.TryGetTag<Weapon>(itemlayout.ItemPermutations[0].Item.Name, out paletteTag))
                                        break;
                                    var palette_index = newScenario.WeaponPalette.FindIndex(v => (v.Object == null ? "" : v.Object.Name) ==
                                    itemlayout.ItemPermutations[0].Item.Name);
                                    if (palette_index == -1)
                                    {
                                        newScenario.WeaponPalette.Add(new Scenario.ScenarioPaletteEntry
                                        {
                                            Object = paletteTag
                                        });
                                        palette_index = newScenario.WeaponPalette.Count - 1;
                                    }
                                    newScenario.Weapons.Add(new Scenario.WeaponInstance
                                    {
                                        PaletteIndex = (short)palette_index,
                                        NameIndex = -1,
                                        PlacementFlags = Scenario.ObjectPlacementFlags.None,
                                        Position = NetgameEquipment.Position,
                                        Rotation = NetgameEquipment.Orientation.Orientation,
                                        Scale = 1,
                                        BspPolicy = Scenario.ScenarioInstance.BspPolicyValue.Default,
                                        OriginBspIndex = 1,
                                        CanAttachToBspFlags = 1,
                                        Source = Scenario.ScenarioInstance.SourceValue.Editor,
                                        UniqueHandle = new DatumHandle(uniqueid),
                                        EditorFolder = -1,
                                        ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Weapon },
                                        WeaponFlags = WeaponFlags,
                                        Multiplayer = new Scenario.MultiplayerObjectProperties
                                        {
                                            SpawnTime = NetgameEquipment.SpawnTimeInSeconds0Default,
                                            AbandonTime = NetgameEquipment.RespawnOnEmptyTime,
                                            EngineFlags = EngineFlags
                                        }
                                    });
                                }
                                else
                                {
                                    if (!Cache.TagCache.TryGetTag<Equipment>(itemlayout.ItemPermutations[0].Item.Name, out paletteTag))
                                        break;
                                    var palette_index = newScenario.EquipmentPalette.FindIndex(v => (v.Object == null ? "" : v.Object.Name) ==
                                    itemlayout.ItemPermutations[0].Item.Name);
                                    if (palette_index == -1)
                                    {
                                        newScenario.EquipmentPalette.Add(new Scenario.ScenarioPaletteEntry
                                        {
                                            Object = paletteTag
                                        });
                                        palette_index = newScenario.EquipmentPalette.Count - 1;
                                    }
                                    newScenario.Equipment.Add(new Scenario.EquipmentInstance
                                    {
                                        PaletteIndex = (short)palette_index,
                                        NameIndex = -1,
                                        PlacementFlags = Scenario.ObjectPlacementFlags.None,
                                        Position = NetgameEquipment.Position,
                                        Rotation = NetgameEquipment.Orientation.Orientation,
                                        Scale = 1,
                                        BspPolicy = Scenario.ScenarioInstance.BspPolicyValue.Default,
                                        OriginBspIndex = 1,
                                        CanAttachToBspFlags = 1,
                                        Source = Scenario.ScenarioInstance.SourceValue.Editor,
                                        UniqueHandle = new DatumHandle(uniqueid),
                                        EditorFolder = -1,
                                        ObjectType = new GameObjectType8 { Halo3ODST = GameObjectTypeHalo3ODST.Equipment },
                                        Multiplayer = new Scenario.MultiplayerObjectProperties
                                        {
                                            SpawnTime = NetgameEquipment.SpawnTimeInSeconds0Default,
                                            AbandonTime = NetgameEquipment.RespawnOnEmptyTime,
                                            EngineFlags = EngineFlags
                                        }
                                    });
                                }
                            }
                            break;
                    }
                }
                uniqueid += 101;
            }



            ConvertNetgameFlags(rawgen2tag, newScenario);

            // Trigger Volumes
            foreach (var vol in gen2Tag.KillTriggerVolumes)
            {
                newScenario.TriggerVolumes.Add(new Scenario.TriggerVolume
                {
                    Name = vol.Name,
                    ObjectName = vol.ObjectName,
                    NodeName = vol.NodeName,
                    Position = vol.Position,
                    Forward = vol.Forward,
                    Up = vol.Up,
                    Extents = vol.Extents,
                    KillVolume = vol.KillTriggerVolume
                });
            }

            // Bsp Switch -> ZoneSet Switch
            foreach (var switchvol in gen2Tag.BspSwitchTriggerVolumes)
            {
                newScenario.ZonesetSwitchTriggerVolumes.Add(new Scenario.ZoneSetSwitchTriggerVolume {
                    Flags = Scenario.ZoneSetSwitchTriggerVolume.FlagBits.TeleportVehicles,
                    BeginZoneSet = -1,
                    TriggerVolume = switchvol.TriggerVolume,
                    CommitZoneSet = switchvol.Destination
                });
            }

            // Kill Trigger Volumes
            foreach (var killvol in gen2Tag.ScenarioKillTriggers)
            {
                newScenario.ScenarioKillTriggers.Add(new Scenario.ScenarioKillTrigger
                {
                    TriggerVolume = killvol.TriggerVolume
                });
            }
        }

        public List<TagHkpMoppCode> ConvertH2MOPP(byte[] moppdata)
        {
            var result = new List<TagHkpMoppCode>();

            using (var moppStream = new MemoryStream(moppdata))
            using (var moppReader = new EndianReader(moppStream, Gen2Cache.Endianness))
            {
                var context = new DataSerializationContext(moppReader);
                var deserializer = new TagDeserializer(Gen2Cache.Version, Gen2Cache.Platform);
                while (!moppReader.EOF)
                {
                    long startOffset = moppReader.Position;
                    Havok.Gen2.MoppCodeHeader moppHeader = deserializer.Deserialize<Havok.Gen2.MoppCodeHeader>(context);
                    byte[] moppCode = moppReader.ReadBytes((int)(moppHeader.Size - 0x30));
                    moppReader.SeekTo((startOffset + moppHeader.Size) + 0xF & ~0xF);

                    result.Add(new TagHkpMoppCode
                    {
                        Info = new CodeInfo
                        {
                            Offset = moppHeader.Offset
                        },
                        ArrayBase = new HkArrayBase
                        {
                            Size = (uint)moppCode.Length,
                            CapacityAndFlags = (uint)(moppCode.Length | 0x80000000)
                        },
                        Data = new TagBlock<byte>(CacheAddressType.Data)
                        {
                            Elements = moppCode.ToList()
                        }
                    });
                }
            }

            return result;
        }

        public byte[] ConvertH2MoppData(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;

            byte[] result;
            using (var inputReader = new EndianReader(new MemoryStream(data), CacheVersionDetection.IsLittleEndian(Gen2Cache.Version, Gen2Cache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
            using (var outputStream = new MemoryStream())
            using (var outputWriter = new EndianWriter(outputStream, CacheVersionDetection.IsLittleEndian(Cache.Version, Cache.Platform) ? EndianFormat.LittleEndian : EndianFormat.BigEndian))
            {
                var dataContext = new DataSerializationContext(inputReader, outputWriter);
                var deserializer = new TagDeserializer(Gen2Cache.Version, Gen2Cache.Platform);
                var serializer = new TagSerializer(Cache.Version, Cache.Platform);
                while (!inputReader.EOF)
                {
                    var header = deserializer.Deserialize<Havok.Gen2.MoppCodeHeader>(dataContext);
                    var dataSize = header.Size - 0x30;
                    var nextOffset = (inputReader.Position + dataSize) + 0xf & ~0xf;


                    List<byte> moppCodes = new List<byte>();
                    for (int j = 0; j < dataSize; j++)
                    {
                        moppCodes.Add(inputReader.ReadByte());
                    }
                    inputReader.SeekTo(nextOffset);

                    var newHeader = new HkpMoppCode
                    {
                        Info = new CodeInfo
                        {
                            Offset = header.Offset
                        },
                        ArrayBase = new HkArrayBase
                        {
                            Size = (uint)moppCodes.Count,
                            CapacityAndFlags = (uint)(moppCodes.Count | 0x80000000)
                        }
                    };

                    serializer.Serialize(dataContext, newHeader);
                    for (int j = 0; j < moppCodes.Count; j++)
                        outputWriter.Write(moppCodes[j]);

                    StreamUtil.Align(outputStream, 0x10);
                }
                result = outputStream.ToArray();
            }
            return result;
        }

        void ConvertGen2EnvironmentMopp(ScenarioStructureBsp sbsp)
        {
            if (sbsp.Physics.CollisionMoppCodes.Count == 0)
                return;

            var data = sbsp.Physics.CollisionMoppCodes[0].Data;
            for (int i = 0; i < data.Count; i++)
            {
                switch (data[i])
                {
                    case 0x00: // HK_MOPP_RETURN
                        break;
                    case 0x05: // HK_MOPP_JUMP8
                    case 0x09: // HK_MOPP_TERM_REOFFSET8
                    case 0x50: // HK_MOPP_TERM8
                    case 0x54: // HK_MOPP_NTERM_8
                    case 0x60: // HK_MOPP_PROPERTY8_0
                    case 0x61: // HK_MOPP_PROPERTY8_1
                    case 0x62: // HK_MOPP_PROPERTY8_2
                    case 0x63: // HK_MOPP_PROPERTY8_3
                        i += 1;
                        break;
                    case 0x06: // HK_MOPP_JUMP16
                    case 0x0A: // HK_MOPP_TERM_REOFFSET16
                    case 0x0C: // HK_MOPP_JUMP_CHUNK
                    case 0x20: // HK_MOPP_SINGLE_SPLIT_X
                    case 0x21: // HK_MOPP_SINGLE_SPLIT_Y
                    case 0x22: // HK_MOPP_SINGLE_SPLIT_Z
                    case 0x26: // HK_MOPP_DOUBLE_CUT_X
                    case 0x27: // HK_MOPP_DOUBLE_CUT_Y
                    case 0x28: // HK_MOPP_DOUBLE_CUT_Z
                    case 0x51: // HK_MOPP_TERM16
                    case 0x55: // HK_MOPP_NTERM_16
                    case 0x64: // HK_MOPP_PROPERTY16_0
                    case 0x65: // HK_MOPP_PROPERTY16_1
                    case 0x66: // HK_MOPP_PROPERTY16_2
                    case 0x67: // HK_MOPP_PROPERTY16_3
                        i += 2;
                        break;
                    case 0x01: // HK_MOPP_SCALE1
                    case 0x02: // HK_MOPP_SCALE2
                    case 0x03: // HK_MOPP_SCALE3
                    case 0x04: // HK_MOPP_SCALE4
                    case 0x07: // HK_MOPP_JUMP24
                    case 0x10: // HK_MOPP_SPLIT_X
                    case 0x11: // HK_MOPP_SPLIT_Y
                    case 0x12: // HK_MOPP_SPLIT_Z
                    case 0x13: // HK_MOPP_SPLIT_YZ
                    case 0x14: // HK_MOPP_SPLIT_YMZ
                    case 0x15: // HK_MOPP_SPLIT_XZ
                    case 0x16: // HK_MOPP_SPLIT_XMZ
                    case 0x17: // HK_MOPP_SPLIT_XY
                    case 0x18: // HK_MOPP_SPLIT_XMY
                    case 0x19: // HK_MOPP_SPLIT_XYZ
                    case 0x1A: // HK_MOPP_SPLIT_XYMZ
                    case 0x1B: // HK_MOPP_SPLIT_XMYZ
                    case 0x1C: // HK_MOPP_SPLIT_XMYMZ
                    case 0x52: // HK_MOPP_TERM24
                    case 0x56: // HK_MOPP_NTERM_24
                        i += 3;
                        break;
                    case 0x57: // HK_MOPP_NTERM_32
                    case 0x68: // HK_MOPP_PROPERTY32_0
                    case 0x69: // HK_MOPP_PROPERTY32_1
                    case 0x6A: // HK_MOPP_PROPERTY32_2
                    case 0x6B: // HK_MOPP_PROPERTY32_3
                        i += 4;
                        break;
                    case 0x0D: // HK_MOPP_DATA_OFFSET
                        i += 5;
                        break;
                    case 0x23: // HK_MOPP_SPLIT_JUMP_X
                    case 0x24: // HK_MOPP_SPLIT_JUMP_Y
                    case 0x25: // HK_MOPP_SPLIT_JUMP_Z
                    case 0x29: // HK_MOPP_DOUBLE_CUT24_X
                    case 0x2A: // HK_MOPP_DOUBLE_CUT24_Y
                    case 0x2B: // HK_MOPP_DOUBLE_CUT24_Z
                        i += 6;
                        break;
                    case byte op when op >= 0x30 && op <= 0x4F:
                        break;
                    case 0x0B: // HK_MOPP_TERM_REOFFSET32
                    case 0x53: // HK_MOPP_TERM32
                        int key = data[i + 4] + ((data[i + 3] + ((data[i + 2] + (data[i + 1] << 8)) << 8)) << 8);
                        key = ConvertShapeKey(key);
                        data[i + 1] = (byte)((key & 0x7F000000) >> 24);
                        data[i + 2] = (byte)((key & 0x00FF0000) >> 16);
                        data[i + 3] = (byte)((key & 0x0000FF00) >> 8);
                        data[i + 4] = (byte)(key & 0x000000FF);
                        i += 4;
                        break;
                    default:
                        throw new NotSupportedException($"Opcode 0x{data[i]:X2}");
                }
            }

            int ConvertShapeKey(int key)
            {
                int type = key >> 29;
                if (type == 1)
                    key |= (5 << 26); // needed to pass group filter
                return key;
            }
        }

        void ConvertH2GametypeFlags(ref TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags EngineFlags,
            TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock NetgameEquipment)
        {
            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.CaptureTheFlag
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.CaptureTheFlag
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.CaptureTheFlag
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.CaptureTheFlag)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.CaptureTheFlag;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Assault;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.Slayer
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.Slayer
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.Slayer
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.Slayer)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Infection;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Slayer;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Vip;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.Oddball
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.Oddball
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.Oddball
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.Oddball)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Oddball;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.KingOfTheHill
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.KingOfTheHill
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.KingOfTheHill
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.KingOfTheHill)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.KingOfTheHill;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.Juggernaut
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.Juggernaut
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.Juggernaut
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.Juggernaut)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Juggernaut;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.Territories
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.Territories
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.Territories
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.Territories)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Territories;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.AllExceptCtf
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.AllExceptCtf
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.AllExceptCtf
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.AllExceptCtf)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Infection;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Slayer;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Vip;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Oddball;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.KingOfTheHill;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Juggernaut;
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.Territories;
            }

            if (NetgameEquipment.GameType1 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType1Value.AllGameTypes
                || NetgameEquipment.GameType2 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType2Value.AllGameTypes
                || NetgameEquipment.GameType3 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType3Value.AllGameTypes
                || NetgameEquipment.GameType4 == TagTool.Tags.Definitions.Gen2.Scenario.ScenarioNetgameEquipmentBlock.GameType4Value.AllGameTypes)
            {
                EngineFlags |= TagTool.Tags.Definitions.Common.GameEngineSubTypeFlags.All;
            }
        }
    }
}
