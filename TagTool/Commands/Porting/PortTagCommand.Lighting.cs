using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;
using TagTool.Geometry;
using TagTool.Tags.Resources;
using TagTool.Lighting;
using TagTool.Commands.Bitmaps;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private LensFlare ConvertLensFlare(LensFlare lensFlare)
        {
            lensFlare.OcclusionReflectionIndex = 0;

            foreach (var reflection in lensFlare.Reflections)
            {
                if (BlamCache.Version >= CacheVersion.HaloReach)
                {
                    reflection.RadiusBounds = reflection.RadiusBoundsReach;
                    reflection.BrightnessBounds = reflection.BrightnessBoundsReach;
                    reflection.RotationOffset_HO = reflection.RotationOffset_Reach;
                    reflection.TintModulationFactor_HO = reflection.TintModulationFactor_Reach;
                }
                else
                {
                    reflection.RotationOffset_HO = reflection.RotationOffset_H3;
                    reflection.TintModulationFactor_HO = reflection.TintModulationFactor_H3;
                }
                reflection.Unknown2 = 0;
                reflection.BitmapOverride = null;

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

            scenarioLightmap.LightmapDataReferences = new List<ScenarioLightmap.DataReferenceBlock>();

            for (int i = 0; i < scenarioLightmap.Lightmaps.Count; i++)
            {
                var entry = scenarioLightmap.Lightmaps[i];

                var wasReplacing = FlagIsSet(PortingFlags.Replace);

                RemoveFlags(PortingFlags.Replace);
                var Lbsp = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, entry, scenarioLightmap, blamTagName);
                Lbsp = ConvertScenarioLightmapBspData(Lbsp);
                if (wasReplacing)
                    SetFlags(PortingFlags.Replace);

                Lbsp.Airprobes = new List<Airprobe>();
                Lbsp.Airprobes.AddRange(scenarioLightmap.Airprobes);

                var groupTag = CacheContext.TagCache.TagDefinitions.GetTagGroupFromTag("Lbsp");

                CachedTag edTag = edTag = CacheContext.TagCacheGenHO.AllocateTag(groupTag);

                if (scenarioLightmap.Lightmaps.Count != 1)
                    edTag.Name = $"{blamTagName}_{i}_data";
                else
                    edTag.Name = $"{blamTagName}_data";

                CacheContext.Serialize(cacheStream, edTag, Lbsp);

                scenarioLightmap.LightmapDataReferences.Add(new ScenarioLightmap.DataReferenceBlock() { LightmapBspData = edTag });
            }


            scenarioLightmap.Airprobes.Clear();

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
            // TODO: cleanup

            var scnr = BlamCache.Deserialize<Scenario>(blamCacheStream, BlamCache.TagCache.FindFirstInGroup("scnr"));
            var blamLightmap = BlamCache.Deserialize<ScenarioLightmap>(blamCacheStream, scnr.Lightmap);
            for (int i = 0; i < scenarioLightmap.LightmapDataReferences.Count; i++)
            {
                var lbspTag = scenarioLightmap.LightmapDataReferences[i].LightmapBspData;
                if (lbspTag == null)
                    continue;

                var Lbsp = BlamCache.Deserialize<ScenarioLightmapBspData>(blamCacheStream, lbspTag);

                Lbsp.BspIndex = (short)i;

                Console.WriteLine($"Converting lightmap bsp: {i + 1}/{ scenarioLightmap.LightmapDataReferences.Count}");

                bool useCache = !string.IsNullOrEmpty(PortingOptions.Current.ReachLightmapCache);

                CachedLightmap convertedLightmap = null;
                var lightmapCachePath = Path.Combine(PortingOptions.Current.ReachLightmapCache, lbspTag.Name);

                if (Lbsp.LightmapSHCoefficientsBitmap != null)
                {
                    convertedLightmap = new CachedLightmap();
                    if (!useCache || !convertedLightmap.Load(lightmapCachePath))
                    {
                        Console.WriteLine("Converting Lightmap... This may take a while! ");
                        var lightmapConverter = new ReachLightmapConverter();
                        lightmapConverter.ProgressUpdated += progress => Console.Write($"\rProgress: {progress * 100:0.0}%");
                        var result = lightmapConverter.Convert(BlamCache, blamCacheStream, Lbsp);

                        Console.WriteLine();

                        convertedLightmap.Height = result.Height;
                        convertedLightmap.Width = result.Width;
                        convertedLightmap.MaxLs = result.MaxLs;
                        convertedLightmap.LinearSH = result.LinearSH;
                        convertedLightmap.Intensity = result.Intensity;

                        if(useCache)
                            convertedLightmap.Store(lightmapCachePath);
                    }
                }

                Lbsp = ConvertStructure(cacheStream, blamCacheStream, resourceStreams, Lbsp, scenarioLightmap, blamTagName);

                if (convertedLightmap != null)
                {
                    Lbsp.LightmapSHCoefficientsBitmap.Name = $"{lbspTag.Name}_16f_lp_array_dxt5";
                    Lbsp.LightmapDominantLightDirectionBitmap.Name = $"{lbspTag.Name}_16f_lp_array_intensity_dxt5";
                    convertedLightmap.ImportIntoLbsp(CacheContext, cacheStream, Lbsp);
                }

                Lbsp = ConvertScenarioLightmapBspDataReach(Lbsp);

                CachedTag edTag = CacheContext.TagCacheGenHO.AllocateTag<ScenarioLightmapBspData>(scenarioLightmap.LightmapDataReferences[i].LightmapBspData.Name);
                CacheContext.Serialize(cacheStream, edTag, Lbsp);
            }

            return ConvertStructure(cacheStream, blamCacheStream, resourceStreams, scenarioLightmap, scenarioLightmap, blamTagName);
        }

        private ScenarioLightmapBspData ConvertScenarioLightmapBspDataReach(ScenarioLightmapBspData Lbsp)
        {
            var lightmapResourceDefinition = BlamCache.ResourceCache.GetRenderGeometryApiResourceDefinition(Lbsp.Geometry.Resource);

            if (lightmapResourceDefinition == null)
                return Lbsp;


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
            }
            
            Lbsp.Geometry.Resource = CacheContext.ResourceCache.CreateRenderGeometryApiResource(newLightmapResourceDefinition);

            return Lbsp;
        }

        private CameraFxSettings ConvertCameraFxSettings(CameraFxSettings cfxs, string blamTagName)
        {
            cfxs.SsaoProperties = new CameraFxSettings.SsaoPropertiesBlock
            {
                Flags = CameraFxSettings.FlagsValue.UseDefault
            };
            cfxs.ColorGrading = new CameraFxSettings.CameraFxValue
            {
                Flags = CameraFxSettings.FlagsValue.UseDefault
            };

            switch (blamTagName)
            {
                // citadel godrays
                case @"levels\dlc\fortress\fortress_fx":
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Radius = 57.0f,
                        AngleBias = 125.0f,
                        Color = new RealRgbColor(0.9882353f, 0.9372549f, 0.6039216f),
                        Strength = 5000.0f,
                        PowerExponent = 0.9f,
                        BlurSharpness = 1.0f,
                        DecoratorDarkening = 0.7f,
                        HemiRejectionFalloff = 4.0f
                    };
                    break;

                // rat's nest godrays
                case @"levels\dlc\armory\sky\armory_camera":
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.MaximumChangeIsRelative | CameraFxSettings.FlagsValue.Fixed2,
                        Radius = 67.5f,
                        AngleBias = -156.0f,
                        Color = new RealRgbColor(1f, 0.7333333f, 0.4745098f),
                        Strength = 5000.0f,
                        PowerExponent = 0.6f,
                        BlurSharpness = 0.08f,
                        DecoratorDarkening = 1.0f,
                        HemiRejectionFalloff = 8.0f
                    };
                    break;

                default:
                    cfxs.GodraysProperties = new CameraFxSettings.GodraysPropertiesBlock
                    {
                        Flags = CameraFxSettings.FlagsValue.UseDefault,
                        Radius = 0.0f,
                        AngleBias = 0.0f,
                        Color = new RealRgbColor(1.0f, 1.0f, 1.0f),
                        Strength = 5000.0f,
                        PowerExponent = 0.0f,
                        BlurSharpness = 0.5f,
                        DecoratorDarkening = 2.0f,
                        HemiRejectionFalloff = 1.0f
                    };
                    break;
            }

            return cfxs;
        }
    }
}

