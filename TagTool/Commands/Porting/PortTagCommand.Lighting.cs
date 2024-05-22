using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;
using TagTool.Geometry;
using TagTool.Lighting;
using TagTool.Havok;
using TagTool.Commands.Common;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private LensFlare ConvertLensFlare(LensFlare lensFlare, Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            lensFlare.OcclusionReflectionIndex = 0;

            // TO DO: verify reach conversions

            foreach (var reflection in lensFlare.Reflections)
            {
                // only H3Original and ODST have Rotation after Axis Offset
                // ODST: counterclockwise rotation starting from alt axis
                if (BlamCache.Version == CacheVersion.Halo3ODST)
                    reflection.RotationOffset = -(reflection.RotationOffsetH3 - 90);
                // retail H3: clockwise rotation from alt axis
                else if (BlamCache.Version == CacheVersion.Halo3Retail && BlamCache.Platform == CachePlatform.Original)
                {
                    if (BlamCache.Platform == CachePlatform.Original)
                        reflection.RotationOffset = reflection.RotationOffsetH3;

                    reflection.RotationOffset += 90;
                }

                // only H3MCC and HO have tint modulation factor after tint color
                if (!((BlamCache.Version == CacheVersion.Halo3Retail && BlamCache.Platform == CachePlatform.MCC)
                    || (BlamCache.Version > CacheVersion.HaloOnlineED && BlamCache.Version <= CacheVersion.HaloOnline700123)))
                {
                    reflection.TintModulationFactor = reflection.TintModulationFactorGen3;
                }

                reflection.UnknownHO = 0;
                //reflection.BitmapOverride = null; // only H3MCC and HO have this cachedtag

                // not necessary to convert HO reflection functions
                if (BlamCache.Version >= CacheVersion.HaloOnlineED && BlamCache.Version < CacheVersion.HaloReach)
                    continue;
                // not necessary to convert H3MCC reflection functions
                if (BlamCache.Platform == CachePlatform.MCC && BlamCache.Version == CacheVersion.Halo3Retail)
                {
                    continue;
                }

                var radius = new byte[52];
                var scale = new byte[52];
                var brightness = new byte[52];

                //
                // Radius
                //

                using (var writer = new EndianWriter(new MemoryStream(radius), EndianFormat.LittleEndian))
                {
                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x08340000);
                    writer.Format = EndianFormat.LittleEndian;
                    byte[] value1 = BitConverter.GetBytes(reflection.RadiusBounds.Lower);
                    byte[] value2 = BitConverter.GetBytes(reflection.RadiusBounds.Upper);
                    writer.Write(value1);
                    writer.Write(value2);
                    writer.Write(0x0);
                    writer.Write(0x0);

                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x0);
                    writer.Write(0x0);
                    writer.Write(0x14000000);
                    writer.Write(0x01000000);

                    writer.Write(0x040000CD);
                    writer.Write(0xFFFF7F7F);
                    writer.Write(0x0000803F);
                    writer.Write(0x0);
                }
                reflection.RadiusCurveFunction = new TagFunction
                {
                    Data = radius,
                };

                //
                // Brightness
                //

                using (var writer = new EndianWriter(new MemoryStream(brightness), EndianFormat.LittleEndian))
                {
                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x08340000);
                    writer.Format = EndianFormat.LittleEndian;
                    byte[] value1 = BitConverter.GetBytes(reflection.BrightnessBounds.Lower);
                    byte[] value2 = BitConverter.GetBytes(reflection.BrightnessBounds.Upper);
                    writer.Write(value1);
                    writer.Write(value2);
                    writer.Write(0x0);
                    writer.Write(0x0);

                    writer.Format = EndianFormat.BigEndian;
                    writer.Write(0x0);
                    writer.Write(0x0);
                    writer.Write(0x14000000);
                    writer.Write(0x01000000);

                    writer.Write(0x040000CD);
                    writer.Write(0xFFFF7F7F);
                    writer.Write(0x0000803F);
                    writer.Write(0x0);
                }

                reflection.BrightnessCurveFunction = new TagFunction
                {
                    Data = brightness,
                };

                //
                // Scale
                //

                using (EndianWriter function = new EndianWriter(new MemoryStream(scale), EndianFormat.BigEndian))
                {
                    function.Write(0x08340000);
                    function.Write(0x0);
                    function.Write(0x0000803F);
                    function.Write(0x0);
                    function.Write(0x0);

                    function.Write(0x0);
                    function.Write(0x0);
                    function.Write(0x14000000);
                    function.Write(0x01000000);

                    function.Write(0x040000CD);
                    function.Write(0xFFFF7F7F);
                    function.Write(0x0000803F);
                    function.Write(0x0);
                }

                reflection.ScaleCurveXFunction = new TagFunction
                {
                    Data = scale,
                };

                reflection.ScaleCurveYFunction = new TagFunction
                {
                    Data = scale,
                };
            }

            return lensFlare;
        }

        public ScenarioLightmap ConvertScenarioLightmap(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, string blamTagName, ScenarioLightmap scenarioLightmap)
        {  
            if (BlamCache.Version > CacheVersion.Halo3Retail || BlamCache.Platform == CachePlatform.MCC)
                return scenarioLightmap;

            scenarioLightmap.PerPixelLightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>();

            for (int i = 0; i < scenarioLightmap.Lightmaps.Count; i++)
            {
                var entry = scenarioLightmap.Lightmaps[i];

                var wasReplacing = FlagIsSet(PortingFlags.Replace);

                RemoveFlags(PortingFlags.Replace);
                var Lbsp = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, entry, scenarioLightmap, blamTagName);
                Lbsp = ConvertScenarioLightmapBspData(Lbsp);
                if (wasReplacing)
                    SetFlags(PortingFlags.Replace);

                Lbsp.Airprobes = new List<Airprobe>(scenarioLightmap.Airprobes);
                Lbsp.SceneryLightProbes = new List<SceneryLightProbe>(scenarioLightmap.SceneryLightProbes);
                Lbsp.MachineLightProbes = new List<MachineLightProbes>(scenarioLightmap.MachineLightProbes);

                var groupTag = CacheContext.TagCache.TagDefinitions.GetTagGroupFromTag("Lbsp");

                CachedTag edTag;

                if (FlagIsSet(PortingFlags.Replace) && CacheContext.TagCache.TryGetCachedTag((scenarioLightmap.Lightmaps.Count != 1 ? $"{blamTagName}_{i}_data" : $"{blamTagName}_data")+".Lbsp", out CachedTag result))
                {
                    CacheContext.Serialize(cacheStream, result, Lbsp);
                    edTag = result;
                }
                else
                {
                    edTag = CacheContext.TagCacheGenHO.AllocateTag(groupTag);

                    if (scenarioLightmap.Lightmaps.Count != 1)
                        edTag.Name = $"{blamTagName}_{i}_data";
                    else
                        edTag.Name = $"{blamTagName}_data";

                    CacheContext.Serialize(cacheStream, edTag, Lbsp);
                }
                scenarioLightmap.PerPixelLightmapDataReferences.Add(new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = edTag });
            }

            scenarioLightmap.Airprobes.Clear();
            scenarioLightmap.SceneryLightProbes.Clear();
            scenarioLightmap.MachineLightProbes.Clear();

            return scenarioLightmap;
        }


        private ScenarioLightmapBspData ConvertScenarioLightmapBspData(ScenarioLightmapBspData Lbsp)
        {
            var lightmapResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);

            if (lightmapResourceDefinition == null)
                return Lbsp;

            var converter = new RenderGeometryConverter(CacheContext, BlamCache);
            var newLightmapResourceDefinition = converter.Convert(Lbsp.Geometry, lightmapResourceDefinition);

            //
            // convert vertex buffers and add them to the new resource
            //

            if (Lbsp.StaticPerVertexLightingBuffers != null)
            {
                foreach (var staticPerVertexLighting in Lbsp.StaticPerVertexLightingBuffers)
                {
                    if (staticPerVertexLighting.VertexBufferIndex != -1)
                    {
                        if (staticPerVertexLighting.VertexBufferIndex >= lightmapResourceDefinition.VertexBuffers.Count)
                        {
                            new TagToolWarning("Invalid per vertex lighting buffer index!");
                            staticPerVertexLighting.VertexBufferIndex = -1;
                            continue;
                        }

                        staticPerVertexLighting.VertexBuffer = lightmapResourceDefinition.VertexBuffers[staticPerVertexLighting.VertexBufferIndex].Definition;
                        VertexBufferConverter.ConvertVertexBuffer(BlamCache.Version, BlamCache.Platform, CacheContext.Version, CacheContext.Platform, staticPerVertexLighting.VertexBuffer);
                        var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                        d3dPointer.Definition = staticPerVertexLighting.VertexBuffer;
                        newLightmapResourceDefinition.VertexBuffers.Add(d3dPointer);
                        // set the new buffer index
                        staticPerVertexLighting.VertexBufferIndex = (short)(newLightmapResourceDefinition.VertexBuffers.Elements.Count - 1);
                    }
                }
            }

            Lbsp.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newLightmapResourceDefinition);

            return Lbsp;
        }


        private ScenarioLightmap ConvertReachLightmap(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, string blamTagName, ScenarioLightmap scenarioLightmap)
        {
            for (int i = 0; i < scenarioLightmap.PerPixelLightmapDataReferences.Count; i++)
            {
                var lbspTag = scenarioLightmap.PerPixelLightmapDataReferences[i].LightmapBspData;
                if (lbspTag == null)
                    continue;

                Console.WriteLine($"Converting lightmap bsp: {i + 1}/{ scenarioLightmap.PerPixelLightmapDataReferences.Count}");

                var Lbsp = BlamCache.Deserialize<ScenarioLightmapBspData>(blamCacheStream, lbspTag);
                Lbsp.BspIndex = (short)i;
                Lbsp = ConvertScenarioLightmapBspDataReach(cacheStream, blamCacheStream, resourceStreams, lbspTag.Name, lbspTag, Lbsp);

                CachedTag edTag = CacheContext.TagCacheGenHO.AllocateTag<ScenarioLightmapBspData>(scenarioLightmap.PerPixelLightmapDataReferences[i].LightmapBspData.Name);
                CacheContext.Serialize(cacheStream, edTag, Lbsp);
            }

            return ConvertStructure(cacheStream, blamCacheStream, resourceStreams, scenarioLightmap, scenarioLightmap, blamTagName);
        }

        private ScenarioLightmapBspData ConvertScenarioLightmapBspDataReach(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, string blamTagName, CachedTag LbspTag, ScenarioLightmapBspData Lbsp)
        {
            var lightmapResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);

            if (lightmapResourceDefinition == null)
                return Lbsp;

            //
            // cconvert lightprobe textures
            //

            CachedLightmap convertedLightmap = null;
            if (Lbsp.LightmapSHCoefficientsBitmap != null)
            {
                var lightmapCachePath = !string.IsNullOrEmpty(PortingOptions.Current.ReachLightmapCache)
                   ? Path.Combine(PortingOptions.Current.ReachLightmapCache, LbspTag.Name)
                   : null;

                convertedLightmap = new CachedLightmap();
                if (lightmapCachePath == null || !convertedLightmap.Load(lightmapCachePath))
                {
                    Console.WriteLine("Converting Lightmap... This may take a while! ");
                    var lightmapConverter = new ReachLightmapConverter();
                    lightmapConverter.ProgressUpdated += progress => Console.Write($"\rProgress: {progress * 100:0.0}%");
                    var result = lightmapConverter.Convert(BlamCache, blamCacheStream, Lbsp);
                    Console.WriteLine();
                    if (result != null)
                    {
                        convertedLightmap.Height = result.Height;
                        convertedLightmap.Width = result.Width;
                        convertedLightmap.MaxLs = result.MaxLs;
                        convertedLightmap.LinearSH = result.LinearSH;
                        convertedLightmap.Intensity = result.Intensity;

                        if (lightmapCachePath != null)
                            convertedLightmap.Store(lightmapCachePath);
                    }
                    else
                    {
                        convertedLightmap = null;
                    }
                }
            }

            // convert main structure (recursive)
            Lbsp = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, Lbsp, blamTagName, blamTagName);

            //
            // import converted lightprobe textures
            //

            if (convertedLightmap != null)
            {
                Lbsp.LightmapSHCoefficientsBitmap.Name = $"{LbspTag.Name}_16f_lp_array_dxt5";
                Lbsp.LightmapDominantLightDirectionBitmap.Name = $"{LbspTag.Name}_16f_lp_array_intensity_dxt5";

                // can't async bitmaps here... only pending should be the lightmaps anyway as they are the first tagref ported from a scenario
                WaitForPendingBitmapConversion();
                ProcessDeferredActions(); // process pending bitmaps (and sounds if any are ready)
                convertedLightmap.ImportIntoLbsp(CacheContext, cacheStream, Lbsp);
            }


            //
            // convert tag light probes
            //

             // TODO: has issues currently for unknown reasons
            foreach (var probe in Lbsp.InstancedGeometryLightProbes)
                probe.LightProbe = VmfConversion.ConvertHalfLightprobe(probe.VmfLightProbe);
                
            foreach (var probe in Lbsp.Airprobes)
                probe.LightProbe = VmfConversion.ConvertHalfLightprobe(probe.VmfLightProbe);

            foreach (var probe in Lbsp.SceneryLightProbes)
                probe.LightProbe = VmfConversion.ConvertHalfLightprobe(probe.VmfLightProbe);

            foreach (var machineProbe in Lbsp.MachineLightProbes)
            {
                foreach (var probe in machineProbe.LightProbes)
                    probe.LightProbe = VmfConversion.ConvertHalfLightprobe(probe.VmfLightProbe);
            }

            //
            // convert vertex buffers and add them to the new resource
            //

            VmfConversion.ConvertStaticPerVertexBuffers(Lbsp, lightmapResourceDefinition, CacheContext.Version, CacheContext.Platform);

            var converter = new RenderGeometryConverter(CacheContext, BlamCache);
            var newLightmapResourceDefinition = converter.Convert(Lbsp.Geometry, lightmapResourceDefinition);

            foreach (var staticPerVertexLighting in Lbsp.StaticPerVertexLightingBuffers)
            {
                if (staticPerVertexLighting.VertexBufferIndexReach != -1)
                {
                    staticPerVertexLighting.VertexBuffer = lightmapResourceDefinition.VertexBuffers[staticPerVertexLighting.VertexBufferIndexReach].Definition;
                       
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = staticPerVertexLighting.VertexBuffer;
                    newLightmapResourceDefinition.VertexBuffers.Add(d3dPointer);
                    // set the new buffer index
                    staticPerVertexLighting.VertexBufferIndex = (short)(newLightmapResourceDefinition.VertexBuffers.Elements.Count - 1);
                }
                else
                {
                    staticPerVertexLighting.VertexBufferIndex = -1;
                }
            }


            Console.WriteLine("Generating cluster per mesh mopp...");
            Lbsp.Geometry.MeshClusterVisibility = new List<RenderGeometry.PerMeshMoppBlock>();
            for (int i = 0; i < Lbsp.Geometry.Meshes.Count; i++)
            {
                var mesh = Lbsp.Geometry.Meshes[i];
                if (mesh.Type == VertexType.World && mesh.Parts.Count > 0)
                    Lbsp.Geometry.MeshClusterVisibility.Add(PerMeshMoppGenerator.Generate(CacheContext, mesh));
                else
                    Lbsp.Geometry.MeshClusterVisibility.Add(new RenderGeometry.PerMeshMoppBlock());
            }
            

            Lbsp.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newLightmapResourceDefinition);

            return Lbsp;
        }

        private CameraFxSettings ConvertCameraFxSettings(CameraFxSettings cfxs, string blamTagName)
        {
            cfxs.Ssao = new CameraFxSettings.SsaoPropertiesBlock
            {
                Flags = CameraFxSettings.SsaoPropertiesBlock.SsaoFlags.UseDefault
            };
            cfxs.ColorGrading = new CameraFxSettings.ColorGradingBlock
            {
                Flags = CameraFxSettings.ColorGradingBlock.CameraFxParameterFlagsCg.UseDefault
            };

            // todo: convert
            if (BlamCache.Version >= CacheVersion.HaloReach)
            {
                cfxs.AutoExposureAntiBloom = new CameraFxSettings.CameraFxValue();
                cfxs.AutoExposureAntiBloom.Flags |= CameraFxSettings.FlagsValue.UseDefault;
                cfxs.BloomIntensity.Flags |= CameraFxSettings.FlagsValue.UseDefault;
            }

            switch (blamTagName)
            {
                // citadel godrays
                case @"levels\dlc\fortress\fortress_fx":
                    cfxs.Lightshafts = new CameraFxSettings.LightshaftsBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Pitch = 57.0f,
                        Heading = 125.0f,
                        Tint = new RealRgbColor(0.9882353f, 0.9372549f, 0.6039216f),
                        DepthClamp = 5000.0f,
                        IntensityClamp = 0.9f,
                        FalloffRadius = 1.0f,
                        Intensity = 0.7f,
                        BlurRadius = 4.0f
                    };
                    break;

                // rat's nest godrays
                case @"levels\dlc\armory\sky\armory_camera":
                    cfxs.Lightshafts = new CameraFxSettings.LightshaftsBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Pitch = 67.5f,
                        Heading = -156.0f,
                        Tint = new RealRgbColor(1f, 0.7333333f, 0.4745098f),
                        DepthClamp = 5000.0f,
                        IntensityClamp = 0.6f,
                        FalloffRadius = 0.08f,
                        Intensity = 1.0f,
                        BlurRadius = 8.0f
                    };
                    break;

                default:
                    cfxs.Lightshafts = new CameraFxSettings.LightshaftsBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.UseDefault,
                        Pitch = 0.0f,
                        Heading = 0.0f,
                        Tint = new RealRgbColor(1.0f, 1.0f, 1.0f),
                        DepthClamp = 5000.0f,
                        IntensityClamp = 0.0f,
                        FalloffRadius = 0.5f,
                        Intensity = 2.0f,
                        BlurRadius = 1.0f
                    };
                    break;
            }

            return cfxs;
        }
    }
}

